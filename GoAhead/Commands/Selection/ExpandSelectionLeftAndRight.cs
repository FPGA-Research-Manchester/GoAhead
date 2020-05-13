using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    class ExpandSelectionLeftAndRight : ExpandSelectionInRow
    {
        protected override void DoCommandAction()
        {
            bool startInUserSelection = StartInUserSelection();

            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                ExpandSelection(t, -1, startInUserSelection);
                ExpandSelection(t, 1, startInUserSelection);
            }

            AddTiles();

            TileSelectionManager.Instance.SelectionChanged();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
