using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Expand the current selection to pairs of CLB/CLEXLM and INT tiles", Wrapper = false)]
    class ExpandSelection : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {
            if (!RAMSelectionManager.Instance.HasMappings)
            {
                RAMSelectionManager.Instance.UpdateMapping();
            }

            List<Tile> expansion = new List<Tile>();

            // get tiles to add
            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                {
                    Tile intTile = FPGATypes.GetInterconnectTile(t);
                    if (intTile.LocationX == t.LocationX && intTile.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.Interconnect))
                    {
                        if (AddToSelection(intTile))
                        {
                            expansion.Add(intTile);
                        }
                        foreach (Tile clbTile in FPGATypes.GetCLTile(intTile))
                        {
                            if (AddToSelection(clbTile))
                            {
                                expansion.Add(clbTile);
                            }
                        }
                    }
                }

                if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect))
                {
                    foreach (Tile clbTile in FPGATypes.GetCLTile(t))
                    {
                        if (clbTile.LocationX == t.LocationX && clbTile.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(clbTile.Location, IdentifierManager.RegexTypes.CLB))
                        {
                            if (AddToSelection(clbTile))
                            {
                                expansion.Add(clbTile);
                            }
                        }
                    }
                }

                if (RAMSelectionManager.Instance.HasMapping(t))
                {
                    foreach (Tile ramBlockMember in RAMSelectionManager.Instance.GetRamBlockMembers(t))
                    {
                        if (AddToSelection(ramBlockMember))
                        {
                            expansion.Add(ramBlockMember);
                        }
                    }
                }
            }

            // expand selection
            foreach (Tile t in expansion)
            {
                TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        private bool AddToSelection(Tile where)
        {
            if (FPGA.FPGA.Instance.Contains(where.Location))
            {
                return (!TileSelectionManager.Instance.IsSelected(where.TileKey));
            }
            else
            {
                return false;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
