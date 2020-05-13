using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Objects;
using GoAhead.Commands.Library;
using GoAhead.Commands.VHDL;
using GoAhead.Settings;
using GoAhead.GUI.AddLibraryManager;

namespace GoAhead.GUI.MacroLibrary
{
    public partial class LilbraryManagerCtrl : UserControl
    {
        public LilbraryManagerCtrl()
        {
            InitializeComponent();

            Bind();
        }

        private void Bind()
        {
            BindingSource bsrc = new BindingSource();
            bsrc.DataSource = Library.Instance.LibraryElements;
            m_listBoxLibraryElementNames.DisplayMember = "Name";
            m_listBoxLibraryElementNames.ValueMember = "Name";
            m_listBoxLibraryElementNames.DataSource = bsrc;
        }

        public string GetSelectedLibraryElementName()
        {
            if (m_listBoxLibraryElementNames.SelectedItem == null)
            {
                return null;
            }
            else
            {
                return m_listBoxLibraryElementNames.SelectedValue.ToString();
            }
        }

        #region Buttons
        private void m_btnSaveLib_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Select File";
            saveFileDialog.Filter = "All library Files|*.binLibrary";

            string caller = "m_btnSaveLib_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                saveFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            //cancel
            if (string.IsNullOrEmpty(saveFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(saveFileDialog.FileName));

            // save
            SaveLibrary saveCmd = new SaveLibrary();
            saveCmd.FileName = saveFileDialog.FileName;
            Commands.CommandExecuter.Instance.Execute(saveCmd);            
        }

        private void m_btnOpenLib_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "All library Files|*.binLibrary";

            string caller = "m_btnOpenLib_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            //cancel
            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(openFileDialog.FileName));

            // load
            LoadLibrary loadCmd = new LoadLibrary();
            loadCmd.FileName = openFileDialog.FileName;
            Commands.CommandExecuter.Instance.Execute(loadCmd);

            Bind();
        }

        private void m_btnClear_Click(object sender, EventArgs e)
        {
            ClearLibrary clearCmd = new ClearLibrary();
            Commands.CommandExecuter.Instance.Execute(clearCmd);
        }

        private void m_btnAddXDL_Click(object sender, EventArgs e)
        {
            AddLibraryManager.AddMacro.AddXDLLibraryElementForm addMacroForm = new AddLibraryManager.AddMacro.AddXDLLibraryElementForm();
            addMacroForm.Show();
        }
        
        private void m_btnRemove_Click(object sender, EventArgs e)
        {
            string libraryElement = GetSelectedLibraryElementName();
            if (libraryElement == null)
            {
                MessageBox.Show("No  library element selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RemoveLibraryElement remCmd = new RemoveLibraryElement();
            remCmd.LibraryElementName = libraryElement;

            Commands.CommandExecuter.Instance.Execute(remCmd);
        }

        private void m_btnSaveLibraryElement_Click(object sender, EventArgs e)
        {
            string libraryElement = GetSelectedLibraryElementName();
            if (libraryElement == null)
            {
                MessageBox.Show("No library element selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Select File";
            saveFileDialog.FileName = libraryElement + ".binNetlist";
            saveFileDialog.Filter = "All binary library Files|*.binNetlist";

            string caller = "m_btnSaveLibraryElement_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                saveFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            //cancel
            if (string.IsNullOrEmpty(saveFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(saveFileDialog.FileName));
            
            SaveLibraryElement saveCmd = new SaveLibraryElement();
            saveCmd.FileName = saveFileDialog.FileName;
            saveCmd.LibraryElementName = libraryElement;


            Commands.CommandExecuter.Instance.Execute(saveCmd);
        }
        
        private void m_btnAddBinaryMacro_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "All binary netlists|*.*";
            openFileDialog.Multiselect = true;

            string caller = "m_btnAddBinaryMacro_Click";
            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(caller))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(caller);
            }

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            //cancel
            if (openFileDialog.FileNames.Length == 0)
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(caller, System.IO.Path.GetDirectoryName(openFileDialog.FileName));
            
            foreach (string fileName in openFileDialog.FileNames)
            {
                AddBinaryLibraryElement addCmd = new AddBinaryLibraryElement();
                addCmd.FileName = openFileDialog.FileName;

                Commands.CommandExecuter.Instance.Execute(addCmd);
            }
        }

        #endregion

        private void m_btnPrintWrapper_Click(object sender, EventArgs e)
        {
            if (GetSelectedLibraryElementName() == null)
            {
                MessageBox.Show("No library element selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintComponentDeclaration printCmd = new PrintComponentDeclaration();
            printCmd.LibraryElement = GetSelectedLibraryElementName();

            Commands.CommandExecuter.Instance.Execute(printCmd);
        }

        private void m_listBoxMacroNames_DoubleClick(object sender, EventArgs e)
        {
            string libraryElement = GetSelectedLibraryElementName();

            if (libraryElement == null)
            {
                return;
            }

            ObjectView.ObjectViewForm frm = new ObjectView.ObjectViewForm(Library.Instance.GetElement(libraryElement), libraryElement + " statistics");
            frm.Show();
        }

        private void m_listBoxMacroNames_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ListBox)
            {
                ListBox listBox = (ListBox)sender;
                Point point = new Point(e.X, e.Y);
                int hoverIndex = listBox.IndexFromPoint(point);
                if (hoverIndex >= 0 && hoverIndex < listBox.Items.Count)
                {
                    m_toolTip.SetToolTip(listBox, listBox.Items[hoverIndex].ToString());
                }
            }    
        }

        private ToolTip m_toolTip = new ToolTip();
    }
}