using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.Commands.CommandExecutionSettings;
using GoAhead.Settings;
using GoAhead.Objects;
using GoAhead.GUI.TileView;
using GoAhead.GUI.FPGAView;
using GoAhead.Commands.GUI;


namespace GoAhead.GUI
{
    public partial class FPGAViewCtrl : UserControl, Interfaces.IResetable
    {

        bool _sync = false;
        [
            Category("Appearance"),
            Description("Property to disable/enable auto-sync on both fpga views.")
        ]

        public bool Sync
        {
            get { return _sync; }
            set { _sync = value; }
        }

        bool _expandSelection = true;
        [
            Category("Behavior"),
            Description("Property to disable/enable auto expand selection.")
        ]

        public bool ExpandSelection
        {
            get { return _expandSelection; }
            set { _expandSelection = value; }
        }


        public FPGAViewCtrl()
        {
            InitializeComponent();

            UpdateDrawingOptionsMenu();

            BindingSource macroBsrc = new BindingSource();
            macroBsrc.DataSource = Library.Instance.LibraryElements;
            m_toolStripDrpDownMacro.ComboBox.DisplayMember = "Name";
            m_toolStripDrpDownMacro.ComboBox.ValueMember = "Name";
            m_toolStripDrpDownMacro.ComboBox.DataSource = macroBsrc;

            BindingSource selBsrc = new BindingSource();
            selBsrc.DataSource = TileSelectionManager.Instance.UserSelectionTypes;

            m_toolStripTopDrpDownSelect.ComboBox.DataSource = selBsrc;
            // be notified about reset
            Commands.Reset.ObjectsToReset.Add(this);

            m_timer.Interval = 1000;
            m_timer.Tick += ShowToolTipAfterTimerFired;

            Reset();

            PrintProgressToGUIHook hook = new PrintProgressToGUIHook();
            hook.ProgressBar = m_toolStripProgressBar;
            CommandExecuter.Instance.AddHook(hook);
        }

        private void UpdateDrawingOptionsMenu()
        {
            m_toolStripDrpDownMenuPaintingClockRegion.Checked = StoredPreferences.Instance.HighLightClockRegions;
            m_toolStripDrpDownMenuPaintingMacros.Checked = StoredPreferences.Instance.HighLightPlacedMacros;
            m_toolStripDrpDownMenuPaintingRAM.Checked = StoredPreferences.Instance.HighLightRAMS;
            m_toolStripDrpDownMenuPaintingSelection.Checked = StoredPreferences.Instance.HighLightSelection;
            m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.Checked = StoredPreferences.Instance.HighLightPossibleMacroPlacements;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                return;
            }

            if (TilePaintStrategy == null)
            {
                return;
            }

            // call stratey
            TilePaintStrategy.Invalidate();

            if (TilePaintStrategy != null)
            {
                if (TilePaintStrategy.TileBitmap != null)
                {
                    m_zoomPictBox.Image = TilePaintStrategy.TileBitmap;
                }
            }
            m_zoomPictBox.Invalidate();
            m_panelSelection.Invalidate();

            if (m_zoomPictBox.Image != null)
            {
                float panelWidth = (float)m_zoomPictBox.Image.Width * m_zoomPictBox.Zoom;
                float panelHeight = (float)m_zoomPictBox.Image.Height * m_zoomPictBox.Zoom;
            }

            base.OnPaint(e);
        }

