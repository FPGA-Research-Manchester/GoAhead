using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;

namespace GoAhead.GUI.BUFGInfo
{
    public partial class BUFGInfoCtrl : UserControl
    {
        public BUFGInfoCtrl()
        {
            InitializeComponent();

            FillTable();
        }

        private void FillTable()
        {
            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles())
            {
                foreach (Slice s in t.Slices.Where(s => s.SliceName.StartsWith("BUFG")))
                {
                    int n = m_dataGrd.Rows.Add();
                    m_dataGrd.Rows[n].Cells[0].Value = t.Location;
                    m_dataGrd.Rows[n].Cells[1].Value = s.SliceName;
                }
            }
        }
    }
}
