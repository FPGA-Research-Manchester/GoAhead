using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Look for the specified path and write it to file", Wrapper = true)]
    class PathSearchOnFPGA : CommandWithFileOutput
	{
        [Parameter(Comment = "The path search module (BFS, DFS, A*)")]
        public String SearchMode = "BFS";

        [Parameter(Comment = "Whether to search in forward (true) or backward direction (false)")]
        public bool Forward = true;

        [Parameter(Comment = "Dont use!")]
        public bool KeepPathsIndependet = false;

        [Parameter(Comment = "Whether to block used ports after finding a path (set to true for incremental search)")]
        public bool BlockUsedPorts = false;

        [Parameter(Comment = "How to print out the results (XDL, CHAIN, TCL)")]
        public String OutputMode = "CHAIN";

        [Parameter(Comment = "Location string where to start")]
        public String StartLocation = "INT_X9Y39";

        [Parameter(Comment = "Start port")]
        public String StartPort = "EL2BEG0";

        [Parameter(Comment = "Location string where to go")]
        public String TargetLocation = "INT_X11Y39";

        [Parameter(Comment = "Target port")]
        public String TargetPort = "EL2BEG0";

        [Parameter(Comment = "Stop after the first MaxSolutions path")]
        public int MaxSolutions = 5;

        [Parameter(Comment = "The max path length")]
        public int MaxDepth = 5;

        [Parameter(Comment = "Print Banner")]
        public bool PrintBanner = true;

        [Parameter(Comment = "Print Latency")]
        public List<bool> PrintLatency = new List<bool>() { false, false, false, false };

        [Parameter(Comment = "Sort the paths found by the latency attribute")]
        public string SortByAttribute = "";


        public List<List<Location>> m_paths = new List<List<Location>>();
        private LatencyManager LatencyMan;

        protected override void DoCommandAction()
        {
            Tile startTile = FPGA.FPGA.Instance.GetTile(StartLocation);
            Port startPip = new Port(StartPort);

            Tile targetTile = FPGA.FPGA.Instance.GetTile(TargetLocation);
            Port targetPip = new Port(TargetPort);
            
            Location startLocation = new Location(startTile, startPip);
            Location targetLocation = new Location(targetTile, targetPip);

            CheckExistence(startLocation);
            CheckExistence(targetLocation);

            LatencyMan = new LatencyManager(PrintLatency);

            // print results
            if (this.PrintBanner && !OutputMode.ToUpper().Equals("TCL"))
            {
                OutputManager.WriteOutput(GetBannerWithLatency(startLocation, targetLocation));
            }

            RouteNet routeCmd = new RouteNet();
            routeCmd.Watch = this.Watch;

            foreach (List<Location> path in routeCmd.Route(this.SearchMode, this.Forward, Enumerable.Repeat(startLocation, 1), targetLocation, 1000, this.MaxDepth, this.KeepPathsIndependet))
            {
                if (!PathSearchOnFPGA.PathAlreadyFound(path, m_paths))
                {
                    m_paths.Add(path);

                    if (this.BlockUsedPorts)
                    {
                        foreach (Location l in path)
                        {
                            if (!l.Tile.IsPortBlocked(l.Pip))
                            {
                                l.Tile.BlockPort(l.Pip, Tile.BlockReason.Blocked);
                            }
                        }
                    }

                    this.ProgressInfo.Progress = this.ProgressStart + (int)((double)m_paths.Count / (double)this.MaxSolutions * this.ProgressShare);

                    if (m_paths.Count >= this.MaxSolutions)
                    {
                        break;
                    }
                }
            }

            if (m_paths.Count == 0)
            {
                //this.OutputManager.WriteOutput("No path found");
            }
            else if (this.OutputMode.ToUpper().Equals("CHAIN"))
            {
                this.PrintAsChain(m_paths);
            }
            else if (this.OutputMode.ToUpper().Equals("XDL"))
            {
                this.OutputManager.WriteOutput(PathToString(startLocation, targetLocation, m_paths));
            }
            else if (this.OutputMode.ToUpper().Equals("TCL"))
            {
                SendToTCL(m_paths);
            }
        }

        public string GetBannerWithLatency(Location start, Location sink)
        {
            string result = GetBanner(start, sink);

            if (!LatencyMan.Print) return result;

            string attributesToPrint = "[";
            for (int i = 0; i < LatencyMan.PresentAttributesCount; i++) if (PrintLatency[i])
                    attributesToPrint += LatencyMan.PresentAttributesNames[i] + (i == LatencyMan.PresentAttributesCount - 1 ? "]" : ", ");

            result += "--Latency: " + attributesToPrint + Environment.NewLine;
            try
            {
                Tile.TimeAttributes sortingAttribute = FPGA.FPGA.Instance.GetTimeAttribute(SortByAttribute);
                result += "--Sorted by: " + SortByAttribute + Environment.NewLine;
            }
            catch (Exception) {}
            result += "---------------------------------------------------------------------------------" + Environment.NewLine;

            return result;
        }

        public static string GetBanner(Location start, Location sink)
        {
            String result = "";
            result += "---------------------------------------------------------------------------------" + Environment.NewLine;
            result += "--From " + start.Tile.Location + "." + start.Pip.Name + " to " + sink.Tile.Location + "." + sink.Pip.Name + Environment.NewLine;
            result += "---------------------------------------------------------------------------------" + Environment.NewLine;

            return result;
        }

        public static void CheckExistence(Location startLocation)
        {
            if (startLocation.Tile == null)
            {
                throw new ArgumentException("Tile " + startLocation.Tile.Location + " not found");
            }

            if (!startLocation.Tile.SwitchMatrix.Contains(startLocation.Pip))
            {
                throw new ArgumentException("Tile " + startLocation.Tile.Location + " does not contain port " + startLocation.Pip);
            }
        }

        public static String PathToString(Location start, Location sink, IEnumerable<List<Location>> paths)
        {
            Tile startTile = FPGA.FPGA.Instance.GetTile(start.Tile.Location);
            Port startPip = new Port(start.Pip.Name);

            Tile targetTile = FPGA.FPGA.Instance.GetTile(sink.Tile.Location);
            Port targetPip = new Port(sink.Pip.Name);

            NetOutpin op = new NetOutpin();
            op.InstanceName = start.Tile.Location;
            op.SlicePort = start.Pip.Name;

            NetInpin ip = new NetInpin();
            ip.InstanceName = sink.Tile.Location;
            ip.SlicePort = sink.Pip.Name;

            foreach (Slice s in startTile.Slices)
            {
                if (s.PortMapping.Contains(startPip))
                {
                    op.InstanceName = s.SliceName;
                }
                if (s.PortMapping.Contains(targetPip))
                {
                    ip.InstanceName = s.SliceName;
                }
            }         

            int netCount = 0;
            StringBuilder buffer = new StringBuilder();
            foreach (List<Location> path in paths.OrderBy(l => l.Count))
            {
                XDLNet n = new XDLNet(path);
                n.Name = "path_" + netCount++;
                n.Add(op);
                n.Add(ip);

                buffer.AppendLine(n.ToString());
            }
            return buffer.ToString();
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

            int attCount = LatencyMan.PresentAttributesCount;

            if (attCount == 0) PrintLatency = new List<bool>();
            else if (PrintLatency.Count != attCount)
            {
                Console.WriteLine("Wrong number of parameters provided. PrintLatency should contain exactly " + attCount + " elements");

                PrintLatency = new List<bool>();
                for (int j = 0; j < attCount; j++) PrintLatency.Add(false);
            }

            List<float> latency = new List<float>();
            for (int j = 0; j < attCount; j++) latency.Add(0);

            Dictionary<Tile.TimeAttributes, int> indices = LatencyMan.PresentAttributesIndices;

            // Sorting by attribute
            Dictionary<StringBuilder, float> pathStringsAndLatencies = new Dictionary<StringBuilder, float>();
            bool sortingAttributePresent = true;
            Tile.TimeAttributes sortingAttribute = Tile.TimeAttributes.Attribute1;
            try { sortingAttribute = FPGA.FPGA.Instance.GetTimeAttribute(SortByAttribute); }
            catch (Exception) { sortingAttributePresent = false; }

            foreach (List<Location> path in paths.OrderBy(l => l.Count))
            {
                StringBuilder buffer = new StringBuilder();

                for (int l = 0; l < latency.Count; l++) latency[l] = 0;

                for (int i = 0; i < path.Count; i++)
                {
                    string nextLine = path[i].ToString();
                    bool latencyStringPrinted = false;

                    for (int j = 0; j < 4; j++)
                    {
                        var att = (Tile.TimeAttributes)Enum.Parse(typeof(Tile.TimeAttributes), j.ToString());
                        if (!indices.ContainsKey(att)) continue;

                        if (PrintLatency[indices[att]])
                        {
                            if(!latencyStringPrinted)
                            {
                                nextLine += " (Latency: ";
                                latencyStringPrinted = true;
                            }
                            if (path[i].Tile.TimeData != null && i != 0 && path[i].Tile.Equals(path[i - 1].Tile))
                            {
                                uint[] pair = new uint[]
                                {
                                    path[i-1].Pip.NameKey,
                                    path[i].Pip.NameKey
                                };

                                if (path[i].Tile.TimeData.ContainsKey(pair))
                                {
                                    int ind = Tile.GetIndexForTimeAttribute(att);
                                    latency[indices[att]] += path[i].Tile.TimeData[pair][ind];                                  
                                }
                            }
                            nextLine += latency[indices[att]] + ", ";
                        }
                    }

                    if (latencyStringPrinted) nextLine = nextLine.Substring(0, nextLine.Length - 2) + ")";

                    nextLine = nextLine.PadRight(maxSegmentLength[i]);
                    nextLine += (i < path.Count - 1 ? " -> " : "");

                    // UltraScale mapping
                    if (i == path.Count - 1)
                    {
                        foreach (string v in VariableManager.Instance.GetAllVariableNames())
                        {
                            try
                            {
                                Regex r = new Regex(VariableManager.Instance.GetValue(v));
                                if (r.IsMatch(path[i].Pip.Name))
                                {
                                    nextLine += " (" + v + ")";
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                    buffer.Append(nextLine);

                }
                if (path.Count != 0)
                {
                    //this.OutputManager.WriteOutput(buffer.ToString());
                    pathStringsAndLatencies.Add(buffer, sortingAttributePresent ? latency[indices[sortingAttribute]] : 0);
                }
            }

            foreach (var path in pathStringsAndLatencies.OrderBy(p => p.Value))
            {
                OutputManager.WriteOutput(path.Key.ToString());
            }
        }

        private void SendToTCL(List<List<Location>> paths)
        {
            List<List<string>> convertedFormat = new List<List<string>>();
            for(int i=0; i<paths.Count; i++)
            {
                List<string> path = new List<string>();
                for(int j=0; j<paths[i].Count; j++)
                {
                    path.Add(paths[i][j].ToString());
                }
                convertedFormat.Add(path);
            }

            TclDLL.Tcl_SetObjResult(Program.mainInterpreter.ptr, TclAPI.Cs2Tcl(convertedFormat));
        }

        public static bool PathAlreadyFound(List<Location> path, List<List<Location>> foundPaths)
        {
            foreach (List<Location> fp in foundPaths.Where(p => p.Count == path.Count))
            {
                bool equal = true;
                for (int i = 0; i < path.Count; i++)
                {
                    if (!fp[i].Equals(path[i]))
                    {
                        equal = false;
                        break;
                    }
                }
                if (equal)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Undo()
        {
            throw new ArgumentException("The method or operation is not implemented.");
        }

        private class LatencyManager
        {
            public int PresentAttributesCount;
            public List<string> PresentAttributesNames;
            public Dictionary<Tile.TimeAttributes, int> PresentAttributesIndices;

            public bool Print { get { return PrintLatency != null && PrintLatency.Contains(true); } }
            private List<bool> PrintLatency;

            public LatencyManager(List<bool> printLatency)
            {
                PrintLatency = printLatency;

                bool[] timeAttributePresence = Tile.GetPresentTimeAttributes();
                PresentAttributesIndices = new Dictionary<Tile.TimeAttributes, int>();
                PresentAttributesNames = new List<string>();

                int pos = 0;
                if (timeAttributePresence != null)
                {
                    for (int i = 0; i < timeAttributePresence.Length; i++)
                    {
                        if (!timeAttributePresence[i]) continue;

                        var a = (Tile.TimeAttributes)Enum.Parse(typeof(Tile.TimeAttributes), i.ToString());
                        PresentAttributesIndices.Add(a, pos);
                        PresentAttributesNames.Add(FPGA.FPGA.Instance.GetTimeModelAttributeName(a));
                        pos++;
                    }
                }                

                PresentAttributesCount = pos;
            }
        }
	}

 
}


