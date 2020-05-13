using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands.BlockingShared;

namespace GoAhead.GUI
{
    public partial class PortSelectionForm : Form
    {
        public PortSelectionForm()
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);

            Init();
        }

        void Init()
        {
            // collect tile type by prefix to reduce tile count in second loop
            Dictionary<string, Tile> prefices = new Dictionary<string, Tile>();
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles())
            {
                string[] atoms = Regex.Split(tile.Location, "_X"); ;
                string prefix = atoms[0];

                if (!prefices.ContainsKey(prefix))
                {
                    prefices.Add(prefix, tile);
                }
            }


            SortedDictionary<string, bool> ports = new SortedDictionary<string, bool>();

            foreach (Tile tile in prefices.Values)
            {
                foreach (Port p in tile.SwitchMatrix.Ports)
                {
                    if ( (m_chkkInvert.Checked &&  (!Regex.IsMatch(p.ToString(), m_txtFilter.Text))) ||
                         (!m_chkkInvert.Checked &&  Regex.IsMatch(p.ToString(), m_txtFilter.Text)))
                    {
                        if(!ports.ContainsKey(p.ToString()))
                        {
                            ports.Add(p.ToString(), false);
                        }
                    }
                }
            }

            // clear and fill left box
            m_lstAvailablePorts.Items.Clear();
            foreach (string s in ports.Keys)
            {
                m_lstAvailablePorts.Items.Add(s);
            }

            // clear right box
            //this.m_lstSelectedPorts.Items.Clear();

            comboBox1.Items.Add("Free");
            foreach(string val in Enum.GetNames(typeof(Tile.BlockReason)))
            {
                comboBox1.Items.Add(val);
            }
        }

        private void m_btnReset_Click(object sender, EventArgs e)
        {
            m_txtFilter.Text = "";
            Init();
        }

        private void m_btnAdd_Click(object sender, EventArgs e)
        {
            foreach (object o in m_lstAvailablePorts.SelectedItems)
            {
                if (!m_lstSelectedPorts.Items.Contains(o.ToString()))
                {
                    m_lstSelectedPorts.Items.Add(o.ToString());
                }
            }   
        }

        private void m_btnRemove_Click(object sender, EventArgs e)
        {
            foreach(object o in m_lstSelectedPorts.SelectedItems)
            {
                m_lstSelectedPorts.Items.Remove(o.ToString());
            }
        }

        private void m_btnBlock_Click(object sender, EventArgs e)
        {
            foreach (object o in m_lstSelectedPorts.Items)
            {
                SetPortUsageInSelection cmd = new SetPortUsageInSelection
                {
                    PortName = o.ToString(),
                    IncludeReachablePorts = m_chkIncludeAllPorts.Checked,
                    CheckForExistence = false,
                    PortUsage = comboBox1.SelectedItem.ToString()
                };

                Commands.CommandExecuter.Instance.Execute(cmd);
            }

            Close();
        }

        private void m_btnRemovePortsFromNets_Click(object sender, EventArgs e)
        {
            foreach (object o in m_lstSelectedPorts.Items)
            {
                Commands.CommandExecuter.Instance.Execute(new Commands.Sets.RemoveArcs(o.ToString()));
            }
            Close();
        }

  

        private void m_txtFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool test = Regex.IsMatch("test", m_txtFilter.Text);
                m_lblRegexpError.Text = "";
                Init();
            }
            catch (Exception error)
            {
                string errorMessage = "No valid regular expression given: " + error.Message;
                m_lblRegexpError.Text = errorMessage;
                //MessageBox.Show(errorMessage);
            }
        }



        private void PortSelectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
