using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.GUI.ScriptDebugger;

namespace GoAhead.Commands.GUI
{
    class OpenScriptInDebugger : GUICommand
    {
        protected override void DoCommandAction()
        {
            if(!System.IO.File.Exists(FileName))
            {
                throw new ArgumentException("Script " + FileName + " does not exist");
            }

            if (m_form == null)
            {
                // prevent tracing this command twice
                UpdateCommandTrace = false;
                // postpone execution until GUI comes up
                ShowGUI.GUICommands.Add(this);
            }
            else
            {
                ScriptDebuggerForm dlg = new ScriptDebuggerForm(m_form, FileName);
                dlg.Show();
            }
        }


        public Form FormToInvalidate
        {
            get { return m_form; }
            set { m_form = value; }
        }

        private Form m_form = null;

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the script to open in the debugger")]
        public string FileName = "script.goa";
    }
}
