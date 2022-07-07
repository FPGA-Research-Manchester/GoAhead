using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GoAhead.FPGA
{
    [Serializable]
    public class WireList : ISerializable, IEnumerable<Wire>
    {
        public WireList()
        {
        }

        public WireList(IEnumerable<Wire> wires)
        {
            foreach(Wire w in wires)
            {
                Add(w);
            }
        }

        public void Add(Wire wire, bool updateIndex = true)
        {
            m_wires.Add(wire);

            if (updateIndex && m_wireKeys != null)
            {
                if (!m_wireKeys.ContainsKey(wire.LocalPipKey))
                {
                    m_wireKeys.Add(wire.LocalPipKey, new Dictionary<uint, Dictionary<int, Dictionary<int, bool>>>());
                }
                if (!m_wireKeys[wire.LocalPipKey].ContainsKey(wire.PipOnOtherTileKey))
                {
                    m_wireKeys[wire.LocalPipKey].Add(wire.PipOnOtherTileKey, new Dictionary<int, Dictionary<int, bool>>());
                }
                if (!m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey].ContainsKey(wire.XIncr))
                {
                    m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey].Add(wire.XIncr, new Dictionary<int, bool>());
                }
                if (!m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey][wire.XIncr].ContainsKey(wire.YIncr))
                {
                    m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey][wire.XIncr].Add(wire.YIncr, wire.LocalPipIsDriver);
                }
            }
        }

        public void Clear()
        {
            ClearCache();
            m_wireKeys.Clear();
            m_wires.Clear();
        }

        public void ClearCache()
        {
            if (m_chache != null)
            {
                m_chache.Clear();
                m_chache = null;
            }
        }

        public void ClearWireKeys()
        {
            m_wireKeys.Clear();
        }

        public bool HasWire(Wire wire)
        {
            if (m_wireKeys == null)
            {
                return m_wires.Contains(wire);
            }
            else
            {
                if (!m_wireKeys.ContainsKey(wire.LocalPipKey))
                {
                    return false;
                }
                else if (!m_wireKeys[wire.LocalPipKey].ContainsKey(wire.PipOnOtherTileKey))
                {
                    return false;
                }
                else if (!m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey].ContainsKey(wire.XIncr))
                {
                    return false;
                }
                else if (!m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey][wire.XIncr].ContainsKey(wire.YIncr))
                {
                    return false;
                }
                else
                {
                    return m_wireKeys[wire.LocalPipKey][wire.PipOnOtherTileKey][wire.XIncr][wire.YIncr] == wire.LocalPipIsDriver;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WireList))
            {
                return false;
            }

            WireList other = (WireList)obj;

            if (other.m_wires.Count != m_wires.Count)
            {
                return false;
            }

            foreach (Wire w in other)
            {
                if (!HasWire(w))
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<Wire> GetAllWires(Port port)
        {
            return GetAllWires(port.NameKey);
        }

        public IEnumerable<Wire> GetAllWires(string portName)
        {
            return GetAllWires(FPGA.Instance.IdentifierListLookup.GetKey(portName));
        }

        public IEnumerable<Wire> GetAllWires(uint key)
        {
            if (m_chache == null)
            {
                m_chache = new Dictionary<uint, List<Wire>>();
                //Console.WriteLine("Creating cache for wire " + port.Name + " in wire list " + this.Key);
            }
            if (!m_chache.ContainsKey(key))
            {
                m_chache.Add(key, new List<Wire>());
                m_chache[key].AddRange(m_wires.Where(w => w.LocalPipKey == key));
            }

            return m_chache[key];
        }

        private WireList(SerializationInfo info, StreamingContext context)
        {
            string[] atoms = info.GetString("wirelist").Split(';');
            int i = 0;
            while (i + 4 < atoms.Length)
            {
                bool localPipIsDriver = atoms[i + 2].Equals("1");
                //Wire w = new Wire(atoms[i + 0], atoms[i + 1], localPipIsDriver, (short)Int32.Parse(atoms[i + 3]), (short)Int32.Parse(atoms[i + 4]));
                Wire w = new Wire((uint)int.Parse(atoms[i + 0]), (uint)int.Parse(atoms[i + 1]), localPipIsDriver, (short)int.Parse(atoms[i + 3]), (short)int.Parse(atoms[i + 4]));
                m_wires.Add(w);
                i += 5;
            }
            Key = int.Parse(info.GetString("hash"));
            // release for GC
            m_wireKeys = null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (Wire w in this)
            {
                buffer.Append(w.GetStringForSerialization());
            }

            info.AddValue("wirelist", buffer.ToString());
            info.AddValue("hash", Key);
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (Wire w in m_wires.OrderBy(w => w.LocalPip))
            {
                buffer.AppendLine(w.GetStringForSerialization());
            }

            return buffer.ToString();
        }

        public override int GetHashCode()
        {
            return Key;
        }

        /// <summary>
        /// the number of wires in this wire list
        /// </summary>
        public int Count
        {
            get { return m_wires.Count; }
        }

        public List<Wire> m_wires = new List<Wire>();
        private Dictionary<uint, List<Wire>> m_chache = null;

        public Dictionary<uint, Dictionary<uint, Dictionary<int, Dictionary<int, bool>>>> m_wireKeys = new Dictionary<uint, Dictionary<uint, Dictionary<int, Dictionary<int, bool>>>>();

        /// <summary>
        /// The hashcode must be calculated after reading in the XDL description of this SwitchMatrix. Afterone, it may not be changed again as the HasCode is used as a key
        /// and on different machines the calculated hash codes may differ!
        /// </summary>
        public int Key { get; set; }

        public IEnumerator<Wire> GetEnumerator()
        {
            foreach (Wire w in m_wires)
            {
                yield return w;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Serializable]
    public class Wire : IComparable
    {
        public Wire(uint localPipKey, uint pipOnOtherTileKey, bool localPipIsDriver, int xIncr, int yIncr)
        {
            m_localPipKey = localPipKey;
            m_pipOnOtherTileKey = pipOnOtherTileKey;
            m_localPipIsDriver = localPipIsDriver;
            m_xIncr = xIncr;
            m_yIncr = yIncr;
            m_cost = 0;
        }

        public Wire(uint localPipKey, uint pipOnOtherTileKey, bool localPipIsDriver, int xIncr, int yIncr, int cost)
        {
            m_localPipKey = localPipKey;
            m_pipOnOtherTileKey = pipOnOtherTileKey;
            m_localPipIsDriver = localPipIsDriver;
            m_xIncr = xIncr;
            m_yIncr = yIncr;
            m_cost = cost;
        }

        public uint LocalPipKey
        {
            get { return m_localPipKey; }
        }

        public string LocalPip
        {
            get { return FPGA.Instance.IdentifierListLookup.GetIdentifier(m_localPipKey); }
        }

        public uint PipOnOtherTileKey
        {
            get { return m_pipOnOtherTileKey; }
        }

        public string PipOnOtherTile
        {
            get { return FPGA.Instance.IdentifierListLookup.GetIdentifier(PipOnOtherTileKey); }
        }

        public bool LocalPipIsDriver
        {
            get { return m_localPipIsDriver; }
        }

        public int XIncr
        {
            get { return m_xIncr; }
        }

        public int YIncr
        {
            get { return m_yIncr; }
        }

        public double Cost
        {
            get { return m_cost; }
            set { m_cost = value; }
        }

        public override int GetHashCode()
        {
            return GetStringForSerialization().GetHashCode();
        }

        public string GetStringForSerialization()
        {
            return m_localPipKey + ";" + m_pipOnOtherTileKey + ";" + (LocalPipIsDriver ? "1" : "0") + ";" + XIncr + ";" + YIncr + ";";
            //return this.LocalPip + ";" + this.PipOnOtherTile + ";" + (this.LocalPipIsDriver ? "1" : "0") + ";" + this.XIncr + ";" + this.YIncr + ";";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Wire))
            {
                return false;
            }

            Wire other = (Wire)obj;

            return
                LocalPipIsDriver == other.LocalPipIsDriver &&
                m_localPipKey == other.m_localPipKey &&
                m_pipOnOtherTileKey == other.m_pipOnOtherTileKey &&
                m_xIncr == other.m_xIncr &&
                m_yIncr == other.m_yIncr &&
                m_cost == other.m_cost;

            //bool equal = this.GetStringForSerialization().Equals(other.GetStringForSerialization());
            //return equal;
        }

        public override string ToString()
        {
            return GetStringForSerialization();
        }

        public int CompareTo(object obj)
        {
            return ToString().CompareTo(obj.ToString());
        }

        public static int CompareByDistance(Wire wire1, Wire wire2)
        {
            double dist1 = Math.Sqrt(Math.Pow(wire1.XIncr, 2) + Math.Pow(wire1.YIncr, 2));
            double dist2 = Math.Sqrt(Math.Pow(wire2.XIncr, 2) + Math.Pow(wire2.YIncr, 2));

            return dist1.CompareTo(dist2);
        }

        private readonly uint m_localPipKey;
        private readonly uint m_pipOnOtherTileKey;
        private readonly int m_xIncr;
        private readonly int m_yIncr;
        public bool m_localPipIsDriver;
        private double m_cost;
    }
}