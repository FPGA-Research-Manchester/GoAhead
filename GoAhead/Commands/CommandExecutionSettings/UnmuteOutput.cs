using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description="Unmute the output of commands")]
    class UnmuteOutput : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.MuteOutput = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.MuteOutput = true;
        }
    }
}
