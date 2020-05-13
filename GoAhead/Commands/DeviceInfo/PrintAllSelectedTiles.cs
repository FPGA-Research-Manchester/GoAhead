using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintAllSelectedTiles : Command
    {
        protected override void DoCommandAction()
        {
            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                OutputManager.WriteOutput(t.Location);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
