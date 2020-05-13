using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.PartGen
{
    public partial class PartgenAllForm : Form
    {
        public PartgenAllForm()
        {
            InitializeComponent();

            //Settings.Preferences.Instance.GUISettings.Open(this);
        }

        private void PartgenAllForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);

        }
    }
}
