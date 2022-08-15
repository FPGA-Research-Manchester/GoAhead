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
    class SetEdgeCosts : Command
    {
        [Parameter(Comment = "The tile to set the port cost for")]
        public string StartTile = "";

        [Parameter(Comment = "The port to set the cost for")]
        public string StartPort = "";

        [Parameter(Comment = "The end tile to set the cost for")]
        public string EndTile = "";

        [Parameter(Comment = "The end port to set the cost for")]
        public string EndPort = "";

        [Parameter(Comment = "The cost value")]
        public int NewCost = 0;

        protected override void DoCommandAction()
        {
            // Find the tiles where the tile string supplied matches the tile's name.
            List<Tile> tileList = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartTile)).OrderBy(t => t.Location).ToList();
            Tile endTile = FPGA.FPGA.Instance.GetAllTiles().Where(t => t.Location.ToString() == EndTile).FirstOrDefault();
            if (endTile == null && (EndTile != "" || EndPort != ""))
            {
                Console.WriteLine("Error when setting cost. Location may not exist.");
                return;
            }
            Location endLoc = new Location(endTile, new Port(EndPort));

            foreach (Tile t in tileList)
            {
                Location fromLoc = new Location(t, new Port(StartPort));
                if(!FPGA.FPGA.Instance.GetAllLocations().Contains(fromLoc))
                {
                    continue;
                }
                foreach (Wire w in t.WireList)
                {
                    Location toLoc = new Location(t.GetTileAtWireEnd(w), new Port(w.PipOnOtherTile));
                    // Update wire cost if it originates at the right port.
                    if (w.LocalPip.Equals(StartPort) && (EndTile == "" || EndPort == "" || toLoc.Equals(endLoc)))
                    {
                        w.Cost = NewCost;
                        FPGA.FPGA.Instance.m_tiles.AddConnection(fromLoc, toLoc, NewCost);
                        //t.AddConnection(fromLoc, toLoc, NewCost);
                    }
                }
                // For locations that you can get to without using wires.
                foreach(Location toLoc in GetReachableLocations(fromLoc)) 
                {
                    if (EndTile == "" || EndPort == "" || toLoc.Equals(endLoc))
                    {
                        FPGA.FPGA.Instance.m_tiles.AddConnection(fromLoc, toLoc, NewCost);
                        //t.AddConnection(fromLoc, toLoc, NewCost);
                    }
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