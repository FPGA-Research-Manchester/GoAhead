using System;
using System.Collections.Generic;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Do nothing")]
	class NOP : Command
	{
        protected override void DoCommandAction() 
        {
        }

		public override void Undo() 
        { 
        }
	}
}


