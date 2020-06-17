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
            this.Invert();
        }

        public override void Undo()
        {
            this.Invert();
        }

        private void Invert()
        {
            TileSet inverseSelection = new TileSet();

            foreach (Tile nextTile in FPGA.FPGA.Instance.GetAllTiles())
            {
                if (!FPGA.TileSelectionManager.Instance.IsSelected(nextTile.TileKey))
                {
                    inverseSelection.Add(nextTile);
                }
            }

            FPGA.TileSelectionManager.Instance.ClearSelection();

            foreach (Tile t in inverseSelection)
            {
                FPGA.TileSelectionManager.Instance.AddToSelection(t.TileKey, false);
            }

            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }
    }
}
