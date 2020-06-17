using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.InterfaceManager
{
    [CommandDescription(Description = "Load an interface from a comma separated values file. This commands round brackets () for indexing. Brackets []<>{} will be replaced with ()", Wrapper = false)]
    class LoadInterfaceAsCSV : Command
    {
        protected override void DoCommandAction()
        {
            TextReader tr = new StreamReader(this.FileName);

            Objects.InterfaceManager.Instance.GenerateCommandsForChanges = false;
            String line = "";
            while ((line = tr.ReadLine()) != null)
            {
                line = line.Replace('[', '(');
                line = line.Replace('{', '(');
                line = line.Replace('<', '(');
                line = line.Replace(']', ')');
                line = line.Replace('}', ')');
                line = line.Replace('>', ')');
                // remove tabs
                line = Regex.Replace(line, @"\s", "");

                if (Regex.IsMatch(line, @"^\s*#") || String.IsNullOrWhiteSpace(line) || String.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line.Contains(';'))
                {
                    foreach (String subSignal in line.Split(';').Where(s => !String.IsNullOrWhiteSpace(s)))
                    {
                        this.AddSignal(subSignal);
                    }
                }
                else
                {
                    this.AddSignal(line);
                }
            };

            tr.Close();
            GoAhead.Objects.InterfaceManager.Instance.GenerateCommandsForChanges = true;
            GoAhead.Objects.InterfaceManager.Instance.LoadCommands.Add(this.ToString());
        }

        private void AddSignal(String subSignal)
        {
            String[] atoms = subSignal.Split(',');
            FPGATypes.InterfaceDirection dir = (FPGATypes.InterfaceDirection)Enum.Parse(typeof(FPGATypes.InterfaceDirection), atoms[2]);

            Signal s = new Signal(atoms[0], atoms[1], dir, this.PartialArea, Int32.Parse(atoms[3]));

            Objects.InterfaceManager.Instance.Add(s);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the file to read the interface from")]
        public String FileName = "";

        [Parameter(Comment = "The name of the partial area the read in interface will be assigned to ")]
        public String PartialArea = "";
    }
}
