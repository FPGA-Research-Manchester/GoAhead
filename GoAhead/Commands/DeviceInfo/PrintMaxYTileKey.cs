using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintMaxYTileKey : Command
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput(FPGA.FPGA.Instance.MaxY.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
