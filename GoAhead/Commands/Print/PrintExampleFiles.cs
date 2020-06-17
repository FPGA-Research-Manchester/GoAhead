using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    [CommandDescription(Description = "For each command, create an empty text filed command.txt in GOAHEAD_HOME/help. Existing files will not be affected.", Wrapper = false, Publish = true)]
    class PrintExampleFiles : Command
    {
        protected override void DoCommandAction()
        {
            foreach (Type type in CommandStringParser.GetAllCommandTypes().OrderBy(t => t.Name))
            {
                Command command = (Command)Activator.CreateInstance(type);
                if (!command.PublishCommand)
                {
                    continue;
                }

                String helpFile = command.GetHelpFilePath();

                if (!File.Exists(helpFile))
                {
                    this.OutputManager.WriteOutput("Creating " + helpFile);
                    File.Create(helpFile);
                }
                else
                {
                    this.OutputManager.WriteOutput("File " + helpFile + " already exists");
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
