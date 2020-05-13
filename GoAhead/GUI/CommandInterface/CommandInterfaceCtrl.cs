using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Commands;

namespace GoAhead.GUI
{
    public partial class CommandInterfaceCtrl : UserControl
    {
        public CommandInterfaceCtrl()
        {
            InitializeComponent();
        }

        private void CommandInterfaceCtrl_Load(object sender, EventArgs e)
        {
            m_cmdBoxCommands.Items.Clear();
            foreach (Type t in CommandStringParser.GetAllCommandTypes())
            {
                bool publish = true;
                foreach (Attribute attr in Attribute.GetCustomAttributes(t))
                {
                    if (attr is CommandDescription)
                    {
                        CommandDescription descr = (CommandDescription)attr;
                        publish = descr.Publish;
                    }
                }
                if (!publish)
                {
                    continue;
                }
                m_cmdBoxCommands.Items.Add(t.Name);

            }
        }

        private void m_cmdBoxCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_cmdBoxCommands.SelectedItem != null)
            {
                string cmdTag = m_cmdBoxCommands.SelectedItem.ToString();
                UpdateParameters(cmdTag);
            }
        }

        private void m_cmdBoxCommands_TextChanged(object sender, EventArgs e)
        {
            string cmdTag = m_cmdBoxCommands.Text;
            UpdateParameters(cmdTag);
        }

        private void UpdateParameters(string cmdTag)
        {
            // clear previous data
            m_dataGrdArguments.Columns.Clear();
            m_dataGrdArguments.Columns.Add("Name", "Name");
            m_dataGrdArguments.Columns.Add("Value", "Value");
            m_dataGrdArguments.Columns.Add("Type", "Type");

            m_dataGrdArguments.Columns[0].ReadOnly = true;
            // the user will edit this column
            m_dataGrdArguments.Columns[1].ReadOnly = false;
            m_dataGrdArguments.Columns[2].ReadOnly = true;

            m_dataGrdArguments.Rows.Clear();

            // clear action text, otherwise the previos text would remain in case the selected command provides no action 
            m_txtAction.Text = "";

            Type type = CommandStringParser.GetAllCommandTypes().FirstOrDefault(t => t.Name.ToString().Equals(cmdTag));
            if (type == null)
            {
                return;
            }

            // create instance to get default value
            Command cmd = (Command)Activator.CreateInstance(type);

            // do not show unpublished commands
            if (!cmd.PublishCommand)
            {
                return;
            }

            // print command action to text box
            m_txtAction.Text = cmd.GetCommandDescription();

            // arguments
            foreach (FieldInfo fi in type.GetFields())
            {
                foreach (object obj in fi.GetCustomAttributes(true).Where(o => o is Parameter))
                {
                    Parameter pf = (Parameter)obj;
                    // skip e.g. Profile
                    if (pf.PrintParameter)
                    {
                        // argument name, default value, type
                        m_dataGrdArguments.Rows.Add(fi.Name, fi.GetValue(cmd), fi.FieldType.Name);
                        int index = m_dataGrdArguments.Rows.Count - 2;
                        if (index < m_dataGrdArguments.Rows.Count)
                        {
                            m_dataGrdArguments.Rows[index].Cells[0].ToolTipText = pf.Comment;
                            m_dataGrdArguments.Rows[index].Cells[1].ToolTipText = pf.Comment;
                            m_dataGrdArguments.Rows[index].Cells[2].ToolTipText = pf.Comment;
                        }
                    }
                }
            }            
        }

        private void m_btnExecute_Click(object sender, EventArgs e)
        {
            if (m_cmdBoxCommands.SelectedItem == null)
            {
                return;
            }

            string cmd = m_cmdBoxCommands.SelectedItem.ToString();
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }
            foreach (DataGridViewRow entry in m_dataGrdArguments.Rows)
            {
                if (entry.Cells.Count < 2)
                {
                    continue;
                }

                if (entry.Cells[0].Value == null || entry.Cells[1].Value == null)
                {
                    continue;
                }

                string name = entry.Cells[0].Value.ToString();
                string value = entry.Cells[1].Value.ToString();
                // remove leading and traling white spaces
                value = Regex.Replace(value, @"^\s*", "");
                value = Regex.Replace(value, @"\s*$", "");

                // quote value, might be "undefined file name"
                cmd += " " + name + "=\"" + value + "\"";
            }
            cmd += ";";
            CommandExecuter.Instance.Execute(cmd);
        }

        private void CommandInterfaceCtrl_Resize(object sender, EventArgs e)
        {
            int gap = 40;
            m_cmdBoxCommands.Width = Width - gap;
            m_txtAction.Width = Width - gap;
            m_dataGrdArguments.Width = Width - gap;
            m_btnExecute.Left = (Width - gap) - m_btnExecute.Width;


            m_dataGrdArguments.Height = Height - 200;
            m_btnExecute.Top = Height - 50;
        }
    }
}
