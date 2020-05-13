using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Misc
{
    [CommandDescription(Description = "Helper command for PrintLUTRouting", Wrapper = true, Publish = true)]
    class PathSearchOnFPGAWithRegexp : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            Tile startTile = FPGA.FPGA.Instance.GetTile(StartLocation);
            Tile targetTile = FPGA.FPGA.Instance.GetTile(TargetLocation);

            List<List<Location>> m_paths = new List<List<Location>>();

            string startPortRegexp = "";
            foreach(string s in StartPortRegexps)
            {
                startPortRegexp += "(" + s + ")|";
            }
            startPortRegexp = startPortRegexp.Remove(startPortRegexp.Length - 1);

            foreach (Port startPort in startTile.SwitchMatrix.GetAllDrivers().Where(p => Regex.IsMatch(p.Name, startPortRegexp)).OrderBy(p => p.Name))
            {
                foreach (Port targetPort in targetTile.SwitchMatrix.GetDrivenPorts().Where(p => Regex.IsMatch(p.Name, TargetPortRegexp)).OrderBy(p => p.Name))
                {
                    PathSearchOnFPGA searchCmd = new PathSearchOnFPGA();
                    searchCmd.Forward = true;
                    searchCmd.MaxDepth = MaxDepth;
                    searchCmd.MaxSolutions = 1;
                    searchCmd.SearchMode = "BFS";
                    searchCmd.StartLocation = startTile.Location;
                    searchCmd.StartPort = startPort.Name;
                    searchCmd.TargetLocation = targetTile.Location;
                    searchCmd.TargetPort = targetPort.Name;
                    searchCmd.PrintBanner = false;
                    CommandExecuter.Instance.Execute(searchCmd);
                    m_paths.AddRange(searchCmd.m_paths);
                    // copy output
                    if (searchCmd.OutputManager.HasOutput)
                    {
                        OutputManager.WriteWrapperOutput(searchCmd.OutputManager.GetOutput());
                    }
                }
                // one blank for readability
                OutputManager.WriteWrapperOutput("");
            }

            return;

            List<Tuple<Tuple<Location, Location>, List<Location>>> solutionSet = new List<Tuple<Tuple<Location, Location>, List<Location>>>();
            foreach (List<Location> p in m_paths)
            {
                solutionSet.Add(new Tuple<Tuple<Location, Location>, List<Location>>(new Tuple<Location, Location>(p[0], p[p.Count - 1]), p));
                //solutionSet.Add(new Tuple<Location, Location>(p[0], p[p.Count - 1]), p);
            }


            List<Tuple<Tuple<Location, Location>, List<Location>>> west = solutionSet.Where(t => Regex.IsMatch(t.Item1.Item1.Pip.Name, StartPortRegexps[0])).Distinct().ToList();
            List<Tuple<Tuple<Location, Location>, List<Location>>> east = solutionSet.Where(t => Regex.IsMatch(t.Item1.Item1.Pip.Name, StartPortRegexps[1])).Distinct().ToList();


            SubSets subSets = new SubSets();
            int tries = 0;
            int size = 4;
            foreach (IEnumerable<Tuple<Tuple<Location, Location>, List<Location>>> wc in subSets.GetAllSubSets(west, size).Where(s => IsUnique(s)))
            {
                var westImuxes =
                   from t in wc
                   select t.Item1.Item2;

                foreach (IEnumerable<Tuple<Tuple<Location, Location>, List<Location>>> ec in subSets.GetAllSubSets(east, size).Where(s => IsUnique(s)))
                {
                    tries++;



                    var eastImuxes =
                        from t in ec
                        select t.Item1.Item2;

                    if (westImuxes.Equals(eastImuxes))
                    {
                        Console.WriteLine(wc);
                        Console.WriteLine(ec);
                    }
                }
            }

            return;

            List<Tuple<Tuple<Location, Location>, List<Location>>> result = new List<Tuple<Tuple<Location,Location>,List<Location>>>();
            foreach (IEnumerable<Tuple<Tuple<Location, Location>, List<Location>>> candidates in subSets.GetAllSubSets(solutionSet, 8))
            {
                List<Location> startPointsAndEndPoints = new List<Location>();
                foreach (Tuple<Tuple<Location, Location>, List<Location>> c in candidates)
                {
                    startPointsAndEndPoints.Add(c.Item1.Item1);
                    //startPointsAndEndPoints.Add(c.Item1.Item2);
                }
                int distinctCount = startPointsAndEndPoints.Distinct().Count();
                if (distinctCount != candidates.Count())
                {
                    continue;
                }

                //List<Tuple<Tuple<Location, Location>, List<Location>>> west = candidates.Where(t => t.Item1.Item1.Pip.Name.StartsWith(this.StartPortRegexps[0])).Distinct().ToList();
                //List<Tuple<Tuple<Location, Location>, List<Location>>> east = candidates.Where(t => t.Item1.Item1.Pip.Name.StartsWith(this.StartPortRegexps[1])).Distinct().ToList();


                if (west.Count == east.Count)
                {
                    var westImuxes =
                        from t in west
                        select t.Item1.Item2;

                    var eastImuxes =
                        from t in east
                        select t.Item1.Item2;

                    if (westImuxes.Equals(eastImuxes))
                    {
                    }
                }
                
                
               // List<Location> westImuxes = west.
            }

        }

        private bool IsUnique(IEnumerable<Tuple<Tuple<Location, Location>, List<Location>>> set)
        {
            List<Location> startPoints = new List<Location>();
            foreach (Tuple<Tuple<Location, Location>, List<Location>> c in set)
            {
                startPoints.Add(c.Item1.Item1);
            }
            int distinctCount = startPoints.Distinct().Count();
            if (distinctCount != 4) {
                return false;
            }

            List<Location> endPoints = new List<Location>();
            foreach (Tuple<Tuple<Location, Location>, List<Location>> c in set)
            {
                endPoints.Add(c.Item1.Item2);
            }
            distinctCount = endPoints.Distinct().Count();
            if (distinctCount != 4)
            {
                return false;
            }


            return true;
        }


        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Location string where to start")]
        public string StartLocation = "INT_X9Y39";

        [Parameter(Comment = "Start ports")]
        public List<string> StartPortRegexps = new List<string>();

        [Parameter(Comment = "Location string where to go")]
        public string TargetLocation = "INT_X11Y39";

        [Parameter(Comment = "Target port")]
        public string TargetPortRegexp = "_L_C";


        [Parameter(Comment = "The max path length")]
        public int MaxDepth = 5;
    }
}
