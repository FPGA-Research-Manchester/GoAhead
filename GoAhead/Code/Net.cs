using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace GoAhead.Code
{
    [Serializable]
    public abstract class Net : NetlistElement
    {
        public static Net CreateNet(string name)
        {
            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGA.FPGATypes.BackendType.ISE:
                    return new XDL.XDLNet(name);
                case FPGA.FPGATypes.BackendType.Vivado:
                    return new TCL.TCLNet(name);
                default:
                    throw new ArgumentException("Unsupported backend type " + FPGA.FPGA.Instance.BackendType);
            }
        }

        public abstract int PipCount
        {
            get;
        }

        public int OutpinCount
        {
            get { return (m_netPins == null ? 0 : m_netPins.Count(np => np is NetOutpin)); }
        }

        public int InpinCount
        {
            get { return (m_netPins == null ? 0 : m_netPins.Count(np => np is NetInpin)); }
        }

        public int NetPinCount
        {
            get { return (m_netPins == null ? 0 : m_netPins.Count); }
        }

        /// <summary>
        /// Add inpin and outpins
        /// </summary>
        /// <param name="pin"></param>
        public void Add(NetPin pin)
        {
            if (m_netPins == null)
            {
                m_netPins = new List<NetPin>();
            }
        
            m_netPins.Add(pin);
        }

        public override string ToString()
        {
            return Name;
        }

        public void ClearNetPins()
        {
            m_netPins = null;
        }

        public void Remove(Predicate<NetPin> p)
        {
            if (m_netPins != null)
            {
                m_netPins.RemoveAll(p);
            }
        }

        public virtual IEnumerable<NetPin> NetPins
        {
            get 
            {
                if (m_netPins != null)
                {
                    foreach (NetPin np in m_netPins)
                    {
                        yield return np;
                    }
                }
            }
        }

        public abstract void BlockUsedResources();

        [DefaultValue(false)]
        public bool ReadOnly { get; set; }

        [DefaultValue("undefined net name")]
        public string Name { get; set; }

        /// <summary>
        /// Inpin and Outpins
        /// </summary>
        protected List<NetPin> m_netPins = null;
    }
}