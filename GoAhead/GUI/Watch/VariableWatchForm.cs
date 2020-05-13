using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.Watch
{
    public partial class VariableWatchForm : Form
    {
        public VariableWatchForm()
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void VariableWatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
