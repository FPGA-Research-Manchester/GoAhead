using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Selection
{
    [Serializable]
    [CommandDescription(Description = "Add tiles in the specified rectangle to the current selection", Wrapper = true)]
    public class AddToSelectionXY : AddToSelectionCommand
    {
        public AddToSelectionXY()
        {
        }

        public AddToSelectionXY(int x1, int y1, int x2, int y2)
        {
            UpperLeftX = Math.Min(x1, x2);
            UpperLeftY = Math.Min(y1, y2);
            LowerRightX = Math.Max(x1, x2);
            LowerRightY = Math.Max(y1, y2);   
        }

        protected override void DoCommandAction()
        {
            Regex filter = new Regex(Filter);
            List<Tile> remove = new List<Tile>();

            // run form min to max
            int startX = Math.Min(UpperLeftX, LowerRightX);
            int endX = Math.Max(UpperLeftX, LowerRightX);

            int startY = Math.Min(UpperLeftY, LowerRightY);
            int endY = Math.Max(UpperLeftY, LowerRightY);
            
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    // click done out of fpga range
                    if (!FPGA.FPGA.Instance.Contains(x, y))
                    {
                        continue;
                    }

                    

                    Tile t = FPGA.FPGA.Instance.GetTile(x,y);
                    TileKey key = t.TileKey;

                    if (!filter.IsMatch(t.Location))
                    {
                        continue;
                    }

                    //deselect or add the selected tile 
                    if (TileSelectionManager.Instance.IsSelected(x, y))
                    {
                        RemoveAndCheckIfPreviousExpandedSelection(t,remove);
                    }
                    else if(!remove.Contains(t))
                    {
                        TileSelectionManager.Instance.AddToSelection(key, false);
                    }


                }
            }


            foreach (Tile tile in remove)
            {
                TileSelectionManager.Instance.RemoveFromSelection(tile.TileKey, false);
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private void RemoveAndCheckIfPreviousExpandedSelection(Tile t, List<Tile> remove)
        {

            if (CheckSelection(t))
            {
                remove.Add(t);
            }

            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
            {
                Tile intTile = FPGATypes.GetInterconnectTile(t);
                if (intTile.LocationX == t.LocationX && intTile.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.Interconnect))
                {
                    //Add interconnect tile.
                    if (CheckSelection(intTile))
                    {
                        remove.Add(intTile);
                    }

                    //Add any adjacent clb tiles.
                    foreach (Tile clbTile in FPGATypes.GetCLTile(intTile))
                    {
                        if (CheckSelection(clbTile))
                        {
                            remove.Add(clbTile);
                        }
                    }

                    //Add any adjacent INT_INTF tiles.
                    foreach (Tile subIntTile in FPGATypes.GetSubInterconnectTile(intTile))
                    {
                        if (CheckSelection(subIntTile))
                        {
                            remove.Add(subIntTile);
                        }
                    }

                }
            }

            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                //Add adjacent CLB tiles.
                foreach (Tile clbTile in FPGATypes.GetCLTile(t))
                {
                    if (clbTile.LocationX == t.LocationX && clbTile.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(clbTile.Location, IdentifierManager.RegexTypes.CLB))
                    {
                        if (CheckSelection(clbTile))
                        {
                            remove.Add(clbTile);
                        }
                    }
                }

                //Add any adjacent INT_INTF tiles.
                foreach (Tile subIntTile in FPGATypes.GetSubInterconnectTile(t))
                {
                    if (CheckSelection(subIntTile))
                    {
                        remove.Add(subIntTile);
                    }
                }

            }

            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                Tile intTile = FPGATypes.GetInterconnectTile(t);
                if (intTile.LocationX == t.LocationX && intTile.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.Interconnect))
                {
                    //Add interconnect tile.
                    if (CheckSelection(intTile))
                    {
                        remove.Add(intTile);
                    }

                    //Add adjacent CLB tiles.
                    foreach (Tile clbTile in FPGATypes.GetCLTile(intTile))
                    {
                        if (CheckSelection(clbTile))
                        {
                            remove.Add(clbTile);
                        }
                    }
                }
            }


            if (RAMSelectionManager.Instance.HasMapping(t))
            {
                foreach (Tile ramBlockMember in RAMSelectionManager.Instance.GetRamBlockMembers(t))
                {
                    if(CheckSelection(ramBlockMember))
                    {
                        remove.Add(ramBlockMember);
                    }
                }
            }

        }


        private bool CheckSelection(Tile where)
        {
            if (FPGA.FPGA.Instance.Contains(where.Location))
            {
                return (TileSelectionManager.Instance.IsSelected(where.TileKey));
            }
            else
            {
                return false;
            }
        }

        protected override string GetPrimitiveValue(System.Reflection.FieldInfo fi)
        {
            // make coordinates relative
            Tile anchor = Objects.SelectionManager.Instance.Anchor;
            string xName = Objects.SelectionManager.Instance.XAnchorName;
            string yName = Objects.SelectionManager.Instance.YAnchorName;
            if (anchor != null)
            {
                int x = int.Parse(fi.GetValue(this).ToString());
                int y = int.Parse(fi.GetValue(this).ToString());
                if (fi.Name.EndsWith("X"))
                {
                    if (x > anchor.TileKey.X)
                    {
                        return xName + "+" + (x - anchor.TileKey.X);
                    }
                    else
                    {
                        return xName + "-" + (anchor.TileKey.X - x);
                    }

                }
                else //if (fi.Name.EndsWith("Y"))
                {
                    if (y > anchor.TileKey.Y)
                    {
                        return yName + "+" + (y - anchor.TileKey.Y);
                    }
                    else
                    {
                        return yName + "-" + (anchor.TileKey.Y - y);
                    }
                }
            }
            else
            {
                return base.GetPrimitiveValue(fi);
            }
        }

        [Parameter(Comment = "Only selected those tiles in the given range that match this filter")]
        public string Filter = ".*";

        [Parameter(Comment = "The X coordinate of the upper left tile")]
        public int UpperLeftX = 0;

        [Parameter(Comment = "The Y coordinate of the upper left tile")]
        public int UpperLeftY = 0;

        [Parameter(Comment = "The X coordinate of the lower right tile")]
        public int LowerRightX = 0;

        [Parameter(Comment = "The Y coordinate of the lower right tile")]
        public int LowerRightY = 0;

    }
}
