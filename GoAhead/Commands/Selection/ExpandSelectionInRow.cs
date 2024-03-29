﻿using System;
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
            Tile ll = TileSelectionManager.Instance.GetUserSelectedTile(".*", UserSelectionType, FPGATypes.Placement.LowerLeft);
            Tile lr = TileSelectionManager.Instance.GetUserSelectedTile(".*", UserSelectionType, FPGATypes.Placement.LowerRight);


            while (FPGA.FPGA.Instance.Contains(x, y))
            {
                Tile other = FPGA.FPGA.Instance.GetTile(x, y);

                bool tileStillInUserSelection = TileSelectionManager.Instance.IsUserSelected(other.TileKey, UserSelectionType) && startInUserSelection;
                bool tileStillOutUserSelection = !TileSelectionManager.Instance.IsUserSelected(other.TileKey, UserSelectionType) && !startInUserSelection;

                if (tileStillInUserSelection || tileStillOutUserSelection)
                {
                    m_expansion.Add(other);
                }

                bool userSelectionBorderCrossed =
                   !TileSelectionManager.Instance.IsUserSelected(other.TileKey, UserSelectionType) && startInUserSelection ||
                    TileSelectionManager.Instance.IsUserSelected(other.TileKey, UserSelectionType) && !startInUserSelection;

                if (userSelectionBorderCrossed)
                {
                    break;
                }

                x += increment;
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        protected bool StartInUserSelection()
        {
            int selectTiles = TileSelectionManager.Instance.NumberOfSelectedTiles;
            if (selectTiles == 0)
            {
                throw new ArgumentException("No tiles selected");
            }

            bool allUserSelected = TileSelectionManager.Instance.GetSelectedTiles().All(t => TileSelectionManager.Instance.IsUserSelected(t.TileKey, UserSelectionType));
            bool noneUserSelected = TileSelectionManager.Instance.GetSelectedTiles().All(t => !TileSelectionManager.Instance.IsUserSelected(t.TileKey, UserSelectionType));

            if (!allUserSelected && !noneUserSelected)
            {
                throw new ArgumentException("Ensure that either all or none tile are user selected");
            }

            return allUserSelected;
        }


        protected void AddTiles()
        {
            foreach (Tile t in m_expansion)
            {
                if (!TileSelectionManager.Instance.IsSelected(t.TileKey))
                {
                    TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                }
            }
            TileSelectionManager.Instance.SelectionChanged();
        }


        protected List<Tile> m_expansion = new List<Tile>();

        [Parameter(Comment = "The name of the user selection that limits the extent of the selection expanding")]
        public string UserSelectionType = "PartialArea";
    }
}
