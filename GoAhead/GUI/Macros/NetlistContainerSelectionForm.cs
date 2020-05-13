using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Objects;
using GoAhead.Commands;
using GoAhead.Settings;
using GoAhead.Commands.Data;

namespace GoAhead.GUI
{
    public abstract partial class NetlistContainerForm : Form
    {
        protected NetlistContainerForm()
        {
            InitializeComponent();

            // add all macros and check the current 
            foreach (Objects.NetlistContainer next in NetlistContainerManager.Instance.NetlistContainer)
            {
                m_chkListMacros.Items.Add(next.Name, false);
            }

            // pre select if there is only one item
            if (m_chkListMacros.Items.Count == 1)
            {
                m_chkListMacros.SetItemChecked(0, true);
            }

            m_chkXDLIncludePorts.Checked = StoredPreferences.Instance.XDL_IncludePortStatements;
            m_chkRunFEScript.Checked = StoredPreferences.Instance.XDL_RunFEScript;

            StoredPreferences.Instance.GUISettings.Open(this);

            m_fileSelectionCtrl.RestorePreviousSelection();
        }

        private void m_btnGenerate_Click(object sender, EventArgs e)
        {
            List<string> netlistContainerNames = new List<string>();
            foreach (object obj in m_chkListMacros.CheckedItems)
            {
                netlistContainerNames.Add(obj.ToString());
            }

            if (m_checkFileExistence && !System.IO.File.Exists(m_fileSelectionCtrl.FileName))
            {
                MessageBox.Show("File " + m_fileSelectionCtrl.FileName + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DoGeneration(m_fileSelectionCtrl.FileName, netlistContainerNames))
            {
                // save settings
                StoredPreferences.Instance.XDL_IncludePortStatements = m_chkXDLIncludePorts.Checked;
                StoredPreferences.Instance.XDL_RunFEScript = m_chkRunFEScript.Checked ;

                Close();
            }
        }
        
        public FileSelectionCtrl FileSelectionCtrl
        {
            get { return m_fileSelectionCtrl; }
            set { m_fileSelectionCtrl = value; }
        }

        protected abstract bool DoGeneration(string fileName, List<string> selectedMacros);

        protected string m_title;
        protected string m_filter;
        protected bool m_checkFileExistence = true;

        private void MacroSelectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoredPreferences.Instance.GUISettings.Close(this);
        }
    }

    public class LaunchFEGUI : NetlistContainerForm
    {
        public LaunchFEGUI()
            :base()
        {
            Text = "Launch Macro in FPGA-Editor";
            m_btnGenerate.Text = "&Launch";
            m_checkFileExistence = false;
            FileSelectionCtrl.Visible = false;
        }

        protected override bool DoGeneration(string fileName, List<string> selectedMacros)
        {
            CommandExecuter.Instance.Execute(
                new LanchNetlistInFE(selectedMacros, "", m_chkXDLIncludePorts.Checked, m_chkXDLIncludeDummyNets.Checked, m_chkRunFEScript.Checked, true));

            return true;
        }
    }
}
