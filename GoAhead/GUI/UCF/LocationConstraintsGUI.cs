using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands;
using GoAhead.Settings;

namespace GoAhead.GUI.UCF
{
    public partial class LocationConstraintsGUI : Form
    {
        public LocationConstraintsGUI()
        {
            InitializeComponent();

            StoredPreferences.Instance.GUISettings.Open(this);

            m_fileSelCtrlUCF.RestorePreviousSelection();
        }

        private void m_btnGenerate_Click(object sender, EventArgs e)
        {
            Commands.UCF.PrintLocationConstraintsForSelection cmd = new GoAhead.Commands.UCF.PrintLocationConstraintsForSelection();
            cmd.Append = m_fileSelCtrlUCF.Append;
            cmd.FileName = m_fileSelCtrlUCF.Text;
            cmd.InstanceName = m_txtInstanceName.Text;
            cmd.SliceNumber = (int)m_numSliecIndex.Value;

            CommandExecuter.Instance.Execute(cmd);

            Close();
        }

        private void LocationConstraintsGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoredPreferences.Instance.GUISettings.Close(this);
        }
        /*
        private void m_btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Select an UCF File";
            saveFileDialog.Filter = "UCF File|*.ucf";

            String caller = "LocationConstraintsGUI.m_btnBrowse_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                saveFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            // cancel
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (saveFileDialog.FileName.Equals(""))
                return;

            this.m_txtFilename.Text = saveFileDialog.FileName;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(saveFileDialog.FileName));
        }*/
   
    }
}
