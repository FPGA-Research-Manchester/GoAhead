using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Search;
using GoAhead.XDLParsing;

namespace GoAhead.Commands.Misc
{
    class PathSearchOnFPGAIncremental : Command
    {
        protected override void DoCommandAction()
        {
            if (this.StartLocations.Count != this.SinkLocations.Count)
            {
                throw new ArgumentException("Number of sinks and source must match");
            }
            List<Location> sources = this.GetLocations(this.StartLocations);
            List<Location> sinks = this.GetLocations(this.SinkLocations);

            // result
            List<List<Location>> paths = new List<List<Location>>();

            Dictionary<Location, PathUsage> pipUsage = new Dictionary<Location, PathUsage>();

            for (int i = 0; i < sources.Count; i++)
            {
                Location source = sources[i];
                Location sink = sinks[i];
                PathSearchOnFPGA.CheckLocationsForExistence(source, sink);

                RouteNet routeCmd = new RouteNet();
                foreach (List<Location> path in routeCmd.Route("BFS", true, Enumerable.Repeat(source, 1), sink, 100, this.MaxDepth, true))
                {
                    if (!PathSearchOnFPGA.PathAlreadyFound(path, paths))
                    {
                        paths.Add(path);

                        // attach
                        PathUsage usage = new PathUsage(source, sink);

                        foreach (Location l in path)
                        {
                            if(
                        }
                    }
                }

            }
        }

        private List<Location> GetLocations(List<String> locationStrings)
        {
            List<Location> result = new List<Location>();
            foreach (String s in locationStrings)
            {
                String[] atoms = s.Split('.');
                if(atoms.Length != 2)
                {
                    throw new ArgumentException("Expecting e.g INT_X34Y3.EE2BEG0, found " + s);
                }
                Tile t = FPGA.FPGA.Instance.GetTile(atoms[0]);
                Port p = new Port(atoms[1]);
                Location loc = new Location(t, p);
                result.Add(loc);
            }
            return result;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private class PathUsage
        {
            public PathUsage(Location source, Location sink)
            {
                this.m_source = source;
                this.m_sink = sink;
            }

            private readonly Location m_source;
            private readonly Location m_sink;
        }

        private class PathUsageManager
        {


        }


        [Parameter(Comment = "List of sources (e.g. INT_X3Y45.LOGIC_OUTS5,INT_X3Y47.LOGIC_OUTS5")]
        public List<String> StartLocations = new List<String>();

        [Parameter(Comment = "List of sinks (e.g. INT_X3Y45.LOGIC_IN3,INT_X3Y47.LOGIC_IN3")]
        public List<String> SinkLocations = new List<String>();

        [Parameter(Comment = "The max path length")]
        public int MaxDepth = 5;
    }
}
