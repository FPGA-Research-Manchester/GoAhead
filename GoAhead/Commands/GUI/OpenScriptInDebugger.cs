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
            if(!System.IO.File.Exists(this.FileName))
            {
                throw new ArgumentException("Script " + this.FileName + " does not exist");
            }

            if (this.m_form == null)
            {
                // prevent tracing this command twice
                this.UpdateCommandTrace = false;
                // postpone execution until GUI comes up
                ShowGUI.GUICommands.Add(this);
            }
            else
            {
                ScriptDebuggerForm dlg = new ScriptDebuggerForm(this.m_form, this.FileName);
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
        public String FileName = "script.goa";
    }
}
