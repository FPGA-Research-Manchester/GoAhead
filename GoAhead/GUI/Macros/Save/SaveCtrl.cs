using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.Data;
using GoAhead.Objects;
using GoAhead.GUI.AddLibraryManager.Save;

namespace GoAhead.GUI.AddLibraryManager.Save
{
    public partial class SaveCtrl : UserControl
    {      
        public SaveCtrl()
        {
            InitializeComponent();

            m_fileSelCtrl.RestorePreviousSelection();
            m_fileSelCtrl.DisableAppendCheckBox();

            foreach (Objects.NetlistContainer next in NetlistContainerManager.Instance.NetlistContainer)
            {
                m_chkListBoxMacros.Items.Add(next.Name, false);
            }

            // pre select if only one
            if (m_chkListBoxMacros.Items.Count == 1)
            {
                m_chkListBoxMacros.SetItemChecked(0, true);
            }
        }

        private void m_btnOk_Click(object sender, EventArgs e)
        {
            string fileName = m_fileSelCtrl.FileName;

            List<string> netlistContainerNames = new List<string>(); 
            foreach(string name in m_chkListBoxMacros.CheckedItems)
            {
                netlistContainerNames.Add(name);
            }

            if(netlistContainerNames.Count == 0)
            {
                MessageBox.Show("No netlist container selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }            
            
            switch (CurrentSaveType)
	        {
		        case SaveForm.SaveType.SaveAsDesign:
                    {
                        SaveAsDesign cmd = new SaveAsDesign();
                        cmd.FileName = fileName;
                        cmd.NetlistContainerNames = netlistContainerNames;
                        Commands.CommandExecuter.Instance.Execute(cmd);
                        break;
                    }
                case SaveForm.SaveType.SaveAsBlocker:
                    {
                        SaveAsBlocker cmd = new SaveAsBlocker();
                        cmd.FileName = fileName;
                        cmd.NetlistContainerNames = netlistContainerNames;
                        Commands.CommandExecuter.Instance.Execute(cmd);
                        break;
                    }                    
                case SaveForm.SaveType.SaveAsMacro:
                    {
                        SaveAsMacro cmd = new SaveAsMacro();
                        cmd.FileName = fileName;
                        cmd.NetlistContainerNames = netlistContainerNames;
                        Commands.CommandExecuter.Instance.Execute(cmd);
                        break;
                    }
                default:
                    throw new ArgumentException("Unexpected save type: " + CurrentSaveType);
	        }


            if (ParentForm != null)
            {
                ParentForm.Close();
            }
        }

        public SaveForm.SaveType CurrentSaveType { get; set; }
    }
}
