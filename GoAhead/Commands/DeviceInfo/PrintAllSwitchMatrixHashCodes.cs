using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintAllSwitchMatrixHashCodes : Command
    {
       protected override void DoCommandAction()
        {
            List<int> hashes = new List<int>();

            foreach (SwitchMatrix sm in FPGA.FPGA.Instance.GetAllSwitchMatrices())
            {
                hashes.Add(sm.HashCode);
            }
            hashes.Sort();

            OutputManager.WriteOutput("The FPGA contains " + FPGA.FPGA.Instance.SwitchMatrixCount + " Switchmatrices with the following hashcodes");
            foreach (int hash in hashes)
            {
                OutputManager.WriteOutput(hash.ToString());
            }
        }

        public override void Undo()
        {
        }
    }
}
