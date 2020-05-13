using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Evaluate the given arithmetic expression", Wrapper = false, Publish = true)]
    class Evaluate : Command
    {
        protected override void DoCommandAction()
        {
            ExpressionParser ep = new ExpressionParser();
            int evaluationResult = 0;
            bool validExpression = ep.Evaluate(Expression, out evaluationResult);

            OutputManager.WriteOutput(Expression + " = " + evaluationResult);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The expression to evaluate")]
        public string Expression = "";
    }
}
