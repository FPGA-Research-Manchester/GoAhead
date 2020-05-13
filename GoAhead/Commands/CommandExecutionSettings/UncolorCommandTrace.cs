using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Do no longer color the command trace")]
    class UncolorCommandTrace : Command
    {
        protected override void DoCommandAction()
        {
            CommandExecuter.Instance.ColorOutput = false;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.ColorOutput = true;
        }
    }
}
