using System;

namespace GoAhead.Code.XDL
{
    [Serializable]
    public class XDLPip
    {
        public XDLPip(string location, string from, string connectionOperator, string to)
        {
            m_location = location;
            m_from = from;
            m_to = to;
            m_connectionOperator = connectionOperator;

            /*
            this.m_locationKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(location);
            this.m_fromKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(from);
            this.m_connectionOperatorKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(connectionOperator);
            this.m_toKey = FPGA.FPGA.Instance.IdentifierListLookup.GetKey(to);
            this.m_comment = comment;
             * */

            //Tile t = FPGA.FPGA.Instance.GetTile(location);
            //this.m_tileKey = t.TileKey;
        }

        /*
        public TileKey TileKey
        {
            get { return this.m_tileKey; }
        }
        */

        public string Location
        {
            get { return m_location; }
            //get { return FPGA.FPGA.Instance.IdentifierListLookup.GetIdentifier(this.m_locationKey); }
        }

        public string From
        {
            get { return m_from; }
            //get { return FPGA.FPGA.Instance.IdentifierListLookup.GetIdentifier(this.m_fromKey); }
        }

        public string To
        {
            get { return m_to; }
            //get { return FPGA.FPGA.Instance.IdentifierListLookup.GetIdentifier(this.m_toKey); }
        }

        public uint FromKey
        {
            get { return FPGA.FPGA.Instance.IdentifierListLookup.GetKey(m_from); }
        }

        public uint ToKey
        {
            get { return FPGA.FPGA.Instance.IdentifierListLookup.GetKey(m_to); }
        }

        /// <summary>
        /// The mux operator that connects the left hand with the right hand pip (either -> or -=)
        /// </summary>
        public string Operator
        {
            get { return m_connectionOperator; }
            //get { return FPGA.FPGA.Instance.IdentifierListLookup.GetIdentifier(this.m_connectionOperatorKey); }
        }

        public override bool Equals(object obj)
        {
            if (obj is XDLPip)
            {
                XDLPip other = (XDLPip)obj;
                if (other.FromKey != FromKey || other.ToKey != ToKey)
                {
                    return false;
                }
                return other.Location.Equals(Location);
                /*
                return
    other.Location.Equals(this.Location) &&
    other.From.Equals(this.From) &&
    other.To.Equals(this.To);*/
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            if (Location.StartsWith("#"))
            {
                // pseudo pip serves as comment
                return Location;
            }
            else
            {
                return "pip " + Location + " " + From + " " + Operator + " " + To + ",";
            }
        }

        private readonly string m_location;
        private readonly string m_from;
        private readonly string m_to;
        private readonly string m_connectionOperator;
        /*
                private readonly ushort m_locationKey;
                private readonly ushort m_fromKey;
                private readonly ushort m_toKey;
                private readonly ushort m_connectionOperatorKey;*/
    }
}