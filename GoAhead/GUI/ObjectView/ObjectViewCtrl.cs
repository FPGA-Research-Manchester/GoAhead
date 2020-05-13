using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI
{
    public partial class ObjectViewCtrl : UserControl
    {
        public ObjectViewCtrl()
        {
            InitializeComponent();
        }

        private void UpdateView()
        {
            if (m_object != null)
            {
                m_txtToString.AppendText(m_object.ToString());
            }
        }

        public object Object
        {
            set { m_object = value; UpdateView(); }
            get { return m_object; }
        }

        private object m_object = null;
    }
}
