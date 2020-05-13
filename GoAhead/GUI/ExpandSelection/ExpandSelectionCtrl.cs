using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands;
using GoAhead.Commands.Selection;

namespace GoAhead.GUI.ExpandSelection
{
    public partial class ExpandSelectionCtrl : UserControl
    {
        public ExpandSelectionCtrl()
        {
            InitializeComponent();

            BindingSource selBsrc = new BindingSource();
            selBsrc.DataSource = TileSelectionManager.Instance.UserSelectionTypes;
            m_cmbBoxUserSel.DataSource = selBsrc;

        }

        private void m_btnOK_Click(object sender, EventArgs e)
        {
            if (m_cmbBoxUserSel.SelectedItem == null)
            {
                MessageBox.Show("No user selection selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string userSel = m_cmbBoxUserSel.SelectedItem.ToString();
            ExpandSelectionInRow expandCmd = null;
            if (m_rbBtnLeft.Checked)
            {
                expandCmd = new ExpandSelectionLeft();
            }
            else if (m_rbBtnRight.Checked)
            {
                expandCmd = new ExpandSelectionRight();
            }
            else if (m_rbBtnLeftRight.Checked)
            {
                expandCmd = new ExpandSelectionLeftAndRight();
            }
            expandCmd.UserSelectionType = userSel;
            CommandExecuter.Instance.Execute(expandCmd);

            if (Gui != null && TileSelectionManager.Instance.NumberOfSelectedTiles > 0)
            {
                Tile t = TileSelectionManager.Instance.GetSelectedTile(".*", FPGATypes.Placement.UpperLeft);
                Gui.FPGAView.UpdateStatusStrip(t.TileKey);
                Gui.FPGAView.Invalidate();
            }

            ParentForm.Close();
        }

        public GUI Gui { get; set; }
    }
}
