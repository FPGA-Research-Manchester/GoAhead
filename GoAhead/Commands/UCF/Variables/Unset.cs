using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.Variables
{
    [CommandDescription(Description = "Unset a variable", Wrapper = false, Publish = true)]
    class Unset : Command
    {
        protected override void DoCommandAction()
        {
            VariableManager.Instance.Unset(this.Variable);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the variable to unset")]
        public String Variable = "a";
    }
}
