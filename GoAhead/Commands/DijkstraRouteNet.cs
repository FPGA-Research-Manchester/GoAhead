using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.InteropServices;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.Selection;
using GoAhead.FPGA;
using GoAhead.Objects;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;

namespace GoAhead.Commands
{
    class DijkstraRouteNet : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            // what to route
            NetlistContainer netlist = GetNetlistContainer();
            XDLNet netToRoute = (XDLNet)netlist.GetNet(NetName);

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

            while (targetQueue.Count > 0)
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
                foreach(List<Location> revPath in Route(startLocations.First(), targetLocation, 100))
                {
                    // extend net
                    if (revPath != null)
                    {
                        XDLNet extension = new XDLNet(revPath);
                        netToRoute.Add(extension);
                    }
                }
                Watch.Stop("route");
            }

            // block the added pips
            netToRoute.BlockUsedResources();
        }

        public IEnumerable<List<Location>> Route(Location startLocation, Location targetLocation, int maxDepth)
        {
            if (startLocation == null)
            {
                throw new ArgumentException("No start locations given");
            }

            DijkstraLocationManager locMan = new DijkstraLocationManager(startLocation, targetLocation);
            Console.WriteLine("Initialisation complete.");
            for (int x = 0; x < 5; x++)
            {
                List<Location> result = locMan.GetShortestPath(startLocation, targetLocation, maxDepth);
                yield return result;
            }   
            //throw new ArgumentException("Could not route from " + startTile.Location + "." + startPip + " to " + targetTile + "." + targetPip);
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
        public string SearchMode = "DIjkstra";
    }

    class DijkstraLocationManager
    {
        public DijkstraLocationManager(Location start, Location end)
        {
            m_startNodeKey = m_graph.AddNode(start);
            m_startNode = start;
            m_locKeys.Add(start, m_startNodeKey);

            m_targetNodeKey = m_graph.AddNode(end);
            m_targetNode = end;
            m_locKeys.Add(end, m_targetNodeKey);

            int startX = Math.Min(m_startNode.Tile.TileKey.X, m_targetNode.Tile.TileKey.X);
            int endX = Math.Max(m_startNode.Tile.TileKey.X, m_targetNode.Tile.TileKey.X);
            int diffX = endX - startX;

            int startY = Math.Min(m_startNode.Tile.TileKey.Y, m_targetNode.Tile.TileKey.Y);
            int endY = Math.Max(m_startNode.Tile.TileKey.Y, m_targetNode.Tile.TileKey.Y);
            int diffY = endY - startY;

            int x1 = startX - diffX;
            int x2 = endX + diffX;
            int y1 = startY - diffY;
            int y2 = endY + diffY;

            AddToSelectionXY selectionCmd = new AddToSelectionXY(x1, y1, x2, y2);
            selectionCmd.Do();

            InitGraph();
        }

        public void InitGraph()
        {
            foreach (Location loc in FPGA.FPGA.Instance.GetAllLocationsInSelection())
            {
                List<Location> reachableLocations = GetReachableLocations(loc).ToList();
                Tile fromTile = loc.Tile;
                if (!m_locKeys.ContainsKey(loc))
                {
                    m_locKeys.Add(loc, m_graph.AddNode(loc));
                }
                foreach (Location connectedLocation in reachableLocations)
                {
                    Tile toTile = connectedLocation.Tile;
                    int diffX = fromTile.TileKey.X - toTile.TileKey.X;
                    int diffY = fromTile.TileKey.Y - toTile.TileKey.Y;
                    if (!m_locKeys.ContainsKey(connectedLocation))
                    {
                        m_locKeys.Add(connectedLocation, m_graph.AddNode(connectedLocation));
                    }
                    m_graph.Connect(m_locKeys[loc], m_locKeys[connectedLocation], Pythagoras(diffX, diffY) + loc.Pip.Cost, $"{loc} -> {connectedLocation}");
                }
            }
        }

        private IEnumerable<Location> GetReachableLocations(Location currentLocation)
        {
            foreach (Port pip in currentLocation.Tile.SwitchMatrix.GetDrivenPorts(currentLocation.Pip).Where(p => FollowPort(currentLocation.Tile, p)))
            {
                Location next = new Location(currentLocation.Tile, pip);
                yield return next;
            }

            foreach (Location next in Navigator.GetDestinations(currentLocation.Tile, currentLocation.Pip).Where(loc => FollowPort(loc.Tile, loc.Pip)))
            {
                if(Math.Abs(next.Tile.LocationX - m_startNode.Tile.LocationX) <= m_maxDist && Math.Abs(next.Tile.LocationY - m_startNode.Tile.LocationY) <= m_maxDist)
                    yield return next;
            }
        }

        public List<Location> GetShortestPath(Location from, Location to, int depth)
        {
            List<Location> path = new List<Location>();
            ShortestPathResult result = DijkstraExtensions.Dijkstra(m_graph, m_startNodeKey, m_targetNodeKey, depth);
            List<Tile> tileList = TileSelectionManager.Instance.GetSelectedTiles().ToList();

            foreach (uint locKey in result.GetPath())
            {
                Location loc = m_locKeys.FirstOrDefault(pair => pair.Value == locKey).Key;
                path.Add(loc);
                tileList.Where(t => t.Equals(loc.Tile)).First().SwitchMatrix.Ports.Where(p => p.Equals(loc.Pip)).First().Cost = 1000;
            }

            DoPostSearchTasks();

            return path;
        }

        private int Pythagoras(int x, int y)
        {
            return (int)Math.Round(Math.Sqrt(x * x + y * y));
        }

        public uint AddToGraph(Location locToAdd)
        {
            uint key = m_graph.AddNode(locToAdd);
            m_locKeys.Add(locToAdd, key);
            return key;
        }

        public void ConnectPorts(Location loc1, Location loc2)
        {
            m_graph.Connect(m_locKeys[loc1], m_locKeys[loc2], loc1.Pip.Cost + loc2.Pip.Cost, "");
        }

        private void DoPostSearchTasks()
        {
            m_locKeys = null;
            m_graph = null;
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

        private int m_maxDist = 20;
        private Location m_startNode;
        private Location m_targetNode;
        private uint m_startNodeKey;
        private uint m_targetNodeKey;
        private Graph<Location, string> m_graph = new Graph<Location, string>();
        private Dictionary<Location, uint> m_locKeys = new Dictionary<Location, uint>();
    }
}