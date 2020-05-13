using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Settings;

namespace GoAhead.GUI.ScriptDebugger
{
    public partial class ScriptDebuggerCtrl : UserControl
    {
        public ScriptDebuggerCtrl()
        {
            InitializeComponent();

            m_eventHanlder = new FileSystemEventHanlder(this);
            ReadloadScriptDelegateInstance = new ReadloadScriptDelegate(ReloadScript);

            m_txtCmds.WordWrap = false;
            m_txtLineNumber.WordWrap = false;

            m_timer.Interval = 1000;
            m_timer.Tick += ShowToolTipAfterTimerFired;
           
        }

        public void Close()
        {
            m_fsw.EnableRaisingEvents = false;
        }

        private void m_btnBrowseForScript_Click_1(object sender, EventArgs e)
        {
            string caller = "m_btnBrowseForScript_Click";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a Script File";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "GoAhead Script File|*.goa";

            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }
            
            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK || string.IsNullOrEmpty(openFileDialog.FileName))
            {
                return;
            }
            
            m_lblScriptFiled.Text = "No Script File loaded";
            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, Path.GetDirectoryName(openFileDialog.FileName));

            LoadScript(openFileDialog.FileName);
        }

        private void LoadScript(string fileName)
        {
            m_eventHanlder.FileName = fileName;
            m_lblScriptFiled.Text = fileName;

            m_fsw = new FileSystemWatcher();
            m_fsw.Path = Directory.GetParent(fileName).FullName;
            m_fsw.EnableRaisingEvents = true;
            m_fsw.Changed += new FileSystemEventHandler(m_eventHanlder.ScriptFileChanged);

            ReloadScript();
        }

        private void m_btnReload_Click(object sender, EventArgs e)
        {
            ReloadScript();
        }

        private void ReloadScript()
        {
            string scriptFile = m_lblScriptFiled.Text;
            if (!File.Exists(scriptFile))
            {
                MessageBox.Show("Script file not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_txtCmds.Clear();
            m_txtLineNumber.Clear();
            m_commandIndeces.Clear();
            m_commandIndex = 0;
            m_form = 0;
            m_length = 0;
            m_lblCmd.Text = "No command selected";

            // copy file into text box
            StreamReader sr = new StreamReader(scriptFile);
            string line = "";

            int lineCount = 0;
            while ((line = sr.ReadLine()) != null)
            {
                m_txtCmds.AppendText(line + Environment.NewLine);
                m_txtLineNumber.AppendText((lineCount + 1).ToString() + Environment.NewLine);
                lineCount++;
            }
            sr.Close();

            m_context = new CommandExecutionContext();

            CommandStringParser parser = new CommandStringParser(m_txtCmds.Text);
            foreach (string cmdStr in parser.Parse())
            {
                Command cmd;
                string errorDescr;
                bool valid = parser.ParseCommand(cmdStr, false, out cmd, out errorDescr);
                if (valid)
                {
                    m_context.Parsed(cmdStr, cmd);
                }
                else
                {
                    MessageBox.Show(errorDescr + Environment.NewLine + parser.State.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            m_context.SetLabels();

            foreach (Tuple<int, int> range in parser.Topology.GetBorders(CommandStringTopology.TopologyType.Comment))
            {
                m_txtCmds.Select(range.Item1, (range.Item2 - range.Item1) + 1);
                m_txtCmds.SelectionColor = Color.Green;
            }
            foreach (Tuple<int, int> range in parser.Topology.GetBorders(CommandStringTopology.TopologyType.CommandTag))
            {
                m_txtCmds.Select(range.Item1, (range.Item2 - range.Item1) + 1);
                m_txtCmds.SelectionColor = Color.Red;               
            }
            foreach (Tuple<int, int> range in parser.Topology.GetBorders(CommandStringTopology.TopologyType.ArgumentNames))
            {
                m_txtCmds.Select(range.Item1, (range.Item2 - range.Item1) + 1);
                m_txtCmds.SelectionColor = Color.Blue;
            }
            foreach (Tuple<int, int> range in parser.Topology.GetBorders(CommandStringTopology.TopologyType.ArgumentValues))
            {
                m_txtCmds.Select(range.Item1, (range.Item2 - range.Item1) + 1);
                m_txtCmds.SelectionColor = Color.Black;
            }
            foreach (Tuple<int, int> range in parser.Topology.GetBorders(CommandStringTopology.TopologyType.CompleteCommand))
            {
                m_commandIndeces.Add(new Tuple<int, int>(range.Item1, range.Item2));
            }

            SetSelectedItemToNextCommandListView();
        }

        private void m_btnClear_Click(object sender, EventArgs e)
        {
            m_txtCmds.Clear();
            m_lblScriptFiled.Text = "No Script File loaded";

            m_fsw.EnableRaisingEvents = false;

            m_lblCmd.Text = "";
        }
        
        private void SetSelectedItemToNextCommandListView()
        {
            m_txtCmds.Select(m_form, m_length);
            //String debug = this.m_txtCmds.Text.Substring(this.m_form, this.m_length);
            if (m_breakPoints.FirstOrDefault(t => t.Item1 == m_form) != null)
            {
                m_txtCmds.SelectionBackColor = m_breakpointColor;
            }
            else
            {
                m_txtCmds.SelectionBackColor = m_defaultColor;
            }
            m_txtCmds.DeselectAll();

            // wrap around
            if (m_commandIndex == m_commandIndeces.Count)
            {
                m_commandIndex = 0;
            }

            if (m_commandIndex < m_commandIndeces.Count)
            {               
                m_length = m_commandIndeces[m_commandIndex].Item2 - m_commandIndeces[m_commandIndex].Item1;
                m_form = m_commandIndeces[m_commandIndex].Item1;

                m_txtCmds.Select(m_form, m_length);
                m_selectedText = m_txtCmds.SelectedText;
                m_txtCmds.SelectionBackColor = m_nextCommandColor;
                m_lblCmd.Text = "Next command: " + m_txtCmds.SelectedText;
                m_txtCmds.DeselectAll();
            }
        }

        /// <summary>
        /// Execute the next command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnExecuteNextCmd_Click(object sender, EventArgs e)
        {
            ExecuteNextCmd();
        }

        /// <summary>
        /// Execute commands until breakpoint is reached
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_btnRun_Click(object sender, EventArgs e)
        {
            while (true)
            {
                bool furtherCommandFound = ExecuteNextCmd();

                // all commands done
                if (!furtherCommandFound)
                {
                    break;
                }
                
                // breakpoint reached
                int from = m_commandIndeces[m_commandIndex].Item1;
                if (m_breakPoints.FirstOrDefault(t => t.Item1 == from) != null)
                {
                    break;
                }
            }
        }

        private bool ExecuteNextCmd()
        {
            // empty list
            if (m_txtCmds.TextLength == 0)
            {
                MessageBox.Show("No command script loaded", "Error", MessageBoxButtons.OK);
                return false;
            }

            if (string.IsNullOrEmpty(m_selectedText))
            {
                MessageBox.Show("No command selected", "Error", MessageBoxButtons.OK);
                return false;
            }

            int nextCommandIndex = m_context.Execute(m_commandIndex);

            //Commands.CommandExecuter.Instance.Execute(this.m_selectedText);

            //bool furherCommandFound = this.m_commandIndex + 1 < this.m_commandIndeces.Count;
            bool furherCommandFound = nextCommandIndex < m_commandIndeces.Count;
            m_commandIndex = nextCommandIndex;

            SetSelectedItemToNextCommandListView();

            // update views
            if (m_InvalidateMeAfterEachCommand != null)
            {
                m_InvalidateMeAfterEachCommand.Invalidate();
            }

            return furherCommandFound;
        }

        private void m_btnStop_Click(object sender, EventArgs e)
        {
            m_commandIndex = 0;
            SetSelectedItemToNextCommandListView();
        }

        private void ScriptDebugger_Load(object sender, EventArgs e)
        {
            if (File.Exists(m_scriptToLoadAtStartup))
            {
                LoadScript(m_scriptToLoadAtStartup);
            }

            // Create the ToolTip and associate with the Form container.
            ToolTip toolTips = new ToolTip();

            // Set up the delays for the ToolTip.
            toolTips.AutoPopDelay = 5000;
            toolTips.InitialDelay = 1000;
            toolTips.ReshowDelay = 500;
            // Force the ToolTip text to be displayed whether or not the form is active.
            toolTips.ShowAlways = true;

            // Set up the ToolTip text for the Button and Checkbox.
            toolTips.SetToolTip(m_btnExecuteNextCmd, "Execute the currently selected line");
            toolTips.SetToolTip(m_btnRun, "Execute all commands until the next breakpoint is reached");
            toolTips.SetToolTip(m_btnStop, "Jump back to begin of script file");
            toolTips.SetToolTip(m_btnBrowseForScript, "Load a script file");
            toolTips.SetToolTip(m_btnClear, "Clear all commands in text box");
        }

        #region ContextMenu
       
        private void m_setBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int from = m_commandIndeces[m_rightClickedCommandIndex].Item1;
            int length = m_commandIndeces[m_rightClickedCommandIndex].Item2 - m_commandIndeces[m_rightClickedCommandIndex].Item1;

            if (m_breakPoints.FirstOrDefault(t => t.Item1 == from) == null)
            {
                m_breakPoints.Add(new Tuple<int,int>(from, length));
                m_txtCmds.Select(from, length);
                m_txtCmds.SelectionBackColor = m_breakpointColor;
                m_txtCmds.DeselectAll();
            }
        }

        private void m_deleteBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int from = m_commandIndeces[m_rightClickedCommandIndex].Item1;
            int length = m_commandIndeces[m_rightClickedCommandIndex].Item2 - m_commandIndeces[m_rightClickedCommandIndex].Item1;

            Tuple<int, int> breakPointInfo = m_breakPoints.FirstOrDefault(t => t.Item1 == from);
            if (breakPointInfo != null)
            {
                m_txtCmds.Select(from, length);
                m_txtCmds.SelectionBackColor = m_defaultColor;
                m_txtCmds.DeselectAll();

                m_breakPoints.Remove(breakPointInfo);
            }
        }

        private void m_removeAllBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Tuple<int, int> pair in m_breakPoints)
            {
                m_txtCmds.Select(pair.Item1, pair.Item2);
                m_txtCmds.SelectionBackColor = m_defaultColor;
                m_txtCmds.DeselectAll();
            }
            m_breakPoints.Clear();
            SetSelectedItemToNextCommandListView();
        }

        #endregion ContextMenu

        private void ScriptDebuggerCtrl_Resize(object sender, EventArgs e)
        {
            if (Width < 200)
            {
                Width = 200;
            }
            if (Height < 400)
            {
                Height = 400;
            }

            //Console.WriteLine("W" + this.Width);
            //Console.WriteLine("H" + this.Height);

            int left = 5;
            int top = 5;
            double listViewShare = 0.8;
            int height = (int)((double)Height * listViewShare) - 4 * top;

            m_txtLineNumber.Left = left;
            m_txtLineNumber.Top = top;
            m_txtLineNumber.Height = height;

            m_txtCmds.Left = left + m_txtLineNumber.Width - 18;
            m_txtCmds.Top = top;
            m_txtCmds.Height = height;
            m_txtCmds.Width = (Width - 4 * left) - m_txtLineNumber.Width + 18;

            m_grpBoxControls.Left = left;
            m_grpBoxControls.Top = top + m_txtCmds.Height + top;
            m_grpBoxControls.Height = (int)((double)Height * (1-listViewShare)) - 4*top;
            m_grpBoxControls.Width = Width-4*left;           
        }

        private void m_txtCmds_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_lstViewContextMenu.Show(this, e.Location);
                int cmdIndex = GetCommandIndex(e.Location);
                if (cmdIndex != -1)
                {
                    m_rightClickedCommandIndex = cmdIndex;
                }
            }
        }

        private void m_txtCmds_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int cmdIndex = GetCommandIndex(e.Location);
                if (cmdIndex != -1)
                {
                    m_commandIndex = cmdIndex;
                    SetSelectedItemToNextCommandListView();
                }
            }
        }

        private int GetCommandIndex(Point p)
        {
            int positionToSearch = m_txtCmds.GetCharIndexFromPosition(p);
            int cmdIndex = m_commandIndeces.FindIndex(t => t.Item1 <= positionToSearch && positionToSearch <= t.Item2);
            return cmdIndex;
        }
 
        public string ScriptToLoadAtStartup
        {
            get { return m_scriptToLoadAtStartup; }
            set { m_scriptToLoadAtStartup = value; }
        }

        public Form InvalidateMeAfterEachCommand
        {
            get 
            { 
                return m_InvalidateMeAfterEachCommand; 
            }
            set 
            { 
                m_InvalidateMeAfterEachCommand = value; 
            }
        }

        #region Tooltips
        private void ShowToolTipAfterTimerFired(object sender, EventArgs e)
        {
            if (StoredPreferences.Instance.ShowToolTips)
            {
                Point p = (Point)m_timer.Tag;

                // only show a new tool tip, if the position changes
                if (m_lastToolTipLocation.Equals(p))
                {
                    return;
                }

                m_lastToolTipLocation = p;

                int cmdIndex = GetCommandIndex(p);
                if (cmdIndex == -1)
                {
                    return;
                }

                int from = m_commandIndeces[cmdIndex].Item1;
                int length = m_commandIndeces[cmdIndex].Item2 - m_commandIndeces[cmdIndex].Item1;

                string commandString = m_txtCmds.Text.Substring(from, length);

                Command cmd = null;
                string errorDescription = "";
                CommandStringParser parser = new CommandStringParser(commandString);
                bool valid = parser.ParseCommand(commandString, false, out cmd, out errorDescription);
                if (valid)
                {
                    // print command action to text box
                    foreach (Attribute attr in Attribute.GetCustomAttributes(cmd.GetType()).Where(a => a is CommandDescription))
                    {
                        CommandDescription descr = (CommandDescription)attr;
                        string toolTip = descr.Description;

                        m_toolTip.Show(toolTip, this, p.X, p.Y + 20, 10000);
                        break;
                    }
                }
            }
        }

        private void m_txtCmds_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_mouseEntered && StoredPreferences.Instance.ShowToolTips)
            {
                // start timer for toll tip
                m_timer.Stop();
                m_timer.Start();
                // store position
                m_timer.Tag = e.Location;
            }
        }

        private void m_txtCmds_MouseEnter(object sender, EventArgs e)
        {
            m_mouseEntered = true;
        }

        private void m_txtCmds_MouseLeave(object sender, EventArgs e)
        {
            m_mouseEntered = false;
        }
        #endregion

        private string m_scriptToLoadAtStartup = "";
        private CommandExecutionContext m_context = new CommandExecutionContext();
               
        private bool m_mouseEntered = false;
        private ToolTip m_toolTip = new ToolTip();
        private Timer m_timer = new Timer();
        /// <summary>
        /// Strange: The mouse move event occurs after the tooltip disappears
        /// Hoever, the location property does not change with the event
        /// In order not to show the tooltip again, we store the last position and only reshow the tooltip if the position changes
        /// </summary>
        private Point m_lastToolTipLocation = new Point(0, 0);

        private readonly Color m_breakpointColor = Color.Orange;
        private readonly Color m_nextCommandColor = Color.DarkGray;
        private readonly Color m_defaultColor = Color.FromName("Control");
        private string m_selectedText = "";
        private int m_form = 0;
        private int m_length = 0;
        private int m_commandIndex = 0;
        private int m_rightClickedCommandIndex = 0;
        private Form m_InvalidateMeAfterEachCommand = null;


        private List<Tuple<int, int>> m_commandIndeces = new List<Tuple<int, int>>();
        private List<Tuple<int, int>> m_breakPoints = new List<Tuple<int, int>>();

        private FileSystemWatcher m_fsw = new FileSystemWatcher();

        public delegate void ReadloadScriptDelegate();
        public ReadloadScriptDelegate ReadloadScriptDelegateInstance;
        private FileSystemEventHanlder m_eventHanlder;
    }

    public class FileSystemEventHanlder
    {
        public FileSystemEventHanlder(ScriptDebuggerCtrl debuggerCtrl)
        {
            m_debuggerCtrl = debuggerCtrl;
        }

        public void ScriptFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(FileName))
            {
                return;

            }
            DateTime lastChange = File.GetLastWriteTime(FileName);
            if (!e.FullPath.Equals(FileName) || e.ChangeType != WatcherChangeTypes.Changed || m_lastChange == lastChange)
            {
                return;
            }

            m_lastChange = lastChange;

            DialogResult result = MessageBox.Show("GOA Script File Changed. Reload?", "File Changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                m_debuggerCtrl.Invoke(m_debuggerCtrl.ReadloadScriptDelegateInstance);
            }
        }

        private ScriptDebuggerCtrl m_debuggerCtrl;
        /// <summary>
        /// stores the last occurences on OnChange event
        /// </summary>
        private DateTime m_lastChange = DateTime.MinValue;

        public string FileName = "";
    }
}
