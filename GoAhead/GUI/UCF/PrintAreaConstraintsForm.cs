using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Settings;
using GoAhead.Commands.UCF;

namespace GoAhead.GUI.UCF
{
    public partial class PrintAreaConstraintsForm : Form
    {
        public PrintAreaConstraintsForm()
        {
            InitializeComponent();

            StoredPreferences.Instance.GUISettings.Open(this);

            m_fileSelUCF.RestorePreviousSelection();
        }

        private void m_btnGenerate_Click(object sender, EventArgs e)
        {
            PrintAreaConstraint cmd = new PrintAreaConstraint();
            cmd.Append = m_fileSelUCF.Append;
            cmd.FileName = m_fileSelUCF.FileName;
            cmd.InstanceName = m_txtInstanceName.Text;

            Commands.CommandExecuter.Instance.Execute(cmd);

            Close();
        }

        private void PrintAreaConstraintsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
