using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.CommandExecutionSettings
{
    [CommandDescription(Description = "Color the command trace")]
    class ColorCommandTrace : Command
    {
       protected override void DoCommandAction()
        {
            CommandExecuter.Instance.ColorOutput = true;
        }

        public override void Undo()
        {
            CommandExecuter.Instance.ColorOutput = false;
        }
    }
}
