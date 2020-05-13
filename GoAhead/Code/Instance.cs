using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using GoAhead.FPGA;

namespace GoAhead.Code
{
    [Serializable]
    public abstract class Instance : NetlistElement
    {
        public override bool Equals(object obj)
        {
            if (obj is Instance)
            {
                Instance other = (Instance)obj;
                return
                    other.Location.Equals(Location) &&
                    other.SliceName.Equals(SliceName);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            // we compare via SliceName
            return SliceName.GetHashCode();
        }

        public TileKey TileKey
        {
            get
            {
                if (m_tileKey == null)
                {
                    DeriveTileKeyAndSliceNumber();
                }

                return m_tileKey;
            }
            set { m_tileKey = value; }
        }

        public int SliceNumber
        {
            get
            {
                if (m_sliceNumber == -1)
                {
                    DeriveTileKeyAndSliceNumber();
                }
                return m_sliceNumber;
            }
            set { m_sliceNumber = value; }
        }

        public void DeriveTileKeyAndSliceNumber()
        {
            Tile tile = FPGA.FPGA.Instance.GetTile(Location);
            m_tileKey = tile.TileKey;
            // sideeffect with get!
            TileKey key = TileKey;
            Tile where = FPGA.FPGA.Instance.GetTile(Location);
            if (where.HasSlice(SliceName))
            {
                m_sliceNumber = where.GetSliceNumberByName(SliceName);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: Can not find " + SliceName + " on tile " + where.Location);
                Console.ResetColor();
                m_sliceNumber = 0;
            }
        }

        public override string ToString()
        {
            // TODO XDL needs slice configuration??
            return Name;
        }

        [DefaultValue("undefined name"), DataMember]
        public string Name { get; set; }

        [DefaultValue(-1), DataMember]
        public int InstanceIndex { get; set; }

        [DefaultValue(-1), DataMember]
        public int LocationX { get; set; }

        [DefaultValue(-1), DataMember]
        public int LocationY { get; set; }

        [DefaultValue("undefined location"), DataMember]
        public string Location { get; set; }

        [DefaultValue("undefined slice"), DataMember]
        public string SliceName { get; set; }

        [DefaultValue("undefined slice type"), DataMember]
        public string SliceType { get; set; }

        [DataMember]
        protected TileKey m_tileKey = null;

        [DataMember]
        protected int m_sliceNumber = -1;
    }
}