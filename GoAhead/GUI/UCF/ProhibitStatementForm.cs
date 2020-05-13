using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.UCF;
using GoAhead.GUI;
using GoAhead.Settings;

namespace GoAhead.GUI.UCF
{
    public partial class ProhibitStatementForm : Form
    {
        public ProhibitStatementForm()
        {
            InitializeComponent();

            StoredPreferences.Instance.GUISettings.Open(this);

            m_fileSelUCF.RestorePreviousSelection();
        }

        private void m_btnGenerate_Click(object sender, EventArgs e)
        {
            PrintProhibitStatementsForSelection cmd = new PrintProhibitStatementsForSelection();
            cmd.Append = m_fileSelUCF.Append;
            cmd.ExcludeUsedSlices = m_chkExclude.Checked;
            cmd.FileName = m_fileSelUCF.FileName;

            Commands.CommandExecuter.Instance.Execute(cmd);

            Close();
        }

        private void ProhibitStatementForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
