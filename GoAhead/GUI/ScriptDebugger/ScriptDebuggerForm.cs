using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.ScriptDebugger
{
    public partial class ScriptDebuggerForm : Form
    {
        public ScriptDebuggerForm(Form invalidateMeAfterEachCommand)
        {
            InitializeComponent();

            m_scriptDebuggerCtrl.InvalidateMeAfterEachCommand = invalidateMeAfterEachCommand;
            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        public ScriptDebuggerForm(Form invalidateMeAfterEachCommand, string scriptToLoadAtStartUp)
        {
            InitializeComponent();

            m_scriptDebuggerCtrl.InvalidateMeAfterEachCommand = invalidateMeAfterEachCommand;
            m_scriptDebuggerCtrl.ScriptToLoadAtStartup = scriptToLoadAtStartUp;

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void ScriptDebuggerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_scriptDebuggerCtrl.Close();
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
