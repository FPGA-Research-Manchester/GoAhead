using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.BlockingShared
{
    class FreeAllRoutingResources : Command
    {
       protected override void DoCommandAction()
        {
            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles())
            {
                t.UnblockAllPorts();
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
