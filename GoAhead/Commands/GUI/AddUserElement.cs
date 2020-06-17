using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.Commands.GUI
{
    abstract class AddUserElement : GUICommand
    {
        protected void UserDefinedAction(object sender, EventArgs e)
        {
            CommandStringParser parser = new CommandStringParser(this.Command);
            bool cmdFound = false;
            foreach(String subComd in parser.Parse())
            {
                cmdFound = true;
                Command cmd = null;
                String errorDescr;
                bool valid = parser.ParseCommand(subComd, true, out cmd, out errorDescr);
                if (valid)
                {
                    CommandExecuter.Instance.Execute(cmd);
                }
                else
                {
                    MessageBox.Show(errorDescr + " when parsing " + this.Command, "Error during command parsing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (!cmdFound && !String.IsNullOrEmpty(this.Command))
            {
                MessageBox.Show("Could not extract a command from " + this.Command + ". Missing semicolon after the last command?", "No commands executed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Parameter(Comment = "The name of the button")]
        public String Name = "No operation (NOP)";

        [Parameter(Comment = "The optional tool tip")]
        public String ToolTip = "";

        [Parameter(Comment = "The string representation of the GoAhead command to execute. E.g. ClearSelection;")]
        public String Command = "NOP;";

        [Parameter(Comment = "Whether this entry is static (True) or dynamic (False). This property is evaluated in the commands ClearStaticUserMenu and ClearDynamicUserMenu ")]
        public bool Static = true;
    }
}
