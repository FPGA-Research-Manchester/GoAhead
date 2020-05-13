using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using GoAhead.Commands;

namespace GoAhead.Commands
{
    [CommandDescription(Description="Print a list of all used arguments names among commands. You may supply this list to an editor for synatx highlighting", Wrapper=false, Publish=true)]
    class PrintCommandParameters : Command
    {
        protected override void DoCommandAction()
        {
            SortedDictionary<string, string> parameters = new SortedDictionary<string,string>();

            foreach (Type type in CommandStringParser.GetAllCommandTypes())
            {
                foreach (FieldInfo fi in type.GetFields())
                {
                    // find ParamterField attr
                    foreach (object obj in fi.GetCustomAttributes(true))
                    {
                        if (obj is Parameter && !parameters.ContainsKey(fi.Name))
                        {
                            parameters.Add(fi.Name, fi.Name);
                        }
                    }
                }
            }

            foreach(string s in parameters.Keys)
            {
                OutputManager.WriteOutput(s);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
