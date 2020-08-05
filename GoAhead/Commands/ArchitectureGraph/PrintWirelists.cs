using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintWirelists : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Local Pip,Target Pip,LocalPipIsDriver,RelativeX,RelativeY,Primitive Number\n");

            foreach (WireList wl in MiniWirelists.Values)
            {
                OutputManager.WriteOutput("Wirelist hashcode: " + wl.Key);
                foreach (PrimitiveWire w in wl)
                {
                    OutputManager.WriteOutput(string.Join(",", PortMappings[w.LocalPip], PortMappings[w.PipOnOtherTile], w.LocalPipIsDriver, w.XIncr, w.YIncr, w.primitiveNumber));
                }

                OutputManager.WriteOutput("\n");
            }
        }
        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Mini Wirelists")]
        public Dictionary<int, WireList> MiniWirelists = null;

        [Parameter(Comment = "Port Mappings")]
        public Dictionary<string, int> PortMappings = null;
    }
}
