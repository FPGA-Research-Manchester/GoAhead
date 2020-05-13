using System;
using System.Text.RegularExpressions;

namespace GoAhead.FPGA.Slices
{
    [Serializable]
    public class S3Slice : Slice
    {
        public S3Slice(Tile containingTile, string name, string type)
            : base(containingTile, name, type)
        {
        }

        protected override string ModifySetting(Tuple<string, string> nextTupel)
        {
            if (Regex.IsMatch(nextTupel.Item1, "^(F|G)$") && !nextTupel.Item2.Equals(":#OFF"))
            {
                return nextTupel.Item1 + ":" + SliceName + "." + nextTupel.Item1 + ":#LUT:" + nextTupel.Item2;
            }
            else if (Regex.IsMatch(nextTupel.Item1, "^XOR") && !nextTupel.Item2.Equals(":#OFF"))
            {
                return nextTupel.Item1 + ":" + SliceName + "." + nextTupel.Item1 + ":";
            }
            else
            {
                return nextTupel.Item1 + ":" + nextTupel.Item2;
            }
        }

        public override void InitAttributes()
        {
            // SLICE L AND M
            m_attributes.Add("BXINV", ":#OFF");
            m_attributes.Add("BYINV", ":#OFF");
            m_attributes.Add("CEINV", ":#OFF");
            m_attributes.Add("CLKINV", ":#OFF");
            m_attributes.Add("COUTUSED", ":#OFF");
            m_attributes.Add("CY0F", ":#OFF");
            m_attributes.Add("CY0G", ":#OFF");
            m_attributes.Add("CYINIT", ":#OFF");
            m_attributes.Add("CYSELF", ":#OFF");
            m_attributes.Add("CYSELG", ":#OFF");
            m_attributes.Add("DXMUX", ":#OFF");
            m_attributes.Add("DYMUX", ":#OFF");
            m_attributes.Add("F", ":#OFF");
            m_attributes.Add("F5USED", ":#OFF");
            m_attributes.Add("FFX", ":#OFF");
            m_attributes.Add("FFX_INIT_ATTR", ":#OFF");
            m_attributes.Add("FFX_SR_ATTR", ":#OFF");
            m_attributes.Add("FFY", ":#OFF");
            m_attributes.Add("FFY_INIT_ATTR", ":#OFF");
            m_attributes.Add("FFY_SR_ATTR", ":#OFF");
            m_attributes.Add("FXMUX", ":#OFF");
            m_attributes.Add("FXUSED", ":#OFF");
            m_attributes.Add("G", ":#OFF");
            m_attributes.Add("GYMUX", ":#OFF");
            m_attributes.Add("REVUSED", ":#OFF");
            m_attributes.Add("SRINV", ":#OFF");
            m_attributes.Add("SYNC_ATTR", ":#OFF");
            m_attributes.Add("XUSED", ":#OFF");
            m_attributes.Add("YBUSED", ":#OFF");
            m_attributes.Add("YUSED", ":#OFF");
            m_attributes.Add("XORF", ":#OFF");
            m_attributes.Add("XORG", ":#OFF");

            if (SliceType.Equals("SLICEM"))
            {
                m_attributes.Add("BYINVOUTUSED", ":#OFF");
                m_attributes.Add("BYOUTUSED", ":#OFF");
                m_attributes.Add("DIF_MUX", ":#OFF");
                m_attributes.Add("DIGUSED", ":#OFF");
                m_attributes.Add("DIG_MUX", ":#OFF");
                m_attributes.Add("F_ATTR", ":#OFF");
                m_attributes.Add("G_ATTR", ":#OFF");
                m_attributes.Add("SHIFTOUTUSED", ":#OFF");
                m_attributes.Add("SLICEWE0USED", ":#OFF");
                m_attributes.Add("SLICEWE1USED", ":#OFF");
                m_attributes.Add("SRFFMUX", ":#OFF");
                m_attributes.Add("WF1USED", ":#OFF");
                m_attributes.Add("WF2USED", ":#OFF");
                m_attributes.Add("WF3USED", ":#OFF");
                m_attributes.Add("WF4USED", ":#OFF");
                m_attributes.Add("WG1USED", ":#OFF");
                m_attributes.Add("WG2USED", ":#OFF");
                m_attributes.Add("WG3USED", ":#OFF");
                m_attributes.Add("WG4USED", ":#OFF");
                m_attributes.Add("XBMUX", ":#OFF");
                m_attributes.Add("YBMUX", ":#OFF");
            }

            /*
            this.m_possibleSettings.Clear();
            foreach (String attribute in this.m_attributes.Keys)
            {
                this.m_possibleSettings.Add(attribute, new List<String>());
            }

            this.m_possibleSettings["BXINV"].Add(":#OFF");
            this.m_possibleSettings["BXINV"].Add(":BX");
            this.m_possibleSettings["BYINV"].Add(":#OFF");
            this.m_possibleSettings["BYINV"].Add(":BY");
            this.m_possibleSettings["CEINV"].Add(":#OFF");
            this.m_possibleSettings["CEINV"].Add(":#OFF");
            this.m_possibleSettings["CLKINV"].Add(":#OFF");
            this.m_possibleSettings["CLKINV"].Add(":CLK");
            this.m_possibleSettings["COUTUSED"].Add(":#OFF");
            this.m_possibleSettings["COUTUSED"].Add(":#OFF");
            this.m_possibleSettings["CY0F"].Add(":#OFF");
            this.m_possibleSettings["CY0G"].Add(":#OFF");
            this.m_possibleSettings["CYINIT"].Add(":#OFF");
            this.m_possibleSettings["CYSELF"].Add(":#OFF");
            this.m_possibleSettings["CYSELF"].Add(":#OFF");
            this.m_possibleSettings["CYSELG"].Add(":#OFF");
            this.m_possibleSettings["DXMUX"].Add(":#OFF");
            this.m_possibleSettings["DXMUX"].Add(":0");
            this.m_possibleSettings["DYMUX"].Add(":#OFF");
            this.m_possibleSettings["DYMUX"].Add(":0");
            this.m_possibleSettings["F5USED"].Add(":#OFF");
            this.m_possibleSettings["F"].Add(":#OFF");
            this.m_possibleSettings["FFX"].Add(":#OFF");
            this.m_possibleSettings["FFX"].Add(":#LATCH");
            this.m_possibleSettings["FFX"].Add(":#FF");
            this.m_possibleSettings["FFX_INIT_ATTR"].Add(":#OFF");
            this.m_possibleSettings["FFX_SR_ATTR"].Add(":#OFF");
            this.m_possibleSettings["FFY"].Add(":#OFF");
            this.m_possibleSettings["FFY"].Add(":#LATCH");
            this.m_possibleSettings["FFY"].Add(":#FF");
            this.m_possibleSettings["FFY_INIT_ATTR"].Add(":#OFF");
            this.m_possibleSettings["FFY_SR_ATTR"].Add(":#OFF");
            this.m_possibleSettings["FXMUX"].Add(":#OFF");
            this.m_possibleSettings["FXMUX"].Add(":F");
            this.m_possibleSettings["FXUSED"].Add(":#OFF");
            this.m_possibleSettings["G"].Add(":#OFF");
            this.m_possibleSettings["GYMUX"].Add(":#OFF");
            this.m_possibleSettings["GYMUX"].Add(":G");
            this.m_possibleSettings["GYMUX"].Add(":#OFF");
            this.m_possibleSettings["REVUSED"].Add(":#OFF");
            this.m_possibleSettings["REVUSED"].Add(":#OFF");
            this.m_possibleSettings["SRINV"].Add(":#OFF");
            this.m_possibleSettings["SYNC_ATTR"].Add(":#OFF");
            this.m_possibleSettings["SYNC_ATTR"].Add(":#OFF");
            this.m_possibleSettings["XUSED"].Add(":#OFF");
            this.m_possibleSettings["XUSED"].Add(":#OFF");
            this.m_possibleSettings["XUSED"].Add(":0");
            this.m_possibleSettings["YUSED"].Add(":#OFF");
            this.m_possibleSettings["YUSED"].Add(":0");

            if (this.SliceType.Equals("SLICEM"))
            {
                this.m_possibleSettings["BYINVOUTUSED"].Add(":#OFF");
                this.m_possibleSettings["BYOUTUSED"].Add(":#OFF");
                this.m_possibleSettings["DIF_MUX"].Add(":#OFF");
                this.m_possibleSettings["DIG_MUX"].Add(":#OFF");
                this.m_possibleSettings["DIGUSED"].Add(":#OFF");
                this.m_possibleSettings["F_ATTR"].Add(":#OFF");
                this.m_possibleSettings["G_ATTR"].Add(":#OFF");
                this.m_possibleSettings["SHIFTOUTUSED"].Add(":#OFF");
                this.m_possibleSettings["SLICEWE0USED"].Add(":#OFF");
                this.m_possibleSettings["SLICEWE1USED"].Add(":#OFF");
                this.m_possibleSettings["SRFFMUX"].Add(":#OFF");
                this.m_possibleSettings["WF1USED"].Add(":#OFF");
                this.m_possibleSettings["WF2USED"].Add(":#OFF");
                this.m_possibleSettings["WF3USED"].Add(":#OFF");
                this.m_possibleSettings["WF4USED"].Add(":#OFF");
                this.m_possibleSettings["WG1USED"].Add(":#OFF");
                this.m_possibleSettings["WG2USED"].Add(":#OFF");
                this.m_possibleSettings["WG3USED"].Add(":#OFF");
                this.m_possibleSettings["WG4USED"].Add(":#OFF");
                this.m_possibleSettings["XBMUX"].Add(":#OFF");
                this.m_possibleSettings["YBMUX"].Add(":#OFF");
            }
             * */
        }
    }
}