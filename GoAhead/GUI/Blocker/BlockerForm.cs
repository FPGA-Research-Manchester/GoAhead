using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.Blocker
{
    public partial class BlockerForm : Form
    {
        public BlockerForm()
        {
            InitializeComponent();

            m_blockerCtrl.CloseMeAfterBlocking = this;

            // get window position
            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void BlockerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // store window position
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
