using System;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands.LibraryElementInstantiation;

namespace GoAhead.GUI.MacroForm
{
    public partial class MacroManagerForm : Form
    {
        public MacroManagerForm()
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void MacroView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
