using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Objects;
using GoAhead.Interfaces;

namespace GoAhead.GUI.Watch
{
    public partial class VariableWatchCtrl : UserControl, IObserver
    {
        public VariableWatchCtrl()
        {
            InitializeComponent();
            VariableManager.Instance.Add(this);
        }

        private void UpdateValues()
        {
            m_dataGrdArguments.Columns.Clear();
            m_dataGrdArguments.Columns.Add("Name", "Name");
            m_dataGrdArguments.Columns.Add("Value", "Value");

            m_dataGrdArguments.Columns[0].ReadOnly = true;
            // the user will edit this column
            m_dataGrdArguments.Columns[1].ReadOnly = false;

            foreach (string varName in VariableManager.Instance.GetAllVariableNames())
            {
                string varValue = VariableManager.Instance.GetValue(varName);

                m_dataGrdArguments.Rows.Add(varName, varValue);
            }            
        }

        public void Notify(object obj)
        {
            UpdateValues();
        }

    }
}
