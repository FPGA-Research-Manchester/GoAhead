using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Commands.UCF;
using GoAhead.Commands.VHDL;
using GoAhead.Code.XDL;
using GoAhead.Commands.LibraryElementInstantiationManager;
using GoAhead.Objects;

namespace GoAhead.GUI.LibraryElementInstantiation
{
    public partial class LibraryElementInstantiationManagerCtrl : UserControl
    {
        public LibraryElementInstantiationManagerCtrl()
        {
            InitializeComponent();

            m_libElInstSelector.SelectionChanged += m_libElInstSelector_SelectionChanged;
        }
        
        private void m_libElInstSelector_SelectionChanged(object sender, EventArgs e)
        {
            List<LibElemInst> instances = m_libElInstSelector.SelectedInstances;

            int count = (from i in instances select i.LibraryElementName).Distinct().Count();
            if (count > 1)
            {
                return;
                //MessageBox.Show("Can only handle one library element kind per selection.", "Multiple library elements used", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // collect all ports
            List<string> ports = new List<string>();
            instances.ForEach(i => ports.AddRange(from p in i.GetLibraryElement().Containter.Ports select Regex.Replace(p.ExternalName, @"\d", "")));

            List<string> prefices = new List<string>();
            prefices.AddRange(ports.Distinct());

            m_grdViewMapping.Rows.Clear();
            foreach (string p in prefices)
            {
                string[] row = new string[] { p, "vhdl_signal", "external" };
                m_grdViewMapping.Rows.Add(row);
            }
        }

        private void m_btnAnnotateSignals_Click(object sender, EventArgs e)
        {
            ClearSignalAnnotations clearCmd = new ClearSignalAnnotations();
            clearCmd.InstantiationFilter = m_libElInstSelector.InstanceFilter;
            Commands.CommandExecuter.Instance.Execute(clearCmd);

            List<string> portMapping = new List<string>();
            foreach (DataGridViewRow row in m_grdViewMapping.Rows)
            {
                string port = row.Cells[0].FormattedValue.ToString();
                string signal = row.Cells[1].FormattedValue.ToString();
                string mapping = row.Cells[2].FormattedValue.ToString();
                if (port.Length > 0 && signal.Length > 0 && mapping.Length > 0)
                {
                    portMapping.Add(port + ":" + signal + ":" + mapping);
                }
            }
            
            AnnotateSignalNames annoteCmd = new AnnotateSignalNames();
            annoteCmd.InstantiationFilter = m_libElInstSelector.InstanceFilter;
            annoteCmd.PortMapping = portMapping;

            Commands.CommandExecuter.Instance.Execute(annoteCmd);
        }

        private void LibraryElementInstantiationManagerCtrl_Resize(object sender, EventArgs e)
        {
            m_grpAnoteSignals.Width = Width - 10;
            m_btnAnnotateSignals.Left = (m_grpAnoteSignals.Width - m_btnAnnotateSignals.Width) - 10;
        }
    }
}
