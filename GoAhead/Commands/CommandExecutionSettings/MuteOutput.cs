using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Mute the output of commands")]
    class MuteOutput : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.MuteOutput = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.MuteOutput = false;
        }
    }
}