        private void m_panelTop_Paint(object sender, PaintEventArgs e)
        {
            if (RectangleSelect || ZoomSelectOngoing)
            {
                int x = Math.Min(m_mouseDownPosition.X, m_currentMousePositionWithRectangleSelect.X);
                x += m_zoomPictBox.HorizontalScroll.Value;
                int y = Math.Min(m_mouseDownPosition.Y, m_currentMousePositionWithRectangleSelect.Y);
                y += m_zoomPictBox.VerticalScroll.Value;
                int width = Math.Abs(m_mouseDownPosition.X - m_currentMousePositionWithRectangleSelect.X);
                int height = Math.Abs(m_mouseDownPosition.Y - m_currentMousePositionWithRectangleSelect.Y);

                if (StoredPreferences.Instance.RectangleWidth != RectanglePenWidth)
                {
                    RectanglePenWidth = StoredPreferences.Instance.RectangleWidth;
                    m_rectanglePen = new Pen(Color.Black, RectanglePenWidth);
                }


                //e.Graphics.DrawRectangle(Pens.Black, x, y, width, height);
                e.Graphics.DrawRectangle(m_rectanglePen, x, y, width, height);
            }

            if (m_zoomPictBox.Image != null)
            {
                float panelWidth = (float)m_zoomPictBox.Image.Width * m_zoomPictBox.Zoom;
                float panelHeight = (float)m_zoomPictBox.Image.Height * m_zoomPictBox.Zoom;
                m_panelSelection.Width = (int)panelWidth;
                m_panelSelection.Height = (int)panelHeight;
            }

            if (PointToSelection)
            {
                double x, y;
                TileSelectionManager.Instance.GetCenterOfSelection(p => true, out x, out y);
                float xf = (float)x * TileSize * ZoomPictureBox.Zoom;
                float yf = (float)y * TileSize * ZoomPictureBox.Zoom;
                Pen pen = new Pen(Brushes.Black, 4);
                e.Graphics.DrawLine(pen, 0, 0, xf, yf);
            }
        }

