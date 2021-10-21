using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.FPGA
{
    class TileWire
    {
        public TileWire() { }
        public TileWire(Tile fromTile, Tile toTile, Wire wire)
        {
            FromTile = fromTile;
            ToTile = toTile;
            Wire = wire;
        }
        public Tile FromTile
        {
            get; set;
        }
        public Tile ToTile
        {
            get; set;
        }
        public Wire Wire
        {
            get; set;
        }
        public static int CompareByDistance(TileWire tw1, TileWire tw2)
        {
            double dist1 = Math.Sqrt(Math.Pow(tw1.Wire.XIncr, 2) + Math.Pow(tw1.Wire.YIncr, 2));
            double dist2 = Math.Sqrt(Math.Pow(tw2.Wire.XIncr, 2) + Math.Pow(tw2.Wire.YIncr, 2));

            return dist1.CompareTo(dist2);
        }
    }
}
