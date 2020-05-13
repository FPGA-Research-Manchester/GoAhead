using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Profile the peak memory consumption before each command")]
    class ProfilePeakMemoryConsumption : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.ProfilePeakMemoryConsumption = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.ProfilePeakMemoryConsumption = false;
        }
    }
}
