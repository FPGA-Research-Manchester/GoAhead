using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using GoAhead.Commands;
using GoAhead.Interfaces;
using GoAhead.Commands.GUI;
using GoAhead.Commands.UCF;
using GoAhead.Commands.VHDL;

namespace GoAhead.GUI
{
    class GUICommandHook : CommandHook
    {
        public GUICommandHook(GUI callee, ConsoleCtrl console)
        {
            m_console = console;
            m_callee = callee;
        }

        public override void CommandTrace(Command cmd)
        {
            m_console.AddToCommandTrace(cmd);
        }

        public override void PreRun(Command cmd)
        {
            if (cmd is GUICommand)
            {
                if (cmd is AddUserButton)
                {
                    AddUserButton addCmd = (AddUserButton)cmd;
                    if (addCmd.ToolStrip == null)
                    {
                        addCmd.ToolStrip = m_callee.UserToolStrip;
                    }
                }
                else if (cmd is AddUserMenu)
                {
                    AddUserMenu addCmd = (AddUserMenu)cmd;
                    if (addCmd.ToolStrip == null)
                    {
                        addCmd.ToolStrip = m_callee.UserMenuItem;
                    }
                }
                else if (cmd is ClearUserMenuCommand)
                {
                    ClearUserMenuCommand clearCmd = (ClearUserMenuCommand)cmd;
                    if (clearCmd.ToolStrip == null)
                    {
                        clearCmd.ToolStrip = m_callee.UserMenuItem;
                    }
                }
                else if (cmd is OpenScriptInDebugger)
                {
                    OpenScriptInDebugger openCmd = (OpenScriptInDebugger)cmd;
                    openCmd.FormToInvalidate = m_callee;
                }
            }

            m_callee.Cursor = Cursors.WaitCursor;   
        }

