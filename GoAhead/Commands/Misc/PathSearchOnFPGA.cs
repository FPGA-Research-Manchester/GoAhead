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
        public string SearchMode = "BFS";

        [Parameter(Comment = "Whether to search in forward (true) or backward direction (false)")]
        public bool Forward = true;

        [Parameter(Comment = "Dont use!")]
        public bool KeepPathsIndependet = false;

        [Parameter(Comment = "Whether to block used ports after finding a path (set to true for incremental search)")]
        public bool BlockUsedPorts = false;

        [Parameter(Comment = "How to print out the results (XDL, CHAIN, TCL)")]
        public string OutputMode = "CHAIN";

        [Parameter(Comment = "Location string where to start")]
        public string StartLocation = "";

        [Parameter(Comment = "Start port")]
        public string StartPort = "";

        [Parameter(Comment = "Location string where to go")]
        public string TargetLocation = "";

        [Parameter(Comment = "Target port")]
        public string TargetPort = "";

        [Parameter(Comment = "Stop after the first MaxSolutions path")]
        public int MaxSolutions = 5;

        [Parameter(Comment = "The max path length")]
        public int MaxDepth = 7;

        [Parameter(Comment = "Print Banner")]
        public bool PrintBanner = true;

        [Parameter(Comment = "Print Latency")]
        public List<string> PrintLatency = new List<string>();

        [Parameter(Comment = "Sort the paths found by the latency attribute")]
        public string SortByAttribute = "";

        // EXPERIMENTAL
        [Parameter(Comment = "Toggle exact regex matching for ports")]
        public bool ExactPortMatch = false;

        public List<List<Location>> m_paths = new List<List<Location>>();
        private LatencyManager LatencyMan;
        private List<object> TCL_output = null;
        private const string line_dash = "---------------------------------------------------------------------------------";

        protected override void DoCommandAction()
        {
            List<Tile> startTiles = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartLocation)).OrderBy(t => t.Location).ToList();
            List<Tile> targetTiles = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, TargetLocation)).OrderBy(t => t.Location).ToList();

            if (startTiles.Count == 0)
                OutputManager.WriteOutput("No start tiles were found matching the supplied regex.");
            if (targetTiles.Count == 0)
                OutputManager.WriteOutput("No target tiles were found matching the supplied regex.");

            // All matched regex start tiles
            for (int i = 0; i < startTiles.Count; i++)
            {
                string startPortRegex = ExactPortMatch ? "^" + StartPort + "$" : StartPort;
                List<Port> startPorts = startTiles[i].SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, startPortRegex)).OrderBy(p => p.Name).ToList();

                if (startPorts.Count == 0)
                    OutputManager.WriteOutput("No ports were found on the start tile matching the supplied regex.");

                // All matched regex target tiles
                for (int j = 0; j < targetTiles.Count; j++)
                {
                    string targetPortRegex = ExactPortMatch ? "^" + TargetPort + "$" : TargetPort;
                    List<Port> targetPorts = targetTiles[j].SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.Name, targetPortRegex)).OrderBy(p => p.Name).ToList();

                    if (targetPorts.Count == 0)
                        OutputManager.WriteOutput("No ports were found on the target tile matching the supplied regex.");

                    // All matched regex start ports
                    for (int k = 0; k < startPorts.Count; k++)
                    {
                        Location startLocation = new Location(startTiles[i], startPorts[k]);

                        // All matched regex target ports
                        for (int l = 0; l < targetPorts.Count; l++)
                        {
                            Location targetLocation = new Location(targetTiles[j], targetPorts[l]);

                            ExecutePathSearch(startLocation, targetLocation);
                        }
                    }
                }
            }

            if (OutputMode.ToUpper().Equals("TCL"))
            {
                TclDLL.Tcl_SetObjResult(Program.mainInterpreter.ptr, TclAPI.Cs2Tcl(TCL_output));
            }
        }

        private void ExecutePathSearch(Location startLocation, Location targetLocation)
        {
            m_paths.Clear();

            CheckExistence(startLocation);
            CheckExistence(targetLocation);

            if (KeepPathsIndependet)
                LatencyMan = new LatencyManager(KeepPathsIndependet);
            else
                LatencyMan = new LatencyManager(PrintLatency, SortByAttribute);

            // print results
            if (PrintBanner && !OutputMode.ToUpper().Equals("TCL"))
            {
                OutputManager.WriteOutput(GetBannerWithLatency(startLocation, targetLocation));
            }

            RouteNet routeCmd = new RouteNet();
            routeCmd.Watch = Watch;

            foreach (List<Location> path in routeCmd.Route(SearchMode, Forward, Enumerable.Repeat(startLocation, 1), targetLocation, 1000, MaxDepth, KeepPathsIndependet))
            {
                if (!PathAlreadyFound(path, m_paths))
                {
                    m_paths.Add(path);

                    if (BlockUsedPorts)
                    {
                        foreach (Location l in path)
                        {
                            if (!l.Tile.IsPortBlocked(l.Pip))
                            {
                                l.Tile.BlockPort(l.Pip, Tile.BlockReason.Blocked);
                            }
                        }
                    }

                    ProgressInfo.Progress = ProgressStart + (int)(m_paths.Count / (double)MaxSolutions * ProgressShare);

                    if (m_paths.Count >= MaxSolutions)
                    {
                        break;
                    }
                }
            }

            if (m_paths.Count == 0)
            {
                OutputManager.WriteOutput("No path found. Try raising the MaxDepth parameter.");
            }
            else if (OutputMode.ToUpper().Equals("CHAIN"))
            {
                PrintAsChain(m_paths);
            }
            else if (OutputMode.ToUpper().Equals("XDL"))
            {
                OutputManager.WriteOutput(PathToString(startLocation, targetLocation, m_paths));
            }
            else if (OutputMode.ToUpper().Equals("TCL"))
            {
                AddToTCLOutput(m_paths);
            }
        }

        public string GetBannerWithLatency(Location start, Location sink)
        {
            string banner = GetBanner(start, sink);

            // Print the latency banner if the user provided input for at least one of PrintLatency or SortByAttribute
            if (PrintLatency.Where(l => !string.IsNullOrEmpty(l)).Count() > 0 || !string.IsNullOrEmpty(SortByAttribute))
                banner += LatencyMan.GetLatencyBanner();

            return banner;            
        }

        public static string GetBanner(Location start, Location sink)
        {
            string result = "";
            result += line_dash + Environment.NewLine;
            result += "--From " + start.Tile.Location + "." + start.Pip.Name + " to " + sink.Tile.Location + "." + sink.Pip.Name + Environment.NewLine;
            result += line_dash + Environment.NewLine;

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

        public static string PathToString(Location start, Location sink, IEnumerable<List<Location>> paths)
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

            // Sorting by attribute
            Dictionary<StringBuilder, float> pathStringsAndLatencies = new Dictionary<StringBuilder, float>();
            int pathNum = 1;
            foreach (List<Location> path in paths.OrderBy(l => l.Count))
            {
                StringBuilder buffer = new StringBuilder();
                LatencyMan.StartLatencyRecorder();
                buffer.Append(pathNum++.ToString() + ". ");

                for (int i = 0; i < path.Count; i++)
                {
                    string nextLine = path[i].ToString();

                    // Attempt printing latency
                    string latency = LatencyMan.RecordLatencyForPathSegment(
                        Forward ? (i == 0 ? null : path[i - 1]) : path[i], 
                        Forward ? path[i] : (i == 0 ? null : path[i - 1]));
                    if (!"".Equals(latency)) nextLine += " " + latency;
                    
                    nextLine = nextLine.PadRight(maxSegmentLength[i]);
                    nextLine += (i < path.Count - 1 ? " -> " : "");

                    // UltraScale mapping - what the hell is this
                    /*if (i == path.Count - 1)
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
                            catch { }
                        }
                    }*/
                    buffer.Append(nextLine);
                }
                if (path.Count != 0)
                {
                    //this.OutputManager.WriteOutput(buffer.ToString());
                    pathStringsAndLatencies.Add(buffer, LatencyMan.StopLatencyRecorder());
                }
            }

            foreach (var path in pathStringsAndLatencies.OrderBy(p => p.Value))
            {
                OutputManager.WriteOutput(path.Key.ToString());
            }
        }

        private void AddToTCLOutput(List<List<Location>> paths)
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

            if (TCL_output == null) TCL_output = new List<object>();
            TCL_output.Add(convertedFormat);
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

            //public bool Print { get { return PrintLatency != null && PrintLatency.Contains(true); } }
            private List<Tile.TimeAttributes> PrintLatency = null;
            private Tile.TimeAttributes? SortingAttribute = null;

            public LatencyManager(List<string> printLatency, string sortingAttribute)
            {
                // Determine present attributes
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

                ValidatePrintLatency(printLatency);
                ValidateSortByAttribute(sortingAttribute);                
            }

            private readonly bool keepPathsIndependent = false;
            public LatencyManager(bool keepPathsIndependent)
            {
                this.keepPathsIndependent = keepPathsIndependent;
            }

            /// <summary>
            /// Validates the parameter PrintLatency
            /// </summary>
            /// <param name="printLatency"></param>
            private void ValidatePrintLatency(List<string> printLatency)
            {
                // If there is any input
                if (printLatency.Where(l => !"".Equals(l)).Count() > 0)
                {
                    // Input list length can't be bigger than the amount of present attributes
                    if (PresentAttributesCount < printLatency.Count)
                    {
                        msg_printLatency = 
                            "none - Printing latency requested for " + printLatency.Count +
                            " entries, but the model contains data for " + PresentAttributesCount + 
                            " attributes.";
                        return;
                    }                    
                }
                // If no input - exit
                else
                {
                    msg_printLatency = "none";
                    return;
                }

                // There is input, prepare for parsing
                PrintLatency = new List<Tile.TimeAttributes>();
                bool parsed = false;

                // Try parsing in boolean format first
                if (PresentAttributesCount == printLatency.Count)
                {
                    for (int i = 0; i < printLatency.Count; i++)
                    {
                        parsed = true;
                        if (bool.TryParse(printLatency[i], out bool b))
                        {
                            if (b) PrintLatency.Add(FPGA.FPGA.Instance.GetTimeAttribute(PresentAttributesNames[i]));
                        }
                        else
                        {
                            PrintLatency.Clear();
                            parsed = false;
                            break;
                        }
                    }
                }                

                // If that didn't work try the string format
                if (!parsed)
                {
                    try
                    {
                        for (int i = 0; i < printLatency.Count; i++)
                        {
                            Tile.TimeAttributes attribute = FPGA.FPGA.Instance.GetTimeAttribute(printLatency[i]);
                            // If the user inputted the same attribute twice, we accept the input but we don't repeat the attribute
                            if (!PrintLatency.Contains(attribute))
                            {
                                PrintLatency.Add(attribute);
                            }
                        }
                        parsed = true;
                    }
                    catch (Exception)
                    {
                        // Invalid attribute name supplied by the user, abort the printing
                        PrintLatency = null;
                    }
                }

                if (!parsed)
                {
                    msg_printLatency = "none - Failed to parse the PrintLatency input.";
                }
            }

            /// <summary>
            /// Validates the parameter SortByAttribute
            /// </summary>
            /// <param name="attribute"></param>
            private void ValidateSortByAttribute(string attribute)
            {
                // If there is any input
                if (!string.IsNullOrEmpty(attribute))
                {
                    try
                    {
                        Tile.TimeAttributes att = FPGA.FPGA.Instance.GetTimeAttribute(attribute);

                        // Ensure that the attribute is in the model
                        if (Tile.GetPresentTimeAttributes()[(int)att])
                        {
                            SortingAttribute = att;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                // If no input - exit
                else
                {
                    msg_sortingAttribute = "none";
                    return;
                }

                // Input was not parsed successfully
                if (SortingAttribute == null)
                {
                    msg_sortingAttribute = "none - Supplied sorting attribute was not found in the model.";
                    return;
                }

                // Check if sorting attribute is included in PrintLatency
                if (PrintLatency == null || (SortingAttribute is Tile.TimeAttributes ta && !PrintLatency.Contains(ta)))
                {
                    msg_sortingAttribute = "none - Supplied sorting attribute was not enabled in PrintLatency.";
                }
            }

            private string msg_printLatency = "";
            private string msg_sortingAttribute = "";

            public string GetLatencyBanner()
            {
                if (keepPathsIndependent)
                {
                    return "--Printing latency is not supported with KeepPathsIndependet" + Environment.NewLine +
                        line_dash + Environment.NewLine;
                }

                string result = "";

                // Latency
                string latencyStr = "";
                
                if (!"".Equals(msg_printLatency))
                {
                    latencyStr = msg_printLatency;
                }
                else if (PrintLatency != null && PrintLatency.Count > 0)
                {
                    for (int i = 0; i < PrintLatency.Count; i++)
                    {
                        latencyStr += FPGA.FPGA.Instance.GetTimeModelAttributeName(PrintLatency[i]) + (i == PrintLatency.Count - 1 ? "" : ",");
                    }
                    latencyStr = "[" + latencyStr + "]";
                }

                // Sorting attribute
                string saStr = "";
                if (!"".Equals(msg_sortingAttribute))
                {
                    saStr = msg_sortingAttribute;
                }
                else if (SortingAttribute is Tile.TimeAttributes att)
                {
                    saStr = FPGA.FPGA.Instance.GetTimeModelAttributeName(att);
                }

                if (!"".Equals(latencyStr))
                    result += "--Latency: " + latencyStr + Environment.NewLine;
                if (!"".Equals(saStr))
                    result += "--Sorted by: " + saStr + Environment.NewLine;

                result += line_dash + Environment.NewLine;

                return result;
            }

            #region Latency Recorder
            private bool recording = false;
            private List<float> rec_latencies = null;
            private string Rec_latencies_str
            {
                get
                {
                    string result = "";
                    if (rec_latencies == null)
                        return result;
                    for (int i = 0; i < rec_latencies.Count; i++)
                        result += rec_latencies[i] + (i == rec_latencies.Count - 1 ? "" : ",");
                    return "(" + result + ")";
                }
            }

            /// <summary>
            /// Refreshes variables required for latency accumulation for a path.
            /// RecordLatencyForPathSegment may be called arbitrarily between this and StopLatencyRecorder.
            /// </summary>
            public void StartLatencyRecorder()
            {
                if (keepPathsIndependent || PrintLatency == null)
                    return;

                rec_latencies = new List<float>();
                foreach (var l in PrintLatency)
                    rec_latencies.Add(0);

                recording = true;
            }

            /// <summary>
            /// Used to add the latency of the segment [start,end] to the total accumulated latency since the StartLatencyRecorder call.
            /// Returns a string displaying the total latency after the processing the segment.
            /// </summary>
            /// <param name="start"></param>
            /// <param name="end"></param>
            /// <returns></returns>
            public string RecordLatencyForPathSegment(Location start, Location end)
            {
                // If the request is invalid
                if (!recording || end == null)
                    return "";

                // If we can't find time data for the required segment
                if (end.Tile.TimeData == null || start == null || !start.Tile.Equals(end.Tile))
                    return Rec_latencies_str;

                // Accumulate latencies
                for (int i = 0; i < PrintLatency.Count; i++)
                {
                    rec_latencies[i] += end.Tile.GetTimeData(start.Pip, end.Pip, PrintLatency[i]);
                }

                return Rec_latencies_str;
            }

            /// <summary>
            /// Stops the accumulation of latency started with StartLatencyRecorder.
            /// Returns the total latency of the sorting attribute, otherwise returns 0.
            /// </summary>
            /// <returns></returns>
            public float StopLatencyRecorder()
            {
                float result = 0;

                if (!recording)
                    return result;

                if (SortingAttribute is Tile.TimeAttributes att)
                {
                    int i = PrintLatency.IndexOf(att);
                    if (i > -1)
                    {
                        result = rec_latencies[i];
                    }
                }

                recording = false;

                return result;
            }
            #endregion
        }
    }

 
}


