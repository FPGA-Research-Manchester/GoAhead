using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Mute the output of commands")]
    class MuteCommandTrace : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.MuteCommandTrace = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.MuteCommandTrace = false;
        }
    }
}
