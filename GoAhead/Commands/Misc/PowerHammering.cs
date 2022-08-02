using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;
using System.IO;

namespace GoAhead.Commands.Misc
{
    [CommandDescription(Description = "Expand as deep and wide as possible, not reusing any ports, for use in power hammering.", Wrapper = true)]
    class PowerHammering : Command
    {
        [Parameter(Comment = "The tile to start at for power hammering.")]
        public string StartLocation = ""; 

        [Parameter(Comment = "The port to start at.")]
        public string StartPort = "";

        [Parameter(Comment = "Location string where to go")]
        public string TargetLocation = "";

        [Parameter(Comment = "Target port")]
        public string TargetPort = "";

        [Parameter(Comment = "The maximum depth to search (large values will make the function take longer to execute)")]
        public int MaxDepth = 5;

        [Parameter(Comment = "If a filename is provided, the output will be written to that file. Otherwise, the console will be used.")]
        public string FileName = "";

        [Parameter(Comment = "Whether to print results in colour (True) or not (False).")]
        public bool PrintInColour = false;

        [Parameter(Comment = "The maximum pips to expand at each level. Any value less than 1 will result in all pips being expanded.")]
        public int MaxPips = 0;

        [Parameter(Comment = "Whether to stay within the selection defined by the user.")]
        public bool UseUserSelection = false;

        [Parameter(Comment = "Whether to operate in Antenna mode or AWS Mode.")]
        public string Mode = "Antenna";

