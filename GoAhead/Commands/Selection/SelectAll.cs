using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description="Select all tiles")]
    class SelectAll : AddToSelectionCommand
    {
       protected override void DoCommandAction()
        {
            TileSelectionManager.Instance.ClearSelection();
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles())
            {
                TileSelectionManager.Instance.AddToSelection(tile.TileKey, false);
            }

            TileSelectionManager.Instance.SelectionChanged();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
