using System;
using System.Text.RegularExpressions;

namespace GoAhead.FPGA.Slices
{
    [Serializable]
    public class S6Slice : Slice
    {
        public S6Slice(Tile containingTile, string name, string type)
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
            if (Regex.IsMatch(ContainingTile.Location, "BRAMSITE"))
            {
                /*
                this.m_attributes.Add("CLKAINV", ":OFF");
                this.m_attributes.Add("CLKBINV", ":OFF");
                this.m_attributes.Add("DATA_WIDTH_A", ":OFF");
                this.m_attributes.Add("DATA_WIDTH_B", ":OFF");
                this.m_attributes.Add("DOA_REG", ":OFF");
                this.m_attributes.Add("DOB_REG", ":OFF");
                this.m_attributes.Add("ENAINV", ":OFF");
                this.m_attributes.Add("ENBINV", ":OFF");
                this.m_attributes.Add("EN_RSTRAM_A", ":OFF");
                this.m_attributes.Add("EN_RSTRAM_B", ":OFF");
                this.m_attributes.Add("RAM_MODE", ":OFF");
                this.m_attributes.Add("REGCEAINV", ":OFF");
                this.m_attributes.Add("REGCEBINV", ":OFF");
                this.m_attributes.Add("RSTAINV", ":OFF");
                this.m_attributes.Add("RSTBINV", ":OFF");
                this.m_attributes.Add("RSTTYPE", ":OFF");
                this.m_attributes.Add("RST_PRIORITY_A", ":OFF");
                this.m_attributes.Add("RST_PRIORITY_B", ":OFF");
                this.m_attributes.Add("WEA0INV", ":OFF");
                this.m_attributes.Add("WEA1INV", ":OFF");
                this.m_attributes.Add("WEA2INV", ":OFF");
                this.m_attributes.Add("WEA3INV", ":OFF");
                this.m_attributes.Add("WEB0INV", ":OFF");
                this.m_attributes.Add("WEB1INV", ":OFF");
                this.m_attributes.Add("WEB2INV", ":OFF");
                this.m_attributes.Add("WEB3INV", ":OFF");
                this.m_attributes.Add("WRITE_MODE_A", ":OFF");
                this.m_attributes.Add("WRITE_MODE_B", ":OFF");
                this.m_attributes.Add("RAMB16BWER:$Blocker_BRAM_" + this.SliceName + ".RAMB16BWER:", ":OFF");
                 */
            }
            else if (Regex.IsMatch(ContainingTile.Location, "CLEX"))
            {
                m_attributes.Add("A5FFSRINIT", ":#OFF");
                m_attributes.Add("A5LUT", ":#OFF");
                m_attributes.Add("A6LUT", ":#OFF");
                m_attributes.Add("AFF", ":#OFF");
                m_attributes.Add("AFFMUX", ":#OFF");
                m_attributes.Add("AFFSRINIT", ":#OFF");
                m_attributes.Add("AOUTMUX", ":#OFF");
                m_attributes.Add("AUSED", ":#OFF");
                m_attributes.Add("B5FFSRINIT", ":#OFF");
                m_attributes.Add("B5LUT", ":#OFF");
                m_attributes.Add("B6LUT", ":#OFF");
                m_attributes.Add("BFF", ":#OFF");
                m_attributes.Add("BFFMUX", ":#OFF");
                m_attributes.Add("BFFSRINIT", ":#OFF");
                m_attributes.Add("BOUTMUX", ":#OFF");
                m_attributes.Add("BUSED", ":#OFF");
                m_attributes.Add("C5FFSRINIT", ":#OFF");
                m_attributes.Add("C5LUT", ":#OFF");
                m_attributes.Add("C6LUT", ":#OFF");
                m_attributes.Add("CEUSED", ":#OFF");
                m_attributes.Add("CFF", ":#OFF");
                m_attributes.Add("CFFMUX", ":#OFF");
                m_attributes.Add("CFFSRINIT", ":#OFF");
                m_attributes.Add("CLKINV", ":#OFF");
                m_attributes.Add("COUTMUX", ":#OFF");
                m_attributes.Add("CUSED", ":#OFF");
                m_attributes.Add("D5FFSRINIT", ":#OFF");
                m_attributes.Add("D5LUT", ":#OFF");
                m_attributes.Add("D6LUT", ":#OFF");
                m_attributes.Add("DFF", ":#OFF");
                m_attributes.Add("DFFMUX", ":#OFF");
                m_attributes.Add("DFFSRINIT", ":#OFF");
                m_attributes.Add("DOUTMUX", ":#OFF");
                m_attributes.Add("DUSED", ":#OFF");
                m_attributes.Add("SRUSED", ":#OFF");
                m_attributes.Add("SYNC_ATTR", ":#OFF");
            }
            else
            {
                // do not add attributes
            }
        }
    }
}