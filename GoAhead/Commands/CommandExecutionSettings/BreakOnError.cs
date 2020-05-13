using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{    
    [CommandDescription(Description = "Upon an error in batch mode, stop the execution ")]
    class BreakOnError : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.OnErrorContinue = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.OnErrorContinue = true;
        }
    }
}
