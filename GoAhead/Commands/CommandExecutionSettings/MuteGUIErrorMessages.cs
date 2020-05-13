using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Mute the GUI error messages of commands")]
    class MuteGUIErrorMessages : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.MuteGUIErrorMessages = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.MuteGUIErrorMessages = false;
        }
    }
}
