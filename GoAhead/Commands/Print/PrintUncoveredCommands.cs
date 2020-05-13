using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace GoAhead.Commands
{
    class PrintUncoveredCommands : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            Dictionary<string, int> cmds = new Dictionary<string, int>();

            TextReader tr = new StreamReader(CommandCoverageFile);
            string line = "";
            while ((line = tr.ReadLine()) != null)
            {
                string[] atoms = Regex.Split(line, ";");
                string cmdName = atoms[0];
                int cmdCount = int.Parse(atoms[1]);
                cmds[cmdName] = cmdCount;
            }
            tr.Close();

            foreach (Type type in CommandStringParser.GetAllCommandTypes().Where(t => !cmds.ContainsKey(t.Name)).OrderBy(t => t.Name))
            {
                OutputManager.WriteOutput("Command " + type.Name + " is uncovered");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The command coverage file (CSV)")]
        public string CommandCoverageFile = "";
    }
}
