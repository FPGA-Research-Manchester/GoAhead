using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Remove a previously stored user selection (see also StoreCurrentSelectionAs)", Wrapper = false, Publish = true)]
    class ClearUserSelection : Command
    {
        protected override void DoCommandAction()
        {
            FPGA.TileSelectionManager.Instance.ClearUserSelection(UserSelectionType);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
        [Parameter(Comment = "The name of the user selection")]
        public string UserSelectionType = "PartialArea";
    }
}
