using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Selection.Anchor
{
    class PrintRelativeAddToSelectionXY : Command
    {        
       protected override void DoCommandAction()
        {
            AddToSelectionXY addCmd = new AddToSelectionXY(UpperLeftX, UpperLeftY, LowerRightX, LowerRightY);
            OutputManager.WriteOutput(addCmd.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The X coordinate of the upper left tile")]
        public int UpperLeftX;
        [Parameter(Comment = "The Y coordinate of the upper left tile")]
        public int UpperLeftY;
        [Parameter(Comment = "The X coordinate of the lower right tile")]
        public int LowerRightX;
        [Parameter(Comment = "The Y coordinate of the lower right tile")]
        public int LowerRightY;


    }
}
