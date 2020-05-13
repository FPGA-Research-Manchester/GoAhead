using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.Selection;

namespace GoAhead.GUI
{
    public partial class UserSelectionForm : Form
    {
        public UserSelectionForm(bool clear, bool store)
        {
            m_clear = clear;
            m_store = store;
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            if (m_clear)
            {
                ClearUserSelection cmd = new ClearUserSelection();
                cmd.UserSelectionType = m_txtName.Text;
                Commands.CommandExecuter.Instance.Execute(cmd);
            }

            if (m_store)
            {
                StoreCurrentSelectionAs cmd = new StoreCurrentSelectionAs();
                cmd.UserSelectionType = m_txtName.Text;
                Commands.CommandExecuter.Instance.Execute(cmd);
            }

            Close();
        }

        /// <summary>
        /// wether to clear the user selection
        /// </summary>
        private readonly bool m_clear;

        /// <summary>
        /// wether to define the user selection
        /// </summary>
        private readonly bool m_store;

        private void UserSelectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }

}
