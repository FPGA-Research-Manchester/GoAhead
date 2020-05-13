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
            NetlistContainer nlc = GetNetlistContainer();

            // the old net must exist
            if (!nlc.Nets.Any(n => n.Name.Equals(OldName)))
            {
                throw new ArgumentException("Could not find net " + OldName);
            }

            // net new netname may not exist
            if (nlc.Nets.Any(n => n.Name.Equals(NewName)))
            {
                throw new ArgumentException("Net " + NewName + " already used");
            }

            // capture net
            Net net = nlc.GetNet(OldName);
            net.Name = NewName;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The old name of the net to rename")]
        public string OldName = "oldName";

        [Parameter(Comment = "The new name of the net")]
        public string NewName = "newName";
    }
}
