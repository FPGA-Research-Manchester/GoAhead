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
            Profile = false;
            Dictionary<string, long> profile = new Dictionary<string, long>();

            if (Append && File.Exists(FileName))
            {
                TextReader tr = new StreamReader(FileName);
                string line = "";
                while ((line = tr.ReadLine()) != null)
                {
                    string[] atoms = Regex.Split(line, ";");
                    if (atoms.Length == 2)
                    {
                        string cmdName = atoms[0];
                        long cmdCount = long.Parse(atoms[1]);
                        profile[cmdName] = cmdCount;
                    }
                }
                tr.Close();                
            }

            if (File.Exists(FileName))
            {
                // bypass later append
                File.Delete(FileName);
            }

            CommandHook hook = CommandExecuter.Instance.GetAllHooks().FirstOrDefault(h => h is ProfilingHook);
            ProfilingHook profileHook = (ProfilingHook)hook;
            if (profileHook == null)
            {
                throw new ArgumentException("No profiler found");
            }
            else
            {
                foreach (KeyValuePair<string, long> tupel in profileHook.TotalProfile)
                {
                    string key = tupel.Key;
                    if(!profile.ContainsKey(key))
                    {
                        profile.Add(key, 0);
                    }
                    profile[key] += tupel.Value;
                }
            }

            //TextWriter tw = new StreamWriter(this.FileName);
            foreach (KeyValuePair<string, long> tupel in profile.OrderBy(t => t.Value))
            {
                OutputManager.WriteOutput(tupel.Key + ";" + profile[tupel.Key]);
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
