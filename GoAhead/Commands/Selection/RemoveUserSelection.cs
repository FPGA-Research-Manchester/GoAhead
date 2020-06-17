using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Remove a before saved user selection (see also StoreCurrentSelectionAs)", Wrapper = true)]
    class RemoveUserSelection : Command
    {
        protected override void DoCommandAction()
        {
            if (!FPGA.TileSelectionManager.Instance.HasUserSelection(this.UserSelectionType))
            {
                throw new ArgumentException("User selection type " + this.UserSelectionType + " does not exist");
            }

            FPGA.TileSelectionManager.Instance.RemoveUserSelection(this.UserSelectionType);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the user selection type")]
        public String UserSelectionType = "PartialArea";
    }
}
