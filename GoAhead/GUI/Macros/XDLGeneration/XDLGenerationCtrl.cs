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

namespace GoAhead.GUI.Macros.XDLGeneration
{
    public partial class XDLGenerationCtrl : UserControl
    {
        public XDLGenerationCtrl()
        {
            InitializeComponent();

            BindingSource bsrc = new BindingSource();
            bsrc.DataSource = Objects.NetlistContainerManager.Instance.NetlistContainerBindingList;

            m_checkedListBoxNetlistContainer.DisplayMember = "Name";
            m_checkedListBoxNetlistContainer.ValueMember = "Name";
            m_checkedListBoxNetlistContainer.DataSource = bsrc;

            m_fileSel.Label = "Output XDL File";
            m_fileSel.Append = false;
            m_fileSel.RestorePreviousSelection();            
        }

        private void m_btnGenerate_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            foreach (object o in m_checkedListBoxNetlistContainer.SelectedItems)
            {
                NetlistContainer nlc = (NetlistContainer)o;
                names.Add(nlc.Name);
            }

            GenerateXDL genCmd = new GenerateXDL();
            genCmd.DesignName = m_txtDesignName.Text;
            genCmd.FileName = m_fileSel.FileName;
            genCmd.IncludeDesignStatement = m_chkIncludeDesignStatement.Checked;
            genCmd.IncludeDummyNets = m_chkIncludeDummyNets.Checked;
            genCmd.IncludeModuleFooter = m_chkIncludeFooter.Checked;
            genCmd.IncludeModuleHeader = m_chkIncludeHeader.Checked;
            genCmd.IncludePorts = m_chkIncludePorts.Checked;
            genCmd.NetlistContainerNames = names;
            genCmd.SortInstancesBySliceName = m_chkSort.Checked;

            Commands.CommandExecuter.Instance.Execute(genCmd);

            if (ParentForm != null)
            {
                ParentForm.Close();
            }
        }
    }
}
