using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands
{
    [AttributeUsage(AttributeTargets.All)]
    public class Parameter : Attribute
    {
        public Parameter()
        {
            this.PrintParameter = true;
        }

        /// <summary>
        /// Whether to print out this Paramter into the command trace or not
        /// </summary>
        public bool PrintParameter
        {
            get { return this.m_printParamater; }
            set { this.m_printParamater = value; }
        }

        /// <summary>
        /// Some comment on the Parameter
        /// </summary>
        public String Comment
        {
            get { return this.m_comment; }
            set { this.m_comment = value; }
        }

        /// <summary>
        /// The backend types this Parameter applies to. 
        /// If this Paramter does not apply to the current backend, the Paraemter will not traced
        /// </summary>
        public FPGA.FPGATypes.BackendType Backend
        {
            get { return this.m_backendTypes; }
            set { this.m_backendTypes = value; }
        }

        private bool m_printParamater = false;
        private FPGA.FPGATypes.BackendType m_backendTypes = FPGA.FPGATypes.BackendType.All;
        private String m_comment = "";
    }

    public class CommandDescription : Attribute
    {
        public String Description { get; set; }
        public bool Wrapper { get; set; }
        public bool Publish { get; set; }

        public CommandDescription()
        {
            this.Description = "";
            this.Wrapper = false;
            this.Publish = true;
        }
    }

    [Serializable]
    public class CommandExecutionProgressInfo
    {
        public CommandExecutionProgressInfo(Command cmd)
        {
            this.m_command = cmd;
        }

        public int Progress
        {
            get 
            { 
                return this.m_progress; 
            }
            set 
            { 
                this.m_progress = value;             

                if (this.m_command.PrintProgress && this.m_progress >= this.m_nextHookShredshold)
                {
                    this.m_nextHookShredshold += 1;
                    foreach (CommandHook hook in CommandExecuter.Instance.GetAllHooks())
                    {
                        hook.ProgressUpdate(this.m_command);
                    }
                }
            }
        }

        private int m_nextHookShredshold = 0;
        private int m_progress;
        private readonly Command m_command;
    }

    [Serializable]
    public abstract class Command
	{
        public Command() { }
        
        /// <summary>
        /// Execute the command
        /// </summary>
        public void Do()
        {
            // best place??
            this.m_progressInfo = new CommandExecutionProgressInfo(this);

            // clean results from previous executions of the same instance
            this.m_errorThrown = false;

            this.OutputManager.Start();
            
            // call base class hook
            this.DoCommandAction();

            this.OutputManager.Stop();
        
        }

        /// <summary>
        /// The actual command specific action is implemented in the base classe
        /// </summary>
        protected abstract void DoCommandAction();
		public abstract void Undo();

        /// <summary>
        /// Print a parseable representation of the command for the command trace
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            String call = "";
            if (!String.IsNullOrEmpty(this.Comment))
            {                
                call += (this.Comment.StartsWith("#") ? "" : "# ") + this.Comment + System.Environment.NewLine;
            }
            call += this.GetType().Name;
         
            foreach (FieldInfo fi in this.GetType().GetFields())
            {
                // find ParamterField attr
                foreach (object obj in fi.GetCustomAttributes(true).Where(o => o is Parameter))
                {
                    // e.g. Profile paramter will not be printed out
                    Parameter par = (Parameter)obj;

                    if (!par.PrintParameter)
                    {
                        continue;
                    }

                    // do not trace paramters which apply to other backend types only
                    if (par.Backend != FPGA.FPGATypes.BackendType.All && par.Backend != FPGA.FPGA.Instance.BackendType)
                    {
                        continue;
                    }

                    String value = "";
                    // detect list of strings
                    if (fi.FieldType == typeof(List<String>))
                    {
                        List<String> list = (List<String>)fi.GetValue(this);
                        for (int i = 0; i < list.Count; i++)
                        {
                            value += list[i] + (i == list.Count - 1 ? "" : ",");
                        }
                    }
                    else if (fi.FieldType == typeof(List<int>))
                    {
                        List<int> list = (List<int>)fi.GetValue(this);
                        for (int i = 0; i < list.Count; i++)
                        {
                            value += list[i] + (i == list.Count - 1 ? "" : ",");
                        }
                    }
                    else
                    {
                        value = this.GetPrimitiveValue(fi);
                        // optionally replace paths by %GOAHEAD_HOME%
                        String goaheadHome = Environment.GetEnvironmentVariable("GOAHEAD_HOME");
                        if (!String.IsNullOrEmpty(goaheadHome))
                        {
                            if (value.StartsWith(goaheadHome))
                            {
                                if (Environment.OSVersion.VersionString.Contains("Windows"))
                                {
                                    value = value.Replace(goaheadHome, "%GOAHEAD_HOME%");
                                }
                                else
                                {
                                    value = value.Replace(goaheadHome, "${GOAHEAD_HOME}");
                                }
                            }
                        }
                    }
                    // quote name if it contains white spaces or a semicolon, e.g.
                    // 1) value="user value";
                    // 2) value="NOP;"
                    if (value.Contains(" ") || value.Contains(";"))
                    {
                        // do neither add double qoutes 
                        // in front of ...
                        if(!value.StartsWith("\""))
                        {
                            value = "\"" + value;
                        }
                        // .. or at the end of value
                        if(!value.EndsWith("\""))
                        {
                            value = value + "\"";
                        };
                    }

                    call += " " + fi.Name + "=" + value;
                }
            }
            // close command with semicolon
            call += ";";

            // insert line break for readibility
            if (call.Length > 200)
            {
                //call = Regex.Replace(call, " ", Environment.NewLine + "\t");
            }

            return call;
        }

        protected virtual String GetPrimitiveValue(FieldInfo fi)
        {
            if (fi.GetValue(this) == null)
            {
                return "";
            }
            else
            {
                return fi.GetValue(this).ToString();
            }
        }

        public void WriteProfilingResultsToOutput()
        {
            this.OutputManager.WriteOutput(this.Watch.GetResults());
        }

        /// <summary>
        /// Return the command description that is part of the class definition
        /// </summary>
        /// <returns></returns>
        public String GetCommandDescription()
        {
            Attribute attr = Attribute.GetCustomAttributes(this.GetType()).FirstOrDefault(a => a is CommandDescription);
            if (attr != null)
            {
                CommandDescription descr = (CommandDescription)attr;
                return descr.Description;
            }
            else
            {
                return "No command description available";
            }
        }

        public bool PublishCommand
        {
            get
            {
                Attribute attr = Attribute.GetCustomAttributes(this.GetType()).FirstOrDefault(a => a is CommandDescription);
                if (attr == null)
                {
                    // publish commands without description 
                    return true;
                }
                else
                {
                    CommandDescription descr = (CommandDescription)attr;
                    return descr.Publish;
                }
            }
        }
        
        public string GetHelpFilePath()
        {
            String goaheadHome = Environment.GetEnvironmentVariable("GOAHEAD_HOME");
            String helpFile = goaheadHome + System.IO.Path.DirectorySeparatorChar + "Help" + System.IO.Path.DirectorySeparatorChar + "Examples" + System.IO.Path.DirectorySeparatorChar + this.GetType().Name + ".txt";
            return helpFile;
        }

        public string GetNoteFilePath()
        {
            String goaheadHome = Environment.GetEnvironmentVariable("GOAHEAD_HOME");
            String helpFile = goaheadHome + System.IO.Path.DirectorySeparatorChar + "Help" + System.IO.Path.DirectorySeparatorChar + "Examples" + System.IO.Path.DirectorySeparatorChar + this.GetType().Name + "_note.txt";
            return helpFile;
        }

        /// <summary>
        /// Wheter the command executer shall execute this command or no
        /// If not, the command will however be passed to all hooks
        /// </summary>
        public bool Execute 
        {
            get { return this.m_execute; }
            set { this.m_execute = value; }
        }
        private bool m_execute = true;

        /// <summary>
        /// Wheter the command thre an error during execution
        /// </summary>
        public bool ErrorThrown
        {
            get { return this.m_errorThrown; }
            set { this.m_errorThrown = value; }
        }
        private bool m_errorThrown = false;
        
        /// <summary>
        /// This comment will be printed into the command trace
        /// </summary>
        public String Comment
        {
            get { return this.m_comment; }
            set { this.m_comment = value; }
        }
        private String m_comment = "";

        /// <summary>
        /// Whether the command will be printed to the command trace
        /// </summary>
        public bool UpdateCommandTrace 
        {
            get { return this.m_updateCommandTrace; }
            set { this.m_updateCommandTrace = value; }
        }
        private bool m_updateCommandTrace = true;

        /// <summary>
        /// The original parsed in string represantion before any Defines or Variables have been resolved
        /// </summary>
        public String OriginalCommandString
        {
            get { return m_originalCommandString; }
            set { m_originalCommandString = value; }
        }         
        private String m_originalCommandString = "";

        public CommandExecutionProgressInfo ProgressInfo
        {
            get { return this.m_progressInfo; }
        }
        private CommandExecutionProgressInfo m_progressInfo = null;

        public CommandOutputManager OutputManager
        {
            get 
            {
                if (this.m_outputManager == null)
                {
                    this.m_outputManager = new CommandOutputManager();
                }
                return this.m_outputManager;
            }
            set { this.m_outputManager = value; }
        }
        private CommandOutputManager m_outputManager = null;

        /// <summary>
        /// The total progress share of this command.
        /// Wrapping command set this value to e.g 40 and thus indicate this command contributes 40% of the overall workload of the wrapped command.
        /// The default is 100 (stand alone command)
        /// </summary>
        public int ProgressShare
        {
            get { return this.m_progressShare; }
            set { this.m_progressShare = value; }
        }
        private int m_progressShare = 100;

        /// <summary>
        /// The start value for the progress of this command.
        /// Wrapping command set this value to e.g 60 and thus indicate the the before wrapped command contributed to 60% of the overall workload of the wrapped command.
        /// The default is 0 (stand alone command)
        /// </summary>
        public int ProgressStart
        {
            get { return this.m_progressStart; }
            set { this.m_progressStart = value; }
        }
        private int m_progressStart = 0;

        /// <summary>
        /// A watch to trace the execution time of the command
        /// </summary>
        public Watch Watch
        {
            get 
            {
                if (this.m_watch == null)
                {
                    this.m_watch = new Watch();
                }
                return this.m_watch; 
            }
            set { this.m_watch = value; }
        }
        private Watch m_watch = null;

        //[Parameter(Comment = "The condition is an arithmetic expression that is considered as false if it evaluates to 0 and considered as true if it evalauates to a value not equal to 0")]
        //public String Condition = "a<10000";

        [Parameter(Comment = "Whether to measure the execution time of this command", PrintParameter = false)]
        public bool Profile = false;

        [Parameter(Comment = "Whether mute output (if any) of this command", PrintParameter = false)]
        public bool Mute = false;

        [Parameter(Comment = "Whether to print the current progress on the console", PrintParameter = false)]
        public bool PrintProgress = false;
    }

   
}

