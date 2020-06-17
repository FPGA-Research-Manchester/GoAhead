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
            foreach(string s in Directions)
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
            bool startInUserSelection = StartInUserSelection();

            List<Tile> tiles = new List<Tile>();

            // add expanded tiles to the list
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles())
            {
                foreach (Direction dir in dirs)
                {
                    tiles.AddRange(ExpandTile(tile, dir, startInUserSelection));
                }
            }

            // add expansion to current selection
            foreach (Tile t in tiles)
            {
                if (!TileSelectionManager.Instance.IsSelected(t.TileKey))
                {
                    TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
                }
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        private List<Tile> ExpandTile(Tile start, Direction dir, bool startInUserSelection)
        {
            List<Tile> tiles = new List<Tile>();

            int x = start.TileKey.X;
            int y = start.TileKey.Y;

            while (FPGA.FPGA.Instance.Contains(x, y))
            {
                Tile other = FPGA.FPGA.Instance.GetTile(x, y);

                if ((TileSelectionManager.Instance.IsUserSelected(other.TileKey, UserSelectionType) && startInUserSelection) ||
                    (!TileSelectionManager.Instance.IsUserSelected(other.TileKey, UserSelectionType) && !startInUserSelection))
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
            if (TileSelectionManager.Instance.NumberOfSelectedTiles == 0)
            {
                throw new ArgumentException("No tiles selected.");
            }

            // the selection should be either all outside or all inside the user selection
            bool allUserSelected = TileSelectionManager.Instance.GetSelectedTiles().All(t => TileSelectionManager.Instance.IsUserSelected(t.TileKey, UserSelectionType));
            bool noneUserSelected = TileSelectionManager.Instance.GetSelectedTiles().All(t => !TileSelectionManager.Instance.IsUserSelected(t.TileKey, UserSelectionType));

            if (!allUserSelected && !noneUserSelected)
            {
                throw new ArgumentException("The selection should be either all outside or all inside the user selection.");
            }

            return allUserSelected;
        }

        [Parameter(Comment = "The directions to expand.")]
        public List<string> Directions = new List<string>();

        [Parameter(Comment = "The name of the user selection that limits the extent of the selection expanding")]
        public string UserSelectionType = "PartialArea";
    }
}
