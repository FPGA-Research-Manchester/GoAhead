using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands
{
    class SetLabel : Command
    {
        protected override void DoCommandAction()
        {
           
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the label")]
        public string LabelName = "label_1";
    }
}