        protected override void DoCommandAction()
        {
            // If MaxPips is 0 or negative, just use a very large integer instead
            MaxPips = MaxPips < 1 ? 9999999 : MaxPips;
            if (!PrintInColour)
            {
                colours = new List<ConsoleColor>() { ConsoleColor.Gray };
            }
            string startPortRegex = "^" + StartPort + "$";
            Tile tile = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartLocation)).OrderBy(t => t.Location).First(); // Get start tile
            Port startPort = tile.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, startPortRegex)).OrderBy(p => p.Name).First(); // Get start port
            //BuildNet(tile); 
            Location location = new Location(tile, startPort);
            currentPath = new List<Location>() { location };

            if (FileName != "")
            {
                stringBuilder.AppendLine(location.ToString());
            }
            else 
            {
                Console.WriteLine(location.ToString());
            }

            if (Mode == "Antenna")
                RecursiveLookup(location, 0);
            else if (Mode == "AWS")
            {
                DepthFirstSearch(location);
                float percent = ((float)longestPath.Count / (float)FPGA.FPGA.Instance.GetAllLocationsInSelection().Count()) * 100;
                Console.WriteLine($"A path was found with {longestPath.Count} nodes, meaning there was a location usage of {percent}%.");
                StringBuilder sb = new StringBuilder();
                paths.Sort((a, b) => a.Count - b.Count);
                foreach (Location loc in longestPath)
                {
                    sb.Append($"{loc}->");
                }
                Console.WriteLine(sb.ToString());
            }
            else
            {
                return;
            }

            if(FileName != "")
            {
                using(StreamWriter sw = new StreamWriter(FileName))
                {
                    sw.Write(stringBuilder);
                }
            }
        }

        private void BuildNet(Tile tile)
        {
            List<Tile> tileList = new List<Tile>();
            if (!IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                tileList = FPGATypes.GetCLTile(tile).ToList();
            }
            else
            {
                tileList = FPGATypes.GetCLTile(FPGATypes.GetInterconnectTile(tile)).ToList();
            }
            foreach (Tile t in tileList)
            {
                foreach (LUTRoutingInfo info in FPGATypes.GetLUTRouting(t))
                {
                    if (info.Port1 != null && !startLUTs.Contains(new Location(tile, info.Port1)))
                        startLUTs.Add(new Location(tile, info.Port1));
                    if (info.Port4 != null && !endLUTs.Contains(new Location(tile, info.Port4)))
                        endLUTs.Add(new Location(tile, info.Port4));

                }
            }
            Console.WriteLine($"There are {startLUTs.Count} starting locations and {endLUTs.Count} end locations");
            List<List<Location>> paths = new List<List<Location>>();
            //foreach(Location startLoc in startLUTs)
            //{
            Location startLoc = new Location(tile, tile.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, "^" + StartPort + "$")).OrderBy(p => p.Name).First());
                foreach (Location endLoc in endLUTs)
                {
                    /*
                    PathSearchOnFPGA cmd = new PathSearchOnFPGA();
                    cmd.StartLocation = startLoc.ToString();
                    cmd.TargetLocation = endLoc.ToString();
                    cmd.MaxSolutions = 1;
                    cmd.MaxDepth = 20;
                    cmd.Do();
                    */
                    RouteNet cmd = new RouteNet();
                    foreach(List<Location> route in cmd.Route("BFS", true, Enumerable.Repeat(startLoc, 1), endLoc, 1000, 20, 0, false)) 
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (Location loc in route)
                        {
                            sb.Append($"{loc}->");
                        }
                        Console.WriteLine(sb.ToString());
                        break;
                    }
                }
            //}
            
        }

        private void PrintAsChain(List<List<Location>> paths)
        {
            Dictionary<int, int> maxSegmentLength = new Dictionary<int, int>();
            foreach (List<Location> path in paths)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    if (!maxSegmentLength.ContainsKey(i))
                    {
                        maxSegmentLength.Add(i, 0);
                    }
                    maxSegmentLength[i] = Math.Max(maxSegmentLength[i], path[i].ToString().Length);
                }
            }

            // Sorting by attribute
            Dictionary<StringBuilder, float> pathStringsAndLatencies = new Dictionary<StringBuilder, float>();
            int pathNum = 1;
            foreach (List<Location> path in paths.OrderBy(l => l.Count))
            {
                StringBuilder buffer = new StringBuilder();
                
                buffer.Append(pathNum++.ToString() + ". ");

                for (int i = 0; i < path.Count; i++)
                {
                    string nextLine = path[i].ToString();
                    nextLine = nextLine.PadRight(maxSegmentLength[i]);
                    nextLine += (i < path.Count - 1 ? " -> " : "");

                    buffer.Append(nextLine);
                }
            }

            foreach (var path in pathStringsAndLatencies.OrderBy(p => p.Value))
            {
                OutputManager.WriteOutput(path.Key.ToString());
            }
        }

        private void RecursiveLookup(Location location, int depth)
        {
            int locs = 0;
            if(depth == MaxDepth)
            {
                paths.Add(currentPath);
                return;
            }
            else
            {
                //Location next in Navigator.GetDestinations(location.Tile, location.Pip).Where(loc => FollowPort(loc.Tile, loc.Pip))
                foreach (Port pip in location.Tile.SwitchMatrix.GetDrivenPorts(location.Pip).Where(p => FollowPort(location.Tile, p) && !location.Tile.IsPortBlocked(p)))
                {
                    if(locs > MaxPips)
                    {
                        break;
                    }
                    Location newLocation = new Location(location.Tile, pip);
                    if (!exploredLocations.Contains(newLocation))
                    {
                        exploredLocations.Add(newLocation);
                        OutputHelper(newLocation, depth);
                        currentPath.Add(newLocation);
                        RecursiveLookup(newLocation, depth + 1);
                        currentPath.Remove(newLocation);
                        locs++;
                    }
                }
                foreach (Location next in Navigator.GetDestinations(location.Tile, location.Pip).Where(loc => FollowPort(loc.Tile, loc.Pip)))
                {
                    if(locs > MaxPips)
                    {
                        break;
                    }
                    if (!exploredLocations.Contains(next))
                    {
                        if(UseUserSelection)
                        {
                            if(!TileSelectionManager.Instance.GetSelectedTiles().Contains(next.Tile))
                            {
                                continue;
                            }
                        }
                        exploredLocations.Add(next);
                        OutputHelper(next, depth);
                        currentPath.Add(next);
                        RecursiveLookup(next, depth + 1);
                        currentPath.Remove(next);
                        locs++;
                    }
                }
            }
        }
        /*
        private class Node
        {
            public Node(Location loc, bool connected) 
            {
                Location = loc;
                IsConnected = connected;
            }
            public Location Location
            {
                get; set;
            }

            public bool IsConnected
            {
                get; set;
            }

        }
        */
        private void DepthFirstSearch(Location startLoc)
        {
            int maxDepth = FPGA.FPGA.Instance.GetAllLocationsInSelection().Count();
            StackLocationManager locMan = new StackLocationManager(startLoc);
            locMan.Add(startLoc);
            while (locMan.LocationsLeft())
            {
                Location currentLocation = locMan.GetNext();

                int depth = locMan.GetDepth(currentLocation, maxDepth);
                if (depth > maxDepth)
                {
                    continue;
                }

                foreach (Location next in GetReachableLocations(currentLocation))
                {
                    if (!locMan.WasAlreadyVisited(next) && FPGA.FPGA.Instance.GetAllLocationsInSelection().Contains(next))
                    {
                        locMan.MarkAsVisited(next, currentLocation);
                        var revPath = locMan.GetPath(new List<Location>() { startLoc }, currentLocation);
                        if (revPath.Count > longestPath.Count)
                        {
                            foreach (LUTRoutingInfo info in FPGATypes.GetLUTRouting(revPath[0].Tile, FPGATypes.LUTRoutingType.LUTRoutingOut))
                            {

                                if (info.Port4.Equals(revPath[0].Pip))
                                {
                                    revPath.Reverse();
                                    longestPath = new List<Location>(revPath);
                                }
                            }
                            revPath.Reverse();
                            longestPath = new List<Location>(revPath);
                        }
                        locMan.Add(next);
                    }
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
                yield return next;
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

        private void OutputHelper(Location next, int depth)
        {
            string format = "¦";
            for (int x = 0; x <= depth; x++)
            {
                format += "-";
            }
            format += "> ";

            if (FileName == "")
            {
                Console.Write(format);
                Console.ForegroundColor = colours[depth % colours.Count];
                Console.WriteLine(next.ToString());
                Console.ResetColor();
            }
            else
            {
                format += next.ToString();
                stringBuilder.AppendLine(format);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private int steps = 0;
        private StringBuilder stringBuilder = new StringBuilder();
        private IEnumerable<Location> locationsInSelection = FPGA.FPGA.Instance.GetAllLocationsInSelection();
        private List<Location> startLUTs = new List<Location>();
        private List<Location> endLUTs = new List<Location>();
        private List<Location> exploredLocations = new List<Location>();
        private List<List<Location>> paths = new List<List<Location>>();
        private List<Location> currentPath = new List<Location>();
        private List<Location> longestPath = new List<Location>();
        private int longPathCount = 1;
        private List<ConsoleColor> colours = new List<ConsoleColor>()
        {
            ConsoleColor.DarkBlue,
            ConsoleColor.Blue,
            ConsoleColor.DarkCyan,
            ConsoleColor.Cyan,
            ConsoleColor.White
        };
    }
}
