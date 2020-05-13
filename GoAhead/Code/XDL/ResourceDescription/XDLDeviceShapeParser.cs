using System;

namespace GoAhead.Code.XDL.ResourceDescription
{
    public class XDLDeviceShapeParser
    {
        public static void Parse(string line)
        {
            string[] atoms = line.Split(' ');
            int x = int.Parse(atoms[1]);
            int y = int.Parse(atoms[2]);

            FPGA.FPGA.Instance.NumberOfExpectedTiles = x * y;
        }
    }
}