using GoAhead.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.TCL.Procs.cshelp
{
    class CsHelpMenuItem : MenuItem
    {
        private readonly DataGridViewRow TargetRow = null;

        public CsHelpMenuItem(string text, DataGridViewRow row) : base(text)
        {
            TargetRow = row;
            Click += SendRowToTerminal;
        }

        private void SendRowToTerminal(object sender, EventArgs e)
        {
            if (TargetRow == null) return;

            CsHelpLayoutPanel panel = (CsHelpLayoutPanel)TargetRow.DataGridView.Parent;
            if (panel == null) return;

            object cell0 = TargetRow.Cells[0].Value;
            if (cell0 == null) return;

            string result = "";

            switch(TargetRow.DataGridView.Name)
            {
                case CsHelp_GUI.CSH_METHODS:
                    result += cell0.ToString();
                    ParameterInfo[] parameters = panel.GetMethodInfoAtRow(TargetRow).GetParameters();
                    for(int i=0; i<parameters.Length; i++)
                    {
                        result += " " + TclAPI.GetStringLabelForType(parameters[i].ParameterType);
                    }
                    break;
                default:
                    result += cell0.ToString();
                    break;
            }

            RichTextBox terminal = ConsoleCtrl.TCLTerminal_input;
            terminal.AppendText(result);
            terminal.Focus();
        }
    }
}
