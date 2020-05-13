using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.SystemCalls
{
    [CommandDescription(Description = "Append the given text to a file", Wrapper = true)]
    class AppendToFile : Command
    {
        protected override void DoCommandAction()
        {
            File.AppendAllText(FileName, Text + (Text.EndsWith(Environment.NewLine) ? "" : Environment.NewLine));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The filename")]
        public string FileName = "";

        [Parameter(Comment = "The text to print out")]
        public string Text = "";
    }
}
