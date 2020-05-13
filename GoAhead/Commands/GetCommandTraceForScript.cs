using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Read in a script and print the commands to command trace without executing the commands", Wrapper = false)]
    class GetCommandTraceForScript : Command
    {
        protected override void DoCommandAction()
        {
            if (!File.Exists(FileName))
            {
                throw new ArgumentException("File " + FileName + " does not exist");
            }

            CommandExecuter.Instance.DoNotExecuteCommandAndUpdateCommandTraceOnly = true;

            // read script and execute commands
            FileInfo fi = new FileInfo(FileName);
            CommandExecuter.Instance.Execute(fi);

            CommandExecuter.Instance.DoNotExecuteCommandAndUpdateCommandTraceOnly = false;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the script to print the command trace for")]
        public string FileName = "script.goa";
    }
}
