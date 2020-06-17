using System;
using System.Collections.Generic;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Store the currentl selection under a name which can later be used to refer to this selection (see also SelectUserSelection)", Wrapper = false, Publish = true)]
    class StoreCurrentSelectionAs : Command
    {
        protected override void DoCommandAction()
        {
            FPGA.TileSelectionManager.Instance.AddCurrentSelectionToUserSelection(this.UserSelectionType);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the user selection type")]
        public String UserSelectionType = "PartialArea";
    }
}