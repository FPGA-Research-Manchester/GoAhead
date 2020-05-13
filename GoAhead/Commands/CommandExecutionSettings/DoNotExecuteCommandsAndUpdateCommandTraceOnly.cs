using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    class DoNotExecuteCommandsAndUpdateCommandTraceOnly : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.DoNotExecuteCommandAndUpdateCommandTraceOnly = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.DoNotExecuteCommandAndUpdateCommandTraceOnly = false;
        }
    }
}
