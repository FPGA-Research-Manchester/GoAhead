using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GoAhead.Objects;

namespace GoAhead.Commands
{
    class CommandExecutionContext
    {
        public void Parsed(String cmdString, Command cmd)
        {
            this.m_commands.Add(new Tuple<String, Command>(cmdString, cmd));

            if (cmd is AddAlias)
            {             
                Command resolvedCommand = null;
                String errorDescr;
                bool valid = this.m_parser.ParseCommand(cmdString, true, out resolvedCommand, out errorDescr);
                resolvedCommand.UpdateCommandTrace = false;
                CommandExecuter.Instance.Execute(resolvedCommand);
                resolvedCommand.UpdateCommandTrace = true;
            }
        }

        public void SetLabels()
        {
            for (int i = 0; i < this.m_commands.Count; i++)
            {
                Command cmd = this.m_commands[i].Item2;
                if (cmd is SetLabel)
                {
                    // execute String and NOT command to reevaluate arithmetic expressions and defines
                    Command nextCmd = null;
                    String errorDescr;
                    bool valid = this.m_parser.ParseCommand(this.m_commands[i].Item1, true, out nextCmd, out errorDescr);

                    SetLabel setLabelCmd = (SetLabel)nextCmd;
                    /*
                    if (LabelManager.Instance.Contains(setLabelCmd.LabelName))
                    {
                        throw new ArgumentException("Label name " + setLabelCmd.LabelName + " already used");
                    }*/

                    // +1 as this command is also be added to commands
                    LabelManager.Instance.SetLabel(setLabelCmd.LabelName, i + 1);
                }
            }
        }

        public int Execute(int cmdIndex)
        {
            int nextCommandIndex = cmdIndex;

            Command cmd = this.m_commands[cmdIndex].Item2;

            //Console.WriteLine(cmd.GetType().Name + "@" + cmdIndex + " out of " + this.m_commands.Count);

            if (cmd is Return)
            {
                // set exit conditions for surrounf while loop
                return this.m_commands.Count;
            }
            else if (cmd is GotoCommand)
            {                
                Command nextCmd = null;
                String errorDescr;
                bool valid = this.m_parser.ParseCommand(this.m_commands[cmdIndex].Item1, true, out nextCmd, out errorDescr);
                // no action, just update command trace
                CommandExecuter.Instance.Execute(nextCmd);

                GotoCommand gotoCmd = (GotoCommand)nextCmd;

                //Console.WriteLine(this.m_commands[cmdIndex].Item1);
                //Console.WriteLine(gotoCmd.ToString());

                if (!LabelManager.Instance.Contains(gotoCmd.LabelName))
                {
                    throw new ArgumentException("Label name " + gotoCmd.LabelName + " not found");
                }

                if(gotoCmd.JumpToLabel())
                {
                    nextCommandIndex = LabelManager.Instance.GetCommandListIndex(gotoCmd.LabelName);
                }
                else
                {
                    nextCommandIndex++;
                }
            }
                /*
            else if (cmd is RunScript)
            {
                Command nextCmd = null;
                String errorDescr;
                bool valid = this.m_parser.ParseCommand(this.m_commands[cmdIndex].Item1, true, out nextCmd, out errorDescr);

                // context
                FileInfo fi = new FileInfo(((RunScript)nextCmd).FileName);
                CommandExecuter.Instance.Execute(fi);

                nextCommandIndex++;
            }*/
            else
            {
                // execute String and NOT command to reevaluate arithmetic expressions
                Command nextCmd = null;
                String errorDescr;
                //Console.WriteLine(this.m_commands[cmdIndex].Key);
                bool valid = this.m_parser.ParseCommand(this.m_commands[cmdIndex].Item1, true, out nextCmd, out errorDescr);
                if (!valid)
                {
                    // forward parse error to hooks
                    foreach (CommandHook hook in CommandExecuter.Instance.GetAllHooks())
                    {
                        hook.ParseError(this.m_commands[cmdIndex].Item1, errorDescr);
                    }
                    throw new ArgumentException(errorDescr);
                }
                // no action, just update command trace
                CommandExecuter.Instance.Execute(nextCmd);
                nextCommandIndex++;
            }
            return nextCommandIndex;
        }

        public void ExecuteAll()
        {
            int cmdIndex = 0;
            while (cmdIndex < this.m_commands.Count)
            {
                cmdIndex = this.Execute(cmdIndex);
            }
        }

        public int CommandCount
        {
            get { return this.m_commands.Count; }
        }

        private CommandStringParser m_parser = new CommandStringParser("");
        private List<Tuple<String, Command>> m_commands = new List<Tuple<String, Command>>();
    }
	
}
