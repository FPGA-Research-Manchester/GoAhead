using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.Commands.Debug;
using GoAhead.Objects;

namespace GoAhead.GUI
{
    public partial class TileViewCtrl : UserControl
    {
        public TileViewCtrl(Tile tile)
        {
            InitializeComponent();

            m_tile = tile;

            InitText();
            InitSwitchMatrix();
            InitWires();
        }

        private void InitText()
        {
            Text = m_tile.Location;

            PrintTileInfo printTileInfoCommand = new PrintTileInfo();
            printTileInfoCommand.Location = m_tile.Location;
            printTileInfoCommand.Do();

            m_txtBox.AppendText(printTileInfoCommand.OutputManager.GetOutput());

            // scroll up
            m_txtBox.SelectionStart = 0;
            m_txtBox.ScrollToCaret();
        }

        /// <summary>
        /// 2nd Tab
        /// </summary>
        private void InitSwitchMatrix()
        {
            if (m_grdViewSwitchMatrix.Columns.Count == 0)
            {
                List<DataGridViewColumn> columns = new List<DataGridViewColumn> { m_in, m_out};
                bool[] atts = Tile.GetPresentTimeAttributes();
                if (atts != null && atts.Contains(true))
                {
                    columns.AddRange(GetTimingColumns());
                    m_cbxShowTimes.Visible = true;
                    m_lblShowTimes.Visible = true;
                }
                m_grdViewSwitchMatrix.Columns.AddRange(columns.ToArray());
            }            

            m_grdViewSwitchMatrix.Rows.Clear();

            if (!FPGA.FPGA.Instance.ContainsSwitchMatrix(m_tile.SwitchMatrixHashCode))
            {
                return;
            }

            GetFilter(m_txtInFilter.Text, out Regex inFilter, out bool inFilterValid);
            m_lblInFilterValid.Text = inFilterValid ? "" : "Invalid regular expression";

            GetFilter(m_txtOutFilter.Text, out Regex outFilter, out bool outFilterValid);
            m_lblOutFilterValid.Text = outFilterValid ? "" : "Invalid regular expression";

            if (!inFilterValid || !outFilterValid) 
            {
                return;
            }

            foreach (Tuple<Port, Port> arc in m_tile.SwitchMatrix.GetAllArcs().Where(a => inFilter.IsMatch(a.Item1.Name) && outFilter.IsMatch(a.Item2.Name)))
            {
                // Default entries
                List<object> entries = new List<object>
                {
                    arc.Item1.Name,
                    arc.Item2.Name
                };

                // If time data is present
                List<float> times = m_tile.GetTimeData(arc.Item1, arc.Item2);
                if (times != null)
                {
                    int index = -1;

                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute1);
                    if (index > -1) entries.Add(times[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute2);
                    if (index > -1) entries.Add(times[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute3);
                    if (index > -1) entries.Add(times[index]);
                    index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute4);
                    if (index > -1) entries.Add(times[index]);
                }

                m_grdViewSwitchMatrix.Rows.Add(entries.ToArray());
            }

            InitLUTRouting();
        }

