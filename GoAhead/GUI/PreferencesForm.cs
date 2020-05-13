using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Settings;
using GoAhead.Commands.CommandExecutionSettings;

namespace GoAhead.GUI
{
    public partial class PreferencesForm : Form
    {
        public PreferencesForm(Form parentForm)
        {
            InitializeComponent();

            m_parentForm = parentForm;

            m_chkExpandSelection.Checked = StoredPreferences.Instance.ExecuteExpandSelection;
            m_chkPrintWrappedCommands.Checked = CommandExecuter.Instance.PrintWrappedCommands;
            m_chkShowToolTips.Checked = StoredPreferences.Instance.ShowToolTips;
            m_chkPrintSelectionResourceInfo.Checked = StoredPreferences.Instance.PrintSelectionResourceInfo;
            m_numDropDownConsoleGUIShare.Value = (decimal)StoredPreferences.Instance.ConsoleGUIShare;
            m_numDropDownRectangleWidth.Value = (decimal)StoredPreferences.Instance.RectangleWidth;

            // get window position
            StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            AccecptSettings(true);
            Close();
        }

        private void m_btnApply_Click(object sender, EventArgs e)
        {
            AccecptSettings(false);
        }

        private void AccecptSettings(bool closeForm)
        {
            StoredPreferences.Instance.ExecuteExpandSelection = m_chkExpandSelection.Checked;
            StoredPreferences.Instance.ShowToolTips = m_chkShowToolTips.Checked;
            StoredPreferences.Instance.PrintSelectionResourceInfo = m_chkPrintSelectionResourceInfo.Checked;
            StoredPreferences.Instance.ConsoleGUIShare = (double)m_numDropDownConsoleGUIShare.Value;
            StoredPreferences.Instance.RectangleWidth = (float)m_numDropDownRectangleWidth.Value;
            StoredPreferences.SavePrefernces();

            if (m_chkPrintWrappedCommands.Checked)
            {
                CommandExecuter.Instance.Execute(new PrintWrappedCommands());
            }
            else
            {
                CommandExecuter.Instance.Execute(new StopToPrintWrappedCommands());
            }

            m_parentForm.Invalidate();

            if (closeForm)
            {
                Close();
            }
        }

        private void m_btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private readonly Form m_parentForm;

        private void PreferencesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // store window position
            StoredPreferences.Instance.GUISettings.Close(this);
        }

        private void m_numDropDownConsoleGUIShare_ValueChanged(object sender, EventArgs e)
        {
            StoredPreferences.Instance.ConsoleGUIShare = (double)m_numDropDownConsoleGUIShare.Value;
            m_parentForm.Invalidate();
        }
    }
}
