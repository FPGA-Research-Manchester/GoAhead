using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Objects;
using GoAhead.Commands.Library;
using GoAhead.Commands.VHDL;
using GoAhead.Settings;
using GoAhead.FPGA;

namespace GoAhead.GUI.AddLibraryManager
{
    public partial class AddXDLLibraryCtrl : UserControl, Interfaces.IObserver
    {
        public AddXDLLibraryCtrl()
        {
            InitializeComponent();

            TileSelectionManager.Instance.Add(this);
        }

        private void m_btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an XDL File";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "All XDL Macro File|*.xdl";
            openFileDialog.Multiselect = false;

            string caller = "m_btnAddXDL_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //cancel
            if (openFileDialog.FileNames.Length == 0)
            {
                return;
            }

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(openFileDialog.FileName));

            m_txtXDLMacro.Text = openFileDialog.FileName;
        }

        private void m_btnAdd_Click(object sender, EventArgs e)
        {
            string fileName = m_txtXDLMacro.Text;

            if (!System.IO.File.Exists(fileName))
            {
                MessageBox.Show("File " + fileName + " not found", "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            AddXDLLibraryElement addCmd = new AddXDLLibraryElement();
            addCmd.FileName = fileName;
            Commands.CommandExecuter.Instance.Execute(addCmd);

            ParentForm.Close();
        }

        public void Notify(object obj)
        {
            if (TileSelectionManager.Instance.NumberOfSelectedTiles != 2)
            {
                // "Select exactly one tile (one pair of CLB and Interconnect Tile)", "Error");
                return;
            }

            Tile clb = null;
            foreach (Tile t in TileSelectionManager.Instance.GetSelectedTiles())
            {
                if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                {
                    clb = t;
                    break;
                }
            }

            if (clb == null)
            {
                // no clb selected
                return;
            }
        }

        private void ParentFormClosed(object sender, FormClosedEventArgs e)
        {
            TileSelectionManager.Instance.Remove(this);
        }


        private void AddMacroCtrl_Load(object sender, EventArgs e)
        {
            ParentForm.FormClosed += new FormClosedEventHandler(ParentFormClosed);
        }
    }
}
