using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Suspend the execution of GoAhead for the given number of miliseconds", Wrapper = false, Publish = true)]
    class Sleep : Command
    {
        protected override void DoCommandAction()
        {
            System.Threading.Thread.Sleep(this.Miliseconds);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The number of miliseconds to sleep")]
        public int Miliseconds = 1000;
    }
}
