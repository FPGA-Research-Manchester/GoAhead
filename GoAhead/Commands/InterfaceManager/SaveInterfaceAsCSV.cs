using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Objects;

namespace GoAhead.Commands.InterfaceManager
{
    [CommandDescription(Description = "Save an interface as comma separated values to file", Wrapper = false)]
    class SaveInterfaceAsCSV : Command
    {
       protected override void DoCommandAction()
        {
            TextWriter tw = new StreamWriter(FileName, Append);

            foreach (Signal s in Objects.InterfaceManager.Instance.GetAllSignals())
            {
                tw.WriteLine(s.ToCSV());
            }

            tw.Close();

        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to save the interface in")]
        public string FileName;

        [Parameter(Comment = "Wheter to append the file")]
        public bool Append = false;
    }
}
