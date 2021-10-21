using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using GoAhead.Commands;
using GoAhead.Objects;
using GoAhead.Settings;
using System.Runtime.InteropServices;
using GoAhead.TCL;

namespace GoAhead
{
	class Program
	{
        public static TclInterpreter mainInterpreter;
        private static TclDLL.Tcl_ObjCmdProc ExecuteGOAcmd;

        [STAThread]
		static void Main(string[] args)
		{
            // TCL API init
            //TclAPI.Initialize();
            mainInterpreter = new TclInterpreter();
            unsafe
            {
                ExecuteGOAcmd = TclProcs.ExecuteGOACommand;
                TclDLL.Helper_RegisterProc(mainInterpreter.ptr, "cs", TclProcs.Cs);
                TclDLL.Helper_RegisterProc(mainInterpreter.ptr, "cshelp", TclProcs.CsHelp);
                TclDLL.Helper_RegisterProc(mainInterpreter.ptr, "cslist", TclProcs.CsList);
                TclDLL.Helper_RegisterProc(mainInterpreter.ptr, "clear", TclProcs.Clear);
                TclDLL.Helper_RegisterProc(mainInterpreter.ptr, "clearcontext", TclProcs.ClearContext);
                //TclDLL.Helper_RegisterProc(mainInterpreter.ptr, "test", TclProcs.Test);
            }
            //int rc = mainInterpreter.EvalScript("puts [testproc Tile]");
            //Console.WriteLine("rc=" + rc + " Interp.Result = " + mainInterpreter.Result);

            // restore settings
            StoredPreferences.LoadPrefernces();

            // check vars
            StringBuilder errorList;
            if (!EnvChecker.CheckEnv(out errorList))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(errorList.ToString());
                Console.ResetColor();                
            }
        
            // detect commands without default constructor
            foreach (Type type in CommandStringParser.GetAllCommandTypes())
            {
                try
                {
                    Command cmd = (Command)Activator.CreateInstance(type);
                    TclDLL.Helper_RegisterProc(mainInterpreter.ptr, type.Name, ExecuteGOAcmd);
                    unsafe
                    {
                        //string[] parts = cmd.ToString().Split(' ');
                        //if (parts[0].EndsWith(";")) parts[0] = parts[0].Substring(0, parts[0].Length - 1);
                        TclDLL.Helper_RegisterProc(mainInterpreter.ptr, type.Name, ExecuteGOAcmd);
                    }                    
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: No default constructor found for command " + type.Name);
                    Console.ResetColor();
                }
            }

            

            // register console hook
            // first hook print progress to clean the % output
            CommandExecuter.Instance.AddHook(new PrintProgressToConsoleHook());
            // the profiling hook must be added before the output hook hooks as it produces output
            CommandExecuter.Instance.AddHook(new ProfilingHook());
            CommandExecuter.Instance.AddHook(new ConsoleCommandHook());
            CommandExecuter.Instance.AddHook(new PrintOutputHook());

            // check if init.goa is found in binary of the current assembly
            string dir = AssemblyDirectory;
            string initFile = dir + Path.DirectorySeparatorChar + "init.goa";

            // if so, execute init.goa
            if (File.Exists(initFile))
            {
                RunScript runInitCmd = new RunScript();
                runInitCmd.FileName = initFile;
                CommandExecuter.Instance.Execute(runInitCmd);
                //FileInfo fi = new FileInfo(initFile);
                //CommandExecuter.Instance.Execute(fi);
            }
            else
            {
                Console.WriteLine("GoAhead did not find the init file: " + initFile);
            }

            bool showGUIOnly = false;
            bool execScript = false;
            string scriptFile = "";
            bool shellMode = false;
            bool serverMode = false;
            int portNumber = 0;
            bool commandMode = false;

