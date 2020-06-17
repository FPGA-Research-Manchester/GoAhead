using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.DeviceInfo
{
    [CommandDescription(Description="Print wire list in form EE2B0;2;0;EE2E0 for the given tile", Wrapper=false, Publish=true)]
    class PrintWireList : CommandWithFileOutput
    {
       protected override void DoCommandAction()
        {
            if (!FPGA.FPGA.Instance.Contains(this.Location))
            {
                throw new ArgumentException("Tile " + this.Location + " not found");
            }

            Tile where = FPGA.FPGA.Instance.GetTile(this.Location);

            foreach (Wire wire in where.WireList)
            {
                if ((wire.LocalPipIsDriver && this.PrintBeginPips) || (!wire.LocalPipIsDriver && this.PrintEndPips))
                {
                    String nextLine = wire.LocalPip + ";" + wire.XIncr + ";" + wire.YIncr + ";" + wire.PipOnOtherTile;
                    this.OutputManager.WriteOutput(nextLine);
                }
            }
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The tile to print, e.g. INT_X3Y143")]
        public String Location = "INT_X3Y143";

        [Parameter(Comment = "Print wires which start in the given tile")]
        public bool PrintBeginPips = true;

        [Parameter(Comment = "Print wires which end in the given tile")]
        public bool PrintEndPips = true;
    }
}
