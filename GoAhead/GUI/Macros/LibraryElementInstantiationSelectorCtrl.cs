using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Objects;

namespace GoAhead.GUI.Macros.LibraryElementInstantiation
{
    public partial class LibraryElementInstantiationSelectorCtrl : UserControl
    {
        public LibraryElementInstantiationSelectorCtrl()
        {
            InitializeComponent();

            BindingSource bsrc = new BindingSource();
            bsrc.DataSource = LibraryElementInstanceManager.Instance.Instances;
            m_grdViewInstances.DataSource = bsrc;

        }

        public event EventHandler SelectionChanged;

        private void m_txtManualFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Regex userFilter = null;
                try
                {
                    userFilter = new Regex(m_txtManualFilter.Text);
                }
                catch (ArgumentException exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // clear selection and reselect all rows that match the user filter
                m_grdViewInstances.ClearSelection();
                for (int index = 0; index < LibraryElementInstanceManager.Instance.Instances.Count; index++)
                {
                    LibElemInst instance = LibraryElementInstanceManager.Instance.Instances[index];
                    if (userFilter.IsMatch(instance.InstanceName))
                    {
                        m_grdViewInstances.Rows[index].Selected = true;
                    }
                }
            }
        }

        public string InstanceFilter
        {
            get { return !string.IsNullOrEmpty(m_txtManualFilter.Text) ? m_txtManualFilter.Text : m_txtAutoFilter.Text; }
        }

        public List<int> SelectedIndeces
        {
            get
            {
                List<int> rowIndeces = new List<int>();
                foreach (DataGridViewRow row in m_grdViewInstances.SelectedRows)
                {
                    rowIndeces.Add(row.Index);
                }
                return rowIndeces;
            }
        }

        public List<LibElemInst> SelectedInstances
        {
            get
            {
                List<int> rowIndeces = new List<int>();
                foreach (DataGridViewRow row in m_grdViewInstances.SelectedRows)
                {
                    rowIndeces.Add(row.Index);
                }

                List<LibElemInst> instances = new List<LibElemInst>();
                foreach (int index in rowIndeces.OrderBy(i => i))
                {
                    instances.Add(LibraryElementInstanceManager.Instance.GetInstantiation(index));
                }
                return instances;
            }
        }

        private void m_grdViewInstances_SelectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }


            string affectedInstances = "";
            foreach (LibElemInst inst in SelectedInstances.OrderBy(i => i.InstanceName))
            {
                affectedInstances += (affectedInstances.Length > 0 ? "|" : "") + @"(^" + inst.InstanceName + "$)";
            }
            //this.m_lblAffectedInstance.Text = "Affected Instances " + this.m_affectedInstances;
            // TODO kann ich Text setzen bei ReadOnly
            m_txtAutoFilter.ReadOnly = false;
            m_txtAutoFilter.Text = affectedInstances;
            m_txtAutoFilter.ReadOnly = true;
            
        }

        private void LibraryElementInstantiationSelectorCtrl_Resize(object sender, EventArgs e)
        {
            m_txtAutoFilter.Left = 120;
            m_txtManualFilter.Left = 120;

            int gap = 250;
            m_txtAutoFilter.Width = m_txtAutoFilter.Left + (Width - gap);
            m_txtManualFilter.Width = m_txtManualFilter.Left + (Width - gap);
        }
    }
}