        public void SaveFPGAViewAsPNG(string fileName)
        {
            m_zoomPictBox.Image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        public Regex TileRegex
        {
            get { return m_tileFilter; }
        }

        private string GetTileFilter()
        {
            // check wether filter is valid regex
            string filter = m_toolStripTxtBoxTileFilter.Text;
            try
            {
                bool test = Regex.IsMatch("test", filter);
            }
            catch (Exception)
            {
                // invalid
                filter = "";
            }
            return filter;
        }

        public Color GetColor(Tile tile, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            Color c = ColorSettings.Instance.GetColor(tile);

            // shift colors for selected tiles
            int incR = 0;
            int incG = 0;
            int incB = 0;

            if (addIncrementForSelectedTiles && TileSelectionManager.Instance.IsSelected(tile.TileKey))
            {
                incR = ColorSettings.Instance.SelectionIncrement.R;
                incG = ColorSettings.Instance.SelectionIncrement.G;
                incB = ColorSettings.Instance.SelectionIncrement.B;
            }
            else if (addIncrementForUserSelectedTiles && TileSelectionManager.Instance.IsUserSelected(tile.TileKey))
            {
                incR = ColorSettings.Instance.UserSelectionIncrement.R;
                incG = ColorSettings.Instance.UserSelectionIncrement.G;
                incB = ColorSettings.Instance.UserSelectionIncrement.B;
            }
            else if (tile.HasNonstopoverBlockedPorts || tile.HasUsedSlice)
            {
                incR = ColorSettings.Instance.BlockedPortsColor.R;
                incG = ColorSettings.Instance.BlockedPortsColor.G;
                incB = ColorSettings.Instance.BlockedPortsColor.B;
            }

            return Color.FromArgb((c.R + incR) % 256, (c.G + incG) % 256, (c.B + incB) % 256);
            // saturation
            //return Color.FromArgb(
            //((c.R + incR) < 255 ? c.R + incR : 255),
            //((c.G + incG) < 255 ? c.G + incG : 255),
            //((c.B + incB) < 255 ? c.B + incB : 255));
        }

        public TileKey GetClickedKey(int x, int y)
        {
            return XYConverter.GetClickedKey(x, y);
        }

        private void FullZoom()
        {
            int width = Width;
            int heigth = Height;

            float horZoom = (float)m_zoomPictBox.Width / (float)((FPGA.FPGA.Instance.MaxX + 1) * TileSize);
            float verZoom = (float)m_zoomPictBox.Height / (float)((FPGA.FPGA.Instance.MaxY + 1) * TileSize);

            m_zoomPictBox.Zoom = Math.Min(horZoom, verZoom);
            Invalidate();
        }

        public int TileSize
        {
            //get { return 6; }
            get { return 16; }
        }

        #region contextMenu
        private void m_contextMenuStoreAsPartialAreas_Click(object sender, EventArgs e)
        {
            StoreCurrentSelectionAs cmd = new StoreCurrentSelectionAs();
            cmd.UserSelectionType = "PartialArea";
            CommandExecuter.Instance.Execute(cmd);
            Invalidate();
        }

        private void m_contextMenuStoreAsUserDefinedName_Click(object sender, EventArgs e)
        {
            UserSelectionForm frm = new UserSelectionForm(false, true);
            frm.ShowDialog();
        }

        private void m_contextMenuClear_Click(object sender, EventArgs e)
        {
            ClearSelection clearCmd = new ClearSelection();
            CommandExecuter.Instance.Execute(clearCmd);
        }

        private void m_contextMenuTunnel_Click_1(object sender, EventArgs e)
        {
            PortSelectionForm dlg = new PortSelectionForm();
            dlg.Show();
        }
        private void m_contextMenuCopyIdentifier_Click(object sender, EventArgs e)
        {
            if (FPGA.FPGA.Instance.Contains(m_rightClickedKey))
            {
                string location = FPGA.FPGA.Instance.GetTile(m_rightClickedKey).Location;
                Clipboard.SetText(location);
            }
        }
        private void m_contextMenuFullZoom_Click(object sender, EventArgs e)
        {
            FullZoom();
        }
        #endregion

        public void ZoomOut()
        {
            m_zoomPictBox.PrevZoom = m_zoomPictBox.Zoom;
            m_zoomPictBox.ZoomPoint = new Point(0,0);
            m_zoomPictBox.Zoom *= 0.9F;
            Invalidate();
        }

        public void ZoomIn()
        {
            m_zoomPictBox.PrevZoom = m_zoomPictBox.Zoom;
            m_zoomPictBox.ZoomPoint = new Point(0,0);
            m_zoomPictBox.Zoom *= 1.1F;

            Invalidate();
        }

        public void PointZoomIn(Point location, Point relativeLocation)
        {
            int x, y;
   
            x = (int) (relativeLocation.X * 0.95);
            y = (int) (relativeLocation.Y * 1.05);

            m_zoomPictBox.PrevZoom = m_zoomPictBox.Zoom;
            m_zoomPictBox.ZoomPoint = new Point(x,y);
            m_zoomPictBox.Zoom *= 1.2F;

            Invalidate();
        }

        public void PointZoomOut(Point location, Point relativeLocation)
        {
            m_zoomPictBox.PrevZoom = m_zoomPictBox.Zoom;
            m_zoomPictBox.ZoomPoint = new Point(0, 0);
            m_zoomPictBox.Zoom *= 0.9F;
            Invalidate();
        }

        #region Toolbar
        private void m_toolStripBtnZoomOut_Click(object sender, EventArgs e)
        {

            ZoomOut();
        }
        private void m_toolStripBtnZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();

        }
        private void m_toolStripBtnExpandSelection_Click(object sender, EventArgs e)
        {
            if (this.ExpandSelection)
                this.ExpandSelection = false;
            else
                this.ExpandSelection = true;         
        }

