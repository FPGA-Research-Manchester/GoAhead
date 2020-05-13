using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO.MemoryMappedFiles;
using GoAhead.Settings;
using GoAhead.Objects;
using GoAhead.Commands.CommandExecutionSettings;
using GoAhead.Commands.Variables;

namespace GoAhead.Commands
{
    public partial class CommandExecuter
	{
        public static CommandExecuter Instance = new CommandExecuter();
        
        /// <summary>
        /// Executes this command
        /// </summary>
        /// <param name="nextCmd"></param>
        public void Execute(params Command[] cmds)
        {
            foreach (Command cmd in cmds)
            {               
                if (DoNotExecuteCommandAndUpdateCommandTraceOnly && !(cmd is Set))
                {
                    cmd.Execute = false;
                }

                if (m_pendingWrapperCommands == 0)
                {
                    if (ProfileAll)
                    {
                        cmd.Profile = true;
                    }
                }

                if (ProfilePeakMemoryConsumption)
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
                    if(PeakNumberOfBytesOfMemoryUsed < totalBytesOfMemoryUsed)
                    {
                        PeakNumberOfBytesOfMemoryUsed = totalBytesOfMemoryUsed;
                    }
                }

                // if the next command is a wrapper command ...
                bool cmdIsWrapper = IsWrapperCommand(cmd);
                if (cmdIsWrapper)
                {
                    m_pendingWrapperCommands++;
                }

                // update command trace
                if ( ((m_updateCommandTrace || PrintWrappedCommands) && !MuteCommandTrace && cmd.UpdateCommandTrace) || cmd is UnmuteCommandTrace)
                {
                    foreach (CommandHook hook in m_hooks)
                    {
                        hook.CommandTrace(cmd);
                    }
                }

                // for wrapper commands, suppress the output
                if (m_pendingWrapperCommands > 0)
                {
                    m_updateCommandTrace = false;
                }

                // second hook for e.g. cursor
                foreach (CommandHook hook in m_hooks)
                {
                    hook.PreRun(cmd);
                }

                // do the command
                if (cmd.Execute)
                {
                    try
                    {
                        cmd.Do();
                    }
                    catch (Exception error)
                    {
                        cmd.ErrorThrown = true;

                        // error hook
                        foreach (CommandHook hook in m_hooks)
                        {
                            hook.Error(cmd, error);
                        }
                    }
                }

                // store command for undo AFTER the exection, thus UnDo will pop the last command before itself
                // otherwise UnDo would undo itself
                m_commandStack.Push(cmd);
                m_commandStrings.Add(cmd.ToString());

                if (cmdIsWrapper)
                {
                    m_pendingWrapperCommands--;
                }

                // and reactivate the output after the wrapper command finished
                // meanwhile, other NON wrapper commands might have been executed
                if (m_pendingWrapperCommands == 0)
                {
                    m_updateCommandTrace = true;
                }

                // hook to dump outputs
                foreach (CommandHook hook in m_hooks)
                {                
                    hook.PostRun(cmd);
                }

                // prevent endless message boxes
                if (cmd.ErrorThrown)
                {
                    if (GUIActive || OnErrorContinue)
                    {
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exiting from command line mode after error in command " + cmd.ToString());
                        Console.ResetColor();
                        Environment.Exit(-1);
                    }
                }

                // do not call cmd.OutputManager.Clear() here as this cleares the output for wrapped commands
            }
        }

        public void Execute(FileInfo fi)
        {
            CommandStringParser parser = new CommandStringParser(fi);
            RunCommandsFromParser(parser);
        }

        public void Execute(string commandString)
		{            
            CommandStringParser parser = new CommandStringParser(commandString);
            RunCommandsFromParser(parser);
		}

        private void RunCommandsFromParser(CommandStringParser parser)
        {
            CommandExecutionContext context = new CommandExecutionContext();
            foreach (string cmdString in parser.Parse())
            {
                Command parsedCmd = null;
                string errorDescr;
                bool valid = parser.ParseCommand(cmdString, false, out parsedCmd, out errorDescr);
                if (valid)
                {
                    context.Parsed(cmdString, parsedCmd);
                }
                else
                {
                    // forward parse error to hooks
                    foreach (CommandHook hook in m_hooks)
                    {
                        hook.ParseError(cmdString, errorDescr);
                    }
                    // we can not execute this command
                    continue;
                }
            }

            context.SetLabels();
            context.ExecuteAll();
        }

