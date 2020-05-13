using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

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

                    TileKey key = new TileKey(x, y);

                    Tile t = FPGA.FPGA.Instance.GetTile(key);
                    if (!filter.IsMatch(t.Location))
                    {
                        continue;
                    }

                    //deselect or add the selected tile 
                    if (TileSelectionManager.Instance.IsSelected(x, y))
                    {
                        TileSelectionManager.Instance.RemoveFromSelection(key, false);
                    }
                    else
                    {
                        TileSelectionManager.Instance.AddToSelection(key, false);
                    }
                }
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
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
