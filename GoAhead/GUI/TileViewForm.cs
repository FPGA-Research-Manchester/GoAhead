using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Search;
using GoAhead.CodeGeneration;
using GoAhead.Objects;

namespace GoAhead.GUI
{
    public partial class TileViewForm : Form
    {
        public TileViewForm(Tile tile)
        {
            InitializeComponent();

            this.m_tile = tile;

            this.InitText();
            this.InitSwitchMatrix();
            this.InitWires();

            Settings.Preferences.Instance.GUISettings.Open(this);
        }

        private void InitText()
        {
            this.Text = this.m_tile.Location;
            for (int i = 0; i < this.m_tile.Slices.Count; i++)
            {
                this.m_txtBox.AppendText("Slice Usage is: " + this.m_tile.Slices[i].Usage + Environment.NewLine);
                this.m_txtBox.AppendText(XDLFile.GetCode(this.m_tile.Slices[i]));
            }

            if (!FPGA.FPGA.Instance.ContainsSwitchMatrix(this.m_tile.SwitchMatrixHashCode))
            {
                return;
            }

            this.m_txtBox.AppendText(this.m_tile.SwitchMatrix.ToString());

            this.m_txtBox.AppendText("Blocked Ports" + Environment.NewLine);

            foreach (Port p in this.m_tile.GetAllBlockedPorts())
            {
                this.m_txtBox.AppendText(p.ToString() + Environment.NewLine);
            }
            
            this.m_txtBox.AppendText("Tile Hash Code: " + this.m_tile.GetHashCode() + Environment.NewLine);
            this.m_txtBox.AppendText("Wire List Hase Code: " + this.m_tile.WireListHashCode + Environment.NewLine);
            this.m_txtBox.AppendText("Switch Matrix Code: " + this.m_tile.SwitchMatrix.GetHashCode() + Environment.NewLine);

            // scroll up
            this.m_txtBox.SelectionStart = 0;
            this.m_txtBox.ScrollToCaret();
        }

        /// <summary>
        /// 2nd Tab
        /// </summary>
        private void InitSwitchMatrix()
        {
            this.m_grdViewSwitchMatrix.Rows.Clear();

            if (Regex.IsMatch(this.m_tile.Location, Objects.IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.CLBRegex)))
            {
                foreach (PortTriplet triplet in FPGA.FPGATypes.GetPortTriplets(this.m_tile))
                {
                    String port1 = "";
                    String port2 = "";
                    String port3 = "";
                    if (triplet.Port1 != null)
                    {
                        port1 = triplet.Port1.Name;
                    }
                    if (triplet.Port2 != null)
                    {
                        port2 = triplet.Port2.Name;
                    }
                    if (triplet.Port3 != null)
                    {
                        port3 = triplet.Port3.Name;
                    }

                    this.m_grdViewSwitchMatrix.Rows.Add(port1, port2, port3);
                }
            }
            else 
            {
                foreach (KeyValuePair<Port, Port> arc in this.m_tile.SwitchMatrix.GetAllArcs())
                {
                    this.m_grdViewSwitchMatrix.Rows.Add(arc.Key.Name, arc.Value.Name, "");
                }
            }
        }

        /// <summary>
        /// 3rd Tab
        /// </summary>
        private void InitWires()
        {
            if (!FPGA.FPGA.Instance.ContainsWireList(this.m_tile.WireListHashCode))
            {
                return;
            }
            this.m_grdViewWires.Rows.Clear();
            
            foreach (Wire wire in this.m_tile.WireList.GetAllWires())
            {
                Tile target = Navigator.GetDestinationByWire(this.m_tile, wire);

                this.m_grdViewWires.Rows.Add(wire.LocalPip, wire.LocalPipIsDriver, wire.PipOnOtherTile, wire.XIncr, wire.YIncr, target);
            }
        }

        private readonly Tile m_tile;

        private void TileView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Preferences.Instance.GUISettings.Close(this);
        }
    }
}