        /// <summary>
        /// Redraw upon enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_toolStripTxtBoxTileFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string pattern = GetTileFilter();
                m_tileFilter = new Regex(pattern, RegexOptions.Compiled);
                TilePaintStrategy.DrawTiles(true, true);
                Invalidate();
            }
        }

        private void m_toolStripDrpDownMenuPainting_Click(object sender, EventArgs e)
        {
            UpdateDrawingOptionsMenu();
        }

        public void UpdateStatusStrip(TileKey selectedKey)
        {
            string statusString = "";
            if (FPGA.FPGA.Instance.Contains(selectedKey))
            {
                Tile selectedTile = FPGA.FPGA.Instance.GetTile(selectedKey);
                statusString = selectedTile.Location + " " + selectedKey.ToString();

                if (selectedTile.Slices.Count > 0)
                {
                    statusString += " (";

                    // only display up to to slice
                    int displayedSlices = Math.Min(2, selectedTile.Slices.Count);
                    for (int i = 0; i < displayedSlices; i++)
                    {
                        statusString += selectedTile.Slices[i].ToString();
                        if (i != selectedTile.Slices.Count - 1)
                        {
                            statusString += ",";
                        }
                    }

                    // add dots for tiles with more than two slices
                    if (selectedTile.Slices.Count > 2)
                    {
                        statusString += "...";
                    }

                    statusString += ") ";
                }
                // 2 element buffer
                m_lastClickedTile = m_currentlyClickedTile;
                m_currentlyClickedTile = selectedTile;
            }

            // print selection info
            if (StoredPreferences.Instance.PrintSelectionResourceInfo)
            {
                statusString += "Selection contains ";
                statusString += TileSelectionManager.Instance.NumberOfSelectedTiles + " tiles, ";
                int clbs = 0;
                int brams = 0;
                int dsps = 0;
                TileSelectionManager.Instance.GetRessourcesInSelection(TileSelectionManager.Instance.GetSelectedTiles(), out clbs, out brams, out dsps);
                int others = TileSelectionManager.Instance.NumberOfSelectedTiles - (clbs + brams + dsps);
                statusString += clbs + " CLBs, ";
                statusString += brams + " BRAMs, and ";
                statusString += dsps + " DPS tiles, and ";
                statusString += others + " other tiles";
            }

            m_statusStripLabelSelectedTile.Text = statusString;
        }

        private void m_toolStripBtnFind_Click(object sender, EventArgs e)
        {
            FindTileGUI dlg = new FindTileGUI(this);
            dlg.Show();
        }

        #endregion Toolbar

        #region ToolbarDropDownMenu
        private void m_toolStripDrpDownMenuPaintingRAM_Click(object sender, EventArgs e)
        {
            StoredPreferences.Instance.HighLightRAMS = m_toolStripDrpDownMenuPaintingRAM.Checked;
            if (!StoredPreferences.Instance.HighLightRAMS)
            {
                // turn off -> repaint all
                TilePaintStrategy.DrawTiles(true, true);
            }

            Invalidate();
        }


        private void m_toolStripDrpDownMenuPaintingSelection_Click(object sender, EventArgs e)
        {
            StoredPreferences.Instance.HighLightSelection = m_toolStripDrpDownMenuPaintingSelection.Checked;
            if (!StoredPreferences.Instance.HighLightSelection)
            {
                // turn off -> repaint all
                TilePaintStrategy.DrawTiles(true, true);
            }

            Invalidate();
        }

        private void m_toolStripDrpDownMenuPaintingClockRegion_Click(object sender, EventArgs e)
        {
            StoredPreferences.Instance.HighLightClockRegions = m_toolStripDrpDownMenuPaintingClockRegion.Checked;
            if (!StoredPreferences.Instance.HighLightClockRegions)
            {
                // turn off -> repaint all
                TilePaintStrategy.DrawTiles(true, true);
            }

            Invalidate();
        }

        private void m_toolStripDrpDownMenuPaintingMacros_Click(object sender, EventArgs e)
        {
            StoredPreferences.Instance.HighLightPlacedMacros = m_toolStripDrpDownMenuPaintingMacros.Checked;
            if (!StoredPreferences.Instance.HighLightPlacedMacros)
            {
                // turn off -> repaint all
                TilePaintStrategy.DrawTiles(true, true);
            }

            Invalidate();
        }

        private void m_toolStripDrpDownMenuPaintingPossibleMacroPlacements_Click(object sender, EventArgs e)
        {
            StoredPreferences.Instance.HighLightPossibleMacroPlacements = m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.Checked;
            if (!StoredPreferences.Instance.HighLightPossibleMacroPlacements)
            {
                // turn off -> repaint all
                TilePaintStrategy.DrawTiles(true, true);
            }

            Invalidate();
        }

        private void m_toolStripDrpDownMenuPaintingToolTips_Click_1(object sender, EventArgs e)
        {
            StoredPreferences.Instance.ShowToolTips = m_toolStripDrpDownMenuPaintingToolTips.Checked;
            Invalidate();
        }

        private void m_toolStripDrpDownMacro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_toolStripDrpDownMacro.SelectedItem == null)
            {
                return;
            }

            if (!(m_toolStripDrpDownMacro.SelectedItem is Objects.LibraryElement))
            {
                // should never happen
                return;
            }
            if (TilePaintStrategy == null)
            {
                return;
            }

            foreach (HighLighter highLighter in TilePaintStrategy.HighLighter.Where(hl => hl is PossibleMacroPlacementHighLighter))
            {
                LibraryElement libElement = (LibraryElement)m_toolStripDrpDownMacro.SelectedItem;
                ((PossibleMacroPlacementHighLighter)highLighter).LibrayElementName = libElement.Name;
            }
        }

        private void m_toolStripTopDrpDownSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // is null after deleting a user selection
            if (m_toolStripTopDrpDownSelect.SelectedItem == null)
            {
                return;
            }
            string userSel = m_toolStripTopDrpDownSelect.SelectedItem.ToString();

            if (TileSelectionManager.Instance.GetUserSelectionTileCount(userSel) > 0)
            {
                SelectUserSelection selCmd = new SelectUserSelection();
                selCmd.UserSelectionType = userSel;
                CommandExecuter.Instance.Execute(selCmd);

                Tile upperLeft = TileSelectionManager.Instance.GetUserSelectedTile("", selCmd.UserSelectionType, FPGATypes.Placement.UpperLeft);
                UpdateStatusStrip(upperLeft.TileKey);
            }
        }

        private void m_toolStripTopBtnClear_Click(object sender, EventArgs e)
        {
            // is null after deleting a user selection
            if (m_toolStripTopDrpDownSelect.SelectedItem == null)
            {
                return;
            }
            ClearUserSelection clearCmd = new ClearUserSelection();
            clearCmd.UserSelectionType = m_toolStripTopDrpDownSelect.SelectedItem.ToString();
            CommandExecuter.Instance.Execute(clearCmd);
        }

        private void m_toolStripDrpDownMenuPaintingToolTips_Click_2(object sender, EventArgs e)
        {
            StoredPreferences.Instance.ShowToolTips = m_toolStripDrpDownMenuPaintingToolTips.Checked;
        }


        private void m_toolStripDrpDownMenuMuteOutput_Click(object sender, EventArgs e)
        {
            if (m_toolStripDrpDownMenuMuteOutput.Checked)
            {
                MuteOutput cmd = new MuteOutput();
                CommandExecuter.Instance.Execute(cmd);
            }
            else
            {
                UnmuteOutput cmd = new UnmuteOutput();
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        private void m_toolStripDrpDownMenuSyncViews_Click(object sender, EventArgs e)
        {
            if (m_toolStripDrpDownMenuSyncViews.Checked)
            {
                this.Sync = true;
            }
            else
            {
                this.Sync = false;
            }
        }





        private void m_toolStripDrpDownMenuPainting_MouseDown(object sender, MouseEventArgs e)
        {
            m_toolStripDrpDownMenuPaintingToolTips.Checked = StoredPreferences.Instance.ShowToolTips;
        }

        #endregion

        public void Reset()
        {
            if (TilePaintStrategy != null)
            {
                TilePaintStrategy.Reset();
                TilePaintStrategy.DrawTiles(true, true);
                m_zoomPictBox.Image = TilePaintStrategy.TileBitmap;
            }

            FullZoom();
            m_zoomPictBox.Invalidate();
            m_panelSelection.Invalidate();
        }

        private void m_zoomPictBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                SelectedFromMouseDownToCurrent();

                UpdateStatusStrip(GetClickedKey(e.X, e.Y));
                RectangleSelect = false;
                ZoomSelectOngoing = false;
                Invalidate();
                m_panelSelection.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                m_rightClickedKey = GetClickedKey(e.X, e.Y);
                m_contextMenu.Show(this, new Point(e.X, e.Y));
            }
            /*
            else if(e.Button == MouseButtons.Middle)
            {
                if (ZoomSelectOngoing)
                {
                    ZoomFromMouseDownToCurrent();

                    ZoomSelectOngoing = false;
                    UpdateStatusStrip(GetClickedKey(e.X, e.Y));
                    RectangleSelect = false;
                    Invalidate();
                    m_panelSelection.Invalidate();
                }
            }
            */
        }

        private void ZoomFromMouseDownToCurrent()
        {
            if (m_currentMousePositionWithRectangleSelect.X < m_mouseDownPosition.X && m_currentMousePositionWithRectangleSelect.Y < m_mouseDownPosition.Y)
            {
                m_zoomPictBox.Zoom *= 0.5f;
            }
            else
            {
                m_zoomPictBox.Zoom *= 1.5f;
            }

            //this.m_zoomPictBox.HorizontalScroll.Value = upperLeftTile.X;
            //this.m_zoomPictBox.VerticalScroll.Value = upperLeftTile.Y;
        }

        private void SelectedFromMouseDownToCurrent()
        {
            bool shiftDown = ModifierKeys == Keys.Shift;
            bool ctrlDown = ModifierKeys == Keys.Control;
            bool altDown = ModifierKeys == Keys.Alt;
            bool altAndCtrlDown = ModifierKeys == (Keys.Control | Keys.Alt);

            if (!ctrlDown && !altAndCtrlDown)
            {
                CommandExecuter.Instance.Execute(new Commands.Selection.ClearSelection());
            }
            TileKey upperLeftTile = null;
            TileKey lowerRightTile = null;

            if (shiftDown)
            {
                TileKey clickedKey = GetClickedKey(m_mouseDownPosition.X, m_mouseDownPosition.Y);
                if (m_lastClickedTile == null)
                {
                    return;
                }
                if (!FPGA.FPGA.Instance.Contains(clickedKey))
                {
                    return;
                }

                upperLeftTile = m_lastClickedTile.TileKey;
                lowerRightTile = clickedKey;
            }
            else
            {
                int upperLeftX = Math.Min(m_mouseDownPosition.X, m_currentMousePositionWithRectangleSelect.X);
                int upperLeftY = Math.Min(m_mouseDownPosition.Y, m_currentMousePositionWithRectangleSelect.Y);
                int lowerRightX = Math.Max(m_mouseDownPosition.X, m_currentMousePositionWithRectangleSelect.X);
                int lowerRightY = Math.Max(m_mouseDownPosition.Y, m_currentMousePositionWithRectangleSelect.Y);

                upperLeftTile = GetClickedKey(upperLeftX, upperLeftY);
                lowerRightTile = GetClickedKey(lowerRightX, lowerRightY);
            }

            if (!string.IsNullOrEmpty(GetTileFilter()))
            {
                AddToSelectionXY addCmd = new AddToSelectionXY();
                addCmd.Filter = GetTileFilter();
                addCmd.LowerRightX = 0;
                addCmd.LowerRightY = 0;
                addCmd.UpperLeftX = 0;
                addCmd.UpperLeftY = 0;
                CommandExecuter.Instance.Execute(addCmd);
            }
            else if (altDown || altAndCtrlDown)
            {
                //Tile ult = FPGA.FPGA.Instance.GetTile(upperLeftTile);
                //Tile lrt = FPGA.FPGA.Instance.GetTile(lowerRightTile);
                try
                {
                    //Switch to using the INT_XxYy tile.

                    AddBlockToSelection addcmd = new AddBlockToSelection(upperLeftTile.X, upperLeftTile.Y, lowerRightTile.X, lowerRightTile.Y);
                    CommandExecuter.Instance.Execute(addcmd);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Use the fine-tile grid as default.
                CommandExecuter.Instance.Execute(new AddToSelectionXY(upperLeftTile.X, upperLeftTile.Y, lowerRightTile.X, lowerRightTile.Y));
            }

            if (StoredPreferences.Instance.ExecuteExpandSelection && this.ExpandSelection)
            {
                CommandExecuter.Instance.Execute(new Commands.Selection.ExpandSelection());
            }
        }


        private void m_zoomPictBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (RectangleSelect)
            {
                m_currentMousePositionWithRectangleSelect = e.Location;
                m_panelSelection.Invalidate();
            }
            if (ZoomSelectStart || ZoomSelectOngoing)
            {
                ZoomSelectStart = false;
                ZoomSelectOngoing = true;
                m_currentMousePositionWithRectangleSelect = e.Location;
                m_panelSelection.Invalidate();

            }
            if (m_mouseEntered && StoredPreferences.Instance.ShowToolTips)
            {
                // start timer for toll tip
                m_timer.Stop();
                m_timer.Start();
                // store position
                m_timer.Tag = e.Location;
            }
        }

        private void ShowToolTipAfterTimerFired(object sender, EventArgs e)
        {
            if (StoredPreferences.Instance.ShowToolTips)
            {
                Point p = (Point)m_timer.Tag;

                // only show a new tool tip, if the position changes
                if (m_lastToolTipLocation.Equals(p))
                {
                    return;
                }

                m_lastToolTipLocation = p;
                TileKey key = GetClickedKey(p.X, p.Y);
                if (FPGA.FPGA.Instance.Contains(key))
                {
                    Tile where = FPGA.FPGA.Instance.GetTile(key);
                    string toolTip = key.ToString() + " " + where.Location;
                    Objects.LibElemInst inst = LibraryElementInstanceManager.Instance.GetInstantiation(where);
                    if (inst != null)
                    {
                        toolTip += Environment.NewLine + inst.ToString();

                        foreach (Tuple<string, string, PortMapper.MappingKind> tuple in inst.PortMapper.GetMappings().OrderBy(t => t.Item1))
                        {
                            toolTip += Environment.NewLine + tuple.Item1 + " => " + tuple.Item2 + " (" + tuple.Item3 + ")";
                        }
                    }
                    if (Blackboard.Instance.HasToolTipInfo(where))
                    {
                        toolTip += Environment.NewLine + Blackboard.Instance.GetToolTipInfo(where);
                    }
                    m_toolTip.Show(toolTip, this, p.X, p.Y + 20, 10000);
                }
            }
        }

        private void m_zoomPictBox_MouseDown(object sender, MouseEventArgs e)
        {
            PointToSelection = false;

            //if (e.Button.Equals(MouseButtons.Left) || e.Button.Equals(MouseButtons.Middle))
            if (e.Button.Equals(MouseButtons.Left))
            {
                // capture old value for range selection
                m_mouseDownPosition = e.Location;
                m_currentMousePositionWithRectangleSelect = e.Location;
                UpdateStatusStrip(GetClickedKey(e.X, e.Y));
            }

            if (e.Button.Equals(MouseButtons.Left))
            {
                RectangleSelect = true;
            }
            /*
            if (e.Button.Equals(MouseButtons.Middle))
            {
                ZoomSelectStart = true;
            } */
        }

        private void m_zoomPictBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TileKey clickedKey = GetClickedKey(e.X, e.Y);
            bool ctrlAndShiftDown = ModifierKeys == (Keys.Control | Keys.Shift);

            if (FPGA.FPGA.Instance.Contains(clickedKey))
            {
                OpenTileView cmd = new OpenTileView
                {
                    X = clickedKey.X,
                    Y = clickedKey.Y,
                    doExpand = !ctrlAndShiftDown
                };
                CommandExecuter.Instance.Execute(cmd);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Point currentPixel, cursorPosition;

            bool ctrlDown = ModifierKeys == Keys.Control;
            cursorPosition = new Point(e.X, e.Y);

            currentPixel = new Point(Math.Min(m_mouseDownPosition.X, m_currentMousePositionWithRectangleSelect.X) + m_zoomPictBox.HorizontalScroll.Value,
                              Math.Min(m_mouseDownPosition.Y, m_currentMousePositionWithRectangleSelect.Y) + m_zoomPictBox.VerticalScroll.Value);
 

            if (ctrlDown && e.Delta/120 >=1.0)
            {
                PointZoomIn(cursorPosition,currentPixel);
            }
            else if (ctrlDown && e.Delta / 120 <= 1.0)
            {
                PointZoomOut(currentPixel, cursorPosition);
            }
        }

        private void m_zoomPictBox_MouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e);
        }

        public ZoomPicBox ZoomPictureBox
        {
            get { return m_zoomPictBox; }
        }
             
        private void m_contextMenuSelectAll_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GetTileFilter()))
            {
                SelectAllWithFilter selCmd = new SelectAllWithFilter();
                selCmd.Filter = GetTileFilter();
                CommandExecuter.Instance.Execute(selCmd);
            }
            else
            {
                SelectAll selCmd = new SelectAll();
                CommandExecuter.Instance.Execute(selCmd);
            }
        }

        private void m_contextMenuInvertSelection_Click(object sender, EventArgs e)
        {
            InvertSelection invCmd = new InvertSelection();
            CommandExecuter.Instance.Execute(invCmd);
        }

        public float RectanglePenWidth
        {
            get { return m_rectanglePen.Width; }
            set { m_rectanglePen.Width = value; }
        }

        #region Member
        /// <summary>
        /// The strategy to get the tile be mouse down coordinates
        /// </summary>
        public ClickToTileKeyConverter XYConverter = null;
        private Regex m_tileFilter = new Regex("", RegexOptions.Compiled);
        private Point m_mouseDownPosition = new Point(0, 0);
        private Point m_currentMousePositionWithRectangleSelect = new Point(0, 0);
        public bool ZoomSelectStart { get; set; }
        public bool ZoomSelectOngoing { get; set; }
        public bool RectangleSelect { get; set; }
        public bool PointToSelection { get; set; }

        private bool m_mouseEntered = false;

        /// <summary>
        /// Strange: The mouse move event occurs after the tooltip disappears
        /// Hoever, the location property does not change with the event
        /// In order not to show the tooltip again, we store the last position and only reshow the tooltip if the position changes
        /// </summary>
        private Point m_lastToolTipLocation = new Point(0, 0);

        /// <summary>
        ///  painting strategy
        /// </summary>
        public Painter TilePaintStrategy = null;

        private ToolTip m_toolTip = new ToolTip();
        private Timer m_timer = new Timer();
        private Tile m_currentlyClickedTile = null;
        private Tile m_lastClickedTile = null;

        private void m_zoomPictBox_MouseEnter(object sender, EventArgs e)
        {
            m_mouseEntered = true;
        }

        private void m_zoomPictBox_MouseLeave(object sender, EventArgs e)
        {
            m_mouseEntered = false;
        }

        private TileKey m_rightClickedKey = null;

        private Pen m_rectanglePen = new Pen(Color.Black, 1);

        #endregion Member


  

    }
}
