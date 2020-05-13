using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;

namespace GoAhead.GUI.ArchitectureGraph
{
    public partial class ExportArchitecture : Form
    {
        public ExportArchitecture()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] intCodes = ExportArchitectureWires.InternalFormatCodes;

            string[] columns = Enumerable.Repeat(string.Empty, intCodes.Length).ToArray();
            string[] delims = Enumerable.Repeat(string.Empty, intCodes.Length).ToArray();

            if (!ParseRequiredFormat(ref columns, ref delims, txtbox_exportFormat.Text, intCodes))
                return;

            ExportArchitectureWires cmd = new ExportArchitectureWires
            {
                Scope = radioButton1.Checked,
                FileName = txtbox_path.Text,
                Append = appendCheckBox.Checked,
                FormattingMethod = radioButton4.Checked,
                ColumnWidth = int.Parse(columnWidthTextBox.Text),
                Columns = columns,
                Delims = delims,
                ExcludeNonSMWires = checkBox1.Checked,
                ExcludeStopoverWires = checkBox2.Checked
            };
            CommandExecuter.Instance.Execute(cmd);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] intCodes = ExportArchitectureSMs.InternalFormatCodes;

            string[] columns = Enumerable.Repeat(string.Empty, intCodes.Length).ToArray();
            string[] delims = Enumerable.Repeat(string.Empty, intCodes.Length).ToArray();

            if (!ParseRequiredFormat(ref columns, ref delims, textBox1.Text, intCodes))
                return;

            ExportArchitectureSMs cmd = new ExportArchitectureSMs
            {
                Scope = radioButton1.Checked,
                FileName = txtbox_path.Text,
                Append = appendCheckBox.Checked,
                FormattingMethod = radioButton4.Checked,
                ColumnWidth = int.Parse(columnWidthTextBox.Text),
                Columns = columns,
                Delims = delims,
                ExcludeStopoverArcs = checkBox3.Checked
            };
            CommandExecuter.Instance.Execute(cmd);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string[] intCodes = ExportArchitectureBELs.InternalFormatCodes;

            string[] columns = Enumerable.Repeat(string.Empty, intCodes.Length).ToArray();
            string[] delims = Enumerable.Repeat(string.Empty, intCodes.Length).ToArray();

            if (!ParseRequiredFormat(ref columns, ref delims, textBox2.Text, intCodes))
                return;

            ExportArchitectureBELs cmd = new ExportArchitectureBELs
            {
                Scope = radioButton1.Checked,
                FileName = txtbox_path.Text,
                Append = appendCheckBox.Checked,
                FormattingMethod = radioButton4.Checked,
                ColumnWidth = int.Parse(columnWidthTextBox.Text),
                Columns = columns,
                Delims = delims
            };
            CommandExecuter.Instance.Execute(cmd);
        }

        private bool ParseRequiredFormat(ref string[] columns, ref string[] delims, string reqFormat, string[] internalCodes)
        {
            string NL = Environment.NewLine;

            // Validity check 1
            bool bracket = false;
            int counter = 0;
            string nestingError = "Incorrect format specified." + NL + "Brackets '{', '}' may not be nested.";
            for (int i = 0; i < reqFormat.Length; i++)
            {
                if(reqFormat[i].Equals('{'))
                {
                    if(bracket)
                    {
                        MessageBox.Show(nestingError, "ERROR");
                        return false;
                    }
                    else bracket = true;
                }
                else if(reqFormat[i].Equals('}'))
                {
                    if(!bracket)
                    {
                        MessageBox.Show(nestingError, "ERROR");
                        return false;
                    }
                    else
                    {
                        bracket = false;
                        counter++;
                    }
                }
            }
            if(counter != internalCodes.Length)
            {
                MessageBox.Show("Incorrect amount of parameters specified: " + counter + NL +
                    "Number should be: " + internalCodes.Length, "ERROR");
                return false;
            }

            // Parsing
            bool charIsCol = false;
            int j = -1;
            for (int i = 0; i < reqFormat.Length; i++)
            {
                switch (reqFormat[i])
                {
                    case '{':
                        j++;
                        charIsCol = true;
                        break;
                    case '}':
                        charIsCol = false;
                        break;
                    default:
                        if (charIsCol) columns[j] += reqFormat[i];
                        else delims[j] += reqFormat[i];
                        break;
                }
            }

            // Validity check 2
            for (int i = 0; i < internalCodes.Length; i++)
            {
                if (!columns.Contains(internalCodes[i]))
                {
                    MessageBox.Show("The specified format does not include" + NL +
                        "the parameter {" + internalCodes[i] + "}", "ERROR");
                    return false;
                }
            }

            return true;
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

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            columnWidthLabel.Enabled = radioButton4.Checked;
            columnWidthTextBox.Enabled = radioButton4.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string NL = Environment.NewLine;
            string info =
                "Your line should contain the 4 parameters:" + NL +
                "{tile1}, {port1}, {tile2}, {port2}" + NL + NL +
                "You can arrange them in any order and add delimiters between them" + NL + NL +
                "Examples:" + NL +
                "{tile1} {port1} -> {tile2} {port2}" + NL +
                "{port1},{tile1}/{port2},{tile2}";

            MessageBox.Show(info, "INFO");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine;
            string info =
                "Choose one of the two ways to export the data:" + NL + NL +
                "Compact:" + NL +
                "Each line is written exactly in the format specified below." + NL + NL +
                "Formatted:" + NL +
                "Each line is further formatted for better readability." + NL +
                "Useful if the file is to be manually inspected." + NL +
                "Uses more memory.";

            MessageBox.Show(info, "INFO");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine;
            string info =
                "Export data for tiles in:" + NL + NL +
                "Selection:" + NL +
                "The tiles currently selected." + NL + NL +
                "All Tiles:" + NL +
                "All the tiles on the FPGA." + NL +
                "Note that this might take some time.";

            MessageBox.Show(info, "INFO");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine;
            string info =
                "Your line should contain the 3 parameters:" + NL +
                "{tile}, {port1}, {port2}" + NL + NL +
                "You can arrange them in any order and add delimiters between them" + NL + NL +
                "Examples:" + NL +
                "{tile} : {port1} -> {port2}" + NL +
                "{port1}.{port2} {tile}";

            MessageBox.Show(info, "INFO");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string NL = Environment.NewLine;
            string info =
                "Your line should contain the 6 parameters:" + NL +
                "{tile}, {slice}, {slicetype}, {bel}, {port}, {porttype}" + NL + NL +
                "You can arrange them in any order and add delimiters between them" + NL + NL +
                "Examples:" + NL +
                "{tile} {slice} {slicetype} {bel} {port} {porttype}" + NL +
                "{bel}.{port}.{porttype} / {tile}.{slice}.{slicetype}";

            MessageBox.Show(info, "INFO");
        }
    }
}
