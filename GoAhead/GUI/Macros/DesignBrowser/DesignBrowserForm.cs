﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.Macros.DesignBrowser
{
    public partial class DesignBrowserForm : Form
    {
        public DesignBrowserForm(string netlistContainerName)
        {
            InitializeComponent();

            m_designBrowserCtrl.NetlistContainerName = netlistContainerName;

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void DesignBrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
