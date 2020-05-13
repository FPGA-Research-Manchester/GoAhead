using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Clear the given netlist container", Wrapper = false, Publish = true)]
    class GC : Command
    {
        protected override void DoCommandAction()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
