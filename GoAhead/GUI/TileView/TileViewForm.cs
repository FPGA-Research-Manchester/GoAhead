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
    public partial class TileViewForm : Form
    {
        public TileViewForm(Tile tile)
        {
            InitializeComponent();

            m_tile = tile;
            Settings.StoredPreferences.Instance.GUISettings.Open(this);

            List<Tile> tiles = new List<Tile>();
            tiles.Add(m_tile);
            if (IdentifierManager.Instance.IsMatch(m_tile.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                //Add adjacent clb tiles.
                tiles.AddRange(FPGATypes.GetCLTile(m_tile));

                //Add adjacent sub-interconnect tiles.
                tiles.AddRange(FPGATypes.GetSubInterconnectTile(m_tile));
            }
            if (IdentifierManager.Instance.IsMatch(m_tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                Tile interconnect = FPGATypes.GetInterconnectTile(m_tile);

                //Add interconnect.
                tiles.Add(interconnect);

                // Add clb tiles.
                foreach(Tile clb in FPGATypes.GetCLTile(interconnect))
                {
                    if(!tiles.Contains(clb))
                    {
                        tiles.Add(clb);
                    }
                }

                //Add sub-interconnect tiles.
                tiles.AddRange(FPGATypes.GetSubInterconnectTile(m_tile));

            }
            if (IdentifierManager.Instance.IsMatch(m_tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                Tile interconnect = FPGATypes.GetInterconnectTile(m_tile);

                //Add interconnect.
                tiles.Add(interconnect);

                // Add clb tiles.
                foreach (Tile clb in FPGATypes.GetCLTile(interconnect))
                {
                    if (!tiles.Contains(clb))
                    {
                        tiles.Add(clb);
                    }
                }
            }
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
