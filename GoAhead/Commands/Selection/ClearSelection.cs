using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Commands.Selection
{
    class ClearSelection : Command
    {
       protected override void DoCommandAction()
        {
            FPGA.TileSelectionManager.Instance.ClearSelection();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
