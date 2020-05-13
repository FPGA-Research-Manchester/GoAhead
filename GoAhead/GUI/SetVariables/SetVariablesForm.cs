using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.Variables;

namespace GoAhead.GUI.SetVariables
{
    public partial class SetVariablesForm : Form
    {
        public SetVariablesForm(List<Input> inputs)
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);

            m_setVariables.Inputs = inputs;

            Width = m_setVariables.Width;
            Height = m_setVariables.Height;
        }
        
        public List<Set> GetSetCommands()
        {
            return m_setVariables.GetSetCommands();
        }

        private void m_setVariables_Load(object sender, EventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }

        private void SetVariablesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Set cmd in m_setVariables.GetSetCommands())
            {
                Commands.CommandExecuter.Instance.Execute(cmd);
            }
        }
    }
}
