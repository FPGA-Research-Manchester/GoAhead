using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Print out the given text as a comment", Wrapper = true)]
    class Print : Command
    {
        protected override void DoCommandAction()
        {
            if (Regex.IsMatch(this.Text, @"^\s*#"))
            {
                this.OutputManager.WriteOutput(this.Text);
            }
            else
            {
                this.OutputManager.WriteOutput("# " + this.Text);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The text to print out")]
        public String Text = "";
    }
}
