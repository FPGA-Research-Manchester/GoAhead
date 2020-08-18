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
            OutputManager.WriteOutput("Wirelist hashcode,Local Pip,Target Pip,LocalPipIsDriver,RelativeX,RelativeY,Primitive Number");
            StringBuilder buffer = new StringBuilder();
            foreach (WireList wl in MiniWirelists.Values)
            {
                foreach (PrimitiveWire w in wl)
                {
                    buffer.AppendLine(wl.Key + "," + string.Join(",", PortMappings[w.LocalPip], PortMappings[w.PipOnOtherTile], w.LocalPipIsDriver, w.XIncr, w.YIncr, w.primitiveNumber));
                }
            }

            OutputManager.WriteOutput(buffer.ToString());
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
