using System;
using System.Collections.Generic;
using System.Linq;
using GoAhead.FPGA.Slices;

namespace GoAhead.FPGA
{
    [Serializable]
    public class RawSlice
    {
        public RawSlice(Slice slice)
        {
            X = slice.X;
            Y = slice.Y;
            Name = slice.SliceName;
            Type = slice.SliceType;
            InPortOutPortMappingHasCode = slice.InPortOutPortMappingHashCode;
            Bels = new List<string>();
            foreach (string s in slice.Bels)
            {
                Bels.Add(s);
            }
        }

        public readonly int X;
        public readonly int Y;
        public readonly string Name;
        public readonly string Type;
        public readonly int InPortOutPortMappingHasCode;
        public readonly List<string> Bels;
    }

    [Serializable]
    public abstract class Slice
    {
        public static Slice GetSliceFromRawSlice(Tile containingTile, RawSlice rawSlice)
        {
            Slice s = GetSlice(containingTile, rawSlice.Name, rawSlice.Type);

            s.m_X = rawSlice.X;
            s.m_Y = rawSlice.Y;
            s.Usage = FPGATypes.SliceUsage.Free;
            s.InPortOutPortMappingHashCode = rawSlice.InPortOutPortMappingHasCode;
            s.m_attributes.Clear();
            // do not call init here to save memory
            // if a slices attribute is get or set, the InitAttributes will be called
            //s.InitAttributes();

            s.ExtractXYCoordindates();
            // vivado onyl
            if (rawSlice.Bels != null)
            {
                foreach (string b in rawSlice.Bels)
                {
                    s.AddBel(b);
                }
            }

            return s;
        }

        public static Slice GetSlice(Tile containingTile, string sliceName, string sliceType)
        {
            Slice slice = null;
            if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex2))
            {
                slice = new V2Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex4))
            {
                slice = new V4Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex5))
            {
                slice = new V5Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex6))
            {
                slice = new V6Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Kintex7))
            {
                slice = new K7Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Artix7))
            {
                slice = new ASlice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Spartan3))
            {
                slice = new S3Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Spartan6))
            {
                slice = new S6Slice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Zynq))
            {
                slice = new ZSlice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.UltraScale))
            {
                slice = new USSlice(containingTile, sliceName, sliceType);
            }
            else if (FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Virtex7))
            {
                slice = new V7Slice(containingTile, sliceName, sliceType);
            }
            else
            {
                slice = new S6Slice(containingTile, sliceName, sliceType);
                Console.WriteLine("Warning: SliceParser.Parse: Unsupported family: " + FPGA.Instance.Family + " (continue with S6)");
            }
            return slice;
        }

        public Slice(Tile containingTile, string name, string type)
        {
            m_containingTile = containingTile;
            m_name = name;
            m_type = type;
            Usage = FPGATypes.SliceUsage.Free;
            m_attributes.Clear();
            //this.InitAttributes();

            ExtractXYCoordindates();
        }

        private void ExtractXYCoordindates()
        {
            int x, y;

            // location in of form LEFTPARTX\d+Y\d+
            if (FPGATypes.GetXYFromIdentifier(m_name, out x, out y))
            {
                m_X = x;
                m_Y = y;
            }
        }

        public abstract void InitAttributes();

        protected virtual string ModifySetting(Tuple<string, string> nextTupel)
        {
            return nextTupel.Item1 + ":" + nextTupel.Item2;
        }

        public override string ToString()
        {
            return m_name;
        }

        public Tile ContainingTile
        {
            get { return m_containingTile; }
        }

        public string SliceName
        {
            get { return m_name; }
        }

        public string SliceType
        {
            get { return m_type; }
        }

        public int X
        {
            get { return m_X; }
        }

        public int Y
        {
            get { return m_Y; }
        }

        public IEnumerable<string> GetAllAttributeValues()
        {
            // called on empty slice?
            if (m_attributes.Count == 0)
            {
                InitAttributes();
            }

            foreach (KeyValuePair<string, string> nextTupel in m_attributes)
            {
                yield return ModifySetting(new Tuple<string, string>(nextTupel.Key, nextTupel.Value));
            }
        }

        public IEnumerable<string> GetAllAttributes()
        {
            // called on empty slice?
            if (m_attributes.Count == 0)
            {
                InitAttributes();
            }

            foreach (KeyValuePair<string, string> nextTupel in m_attributes)
            {
                yield return nextTupel.Key;
            }
        }

        public void SetAttributeValue(string attribute, string value)
        {
            // called on empty slice?
            if (m_attributes.Count == 0)
            {
                InitAttributes();
            }

            if (!m_attributes.ContainsKey(attribute))
                throw new ArgumentException("Attribute " + attribute + " not known on slice " + SliceName);

            m_attributes[attribute] = value;
        }

        public string GetAttributeValue(string attribute)
        {
            // called on empty slice?
            if (m_attributes.Count == 0)
            {
                InitAttributes();
            }

            if (!m_attributes.ContainsKey(attribute))
                throw new ArgumentException("Attribute " + attribute + " not known on slice " + SliceName);

            return m_attributes[attribute];
        }

        public InPortOutPortMapping PortMapping
        {
            get { return FPGA.Instance.GetInPortOutPortMapping(InPortOutPortMappingHashCode); }
        }

        public void ClearAttributes()
        {
            m_attributes.Clear();
        }

        public void Reset()
        {
            m_attributes.Clear();
            Usage = FPGATypes.SliceUsage.Free;
            //this.InitAttributes();
        }

        public void AddBel(string bel)
        {
            if (!m_bels.ContainsKey(bel))
            {
                m_bels.Add(bel, FPGATypes.SliceUsage.Free);
            }
        }

        public IEnumerable<string> Bels
        {
            get { return m_bels.Keys.AsEnumerable(); }
        }

        public void SetBelUsage(string bel, FPGATypes.SliceUsage usage)
        {
            if (bel != null)
            {
                m_bels[bel] = usage;
            }
        }

        public FPGATypes.SliceUsage GetBelUsage(string bel)
        {
            return m_bels[bel];
        }


        private Dictionary<string, FPGATypes.SliceUsage> m_bels = new Dictionary<string, FPGATypes.SliceUsage>();

        /// <summary>
        /// A slice can have different usages (e.g Free, Macro)
        /// </summary>
        public FPGATypes.SliceUsage Usage { get; set; }

        private int m_X = 0;
        private int m_Y = 0;

        /// <summary>
        /// The port mappings are shared and therefore stored in FPGA
        /// </summary>
        public int InPortOutPortMappingHashCode { get; set; }

        protected readonly string m_name;
        protected readonly string m_type;
        protected readonly Tile m_containingTile;
        protected Dictionary<string, string> m_attributes = new Dictionary<string, string>();

        //protected Dictionary<String, List<String>> m_possibleSettings = null; //new Dictionary<String, List<String>>();
    }
}