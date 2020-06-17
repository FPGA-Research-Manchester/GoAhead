using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print all currently loaded library elements", Wrapper = false)]
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
