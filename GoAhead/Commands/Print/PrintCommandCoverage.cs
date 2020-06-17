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
            Dictionary<String, int> cmds = new Dictionary<String, int>();

            if (this.Append && File.Exists(this.FileName))
            {
                TextReader tr = new StreamReader(this.FileName);
                String line = "";
                while((line = tr.ReadLine()) != null)
                {
                    String[] atoms = Regex.Split(line, ";");
                    String cmdName = atoms[0];
                    int cmdCount = Int32.Parse(atoms[1]);
                    cmds[cmdName] = cmdCount;
                }
                tr.Close();

                // bypass later append
                File.Delete(this.FileName);
            }
            foreach (Command cmd in CommandExecuter.Instance.GetAllCommands())
            {
                if (!cmds.ContainsKey(cmd.GetType().Name))
                {
                    cmds.Add(cmd.GetType().Name, 0);
                }
                cmds[cmd.GetType().Name] += 1;
            }

            TextWriter tw = new StreamWriter(this.FileName);
            foreach (String cmdName in cmds.Keys.OrderBy(c => c))
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
