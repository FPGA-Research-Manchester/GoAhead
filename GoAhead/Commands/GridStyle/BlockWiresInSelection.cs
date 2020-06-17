using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.GridStyle
{
    class BlockWiresInSelection : Command
    {
        protected override void DoCommandAction()
        {
            List<Tile> selection = new List<Tile>();

            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(tile =>
                     IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                selection.Add(t);
            }

            // get boundaries of the selection
            int minX = selection.OrderBy(t => t.TileKey.X).FirstOrDefault().TileKey.X;
            int maxX = selection.OrderByDescending(t => t.TileKey.X).FirstOrDefault().TileKey.X;
            int minY = selection.OrderBy(t => t.TileKey.Y).FirstOrDefault().TileKey.Y;
            int maxY = selection.OrderByDescending(t => t.TileKey.Y).FirstOrDefault().TileKey.Y;

            foreach (Tile tile in selection)
            {
                foreach (Port p in tile.SwitchMatrix.Ports)
                {
                    if(Regex.IsMatch(p.Name, this.PortsToUnblockRegex))
                    { 
                        bool isInSelection = false;

                        // check for each destination whether the tile is in the current selection
                        foreach (Location l in Navigator.GetDestinations(tile.Location, p.Name))
                        {
                            int posX = l.Tile.TileKey.X;
                            int posY = l.Tile.TileKey.Y;

                            isInSelection = posX >= minX && posX <= maxX && posY >= minY && posY <= maxY;

                            if(!isInSelection)
                            {
                                break;
                            }
                        }

                        // unblock the port and the other ports connected to the same wire
                        if (isInSelection)
                        {
                            tile.BlockPort(p, Tile.BlockReason.ExcludedFromBlocking);

                            foreach (Location l in Navigator.GetDestinations(tile.Location, p.Name))
                            {
                                foreach (Wire w in tile.WireList.GetAllWires(p))
                                {
                                    Tile other = l.Tile;

                                    other.BlockPort(w.PipOnOtherTile, Tile.BlockReason.ExcludedFromBlocking);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Ports that must be prevent from blocking in regular expression.")]
        public string PortsToUnblockRegex = ".*(1|2)(BEG|END).*";
    }
}
