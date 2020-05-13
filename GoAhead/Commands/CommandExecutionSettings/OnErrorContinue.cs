using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Continue with the next command upon on error in batch mod")]
    class OnErrorContinue : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.OnErrorContinue = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.OnErrorContinue = false;
        }
    }
}
