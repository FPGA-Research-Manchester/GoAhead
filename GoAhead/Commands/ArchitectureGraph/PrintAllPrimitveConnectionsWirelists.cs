using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllPrimitveConnectionsWirelists : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Local Pip,Target Pip,LocalPipIsDriver,PrimitiveNumber,Primitive Wirelist Hashcode\n");

            foreach (WireList wl in PrimitiveWirelists.Values)
            {
                foreach (PrimitiveWire w in wl)
                    OutputManager.WriteOutput(string.Join(",", w.LocalPip, w.PipOnOtherTile, w.LocalPipIsDriver, w.primitiveNumber, wl.Key));

                OutputManager.WriteOutput("\n");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Primitive Connections Wirelists")]
        public Dictionary<int, WireList> PrimitiveWirelists = null;
    }
}
