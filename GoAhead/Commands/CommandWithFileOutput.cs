using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    public abstract class CommandWithFileOutput : Command
    {
        [Parameter(Comment = "The name of the file to save the output in.  FileName will be created, if it does not exist. FileName may be left empty to prevent any file accesses.")]
        public String FileName = "";

        [Parameter(Comment = "Whether to append the output (true) or not (false) in case FileName contains a valid path.")]
        public bool Append = true;

        [Parameter(Comment = "Whether to create a backup file of FileName with the extension .bak. If the backup files already exists, it will be overwritten.")]
        public bool CreateBackupFile = true;
    }
}
