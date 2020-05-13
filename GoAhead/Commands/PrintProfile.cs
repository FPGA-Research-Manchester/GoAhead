using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GoAhead.Commands
{
    [CommandDescription(Description="Print all collected profiling information", Publish=true, Wrapper=false)]
    class PrintProfile : Command
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput(WatchManager.Instance.GetResults());

            Process currentProcess = Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            OutputManager.WriteOutput("# Peak memory consumption (bytes) " + CommandExecuter.Instance.PeakNumberOfBytesOfMemoryUsed);
            OutputManager.WriteOutput("# Current memory consumption (bytes) " + totalBytesOfMemoryUsed);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
