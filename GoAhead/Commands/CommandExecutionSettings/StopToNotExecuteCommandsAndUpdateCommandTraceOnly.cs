using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    class StopToNotExecuteCommandsAndUpdateCommandTraceOnly : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.DoNotExecuteCommandAndUpdateCommandTraceOnly = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.DoNotExecuteCommandAndUpdateCommandTraceOnly = true;
        }
    }
}
