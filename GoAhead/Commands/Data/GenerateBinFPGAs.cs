using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Create binFPGAs for all *.xdl and *.viv_rpt files found in given directory")]
    class GenerateBinFPGAs : Command
    {
        protected override void DoCommandAction()
        {
            if (!System.IO.Directory.Exists(Directory))
            {
                throw new ArgumentException("Directory " + Directory + " does not exist");
            }
            string[] files = System.IO.Directory.GetFiles(Directory).Where(s =>
                s.EndsWith("xdl", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith("viv_rpt", StringComparison.OrdinalIgnoreCase)).ToArray();

            int fileCount = 0;
            foreach (string file in files)
            {
                string binFPGAFile = Path.ChangeExtension(file, "binFPGA");
                if (File.Exists(binFPGAFile))
                {
                    OutputManager.WriteOutput("File " + binFPGAFile + " already exists, skipping " + file);
                    continue;
                }

                if (file.EndsWith("xdl", StringComparison.OrdinalIgnoreCase))
                {
                    ReadXDL readCmd = new ReadXDL();
                    readCmd.FileName = file;
                    readCmd.PrintProgress = PrintProgress;
                    readCmd.Profile = Profile;
                    readCmd.ExcludePipsToBidirectionalWiresFromBlocking = ExcludePipsToBidirectionalWiresFromBlocking;
                    CommandExecuter.Instance.Execute(readCmd);
                }
                else if (file.EndsWith("viv_rpt", StringComparison.OrdinalIgnoreCase))
                {
                    ReadVivadoFPGA readCmd = new ReadVivadoFPGA();
                    readCmd.FileName = file;
                    readCmd.PrintProgress = PrintProgress;
                    readCmd.Profile = Profile;
                    readCmd.ExcludePipsToBidirectionalWiresFromBlocking = ExcludePipsToBidirectionalWiresFromBlocking;
                    CommandExecuter.Instance.Execute(readCmd);
                }
                else
                {
                    continue;
                }
                SaveBinFPGA saveCmd = new SaveBinFPGA();
                saveCmd.FileName = binFPGAFile;
                CommandExecuter.Instance.Execute(saveCmd);

                ProgressInfo.Progress = (int)((double)fileCount++ / (double)files.Length);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Whether to run a ExcludePipsToBidirectionalWiresFromBlocking command after reading")]
        public bool ExcludePipsToBidirectionalWiresFromBlocking = true;

        [Parameter(Comment = "The directory to look for *.xdl or *.viv_rpt and files. Resulting binFPGAs will also be stored here")]
        public string Directory = "";
    }
}
