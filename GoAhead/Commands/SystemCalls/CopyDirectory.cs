using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace GoAhead.Commands.SystemCalls
{
    [CommandDescription(Description = "Copy (and overwrite) a directory", Publish = true, Wrapper = false)]
    class CopyDirectory : Command
    {
       protected override void DoCommandAction()
        {
            DirectoryInfo diSource = new DirectoryInfo(this.SourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(this.TargetDirectory);

            this.CopyAll(diSource, diTarget);
        }
         
        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //this.OutputManager.WriteOutput("Copying " + target.FullName + @"\" + fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the directory to copy")]
        public String SourceDirectory = "";

        [Parameter(Comment = "The name of the target directory")]
        public String TargetDirectory = "";
    }
}
