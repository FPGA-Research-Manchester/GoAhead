using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Objects;

namespace GoAhead.GUI.Macros.LibraryManager
{
    public partial class LibraryElementSelectorCtrl : UserControl
    {
        public LibraryElementSelectorCtrl()
        {
            InitializeComponent();

            BindingSource bsrc = new BindingSource();
            bsrc.DataSource = Library.Instance.LibraryElements;
            m_cmbBoxLibraryElementNames.DisplayMember = "Name";
            m_cmbBoxLibraryElementNames.ValueMember = "Name";
            m_cmbBoxLibraryElementNames.DataSource = bsrc;

            m_cmbBoxLibraryElementNames.SelectedItem = Library.Instance.LibraryElements.Count == 0 ? null : Library.Instance.LibraryElements[0].Name;
        }

        public string SelectedLibraryElementName
        {
            get
            {
                return m_cmbBoxLibraryElementNames.SelectedItem == null ? "" : ((LibraryElement)m_cmbBoxLibraryElementNames.SelectedItem).Name;
            }
        }

        /// <summary>
        /// Gets or sets the label text
        /// </summary>
        public string Label
        {
            get { return m_lblElements.Text; }
            set { m_lblElements.Text = value;  }
        }
    }
}
