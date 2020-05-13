using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.Objects;
using GoAhead.Code.XDL;

namespace GoAhead.GUI.Macros.DesignBrowser
{
    public partial class DesignBrowserCtrl : UserControl
    {
        public DesignBrowserCtrl()
        {
            InitializeComponent();
        }

        private void DesignBrowserCtrl_Load(object sender, EventArgs e)
        {
            PrintResourceConsumptionPerModule printResCmd = new PrintResourceConsumptionPerModule();
            printResCmd.NetlistContainerName = NetlistContainerName;
            printResCmd.TreeView = m_treeView;
            printResCmd.Do();
        }
                
        public string NetlistContainerName { get; set; }

        private void m_treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(!Objects.NetlistContainerManager.Instance.Contains(NetlistContainerName))
            {
                return;
            }

            NetlistContainer nlc = Objects.NetlistContainerManager.Instance.Get(NetlistContainerName);
            
            int slices = 0;
            int dsp = 0;
            int bram = 0;

            // primitive
            if (e.Node.Tag != null)
            {
                m_txtInstanceCode.Clear();

                if (!(e.Node.Tag is TreeNodeTag))
                { 
                }

                TreeNodeTag tag = (TreeNodeTag)e.Node.Tag;
                XDLInstance inst = tag.Instance;

                if (tag.Instance == null)
                {
                    m_txtInstanceCode.AppendText("no primitive code available for hierarchy node");

                    foreach (TreeNode leave in e.Node.GetChildNodes())
                    {
                        TreeNodeTag leaveTag = (TreeNodeTag)leave.Tag;
                        XDLInstance leaveInstance = leaveTag.Instance;
                        if (leaveInstance != null)
                        {
                            if (IdentifierManager.Instance.IsMatch(leaveInstance.Location, IdentifierManager.RegexTypes.CLB)) { slices += 1; }
                            else if (IdentifierManager.Instance.IsMatch(leaveInstance.Location, IdentifierManager.RegexTypes.BRAM)) { dsp += 1; }
                            else if (IdentifierManager.Instance.IsMatch(leaveInstance.Location, IdentifierManager.RegexTypes.DSP)) { bram += 1; }
                        }
                    }

                }
                else
                {
                    m_txtInstanceCode.AppendText(inst.ToString());

                    if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.CLB)) { slices = 1; }
                    else if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.BRAM)) { dsp = 1; }
                    else if (IdentifierManager.Instance.IsMatch(inst.Location, IdentifierManager.RegexTypes.DSP)) { bram = 1; }
                }
            }
            m_lblSlices.Text = slices + " Slices";
            m_lblBRAM.Text = bram + " BRAMs";
            m_lblDSP.Text = dsp + " DSPs";
        }
    }
}