        public override void Error(Command failedCommand, Exception error)
        {
            m_console.AddToErrorTrace(Environment.NewLine + "Failed to run command " + failedCommand.ToString() + ": " + error.Message + Environment.NewLine);
            m_console.AddToErrorTrace(failedCommand.OutputManager.GetOutput());

            if (!CommandExecuter.Instance.MuteGUIErrorMessages)
            {
                DialogResult result = MessageBox.Show("Command " + failedCommand.GetType().Name + " failed. Check error and warning trace! Continue to display this error message dialog (Yes) or dump errors to console only (No)?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (result == DialogResult.No)
                {
                    CommandExecuter.Instance.MuteGUIErrorMessages = true;
                    MessageBox.Show("To restart displaying error message, issue a UnmuteGUIErrorMessages command", "Hint", MessageBoxButtons.OK);
                }
            }
        }

        public override void PostRun(Command cmd)
        {
            // here in GUI, no filtering for CommandWithFileOutput

            // no print outs when mute
            if (!CommandExecuter.Instance.MuteOutput && !cmd.Mute)
            {
                m_console.AddToOutputTrace(cmd.OutputManager.GetOutput());
                m_console.AddToWarningsTrace(cmd.OutputManager.GetWarnings());
                m_console.AddToUCFTrace(cmd.OutputManager.GetUCFOuput());
                m_console.AddToVHDLTrace(cmd.OutputManager.GetVHDLOuput());
                m_console.AddToTCLTrace(cmd.OutputManager.GetTCLOuput());
            }

            m_callee.Cursor = Cursors.Default;
        }

        public override void ParseError(string cmd, string error)
        {
            m_console.AddToErrorTrace("Failed to parse command " + cmd.ToString() + ": " + error + ". " + Environment.NewLine);

            MessageBox.Show("Command " + cmd + " could not be parsed. Check error trace!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        public override void ProgressUpdate(Command cmd)
        {
        }           

        private readonly GUI m_callee;
        private readonly ConsoleCtrl m_console;        
    }
}

namespace GoAhead
{
    public class ConsoleCommandHook : CommandHook
    {
        public override void CommandTrace(Command cmd)
        {
            string output = cmd.ToString();
            if (!CommandExecuter.Instance.ColorOutput)
            {
                Console.WriteLine(output);
            }
            else
            {
                int index = 0;
                // print command name in red
                Console.ForegroundColor = ConsoleColor.Red;
                while (index < output.Length && output[index] != ' ' && output[index] != ';')
                {
                    Console.Write(output[index]);
                    index++;
                }
                Console.ResetColor();
                // print blank
                Console.Write(output[index++]);

                string argumentPart = cmd.ToString();
                argumentPart = argumentPart.Substring(index, argumentPart.Length - index);

                CommandStringParser parser = new CommandStringParser("");

                foreach (NameValuePair nameValuePair in parser.GetNameValuePairs(argumentPart))
                {
                    // next command starts
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    for (int i = 0; i < nameValuePair.Name.Length; i++)
                    {
                        Console.Write(output[index++]);
                    }
                    Console.ResetColor();
                    Console.Write(output[index++]);                    
                    for (int i = 0; i < nameValuePair.Value.Length; i++)
                    {
                        Console.Write(output[index++]);
                    }
                    if (index < output.Length)
                    {
                        Console.Write(output[index++]);
                    }
                }
                Console.WriteLine("");
                // restore console color for next output (after end of program)
                Console.ResetColor();
                return;
            }
        }

        public override void PreRun(Command cmd)
        {
            if (!cmd.PublishCommand)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: You are using the unpublished command " + cmd.GetType().Name);
                Console.ResetColor();
            }
        }

        public override void Error(Command failedCommand, Exception error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to run command " + failedCommand.ToString() + ": " + error.Message);
            Console.WriteLine("Failed to run command " + failedCommand.ToString() + ": " + error.StackTrace);
            Console.WriteLine(failedCommand.OutputManager.GetOutput());
            Console.ResetColor();
        }

        public override void PostRun(Command cmd)
        {
            // no print outs when mute
            if (CommandExecuter.Instance.MuteOutput || cmd.Mute)
            {
                return;
            }

            // no console print out for command with file outputs
            if (cmd is CommandWithFileOutput)
            {
                CommandWithFileOutput cmdWithOutput = (CommandWithFileOutput)cmd;
                if (!string.IsNullOrEmpty(cmdWithOutput.FileName))
                {
                    return;
                }
            }

            if (cmd.OutputManager.HasWarnings)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Write(cmd.OutputManager.GetWarnings());
                Console.ResetColor();
            }
                        
            if (cmd.OutputManager.HasOutput) 
            {
                Write(cmd.OutputManager.GetOutput());
            }
            // console takes verly long to print huge text, supress output
            /*
            if (cmd.OutputManager.HasUCFOutput) 
            {
                this.Write(cmd.OutputManager.GetUCFOuput());
            }
            if (cmd.OutputManager.HasVHDLOutput) 
            {
                this.Write(cmd.OutputManager.GetVHDLOuput());
            }*/
        }

        private void Write(string text)
        {
            if(text.EndsWith(Environment.NewLine))
            {
                Console.Write(text);
            }
            else
            {
                Console.WriteLine(text);
            }

        }

        public override void ParseError(string cmd, string error)
        {
            Console.WriteLine("Failed to parse the command ");           
            Console.WriteLine(cmd.ToString() + ": ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine(error);
            Console.WriteLine("");
            Console.ResetColor();
        }

        public override void ProgressUpdate(Command cmd)
        {
        }
    }

    public class PrintOutputHook : CommandHook
    {
        public override void CommandTrace(Command cmd)
        {
        }

        public override void PreRun(Command cmd)
        {
        }

        public override void Error(Command cmd, Exception error)
        {
        }

        public override void PostRun(Command cmd)
        {
            if (cmd.ErrorThrown)
            {
                return; 
            }

            if (cmd is CommandWithFileOutput && cmd.Execute)
            {
                CommandWithFileOutput cmdWithOutput = (CommandWithFileOutput)cmd;
                if (string.IsNullOrEmpty(cmdWithOutput.FileName))
                {
                    return;
                }
                try
                {
                    if (cmdWithOutput.CreateBackupFile && File.Exists(cmdWithOutput.FileName))
                    {
                        string backupFileName = string.IsNullOrEmpty(Path.GetDirectoryName(cmdWithOutput.FileName)) ? 
                                                                                   Path.GetFileNameWithoutExtension(cmdWithOutput.FileName) :
                            Path.GetDirectoryName(cmdWithOutput.FileName) + @"\" + Path.GetFileNameWithoutExtension(cmdWithOutput.FileName);
                        backupFileName += ".bak";
                        File.Copy(cmdWithOutput.FileName, backupFileName, true);
                    }
                    TextWriter tw = new StreamWriter(cmdWithOutput.FileName, cmdWithOutput.Append);
                    if (cmd.OutputManager.HasUCFOutput)
                    {
                        Write(tw, !cmdWithOutput.OutputManager.UCFTraceEndsWithNewLine, cmdWithOutput.OutputManager.GetUCFOuput());
                    }
                    if (cmd.OutputManager.HasVHDLOutput)
                    {
                        Write(tw, !cmdWithOutput.OutputManager.VHDLTraceEndsWithNewLine, cmdWithOutput.OutputManager.GetVHDLOuput());
                    }
                    if (cmd.OutputManager.HasTCLOutput)
                    {
                        Write(tw, !cmdWithOutput.OutputManager.TCLTraceEndsWithNewLine, cmdWithOutput.OutputManager.GetTCLOuput());
                    }
                    if (cmd.OutputManager.HasOutput)
                    {
                        Write(tw, !cmdWithOutput.OutputManager.OutputEndsWithNewLine, cmdWithOutput.OutputManager.GetOutput());
                    }
                    if (cmd.OutputManager.HasWrapperOutput)
                    {
                        Write(tw, !cmdWithOutput.OutputManager.WrapperTraceEndsWithNewLine, cmdWithOutput.OutputManager.GetWrapperOuput());
                    }

                    tw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void Write(TextWriter tw, bool writeLine, string text)
        {
            if (writeLine)
            {
                tw.WriteLine(text);
            }
            else
            {
                tw.Write(text);
            }

        }

        public override void ParseError(string cmd, string error)
        {
        }

        public override void ProgressUpdate(Command cmd)
        {
        }
    }

    /// <summary>
    /// Measures the execution time of each command
    /// </summary>
    class ProfilingHook : CommandHook
    {
        public override void CommandTrace(Command cmd)
        {
        }

        public override void PreRun(Command cmd)
        {
            if (cmd.Profile)
            {
                cmd.Watch.Start(cmd.GetType().Name);
            }
        }

        public override void Error(Command cmd, Exception error)
        {
            if (cmd.Profile)
            {
                PrintResults(cmd);
            }
        }

        public override void PostRun(Command cmd)
        {
            if (cmd.Profile)
            {
                PrintResults(cmd);

                m_totalProfile[cmd.GetType().Name] = cmd.Watch.TotalDuration;
            }
        }

        public override void ParseError(string cmd, string error)
        {
        }

        private void PrintResults(Command cmd)
        {
            cmd.Watch.Stop(cmd.GetType().Name);
            cmd.WriteProfilingResultsToOutput();
        }

        public override void ProgressUpdate(Command cmd)
        {
        }               

        public Dictionary<string, long> TotalProfile
        {
            get { return m_totalProfile; }
            set { m_totalProfile = value; }
        }

        private Dictionary<string, long> m_totalProfile = new Dictionary<string, long>();
    }

    class PrintProgressToConsoleHook : CommandHook
    {
        public override void CommandTrace(Command cmd)
        {
        }

        public override void PreRun(Command cmd)
        {
        }

        public override void Error(Command cmd, Exception error)
        {
        }

        public override void PostRun(Command cmd)
        {
            if (m_clearConsoleInPostRun && !CommandExecuter.Instance.MuteOutput && !cmd.Mute)
            {
                // clear console
                Console.Write("\r");
                Console.Write("     ");
                Console.Write("\r");
            }
        }

        public override void ParseError(string cmd, string error)
        {
        }

        public override void ProgressUpdate(Command cmd)
        {
            if (!CommandExecuter.Instance.MuteOutput && !cmd.Mute)
            {
                Console.Write("\r{0} %", cmd.ProgressInfo.Progress);
                m_clearConsoleInPostRun = true;
            }
        }

        private bool m_clearConsoleInPostRun = false;
    }

    class PrintProgressToGUIHook : CommandHook
    {
        public override void  CommandTrace(Command cmd)
        {
        }

        public override void PreRun(Command cmd)
        {
            ProgressBar.Value = 0;
        }

        public override void Error(Command cmd, Exception error)
        {
            ProgressBar.Value = 0;
        }

        public override void PostRun(Command cmd)
        {
            ProgressBar.Value = 0;
        }

        public override void ParseError(string cmd, string error)
        {
            ProgressBar.Value = 0;
        }

        public override void  ProgressUpdate(Command cmd)
        {
            if (!CommandExecuter.Instance.MuteOutput && !cmd.Mute && ProgressBar != null)
            {
                ProgressBar.PerformStep();
                foreach (Form f in m_forms)
                {
                    f.BringToFront();
                    f.Update();
                }
            }
        }

        public ToolStripProgressBar ProgressBar
        {
            get { return m_progressBar; }
            set { m_progressBar = value; }
        }

        private ToolStripProgressBar m_progressBar;
        public List<Form> m_forms = new List<Form>();
    
    }

}