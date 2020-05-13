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
            CommandStringParser parser = new CommandStringParser(Command);
            bool cmdFound = false;
            foreach(string subComd in parser.Parse())
            {
                cmdFound = true;
                Command cmd = null;
                string errorDescr;
                bool valid = parser.ParseCommand(subComd, true, out cmd, out errorDescr);
                if (valid)
                {
                    CommandExecuter.Instance.Execute(cmd);
                }
                else
                {
                    MessageBox.Show(errorDescr + " when parsing " + Command, "Error during command parsing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (!cmdFound && !string.IsNullOrEmpty(Command))
            {
                MessageBox.Show("Could not extract a command from " + Command + ". Missing semicolon after the last command?", "No commands executed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Parameter(Comment = "The name of the button")]
        public string Name = "No operation (NOP)";

        [Parameter(Comment = "The optional tool tip")]
        public string ToolTip = "";

        [Parameter(Comment = "The string representation of the GoAhead command to execute. E.g. ClearSelection;")]
        public string Command = "NOP;";

        [Parameter(Comment = "Whether this entry is static (True) or dynamic (False). This property is evaluated in the commands ClearStaticUserMenu and ClearDynamicUserMenu ")]
        public bool Static = true;
    }
}
