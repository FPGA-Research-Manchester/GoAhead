using System;

namespace GoAhead.FPGA
{
    [Serializable]
    public class TileKey
    {
        public TileKey(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public int X
        {
            get { return m_x; }
        }

        public int Y
        {
            get { return m_y; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TileKey))
            {
                return false;
            }

            TileKey other = (TileKey)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return "X" + m_x + "Y" + m_y;
        }

        private readonly int m_x;
        private readonly int m_y;
    }
}