using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Unmute the GUI error messages of commands")]
    class UnmuteGUIErrorMessages : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.MuteGUIErrorMessages = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.MuteGUIErrorMessages = true;
        }
    }
}
