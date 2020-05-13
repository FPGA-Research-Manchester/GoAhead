using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Identifier
{
    [CommandDescription(Description = "Set the regexp GoAhead uses to identify left and right block tiles for the given Family", Wrapper = false, Publish = true)]
    class SetOrientedBlockIdentifierRegexp : SetIdentifierCommand
    {
        [Parameter(Comment = "CLB, Interconnect or BRAM")]
        public string BlockType= "";

        [Parameter(Comment = "Left or Right")]
        public string Orientation = "";

        protected override void DoCommandAction()
        {
            bool left;
            if (Orientation.Equals("Left", StringComparison.InvariantCultureIgnoreCase))
            {
                left = true;
            }
            else if (Orientation.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
            {
                left = false;
            }
            else
            {
                throw new ArgumentException("Invalid orientation supplied");
            }

            if (BlockType.Equals("CLB", StringComparison.InvariantCultureIgnoreCase))
            {
                var t = left ? IdentifierManager.RegexTypes.CLB_left : IdentifierManager.RegexTypes.CLB_right;
                IdentifierManager.Instance.SetRegex(t, FamilyRegexp, IdentifierRegexp);
            }
            else if (BlockType.Equals("Interconnect", StringComparison.InvariantCultureIgnoreCase))
            {
                var t = left ? IdentifierManager.RegexTypes.Interconnect_left : IdentifierManager.RegexTypes.Interconnect_right;
                IdentifierManager.Instance.SetRegex(t, FamilyRegexp, IdentifierRegexp);
            }
            else if (BlockType.Equals("BRAM", StringComparison.InvariantCultureIgnoreCase))
            {
                var t = left ? IdentifierManager.RegexTypes.BRAM_left : IdentifierManager.RegexTypes.BRAM_right;
                IdentifierManager.Instance.SetRegex(t, FamilyRegexp, IdentifierRegexp);
            }
            else
            {
                throw new ArgumentException("Invalid block type supplied");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
