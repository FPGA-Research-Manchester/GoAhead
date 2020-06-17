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
            bool valid = ep.Evaluate(this.Condition, out evaluationResult);
            
            if (!valid)
            {
                throw new ArgumentException("Condition" + this.Condition + " is not a valid arithmetic expression");
            }

            CommandStringParser parser = new CommandStringParser(evaluationResult != 0 ? this.Then : this.Else);
            foreach (String cmdString in parser.Parse())
            {
                Command cmd = null;
                String errorDescr = "";
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
        public String Condition = "a<10000";

        [Parameter(Comment = "Execute these commands if condition evaluates to true")]
        public String Then = "ClearSelection;InvertSelection;";

        [Parameter(Comment = "Execute these commands if condition evaluates to false")]
        public String Else = "ClearSelection;AddToSelectionXY UpperLeftX=138 UpperLeftY=85 LowerRightX=145 LowerRightY=125;ExpandSelection;";        

    }
}
