using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Misc.FPL2013
{
    class IncrementalSearch : Command
    {
        protected override void DoCommandAction()
        {
            // read input
            Queue<Tuple<Location, Location>> fromToTuples = null;
            ReadSearchInput(out fromToTuples);           

            // result
            List<List<Location>> paths = new List<List<Location>>();
            List<XDLNet> nets = new List<XDLNet>();

            RouteNet routeCmd = new RouteNet();
            routeCmd.Watch = Watch;

            int size = fromToTuples.Count;
            int count = 0;

            UsageManager usageManager = new UsageManager();

            // upon sink chnmage, block the last nets
            Location lastSink = null;

            while (fromToTuples.Count > 0) 
            {
                ProgressInfo.Progress = ProgressStart + (int)((double)count++ / (double)size * ProgressShare);

                Tuple<Location, Location> tuple = fromToTuples.Dequeue();
                Location source = tuple.Item1;
                Location sink = tuple.Item2;

                bool pathFound = false;
                
                bool sinkChange = lastSink == null ? false : !lastSink.Equals(sink);
                lastSink = sink;
                if (sinkChange)
                {
                    BlockPips(nets);
                }
                Usage usage = new Usage(source, sink);

                //if (source.Tile.Location.Equals("CLEXL_X22Y16") && source.Pip.Name.Equals("XX_AQ") && sink.Tile.Location.Equals("CLEXM_X23Y18") && sink.Pip.Name.Equals("X_AX"))
                if (source.Tile.Location.Equals("CLEXL_X22Y16") && source.Pip.Name.Equals("XX_AQ") && sink.Tile.Location.Equals("CLEXM_X23Y16") && sink.Pip.Name.Equals("X_AX"))
                {

                }

                List<Location> initialSearchFront = new List<Location>();
                foreach (Location location in usageManager.GetLocationsWithExclusiveUsage(usage).OrderBy(l => Distance(l, sink)))
                {
                    initialSearchFront.Add(location);
                }
                // add default source after the others
                initialSearchFront.Add(source);          
               
                // truncate on first run
                TextWriter tw = new StreamWriter(OutputFile, count > 1);
                tw.Write(PathSearchOnFPGA.GetBanner(source, sink));



                //Console.WriteLine("Running path " + count + " " + usage + (initialSearchFront.Count > 1 ? " with shortcut" : ""));

                if (initialSearchFront.Count > 1)
                {

                }

                Watch.Start("search");
                foreach (List<Location> path in routeCmd.Route("BFS", true, initialSearchFront, sink, 100, MaxDepth, false))
                {
                    if (!PathSearchOnFPGA.PathAlreadyFound(path, paths))
                    {
                        paths.Add(path);
                        
                        XDLNet n = PathToNet(source, sink, path);
                        nets.Add(n);
                        usage.Net = n;

                        tw.Write(PathSearchOnFPGA.PathToString(source, sink, Enumerable.Repeat(path, 1)));                  
                        pathFound = true; 
                                                
                         // no blocking on CLEX LOGIC  
                        foreach (XDLPip pip in GetPipsToBlock(n))
                        {
                            Location l = new Location(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.From));
                            //Location r = new Location(FPGA.FPGA.Instance.GetTile(pip.Location), new Port(pip.To));
                            usageManager.Add(l, usage, n);
                            //usageManager.Add(r, usage);
                        }
                        break;
                    }              
                }
                Watch.Stop("search");

                if (!pathFound)
                {
                    tw.WriteLine("No path found");
                    string trigger = ("if (source.Tile.Location.Equals(\"" + source.Tile.Location + "\") && source.Pip.Name.Equals(\"" + source.Pip.Name +  "\") && sink.Tile.Location.Equals(\"" + sink.Tile.Location + "\") && sink.Pip.Name.Equals(\"" + sink.Pip.Name + "\"))");
                    Console.WriteLine(trigger);
                }

                tw.Close();
                
                if (nets.Count % 20 == 0)
                {
                //    Console.WriteLine(this.Watch.GetResults());
                }
            }            

        }

        private void BlockPips(List<XDLNet> nets)
        {
            foreach (XDLNet net in nets)
            {
                // no blocking on CLEX LOGIC  
                foreach (XDLPip pip in GetPipsToBlock(net))
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                    if (!t.IsPortBlocked(pip.From))
                    {
                        t.BlockPort(pip.From, Tile.BlockReason.Blocked);
                    }
                    if (!t.IsPortBlocked(pip.To))
                    {
                        t.BlockPort(pip.To, Tile.BlockReason.Blocked);
                    }
                }
            }
            nets.Clear();
        }

        private int Distance(Location from, Location to)
        {
            return Math.Abs(from.Tile.LocationX - to.Tile.LocationX) + Math.Abs(from.Tile.LocationY - to.Tile.LocationY);
        }

        public XDLNet PathToNet(Location start, Location sink, List<Location> path)
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

            XDLNet n = new XDLNet(path);
            n.Name = "path_0";
            n.Add(op);
            n.Add(ip);

            return n;
        }

        private Regex m_logicout = new Regex("LOGICOUT", RegexOptions.Compiled);

        private IEnumerable<XDLPip> GetPipsToBlock(XDLNet n)
        {
            return n.Pips.Where(p => 
                !(IdentifierManager.Instance.IsMatch(p.Location, IdentifierManager.RegexTypes.CLB) ||
                  m_logicout.IsMatch(p.From) ||
                  m_logicout.IsMatch(p.To)));
        }

        private void ReadSearchInput(out Queue<Tuple<Location, Location>> fromToTuples)
        {
            fromToTuples = new Queue<Tuple<Location,Location>>();

            TextReader tr = new StreamReader(InputFile);
            string l = "";
            while ((l = tr.ReadLine()) != null)
            {
                string[] tuples = l.Split('-');
                if (tuples.Length != 2)
                {                    
                    Console.WriteLine("Skipping " + l);
                    continue;
                }

                string[] left = tuples[0].Split('.');
                string[] right = tuples[1].Split('.');

                if (left.Length != 2 || right.Length != 2)
                {
                    Console.WriteLine("Skipping " + l);
                    continue;
                }

                Tile sourceTile = FPGA.FPGA.Instance.GetTile(left[0]);
                Tile sinkTile = FPGA.FPGA.Instance.GetTile(right[0]);

                Location source = new Location(sourceTile, new Port(left[1]));
                Location sink = new Location(sinkTile, new Port(right[1]));

                if (source.Tile.Location.Equals("CLEXL_X4Y10"))
                {
                    continue;
                }

                PathSearchOnFPGA.CheckExistence(source);
                PathSearchOnFPGA.CheckExistence(sink);

                fromToTuples.Enqueue(new Tuple<Location, Location>(source, sink));

            }
            tr.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private class Usage
        {
            public Usage(Location source, Location sink)
            {
                m_source = source;
                m_sink = sink;
            }

            public Location Source
            {
                get { return m_source; }
            } 

            public Location Sink
            {
                get { return m_sink; }
            }

            public override string ToString()
            {
                return Source + " -> " + Sink;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Usage))
                {
                    return false;
                }

                Usage other = (Usage)obj;

                return other.Source.Equals(Source);// && other.Sink.Equals(this.Sink);
            }

            public XDLNet Net
            {
                set { m_net = value; }
            }

            private readonly Location m_source;
            private readonly Location m_sink;
            private XDLNet m_net;
        }

        private class UsageManager
        {
            public void Add(Location location, Usage usage, XDLNet n)
            {
                if (!m_usages.ContainsKey(location))
                {
                    m_usages.Add(location, new List<Usage>());
                }

                if (!m_usages[location].Contains(usage))
                {
                    m_usages[location].Add(usage);
                }
            }

            public IEnumerable<Location> GetLocationsWithExclusiveUsage(Usage usage)
            {
                foreach(KeyValuePair<Location, List<Usage>> tuples in m_usages)
                {
                    if (tuples.Value.Count != 1)
                    {
                        continue;
                    }
                    if (!tuples.Value[0].Equals(usage))
                    {
                        continue;
                    }

                    yield return tuples.Key;
                }
            }

            private Dictionary<Location, List<Usage>> m_usages = new Dictionary<Location, List<Usage>>();

        }

        [Parameter(Comment = "Input file")]
        public string InputFile = "";

        [Parameter(Comment = "The max path length")]
        public int MaxDepth = 5;

        [Parameter(Comment = "Stop after the first MaxSolutions path")]
        public int MaxSolutions = 5;

        [Parameter(Comment = "The file to store the results in")]
        public string OutputFile = "results_all.txt";
    }
}
