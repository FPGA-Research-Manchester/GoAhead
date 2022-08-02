using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.Misc
{
    [CommandDescription(Description = "Sets the cost for a port or ports in one or more tiles.")]
    class SetPortCosts : Command
    {
        [Parameter(Comment = "The tile to set the port cost for")]
        public string TileLocation = "";

        [Parameter(Comment = "The port to set the cost for")]
        public string PortLocation = "";

        [Parameter(Comment = "The cost value")]
        public int NewCost = 0;

        protected override void DoCommandAction()
        {
            // Find the tiles where the tile string supplied matches the tile's name.
            List<Tile> tileList = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, TileLocation)).OrderBy(t => t.Location).ToList();
            foreach (Tile t in tileList)
            {
                Location fromLoc = new Location(t, new Port(PortLocation));
                if(!FPGA.FPGA.Instance.GetAllLocations().Contains(fromLoc))
                {
                    continue;
                }
                foreach (Wire w in t.WireList)
                {
                    // Update wire cost if it originates at the right port.
                    if (w.LocalPip.Equals(PortLocation))
                    {
                        w.Cost = NewCost;
                    }
                }
                foreach(Location toLoc in GetReachableLocations(fromLoc)) 
                {
                    t.AddConnection(fromLoc, toLoc, NewCost);
                }
            }
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

        public override void Undo()
        {
            throw new ArgumentException("The method or operation is not implemented.");
        }
    }
}
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.Misc
{
    class SetEdgeCosts
    {
        [CommandDescription(Description = "Sets the cost for a wire using the tiles and ports it goes to and from.")]
        class SetPortCosts : Command
        {
            [Parameter(Comment = "The start tile to set the port cost for")]
            public string StartTile = "";

            [Parameter(Comment = "The start port to set the cost for")]
            public string StartPort = "";

            [Parameter(Comment = "The end tile to set the port cost for")]
            public string EndTile = "";

            [Parameter(Comment = "The end port to set the cost for")]
            public string EndPort = "";

            [Parameter(Comment = "The latency value")]
            public double NewCost = 0;

            protected override void DoCommandAction()
            {
                int wiresUpdated = 0;
                // Find tiles that satisfy the supplied tile regex, ordered alphabetically.
                List<Tile> tileList = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartTile)).OrderBy(t => t.Location).ToList();
                foreach (Tile t in tileList)
                {
                    // Get the tile's wire list via an integer hash code.
                    WireList wl = FPGA.FPGA.Instance.GetWireList(t.WireListHashCode);
                    foreach (Wire w in wl)
                    {
                        // Check that the wire matches input parameters.
                        if (w.LocalPip.Equals(StartPort) && w.PipOnOtherTile.Equals(EndPort) && t.GetTileAtWireEnd(w).TileKey.ToString().Equals(EndTile))
                        {
                            w.Cost = NewCost;
                            wiresUpdated++;
                        }
                    }
                }
                if (wiresUpdated == 0)
                    Console.WriteLine("No wires were found from start location(s) to target location(s).");
                else
                    Console.WriteLine($"{wiresUpdated} wires were updated with the new cost.");
            }
            public override void Undo()
            {
                throw new ArgumentException("The method or operation is not implemented.");
            }
        }
    }
}
 */