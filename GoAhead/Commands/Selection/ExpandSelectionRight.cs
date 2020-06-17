using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    class ExpandSelectionRight : ExpandSelectionInRow
    {
        protected override void DoCommandAction()
        {
            bool startInUserSelection = this.StartInUserSelection();

            foreach (Tile t in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                this.ExpandSelection(t, 1, startInUserSelection);
            }

            this.AddTiles();

            FPGA.TileSelectionManager.Instance.SelectionChanged();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
