using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.ObjectView
{
    public partial class ObjectViewForm : Form
    {
        public ObjectViewForm(object obj, string title)
        {
            InitializeComponent();

            m_objView.Object = obj;
            Text = title;

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void ObjectViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
