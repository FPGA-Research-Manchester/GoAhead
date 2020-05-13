using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code.TCL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Fuse all nets in a netlist container", Wrapper = true, Publish = true)]
    class AddHierarchy : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.Vivado);

            TCLContainer netlistContainer = (TCLContainer) GetNetlistContainer();
            TCLDesignHierarchy hier = new TCLDesignHierarchy();
            hier.Name = Name;
            hier.Properties.SetProperty("REF_NAME", Reference, true);
            netlistContainer.Add(hier);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the hierarchy, e.g inst_PartialSubsystem")]
        public string Name = "inst_PartialSubsystem3";

        [Parameter(Comment = "The reference name for the create_cell command, e.g PartialSubsystem")]
        public string Reference = "PartialSubsystem";
    }
}
