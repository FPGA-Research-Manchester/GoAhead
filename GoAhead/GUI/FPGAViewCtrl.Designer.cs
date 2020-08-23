using System.Drawing;

namespace GoAhead.GUI
{
    public partial class FPGAViewCtrl : GoAhead.Interfaces.IResetable
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            this.m_rectanglePen.Dispose();
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPGAViewCtrl));
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.m_statusStripLabelSelectedTile = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_contextMenuStore = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuStoreAsPartialAreas = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuStoreAsUserDefinedName = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuTunnel = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuInvertSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.m_contextMenuCopyIdentifier = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_contextMenuFullZoom = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripBtnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.m_toolStripBtnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.m_toolStripBtnExpandSelection = new System.Windows.Forms.ToolStripButton();
            this.m_checkBoxExpandSelection = new System.Windows.Forms.CheckBox();
            this.m_toolStripBtnFind = new System.Windows.Forms.ToolStripButton();
            this.m_toolStripLblFilter = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_toolStripTxtBoxTileFilter = new System.Windows.Forms.ToolStripTextBox();
            this.m_toolStripDrpDownMenuPainting = new System.Windows.Forms.ToolStripDropDownButton();
            this.m_toolStripDrpDownMenuPaintingRAM = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuPaintingClockRegion = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuPaintingMacros = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuPaintingToolTips = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuMuteOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuSyncViews = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripDrpDownMenuPaintingSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_toolStripLblMacros = new System.Windows.Forms.ToolStripLabel();
            this.m_toolStripDrpDownMacro = new System.Windows.Forms.ToolStripComboBox();
            this.m_toolStripLabelTopSelect = new System.Windows.Forms.ToolStripLabel();
            this.m_toolStripTopDrpDownSelect = new System.Windows.Forms.ToolStripComboBox();
            this.m_toolStripBottom = new System.Windows.Forms.ToolStrip();
            this.m_toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.m_zoomPictBox = new GoAhead.GUI.ZoomPicBox();
            this.m_panelSelection = new System.Windows.Forms.Panel();
            this.m_statusStrip.SuspendLayout();
            this.m_contextMenu.SuspendLayout();
            this.m_toolStripBottom.SuspendLayout();
            this.m_zoomPictBox.SuspendLayout();

            this.m_zoomPictBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.m_zoomPictBox_MouseWheel);
            this.SuspendLayout();
            // 
            // m_statusStrip
            // 
            this.m_statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_statusStripLabelSelectedTile});
            this.m_statusStrip.Location = new System.Drawing.Point(0, 523);
            this.m_statusStrip.Name = "m_statusStrip";
            this.m_statusStrip.Size = new System.Drawing.Size(1085, 22);
            this.m_statusStrip.TabIndex = 0;
            this.m_statusStrip.Text = "statusStrip1";
            // 
            // m_statusStripLabelSelectedTile
            // 
            this.m_statusStripLabelSelectedTile.Name = "m_statusStripLabelSelectedTile";
            this.m_statusStripLabelSelectedTile.Size = new System.Drawing.Size(0, 17);
            // 
            // m_contextMenu
            // 
            this.m_contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_contextMenuStore,
            this.m_contextMenuTunnel,
            this.m_contextMenuClear,
            this.m_contextMenuInvertSelection,
            this.m_contextMenuSelectAll,
            this.m_contextMenuCopyIdentifier,
            this.toolStripSeparator3,
            this.m_contextMenuFullZoom});
            this.m_contextMenu.Name = "m_contextMenu";
            this.m_contextMenu.Size = new System.Drawing.Size(269, 164);
            // 
            // m_contextMenuStore
            // 
            this.m_contextMenuStore.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_contextMenuStoreAsPartialAreas,
            this.m_contextMenuStoreAsUserDefinedName});
            this.m_contextMenuStore.Name = "m_contextMenuStore";
            this.m_contextMenuStore.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuStore.Text = "Define current selection as";
            // 
            // m_contextMenuStoreAsPartialAreas
            // 
            this.m_contextMenuStoreAsPartialAreas.Name = "m_contextMenuStoreAsPartialAreas";
            this.m_contextMenuStoreAsPartialAreas.Size = new System.Drawing.Size(176, 22);
            this.m_contextMenuStoreAsPartialAreas.Text = "Partial area";
            this.m_contextMenuStoreAsPartialAreas.Click += new System.EventHandler(this.m_contextMenuStoreAsPartialAreas_Click);
            // 
            // m_contextMenuStoreAsUserDefinedName
            // 
            this.m_contextMenuStoreAsUserDefinedName.Name = "m_contextMenuStoreAsUserDefinedName";
            this.m_contextMenuStoreAsUserDefinedName.Size = new System.Drawing.Size(176, 22);
            this.m_contextMenuStoreAsUserDefinedName.Text = "User Defined Name";
            this.m_contextMenuStoreAsUserDefinedName.Click += new System.EventHandler(this.m_contextMenuStoreAsUserDefinedName_Click);
            // 
            // m_contextMenuTunnel
            // 
            this.m_contextMenuTunnel.Name = "m_contextMenuTunnel";
            this.m_contextMenuTunnel.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuTunnel.Text = "Define Non-Blockable Ports (Tunnel)";
            this.m_contextMenuTunnel.Click += new System.EventHandler(this.m_contextMenuTunnel_Click_1);
            // 
            // m_contextMenuClear
            // 
            this.m_contextMenuClear.Name = "m_contextMenuClear";
            this.m_contextMenuClear.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuClear.Text = "Clear Selection";
            this.m_contextMenuClear.Click += new System.EventHandler(this.m_contextMenuClear_Click);
            // 
            // m_contextMenuInvertSelection
            // 
            this.m_contextMenuInvertSelection.Name = "m_contextMenuInvertSelection";
            this.m_contextMenuInvertSelection.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuInvertSelection.Text = "Invert Selection";
            this.m_contextMenuInvertSelection.Click += new System.EventHandler(this.m_contextMenuInvertSelection_Click);
            // 
            // m_contextMenuSelectAll
            // 
            this.m_contextMenuSelectAll.Name = "m_contextMenuSelectAll";
            this.m_contextMenuSelectAll.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuSelectAll.Text = "Select All";
            this.m_contextMenuSelectAll.Click += new System.EventHandler(this.m_contextMenuSelectAll_Click);
            // 
            // m_contextMenuCopyIdentifier
            // 
            this.m_contextMenuCopyIdentifier.Name = "m_contextMenuCopyIdentifier";
            this.m_contextMenuCopyIdentifier.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuCopyIdentifier.Text = "Copy Identifier";
            this.m_contextMenuCopyIdentifier.Click += new System.EventHandler(this.m_contextMenuCopyIdentifier_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(265, 6);
            // 
            // m_contextMenuFullZoom
            // 
            this.m_contextMenuFullZoom.Name = "m_contextMenuFullZoom";
            this.m_contextMenuFullZoom.Size = new System.Drawing.Size(268, 22);
            this.m_contextMenuFullZoom.Text = "Full Zoom";
            this.m_contextMenuFullZoom.Click += new System.EventHandler(this.m_contextMenuFullZoom_Click);
            // 
            // m_toolStripBtnZoomIn
            // 
            this.m_toolStripBtnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_toolStripBtnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("m_toolStripBtnZoomIn.Image")));
            this.m_toolStripBtnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_toolStripBtnZoomIn.Name = "m_toolStripBtnZoomIn";
            this.m_toolStripBtnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.m_toolStripBtnZoomIn.Text = "Zoom In";
            this.m_toolStripBtnZoomIn.Click += new System.EventHandler(this.m_toolStripBtnZoomIn_Click);
            // 
            // m_toolStripBtnZoomOut
            // 
            this.m_toolStripBtnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_toolStripBtnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("m_toolStripBtnZoomOut.Image")));
            this.m_toolStripBtnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_toolStripBtnZoomOut.Name = "m_toolStripBtnZoomOut";
            this.m_toolStripBtnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.m_toolStripBtnZoomOut.Text = "Zoom Out";
            this.m_toolStripBtnZoomOut.Click += new System.EventHandler(this.m_toolStripBtnZoomOut_Click);
            // 
            // m_toolStripBtnExpandSelection
            // 

        
            this.m_toolStripBtnExpandSelection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_toolStripBtnExpandSelection.Name = "m_toolStripBtnExpandSelection";
            this.m_toolStripBtnExpandSelection.Size = new System.Drawing.Size(23, 22);
            this.m_toolStripBtnExpandSelection.Text = "Expand Selection";
            this.m_toolStripBtnExpandSelection.Click += new System.EventHandler(this.m_toolStripBtnExpandSelection_Click);
            this.m_toolStripBtnExpandSelection.Checked = true;
            this.m_toolStripBtnExpandSelection.CheckOnClick = true;
            
          
            
                            
            
            // 
            // m_toolStripBtnFind
            // 
            this.m_toolStripBtnFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_toolStripBtnFind.Image = ((System.Drawing.Image)(resources.GetObject("m_toolStripBtnFind.Image")));
            this.m_toolStripBtnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_toolStripBtnFind.Name = "m_toolStripBtnFind";
            this.m_toolStripBtnFind.Size = new System.Drawing.Size(23, 22);
            this.m_toolStripBtnFind.Text = "Find";
            this.m_toolStripBtnFind.Click += new System.EventHandler(this.m_toolStripBtnFind_Click);
            // 
            // m_toolStripLblFilter
            // 
            this.m_toolStripLblFilter.Name = "m_toolStripLblFilter";
            this.m_toolStripLblFilter.Size = new System.Drawing.Size(55, 22);
            this.m_toolStripLblFilter.Text = "Tile Filter";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // m_toolStripTxtBoxTileFilter
            // 
            this.m_toolStripTxtBoxTileFilter.Name = "m_toolStripTxtBoxTileFilter";
            this.m_toolStripTxtBoxTileFilter.Size = new System.Drawing.Size(150, 25);
            this.m_toolStripTxtBoxTileFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_toolStripTxtBoxTileFilter_KeyUp);
            // 
            // m_toolStripDrpDownMenuPainting
            // 
            this.m_toolStripDrpDownMenuPainting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.m_toolStripDrpDownMenuPainting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_toolStripDrpDownMenuPaintingRAM,
            this.m_toolStripDrpDownMenuPaintingClockRegion,
            this.m_toolStripDrpDownMenuPaintingMacros,
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements,
            this.m_toolStripDrpDownMenuPaintingToolTips,
            this.m_toolStripDrpDownMenuMuteOutput,
            this.m_toolStripDrpDownMenuPaintingSelection,
            this.m_toolStripDrpDownMenuSyncViews});
            this.m_toolStripDrpDownMenuPainting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_toolStripDrpDownMenuPainting.Name = "m_toolStripDrpDownMenuPainting";
            this.m_toolStripDrpDownMenuPainting.Size = new System.Drawing.Size(62, 22);
            this.m_toolStripDrpDownMenuPainting.Text = "Options";
            this.m_toolStripDrpDownMenuPainting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.m_toolStripDrpDownMenuPainting.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPainting_Click);
            this.m_toolStripDrpDownMenuPainting.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_toolStripDrpDownMenuPainting_MouseDown);
            // 
            // m_toolStripDrpDownMenuPaintingRAM
            // 
            this.m_toolStripDrpDownMenuPaintingRAM.CheckOnClick = true;
            this.m_toolStripDrpDownMenuPaintingRAM.Name = "m_toolStripDrpDownMenuPaintingRAM";
            this.m_toolStripDrpDownMenuPaintingRAM.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuPaintingRAM.Text = "RAM";
            this.m_toolStripDrpDownMenuPaintingRAM.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPaintingRAM_Click);
            // 
            // m_toolStripDrpDownMenuPaintingClockRegion
            // 
            this.m_toolStripDrpDownMenuPaintingClockRegion.CheckOnClick = true;
            this.m_toolStripDrpDownMenuPaintingClockRegion.Name = "m_toolStripDrpDownMenuPaintingClockRegion";
            this.m_toolStripDrpDownMenuPaintingClockRegion.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuPaintingClockRegion.Text = "Clock Region";
            this.m_toolStripDrpDownMenuPaintingClockRegion.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPaintingClockRegion_Click);
            // 
            // m_toolStripDrpDownMenuPaintingMacros
            // 
            this.m_toolStripDrpDownMenuPaintingMacros.CheckOnClick = true;
            this.m_toolStripDrpDownMenuPaintingMacros.Name = "m_toolStripDrpDownMenuPaintingMacros";
            this.m_toolStripDrpDownMenuPaintingMacros.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuPaintingMacros.Text = "Placed Macros";
            this.m_toolStripDrpDownMenuPaintingMacros.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPaintingMacros_Click);
            // 
            // m_toolStripDrpDownMenuPaintingPossibleMacroPlacements
            // 
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.CheckOnClick = true;
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.Name = "m_toolStripDrpDownMenuPaintingPossibleMacroPlacements";
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.Text = "Possible Macro Placements";
            this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPaintingPossibleMacroPlacements_Click);
            // 
            // m_toolStripDrpDownMenuPaintingToolTips
            // 
            this.m_toolStripDrpDownMenuPaintingToolTips.CheckOnClick = true;
            this.m_toolStripDrpDownMenuPaintingToolTips.Name = "m_toolStripDrpDownMenuPaintingToolTips";
            this.m_toolStripDrpDownMenuPaintingToolTips.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuPaintingToolTips.Text = "Tool Tips";
            this.m_toolStripDrpDownMenuPaintingToolTips.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPaintingToolTips_Click_2);
            // 
            // m_toolStripDrpDownMenuMuteOutput
            // 
            this.m_toolStripDrpDownMenuMuteOutput.CheckOnClick = true;
            this.m_toolStripDrpDownMenuMuteOutput.Name = "m_toolStripDrpDownMenuMuteOutput";
            this.m_toolStripDrpDownMenuMuteOutput.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuMuteOutput.Text = "Mute Output";
            this.m_toolStripDrpDownMenuMuteOutput.Click += new System.EventHandler(this.m_toolStripDrpDownMenuMuteOutput_Click);
            // 
            // m_toolStripDrpDownMenuPaintingSelection
            // 
            this.m_toolStripDrpDownMenuPaintingSelection.CheckOnClick = true;
            this.m_toolStripDrpDownMenuPaintingSelection.Name = "m_toolStripDrpDownMenuPaintingSelection";
            this.m_toolStripDrpDownMenuPaintingSelection.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuPaintingSelection.Text = "Highligt Selection";
            this.m_toolStripDrpDownMenuPaintingSelection.Click += new System.EventHandler(this.m_toolStripDrpDownMenuPaintingSelection_Click);
            //
            // m_toolStripDrpDownMenuSyncViews
            //
            this.m_toolStripDrpDownMenuSyncViews.CheckOnClick = true;
            this.m_toolStripDrpDownMenuSyncViews.Name = "m_toolStripDrpDownMenuSyncViews";
            this.m_toolStripDrpDownMenuSyncViews.Size = new System.Drawing.Size(218, 22);
            this.m_toolStripDrpDownMenuSyncViews.Text = "Sync Views";
            this.m_toolStripDrpDownMenuSyncViews.Click += new System.EventHandler(this.m_toolStripDrpDownMenuSyncViews_Click);

            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // m_toolStripLblMacros
            // 
            this.m_toolStripLblMacros.Name = "m_toolStripLblMacros";
            this.m_toolStripLblMacros.Size = new System.Drawing.Size(49, 22);
            this.m_toolStripLblMacros.Text = "Macros:";
            // 
            // m_toolStripDrpDownMacro
            // 
            this.m_toolStripDrpDownMacro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_toolStripDrpDownMacro.Name = "m_toolStripDrpDownMacro";
            this.m_toolStripDrpDownMacro.Size = new System.Drawing.Size(150, 25);
            this.m_toolStripDrpDownMacro.SelectedIndexChanged += new System.EventHandler(this.m_toolStripDrpDownMacro_SelectedIndexChanged);
            // 
            // m_toolStripLabelTopSelect
            // 
            this.m_toolStripLabelTopSelect.Name = "m_toolStripLabelTopSelect";
            this.m_toolStripLabelTopSelect.Size = new System.Drawing.Size(38, 22);
            this.m_toolStripLabelTopSelect.Text = "Select";
            // 
            // m_toolStripTopDrpDownSelect
            // 
            this.m_toolStripTopDrpDownSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_toolStripTopDrpDownSelect.Name = "m_toolStripTopDrpDownSelect";
            this.m_toolStripTopDrpDownSelect.Size = new System.Drawing.Size(121, 25);
            this.m_toolStripTopDrpDownSelect.SelectedIndexChanged += new System.EventHandler(this.m_toolStripTopDrpDownSelect_SelectedIndexChanged);
            // 
            // m_toolStripBottom
            // 
            this.m_toolStripBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_toolStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_toolStripBtnZoomIn,
            this.m_toolStripBtnZoomOut,
            this.m_toolStripBtnExpandSelection,
            this.m_toolStripBtnFind,
            this.m_toolStripLblFilter,
            this.toolStripSeparator2,
            this.m_toolStripTxtBoxTileFilter,
            this.m_toolStripDrpDownMenuPainting,
            this.toolStripSeparator1,
            this.m_toolStripLblMacros,
            this.m_toolStripDrpDownMacro,
            this.m_toolStripLabelTopSelect,
            this.m_toolStripTopDrpDownSelect,
            this.m_toolStripProgressBar});
            this.m_toolStripBottom.Location = new System.Drawing.Point(0, 498);
            this.m_toolStripBottom.Name = "m_toolStripBottom";
            this.m_toolStripBottom.Size = new System.Drawing.Size(1085, 25);
            this.m_toolStripBottom.TabIndex = 3;
            this.m_toolStripBottom.Text = "Tool Bar";
            // 
            // m_toolStripProgressBar
            // 
            this.m_toolStripProgressBar.Name = "m_toolStripProgressBar";
            this.m_toolStripProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.m_toolStripProgressBar.Size = new System.Drawing.Size(150, 22);
            this.m_toolStripProgressBar.Step = 1;
            this.m_toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // m_zoomPictBox
            // 
            this.m_zoomPictBox.AutoScroll = true;
            this.m_zoomPictBox.AutoScrollMinSize = new System.Drawing.Size(1085, 498);
            this.m_zoomPictBox.Controls.Add(this.m_panelSelection);
            this.m_zoomPictBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_zoomPictBox.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.m_zoomPictBox.Image = null;
            this.m_zoomPictBox.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            this.m_zoomPictBox.Location = new System.Drawing.Point(0, 0);
            this.m_zoomPictBox.Name = "m_zoomPictBox";
            this.m_zoomPictBox.Size = new System.Drawing.Size(1085, 498);
            this.m_zoomPictBox.TabIndex = 4;
            this.m_zoomPictBox.Text = "zoomPicBox1";
            this.m_zoomPictBox.Zoom = 1F;
            this.m_zoomPictBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.m_zoomPictBox_MouseDoubleClick);
            this.m_zoomPictBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_zoomPictBox_MouseDown);
            this.m_zoomPictBox.MouseEnter += new System.EventHandler(this.m_zoomPictBox_MouseEnter);
            this.m_zoomPictBox.MouseLeave += new System.EventHandler(this.m_zoomPictBox_MouseLeave);
            this.m_zoomPictBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_zoomPictBox_MouseMove);
            this.m_zoomPictBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.m_zoomPictBox_MouseUp);
            // 
            // m_panelSelection
            // 
            this.m_panelSelection.BackColor = System.Drawing.Color.Transparent;
            this.m_panelSelection.Enabled = false;
            this.m_panelSelection.Location = new System.Drawing.Point(0, 0);
            this.m_panelSelection.Name = "m_panelSelection";
            this.m_panelSelection.Size = new System.Drawing.Size(791, 373);
            this.m_panelSelection.TabIndex = 0;
            this.m_panelSelection.Paint += new System.Windows.Forms.PaintEventHandler(this.m_panelTop_Paint);
            // 
            // FPGAViewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.m_zoomPictBox);
            this.Controls.Add(this.m_toolStripBottom);
            this.Controls.Add(this.m_statusStrip);
            this.Name = "FPGAViewCtrl";
            this.Size = new System.Drawing.Size(1085, 545);
            this.m_statusStrip.ResumeLayout(false);
            this.m_statusStrip.PerformLayout();
            this.m_contextMenu.ResumeLayout(false);
            this.m_toolStripBottom.ResumeLayout(false);
            this.m_toolStripBottom.PerformLayout();
            this.m_zoomPictBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel m_statusStripLabelSelectedTile;
        private System.Windows.Forms.ContextMenuStrip m_contextMenu;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuStore;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuStoreAsPartialAreas;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuStoreAsUserDefinedName;
        private ZoomPicBox m_zoomPictBox;
        private System.Windows.Forms.Panel m_panelSelection;
        private System.Windows.Forms.ToolStripButton m_toolStripBtnZoomIn;
        private System.Windows.Forms.ToolStripButton m_toolStripBtnZoomOut;
        private System.Windows.Forms.ToolStripButton m_toolStripBtnExpandSelection;
        private System.Windows.Forms.CheckBox m_checkBoxExpandSelection;
        private System.Windows.Forms.ToolStripButton m_toolStripBtnFind;
        private System.Windows.Forms.ToolStripLabel m_toolStripLblFilter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox m_toolStripTxtBoxTileFilter;
        private System.Windows.Forms.ToolStripDropDownButton m_toolStripDrpDownMenuPainting;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuPaintingRAM;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuPaintingClockRegion;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuPaintingMacros;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuPaintingPossibleMacroPlacements;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel m_toolStripLblMacros;
        private System.Windows.Forms.ToolStripComboBox m_toolStripDrpDownMacro;
        private System.Windows.Forms.ToolStripLabel m_toolStripLabelTopSelect;
        private System.Windows.Forms.ToolStripComboBox m_toolStripTopDrpDownSelect;
        private System.Windows.Forms.ToolStrip m_toolStripBottom;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuClear;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuTunnel;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuInvertSelection;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuCopyIdentifier;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuPaintingToolTips;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuMuteOutput;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuSyncViews;
       
        private System.Windows.Forms.ToolStripProgressBar m_toolStripProgressBar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem m_contextMenuFullZoom;
        private System.Windows.Forms.ToolStripMenuItem m_toolStripDrpDownMenuPaintingSelection;
    }
}
