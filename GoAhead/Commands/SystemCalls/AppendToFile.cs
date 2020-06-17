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
            System.IO.File.AppendAllText(this.FileName, this.Text + (this.Text.EndsWith(System.Environment.NewLine) ? "" : System.Environment.NewLine));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The filename")]
        public String FileName = "";

        [Parameter(Comment = "The text to print out")]
        public String Text = "";
    }
}
