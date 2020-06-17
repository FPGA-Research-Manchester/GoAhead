using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.GridStyle
{
    [CommandDescription(Description = "Expands the current selection to the given directions.")]
    class ExpandSelectionInGivenDirections : Command
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        protected override void DoCommandAction()
        {
            List<Direction> dirs = new List<Direction>();

            // parse List<String> to List<Direction>
            foreach(String s in this.Directions)
            {
                dirs.Add((Direction) Enum.Parse(typeof(Direction), s, true));
            }

            ExpandSelection(dirs);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private void ExpandSelection(List<Direction> dirs)
        {
            bool startInUserSelection = this.StartInUserSelection();

            List<Tile> tiles = new List<Tile>();

            // add expanded tiles to the list
            foreach (Tile tile in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                foreach (Direction dir in dirs)
                {
                    tiles.AddRange(ExpandTile(tile, dir, startInUserSelection));
                }
            }

            // add expansion to current selection
            foreach (Tile t in tiles)
            {
                if (!FPGA.TileSelectionManager.Instance.IsSelected(t.TileKey))
                {
                    FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                }
            }

            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }

        private List<Tile> ExpandTile(Tile start, Direction dir, bool startInUserSelection)
        {
            List<Tile> tiles = new List<Tile>();

            int x = start.TileKey.X;
            int y = start.TileKey.Y;

            while (FPGA.FPGA.Instance.Contains(x, y))
            {
                Tile other = FPGA.FPGA.Instance.GetTile(x, y);

                if ((FPGA.TileSelectionManager.Instance.IsUserSelected(other.TileKey, this.UserSelectionType) && startInUserSelection) ||
                    (!FPGA.TileSelectionManager.Instance.IsUserSelected(other.TileKey, this.UserSelectionType) && !startInUserSelection))
                {
                    tiles.Add(other);
                }
                else 
                {
                    break;
                }

                // coordinate system starts at left top
                switch(dir)
                {
                    case Direction.Left:
                        x += -1;
                        break;
                    case Direction.Right:
                        x += 1;
                        break;
                    case Direction.Up:
                        y += -1;
                        break;
                    case Direction.Down:
                        y += 1;
                        break;
                    default:
                        break;
                }
            }

            return tiles;
        }

        private bool StartInUserSelection()
        {
            if (FPGA.TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                throw new ArgumentException("No tiles selected.");
            }

            // the selection should be either all outside or all inside the user selection
            bool allUserSelected = FPGA.TileSelectionManager.Instance.GetSelectedTiles().All(t => FPGA.TileSelectionManager.Instance.IsUserSelected(t.TileKey, this.UserSelectionType));
            bool noneUserSelected = FPGA.TileSelectionManager.Instance.GetSelectedTiles().All(t => !FPGA.TileSelectionManager.Instance.IsUserSelected(t.TileKey, this.UserSelectionType));

            if (!allUserSelected && !noneUserSelected)
            {
                throw new ArgumentException("The selection should be either all outside or all inside the user selection.");
            }

            return allUserSelected;
        }

        [Parameter(Comment = "The directions to expand.")]
        public List<String> Directions = new List<String>();

        [Parameter(Comment = "The name of the user selection that limits the extent of the selection expanding")]
        public String UserSelectionType = "PartialArea";
    }
}
