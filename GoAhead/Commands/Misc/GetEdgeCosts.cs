using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoAhead.Commands.Misc
{
    internal class GetEdgeCosts : Command
    {
        [Parameter(Comment = "The tile to see cost for")]
        public string StartTile = "";

        [Parameter(Comment = "The port to see the cost for")]
        public string StartPort = "";

        [Parameter(Comment = "The end tile to see cost for")]
        public string EndTile = "";

        [Parameter(Comment = "The end port to see the cost for")]
        public string EndPort = "";

        [Parameter(Comment = "Hide edges with a weight of 1")]
        public bool HideUnweightedEdges = false;

        protected override void DoCommandAction()
        {
            var endTiles = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartTile) && !Regex.IsMatch(t.Location, "NULL")).ToList();
            List<Location> endLocs = new List<Location>();
            foreach(Tile t in endTiles)
            {
                var locs = FPGA.FPGA.Instance.GetAllLocationsFromTile(t);
                int count = locs.Count();
                foreach (Port p in t.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.ToString(), EndPort) && locs.Contains(new Location(t, p))))
                {
                    endLocs.Add(new Location(t, p));
                }
            }
            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartTile) && !Regex.IsMatch(t.Location, "NULL")))
            {
                var locs = FPGA.FPGA.Instance.GetAllLocationsFromTile(t);
                foreach (Port p in t.SwitchMatrix.Ports.Where(p => Regex.IsMatch(p.ToString(), StartPort) && locs.Contains(new Location(t, p))))
                {
                    Location fromLoc = new Location(t, p);
                    foreach (Location endLoc in endLocs)
                    {
                        if (GetReachableLocations(fromLoc).Contains(endLoc))
                        {
                            int cost = FPGA.FPGA.Instance.m_tiles.GetConnectionCost(fromLoc, endLoc) ?? 1;
                            if(cost != 1 || !HideUnweightedEdges)    
                                Console.WriteLine($"{fromLoc} -> {endLoc} : {cost}");
                        }
                    }
                }
            }
        }
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Location> GetReachableLocations(Location currentLocation, bool useUserSelection = false)
        {
            foreach (Port pip in currentLocation.Tile.SwitchMatrix.GetDrivenPorts(currentLocation.Pip).Where(p => FollowPort(currentLocation.Tile, p)))
            {
                Location next = new Location(currentLocation.Tile, pip);
                if (useUserSelection && !FPGA.FPGA.Instance.GetAllLocationsInSelection().Contains(next))
                    continue;
                yield return next;
            }

            foreach (Location next in Navigator.GetDestinations(currentLocation.Tile, currentLocation.Pip).Where(loc => FollowPort(loc.Tile, loc.Pip)))
            {
                if (useUserSelection && !FPGA.FPGA.Instance.GetAllLocationsInSelection().Contains(next))
                    continue;
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
    }
}
