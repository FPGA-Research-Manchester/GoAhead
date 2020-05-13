using System;
using System.Text.RegularExpressions;

namespace GoAhead.FPGA.Slices
{
    [Serializable]
    public class USSlice : Slice
    {
        public USSlice(Tile containingTile, string name, string type)
            : base(containingTile, name, type)
        {
        }

        public override void InitAttributes()
        {
        }
    }
}