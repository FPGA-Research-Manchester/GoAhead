using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Commands.Data;
using GoAhead.Code;

namespace GoAhead.GUI.ReadVivadoFPGADebugger
{
    public partial class ReadVivadoFPGADebugger : Form
    {
        public ReadVivadoFPGADebugger()
        {
            InitializeComponent();

            flow_wireIncludesExpand.MouseHover += Flow_Expand_MouseHover;
            flow_wireIncludesExpand.MouseLeave += Flow_Expand_MouseLeave;
            flow_wireIgnoresExpand.MouseHover += Flow_Expand_MouseHover;
            flow_wireIgnoresExpand.MouseLeave += Flow_Expand_MouseLeave;

            // Fill in the fields with the existing values
            txtbox_path.Text = Commands.Data.ReadVivadoFPGADebugger.FilePath;
            radioButton3.Checked = Commands.Data.ReadVivadoFPGADebugger.Mode == Commands.Data.ReadVivadoFPGADebugger.OutputMode.Complete;
            radioButton4.Checked = Commands.Data.ReadVivadoFPGADebugger.Mode == Commands.Data.ReadVivadoFPGADebugger.OutputMode.Compact;
            checkBox_IIW.Checked = Commands.Data.ReadVivadoFPGADebugger.IgnoreIdenticalLocationWires;

            List<string[]> wireIncludes = Commands.Data.ReadVivadoFPGADebugger.GetWireRegexStrings(Commands.Data.ReadVivadoFPGADebugger.RegexType.Include);
            if (wireIncludes != null)
            {
                for (int i = 0; i < wireIncludes.Count; i++)
                {
                    AddTableRow(true);
                    for (int j = 0; j < 4; j++)
                        ((RichTextBox)table_includes.GetControlFromPosition(j, i + 1)).Text = wireIncludes[i][j];
                }
            }
            else
            {
                AddTableRow(true);
                for (int j = 0; j < 4; j++)
                    ((RichTextBox)table_includes.GetControlFromPosition(j, 1)).Text = ".*";
            }

            List<string[]> wireIgnores = Commands.Data.ReadVivadoFPGADebugger.GetWireRegexStrings(Commands.Data.ReadVivadoFPGADebugger.RegexType.Ignore);
            if (wireIgnores != null)
            {
                for (int i = 0; i < wireIgnores.Count; i++)
                {
                    AddTableRow(false);
                    for (int j = 0; j < 4; j++)
                        ((RichTextBox)table_ignores.GetControlFromPosition(j, i + 1)).Text = wireIgnores[i][j];
                }
            }
        }

        private void OnProceed(object sender, EventArgs e)
        {
            // General setup
            ReadVivadoFPGADebugger_Setup cmd_setup = new ReadVivadoFPGADebugger_Setup
            {
                DebugFileOutput = txtbox_path.Text,
                Format = (radioButton3.Checked ? "Complete" : "Compact"),
                IgnoreIdenticalLocationWires = checkBox_IIW.Checked
            };
            CommandExecuter.Instance.Execute(cmd_setup);

            // Remove exiting regexes
            ReadVivadoFPGADebugger_DeleteWireRegexes cmd_wires = new ReadVivadoFPGADebugger_DeleteWireRegexes();
            CommandExecuter.Instance.Execute(cmd_wires);

            // Add wire includes
            for (int i = 1; i < table_includes.RowCount; i++)
            {
                ReadVivadoFPGADebugger_AddWireInclude cmd_include = new ReadVivadoFPGADebugger_AddWireInclude
                {
                    WireRegex_StartTile = ((RichTextBox)table_includes.GetControlFromPosition(0, i)).Text,
                    WireRegex_StartPort = ((RichTextBox)table_includes.GetControlFromPosition(1, i)).Text,
                    WireRegex_EndTile = ((RichTextBox)table_includes.GetControlFromPosition(2, i)).Text,
                    WireRegex_EndPort = ((RichTextBox)table_includes.GetControlFromPosition(3, i)).Text
                };
                CommandExecuter.Instance.Execute(cmd_include);
            }

            // Add wire ignores
            for (int i = 1; i < table_ignores.RowCount; i++)
            {
                ReadVivadoFPGADebugger_AddWireIgnore cmd_ignore = new ReadVivadoFPGADebugger_AddWireIgnore
                {
                    WireRegex_StartTile = ((RichTextBox)table_ignores.GetControlFromPosition(0, i)).Text,
                    WireRegex_StartPort = ((RichTextBox)table_ignores.GetControlFromPosition(1, i)).Text,
                    WireRegex_EndTile = ((RichTextBox)table_ignores.GetControlFromPosition(2, i)).Text,
                    WireRegex_EndPort = ((RichTextBox)table_ignores.GetControlFromPosition(3, i)).Text
                };
                CommandExecuter.Instance.Execute(cmd_ignore);
            }

            Hide();
            GUI.ReadVivadoFPGADialog(checkBox_excludePips.Checked);
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Hide();
            ReadVivadoFPGADebugger_Reset cmd = new ReadVivadoFPGADebugger_Reset();
            CommandExecuter.Instance.Execute(cmd);
        }

