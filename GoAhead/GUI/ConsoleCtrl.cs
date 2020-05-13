using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.TCL;

namespace GoAhead.GUI
{
    public partial class ConsoleCtrl : UserControl, Interfaces.IResetable
    {
        public ConsoleCtrl()
        {
            InitializeComponent();

            Commands.Reset.ObjectsToReset.Add(this);

            // geht das nicht im Wizard?
            m_txtInput.AllowDrop = true;
            m_txtInput.DragEnter += new DragEventHandler(m_txtInput_DragEnter);
            m_txtInput.DragDrop += new DragEventHandler(m_txtInput_DragDrop);
        }

        /// <summary>
        /// Clear all text boxes (expect commmand trace and input) upon reset
        /// </summary>
        public void Reset()
        {
            m_txtErrorTrace.Clear();
            m_txtOutputTrace.Clear();
            m_txtUCF.Clear();
            m_txtVHDL.Clear();
            m_txtTCL.Clear();
        }

        private Form parent = null;
        new public Form Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                Program.mainInterpreter.context = value;
            }
        }

        #region DragAndDrop
            
        private void m_txtInput_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void m_txtInput_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                m_txtInput.Clear();
                foreach (string file in files)
                {
                    m_txtInput.AppendText(File.ReadAllText(file));
                }
            }
            else if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string line = e.Data.GetData(DataFormats.Text).ToString();
                m_txtInput.AppendText(line);
            }
        }

        #endregion DrapAndDrop

        #region AddText
        public void AddToCommandTrace(Command cmd)
        {
            CommandStringParser parser = new CommandStringParser("");

            bool valid = parser.SplitCommand(cmd.ToString(), out string cmdTag, out string argumentPart);

            int offset = m_txtCommandTrace.TextLength;

            string cmdString = cmd.ToString() + Environment.NewLine;
            // dump command
            m_txtCommandTrace.AppendText(cmdString);
            
            if(!valid)
            {
                return;
            }

            // color command name red
            m_txtCommandTrace.Select(offset, offset + cmdTag.Length);
            m_txtCommandTrace.SelectionColor = Color.Red;

            // let offset point right behind the command
            offset += cmdTag.Length;

            // color argument names blue and argument values black
            if (!string.IsNullOrEmpty(argumentPart))
            {
                foreach (NameValuePair nameValuePair in parser.GetNameValuePairs(argumentPart))
                {
                    m_txtCommandTrace.Select(offset + nameValuePair.NameFrom, offset + nameValuePair.NameTo);
                    m_txtCommandTrace.SelectionColor = Color.Blue;

                    m_txtCommandTrace.Select(offset + nameValuePair.ValueFrom, offset + nameValuePair.ValueTo);
                    m_txtCommandTrace.SelectionColor = Color.Black;
                }
            }
            m_txtCommandTrace.DeselectAll();
            m_txtCommandTrace.ScrollToCaret();
            return;
           
            /*
            int index = 0;
            while (index < cmdString.Length && cmdString[index] != ' ' && cmdString[index] != ';')
            {
                index++;
            }
            this.m_txtCommandTrace.Select(offset, offset + index);
            this.m_txtCommandTrace.SelectionColor = Color.Red;

            this.m_txtCommandTrace.Select(offset + index, this.m_txtCommandTrace.TextLength);
            this.m_txtCommandTrace.SelectionColor = Color.Blue;
            this.m_txtCommandTrace.DeselectAll();
            this.m_txtCommandTrace.ScrollToCaret();*/
        }
        public void AddToErrorTrace(string msg)
        {
            m_txtErrorTrace.AppendText(msg);
            m_txtErrorTrace.ScrollToCaret();
        }
        public void AddToOutputTrace(string msg)
        {
            m_txtOutputTrace.AppendText(msg);
            m_txtOutputTrace.ScrollToCaret();
        }
        public void AddToWarningsTrace(string msg)
        {
            m_txtWarningsTrace.AppendText(msg);
            m_txtWarningsTrace.ScrollToCaret();
        }
        public void AddToUCFTrace(string msg)
        {
            m_txtUCF.AppendText(msg);
            m_txtUCF.ScrollToCaret();
        }
        public void AddToVHDLTrace(string msg)
        {
            m_txtVHDL.AppendText(msg);
            m_txtVHDL.ScrollToCaret();
        }
        public void AddToTCLTrace(string msg)
        {
            m_txtTCL.AppendText(msg);
            m_txtTCL.ScrollToCaret();
        }
        #endregion AddText

        #region ContextMenu
        private void m_ctxtMenuSelectAll_Click(object sender, EventArgs e)
        {
            m_origionOfRightClick.SelectAll();
        }

        private void m_ctxtMenuCopy_Click(object sender, EventArgs e)
        {
            m_origionOfRightClick.Copy();
        }

        private void m_ctxtMenuCopyAll_Click(object sender, EventArgs e)
        {
            m_origionOfRightClick.SelectAll();
            m_origionOfRightClick.Copy();
        }

        private void m_ctxtMenuDelete_Click(object sender, EventArgs e)
        {
            m_origionOfRightClick.Clear();
        }

        private void m_ctxtMenuPaste_Click(object sender, EventArgs e)
        {
            m_origionOfRightClick.Paste();
        }

        private void ShowContextMenu(object sender, MouseEventArgs e)
        {            
            m_ctxtMenu.Show(this, new Point(e.X, e.Y));
        }
        #endregion ContextMenu

        
        /// <summary>
        /// handle user cmds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_txtInputTrace_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                try
                {
                    CommandStringParser parser = new CommandStringParser(m_txtInput.Text);
                    foreach (string cmdString in parser.Parse())
                    {
                        Command cmd;
                        string errorDescr;
                        bool valid = parser.ParseCommand(cmdString, true, out cmd, out errorDescr);
                        if (valid)
                        {
                            CommandExecuter.Instance.Execute(cmd);
                        }
                        else
                        {
                            MessageBox.Show("Could not parse command " + Environment.NewLine + cmdString + Environment.NewLine + "Error: " + errorDescr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // update shell
                        Objects.CommandStackManager.Instance.Execute();

                        if (Parent != null)
                        {
                            Parent.Invalidate();
                        }
                    }
                
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "Error", MessageBoxButtons.OK);
                }
                m_txtInput.Clear();
            }

        }

        #region TCL Terminal
        private static readonly int TCL_input_memory_limit = 10;
        private List<string> TCL_input_memory = new List<string>();
        private int TCL_input_memory_navigator = -1;
        private void TCLTerminal_MaintainMemory(string newInput)
        {
            if(TCL_input_memory.Count == TCL_input_memory_limit)
            {
                TCL_input_memory.RemoveAt(0);
            }
            
            TCL_input_memory.Add(newInput);
            TCL_input_memory_navigator = TCL_input_memory.Count;
        }

        public static RichTextBox TCLTerminal_input => TCL_input;
        public static RichTextBox TCLTerminal_output => TCL_output;


        private void TCL_input_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Return)
            {
                e.SuppressKeyPress = true;
                TCL_input.Text = TCL_input.Text.Trim();
                TCLTerminal_MaintainMemory(TCL_input.Text);

                // TODO: handle r better
                int r = Program.mainInterpreter.EvalScript(TCL_input.Text);
                string result = Program.mainInterpreter.Result;

                // Echo the input unless it's 'clear'
                if (TCL_input.Text != "clear" && TCL_input.Text != "clearcontext")
                {
                    // Try to recognize the first word as a TCL command
                    bool appended = false;
                    string firstWord = TCL_input.Text, restOfStr = "";
                    int firstSpace = TCL_input.Text.IndexOf(' ');
                    if (firstSpace > 0)
                    {
                        firstWord = TCL_input.Text.Substring(0, firstSpace);
                        restOfStr = TCL_input.Text.Substring(firstSpace);
                    }
                    if (CommandsList.Commands.Contains(firstWord))
                    {
                        TCL_output.AppendText(firstWord, TCL.Procs.cshelp.CsHelp_GUI.color_standardTcl);
                        TCL_output.AppendText(restOfStr);
                        appended = true;
                    }
                    else if (CommandsList.ExtraCommands.Contains(firstWord))
                    {
                        TCL_output.AppendText(firstWord, TCL.Procs.cshelp.CsHelp_GUI.color_extraTcl);
                        TCL_output.AppendText(restOfStr);
                        appended = true;
                    }
                    if (!appended) TCL_output.AppendText(TCL_input.Text);
                    TCL_output.AppendText(Environment.NewLine);
                }                

                // Input's success
                if (r != 0)
                {
                    string errormsg = "ERROR" + Environment.NewLine;
                    string msg = Program.mainInterpreter.ErrorMessage;
                    if (msg != "") errormsg += msg + Environment.NewLine;
                    if (result != "") errormsg += result + Environment.NewLine;

                    TCL_output.AppendText(errormsg, Color.Red);
                    TclDLL.Tcl_ResetResult(Program.mainInterpreter.ptr);
                }
                else if (result.Length > 0)
                {
                    if (result.Length > 2000)
                    {
                        TCL_output.AppendText(result.Substring(0, 2000) + "...", Color.Blue);
                    }
                    else
                    {
                        TCL_output.AppendText(result, Color.Blue);
                    }

                    TCL_output.AppendText(Environment.NewLine);
                }

                // Clear input
                TCL_input.Text = "";
                TCL_output.SelectionStart = TCL_output.Text.Length;
                TCL_output.SelectionLength = 0;
                TCL_output.ScrollToCaret();

                //TCL_input.Focus();
            }
            else if (e.KeyValue == (char)Keys.Down)
            {
                e.SuppressKeyPress = true;
                if (TCL_input_memory_navigator <= -1) return;

                if (TCL_input_memory_navigator > 0) TCL_input_memory_navigator--;
                TCL_input.Text = TCL_input_memory[TCL_input_memory_navigator];
                TCL_input.SelectionStart = TCL_input.Text.Length;
                TCL_input.SelectionLength = 0;
            }
            else if (e.KeyValue == (char)Keys.Up)
            {
                e.SuppressKeyPress = true;
                if (TCL_input_memory_navigator <= -1 || TCL_input_memory_navigator == TCL_input_memory.Count) return;

                if (TCL_input_memory_navigator < TCL_input_memory.Count - 1) TCL_input_memory_navigator++;
                TCL_input.Text = TCL_input_memory[TCL_input_memory_navigator];
                TCL_input.SelectionStart = TCL_input.Text.Length;
                TCL_input.SelectionLength = 0;
            }
            /*else if (e.KeyValue == (char)Keys.Space)
            {
                string text = TCL_input.Text;
                if (!text.Contains(' '))
                {
                    if (CommandsList.Commands.Contains(text))
                    {
                        TCL_input.Text = "";
                        TCL_input.AppendText(text, Color.Red);
                    }
                }
            }*/
        }

        //source
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "C:\\";
            fileDialog.Title = "Select path";
            fileDialog.CheckFileExists = false;
            fileDialog.CheckPathExists = true;
            fileDialog.DefaultExt = "txt";
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string script = File.ReadAllText(fileDialog.FileName);
                /*for(int i=0; i<lines.Length; i++)
                {
                    int r = Program.mainInterpreter.EvalScript(lines[i]);
                    string result = Program.mainInterpreter.Result;
                    m_txtTCLInput.AppendText(result + (result == "" ? "" : Environment.NewLine));
                }*/
                int r = Program.mainInterpreter.EvalScript(script);
                if (r != 0) TclDLL.Tcl_SetObjResult(Program.mainInterpreter.ptr, TclAPI.Cs2Tcl("ERROR"));
                // TODO: 'Switch (r)' or something of the sort

                string result = Program.mainInterpreter.Result;
                TCL_input.AppendText(result + (result == "" ? "" : Environment.NewLine));
                TCL_input.SelectAll();
                TCL_input.SelectionProtected = true;
                TCL_input.SelectionStart = TCL_input.Text.Length;
                TCL_input.SelectionLength = 0;
                TCL_input.ScrollToCaret();
            }
        }

        #endregion

        #region ContextMenu
        private void m_txtCommandTrace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtCommandTrace;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtErrorTrace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtErrorTrace;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtOutputTrace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtOutputTrace;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtWarningsTrace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtWarningsTrace;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtInput_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtInput;
                m_ctxtMenuPaste.Visible = true;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtUCF_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtUCF;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtVHDL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtVHDL;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }

        private void m_txtTCL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_origionOfRightClick = m_txtTCL;
                m_ctxtMenuPaste.Visible = false;
                ShowContextMenu(sender, e);
            }
        }


        #endregion ContextMenu

        private void m_txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            string command = "";

            if (e.KeyValue == (char)Keys.Up)
            {
                command = Objects.CommandStackManager.Instance.Up();
            }
            else if (e.KeyValue == (char)Keys.Down)
            {
                command = Objects.CommandStackManager.Instance.Down();
            }

            if(!string.IsNullOrEmpty(command))
            {
                m_txtInput.Clear();
                m_txtInput.Text = command;
            }
        }

        /// <summary>
        /// current context menu txt bos
        /// </summary>
        private RichTextBox m_origionOfRightClick;

        }
}
