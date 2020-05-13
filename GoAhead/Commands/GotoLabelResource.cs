using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands
{
    class GotoLabelResource : GotoCommand
    {
        protected override void DoCommandAction()
        {
            // nothing todo, jumping is done in command execution context
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        public override bool JumpToLabel()
        {
            return TileSelectionManager.Instance.GetSelectedTiles().Any(t => Regex.IsMatch(t.Location, Condition));
        }

        [Parameter(Comment = "The conidition is considered as true it at least any of the currently selected tiles matches this regular expression")]
        public string Condition = "(BRAM)|(DSP)";
    }
}
