using System;

namespace GoAhead.FPGA.Slices
{
    [Serializable]
    public class V2Slice : Slice
    {
        public V2Slice(Tile containingTile, string name, string type)
            : base(containingTile, name, type)
        {
        }

        public override void InitAttributes()
        {
        }
    }
}