using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection.Anchor
{
    [CommandDescription(Description = "Set the selection anchor. All AddToSelectionXY command will have values of X1, X2, Y1 and Y2 relative to the given anchor", Wrapper = false)]
    class SetAddToSelectionAnchorLoc : SetAddToSelectionAnchorCommand
    {
       protected override void DoCommandAction()
        {
            if (!FPGA.FPGA.Instance.Contains(Location))
            {
                throw new ArgumentException("Tile not found: " + ToString());
            }

            Tile anchor = FPGA.FPGA.Instance.GetTile(Location);
            SetAnchor(anchor);
        }

        public override void Undo()
        {
            throw new NotImplementedException("Not implemented");
        }

        [Parameter(Comment = "The identifier string of the tile where to set the anchor, e.g INT_X10Y24")]
        public string Location = "INT_X10Y24";
    }
}
