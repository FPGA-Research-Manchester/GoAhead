using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands
{
    [CommandDescription(Description="Execute a jump to the given label if the given condition evaluates to true")]
    class GotoLabel : GotoCommand
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
            ExpressionParser ep = new ExpressionParser();
            int evaluationResult = 0;
            bool valid = ep.Evaluate(this.Condition, out evaluationResult);
            return (evaluationResult != 0);
        }

        [Parameter(Comment = "The condition is an arithmetic expression that is considered as false if it evaluates to 0 and considered as true if it evalauates to a value not equal to 0")]
        public String Condition = "a<10000";
    }
}
