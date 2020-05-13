using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands;
using GoAhead.FPGA;
using GoAhead.Settings;

namespace GoAhead.GUI
{
    public partial class FindTileGUI : Form
    {
        public FindTileGUI(Form invalidateMeAfterEachCommand)
        {
            InitializeComponent();

            m_formToInvalidateAfterEachSearch = invalidateMeAfterEachCommand;

            StoredPreferences.Instance.GUISettings.Open(this);
        }

        public FindTileGUI(UserControl invalidateMeAfterEachCommand)
        {
            InitializeComponent();

            m_userCtrlToInvalidateAfterEachSearch = invalidateMeAfterEachCommand;
        }

        private void InvalidateCalles()
        {
            if (m_formToInvalidateAfterEachSearch != null)
            {
                m_formToInvalidateAfterEachSearch.Invalidate();
            }
            if (m_userCtrlToInvalidateAfterEachSearch != null)
            {
                if (m_userCtrlToInvalidateAfterEachSearch is FPGAViewCtrl)
                {
                    FPGAViewCtrl fpgaView = (FPGAViewCtrl)m_userCtrlToInvalidateAfterEachSearch;
                    fpgaView.PointToSelection = true;
                }

                m_userCtrlToInvalidateAfterEachSearch.Invalidate();
            }
        }

        private void m_btnFindXY_Click(object sender, EventArgs e)
        {
            try
            {
                int x = int.Parse(m_txtX.Text);
                int y = int.Parse(m_txtY.Text);

                CommandExecuter.Instance.Execute(new Commands.Selection.ClearSelection());
                CommandExecuter.Instance.Execute(new Commands.Selection.AddToSelectionXY(x, y, x, y));
                if (StoredPreferences.Instance.ExecuteExpandSelection)
                {
                    CommandExecuter.Instance.Execute(new Commands.Selection.ExpandSelection());
                }

                InvalidateCalles();
            }
            catch (Exception error)
            {
                MessageBox.Show("Input error", "Errror: " + error.Message);
            }
        }

        private void m_btnFindSlice_Click(object sender, EventArgs e)
        {
            string sliceName = m_txtSliceName.Text.Replace(" ", string.Empty);
            foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles())
            {
                foreach (Slice s in t.Slices)
                {
                    if (s.SliceName.Equals(sliceName))
                    {
                        CommandExecuter.Instance.Execute(new Commands.Selection.ClearSelection());
                        CommandExecuter.Instance.Execute(new Commands.Selection.AddToSelectionXY(t.TileKey.X, t.TileKey.Y, t.TileKey.X, t.TileKey.Y));
                        if (StoredPreferences.Instance.ExecuteExpandSelection)
                        {
                            CommandExecuter.Instance.Execute(new Commands.Selection.ExpandSelection());
                        }

                        InvalidateCalles();
                        return;
                    }
                }
            }
        }

        private void m_btnFindLocation_Click(object sender, EventArgs e)
        {
            string location = m_txtLocation.Text.Replace(" ", string.Empty);

            if (!FPGA.FPGA.Instance.Contains(location))
            {
                MessageBox.Show("Location not found", "Error", MessageBoxButtons.OK);
                return;
            }

            Tile t = FPGA.FPGA.Instance.GetTile(location);

            CommandExecuter.Instance.Execute(new Commands.Selection.ClearSelection());
            CommandExecuter.Instance.Execute(new Commands.Selection.AddToSelectionXY(t.TileKey.X, t.TileKey.Y, t.TileKey.X, t.TileKey.Y));
            if (StoredPreferences.Instance.ExecuteExpandSelection)
            {
                CommandExecuter.Instance.Execute(new Commands.Selection.ExpandSelection());
            }
            InvalidateCalles();
        }

        private void FindTileGUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            StoredPreferences.Instance.GUISettings.Close(this);
        }


        private Form m_formToInvalidateAfterEachSearch = null;
        private UserControl m_userCtrlToInvalidateAfterEachSearch = null;
    }
}
