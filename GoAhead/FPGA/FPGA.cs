using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;

namespace GoAhead.FPGA
{
    [Serializable]
    public partial class FPGA
    {
        private FPGA()
        {
        }

        public void Add(Tile tile)
        {
            m_tiles.Add(tile);

            if (tile.TileKey.X > MaxX)
            {
                MaxX = tile.TileKey.X;
            }
            if (tile.TileKey.Y > MaxY)
            {
                MaxY = tile.TileKey.Y;
            }

            if (!m_location2TileKey.ContainsKey(tile.Location))
            {
                m_location2TileKey.Add(tile.Location, tile.TileKey);
            }
        }

        public void Add(int hashCode, WireList wires)
        {
            m_wires.Add(hashCode, wires);
        }

        public void Add(int hashcode, SwitchMatrix matrix)
        {
            m_matrices.Add(hashcode, matrix);
        }

        public void Add(InPortOutPortMapping portMapping)
        {
            m_inOutPortMappings.Add(portMapping.GetHashCode(), portMapping);
        }

        public bool Contains(string tileIdentifier)
        {
            return m_location2TileKey.ContainsKey(tileIdentifier);
        }

        public bool Contains(TileKey key)
        {
            return m_tiles.Contains(key);
        }

        public bool Contains(int x, int y)
        {
            return m_tiles.Contains(x, y);
        }

        public bool ContainsSlice(string sliceName)
        {
            return GetSlice(sliceName) != null;
        }

        public bool Contains(SwitchMatrix matrix)
        {
            return m_matrices.ContainsKey(matrix.HashCode);
        }

        public bool ContainsWireList(int hashCode)
        {
            return m_wires.ContainsKey(hashCode);
        }

        public bool ContainsSwitchMatrix(int hashCode)
        {
            return m_matrices.ContainsKey(hashCode);
        }

        public bool Contains(Tile tile)
        {
            return m_tiles.Contains(tile);
        }

        public bool Contains(WireList wires)
        {
            return m_wires.ContainsKey(wires.GetHashCode());
        }

        public bool Contains(InPortOutPortMapping portMapping)
        {
            return m_inOutPortMappings.ContainsKey(portMapping.GetHashCode());
        }

        public Tile GetTile(TileKey key)
        {
            if (Contains(key))
            {
                return m_tiles.GetTile(key);
            }
            else
            {
                throw new ArgumentException("Tile " + key.ToString() + " not found");
            }
        }

        public Tile GetTile(int x, int y)
        {
            if (m_tiles.Contains(x, y))
            {
                return m_tiles.GetTile(x, y);
            }
            else
            {
                throw new ArgumentException("Tile " + x.ToString() + y.ToString() + " not found");
            }
        }

        public Tile GetTile(string location)
        {
            if (m_location2TileKey.ContainsKey(location))
            {
                TileKey key = m_location2TileKey[location];
                return m_tiles.GetTile(key);//[key.X][key.Y];
            }
            else
            {
                return null;
                throw new ArgumentException("Tile " + location + " not found");
            }
        }

        public int GetMaxTileKeyX()
        {
            if (m_maxTileKeyX > 0)
            {
                return m_maxTileKeyX;
            }
            else
            {
                foreach (Tile t in GetAllTiles())
                {
                    if (t.TileKey.X > m_maxTileKeyX)
                    {
                        m_maxTileKeyX = t.TileKey.X;
                    }
                }

                return m_maxTileKeyX;
            }
        }

        public int GetMaxTileKeyY()
        {
            if (m_maxTileKeyY > 0)
            {
                return m_maxTileKeyY;
            }
            else
            {
                foreach (Tile t in GetAllTiles())
                {
                    if (t.TileKey.Y > m_maxTileKeyY)
                    {
                        m_maxTileKeyY = t.TileKey.Y;
                    }
                }

                return m_maxTileKeyY;
            }
        }

        public SwitchMatrix GetSwitchMatrix(int hashCode)
        {
            return m_matrices[hashCode];
        }

        public WireList GetWireList(int hashCode)
        {
            if (!m_wires.ContainsKey(hashCode))
            {
                return null;
            }

            return m_wires[hashCode];
        }

        public WireList GetIncomingWireList(int hashCode)
        {
            if (m_incomingWires == null) return null;
            if (!m_incomingWires.ContainsKey(hashCode)) return null;

            return m_incomingWires[hashCode];
        }

        public Slice GetSlice(string sliceName)
        {
            if (m_sliceName2TileKey == null)
            {
                m_sliceName2TileKey = new Dictionary<string, TileKey>();
            }
            if (m_sliceName2TileKey.Count == 0)
            {
                foreach (Tile t in GetAllTiles())
                {
                    foreach (Slice s in t.Slices)
                    {
                        m_sliceName2TileKey.Add(s.SliceName, t.TileKey);
                    }
                }
            }

            if (m_sliceName2TileKey.ContainsKey(sliceName))
            {
                Tile t = GetTile(m_sliceName2TileKey[sliceName]);
                return t.GetSliceByName(sliceName);
            }
            else
            {
                foreach (Tile t in GetAllTiles())
                {
                    foreach (Slice s in t.Slices)
                    {
                        if (s.SliceName.Equals(sliceName))
                        {
                            return s;
                        }
                    }
                }
            }
            return null;
            throw new ArgumentException("Slice " + sliceName + " not found");
        }

