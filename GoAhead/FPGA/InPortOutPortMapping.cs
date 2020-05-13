using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.FPGA
{
    [Serializable]
    public class InPortOutPortMapping
    {
        public void AddSlicePort(Port port, FPGATypes.PortDirection direction)
        {
            if (direction.Equals(FPGATypes.PortDirection.In))
            {
                m_InPorts.Add(port);
            }
            else
            {
                m_OutPorts.Add(port);
            }
        }

        public void Add(string sliceElementName, Port p)
        {
            if (!m_sliceElementPorts.ContainsKey(sliceElementName))
            {
                m_sliceElementPorts.Add(sliceElementName, new List<Port>());
            }
            m_sliceElementPorts[sliceElementName].Add(p);
        }

        public bool Contains(Port port)
        {
            return m_InPorts.Contains(port) || m_OutPorts.Contains(port);
        }

        public bool IsSliceOutPort(Port port)
        {
            return m_OutPorts.Contains(port);
        }

        /// <summary>
        /// Return whether this slice contains an outport that ends with the given suffix
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public bool IsSliceOutPort(string suffix)
        {
            foreach (Port p in m_OutPorts)
            {
                if (p.Name.EndsWith(suffix))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return whether this slice contains an inport that ends with the given suffix
        /// </summary>
        /// <param name="suffix"></param>   
        /// <returns></returns>
        public bool IsSliceInPort(string suffix)
        {
            foreach (Port p in m_InPorts)
            {
                if (p.Name.EndsWith(suffix))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSliceInPort(Port port)
        {
            return m_InPorts.Contains(port);
        }

        public IEnumerable<Port> Ports
        {
            get
            {
                foreach (Port i in m_InPorts)
                {
                    yield return i;
                }
                foreach (Port o in m_OutPorts)
                {
                    yield return o;
                }
            }
        }

        public IEnumerable<Port> GetPorts(FPGATypes.PortDirection direction)
        {
            if (direction == FPGATypes.PortDirection.In)
            {
                foreach (Port p in m_InPorts)
                {
                    yield return p;
                }
            }
            else if (direction == FPGATypes.PortDirection.Out)
            {
                foreach (Port p in m_OutPorts)
                {
                    yield return p;
                }
            }
            else
            {
                throw new ArgumentException("Unknown direction found in PortMapping.GetAllPorts: " + direction);
            }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (Port p in m_InPorts)
            {
                buffer.AppendLine(p + " " + FPGATypes.PortDirection.In);
            }
            foreach (Port p in m_OutPorts)
            {
                buffer.AppendLine(p + " " + FPGATypes.PortDirection.In);
            }
            // vivado only
            if (m_sliceElementPorts != null)
            {
                foreach (KeyValuePair<string, List<Port>> t in m_sliceElementPorts)
                {
                    buffer.AppendLine(t.Key + " " + string.Join(" ", t.Value));
                }
            }
            return buffer.ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public IEnumerable<string> SliceElements
        {
            get { return m_sliceElementPorts.Keys; }
        }

        public IEnumerable<Port> GetPorts(string sliceElementName)
        {
            if (!m_sliceElementPorts.ContainsKey(sliceElementName))
            {
                throw new ArgumentException("Cannot access port for slice element " + sliceElementName);
            }
            return m_sliceElementPorts[sliceElementName];
        }

        protected List<Port> m_InPorts = new List<Port>();
        protected List<Port> m_OutPorts = new List<Port>();
        protected Dictionary<string, List<Port>> m_sliceElementPorts = new Dictionary<string, List<Port>>();
    }
}