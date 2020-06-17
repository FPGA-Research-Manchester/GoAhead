using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.GUI
{
    class DrawLine : Command
    {
        protected override void DoCommandAction()
        {
            LineManager.Orienation orienation = LineManager.Orienation.Undefined;
            if(this.Orientation.Equals("H"))
            {
                orienation = LineManager.Orienation.Horizontal;
            }
            else if(this.Orientation.Equals("V"))
            {
                orienation = LineManager.Orienation.Vertical;
            }
            else
            {
                throw new ArgumentException("The paramter Orientation only supports either H or V");
            }
            LineManager.Instance.AddSetting(this.FamilyRegexp, orienation, this.Offset, this.IdentifierRegexp);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The device name for which to draw lines")]
        public String FamilyRegexp = "Spartan6";

        [Parameter(Comment = "Whether to draw a vertical (V) or an horizontsl (H) line")]
        public String Orientation = "H";

        [Parameter(Comment = "The number of tiles to move the line: If Orienatation=H (V) the line is moved up (left) if Offset < 0 or down (right) if Offset > 0")]
        public int Offset = 0;

        [Parameter(Comment = "Draw a horizontal line beside each tile whose identifer matches this regular expression")]
        public String IdentifierRegexp = "^HCLK";
    }
}
