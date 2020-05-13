using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.TCL.Procs.cshelp
{
    public partial class CsHelpForm : Form
    {
        public CsHelpForm()
        {
            InitializeComponent();
        }

        private void RefreshData(object sender, EventArgs e)
        {
            var c = formPanel.Controls.Find("cshelppanel", false);
            if (c.Length == 1 && c[0] is CsHelpLayoutPanel cshelp)
            {
                cshelp.PopulateGrids(checkBox1.Checked, scopeLabel.Text);
            }
        }

        public void SetScopeLabel(string str)
        {
            scopeLabel.Text = str;
        }

        public void DeactivateObjectElements()
        {
            button1.Visible = false;
            checkBox1.Visible = false;
        }

        public void ParameterlessMode()
        {
            DeactivateObjectElements();
            SetScopeLabel("");
            label1.Text = "cshelp";
        }
    }
}