        public IEnumerable<Tile> GetAllTiles()
        {
            return m_tiles;
        }

        // Be very careful using this, it's not pretty and it's not quick.
        // It does however simplify PathSearchOnFPGA a heck of a lot as I can skip
        // straight to searching locations rather than faffing about splitting the
        // incoming location (which could also be a regex!!!!) into a Tile and Port.
        public IEnumerable<Location> GetAllLocations()
        {
            foreach(Tile t in GetAllTiles().Where(t => !Regex.IsMatch(t.ToString(), "NULL")))
            {
                foreach(Port p in t.SwitchMatrix.Ports)
                {
                    yield return new Location(t, p);
                }
            }
        }

        public IEnumerable<Location> GetAllLocationsInSelection()
        {
            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles().Where(t => !Regex.IsMatch(t.ToString(), "NULL")))
            {
                foreach (Port p in t.SwitchMatrix.Ports)
                {
                    yield return new Location(t, p);
                }
            }
        }

        public IEnumerable<Location> GetAllLocationsFromTile(Tile t)
        {
            foreach(Port p in t.SwitchMatrix.Ports)
            {
                yield return new Location(t, p);
            }
        }

        public IEnumerable<SwitchMatrix> GetAllSwitchMatrices()
        {
            foreach (SwitchMatrix sm in m_matrices.Values)
            {
                yield return sm;
            }
        }

        public IEnumerable<WireList> GetAllWireLists()
        {
            return m_wires.Values;
        }

        public void Reset()
        {
            Instance = new FPGA();
        }

        public void ClearWireList()
        {
            m_wires.Clear();
        }

        public InPortOutPortMapping GetInPortOutPortMapping(int hashCode)
        {
            return m_inOutPortMappings[hashCode];
        }

        public int TileCount
        {
            get { return m_tiles.Count; }
        }

        public int SwitchMatrixCount
        {
            get { return m_matrices.Count; }
        }

        public int WireListCount
        {
            get { return m_wires.Count; }
        }

        public int InPortOutPortMappingCount
        {
            get { return m_inOutPortMappings.Count; }
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.AppendLine("Family: " + Family);
            buffer.AppendLine("Device: " + DeviceName);
            buffer.AppendLine("Tiles: " + m_tiles.Count);
            buffer.AppendLine("Switchmatrices: " + m_matrices.Count);
            buffer.AppendLine("InOutPort-Mappings: " + m_inOutPortMappings.Count);
            buffer.AppendLine("Wirelists: " + m_wires.Count);
            buffer.AppendLine("Identifier: " + IdentifierListLookup.Count);
            buffer.AppendLine("");

            /*
            int index = 0;
            foreach(WireList wl in this.GetAllWireLists())
            {
                buffer.AppendLine("Wirelist " + index++ + " has " + wl.Count + " wires");
            }*/

            return buffer.ToString();
        }

        private void AddWLDtoFPGA(Tile tile)
        {
            if (tile.IncomingWireList == null) return;

            // Try to find existing equivalent wire list
            foreach(var entry in m_incomingWires)
            {
                if(tile.IncomingWireList.SequenceEqual(entry.Value))
                {
                    tile.IncomingWireListHashCode = entry.Key;
                    return;
                }
            }

            // If not found, create a new one
            int key = m_incomingWires.Count;
            m_incomingWires.Add(key, new WireList(tile.IncomingWireList) { Key = key });
            tile.IncomingWireListHashCode = key;
        }

        public void DoPreSerializationTasks()
        {
            // no clear as this field might be NULL after deserialization
            m_rawTiles = new Queue<RawTile>();
            foreach (Tile t in GetAllTiles())
            {
                AddWLDtoFPGA(t);
                RawTile rawTile = new RawTile(t);
                m_rawTiles.Enqueue(rawTile);
            }
            //this.m_wires.Clear();
            m_tiles.Clear();
            m_location2TileKey.Clear();

            // no clear as this field might be NULL after deserialization
            m_rawMatrices = new Queue<RawSwitchMatrix>();
            foreach (SwitchMatrix sm in m_matrices.Values)
            {
                RawSwitchMatrix rawSm = new RawSwitchMatrix(sm);
                m_rawMatrices.Enqueue(rawSm);
            }
            m_matrices.Clear();
        }

