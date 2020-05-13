using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Profile all following commands", Wrapper = false, Publish = true)]
    class ProfileAll : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.ProfileAll = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.ProfileAll = false;
        }
    }
}
