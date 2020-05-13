using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Commands.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GoAhead.GUI.PartGen
{
    public partial class PartGenAllCtrl : UserControl
    {
        public PartGenAllCtrl()
        {
            InitializeComponent();

            m_numDrpMaxGregreeofParallelism.Minimum = 1;
            m_numDrpMaxGregreeofParallelism.Maximum = Environment.ProcessorCount;

            FillDeviceList(".*");
        }

        private void FillDeviceList(string filter)
        {
            m_lstBoxDevices.Items.Clear();

            foreach (string package in PartGenAll.GetAllPackages().Where(p => Regex.IsMatch(p, filter)))
            {
                m_lstBoxDevices.Items.Add(package);
            }
        }

        private void m_btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowNewFolderButton = true;

            // cancel
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(dlg.SelectedPath))
                return;

            m_txtPath.Text = dlg.SelectedPath;
        }

        private string GetFPGAFilter()
        {
            List<string> filters = new List<string>();
            if (m_chkBoxKintex7.Checked)
            {
                filters.Add("xc7");
            }
            if (m_chkBoxSpartan3.Checked)
            {
                filters.Add("xc3s");
            } 
            if (m_chkBoxSpartan6.Checked)
            {
                filters.Add("xc6s");
            } 
            if (m_chkBoxVirtex4.Checked)
            {
                filters.Add("xc4v");
            } 
            if (m_chkBoxVirtex5.Checked)
            {
                filters.Add("xc5v");
            } 
            if (m_chkBoxVirtex6.Checked)
            {
                filters.Add("xc6v");
            }

            foreach (object obj in m_lstBoxDevices.SelectedItems)
            {
                filters.Add(obj.ToString() + "$");
            }

            // build up astring for a regexp
            string regexp = "";
            foreach (string f in filters)
            {
                string fInBrackets = "(^" + f + ")";
                if (string.IsNullOrEmpty(regexp))
                {
                    regexp += fInBrackets;
                }
                else
                {
                    regexp += "|" + fInBrackets;
                }
            }
            return regexp;
        }

        private void m_btnOk_Click(object sender, EventArgs e)
        {
            string filter = GetFPGAFilter();

            if (!m_txtFilter.Text.Equals(filter))
            {
                filter = m_txtFilter.Text;
            }

            if (string.IsNullOrEmpty(filter))
            {
                MessageBox.Show("No FPGA families selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Directory.Exists(m_txtPath.Text))
            {
                MessageBox.Show("Store path " + m_txtPath.Text + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            PartGenAll partGenCmd = new PartGenAll();
            partGenCmd.FPGAFilter = filter;
            partGenCmd.KeepExisting_binFPGAS = m_chkBoxKeepBinFPGAs.Checked;
            partGenCmd.KeepXDLFiles = m_chkBoxKeepXDL.Checked;
            partGenCmd.StorePath = m_txtPath.Text;
            partGenCmd.AllCons = m_chkBoxAllConns.Checked;
            //partGenCmd.MaxDegreeOfParallelism = (int) this.m_numDrpMaxGregreeofParallelism.Value;
            partGenCmd.ExcludePipsToBidirectionalWiresFromBlocking = m_chkBoxExcludeBirectionalWires.Checked;

            CommandExecuter.Instance.Execute(partGenCmd);
        }

        #region Filter
                
        private void m_txtDeviceFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bool test = Regex.IsMatch("test", m_txtDeviceFilter.Text);
                FillDeviceList(m_txtDeviceFilter.Text);
                m_lblDeviceFilter.Text = "Filter";
            }
            catch (Exception)
            {
                m_lblDeviceFilter.Text = "Filter (invalid)";
            }
        }
        
        private void UpdateFilter()
        {
            m_txtFilter.Text = GetFPGAFilter();
        }
        private void m_chkBoxKintex7_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void m_chkBoxVirtex6_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void m_chkBoxVirtex5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void m_chkBoxVirtex4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void m_chkBoxSpartan6_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void m_chkBoxSpartan3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void m_lstBoxDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        #endregion Filter


    }
}
