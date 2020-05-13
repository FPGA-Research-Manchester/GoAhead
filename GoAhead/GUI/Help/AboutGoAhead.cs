using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;

namespace GoAhead.GUI
{
    public partial class AboutGoAhead : Form
    {
        public AboutGoAhead()
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void AboutGoAhead_Load(object sender, EventArgs e)
        {
            // write out the whole version number
            PrintVersion cmd = new PrintVersion();
            cmd.Do();

            m_lblVersion.Text = cmd.OutputManager.GetOutput();
        }

        private void m_btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void m_lblLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m_lblLink.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start(m_lblLink.Text);
        }

        private void AboutGoAhead_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }


    }
}
