using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.FPGA
{
    [Serializable]
    public class RawTile
    {
        public RawTile(Tile tile)
        {
            TileKeyX = tile.TileKey.X;
            TileKeyY = tile.TileKey.Y;
            TileLocationString = tile.Location;
            SwitchMatrixHashCode = tile.SwitchMatrixHashCode;
            WireListHashCode = tile.WireListHashCode;
            IncomingWireListHashCode = tile.IncomingWireListHashCode;
            ClockRegion = tile.ClockRegion;
            foreach (Slice slice in tile.Slices)
            {
                RawSlice rawSlice = new RawSlice(slice);
                RawSlices.Add(rawSlice);
            }

            foreach (string excludedPort in tile.GetAllBlockedPorts(Tile.BlockReason.ExcludedFromBlocking))
            {
                PortsToExcludeFromBlocking.Add(excludedPort);
            }
            foreach (string port in tile.GetAllBlockedPorts(Tile.BlockReason.Stopover))
            {
                PortsOnArcsWithStopovers.Add(port);
            }

            // Time Data
            if (tile.TimeData != null)
            {
                foreach (var data in tile.TimeData)
                {
                    TimingData_Pairs_1.Add(data.Key[0]);
                    TimingData_Pairs_2.Add(data.Key[1]);
                    int index = -1;
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute1);
                    if (index > -1)
                        TimingData_Times_1.Add(data.Value[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute2);
                    if (index > -1)
                        TimingData_Times_2.Add(data.Value[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute3);
                    if (index > -1)
                        TimingData_Times_3.Add(data.Value[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute4);
                    if (index > -1)
                        TimingData_Times_4.Add(data.Value[index]);
                }
            }   
            
            if(tile.WiresTrajectoriesData != null)
            {
                foreach(var data in tile.WiresTrajectoriesData)
                {
                    WiresTrajectoriesData_keys.Add(data.Key);
                    string vals = "";
                    foreach (uint u in data.Value) vals += u.ToString() + " ";
                    vals = vals.TrimEnd(' ');
                    WiresTrajectoriesData_vals.Add(vals);
                }
            }
        }

        public readonly int TileKeyX;
        public readonly int TileKeyY;

        public readonly string TileLocationString;
        public readonly int SwitchMatrixHashCode;
        public readonly int WireListHashCode;
        public readonly int IncomingWireListHashCode;

        public readonly string ClockRegion;

        public readonly List<RawSlice> RawSlices = new List<RawSlice>();
        public readonly List<string> PortsToExcludeFromBlocking = new List<string>();
        public readonly List<string> PortsOnArcsWithStopovers = new List<string>();

        public readonly List<uint> TimingData_Pairs_1 = new List<uint>();
        public readonly List<uint> TimingData_Pairs_2 = new List<uint>();
        public readonly List<float> TimingData_Times_1 = new List<float>();
        public readonly List<float> TimingData_Times_2 = new List<float>();
        public readonly List<float> TimingData_Times_3 = new List<float>();
        public readonly List<float> TimingData_Times_4 = new List<float>();

        public readonly List<uint> WiresTrajectoriesData_keys = new List<uint>();
        // string = concatinated uints with ' ' delimiter
        public readonly List<string> WiresTrajectoriesData_vals = new List<string>();
    }

    

    [Serializable]
    public class Tile
    {

        public enum BlockReason { Blocked = 0, ExcludedFromBlocking = 1, OccupiedByMacro = 2, Stopover = 3, ToBeBlocked = 4 };

        private List<Tile> m_subTiles = null;

        private List<Slice> m_slices = null;

        /// <summary>
        /// Contains all blocked ports
        /// True: this port was blocked becuase a macro has been instantiated
        /// False: this port was blocked because of anything else (net, blocker, exclude from further blocking, ...)
        /// </summary>
        //private Dictionary<String, bool> m_blockedPorts = null;
        private Dictionary<uint, BlockReason> m_blockedPorts = null;

        public Dictionary<uint, List<uint>> WiresTrajectoriesData = null;
        public void AddWireTrajectoryData(uint pip, uint tile)
        {
            if (WiresTrajectoriesData == null)
                WiresTrajectoriesData = new Dictionary<uint, List<uint>>();

            if (!WiresTrajectoriesData.ContainsKey(pip))
                WiresTrajectoriesData.Add(pip, new List<uint>());

            WiresTrajectoriesData[pip].Add(tile);
        }

        public IEnumerable<string> GetTrajectoryTilesForWire(string pip)
        {
            if (WiresTrajectoriesData == null)
                return Enumerable.Empty<string>();

            uint pipkey = FPGA.Instance.IdentifierListLookup.GetKey(pip);
            if (!WiresTrajectoriesData.ContainsKey(pipkey))
                return Enumerable.Empty<string>();

            return WiresTrajectoriesData[pipkey].Select(t => FPGA.Instance.IdentifierListLookup.GetIdentifier(t));
        }

        private int m_wireListHashCode = -1;
        private readonly TileKey m_key;
        private readonly string m_location;
        private readonly int m_locationX;
        private readonly int m_locationY;
        private string m_clockRegion;

        private Dictionary<Tuple<Location, Location>, int> m_dijkstraWires;

        public void AddConnection(Location from, Location to, int cost)
        {
            m_dijkstraWires.Add(new Tuple<Location, Location>(from, to), cost);
        }
        public int? GetConnectionCost (Location from, Location to)
        {
            try
            {
                return m_dijkstraWires[new Tuple<Location, Location>(from, to)];
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
        }
        public void SetConnectionCost (Location from, Location to, int newCost)
        {
            m_dijkstraWires[new Tuple<Location, Location>(from, to)] = newCost;
        }

        private static Regex m_tileMatch = new Regex(@"X\d+Y\d+", RegexOptions.Compiled);
        private static Regex m_tileMatchS3 = new Regex(@"^R\d+C\d+", RegexOptions.Compiled);

        public SwitchMatrix SwitchMatrix
        {
            get { return FPGA.Instance.GetSwitchMatrix(SwitchMatrixHashCode); }
        }

        public WireList WireList
        {
            get { return FPGA.Instance.GetWireList(WireListHashCode); }
        }

        private List<Wire> incomingWires = null;
        public void AddIncomingWire(Wire wire)
        {
            if (incomingWires == null) incomingWires = new List<Wire>();
            incomingWires.Add(wire);
        }

        public IEnumerable<Wire> IncomingWireList
        {
            get
            {
                if (incomingWires != null) return incomingWires;
                return FPGA.Instance.GetIncomingWireList(IncomingWireListHashCode);
            }
        }

        public Tile GetTileOfIncomingWire(Wire wire)
        {
            return Navigator.GetDestinationByWire(this, wire, false);
        }

        public Tile GetTileAtWireEnd(Wire wire)
        {
            return Navigator.GetDestinationByWire(this, wire);
        }

        public string ClockRegion
        {
            get { return m_clockRegion; }
            set { m_clockRegion = value; }
        }

        public int SwitchMatrixHashCode { get; set; }

        public int WireListHashCode
        {
            get { return m_wireListHashCode; }
            set { m_wireListHashCode = value; }
        }

        public int IncomingWireListHashCode {get; set;}

        public Tile(RawTile rawTile)
        {
            m_key = new TileKey(rawTile.TileKeyX, rawTile.TileKeyY);
            m_location = rawTile.TileLocationString;
            SwitchMatrixHashCode = rawTile.SwitchMatrixHashCode;
            WireListHashCode = rawTile.WireListHashCode;
            IncomingWireListHashCode = rawTile.IncomingWireListHashCode;
            m_dijkstraWires = new Dictionary<Tuple<Location, Location>, int> ();
            // vivado
            m_clockRegion = rawTile.ClockRegion != null ? rawTile.ClockRegion : "unknown";
            // location in of form LEFTPART_X\d+Y\d+
            // and left part may contain X. extract x and y digits from tight part
            if (m_tileMatch.IsMatch(m_location))
            {
                string[] atoms = m_location.Split('X', 'Y');
                m_locationX = int.Parse(atoms[atoms.Length - 2]);
                m_locationY = int.Parse(atoms[atoms.Length - 1]);
            }
            else if (m_tileMatchS3.IsMatch(m_location)) // Spartan 3
            {
                string[] atoms = m_location.Split('R', 'C');
                m_locationX = int.Parse(atoms[atoms.Length - 1]); // reverse!
                m_locationY = int.Parse(atoms[atoms.Length - 2]);
            }

            foreach (string portName in rawTile.PortsToExcludeFromBlocking)
            {
                // restore s6 bugfix
                BlockPort(portName, BlockReason.ExcludedFromBlocking);
            }
            if (rawTile.PortsOnArcsWithStopovers != null)
            {
                foreach (string portName in rawTile.PortsOnArcsWithStopovers)
                {
                    BlockPort(portName, BlockReason.Stopover);
                }
            }

            if (!AddTimeData(rawTile))
            {
                //Console.WriteLine("Adding time data failed for tile: " + m_location);
            }

            if(rawTile.WiresTrajectoriesData_keys != null && rawTile.WiresTrajectoriesData_vals != null)
            {
                if(rawTile.WiresTrajectoriesData_keys.Count == rawTile.WiresTrajectoriesData_vals.Count)
                {
                    for(int i=0; i<rawTile.WiresTrajectoriesData_keys.Count; i++)
                    {
                        uint pip = rawTile.WiresTrajectoriesData_keys[i];
                        string[] vals = rawTile.WiresTrajectoriesData_vals[i].Split(' ');

                        foreach(var val in vals)
                        {
                            if(uint.TryParse(val, out uint tile))
                            {
                                AddWireTrajectoryData(pip, tile);
                            }
                            else
                            {
                                Console.WriteLine("Failed parsing " + val + " into uint");
                            }
                        }
                    }
                }
            }
        }

        public Tile(TileKey key, string location)
        {
            m_key = key;
            m_location = location;
            m_dijkstraWires = new Dictionary<Tuple<Location, Location>, int> ();

            // location in of form LEFTPART_X\d+Y\d+
            // and left part may contain X. extract x and y digits from tight part
            if (m_tileMatch.IsMatch(m_location))
            {
                string[] atoms = m_location.Split('X', 'Y');
                m_locationX = (short)int.Parse(atoms[atoms.Length - 2]);
                m_locationY = (short)int.Parse(atoms[atoms.Length - 1]);
            }
            else if (m_tileMatchS3.IsMatch(m_location)) // Spartan 3
            {
                string[] atoms = m_location.Split('R', 'C');
                m_locationX = (short)int.Parse(atoms[atoms.Length - 1]);  // reverse!
                m_locationY = (short)int.Parse(atoms[atoms.Length - 2]);
            }
        }

        public override string ToString()
        {
            return m_key.ToString() + ":" + Location;
        }

        public TileKey TileKey
        {
            get { return m_key; }
        }

        public List<Slice> Slices
        {
            get
            {
                if (m_slices == null)
                {
                    m_slices = new List<Slice>();
                }
                return m_slices;
            }
        }

        public List<Tile> Subtiles
        {
            get
            {
                if (m_subTiles == null)
                {
                    m_subTiles = new List<Tile>();
                }
                return m_subTiles;
            }
        }

        public string Location
        {
            get { return m_location; }
        }

        public int LocationX
        {
            get { return m_locationX; }
        }

        public int LocationY
        {
            get { return m_locationY; }
        }

        public void Add(Tile subtile)
        {
            Subtiles.Add(subtile);
        }

        public void BlockPort(string portName, BlockReason reason)
        {
            BlockPort(new Port(portName), reason);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="blockedByMacro">True: this port was blocked because an instantiated macro now occupies this port. False anything else, blocked by a net, excluded from future blocking, blocked by a macro</param>
        public void BlockPort(Port p, BlockReason reason)
        {
            if (m_blockedPorts == null)
            {
                m_blockedPorts = new Dictionary<uint, BlockReason>();
            }

            if (!m_blockedPorts.ContainsKey(p.NameKey))
            {
                m_blockedPorts.Add(p.NameKey, reason);
            }
            // notify FPGA about blocked port
            //FPGAEventManager.Instance.PortBlockingChanged(this, p);
        }

        public void UnblockPort(Port p)
        {
            if (m_blockedPorts != null)
            {
                m_blockedPorts.Remove(p.NameKey);
            }
            // notify FPGA about blocked port
            //FPGAEventManager.Instance.PortBlockingChanged(this, p);
        }

        public void UnblockPort(string portName)
        {
            if (m_blockedPorts != null)
            {
                // intern key
                Port p = new Port(portName);
                m_blockedPorts.Remove(p.NameKey);
            }
        }

        public bool IsPortBlocked(Port port)
        {
            if (m_blockedPorts == null)
            {
                return false;
            }

            return m_blockedPorts.ContainsKey(port.NameKey);
        }

        public bool IsPortBlocked(string portName)
        {
            if (m_blockedPorts == null)
            {
                return false;
            }

            return IsPortBlocked(new Port(portName));
        }

        public bool IsPortBlocked(Port port, BlockReason reason)
        {
            if (m_blockedPorts == null)
            {
                return false;
            }

            if (!m_blockedPorts.ContainsKey(port.NameKey))
            {
                return false;
            }

            return m_blockedPorts[port.NameKey].Equals(reason);
        }

        public bool IsPortBlocked(string portName, BlockReason reason)
        {
            if (m_blockedPorts == null)
            {
                return false;
            }
            // intern key
            Port p = new Port(portName);
            return IsPortBlocked(p, reason);
            //return this.m_blockedPorts.ContainsKey(p.NameKey);
        }

        public void UnblockAllPorts()
        {
            if (m_blockedPorts != null)
            {
                m_blockedPorts.Clear();
            }
        }

        public bool HasUsedSlice
        {
            get
            {
                return Slices.Count(s => !s.Usage.Equals(FPGATypes.SliceUsage.Free)) > 0;
            }
        }

        public bool HasBlockedPorts
        {
            get { return (m_blockedPorts == null ? false : m_blockedPorts.Count != 0); }
        }

        public bool HasNonstopoverBlockedPorts
        {
            get { return m_blockedPorts == null ? false : m_blockedPorts.Count(p => p.Value != BlockReason.Stopover) > 0; }
        }

        public int AllBlockedPortsHash
        {
            get
            {
                StringBuilder buffer = new StringBuilder();
                if (m_blockedPorts != null)
                {
                    foreach (KeyValuePair<uint, BlockReason> tupel in m_blockedPorts.OrderBy(t => t.Key))
                    {
                        string portName = FPGA.Instance.IdentifierListLookup.GetIdentifier(tupel.Key);
                        buffer.AppendLine(portName);
                    }
                }
                return buffer.ToString().GetHashCode();
            }
        }

        public IEnumerable<string> GetAllBlockedPorts()
        {
            if (m_blockedPorts != null)
            {
                foreach (KeyValuePair<uint, BlockReason> tupel in m_blockedPorts.OrderBy(t => t.Key))
                {
                    string portName = FPGA.Instance.IdentifierListLookup.GetIdentifier(tupel.Key);
                    yield return portName;
                }
            }
        }

        public IEnumerable<string> GetAllBlockedPorts(BlockReason reason)
        {
            if (m_blockedPorts != null)
            {
                foreach (KeyValuePair<uint, BlockReason> tupel in m_blockedPorts.Where(t => t.Value.Equals(reason)))
                {
                    string portName = FPGA.Instance.IdentifierListLookup.GetIdentifier(tupel.Key);
                    yield return portName;
                }
            }
        }

        public void Reset()
        {
            if (m_slices != null)
            {
                foreach (Slice slice in m_slices)
                {
                    slice.Reset();
                }
            }
            if (m_blockedPorts != null)
            {
                foreach (KeyValuePair<uint, BlockReason> item in m_blockedPorts.Where(
                    i => i.Value != BlockReason.ExcludedFromBlocking && i.Value != BlockReason.Stopover).ToList())
                {
                    m_blockedPorts.Remove(item.Key);
                }
                //this.m_blockedPorts.Clear();
            }
        }

        public bool HasSlice(string sliceName)
        {
            for (int i = 0; i < Slices.Count; i++)
            {
                if (m_slices[i].SliceName.Equals(sliceName))
                {
                    return true;
                }
            }
            return false;
        }

        public Slice GetSliceByName(string sliceName)
        {
            return Slices[(int)GetSliceNumberByName(sliceName)];
        }

        public int GetSliceNumberByName(string sliceName)
        {
            for (int i = 0; i < Slices.Count; i++)
            {
                if (Slices[(int)i].SliceName.Equals(sliceName))
                {
                    return i;
                }
            }
            throw new ArgumentException("No slice named " + sliceName + " found on tile " + Location);
        }

        public int GetSliceNumberByPortName(Port p)
        {
            for (int i = 0; i < Slices.Count; i++)
            {
                if (Slices[i].PortMapping.Contains(p))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool IsSlicePort(string portName)
        {
            return IsSliceInPort(portName) || IsSliceOutPort(portName);
        }

        /// <summary>
        /// Returns wether on of the slice provides the given port as an input
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool IsSliceInPort(Port p)
        {
            foreach (Slice s in Slices)
            {
                if (FPGA.Instance.GetInPortOutPortMapping(s.InPortOutPortMappingHashCode).IsSliceInPort(p))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSliceInPort(string portName)
        {
            return IsSliceInPort(new Port(portName));
        }

        public bool IsSliceOutPort(Port p)
        {
            foreach (Slice s in Slices)
            {
                if (FPGA.Instance.GetInPortOutPortMapping(s.InPortOutPortMappingHashCode).IsSliceOutPort(p))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSliceOutPort(string portName)
        {
            return IsSliceOutPort(new Port(portName));
        }

        #region Time Model
        public enum TimeAttributes
        {
            Attribute1 = 0,
            Attribute2 = 1,
            Attribute3 = 2,
            Attribute4 = 3
        }

        /// <summary>
        /// Key: unit[] should be a pair of port identifiers designating a switch matrix connection
        /// Value: float[] is the time data for the port pair and it can have up to 4 elements
        /// </summary>
        public Dictionary<uint[], float[]> TimeData = null;

        /// <summary>
        /// This stores indices of the time attributes in the TimeData dictionary values
        /// As the float[] may be of arbitrary length, this is how we find the correct attibute data
        /// </summary>
        public static Dictionary<TimeAttributes, int> TimeModelAttributeIndices = null;

        public static int GetIndexForTimeAttribute(TimeAttributes attribute)
        {
            if (TimeModelAttributeIndices == null) return -1;
            if (!TimeModelAttributeIndices.ContainsKey(attribute)) return -1;

            return TimeModelAttributeIndices[attribute];
        }

        /// <summary>
        /// Returns a bool[4] indicating whether the respective attribute is present in the model
        /// </summary>
        /// <returns></returns>
        public static bool[] GetPresentTimeAttributes()
        {
            if (TimeModelAttributeIndices == null) return null;

            return new bool[]
            {
                TimeModelAttributeIndices.ContainsKey(TimeAttributes.Attribute1),
                TimeModelAttributeIndices.ContainsKey(TimeAttributes.Attribute2),
                TimeModelAttributeIndices.ContainsKey(TimeAttributes.Attribute3),
                TimeModelAttributeIndices.ContainsKey(TimeAttributes.Attribute4)
            };
        }

        public void AddTimeData(string port1, string port2, float[] data)
        {
            AddTimeData(FPGA.Instance.IdentifierListLookup.GetKey(port1), FPGA.Instance.IdentifierListLookup.GetKey(port2), data);
        }

        public void AddTimeData(uint port1, uint port2, float[] data)
        {
            if (TimeData == null)
                TimeData = new Dictionary<uint[], float[]>(new UIntEqualityComparer());

            uint[] pair = new uint[]
            {
                port1,
                port2
            };

            TimeData[pair] = data;
        }

        /// <summary>
        /// Returns stored time data for a specific attribute, 0 if not found
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public float GetTimeData(Port from, Port to, TimeAttributes attribute)
        {
            int i = GetIndexForTimeAttribute(attribute);
            if (i < 0) return 0;

            List<float> data = GetTimeData(from, to);
            if (data == null || i >= data.Count) return 0;

            return data[i];
        }        

        /// <summary>
        /// Returns stored time data for all present attributes.
        /// Use GetIndexForTimeAttribute as index for this list to get the data for a specific attribute.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<float> GetTimeData(Port from, Port to)
        {
            if (TimeData == null) return null;

            uint[] pair = new uint[]
            {
                from.NameKey,
                to.NameKey
            };

            if (!TimeData.ContainsKey(pair)) return null;

            return TimeData[pair].ToList();
        }

        public List<float> GetTimeData(string from, string to)
        {
            return GetTimeData(new Port(from), new Port(to));
        }

        private bool AddTimeData(RawTile rawTile)
        {
            // To parse in valid serialized data, all of the following lists must be non-null
            if (rawTile.TimingData_Pairs_1 == null || 
                rawTile.TimingData_Pairs_2 == null ||
                rawTile.TimingData_Times_1 == null ||
                rawTile.TimingData_Times_2 == null ||
                rawTile.TimingData_Times_3 == null ||
                rawTile.TimingData_Times_4 == null)
                return false;

            // Check for consistency, both pair lists must be of same length and 
            // if any time list has elements it must also be the same length
            int size = rawTile.TimingData_Pairs_1.Count;

            if (rawTile.TimingData_Pairs_2.Count != size) return false;

            bool att1 = false;
            if (rawTile.TimingData_Times_1.Count != 0)
            {
                if (rawTile.TimingData_Times_1.Count != size) return false;
                att1 = true;
            }
            bool att2 = false;
            if (rawTile.TimingData_Times_2.Count != 0)
            {
                if (rawTile.TimingData_Times_2.Count != size) return false;
                att2 = true;
            }
            bool att3 = false;
            if (rawTile.TimingData_Times_3.Count != 0)
            {
                if (rawTile.TimingData_Times_3.Count != size) return false;
                att3 = true;
            }
            bool att4 = false;
            if (rawTile.TimingData_Times_4.Count != 0)
            {
                if (rawTile.TimingData_Times_4.Count != size) return false;
                att4 = true;
            }

            // The serialized data is valid, we can now read it in
            for (int i = 0; i < size; i++)
            {
                List<float> attVals = new List<float>();
                if (att1) attVals.Add(rawTile.TimingData_Times_1[i]);
                if (att2) attVals.Add(rawTile.TimingData_Times_2[i]);
                if (att3) attVals.Add(rawTile.TimingData_Times_3[i]);
                if (att4) attVals.Add(rawTile.TimingData_Times_4[i]);
                AddTimeData(rawTile.TimingData_Pairs_1[i], rawTile.TimingData_Pairs_2[i], attVals.ToArray());
            }

            return true;
        }
        #endregion // Time Model
    }
}