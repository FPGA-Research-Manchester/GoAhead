using System;

namespace GoAhead.FPGA
{
    /// <summary>
    /// The base class for all Elements on FPGAs
    /// </summary>
    [Serializable()]
    public abstract class FPGAElement : IComparable
    {
        /// <summary>
        /// All FPGAElements can be distiguished by their String representation
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            return string.CompareOrdinal(ToString(), obj.ToString());
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            if (!m_hashCodeValid)
            {
                m_hashCode = ToString().GetHashCode();
                m_hashCodeValid = true;
            }
            return m_hashCode;
        }

        public abstract override string ToString();

        private int m_hashCode = 0;
        private bool m_hashCodeValid = false;
    }
}