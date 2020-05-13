using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Stop to profile the peak memory consumption before each command")]
    class StopToProfilePeakMemoryConsumption : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.ProfilePeakMemoryConsumption = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.ProfilePeakMemoryConsumption = true;
        }
    }
}
