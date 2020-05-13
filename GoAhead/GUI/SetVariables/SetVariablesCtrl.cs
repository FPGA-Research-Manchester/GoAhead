using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.Variables;
using GoAhead.FPGA;
using GoAhead.Commands.Selection;
using GoAhead.Objects;

namespace GoAhead.GUI.SetVariables
{
    public partial class SetVariablesCtrl : UserControl, Interfaces.IObserver
    {
        public SetVariablesCtrl()
        {
            InitializeComponent();
            TileSelectionManager.Instance.Add(this);
        }

        public void Notify(object obj)
        {
            if (TileSelectionManager.Instance.NumberOfSelectedTiles == 2)
            {
                Tile clb = TileSelectionManager.Instance.GetFirstSelectedTile(FPGATypes.Placement.UpperLeft, IdentifierManager.RegexTypes.CLB);
                Tile interconnect = TileSelectionManager.Instance.GetFirstSelectedTile(FPGATypes.Placement.UpperLeft, IdentifierManager.RegexTypes.Interconnect);

                m_txtTileIdentifier.Text = clb.Location + " " + interconnect.Location;
                m_txtTileIdentifier.Width = 400;
            }
        }

        public List<Input> Inputs
        {
            get { return m_inputs; }
            set { m_inputs = value; AddControls(); }
        }

        private void AddControls()
        {
            if(m_inputs == null)
            {
                return;
            }
            // shrink to group box
            Parent.Width = Math.Max(m_groupBoxCtrl.Width, Parent.Width);
            Parent.Height = m_groupBoxCtrl.Height;
            // grow by controls
            int y = m_groupBoxCtrl.Height;
            foreach (Input input in m_inputs)
            {
                SetVarCtrl setCtrl = new SetVarCtrl();
                setCtrl.Input = input;
                setCtrl.Location = new Point(0, y);
                Parent.Width = Math.Max(setCtrl.Width, Parent.Width);
                //this.Parent.Width = Math.Max(setCtrl.Width, this.Parent.Width);
                Parent.Height = Parent.Height + setCtrl.Height;
                m_groupBoxCtrl.Width = Parent.Width;

                y += setCtrl.Height;

                TileSelectionManager.Instance.Add(setCtrl);
                m_setVarControls.Add(setCtrl);
                Controls.Add(setCtrl);
            }
        }

        public List<Set> GetSetCommands()
        {
            List<Set> result = new List<Set>();            
            m_setVarControls.ForEach(ctrl => result.AddRange(ctrl.SetCommands));
            return result;
        }        

        private void m_btnOk_Click(object sender, EventArgs e)
        {
            ParentForm.Close();
        }

        private List<SetVarCtrl> m_setVarControls = new List<SetVarCtrl>();
        private List<Input> m_inputs = null;

       
    }
}
