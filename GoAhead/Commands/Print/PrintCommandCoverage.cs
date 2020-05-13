using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.CommandExecutionSettings
{
    class PrintCommandCoverage : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            Dictionary<string, int> cmds = new Dictionary<string, int>();

            if (Append && File.Exists(FileName))
            {
                TextReader tr = new StreamReader(FileName);
                string line = "";
                while((line = tr.ReadLine()) != null)
                {
                    string[] atoms = Regex.Split(line, ";");
                    string cmdName = atoms[0];
                    int cmdCount = int.Parse(atoms[1]);
                    cmds[cmdName] = cmdCount;
                }
                tr.Close();

                // bypass later append
                File.Delete(FileName);
            }
            foreach (Command cmd in CommandExecuter.Instance.GetAllCommands())
            {
                if (!cmds.ContainsKey(cmd.GetType().Name))
                {
                    cmds.Add(cmd.GetType().Name, 0);
                }
                cmds[cmd.GetType().Name] += 1;
            }

            TextWriter tw = new StreamWriter(FileName);
            foreach (string cmdName in cmds.Keys.OrderBy(c => c))
            {
                tw.WriteLine(cmdName + ";" + cmds[cmdName]);
            }
            tw.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