            if (args.Length == 0)
            {
                showGUIOnly = true;
            }
            else
            {
                int i = 0;
                while(i < args.Length)
                {
                    switch (args[i])
                    {
                        case "-gui":
                            showGUIOnly = true;
                            break;                      
                        case "-exec":
                            execScript = true;
                            scriptFile = GetScriptFileName(args, i+1);
                            i++;
                            break;
                        case "-shell":
                            shellMode = true;
                            break;
                        case "-command":
                        case "-commands":
                            commandMode = true;
                            break;
                        case "-server":
                            try
                            {
                                portNumber = int.Parse(args[i + 1]);
                                i++;
                                serverMode = true;

                            }
                            catch (IndexOutOfRangeException ex)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("No port number supplied for server mode!");
                                Console.ResetColor();
                                serverMode = false;
                            }
                            break;
                        default:
                            if (args[i].EndsWith(".goa") && File.Exists(args[i]))
                            {
                                execScript = true;
                                scriptFile = GetScriptFileName(args, i);
                            }
                            break;
                    }
                    i++;
                }
                
            }
            if (showGUIOnly)
            {
                // open gui
                CommandExecuter.Instance.Execute(new Commands.GUI.ShowGUI());
            }
            else if (execScript)
			{
                if (!File.Exists(scriptFile))
                {
                    string errorMessage = "Error: File " + scriptFile + " not found";
                    // allow the test scripts to catch this string (goahead -exec script.goa | tee.goa)
                    Console.WriteLine(errorMessage);
                    throw new ArgumentException(errorMessage);
                }

                // command file mode
                FileInfo fi = new FileInfo(scriptFile);
                CommandExecuter.Instance.Execute(fi);
			}
            else if (shellMode)
            {
                Objects.CommandShell shell = new Objects.CommandShell();
                shell.Run();
            }
            else if (serverMode)
            {
                Objects.CommandServer server = new Objects.CommandServer();
                server.Run(portNumber);
            }
            else if (commandMode)
            {
                string cmdString = "";
                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        cmdString += args[i] + " ";
                    }
                }
                if (string.IsNullOrEmpty(cmdString))
                {
                    Console.WriteLine("GoAhead was started with -commands, but no command was given");
                }
                Command cmd;
                string errorDescr;
                CommandStringParser parser = new CommandStringParser(cmdString);
                foreach (string subCommandString in parser.Parse())
                {
                    bool valid = parser.ParseCommand(subCommandString, true, out cmd, out errorDescr);
                    if (!valid)
                    {
                        Console.WriteLine(errorDescr);
                    }
                    CommandExecuter.Instance.Execute(cmd);
                }
            }
            else
            {
                Console.WriteLine("No switch found. Start GoAhead with one of the following options:");
                Console.WriteLine("GoAhead -gui                     : Open GoAhead in GUI-Mode");
                Console.WriteLine("GoAhead -exec script.goa         : Execute script.goa");
                Console.WriteLine("GoAhead script.goa               : Execute script.goa");
                Console.WriteLine("GoAhead -shell                   : Start GoAhead shell (interactive Command mode)");
                Console.WriteLine("GoAhead -command(s)              : Execute GoAhead commands (e.g GoAhead -command \"FixS6XDLBug XDLInFile=in.xdl XDLOutFile=out.xdl;\"");
                Console.WriteLine("GoAhead.exe -server portNumber   : Execute GoAhead in server mode on the specified port");
            }

            // save settings
            StoredPreferences.SavePrefernces();
		}

        private static string GetScriptFileName(string[] args, int i)
        {
            if (i >= args.Length)
            {
                string errorMessage = "Error: -exec must be followed by a script, e.g. -exec script.goa";
                // allow the test scripts to catch this string (goahead -exec script.goa | tee.goa)
                Console.WriteLine(errorMessage);
                throw new ArgumentException(errorMessage);
            }
            string scriptFile = args[i];
            scriptFile = Regex.Replace(scriptFile, @"\s*$", string.Empty);
            return scriptFile;
        }

        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

	}
}
