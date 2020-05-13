using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Debug
{
    class PrintGoAheadInternals : Command
    {      
        protected override void DoCommandAction()
        {
            foreach (object o in ObjectsToPrint)
            {
                OutputManager.WriteOutput(o.GetType().Name);
                OutputManager.WriteOutput(o.ToString());
                OutputManager.WriteOutput("------------------------------------------------------");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public static List<object> ObjectsToPrint = new List<object>();
    }
}
