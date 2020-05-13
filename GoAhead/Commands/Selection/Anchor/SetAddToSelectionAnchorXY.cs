using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection.Anchor
{
    [CommandDescription(Description = "Set the selection anchor. All AddToSelectionXY command will have values of X1, X2, Y1 and Y2 relative to the given anchor", Wrapper = true)]
    class SetAddToSelectionAnchorXY : SetAddToSelectionAnchorCommand
    {
       protected override void DoCommandAction()
        {
            if (!FPGA.FPGA.Instance.Contains(X, Y))
            {
                throw new ArgumentException("Tile not found: " + ToString());
            }

            Tile anchor = FPGA.FPGA.Instance.GetTile(X, Y);
            SetAnchor(anchor);
        }

        public override void Undo()
        {
            throw new NotImplementedException("Not implemented");
        }

        [Parameter(Comment = "The X coordinate of the selection anchor")]
        public int X = 0;

        [Parameter(Comment = "The Y coordinate of the selection anchor")]
        public int Y = 0;

    }
}
