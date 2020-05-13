using System;
using System.Collections.Generic;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Invert the current selection", Wrapper = false)]
    class InvertSelection : AddToSelectionCommand
    {
       protected override void DoCommandAction()
        {
            Invert();
        }

        public override void Undo()
        {
            Invert();
        }

        private void Invert()
        {
            TileSet inverseSelection = new TileSet();

            foreach (Tile nextTile in FPGA.FPGA.Instance.GetAllTiles())
            {
                if (!TileSelectionManager.Instance.IsSelected(nextTile.TileKey))
                {
                    inverseSelection.Add(nextTile);
                }
            }

            TileSelectionManager.Instance.ClearSelection();

            foreach (Tile t in inverseSelection)
            {
                TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
            }

            TileSelectionManager.Instance.SelectionChanged();
        }
    }
}
