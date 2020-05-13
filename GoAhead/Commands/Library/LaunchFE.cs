using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoAhead.Commands.Library
{
    [CommandDescription(Description="Launch the FPGA-Editor with the given file (ncd or nmc)")]
    class LaunchFE : Command
    {
       protected override void DoCommandAction()
        {
            if(!System.IO.File.Exists(File))
            {
                throw new ArgumentException("File " + File + " does not exits");
            }

            string tempBatchFile = Path.GetTempFileName();
 	        StreamWriter batchFile = new StreamWriter(tempBatchFile);
            batchFile.WriteLine("start fpga_editor " + File);

            batchFile.Close();

            System.Diagnostics.Process.Start(tempBatchFile);
        }

        public override void  Undo()
        {
 	        throw new NotImplementedException();
        }


        [Parameter(Comment = "The file no open ")]
        public string File = "design.ncd";
    }
}
