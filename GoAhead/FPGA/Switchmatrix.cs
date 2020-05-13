using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.FPGA
{
    [Serializable]
    public class RawSwitchMatrix
    {
        public RawSwitchMatrix(SwitchMatrix switchMatrix)
        {
            foreach (Port driver in switchMatrix.GetAllDrivers())
            {
                InPorts.Add(driver.Name);
                List<string> drivenPorts = new List<string>();
                foreach (Port drivenPort in switchMatrix.GetDrivenPorts(driver))
                {
                    drivenPorts.Add(drivenPort.Name);
                }
                OutPorts.Add(drivenPorts);
            }

            HashCode = switchMatrix.HashCode;
        }

        /// <summary>
        /// The hashcode of the switch matrix. This valud should not be recalculated as this may result in different values on differnt machines
        /// </summary>
        public int HashCode = 0;

        public List<string> InPorts = new List<string>();
        public List<List<string>> OutPorts = new List<List<string>>();
    }

    [Serializable]
    public class SwitchMatrix : FPGAElement
    {
        public SwitchMatrix()
        {
        }

        public SwitchMatrix(RawSwitchMatrix rawSwitchMatrix)
        {
            for (int i = 0; i < rawSwitchMatrix.InPorts.Count; i++)
            {
                Port fromPort = new Port(rawSwitchMatrix.InPorts[i]);
                for (int j = 0; j < rawSwitchMatrix.OutPorts[i].Count; j++)
                {
                    Port toPort = new Port(rawSwitchMatrix.OutPorts[i][j]);
                    Add(fromPort, toPort);
                }
            }

            HashCode = rawSwitchMatrix.HashCode;
        }

        public void Add(Port from, Port to)
        {
            if (!m_in2out.ContainsKey(from))
            {
                m_in2out.Add(from, new Dictionary<Port, bool>());
                m_in2out[from].Add(to, false);
            }
            else if (!m_in2out[from].ContainsKey(to))
            {
                m_in2out[from].Add(to, false);
            }

            if (!m_allPortsFrom.ContainsKey(from.NameKey))
            {
                m_allPortsFrom.Add(from.NameKey, from);
            }
            if (!m_allPortsTo.ContainsKey(to.NameKey))
            {
                m_allPortsTo.Add(to.NameKey, to);
            }
        }

        public bool IsDriver(Port port)
        {
            return GetAllDrivers().Contains(port);
        }

        public Port GetPortByName(string portName)
        {
            if (Contains(portName)) return new Port(portName);

            return null;
        }

        public IEnumerable<Port> GetDrivenPorts()
        {
            Dictionary<uint, Port> returnedPorts = new Dictionary<uint, Port>();
            foreach (Port driver in m_in2out.Keys)
            {
                foreach (var drivenPort in m_in2out[driver])
                {
                    if (!returnedPorts.ContainsKey(drivenPort.Key.NameKey))
                    {
                        returnedPorts.Add(drivenPort.Key.NameKey, drivenPort.Key);
                        yield return drivenPort.Key;
                    }
                }
            }
        }

        public IEnumerable<Port> GetDrivenPorts(Port driver)
        {
            if (driver.Name.Equals("LOGIC_OUTS_W1"))
            {
            }

            if (m_in2out.ContainsKey(driver))
            {
                foreach (var drivenPort in m_in2out[driver])
                {
                    yield return drivenPort.Key;
                }
            }
        }

        public Port GetFirstDrivenPort(Port driver)
        {
            if (m_in2out.ContainsKey(driver))
            {
                foreach (var drivenPort in m_in2out[driver])
                {
                    return drivenPort.Key;
                }
            }
            throw new ArgumentException(driver.Name + " drives no port");
        }

        /// <summary>
        /// Get all ports that drive other pins
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Port> GetAllDrivers()
        {
            foreach (var incoming in m_in2out)
            {
                yield return incoming.Key;
            }
        }

        public IEnumerable<Port> GetAllDrivers(Port drivenPort)
        {
            foreach (var driverAndConnections in m_in2out)
            {
                if (driverAndConnections.Value.ContainsKey(drivenPort)) 
                {
                    yield return driverAndConnections.Key;
                }
            }
        }

        /// <summary>
        /// Get all ports that drive other pins and match the filter with the lowest connectivity first
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Port> GetAllDriversSortedAscByConnectivity(Predicate<Port> filter)
        {
            // m_sortedPorts might be null after deserialization
            if (m_sortedPorts == null)
            {
                m_sortedPorts = new Dictionary<Port, int>();
            }

            if (m_sortedPorts.Count == 0)
            {
                foreach (var incoming in m_in2out)
                {
                    // predicate is applied later as the port may be blocked meanwhile
                    m_sortedPorts.Add(incoming.Key, incoming.Value.Count);
                }
            }

            foreach (KeyValuePair<Port, int> tupel in m_sortedPorts.OrderBy(key => key.Value))
            {
                if (filter(tupel.Key))
                {
                    yield return tupel.Key;
                }
            }
        }

        public void ClearBlockingPortList()
        {
            m_sortedPorts = null;
        }

        public int ArcCount
        {
            get { return m_in2out.Sum(d => d.Value.Count); }
        }

        public IEnumerable<Tuple<Port, Port>> GetAllArcs()
        {
            foreach (var incoming in m_in2out)
            {
                foreach (Port p in incoming.Value.Keys)
                {
                    Tuple<Port, Port> result = new Tuple<Port, Port>(incoming.Key, p);
                    yield return result;
                }
            }
        }

        public Tuple<Port, Port> GetFirstArcOrDefault(Predicate<Port> outerFilter, Predicate<Tuple<Port, Port>> innerFilter)
        {
            foreach (var incoming in m_in2out)
            {
                if (outerFilter(incoming.Key))
                {
                    foreach (Port p in incoming.Value.Keys)
                    {
                        Tuple<Port, Port> result = new Tuple<Port, Port>(incoming.Key, p);
                        if (innerFilter(result))
                        {
                            return result;
                        }
                    }
                }
            }
            return new Tuple<Port, Port>(null, null);
        }

        public IEnumerable<Port> Ports
        {
            get
            {
                Dictionary<Port, bool> returnedPorts = new Dictionary<Port, bool>();
                foreach (var incoming in m_in2out)
                {
                    if (!returnedPorts.ContainsKey(incoming.Key))
                    {
                        returnedPorts.Add(incoming.Key, false);
                        yield return incoming.Key;
                    }

                    foreach (var outgoing in incoming.Value)
                    {
                        if (!returnedPorts.ContainsKey(outgoing.Key))
                        {
                            returnedPorts.Add(outgoing.Key, false);
                            yield return outgoing.Key;
                        }
                    }
                }
            }
        }

        public bool Contains(Port from, Port to)
        {
            if (!m_in2out.ContainsKey(from))
            {
                return false;
            }

            if (m_in2out[from].ContainsKey(to))
            {
                return true;
            }
            else
            {
                // ROUTETHROUGH
                foreach (Port key in m_in2out[from].Keys)
                {
                    if (key.Name.Contains("ROUTETHROUGH") && key.Name.StartsWith(to.Name))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool Contains(string from, string to)
        {
            return Contains(new Port(from), new Port(to));
        }

        public bool Contains(string portName)
        {
            return Contains(new Port(portName));
        }

        public bool Contains(Port port)
        {
            return
                m_allPortsFrom.ContainsKey(port.NameKey) ||
                m_allPortsTo.ContainsKey(port.NameKey);
        }

        public bool ContainsLeft(Port port)
        {
            return m_allPortsFrom.ContainsKey(port.NameKey);
        }

        public bool ContainsRight(Port port)
        {
            return m_allPortsTo.ContainsKey(port.NameKey);
        }

        /// <summary>
        /// Sorted string representation to allow compares
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (var incoming in m_in2out.OrderBy(t => t.Key.Name))
            {
                foreach (var outgoing in incoming.Value.OrderBy(p => p.Key.Name))
                {
                    buffer.AppendLine(incoming.Key + "\t" + outgoing.Key);
                }
            }

            return buffer.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SwitchMatrix))
            {
                return false;
            }

            SwitchMatrix other = (SwitchMatrix)obj;

            // compare all ports
            if (other.m_allPortsFrom.Count != m_allPortsFrom.Count)
            {
                return false;
            }
            if (other.m_allPortsTo.Count != m_allPortsTo.Count)
            {
                return false;
            }

            foreach (Port otherPort in other.Ports)
            {
                if (!Contains(otherPort))
                {
                    return false;
                }
            }
            return true;

            //return this.ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public void RemoveAllDrivers(string drivenPortName)
        {
            Port drivenPort = new Port(drivenPortName);
            List<Port> driversToDelete = new List<Port>();
            foreach (var tupel in m_in2out)
            {
                if (tupel.Value.ContainsKey(drivenPort))
                {
                    tupel.Value.Remove(drivenPort);
                }
                if (tupel.Value.Count == 0)
                {
                    driversToDelete.Add(tupel.Key);
                }
            }
            foreach (Port p in driversToDelete)
            {
                m_in2out.Remove(p);
            }
        }

        public int Outputs
        {
            get { return m_allPortsTo.Count; }
        }

        public int Inputs
        {
            get { return m_allPortsFrom.Count; }
        }

        /// <summary>
        /// The hashcode must be calculated after reading in the XDl description of this SwitchMatrix. After this, it may not be changed again as the HasCode is used as a key
        /// and on different machines the calculated hash codes may differ!
        /// </summary>
        public int HashCode { get; set; }

        private Dictionary<Port, Dictionary<Port, bool>> m_in2out = new Dictionary<Port, Dictionary<Port, bool>>();
        private Dictionary<uint, Port> m_allPortsFrom = new Dictionary<uint, Port>();
        private Dictionary<uint, Port> m_allPortsTo = new Dictionary<uint, Port>();

        private Dictionary<Port, int> m_sortedPorts;
    }
}