using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Add CLBS, INT, DSP, and BRAM tiles in the specified rectangle to the current selection", Wrapper = true)]
    class AddBlockToSelection : AddToSelectionCommand
    {
        public AddBlockToSelection()
        { 
        }

        public AddBlockToSelection(int x1, int y1, int x2, int y2)
        {
            // run form min to max
            int startX = Math.Min(x1, x2);
            int endX = Math.Max(x1, x2);

            int startY = Math.Min(y1, y2);
            int endY = Math.Max(y1, y2);

            int maxX = int.MinValue;
            int minX = int.MaxValue;
            int maxY = int.MinValue;
            int minY = int.MaxValue;

            Tile ul = null;
            Tile lr = null;

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    // click done out of FPGA range
                    if (!FPGA.FPGA.Instance.Contains(x, y))
                    {
                        continue;
                    }

                    Tile t = FPGA.FPGA.Instance.GetTile(x, y);
                    UpdateUpperLeftAndLowerRightCorner(ref maxX, ref minX, ref maxY, ref minY, ref ul, ref lr, t);
                }
            }

            // clicked in BRAM/DSP block?
            if( (ul == null || lr == null) && FPGA.FPGA.Instance.Contains(startX, endY))
            {
                Tile t = FPGA.FPGA.Instance.GetTile(startX, endY);
                if (RAMSelectionManager.Instance.HasMapping(t))
                {
                    foreach (Tile member in RAMSelectionManager.Instance.GetRamBlockMembers(t))
                    {
                        UpdateUpperLeftAndLowerRightCorner(ref maxX, ref minX, ref maxY, ref minY, ref ul, ref lr, member);
                    }
                }
            }

                       
            if(ul == null || lr == null)
            {
                throw new ArgumentException("Could not derive upper left and lower right anchor");
            }

            //go from INT_INTF to INT
            if (IdentifierManager.Instance.IsMatch(ul.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                ul = FPGATypes.GetInterconnectTile(ul);
            }
            if (IdentifierManager.Instance.IsMatch(lr.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                lr = FPGATypes.GetInterconnectTile(lr);
            }


            // go from CLB to INT
            if (IdentifierManager.Instance.IsMatch(ul.Location, IdentifierManager.RegexTypes.CLB))
            {
                ul = FPGATypes.GetInterconnectTile(ul);
            }
            if (IdentifierManager.Instance.IsMatch(lr.Location, IdentifierManager.RegexTypes.CLB))
            {
                lr = FPGATypes.GetInterconnectTile(lr);
            }


            UpperLeftTile = ul.Location;
            LowerRightTile = lr.Location;
        }

        private void UpdateUpperLeftAndLowerRightCorner(ref int maxX, ref int minX, ref int maxY, ref int minY, ref Tile ul, ref Tile lr, Tile t)
        {
            if (Consider(t))
            {
                if (t.LocationX <= minX && t.LocationY >= maxY)
                {
                    minX = t.LocationX;
                    maxY = t.LocationY;
                    ul = t;
                }
                if (t.LocationX >= maxX && t.LocationY <= minY)
                {
                    maxX = t.LocationX;
                    minY = t.LocationY;
                    lr = t;
                }
            }
        }

        protected override void DoCommandAction()
        {
            Tile ul = GetCorner(UpperLeftTile, FPGATypes.Direction.West);
            Tile lr = GetCorner(LowerRightTile, FPGATypes.Direction.East);
            if (ul == null || lr == null)
            {
                OutputManager.WriteWarning("Can not resolve " + UpperLeftTile + " or " + LowerRightTile);
                return;
            }
            // TODO sort lu kann auch unten links und lr kann auch oben rechts sein
            /*
            ClearSelection;
            AddBlockToSelection UpperLeftTile=INT_X21Y174 LowerRightTile=INT_X23Y170;
            ExpandSelection;
            [12:01:34 PM] diko: ClearSelection;
            AddBlockToSelection UpperLeftTile=INT_X23Y174 LowerRightTile=INT_X21Y170;
            ExpandSelection;
            */

            int startX = Math.Min(ul.TileKey.X, lr.TileKey.X);
            int endX = Math.Max(ul.TileKey.X, lr.TileKey.X);

            int startY = Math.Min(ul.TileKey.Y, lr.TileKey.Y);
            int endY = Math.Max(ul.TileKey.Y, lr.TileKey.Y);

            Regex filter = new Regex(Filter);

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    // click done out of FPGA range
                    if (!FPGA.FPGA.Instance.Contains(x, y))
                    {
                        continue;
                    }

                    Tile t = FPGA.FPGA.Instance.GetTile(x, y);
                    if (!filter.IsMatch(t.Location))
                    {
                        continue;
                    }

                    if (Consider(t))
                    {
                        // deselect or add the selected tile 
                        if (TileSelectionManager.Instance.IsSelected(x, y))
                        {
                            TileSelectionManager.Instance.RemoveFromSelection(t.TileKey, false);
                        }
                        else
                        {
                            TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                        }
                    }                
                }
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        private Tile GetCorner(string identifier, FPGATypes.Direction dir)
        {
            Tile tile = null;
            if (FPGA.FPGA.Instance.Contains(identifier))
            {             
                tile = FPGA.FPGA.Instance.GetTile(identifier);
            }
            else if (identifier.Contains("_"))
            {
                int split = identifier.IndexOf('_');
                string prefix = identifier.Substring(0, split + 1);
                string suffix = identifier.Substring(split, identifier.Length - split);
                tile = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => t.Location.StartsWith(prefix) && t.Location.EndsWith(suffix));
                // if we can not resolve the identifer, triggger error handling in Do
                if(tile==null)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            List<Tile> otherTiles = new List<Tile>();            
            otherTiles.Add(tile);

            // there might be more left interconnects
            if (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                otherTiles.Add(FPGATypes.GetInterconnectTile(tile));
            }
            else if (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                foreach(Tile o in FPGATypes.GetCLTile(tile))
                {
                    otherTiles.Add(o);
                }
            }
            if(dir == FPGATypes.Direction.West)
            {
                int min = otherTiles.Min(t => t.TileKey.X);
                return otherTiles.FirstOrDefault(t => t.TileKey.X == min);
            }
            else if (dir == FPGATypes.Direction.East)
            {
                int max = otherTiles.Max(t => t.TileKey.X);
                return otherTiles.FirstOrDefault(t => t.TileKey.X == max);
            }
            else
            {
                throw new ArgumentException(GetType().Name + ".GetCorner only supports East and West");
            }
        }

        private bool Consider(Tile t)
        {
            return 
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB) ||
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect) ||
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.BRAM) ||
                IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.DSP);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Only selected those tiles in the given range that match this filter", PrintParameter=false)]
        public string Filter = ".*";
        
        [Parameter(Comment = "The upper left tile")]
        public string UpperLeftTile = "";

        [Parameter(Comment = "The lower right tile")]
        public string LowerRightTile = "";
    }
}