        /// <summary>
        /// Restore m_location2TileKey, m_tiles and m_matrices
        /// </summary>
        public void DoPostSerializationTasks()
        {
            m_location2TileKey.Clear();
            m_tiles.Clear();
            m_maxTileKeyX = 0;
            MaxX = 0;
            MaxY = 0;

            while (m_rawTiles.Count > 0)
            {
                RawTile rawTile = m_rawTiles.Dequeue();
                Tile tile = new Tile(rawTile);

                foreach (RawSlice rawSlice in rawTile.RawSlices)
                {
                    Slice slice = Slice.GetSliceFromRawSlice(tile, rawSlice);
                    tile.Slices.Add(slice);
                }

                Add(tile);
            }

            m_matrices.Clear();
            while (m_rawMatrices.Count > 0)
            {
                RawSwitchMatrix rawSM = m_rawMatrices.Dequeue();
                SwitchMatrix sm = new SwitchMatrix(rawSM);
                Add(sm.HashCode, sm);
            }

            m_rawTiles.Clear();
            m_rawMatrices.Clear();

            // Time Model
            if (TimeModelAttributes != null) 
            {
                Tile.TimeModelAttributeIndices = new Dictionary<Tile.TimeAttributes, int>();
                int c = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (TimeModelAttributes[i])
                    {
                        Tile.TimeModelAttributeIndices.Add((Tile.TimeAttributes)i, c);
                        c++;
                    }
                }
            }

            Commands.Debug.PrintGoAheadInternals.ObjectsToPrint.Add(this);
        }

        public string DeviceName
        {
            get { return m_deviceName; }
            set { m_deviceName = value; }
        }

        private string m_deviceName = "undefined_device_name";

        public FPGATypes.FPGAFamily Family
        {
            get { return m_familiy; }
            set { m_familiy = value; }
        }

        private FPGATypes.FPGAFamily m_familiy = FPGATypes.FPGAFamily.Undefined;

        public IdentifierLookup IdentifierListLookup
        {
            get { return m_identifierLookUp; }
            set { m_identifierLookUp = value; }
        }

        private IdentifierLookup m_identifierLookUp = new IdentifierLookup();

        public Tile Current = null;

        /// <summary>
        /// The class FPGA is a Singelton
        /// </summary>
        public static FPGA Instance = new FPGA();

        /// <summary>
        /// set in device shape parser
        /// </summary>
        [DefaultValue(0)]
        public int NumberOfExpectedTiles { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(int.MinValue)]
        public int MaxX { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(FPGATypes.BackendType.ISE)]
        public FPGATypes.BackendType BackendType { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(int.MinValue)]
        public int MaxY { get; set; }

        private int m_maxTileKeyX = int.MinValue;
        private int m_maxTileKeyY = int.MinValue;

        #region Time Model
        private List<bool> TimeModelAttributes = null;
        public void AddTimeModel()
        {
            TimeModelAttributes = new List<bool>
            {
                Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute1) > -1,
                Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute2) > -1,
                Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute3) > -1,
                Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute4) > -1
            };
        }

        private List<string> TimeModelAttributesNames = null;
        public List<string> GetTimeModelAttributesNames
        {
            get
            {
                if (TimeModelAttributesNames == null)
                {
                    TimeModelAttributesNames = new List<string>
                    {
                        Tile.TimeAttributes.Attribute1.ToString(),
                        Tile.TimeAttributes.Attribute2.ToString(),
                        Tile.TimeAttributes.Attribute3.ToString(),
                        Tile.TimeAttributes.Attribute4.ToString()
                    };
                }
                return TimeModelAttributesNames;
            }
        }

        public string GetTimeModelAttributeName(Tile.TimeAttributes attribute)
        {
            return GetTimeModelAttributesNames[(int)attribute];
        }

        public void SetTimeModelAttributeName(Tile.TimeAttributes attribute, string newName)
        {
            GetTimeModelAttributesNames[(int)attribute] = newName;
        }

        public Tile.TimeAttributes GetTimeAttribute(string name)
        {
            for (int i = 0; i < 4; i++) 
            {
                if (GetTimeModelAttributesNames[i] == name)
                {
                    return (Tile.TimeAttributes)i;
                }
            }

            throw new ArgumentException("Attribute " + name + " not found");
        }
        #endregion

        public Dictionary<string, TileKey> m_location2TileKey = new Dictionary<string, TileKey>();
        public Dictionary<string, TileKey> m_sliceName2TileKey = new Dictionary<string, TileKey>();
        public TileSet m_tiles = new TileSet();

        public Dictionary<int, SwitchMatrix> m_matrices = new Dictionary<int, SwitchMatrix>();
        public Dictionary<int, WireList> m_wires = new Dictionary<int, WireList>();
        public Dictionary<int, WireList> m_incomingWires = new Dictionary<int, WireList>();
        public Dictionary<int, InPortOutPortMapping> m_inOutPortMappings = new Dictionary<int, InPortOutPortMapping>();

        public Dictionary<uint, Pair<uint, uint>> portPairs = new Dictionary<uint, Pair<uint, uint>>();

        private Queue<RawTile> m_rawTiles = new Queue<RawTile>();
        private Queue<RawSwitchMatrix> m_rawMatrices = new Queue<RawSwitchMatrix>();
    }
}