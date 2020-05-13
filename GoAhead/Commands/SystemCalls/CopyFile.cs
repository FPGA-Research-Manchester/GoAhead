using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GoAhead.Commands.SystemCalls
{
    [CommandDescription(Description = "Copy (and overwrite) a file", Publish=true, Wrapper=false)]
    class CopyFile : Command
    {
        protected override void DoCommandAction()
        {
            File.Copy(SourceFile, TargetFile, true);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to copy")]
        public string SourceFile = "";

        [Parameter(Comment = "The name of the target file")]
        public string TargetFile = "";
    }
}
