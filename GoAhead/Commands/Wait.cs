using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Stop execution and wait for a key input on the console", Wrapper = false, Publish = true)]
    class Wait : Command
    {
        protected override void DoCommandAction()
        {
            Console.WriteLine("# Press any key to continue");
            Console.ReadKey();
            Console.WriteLine("# GoAhead continues ...");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
