using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Expand the current selection by including all unselected tiles surrounded by selected tiles (the current selection should be a rectangle)", Wrapper = false)]
    class ExpandSelectionSpecialTiles : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {
            Tile ul = FPGA.TileSelectionManager.Instance.GetSelectedTile(".*", FPGATypes.Placement.UpperLeft);
            Tile lr = FPGA.TileSelectionManager.Instance.GetSelectedTile(".*", FPGATypes.Placement.LowerRight);

            for (int x = ul.TileKey.X; x <= lr.TileKey.X; x++)
            {
                for (int y = ul.TileKey.Y; y <= lr.TileKey.Y; y++)
                {
                    TileKey key = new TileKey(x, y);

                    // click done out of fpga range
                    if (!FPGA.FPGA.Instance.Contains(key))
                    {
                        continue;
                    }
                    // add if not added already
                    if (!FPGA.TileSelectionManager.Instance.IsSelected(key))
                    {
                        FPGA.TileSelectionManager.Instance.AddToSelection(key, false);
                    }
                }
            }

            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
