using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    class PrintPartitionPinConstraintsForSelection : PrintPartitionPinConstraintsForTile
    {
        private const string MODE_ROW_WISE = "row-wise";
        private const string MODE_COLUMN_WISE = "column-wise";

        private const string HORIZONTAL_LEFT_TO_RIGHT = "left-to-right";
        private const string HORIZONTAL_RIGHT_TO_LEFT = "right-to-left";

        private const string VERTICAL_TOP_DOWN = "top-down";
        private const string VERTICAL_BOTTOM_UP = "bottom-up";

        protected override void DoCommandAction()
        {
            this.CheckParameters();

            List<TileKey> intTiles = new List<TileKey>();

            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles().Where(tile => 
                     IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                intTiles.Add(t.TileKey);
            }

            var modeClusters =
                from key in intTiles
                group key by this.IsRowWise() ? key.Y : key.X into cluster
                select cluster;

            List<Tile> tilesInFinalOrder = new List<Tile>();

            // order tiles
            if (this.IsRowWise())
            {
                foreach (IGrouping<int, TileKey> group in (this.IsTopDown() ? modeClusters.OrderBy(g => g.Key) : modeClusters.OrderByDescending(g => g.Key)))
                {
                    foreach (TileKey key in (this.IsLeftToRight() ? group.OrderBy(k => k.X) : group.OrderBy(k => k.X).Reverse()))
                    {
                        tilesInFinalOrder.Add(FPGA.FPGA.Instance.GetTile(key));
                    }
                }
            }
            else
            {
                foreach (IGrouping<int, TileKey> group in (this.IsLeftToRight() ? modeClusters.OrderBy(g => g.Key) : modeClusters.OrderByDescending(g => g.Key)))
                {
                    foreach (TileKey key in (this.IsTopDown() ? group.OrderBy(k => k.Y) : group.OrderBy(k => k.Y).Reverse()))
                    {
                        tilesInFinalOrder.Add(FPGA.FPGA.Instance.GetTile(key));
                    }
                }
            }

            int startIndex = this.IndexOffset;

            foreach (Tile t in tilesInFinalOrder)
            {
                if(startIndex >= this.NumberOfSignals)
                {
                    break;
                }

                this.SwitchboxName = t.Location;
                this.StartIndex = startIndex;

                base.DoCommandAction();

                startIndex += SIGNALS_PER_TILE;
            }
        }

        private bool IsRowWise()
        {
            return this.Mode.Equals(MODE_ROW_WISE);
        }

        private bool IsColumnWise()
        {
            return this.Mode.Equals(MODE_COLUMN_WISE);
        }

        private bool IsLeftToRight()
        {
            return this.Horizontal.Equals(HORIZONTAL_LEFT_TO_RIGHT);
        }
		
        private bool IsRightToLeft()
        {
            return this.Horizontal.Equals(HORIZONTAL_RIGHT_TO_LEFT);
        }
		
        private bool IsTopDown()
        {
            return this.Vertical.Equals(VERTICAL_TOP_DOWN);
        }
		
        private bool IsBottomUp()
        {
            return this.Vertical.Equals(VERTICAL_BOTTOM_UP);
        }

        private void CheckParameters()
        {
            bool modeIsCorrect = this.IsRowWise() || this.IsColumnWise();
            bool horizontalIsCorrect = this.IsLeftToRight() || this.IsRightToLeft();
            bool verticalIsCorrect = this.IsTopDown() || this.IsBottomUp();
            bool indexOffsetIsCorrect = this.IndexOffset >= 0;

            if(!modeIsCorrect || !horizontalIsCorrect || !verticalIsCorrect || !indexOffsetIsCorrect)
            {
                throw new ArgumentException("Unexpected format in parameter Mode, Horizontal or/and Vertical.");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The number of signals. Should be a modulus of 4.")]
        public int NumberOfSignals = 128;

        [Parameter(Comment = "Either row-wise or column-wise")]
        public String Mode = MODE_ROW_WISE;

        [Parameter(Comment = "Either left-to-right or right-to-left")]
        public String Horizontal = HORIZONTAL_LEFT_TO_RIGHT;

        [Parameter(Comment = "Either top-down or bottom-up")]
        public String Vertical = VERTICAL_TOP_DOWN;

        [Parameter(Comment = "Either top-down or bottom-up")]
        public int IndexOffset = 0;
    }
}
