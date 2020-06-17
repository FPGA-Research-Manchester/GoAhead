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
            Dictionary<String, int> cmds = new Dictionary<String, int>();

            TextReader tr = new StreamReader(this.CommandCoverageFile);
            String line = "";
            while ((line = tr.ReadLine()) != null)
            {
                String[] atoms = Regex.Split(line, ";");
                String cmdName = atoms[0];
                int cmdCount = Int32.Parse(atoms[1]);
                cmds[cmdName] = cmdCount;
            }
            tr.Close();

            foreach (Type type in CommandStringParser.GetAllCommandTypes().Where(t => !cmds.ContainsKey(t.Name)).OrderBy(t => t.Name))
            {
                this.OutputManager.WriteOutput("Command " + type.Name + " is uncovered");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The command coverage file (CSV)")]
        public String CommandCoverageFile = "";
    }
}
