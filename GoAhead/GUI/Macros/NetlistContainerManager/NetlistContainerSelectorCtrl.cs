using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Objects;

namespace GoAhead.GUI.Macros.NetlistContainerManager
{
    public partial class NetlistContainerSelectorCtrl : UserControl
    {
        public NetlistContainerSelectorCtrl()
        {
            InitializeComponent();

            BindingSource bsrc = new BindingSource();
            bsrc.DataSource = Objects.NetlistContainerManager.Instance.NetlistContainerBindingList;

            m_cmbBoxNetlistContainerNames.DisplayMember = "Name";
            m_cmbBoxNetlistContainerNames.ValueMember = "Name";
            m_cmbBoxNetlistContainerNames.DataSource = bsrc;

            m_cmbBoxNetlistContainerNames.SelectedItem = Objects.NetlistContainerManager.Instance.NetlistContainerBindingList.Count == 0 ? null : Objects.NetlistContainerManager.Instance.NetlistContainerBindingList[0];
        }

        public string SelectedNetlistContainerName
        {
            get
            {
                return m_cmbBoxNetlistContainerNames.SelectedItem == null ? "" : ((NetlistContainer)m_cmbBoxNetlistContainerNames.SelectedItem).Name;
            }
        }

        /// <summary>
        /// Gets or sets the label text
        /// </summary>
        public string Label
        {
            get { return m_lblNetlistContainer.Text; }
            set { m_lblNetlistContainer.Text = value;  }
        }
    }
}
