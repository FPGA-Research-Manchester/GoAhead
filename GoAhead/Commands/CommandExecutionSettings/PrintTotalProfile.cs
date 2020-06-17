using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.CommandExecutionSettings
{
    class PrintTotalProfile : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // surpreess own output after ProfileAll
            this.Profile = false;
            Dictionary<String, long> profile = new Dictionary<String, long>();

            if (this.Append && File.Exists(this.FileName))
            {
                TextReader tr = new StreamReader(this.FileName);
                String line = "";
                while ((line = tr.ReadLine()) != null)
                {
                    String[] atoms = Regex.Split(line, ";");
                    if (atoms.Length == 2)
                    {
                        String cmdName = atoms[0];
                        long cmdCount = long.Parse(atoms[1]);
                        profile[cmdName] = cmdCount;
                    }
                }
                tr.Close();                
            }

            if (File.Exists(this.FileName))
            {
                // bypass later append
                File.Delete(this.FileName);
            }

            CommandHook hook = CommandExecuter.Instance.GetAllHooks().FirstOrDefault(h => h is ProfilingHook);
            ProfilingHook profileHook = (ProfilingHook)hook;
            if (profileHook == null)
            {
                throw new ArgumentException("No profiler found");
            }
            else
            {
                foreach (KeyValuePair<String, long> tupel in profileHook.TotalProfile)
                {
                    String key = tupel.Key;
                    if(!profile.ContainsKey(key))
                    {
                        profile.Add(key, 0);
                    }
                    profile[key] += tupel.Value;
                }
            }

            //TextWriter tw = new StreamWriter(this.FileName);
            foreach (KeyValuePair<String, long> tupel in profile.OrderBy(t => t.Value))
            {
                this.OutputManager.WriteOutput(tupel.Key + ";" + profile[tupel.Key]);
                //tw.WriteLine(tupel.Key + ";" + profile[tupel.Key]);
            }
            //tw.Close();
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
