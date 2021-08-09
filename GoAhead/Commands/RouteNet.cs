using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Route a single net", Wrapper = false, Publish = true)]
    class RouteNet : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            // what to route
            NetlistContainer netlist = GetNetlistContainer();
            XDLNet netToRoute = (XDLNet) netlist.GetNet(NetName);

            int outpinCount = netToRoute.NetPins.Count(np => np is NetOutpin);
            if (outpinCount != 1)
            {
                throw new ArgumentException("Can not route nets with " + outpinCount + " outpins");
            }
            NetPin outpin = netToRoute.NetPins.First(np => np is NetOutpin);

            // start to route from here
            List<Location> startLocations = new List<Location>();
            List<Location> targetLocations = new List<Location>();

            // route from outpin
            string startTileName = netlist.GetInstanceByName(outpin.InstanceName).Location;
            Tile startTile = FPGA.FPGA.Instance.GetTile(startTileName);
            Slice startSlice = startTile.GetSliceByName(netlist.GetInstanceByName(outpin.InstanceName).SliceName);
            Port startPip = startSlice.PortMapping.Ports.Where(p => p.Name.EndsWith(outpin.SlicePort)).First();
            Location outpinLocation = new Location(startTile, startPip);
            startLocations.Add(outpinLocation);

            Queue<Location> targetQueue = new Queue<Location>(targetLocations);
            foreach (NetPin inpin in netToRoute.NetPins.Where(np => np is NetInpin).OrderBy(np => np.InstanceName))
            {
                string targetTileName = netlist.GetInstanceByName(inpin.InstanceName).Location;
                Tile targetTile = FPGA.FPGA.Instance.GetTile(targetTileName);
                Slice targetSlice = targetTile.GetSliceByName(netlist.GetInstanceByName(inpin.InstanceName).SliceName);
                Port targetPip = targetSlice.PortMapping.Ports.Where(p => p.Name.EndsWith(inpin.SlicePort)).First();
                Location inpinLocation = new Location(targetTile, targetPip);

                targetQueue.Enqueue(inpinLocation);
            }

            while(targetQueue.Count > 0)
            {
                // start with new routing
                foreach (XDLPip pip in netToRoute.Pips)
                {
                    Tile newStartTile = FPGA.FPGA.Instance.GetTile(pip.Location);
                    startLocations.Add(new Location(newStartTile, new Port(pip.From)));
                }

                // dequeue next target
                Location targetLocation = targetQueue.Dequeue();

                Watch.Start("route");
                List<Location> revPath = Route(SearchMode, true, startLocations, targetLocation, 0, 100, 0, false).FirstOrDefault();
                Watch.Stop("route");

                // extend net
                if (revPath != null)
                {
                    XDLNet extension = new XDLNet(revPath);
                    netToRoute.Add(extension);
                }
            }

            // block the added pips
            netToRoute.BlockUsedResources();
        }

        public IEnumerable<List<Location>> Route(string searchMode, bool forward, IEnumerable<Location> startLocations, Location targetLocation, int distanceLimit, int maxDepth, int minDepth, bool keepPathsIndependet)
        {
            if (startLocations.Count() == 0)
            {
                throw new ArgumentException("No start locations given");
            }

            Location startLocation = startLocations.First();

            LocationManager locMan = null;
            // how to route
            switch (searchMode)
            {
                case "BFS":
                    locMan = new QueueLocationManager(startLocation);
                    break;
                case "DFS":
                    locMan = new StackLocationManager(startLocation);
                    break;
                case "A*":
                    locMan = new SortedQLocationManager(startLocation, targetLocation.Tile);
                    break;
                default:
                    throw new ArgumentException("SearchMode " + searchMode + " not supported. See paramter SearchMode for posible values.");
            }

            locMan.Add(startLocations);

            if (!keepPathsIndependet)
            {
                foreach (List<Location> l in RouteClassic(forward, startLocations, targetLocation, distanceLimit, maxDepth, minDepth, locMan))
                {
                    yield return l;
                }
            }
            else
            {
                foreach (List<Location> l in RouteLocal(forward, startLocations, targetLocation, distanceLimit, maxDepth, minDepth, startLocation, locMan))
                {
                    yield return l;
                }
            }

            locMan.Clear();

            //throw new ArgumentException("Could not route from " + startTile.Location + "." + startPip + " to " + targetTile + "." + targetPip);
        }

        private IEnumerable<List<Location>> RouteClassic(bool forward, IEnumerable<Location> startLocations, Location targetLocation, int distanceLimit, int maxDepth, int minDepth, LocationManager locMan)
        {
            Tile intTile = targetLocation.Tile;// FPGATypes.GetInterconnectTile(targetLocation.Tile);

            while (locMan.LocationsLeft())
            {
                Location currentLocation = locMan.GetNext();

                int depth = locMan.GetDepth(currentLocation, maxDepth);
                if (depth > maxDepth)
                {
                    continue;
                }
                
                /*
                List<Tuple<Location, int>> reachables = new List<Tuple<Location, int>>();
                foreach (Location r in RouteNet.GetReachableLocations(currentLocation, forward))
                {
                    if (r.Tile.Location.Equals(currentLocation.Tile.Location))
                    {
                        int distance = Navigator.GetDestinations(r.Tile, r.Pip).Any() ? Navigator.GetDestinations(r.Tile, r.Pip).Min(l => RouteNet.Distance(l.Tile, targetLocation.Tile)) : 0;
                        reachables.Add(new Tuple<Location,int>(r, distance));
                    }
                    else
                    {
                        reachables.Add(new Tuple<Location, int>(r, RouteNet.Distance(r.Tile, targetLocation.Tile)));
                    }
                }

                foreach (Tuple<Location, int> tuple in reachables.OrderByDescending(t => t.Item2))
                 * */
                foreach (Location next in GetReachableLocations(currentLocation, forward))
                {
                    //Location next = tuple.Item1;
                    if (targetLocation.Equals(next))
                    {
                        /* Previously, if the targetLocation was found then the function would 
                         * bypass depth checks and simply add the destination to the path.
                         * Now, if the depth were to exceed the MaxDepth setting, the path would
                         * be invalid and the program would attempt to find a new path.
                         */
                        if(depth + 1 > maxDepth || depth + 1 < minDepth)
                        {
                            continue;
                        }
                        List<Location> revPath = locMan.GetPath(startLocations, currentLocation);
                        revPath.Add(next);
                        yield return revPath;
                    }
                    else
                    {
                        bool close = LocationLeadsCloserToTarget(currentLocation, next, targetLocation.Tile, distanceLimit);

                        //if (!visited.ContainsKey(next) && close)
                        if (!locMan.WasAlreadyVisited(next) && close)
                        {
                            locMan.MarkAsVisited(next, currentLocation);
                            //parent.Add(next, currentLocation);
                            locMan.Add(next);
                        }
                    }
                }
            }
        }

        private IEnumerable<List<Location>> RouteLocal(bool forward, IEnumerable<Location> startLocations, Location targetLocation, int distanceLimit, int maxDepth, int minDepth, Location startLocation, LocationManager locMan)
        {
            while (locMan.LocationsLeft())
            {
                Location currentLocation = locMan.GetNext();

                if (currentLocation.Path != null)
                {
                    if (currentLocation.Path.Count > maxDepth)
                    {
                        // save memory
                        currentLocation.Path.Clear();
                        continue;
                    }
                }
                
                foreach (Location l in GetReachableLocations(currentLocation, forward))
                {
                    bool targetReached = targetLocation.Equals(l);

                    // TODO when LOGICIN und auf Interconnect der Zielkachel, versuche direkt bis uzm Ziel zu routen

                    if (targetReached)
                    {
                        // as we empty the list, return a copy
                        if (currentLocation.Path == null)
                        {
                            currentLocation.Path = new List<Location>();
                            //currentLocation.Path.Add(
                            // TODO single step paths a la INT_X10Y105.EE2BEG0
                        }
                        List<Location> copy = new List<Location>(currentLocation.Path);
                        copy.Add(l);
                        yield return copy;
                    }
                    else
                    {
                        bool close = LocationLeadsCloserToTarget(currentLocation, l, targetLocation.Tile, distanceLimit);

                        //this.Watch.Start("compare");
                        // same tile
                        bool targetTile = l.Tile.LocationX == targetLocation.Tile.LocationX && l.Tile.LocationY == targetLocation.Tile.LocationY;
                        bool startTile = l.Tile.LocationX == startLocation.Tile.LocationX && l.Tile.LocationY == startLocation.Tile.LocationY;
                        bool useTile = targetTile || startTile;
                        if (!useTile)
                        {
                            // if we are not on target tile, only accept interconnects
                            useTile = IdentifierManager.Instance.IsMatch(l.Tile.Location, IdentifierManager.RegexTypes.Interconnect) && !l.Pip.Name.StartsWith("LOGICIN_");
                        }
                        //this.Watch.Stop("compare");

                        //this.Watch.Start("add");
                        bool add = false; 
                        // prevent use less comprae (see if)
                        if (useTile)
                        {
                            add = currentLocation.Path == null ? true : !currentLocation.Path.Contains(l) && close;
                        }
                        //this.Watch.Stop("add");

                        if (add && useTile)
                        {
                            l.Path = new List<Location>();
                            if (currentLocation.Path != null)
                            {
                                l.Path.AddRange(currentLocation.Path);
                            }
                            l.Path.Add(currentLocation);
                            locMan.Add(l);
                        }
                    }
                }

                if (currentLocation.Path != null)
                {
                    // save memory
                    currentLocation.Path.Clear();
                }
            }
        }

        private IEnumerable<Location> GetReachableLocations(Location currentLocation, bool forward)
        {
            if (forward)
            {
                foreach (Port pip in currentLocation.Tile.SwitchMatrix.GetDrivenPorts(currentLocation.Pip).Where(p => FollowPort(currentLocation.Tile, p)))
                {
                    Location next = new Location(currentLocation.Tile, pip);
                    yield return next;
                }

                foreach (Location next in Navigator.GetDestinations(currentLocation.Tile, currentLocation.Pip).Where(loc => FollowPort(loc.Tile, loc.Pip)))
                {
                    yield return next;
                }
            }
            else
            {
                foreach (Tuple<Port, Port> arc in currentLocation.Tile.SwitchMatrix.GetAllArcs().Where(a => a.Item2.Name.Equals(currentLocation.Pip.Name) && FollowPort(currentLocation.Tile, a.Item1)))
                {
                    Location next = new Location(currentLocation.Tile, arc.Item1);
                    yield return next;
                }

                foreach (Location next in Navigator.GetDestinations(currentLocation.Tile, currentLocation.Pip, false).Where(loc => FollowPort(loc.Tile, loc.Pip)))
                {
                    yield return next;
                }
            }
        }

        private bool FollowPort(Tile t, Port p)
        {
            if (t.IsPortBlocked(p))
            {
                if (t.IsPortBlocked(p, Tile.BlockReason.Stopover))
                    return true;

                return false;
            }

            return true;
        }
       
        private bool LocationLeadsCloserToTarget(Location current, Location other, Tile target, int distanceLimit)
        {
            int currentDistance = Distance(current.Tile, target);
            int newDistance = Distance(other.Tile, target);

            return newDistance <= currentDistance + distanceLimit;
        }

        private int Distance(Tile from, Tile to)
        {
            return Math.Abs(from.LocationX - to.LocationX) + Math.Abs(from.LocationY - to.LocationY);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name to add to the macro")]
        public string NetName = "net";

        [Parameter(Comment = "The path search modue (BFS, DFS, A*)")]
        public string SearchMode = "BFS";
    }

    abstract class LocationManager
    {
        private LocationManager()
        {
        }

        protected LocationManager(Location start)
        {
            m_startLocation = start;
        }
        
        public abstract bool LocationsLeft();
        public abstract void Add(Location loc);
        public void Add(IEnumerable<Location> locations)
        {
            foreach (Location l in locations)
            {
                Add(l);
            }
        }
        public abstract Location GetNext();

        /// <summary>
        /// Return the number of to be visited locations
        /// </summary>
        /// <returns></returns>
        public abstract int Count();

        public abstract void Clear();

        public void MarkAsVisited(Location next, Location from)
        {
            m_visited.Add(next, false);
            m_parent.Add(next, from);
        }

        public bool WasAlreadyVisited(Location next)
        {
            return m_visited.ContainsKey(next);           
        }

        public List<Location> GetPath(IEnumerable<Location> startLocations, Location currentLocation)
        {
            List<Location> revPath = new List<Location>();
            revPath.Add(currentLocation);
            if (m_parent.ContainsKey(currentLocation))
            {
                Location pre = m_parent[currentLocation];
                while (true)
                {
                    Location start = startLocations.FirstOrDefault(l => l.Equals(pre));
                    bool connectedToStartLocation = start != null ? pre.Pip.Name.Equals(start.Pip.Name) : false;
                    if (connectedToStartLocation)
                    {
                        revPath.Add(start);
                        break;
                    }

                    revPath.Add(pre);
                    pre = m_parent[pre];
                }
                revPath.Reverse();
            }
            return revPath;
        }

        public int GetDepth(Location currentLocation, int maxDepth)
        {
            int depth = 0;
            if (m_parent.ContainsKey(currentLocation))
            {
                Location pre = m_parent[currentLocation];
                depth++;
                while (!pre.Equals(m_startLocation))
                {
                    if (depth > maxDepth)
                    {
                        return depth;
                    }

                    if (!m_parent.ContainsKey(pre))
                    {
                        // partial routing already
                        return depth;
                    }
                    pre = m_parent[pre];
                    depth++;
                }
            }
            return depth;
        }

        protected readonly Location m_startLocation;
        protected Dictionary<Location, bool> m_visited = new Dictionary<Location, bool>();
        protected Dictionary<Location, Location> m_parent = new Dictionary<Location, Location>();
    }

    class SortedQLocationManager : LocationManager
    {
        public SortedQLocationManager(Location start, Tile target)
            :base(start)
        {
            m_start = start.Tile;
            m_target = target;
        }

        public override bool LocationsLeft()
        {
            return m_locations.Count > 0;
        }

        public override int Count()
        {
            return m_locations.Sum(t => t.Value.Count());
        }

        public override void Clear()
        {
            m_locations.Clear();
        }

        public override void Add(Location loc)
        {
            // do not route through CLBS expect start and target
            if(IdentifierManager.Instance.IsMatch(loc.Tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                bool startMatch = m_start.Location.Equals(loc.Tile.Location);
                bool targetMatch = m_target.Location.Equals(loc.Tile.Location);// && this.m_target.SwitchMatrix.Contains(loc.Pip, this.m_targetPip);

                if (!startMatch && !targetMatch)
                {
                    return;
                }
            }

            int distance = Distance(loc.Tile, m_target);
            if (!m_locations.ContainsKey(distance))
            {
                m_locations.Add(distance, new SortedList<int, List<Location>>());
            }
            
            int cost = GetDepth(loc, 100);
            if (!m_locations[distance].ContainsKey(cost))
            {
                m_locations[distance].Add(cost, new List<Location>());
            }
            
            m_locations[distance][cost].Add(loc);
        }

        private int Distance(Tile from, Tile to)
        {
            return Math.Abs(from.LocationX - to.LocationX) + Math.Abs(from.LocationY - to.LocationY);
        }

        public override Location GetNext()
        {
            int minimalDistance = m_locations.Keys[0];
            SortedList<int, List<Location>> closestLocations = m_locations[minimalDistance];

            int shortestPathLength = closestLocations.Keys[0];
            List<Location> closesPathsWithShortedPathLength = closestLocations[shortestPathLength];

            // continue with the shortest paths
            Location next = closesPathsWithShortedPathLength[0];

            // clean up
            closesPathsWithShortedPathLength.RemoveAt(0);
            if (closesPathsWithShortedPathLength.Count == 0)
            {
                closestLocations.RemoveAt(0);
            }
            if (closestLocations.Count == 0)
            {
                m_locations.RemoveAt(0); // remove the first, i.e. the minimal distance entry
            }
            return next;
        }
        
        private SortedList<int, SortedList<int, List<Location>>> m_locations = new SortedList<int, SortedList<int, List<Location>>>();
        protected readonly Tile m_start;
        private readonly Tile m_target;
    }

    class StackLocationManager : LocationManager
    {
        public StackLocationManager(Location start)
            : base(start)
        {
        }
        
        public override bool LocationsLeft()
        {
            return m_locationStack.Count > 0;
        }

        public override void Add(Location loc)
        {
            m_locationStack.Push(loc);
        }

        public override Location GetNext()
        {
            return m_locationStack.Pop();
        }

        public override int Count()
        {
            return m_locationStack.Count;
        }

        public override void Clear()
        {
            m_locationStack.Clear();
        }
                    
        private Stack<Location> m_locationStack = new Stack<Location>();
    }
       
    class QueueLocationManager : LocationManager
    {
        public QueueLocationManager(Location start)
            : base(start)
        {
        }

        public override bool LocationsLeft()
        {
            return m_locationQueue.Count > 0;
        }

        public override void Add(Location loc)
        {
            m_locationQueue.Enqueue(loc);
        }

        public override Location GetNext()
        {
            return m_locationQueue.Dequeue();
        }
                        
        public override int Count()
        {
            return m_locationQueue.Count;
        }

        public override void Clear()
        {
            m_locationQueue.Clear();
        }

        private Queue<Location> m_locationQueue = new Queue<Location>(2000000);
    }


}
