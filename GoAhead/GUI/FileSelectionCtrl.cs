using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Settings;

namespace GoAhead.GUI
{
    public partial class FileSelectionCtrl : UserControl
    {
        public FileSelectionCtrl()
        {
            InitializeComponent();
            m_lblFile.Text = Label;

        }

        public void RestorePreviousSelection()
        {
            if (StoredPreferences.Instance.TextBoxSettings.HasSetting(CallerString))
            {
                m_txtFile.Text = StoredPreferences.Instance.TextBoxSettings.GetSetting(CallerString);
            }
            m_lblFile.Text = Label;
        }

        private string CallerString
        {
            get { return "FileSelectionCtrl" + Label; }
        }

        string _label = "Label";
        [
            Category("Appearance"),
            Description("The String to display on the label")
        ]
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        string _filter = "All files|*.*";
        [
            Category("Appearance"),
            Description("The file filter in the browse dialog")
        ]
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

        /// <summary>
        /// The value for FileName for CommandWithFileOutput
        /// </summary>
        public string FileName
        {
            get { return m_txtFile.Text; }
            set { m_txtFile.Text = value; }
        }

        /// <summary>
        /// The value for Append for CommandWithFileOutput
        /// </summary>
        public bool Append
        {
            get { return m_chkBoxAppend.Checked; }
            set { m_chkBoxAppend.Checked = value; }
        }

        public void DisableAppendCheckBox()
        {
            m_chkBoxAppend.Visible = false;
        }

        private void m_btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a File";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = false;
            openFileDialog.Filter = Filter;

            if (StoredPreferences.Instance.FileDialogSettings.HasSetting(CallerString))
            {
                openFileDialog.InitialDirectory = StoredPreferences.Instance.FileDialogSettings.GetSetting(CallerString);
            }

            // cancel
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrEmpty(openFileDialog.FileName))
                return;

            // store last user path
            StoredPreferences.Instance.FileDialogSettings.AddOrUpdateSetting(CallerString, System.IO.Path.GetDirectoryName(openFileDialog.FileName));

            m_txtFile.Text = openFileDialog.FileName;

            StoredPreferences.Instance.TextBoxSettings.AddOrUpdateSetting(CallerString, m_txtFile.Text);
           
        }

        private void m_txtFile_TextChanged(object sender, EventArgs e)
        {
            StoredPreferences.Instance.TextBoxSettings.AddOrUpdateSetting(CallerString, m_txtFile.Text);
        }
    }
}
