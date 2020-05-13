using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Commands.Library;
using GoAhead.Objects;

namespace GoAhead.GUI.AddLibraryManager
{
    internal partial class ReadDesignCtrl : UserControl
    {
        public ReadDesignCtrl()
        {
            InitializeComponent();

            this.m_fileSelCtrl.RestorePreviousSelection();
            this.m_fileSelCtrl.DisableAppendCheckBox();
        }

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            if(!File.Exists(this.m_fileSelCtrl.FileName))
            {
                MessageBox.Show(this.m_fileSelCtrl.FileName + " does not exist", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenDesign readCmd = new OpenDesign();
            readCmd.NetlistContainerName = this.m_netlistContainerSelector.SelectedNetlistContainerName;
            readCmd.FileName = this.m_fileSelCtrl.FileName;

            CommandExecuter.Instance.Execute(readCmd);

            if (this.ParentForm != null)
            {
                this.ParentForm.Close();
            }
        }
    }
}
