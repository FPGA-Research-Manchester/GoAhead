using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Stop to profile all commands", Wrapper = false, Publish = true)]
    class StopToProfileAll : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.ProfileAll = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.ProfileAll = true;
        }
    }
}