        private void InitLUTRouting()
        {
            m_grdViewLUTRouting.Rows.Clear();

            // LUT routing requires wire lists
            if (FPGA.FPGA.Instance.WireListCount == 0)
            {
                return;
            }
            if (IdentifierManager.Instance.IsMatch(m_tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                Regex filter1 = null;
                Regex filter2 = null;
                Regex filter3 = null;
                Regex filter4 = null;
                bool filter1Valid = false;
                bool filter2Valid = false;
                bool filter3Valid = false;
                bool filter4Valid = false;
                GetFilter(m_txtLRLutOutFilter.Text, out filter1, out filter1Valid);
                GetFilter(m_txtLREndFilter.Text, out filter2, out filter2Valid);
                GetFilter(m_txtLRBegFilter.Text, out filter3, out filter3Valid);
                GetFilter(m_txtLRLUTInFilter.Text, out filter4, out filter4Valid);

                if (!filter1Valid || !filter2Valid || !filter3Valid || !filter4Valid)
                {
                    return;
                }

                foreach (LUTRoutingInfo info in FPGATypes.GetLUTRouting(m_tile))
                {
                    string port1 = info.Port1 != null ? info.Port1.Name : "";
                    string port2 = info.Port2 != null ? info.Port2.Name : "";
                    string port3 = info.Port3 != null ? info.Port3.Name : "";
                    string port4 = info.Port4 != null ? info.Port4.Name : "";

                    if (filter1.IsMatch(port1) && filter2.IsMatch(port2) && filter3.IsMatch(port3) && filter4.IsMatch(port4))
                    {
                        m_grdViewLUTRouting.Rows.Add(port1, port2, port3, port4);
                    }
                }
            }
        }

        private void GetFilter(string regexp, out Regex filter, out bool valid)
        {
            filter = null;
            valid = false;

            if (string.IsNullOrEmpty(regexp))
            {
                filter = new Regex("", RegexOptions.Compiled);
                valid = true;
            }
            else
            {
                valid = true;
                try
                {
                    filter = new Regex(regexp, RegexOptions.Compiled);
                }
                catch (Exception)
                {
                    valid = false;
                }
            }
        }

        /// <summary>
        /// 3rd Tab
        /// </summary>
        private void InitWires()
        {
            if (!FPGA.FPGA.Instance.ContainsWireList(m_tile.WireListHashCode))
            {
                return;
            }
            m_grdViewWires.Rows.Clear();

            foreach (Wire wire in m_tile.WireList)
            {
                Tile target = Navigator.GetDestinationByWire(m_tile, wire);

                m_grdViewWires.Rows.Add(wire.LocalPip, wire.LocalPipIsDriver, wire.PipOnOtherTile, wire.XIncr, wire.YIncr, target);
            }
        }

        private DataGridViewColumn[] GetTimingColumns()
        {
            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            timing_attribute1.HeaderText = FPGA.FPGA.Instance.GetTimeModelAttributeName(Tile.TimeAttributes.Attribute1);
            timing_attribute2.HeaderText = FPGA.FPGA.Instance.GetTimeModelAttributeName(Tile.TimeAttributes.Attribute2);
            timing_attribute3.HeaderText = FPGA.FPGA.Instance.GetTimeModelAttributeName(Tile.TimeAttributes.Attribute3);
            timing_attribute4.HeaderText = FPGA.FPGA.Instance.GetTimeModelAttributeName(Tile.TimeAttributes.Attribute4);

            int index = -1;
            index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute1);
            if (index > -1) columns.Add(timing_attribute1);
            index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute2);
            if (index > -1) columns.Add(timing_attribute2);
            index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute3);
            if (index > -1) columns.Add(timing_attribute3);
            index = Tile.GetIndexForTimeAttribute(Tile.TimeAttributes.Attribute4);
            if (index > -1) columns.Add(timing_attribute4);

            return columns.ToArray();
        }

        private void m_txtInFilter_TextChanged(object sender, EventArgs e)
        {
            InitSwitchMatrix();
        }

        private void m_txtOutFilter_TextChanged(object sender, EventArgs e)
        {
            InitSwitchMatrix();
        }

        private void m_txtLRLutOutFilter_TextChanged(object sender, EventArgs e)
        {
            InitLUTRouting();
        }

        private void m_txtLREndFilter_TextChanged(object sender, EventArgs e)
        {
            InitLUTRouting();
        }

        private void m_txtLRBegFilter_TextChanged(object sender, EventArgs e)
        {
            InitLUTRouting();
        }

        private void m_txtLRLUTInFilter_TextChanged(object sender, EventArgs e)
        {
            InitLUTRouting();
        }

        private void m_tabSwitchMatrix_Resize(object sender, EventArgs e)
        {
            m_grdViewSwitchMatrix.Width = m_tabSwitchMatrix.Width - 7;
            m_grdViewSwitchMatrix.Top = m_grpFilter.Height + 5;
            m_grdViewSwitchMatrix.Height = (m_tabSwitchMatrix.Height - m_grpFilter.Height) - 7;
        }

        private readonly Tile m_tile;

        private void CbxShowTimes_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 2; i < m_grdViewSwitchMatrix.Columns.Count; i++)
            {
                m_grdViewSwitchMatrix.Columns[i].Visible = m_cbxShowTimes.Checked;
            }
        }
    }
}
