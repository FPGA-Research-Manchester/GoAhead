using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.BUFGInfo
{
    public partial class BUFGInfoForm : Form
    {
        public BUFGInfoForm()
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void BUFGInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
