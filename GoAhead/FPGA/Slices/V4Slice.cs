using System;
using System.Text.RegularExpressions;

namespace GoAhead.FPGA.Slices
{
    [Serializable]
    public class V4Slice : Slice
    {
        public V4Slice(Tile containingTile, string name, string type)
            : base(containingTile, name, type)
        {
            /* Slice order in FE
             3
             1
            2
            0
             */
        }

        protected override string ModifySetting(Tuple<string, string> nextTupel)
        {
            //if (Regex.IsMatch(nextTupel.Key, "^F|F$|^[A-D]FF$") && !nextTupel.Value.Equals(":#OFF"))
            if (Regex.IsMatch(nextTupel.Item1, "(^(F|G)$)|(^FF(X|Y))|(^CYMUX(F|G))") && !nextTupel.Item2.Equals(":OFF"))
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
            // for non CLB slice types, do nothing
            if (!Regex.IsMatch(m_type, "SLICE"))
                return;

            // in V4 there are 2 slice types in CLBs
            bool memory = m_type.Equals("SLICEM");
            bool logic = m_type.Equals("SLICEL");

            if (memory)
            {
                m_attributes.Add("BXINV", ":OFF");
                m_attributes.Add("BYINV", ":OFF");
                m_attributes.Add("BYINVOUTUSED", ":OFF");
                m_attributes.Add("BYOUTUSED", ":OFF");
                m_attributes.Add("CEINV", ":OFF");
                m_attributes.Add("CLKINV", ":OFF");
                m_attributes.Add("COUTUSED", ":OFF");
                m_attributes.Add("CY0F", ":OFF");
                m_attributes.Add("CY0G", ":OFF");
                m_attributes.Add("CYINIT", ":OFF");
                m_attributes.Add("CYMUXF", ":OFF"); // manually added
                m_attributes.Add("CYMUXG", ":OFF"); // manually added
                m_attributes.Add("DIF_MUX", ":OFF");
                m_attributes.Add("DIGUSED", ":OFF");
                m_attributes.Add("DIG_MUX", ":OFF");
                m_attributes.Add("DXMUX", ":OFF");
                m_attributes.Add("DYMUX", ":OFF");
                m_attributes.Add("F", ":OFF");
                m_attributes.Add("F5USED", ":OFF");
                m_attributes.Add("FFX", ":OFF");
                m_attributes.Add("FFX_INIT_ATTR", ":OFF");
                m_attributes.Add("FFX_SR_ATTR", ":OFF");
                m_attributes.Add("FFY", ":OFF");
                m_attributes.Add("FFY_INIT_ATTR", ":OFF");
                m_attributes.Add("FFY_SR_ATTR", ":OFF");
                m_attributes.Add("FXMUX", ":OFF");
                m_attributes.Add("FXUSED", ":OFF");
                m_attributes.Add("F_ATTR", ":OFF");
                m_attributes.Add("G", ":OFF");
                m_attributes.Add("GYMUX", ":OFF");
                m_attributes.Add("G_ATTR", ":OFF");
                m_attributes.Add("REVUSED", ":OFF");
                m_attributes.Add("SHIFTOUTUSED", ":OFF");
                m_attributes.Add("SLICEWE0USED", ":OFF");
                m_attributes.Add("SLICEWE1USED", ":OFF");
                m_attributes.Add("SRFFMUX", ":OFF");
                m_attributes.Add("SRINV", ":OFF");
                m_attributes.Add("SYNC_ATTR", ":OFF");
                m_attributes.Add("WF1USED", ":OFF");
                m_attributes.Add("WF2USED", ":OFF");
                m_attributes.Add("WF3USED", ":OFF");
                m_attributes.Add("WF4USED", ":OFF");
                m_attributes.Add("WG1USED", ":OFF");
                m_attributes.Add("WG2USED", ":OFF");
                m_attributes.Add("WG3USED", ":OFF");
                m_attributes.Add("WG4USED", ":OFF");
                m_attributes.Add("XBMUX", ":OFF");
                m_attributes.Add("XBUSED", ":OFF");
                m_attributes.Add("XMUXUSED", ":OFF");
                m_attributes.Add("XUSED", ":OFF");
                m_attributes.Add("YBMUX", ":OFF");
                m_attributes.Add("YBUSED", ":OFF");
                m_attributes.Add("YMUXUSED", ":OFF");
                m_attributes.Add("YUSED", ":OFF");
            }

            if (logic)
            {
                m_attributes.Add("BXINV", ":OFF");
                m_attributes.Add("BYINV", ":OFF");
                m_attributes.Add("CEINV", ":OFF");
                m_attributes.Add("CLKINV", ":OFF");
                m_attributes.Add("COUTUSED", ":OFF");
                m_attributes.Add("CYMUXF", ":OFF"); // manually added
                m_attributes.Add("CYMUXG", ":OFF"); // manually added
                m_attributes.Add("CY0F", ":OFF");
                m_attributes.Add("CY0G", ":OFF");
                m_attributes.Add("CYINIT", ":OFF");
                m_attributes.Add("DXMUX", ":OFF");
                m_attributes.Add("DYMUX", ":OFF");
                m_attributes.Add("F", ":OFF");
                m_attributes.Add("F5USED", ":OFF");
                m_attributes.Add("FFX", ":OFF");
                m_attributes.Add("FFX_INIT_ATTR", ":OFF");
                m_attributes.Add("FFX_SR_ATTR", ":OFF");
                m_attributes.Add("FFY", ":OFF");
                m_attributes.Add("FFY_INIT_ATTR", ":OFF");
                m_attributes.Add("FFY_SR_ATTR", ":OFF");
                m_attributes.Add("FXMUX", ":OFF");
                m_attributes.Add("FXUSED", ":OFF");
                m_attributes.Add("G", ":OFF");
                m_attributes.Add("GYMUX", ":OFF");
                m_attributes.Add("REVUSED", ":OFF");
                m_attributes.Add("SRINV", ":OFF");
                m_attributes.Add("SYNC_ATTR", ":OFF");
                m_attributes.Add("XBUSED", ":OFF");
                m_attributes.Add("XMUXUSED", ":OFF");
                m_attributes.Add("XUSED", ":OFF");
                m_attributes.Add("YBUSED", ":OFF");
                m_attributes.Add("YMUXUSED", ":OFF");
                m_attributes.Add("YUSED", ":OFF");
            }
        }
    }
}