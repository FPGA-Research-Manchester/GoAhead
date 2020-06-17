using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print the name of all instantiated library elements", Wrapper = false)]
    class PrintInstantiations : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            foreach (LibElemInst inst in Objects.LibraryElementInstanceManager.Instance.GetAllInstantiations())
            {
                this.OutputManager.WriteOutput(inst.ToString());
            }
        }

        public override void Undo()
        {
        }
    }
}
