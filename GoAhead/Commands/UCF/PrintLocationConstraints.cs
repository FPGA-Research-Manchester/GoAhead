using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.UCF
{
    [CommandDescription(Description="Print UCF (ISE) or TCL (Vivado) placement constraints for all instantiated library elements", Wrapper=true)]
    class PrintLocationConstraints : UCFCommand
    {
        protected override void DoCommandAction()
        {
            // UCF/TCL location constraints
            foreach (LibElemInst inst in Objects.LibraryElementInstanceManager.Instance.GetAllInstantiations().Where(i => Regex.IsMatch(i.InstanceName, this.InstantiationFilter)))
            {
                LibraryElement libEl = Objects.Library.Instance.GetElement(inst.LibraryElementName);

                PrintLocationConstraint getLoc = new PrintLocationConstraint();
                getLoc.Location = inst.AnchorLocation;
                getLoc.SliceNumber = inst.SliceNumber;
                getLoc.InstanceName = this.HierarchyPrefix + inst.InstanceName;
                getLoc.BEL = libEl.BEL;
                getLoc.Mute = this.Mute;
                CommandExecuter.Instance.Execute(getLoc);

                // copy output
                if (getLoc.OutputManager.HasUCFOutput)
                {
                    this.OutputManager.WriteWrapperOutput(getLoc.OutputManager.GetUCFOuput());
                }
            }
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "Only consider those macro instantiations with this prefix")]
        public String InstantiationFilter = "";

        [Parameter(Comment = "The prefix to insert before each instance name. The prefix can be used to insert hierarchies, e.g. partial_subsystem/")]
        public String HierarchyPrefix = "";
    }
}
