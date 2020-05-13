using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Unmute the output of commands")]
    class UnmuteCommandTrace : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.MuteCommandTrace = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.MuteCommandTrace = true;
        }
    }
}
