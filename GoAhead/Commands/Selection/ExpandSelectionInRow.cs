using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    abstract class ExpandSelectionInRow : Command
    {
        protected void ExpandSelection(Tile start, int increment, bool startInUserSelection)
        {
            int x = start.TileKey.X;
            int y = start.TileKey.Y;

            // get boundaries
            Tile ll = FPGA.TileSelectionManager.Instance.GetUserSelectedTile(".*", this.UserSelectionType, FPGATypes.Placement.LowerLeft);
            Tile lr = FPGA.TileSelectionManager.Instance.GetUserSelectedTile(".*", this.UserSelectionType, FPGATypes.Placement.LowerRight);


            while (FPGA.FPGA.Instance.Contains(x, y))
            {
                Tile other = FPGA.FPGA.Instance.GetTile(x, y);

                bool tileStillInUserSelection = FPGA.TileSelectionManager.Instance.IsUserSelected(other.TileKey, this.UserSelectionType) && startInUserSelection;
                bool tileStillOutUserSelection = !FPGA.TileSelectionManager.Instance.IsUserSelected(other.TileKey, this.UserSelectionType) && !startInUserSelection;

                if (tileStillInUserSelection || tileStillOutUserSelection)
                {
                    this.m_expansion.Add(other);
                }

                bool userSelectionBorderCrossed =
                   !FPGA.TileSelectionManager.Instance.IsUserSelected(other.TileKey, this.UserSelectionType) && startInUserSelection ||
                    FPGA.TileSelectionManager.Instance.IsUserSelected(other.TileKey, this.UserSelectionType) && !startInUserSelection;

                if (userSelectionBorderCrossed)
                {
                    break;
                }

                x += increment;
            }

            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }

        protected bool StartInUserSelection()
        {
            int selectTiles = FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles;
            if (selectTiles == 0)
            {
                throw new ArgumentException("No tiles selected");
            }

            bool allUserSelected = FPGA.TileSelectionManager.Instance.GetSelectedTiles().All(t => FPGA.TileSelectionManager.Instance.IsUserSelected(t.TileKey, this.UserSelectionType));
            bool noneUserSelected = FPGA.TileSelectionManager.Instance.GetSelectedTiles().All(t => !FPGA.TileSelectionManager.Instance.IsUserSelected(t.TileKey, this.UserSelectionType));

            if (!allUserSelected && !noneUserSelected)
            {
                throw new ArgumentException("Ensure that either all or none tile are user selected");
            }

            return allUserSelected;
        }


        protected void AddTiles()
        {
            foreach (Tile t in this.m_expansion)
            {
                if (!FPGA.TileSelectionManager.Instance.IsSelected(t.TileKey))
                {
                    FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                }
            }
            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }


        protected List<Tile> m_expansion = new List<Tile>();

        [Parameter(Comment = "The name of the user selection that limits the extent of the selection expanding")]
        public String UserSelectionType = "PartialArea";
    }
}
