using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Evaluate the given condition. If consdition evaluates to true, execute the commands in the then block, otherwise execute the command in the else block ", Wrapper=true)]
    class If : Command
    {
        protected override void DoCommandAction()
        {            
            ExpressionParser ep = new ExpressionParser();
            int evaluationResult = 0;
            bool valid = ep.Evaluate(Condition, out evaluationResult);
            
            if (!valid)
            {
                throw new ArgumentException("Condition" + Condition + " is not a valid arithmetic expression");
            }

            CommandStringParser parser = new CommandStringParser(evaluationResult != 0 ? Then : Else);
            foreach (string cmdString in parser.Parse())
            {
                Command cmd = null;
                string errorDescr = "";
                parser.ParseCommand(cmdString, true, out cmd, out errorDescr);
                // only the alias it self shall appear in the command trace
                cmd.UpdateCommandTrace = false;
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The condition is an arithmetic expression that is considered as false if it evaluates to 0 and considered as true otherwise")]
        public string Condition = "a<10000";

        [Parameter(Comment = "Execute these commands if condition evaluates to true")]
        public string Then = "ClearSelection;InvertSelection;";

        [Parameter(Comment = "Execute these commands if condition evaluates to false")]
        public string Else = "ClearSelection;AddToSelectionXY UpperLeftX=138 UpperLeftY=85 LowerRightX=145 LowerRightY=125;ExpandSelection;";        

    }
}
