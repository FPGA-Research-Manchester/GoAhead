using System;
using System.Runtime.Serialization;

namespace GoAhead.FPGA
{
    [Serializable, DataContract]
    public class Port
    {
        public Port(string name)
        {
            m_nameKey = FPGA.Instance.IdentifierListLookup.GetKey(name);
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name
        {
            get { return FPGA.Instance.IdentifierListLookup.GetIdentifier(m_nameKey); }
        }

        public uint NameKey
        {
            get { return m_nameKey; }
        }

        public int Cost
        {
            get { return m_cost; }
            set { m_cost = value; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Port))
            {
                return false;
            }

            Port other = (Port)obj;

            return m_nameKey == other.m_nameKey;
        }

        public override int GetHashCode()
        {
            return (int) m_nameKey;
        }

        [DataMember]
        private readonly uint m_nameKey;

        private int m_cost;
        //private readonly String m_name;
    }
}