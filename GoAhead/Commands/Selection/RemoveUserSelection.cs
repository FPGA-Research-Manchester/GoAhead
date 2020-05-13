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
            if (!FPGA.TileSelectionManager.Instance.HasUserSelection(UserSelectionType))
            {
                throw new ArgumentException("User selection type " + UserSelectionType + " does not exist");
            }

            FPGA.TileSelectionManager.Instance.RemoveUserSelection(UserSelectionType);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the user selection type")]
        public string UserSelectionType = "PartialArea";
    }
}
