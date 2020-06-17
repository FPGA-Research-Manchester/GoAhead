using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GoAhead.Objects;
using GoAhead.Commands.Variables;

namespace GoAhead.Commands.SystemCalls
{
    [CommandDescription(Description = "Run the specified command (system call)", Wrapper = true)]
    class Execute : Command
    {
        protected override void DoCommandAction()
        {
            String cmdName = this.Command;

            //ProcessStartInfo cmdInfo = new ProcessStartInfo(cmdName, arguments);
            ProcessStartInfo cmdInfo = new ProcessStartInfo("cmd.exe", "/c " + this.Command);
            cmdInfo.UseShellExecute = false;
            cmdInfo.ErrorDialog = true;
            cmdInfo.RedirectStandardOutput = true;
            cmdInfo.RedirectStandardError = true;
            cmdInfo.CreateNoWindow = true;
            Process process = System.Diagnostics.Process.Start(cmdInfo);
            process.WaitForExit();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = @"Execute this command, e.g. dir c:\\work")]
        public String Command = "";
    }
}
