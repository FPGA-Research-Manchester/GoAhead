using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Print the given text to file (optionally append or create backup)", Wrapper = false, Publish = true)]
    class PrintToFile : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            this.OutputManager.WriteOutput(this.Text);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The text to add to the given file")]
        public String Text = "# text";
    }
}
