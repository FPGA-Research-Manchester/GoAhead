using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintPortNames : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // header line
            OutputManager.WriteOutput("Port Hashcode,Port name\n");

            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<string, int> pair in PortMappings)
            {
                buffer.AppendLine(pair.Value + "," + pair.Key);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Port Names")]
        public Dictionary<string, int> PortMappings = new Dictionary<string, int>();
    }
}
