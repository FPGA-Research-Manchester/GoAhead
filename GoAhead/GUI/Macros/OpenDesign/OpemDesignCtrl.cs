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
    public partial class ReadDesignCtrl : UserControl
    {
        public ReadDesignCtrl()
        {
            InitializeComponent();

            m_fileSelCtrl.RestorePreviousSelection();
            m_fileSelCtrl.DisableAppendCheckBox();
        }

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            if(!File.Exists(m_fileSelCtrl.FileName))
            {
                MessageBox.Show(m_fileSelCtrl.FileName + " does not exist", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenDesign readCmd = new OpenDesign();
            readCmd.NetlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName;
            readCmd.FileName = m_fileSelCtrl.FileName;

            CommandExecuter.Instance.Execute(readCmd);

            if (ParentForm != null)
            {
                ParentForm.Close();
            }
        }
    }
}
