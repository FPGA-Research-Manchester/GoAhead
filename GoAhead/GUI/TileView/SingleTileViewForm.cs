using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.GUI.TileView
{
    public partial class SingleTileViewForm : Form
    {
        public SingleTileViewForm(Tile tile)
        {
            InitializeComponent();

            m_tile = tile;
            Settings.StoredPreferences.Instance.GUISettings.Open(this);

            List<Tile> tiles = new List<Tile>();
            tiles.Add(m_tile);
            
            foreach (Tile t in tiles)
            {
                TabPage page = new TabPage();
                page.Text = t.Location;
                TileViewCtrl viewCtrl = new TileViewCtrl(t);
                viewCtrl.Dock = DockStyle.Fill;
                page.Controls.Add(viewCtrl);
                m_tabTop.TabPages.Add(page);
            }
        }

        private void TileViewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }

        private readonly Tile m_tile;
    }
}
