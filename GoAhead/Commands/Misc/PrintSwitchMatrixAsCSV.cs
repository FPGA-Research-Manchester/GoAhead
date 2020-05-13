using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Misc
{
    class PrintSwitchMatrixAsCSV : Command
    {
        public override void Do()
        {
            if (!Regex.IsMatch(this.StartLocation, Objects.IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.CLBRegex)))
            {
                throw new Exception("StartLocation must be a CLB");
            }

            Tile clb = FPGA.FPGA.Instance.GetTile(this.StartLocation);
            foreach (PortTriplet triplet in FPGA.FPGATypes.GetPortTriplets(clb))
            {
                String nextLine = "";
                String port1 = "";
                String port2 = "";
                String port3 = "";
                if (triplet.Port1 != null)
                {
                    port1 = triplet.Port1.Name;
                }
                if (triplet.Port2 != null)
                {
                    port2 = triplet.Port2.Name;
                }
                if (triplet.Port3 != null)
                {
                    port3 = triplet.Port3.Name;
                }
                nextLine = port1 + ";" + port2 + ";" + port3;

                this.WriteOutput(nextLine);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Paramter(Comment = "CLB location")]
        public String StartLocation = "";
    }
}
