using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    abstract class GotoCommand : Command
    {
        public abstract bool JumpToLabel();

        [Parameter(Comment = "The name of the label")]
        public string LabelName = "label_1";
    }
}
