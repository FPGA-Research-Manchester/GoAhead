using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Library
{
    class PrintLibrary : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            foreach (LibraryElement el in Objects.Library.Instance.GetAllElements())
            {
                this.OutputManager.WriteOutput(el.ToString());
            }
        }

        public override void Undo()
        {
        }
    }
}
