using System;

namespace GoAhead.Code.XDL.ResourceDescription
{
    public class XDLResourceReportParser
    {
        public static void Parse(string line)
        {
            //expect (xdl_resource_report v0.2 xc5vlx30ff324-3 virtex5
            string[] atoms = line.Split(' ');

            if (atoms.Length != 4)
                throw new ArgumentException("Unexpected xdl_resource_report: " + line);

            FPGA.FPGA.Instance.DeviceName = atoms[2];
            string family = atoms[3];

            SetFPGAFamily(family);
        }

        private static void SetFPGAFamily(string family)
        {
            if (family.Equals("virtex2"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Virtex2;
            }
            else if (family.Equals("virtex4"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Virtex4;
            }
            else if (family.Equals("virtex5"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Virtex5;
            }
            else if (family.StartsWith("virtex6"))//atoms[3].Equals("virtex6l"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Virtex6;
            }
            else if (family.StartsWith("kintex7"))//atoms[3].Equals("kintex7l"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Kintex7;
            }
            else if (family.EndsWith("artix7")) // EndsWith to get aartix7!
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Artix7;
            }
            else if (family.StartsWith("spartan3"))//atoms[3].Equals("spartan3l"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Spartan3;
            }
            else if (family.StartsWith("spartan6"))//atoms[3].Equals("spartan6l"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Spartan6;
            }
            else if (family.StartsWith("zynq"))
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Zynq;
            }
            else
            {
                FPGA.FPGA.Instance.Family = FPGA.FPGATypes.FPGAFamily.Spartan6;
            }
            //else
            //{
            //    throw new ArgumentException("Unexpected family found: " + atoms[3]);
            //}
        }
    }
}