using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllSwitchMatrixWirelists : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Local Pip,Target Pip,LocalPipIsDriver,RelativeX,RelativeY,Primitive Number\n");

            foreach (WireList wl in InterconnectWirelists.Values)
            {
                OutputManager.WriteOutput("Wirelist hashcode: " + wl.Key);
                foreach (PrimitiveWire w in wl)
                    OutputManager.WriteOutput(string.Join(",", w.LocalPip, w.PipOnOtherTile, w.LocalPipIsDriver, w.XIncr, w.YIncr, w.primitiveNumber));

                OutputManager.WriteOutput("\n");

            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Interconnect Wirelists")]
        public Dictionary<int, WireList> InterconnectWirelists = null;
    }
}
