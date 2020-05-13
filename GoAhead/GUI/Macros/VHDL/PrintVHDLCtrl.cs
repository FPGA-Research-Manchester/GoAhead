using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.VHDL;
using GoAhead.Commands.UCF;

namespace GoAhead.GUI.Macros.VHDL
{
    public partial class PrintVHDLCtrl : UserControl
    {
        public PrintVHDLCtrl()
        {
            InitializeComponent();

            m_fileSelVHDLWrapper.RestorePreviousSelection();
            m_fileSelVHDLWrapperInstantiation.RestorePreviousSelection();
            m_fileSelUCF.RestorePreviousSelection();
        }

        private void m_btnPrintWrapper_Click(object sender, EventArgs e)
        {
            PrintVHDLWrapper printCmd = new PrintVHDLWrapper();
            printCmd.Append = m_fileSelVHDLWrapper.Append;
            printCmd.CreateBackupFile = true;
            printCmd.EntityName = m_txtEntityName.Text;
            printCmd.FileName = m_fileSelVHDLWrapper.FileName;
            printCmd.InstantiationFilter = m_libElInstSelector.InstanceFilter;

            Commands.CommandExecuter.Instance.Execute(printCmd);
        }

        private void m_btnPrintWrapperInstantiation_Click(object sender, EventArgs e)
        {
            PrintVHDLWrapperInstantiation printCmd = new PrintVHDLWrapperInstantiation();
            printCmd.Append = m_fileSelVHDLWrapperInstantiation.Append;
            printCmd.CreateBackupFile = true;
            printCmd.EntityName = m_txtEntityName.Text;
            printCmd.FileName = m_fileSelVHDLWrapperInstantiation.FileName;
            printCmd.InstantiationFilter = m_libElInstSelector.InstanceFilter;

            Commands.CommandExecuter.Instance.Execute(printCmd);
        }

        private void m_btnPrintUCF_Click(object sender, EventArgs e)
        {
            PrintLocationConstraints printCmd = new PrintLocationConstraints();
            printCmd.InstantiationFilter = m_libElInstSelector.InstanceFilter;
            printCmd.HierarchyPrefix = m_txtHierarchyPrefix.Text;
            printCmd.FileName = m_fileSelUCF.FileName;
            printCmd.Append = m_fileSelUCF.Append;
            printCmd.CreateBackupFile = true;

            Commands.CommandExecuter.Instance.Execute(printCmd);
        }
    }
}