        /// <summary>
        /// No labels here!!
        /// </summary>
        public void ExecuteWithOutContext(string commandString)
        {
            CommandStringParser parser = new CommandStringParser(commandString);
            foreach (string cmdString in parser.Parse())
            {
                Command parsedCmd = null;
                string errorDescr;
                bool valid = parser.ParseCommand(cmdString, true, out parsedCmd, out errorDescr);
                if (valid)
                {
                    Execute(parsedCmd);                    
                }
                else
                {
                    throw new ArgumentException(errorDescr);
                }
            }
        }

        public List<string> ReadCommandFile(string commandFile)
        {
            List<string> commandString = new List<string>();

            FileStream fs = File.OpenRead(commandFile);
            byte[] byteBuffer = new byte[fs.Length];
            int length = Convert.ToInt32(fs.Length);
            fs.Read(byteBuffer, 0, length);
            fs.Close();

            CommandStringParser parser = new CommandStringParser(byteBuffer, length);
            foreach (string cmd in parser.Parse())
            {
                commandString.Add(cmd);
            }

            return commandString;
        }

        public Command PopLastCommand()
        {
            return m_commandStack.Pop();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            foreach (Command cmd in m_commandStack)
            {
                yield return cmd;
            }
        }

        /// <summary>
        /// Return whether the given command has an attribute which marks the commands as a wrapper command (= wheter it calls other commands)
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private bool IsWrapperCommand(Command cmd)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(cmd.GetType());
            CommandDescription descr = (CommandDescription)attrs.FirstOrDefault(a => a is CommandDescription);
            return descr != null ? descr.Wrapper : false;
        }

        public void AddHook(CommandHook hook)
        {
            m_hooks.Add(hook);
        }

        public IEnumerable<CommandHook> GetAllHooks()
        {
            foreach (CommandHook hook in m_hooks)
            {
                yield return hook;
            }
        }

        /// <summary>
        /// The number of executed commands
        /// </summary>
        public int CommandCount
        {
            get { return m_commandStrings.Count; }
        }

        public string GetCommandString(int i)
        {
            return m_commandStrings[i];
        }

		/// <summary>
		/// store all command for undo
		/// </summary>
        private Stack<Command> m_commandStack = new Stack<Command>();

		private List<string> m_commandStrings = new List<string>();
        
        private List<CommandHook> m_hooks = new List<CommandHook>();
        
        private bool m_updateCommandTrace = true;

        /// <summary>
        /// The number of started Wrapping commands
        /// </summary>
        private int m_pendingWrapperCommands = 0;

        /// <summary>
        /// Whether the GUI is active or not (evaluated after erroneous commands)
        /// </summary>
        public bool GUIActive { get; set; }

        /// <summary>
        // Whether to create pop message boxes for error messages from commands
        /// </summary>
        public bool MuteGUIErrorMessages { get; set; }
        
        /// <summary>
        // /Whether command output (VHDL, UCF) is printed or not
        /// </summary>
        public bool MuteOutput { get; set; }

        /// <summary>
        // /More print outs
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>        
        /// Whether to print out command trace or not
        /// </summary>
        public bool MuteCommandTrace { get; set; }

        /// <summary>
        /// Whether to print out command trace or not
        /// </summary>
        public bool PrintWrappedCommands { get; set; }
        
        /// <summary>
        /// Whether to profile all commands or not
        /// </summary>
        public bool ProfileAll { get; set; }

        /// <summary>
        /// Whether to track the peak memory consumption or not
        /// </summary>
        public bool ProfilePeakMemoryConsumption { get; set; }
         
        /// <summary>
        /// the peak memory usage
        /// </summary>
        public long PeakNumberOfBytesOfMemoryUsed = 0;

        /// <summary>
        /// Whether the console command trace is colored
        /// </summary>
        public bool ColorOutput { get; set; }

        /// <summary>
        /// Whether to continue with the next command after an error in batch mode
        /// </summary>
        public bool OnErrorContinue { get; set; }

        /// <summary>
        /// Whether to not execute commands but only update the command trace
        /// </summary>
        public bool DoNotExecuteCommandAndUpdateCommandTraceOnly { get; set; }
    }
}


