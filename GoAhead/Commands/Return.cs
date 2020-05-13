using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Stop executing the commands (e.g., a script)", Wrapper = true)]
    class Return : Command
    {
        protected override void DoCommandAction()
        {
            // nothing todo, stopping the current command list is done in command execution context
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
