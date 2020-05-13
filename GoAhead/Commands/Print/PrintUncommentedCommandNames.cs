using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "Print out all commands that do not have command description in the source code", Wrapper=false, Publish=false)]
    class PrintUncommentedCommandNames : Command
    {
        protected override void DoCommandAction()
        {
            foreach (Type type in CommandStringParser.GetAllCommandTypes().OrderBy(t => t.Name))
            {
                // create instance to get default value
                Command cmd = (Command)Activator.CreateInstance(type);

                bool commandDescriptionFound = false;
                foreach (Attribute attr in Attribute.GetCustomAttributes(type).Where(a => a is CommandDescription))
                {
                    CommandDescription descr = (CommandDescription)attr;
                    if (!string.IsNullOrEmpty(descr.Description))
                    {
                        commandDescriptionFound = true;
                        break;
                    }
                }
                if (!commandDescriptionFound)
                
                {
                    OutputManager.WriteOutput(type.Name + " has no Command Description");
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
