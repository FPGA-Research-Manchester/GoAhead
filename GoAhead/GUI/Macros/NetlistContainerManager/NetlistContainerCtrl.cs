using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using GoAhead.Objects;
using GoAhead.Commands;
using GoAhead.Commands.Debug;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Settings;
using GoAhead.GUI.Macros.DesignBrowser;

namespace GoAhead.GUI
{
    public partial class NetlistContainerCtrl : UserControl
    {
        public NetlistContainerCtrl()
        {
            InitializeComponent();

            CutOff cutCmd = new CutOff();
            m_lblCutOff.Text = cutCmd.GetCommandDescription();

            PrintStatistics printCmd = new PrintStatistics();
            m_lblStatistics.Text = printCmd.GetCommandDescription();

            OpenDesign readCmd = new OpenDesign();
            m_lblRead.Text = readCmd.GetCommandDescription();

            FuseNets fuseCmd = new FuseNets();
            m_lblFuse.Text = fuseCmd.GetCommandDescription();
        }

        private void m_btnAdd_Click(object sender, EventArgs e)
        {
            if (m_txtMacroName.Text.Length == 0)
            {
                MessageBox.Show("Netlist container name given", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                CommandExecuter.Instance.Execute(new AddNetlistContainer(m_txtMacroName.Text));
            }
        }

        private void m_btnCutOff_Click(object sender, EventArgs e)
        {
            CutOff cutCmd = new CutOff();
            cutCmd.NetlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;
            CommandExecuter.Instance.Execute(cutCmd);
        }   

        private void m_btnMacroStatistics_Click(object sender, EventArgs e)
        {                            
            PrintStatistics printCmd = new PrintStatistics();
            printCmd.NetlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;
            printCmd.PrintAntennas = true;
            printCmd.NetNameLimit = int.MaxValue;

            CommandExecuter.Instance.Execute(printCmd);
        }

        private void m_btnReadDesign_Click(object sender, EventArgs e)
        {
            string caller = "m_btnReadDesignIntoMacro_Click";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an XDL File";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "XDL File|*.xdl";

            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(openFileDialog.FileName));

            OpenDesign readCmd = new OpenDesign();
            readCmd.FileName = openFileDialog.FileName;
            readCmd.NetlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;
            CommandExecuter.Instance.Execute(readCmd);

            Invalidate();
        }

        private void m_btnFuse_Click(object sender, EventArgs e)
        {
            FuseNets fuseCmd = new FuseNets();
            fuseCmd.NetlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;
            fuseCmd.PrintProgress = true;
            CommandExecuter.Instance.Execute(fuseCmd);
        }

        private void m_btnDesignBrowser_Click(object sender, EventArgs e)
        {
            DesignBrowserForm frm = new DesignBrowserForm(m_netlistContainerSelector.SelectedNetlistContainerName);
            frm.Show();
        }             
    }
}
