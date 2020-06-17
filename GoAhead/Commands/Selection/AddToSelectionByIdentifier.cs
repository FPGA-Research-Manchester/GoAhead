using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;


namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Add tiles in the bounding box INT_X+UpperLeftX+YUpperLeftY:INT_X+LowerRightX+Y+LowerRightY to the current selection", Wrapper = true, Publish=false)]
    class AddToSelectionByIdentifier : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {
            String t1Identifier = "";
            String t2Identifier = "";
            bool validIdentifierFound = false;

            if (this.UpperLeftIdentifierPrefices.Count == 0)
            {
                bool t1Found = false;
                for (int i = 0; i < IdentifierPrefixManager.Instance.Prefices.Count; i++)
                {
                    String prefix = IdentifierPrefixManager.Instance.Prefices[i];
                    t1Identifier = prefix + "X" + this.UpperLeftX + "Y" + this.UpperLeftY;
                    if (FPGA.FPGA.Instance.Contains(t1Identifier))
                    {
                        t1Found = true;
                        break;
                    }
                    // TODO t1Identifier for slices
                }
                bool t2Found = false;
                for (int i = 0; i < IdentifierPrefixManager.Instance.Prefices.Count; i++)
                {
                    String prefix = IdentifierPrefixManager.Instance.Prefices[i];
                    t2Identifier = prefix + "X" + this.LowerRightX + "Y" + this.LowerRightY;
                    if (FPGA.FPGA.Instance.Contains(t2Identifier))
                    {
                        t2Found = true;
                        break;
                    }
                    // TODO t2Identifier for slices
                }
                validIdentifierFound = t1Found && t2Found;
            }
            else
            {
                for (int i = 0; i < this.UpperLeftIdentifierPrefices.Count; i++)
                {
                    t1Identifier = this.UpperLeftIdentifierPrefices[i] + "X" + this.UpperLeftX + "Y" + this.UpperLeftY;
                    t2Identifier = this.LowerRightIdentifierPrefices[i] + "X" + this.LowerRightX + "Y" + this.LowerRightY;

                    if (FPGA.FPGA.Instance.Contains(t1Identifier) && FPGA.FPGA.Instance.Contains(t2Identifier))
                    {
                        validIdentifierFound = true;
                        break;
                    }
                }
            }

            if (!validIdentifierFound)
            {
                throw new ArgumentException("Could not any of the given identifie prefices");
            }

            Tile t1 = FPGA.FPGA.Instance.GetTile(t1Identifier);
            Tile t2 = FPGA.FPGA.Instance.GetTile(t2Identifier);

            int upperLeftX = Math.Min(t1.TileKey.X, t2.TileKey.X);
            int lowerRightX = Math.Max(t1.TileKey.X, t2.TileKey.X);
            int upperLeftY = Math.Min(t1.TileKey.Y, t2.TileKey.Y);
            int lowerRightY = Math.Max(t1.TileKey.Y, t2.TileKey.Y);
            
            for (int x = upperLeftX; x <= lowerRightX; x++)
            {
                for (int y = upperLeftY; y <= lowerRightY; y++)
                {
                    // click done out of fpga range
                    if (!FPGA.FPGA.Instance.Contains(x, y))
                        continue;

                    TileKey key = new TileKey(x, y);

                    //deselect or add the selected tile 
                    if (FPGA.TileSelectionManager.Instance.IsSelected(x, y))
                        FPGA.TileSelectionManager.Instance.RemoveFromSelection(key, false);
                    else
                        FPGA.TileSelectionManager.Instance.AddToSelection(key, false);
                }
            }

            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }
        
        public override void Undo()
        {
        }

        // TOOD default value ("INT_BRAM_, INT_DSP_, INT_");

        [Parameter(Comment = "The possible prefices of the tile identifier that will be composed the X and Y values")]
        public List<String> UpperLeftIdentifierPrefices = new List<String>();
        [Parameter(Comment = "The possible prefices of the tile identifier that will be composed the X and Y values")]
        public List<String> LowerRightIdentifierPrefices = new List<String>();
        [Parameter(Comment = "The X value of the upper left tile identifier (the X value from e.g INT_X10Y24")]
        public int UpperLeftX = 10;
        [Parameter(Comment = "The Y value of the upper left tile identifier (the Y value from e.g INT_X10Y24")]
        public int UpperLeftY = 24;
        [Parameter(Comment = "The X value of the lower right tile identifier (the X value from e.g INT_X19Y44")]
        public int LowerRightX = 19;
        [Parameter(Comment = "The Y value of the lower right tile identifier (the Y value from e.g INT_X19Y44")]
        public int LowerRightY = 44;
    }
}
