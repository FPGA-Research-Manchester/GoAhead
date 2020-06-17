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
            foreach (Object o in PrintGoAheadInternals.ObjectsToPrint)
            {
                this.OutputManager.WriteOutput(o.GetType().Name);
                this.OutputManager.WriteOutput(o.ToString());
                this.OutputManager.WriteOutput("------------------------------------------------------");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public static List<Object> ObjectsToPrint = new List<Object>();
    }
}
