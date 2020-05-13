using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.BlockingShared;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.GUI.Blocker
{
    public partial class BlockerCtrl : UserControl
    {
        public BlockerCtrl()
        {
            InitializeComponent();
        }

        private void m_btnBlock_Click(object sender, EventArgs e)
        {
            BlockSelection blockCmd = new BlockSelection
            {
                BlockWithEndPips = m_chkUseEndPips.Checked,
                NetlistContainerName = m_netlistContainerSelector.SelectedNetlistContainerName,
                Prefix = m_txtPrefix.Text,
                PrintUnblockedPorts = m_chkPrintUnblockedPorts.Checked,
                SliceNumber = (int)m_numDrpSliceNumber.Value,
                PrintProgress = true,
                BlockOnlyMarkedPorts = m_chkBlockMarkedPortsOnly.Checked
            };
            Commands.CommandExecuter.Instance.Execute(blockCmd);

            if (CloseMeAfterBlocking != null)
            {
                CloseMeAfterBlocking.Close();
            }
        }

        public Form CloseMeAfterBlocking = null;
        
    }
}
