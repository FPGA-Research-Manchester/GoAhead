using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.SystemCalls
{
    [CommandDescription(Description = "Recursively delete the given directory", Wrapper=false, Publish=true)]
    class RemoveDirectory : Command
    {
       protected override void DoCommandAction()        
        {
            if (System.IO.Directory.Exists(this.Path))
            {
                System.IO.Directory.Delete(this.Path, true);
            } 
            // trying to remove a non existing directory will cause an error
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the directory to delete")]
        public String Path = "";
    }
}
