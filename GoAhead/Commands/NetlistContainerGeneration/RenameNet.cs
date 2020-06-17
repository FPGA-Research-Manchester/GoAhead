using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Rename a net in a netlist container", Wrapper = false, Publish = true)]
    class RenameNet : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = this.GetNetlistContainer();

            // the old net must exist
            if (!nlc.Nets.Any(n => n.Name.Equals(this.OldName)))
            {
                throw new ArgumentException("Could not find net " + this.OldName);
            }

            // net new netname may not exist
            if (nlc.Nets.Any(n => n.Name.Equals(this.NewName)))
            {
                throw new ArgumentException("Net " + this.NewName + " already used");
            }

            // capture net
            Net net = nlc.GetNet(this.OldName);
            net.Name = this.NewName;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The old name of the net to rename")]
        public String OldName = "oldName";

        [Parameter(Comment = "The new name of the net")]
        public String NewName = "newName";
    }
}
