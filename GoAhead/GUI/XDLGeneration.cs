using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoAhead.Macro;
using GoAhead.Commands;

namespace GoAhead.GUI
{
    public partial class XDLGeneration : Form
    {
        public XDLGeneration()
        {
            InitializeComponent();

            // add all macros and check the current 
            foreach (Macro.Macro next in MacroManager.Instance.GetAllMacros())
            {
                this.m_chkListMacros.Items.Add(next.MacroName, next.MacroName.Equals(MacroManager.Instance.CurrentMacro.MacroName));
            }

            this.m_txtFileName.Text = XDLGeneration.m_fileName;
        }

        private void m_btnGenerate_Click(object sender, EventArgs e)
        {
            List<String> selectedMacros = new List<string>();
            foreach (Object obj in this.m_chkListMacros.CheckedItems)
            {
                selectedMacros.Add(obj.ToString());
            }

            if (!System.IO.File.Exists(this.m_txtFileName.Text))
            {
                MessageBox.Show("File " + this.m_txtFileName.Text + " does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CommandExecuter.Instance.Execute(new GenerateXDL(this.m_txtFileName.Text, selectedMacros));

            this.Close();
        }

        private void m_btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an XDL File";
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Xilinx Design Language|*.xdl";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (openFileDialog.FileName.Equals(""))
                return;

            XDLGeneration.m_fileName = openFileDialog.FileName;
            this.m_txtFileName.Text = XDLGeneration.m_fileName;
        }

        private static String m_fileName = "";
    }
}
