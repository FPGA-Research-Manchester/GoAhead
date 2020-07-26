using GoAhead.Commands.Variables;
using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintVCCPipsInAllInterconnects : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Port Driven By VCC_Wire Port,Switch matrix hashcode\n");
            
            HashSet<int> printedSwitchMatrices = new HashSet<int>();

            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                // filter out interconnects
                if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect) && IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    continue;

                if (printedSwitchMatrices.Contains(t.SwitchMatrixHashCode))
                    continue;

                foreach (Port p in t.SwitchMatrix.GetDrivenPorts(new Port("VCC_WIRE")))
                {
                    OutputManager.WriteOutput(string.Join(",", p.Name, t.SwitchMatrixHashCode));
                }
                printedSwitchMatrices.Add(t.SwitchMatrixHashCode);

                OutputManager.WriteOutput("\n");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

    }
}
