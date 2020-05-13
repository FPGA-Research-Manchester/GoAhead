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
            if (Regex.IsMatch(Text, @"^\s*#"))
            {
                OutputManager.WriteOutput(Text);
            }
            else
            {
                OutputManager.WriteOutput("# " + Text);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The text to print out")]
        public string Text = "";
    }
}
