using System;
using System.ComponentModel;
using GoAhead.FPGA;

namespace GoAhead.Code
{
    [Serializable]
    public abstract class NetPin
    {
        public static NetPin Copy(NetPin other)
        {
            NetPin result = null;
            if (other is NetInpin)
            {
                result = new NetInpin();
            }
            else if (other is NetOutpin)
            {
                result = new NetOutpin();
            }
            else
            {
                throw new ArgumentException("Unexpected type in NetPin.Copy()");
            }

            //result.m_instanceNameKey = other.m_instanceNameKey;
            result.InstanceName = other.InstanceName;
            //result.m_slicePortKey = other.m_slicePortKey;
            result.SlicePort = other.SlicePort;
            result.TileName = other.TileName;
            result.SliceName = other.SliceName;
            result.Code = other.Code;
            return result;
        }

        public static NetPin CreateNetpin(string direction)
        {
            // XDL->outpin, TCL->out
            // XDL->inpin, TCL->in
            if (direction.ToLower().StartsWith("out"))
            {
                return new NetOutpin();
            }
            else if (direction.ToLower().StartsWith("in"))
            {
                return new NetInpin();
            }
            else
            {
                throw new ArgumentException("Unexpected direction in CreateNetpin " + direction);
            }
        }

        public static string GetSlicePortString(string portName)
        {
            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex4))
            {
                string[] atoms = portName.Split('_');
                return atoms[0];
            }
            else
            {
                string[] atoms = portName.Split('_');
                if (atoms.Length == 3)
                {
                    return atoms[2];
                }
                else if (atoms.Length == 2)
                {
                    return atoms[1];
                }
                else
                {
                    throw new ArgumentException("Could not split " + portName);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is NetPin))
            {
                return false;
            }

            NetPin other = (NetPin)obj;

            return
                InstanceName == other.InstanceName &&
                SlicePort == other.SlicePort &&
                GetDirection().Equals(other.GetDirection());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return GetDirection() + " \"" + InstanceName + "\" " + SlicePort + ", ";
        }

        public abstract string GetDirection();
        
        public string TileName
        {
            get { return m_tileName; }
            set { m_tileName = value; }
        }

        public string SliceName
        {
            get { return m_sliceName; }
            set { m_sliceName = value; }
        }



        public string Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        private string m_tileName = null;
        private string m_sliceName = null;
        private string m_code = null;

        /// <summary>
        /// The name of instance (slice) in which the ports resides
        /// </summary>
        [DefaultValue("undefined slice")]
        public string InstanceName { get; set; }

        [DefaultValue("undefined slice port")]
        public string SlicePort { get; set; }
    }
}