        private void pathBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.Title = "Select path";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbox_path.Text = saveFileDialog1.FileName;
            }
        }

        private static string NL => Environment.NewLine;
        private void button2_Click(object sender, EventArgs e)
        {
            string info =
                "Complete: " + NL +
                "Every line represents one wire." + NL +
                "e.g. StartTile.StartPort->EndTile.EndPort" + NL +
                NL +
                "Compact:" + NL +
                "Every line represents one tile and all of its wires." + NL +
                "e.g. StartTile StartPort1->EndTile1.EndPort1 StartPort2->EndTile2.EndPort2 ...";

            MessageBox.Show(info, "Format Info");
        }

        private void btn_addWireInclude_Click(object sender, EventArgs e)
        {
            AddTableRow(true);
        }

        private void btn_addWireIgnore_Click(object sender, EventArgs e)
        {
            AddTableRow(false);
        }

        private void RemoveTableRow(object sender, EventArgs e)
        {
            TableLayoutPanel panel = (TableLayoutPanel)((Control)sender).Parent;

            HelperMethodsLibrary.RemoveRowInTableLayoutPanel(panel, panel.GetPositionFromControl((Control)sender).Row);
            UpdateLabelCounter(panel);
        }      
        
        private void UpdateLabelCounter(TableLayoutPanel panel)
        {
            bool table = panel.Equals(table_includes);

            Label counter = table ? lbl_wireIncludesExpand : lbl_wireIgnoresExpand;
            counter.Text = (table ? "Includes (" : "Ignores (") + (panel.RowCount - 1) + ")";
        }

        /// <summary>
        /// True for includes, false for ignores
        /// </summary>
        /// <param name="includes"></param>
        private void AddTableRow(bool table)
        {
            TableLayoutPanel panel = table ? table_includes : table_ignores;

            panel.SuspendLayout();

            int r = panel.RowCount;
            panel.RowCount++;
            panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

            for (int i = 0; i < 4; i++)
            {
                RichTextBox rtb = new RichTextBox
                {
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Multiline = false,
                    Text = ".*"
                };
                panel.Controls.Add(rtb);
            }

            Button deleteBtn = new Button
            {
                BackgroundImage = Properties.Resources.icons8_delete_64,
                BackgroundImageLayout = ImageLayout.Stretch,
                Cursor = Cursors.Hand,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                UseVisualStyleBackColor = true
            };
            deleteBtn.FlatAppearance.BorderSize = 0;
            deleteBtn.FlatAppearance.MouseOverBackColor = SystemColors.Control;
            deleteBtn.FlatAppearance.MouseDownBackColor = SystemColors.Control;
            panel.Controls.Add(deleteBtn);
            deleteBtn.Click += new EventHandler(RemoveTableRow);

            UpdateLabelCounter(panel);            

            panel.ResumeLayout();
        }

        private bool includesExpanded = false, ignoresExpanded = false;
        private void OnExpandRegexTableClicked(object sender, EventArgs e)
        {
            bool expanding = false;
            Button image = null;
            int row = -1;

            if (sender.Equals(flow_wireIncludesExpand))
            {
                expanding = !includesExpanded;
                includesExpanded = !includesExpanded;

                image = btn_wireIncludesExpand;
                row = 2;
            }
            else if (sender.Equals(flow_wireIgnoresExpand))
            {
                expanding = !ignoresExpanded;
                ignoresExpanded = !ignoresExpanded;

                image = btn_wireIgnoresExpand;
                row = 4;
            }
            else return;

            table_wiresTab.SuspendLayout();
            if (expanding)
            {
                image.BackgroundImage = Properties.Resources.icons8_expand_arrow_64;
                table_wiresTab.RowStyles[row].SizeType = SizeType.AutoSize;
            }
            else
            {
                image.BackgroundImage = Properties.Resources.icons8_collapse_arrow_64;
                table_wiresTab.RowStyles[row].SizeType = SizeType.Absolute;
                table_wiresTab.RowStyles[row].Height = 0;
            }
            table_wiresTab.ResumeLayout();
        }
    }
}
