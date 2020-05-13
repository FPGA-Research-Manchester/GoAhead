using System;
using System.Text.RegularExpressions;

namespace GoAhead.FPGA.Slices
{
    [Serializable]
    public class V7Slice : Slice
    {
        public V7Slice(Tile containingTile, string name, string type)
            : base(containingTile, name, type)
        {
        }

        protected override string ModifySetting(Tuple<string, string> nextTupel)
        {
            if (Regex.IsMatch(nextTupel.Item1, "^[A-D][5-6]LUT$|^[A-D]FF$") && !nextTupel.Item2.Equals(":#OFF"))
            {
                // A6LUT:replace_with_instance_name.A6LUT:#LUT:O6=A6 OR
                // AFF:replace_with_instance_name.AFF:#FF
                return nextTupel.Item1 + ":" + SliceName + "." + nextTupel.Item1 + ":" + nextTupel.Item2;
            }
            else
            {
                return nextTupel.Item1 + ":" + nextTupel.Item2;
            }
        }

        public override void InitAttributes()
        {
            if (!Regex.IsMatch(m_type, "SLICE"))
                return;
            bool memory = m_type.Equals("SLICEM");

            m_attributes.Add("A5FFINIT", ":#OFF");
            m_attributes.Add("A5FFMUX", ":#OFF");
            m_attributes.Add("A5FFSR", ":#OFF");
            m_attributes.Add("A5LUT", ":#OFF");
            m_attributes.Add("A5RAMMODE", ":#OFF");
            m_attributes.Add("A6LUT", ":#OFF");
            m_attributes.Add("A6RAMMODE", ":#OFF");
            m_attributes.Add("ACY0", ":#OFF");
            m_attributes.Add("ADI1MUX", ":#OFF");
            m_attributes.Add("AFF", ":#OFF");
            m_attributes.Add("AFFINIT", ":#OFF");
            m_attributes.Add("AFFMUX", ":#OFF");
            m_attributes.Add("AFFSR", ":#OFF");
            m_attributes.Add("AOUTMUX", ":#OFF");
            m_attributes.Add("AUSED", ":#OFF");
            m_attributes.Add("B5FFINIT", ":#OFF");
            m_attributes.Add("B5FFMUX", ":#OFF");
            m_attributes.Add("B5FFSR", ":#OFF");
            m_attributes.Add("B5LUT", ":#OFF");
            m_attributes.Add("B5RAMMODE", ":#OFF");
            m_attributes.Add("B6LUT", ":#OFF");
            m_attributes.Add("B6RAMMODE", ":#OFF");
            m_attributes.Add("BCY0", ":#OFF");
            m_attributes.Add("BDI1MUX", ":#OFF");
            m_attributes.Add("BFF", ":#OFF");
            m_attributes.Add("BFFINIT", ":#OFF");
            m_attributes.Add("BFFMUX", ":#OFF");
            m_attributes.Add("BFFSR", ":#OFF");
            m_attributes.Add("BOUTMUX", ":#OFF");
            m_attributes.Add("BUSED", ":#OFF");
            m_attributes.Add("C5FFINIT", ":#OFF");
            m_attributes.Add("C5FFMUX", ":#OFF");
            m_attributes.Add("C5FFSR", ":#OFF");
            m_attributes.Add("C5LUT", ":#OFF");
            m_attributes.Add("C5RAMMODE", ":#OFF");
            m_attributes.Add("C6LUT", ":#OFF");
            m_attributes.Add("C6RAMMODE", ":#OFF");
            m_attributes.Add("CCY0", ":#OFF");
            m_attributes.Add("CDI1MUX", ":#OFF");
            m_attributes.Add("CEUSEDMUX", ":#OFF");
            m_attributes.Add("CFF", ":#OFF");
            m_attributes.Add("CFFINIT", ":#OFF");
            m_attributes.Add("CFFMUX", ":#OFF");
            m_attributes.Add("CFFSR", ":#OFF");
            m_attributes.Add("CLKINV", ":#OFF");
            m_attributes.Add("COUTMUX", ":#OFF");
            m_attributes.Add("COUTUSED", ":#OFF");
            m_attributes.Add("CUSED", ":#OFF");
            m_attributes.Add("D5FFINIT", ":#OFF");
            m_attributes.Add("D5FFMUX", ":#OFF");
            m_attributes.Add("D5FFSR", ":#OFF");
            m_attributes.Add("D5LUT", ":#OFF");
            m_attributes.Add("D5RAMMODE", ":#OFF");
            m_attributes.Add("D6LUT", ":#OFF");
            m_attributes.Add("D6RAMMODE", ":#OFF");
            m_attributes.Add("DCY0", ":#OFF");
            m_attributes.Add("DFF", ":#OFF");
            m_attributes.Add("DFFINIT", ":#OFF");
            m_attributes.Add("DFFMUX", ":#OFF");
            m_attributes.Add("DFFSR", ":#OFF");
            m_attributes.Add("DOUTMUX", ":#OFF");
            m_attributes.Add("DUSED", ":#OFF");
            m_attributes.Add("PRECYINIT", ":#OFF");
            m_attributes.Add("SRUSEDMUX", ":#OFF");
            m_attributes.Add("SYNC_ATTR", ":#OFF");
            m_attributes.Add("WA7USED", ":#OFF");
            m_attributes.Add("WA8USED", ":#OFF");
            m_attributes.Add("WEMUX", ":#OFF");
        }
    }
}