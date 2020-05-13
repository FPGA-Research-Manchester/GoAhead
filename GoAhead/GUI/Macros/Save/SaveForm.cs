using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.AddLibraryManager.Save
{
    public partial class SaveForm : Form
    {
        public enum SaveType { SaveAsDesign = 0, SaveAsBlocker = 1, SaveAsMacro = 2 }

        public SaveForm(SaveType type)
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);

            m_saveCtrl.CurrentSaveType = type;

            switch (type)
            {
                case SaveType.SaveAsDesign:
                    Text = "Save As Design";
                    break;
                case SaveType.SaveAsBlocker:
                    Text = "Save As Blocker";
                    break;
                case SaveType.SaveAsMacro:
                    Text = "Save As Macro";
                    break;
                default:
                    break;
            }
        }

        private void SaveForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }               
    }
}
