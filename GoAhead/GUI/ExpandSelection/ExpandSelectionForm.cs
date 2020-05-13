using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.ExpandSelection
{
    public partial class ExpandSelectionForm : Form
    {
        public ExpandSelectionForm(GUI gui)
        {
            InitializeComponent();

            m_expandSelectionCtrl.Gui = gui;

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        public ExpandSelectionForm()
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void ExpandSelectionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }

    }
}
