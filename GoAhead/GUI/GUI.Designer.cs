using System.Drawing;
namespace GoAhead.GUI
{
    partial class GUI
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
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_statusStrip = new System.Windows.Forms.StatusStrip();
            this.m_tabView = new System.Windows.Forms.TabControl();
            this.m_tabBlock = new System.Windows.Forms.TabPage();
            this.m_fpgaViewBlock = new GoAhead.GUI.FPGAViewCtrl();
            this.m_tabAll = new System.Windows.Forms.TabPage();
            this.m_fpgaViewAll = new GoAhead.GUI.FPGAViewCtrl();
            this.m_menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileOpenRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileOpenXDL = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileOpenVivado = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileDebugVivado = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileOpenDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuAddTimingData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileSaveAsDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileSaveAsMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileSaveAsBlocker = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileSaveRaw = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileSaveBitmap = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuFileResetAll = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileDeviceInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFileBUFGInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuFilePartGenAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEditClearSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEditInvertSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEditExpandSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEditPreferncesExpandLeftRight = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEditPreferncesExpandSpecialTiles = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuEditPrefernces = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsCutOffFromDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsRunScript = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsSingleStepScript = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsSingleCommand = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuCommandsBlock = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuCommandsSaveSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsSaveSelectionExternalFile = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsSaveSelectionFromCurrentDesign = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuCommandsTunnel = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuMacro = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuMacroMana = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.m_menuMacroGnerateXDLCode = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuMacroLaunchFE = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuUCF = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuUCFProhibits = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuUCFLocations = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuUCFArea = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuHelpPrintCommandHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuStrip = new System.Windows.Forms.MenuStrip();
            this.m_menuExtrasWatch = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasPrintCommandNames = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasPrintCommandParameter = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasExportAG = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasRedraw = new System.Windows.Forms.ToolStripMenuItem();
            this.watchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasHookScripts = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuExtrasGOAHEAD_HOME = new System.Windows.Forms.ToolStripMenuItem();
            this.m_menuUserMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.m_toolStripUserButtons = new System.Windows.Forms.ToolStrip();
            this.m_console = new GoAhead.GUI.ConsoleCtrl();
            this.m_tabView.SuspendLayout();
            this.m_tabBlock.SuspendLayout();
            this.m_tabAll.SuspendLayout();
            this.m_menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_statusStrip
            // 
            this.m_statusStrip.Location = new System.Drawing.Point(0, 634);
            this.m_statusStrip.Name = "m_statusStrip";
            this.m_statusStrip.Size = new System.Drawing.Size(1203, 22);
            this.m_statusStrip.TabIndex = 1;
            // 
            // m_tabView
            // 
            this.m_tabView.Controls.Add(this.m_tabBlock);
            this.m_tabView.Controls.Add(this.m_tabAll);
            this.m_tabView.Location = new System.Drawing.Point(0, 52);
            this.m_tabView.Name = "m_tabView";
            this.m_tabView.SelectedIndex = 0;
            this.m_tabView.Size = new System.Drawing.Size(1203, 436);
            this.m_tabView.TabIndex = 4;
            // 
            // m_tabBlock
            // 
            this.m_tabBlock.Controls.Add(this.m_fpgaViewBlock);
            this.m_tabBlock.Location = new System.Drawing.Point(4, 22);
            this.m_tabBlock.Name = "m_tabBlock";
            this.m_tabBlock.Size = new System.Drawing.Size(1195, 410);
            this.m_tabBlock.TabIndex = 3;
            this.m_tabBlock.Text = "Block View";
            this.m_tabBlock.UseVisualStyleBackColor = true;
            // 
            // m_fpgaViewBlock
            // 
            this.m_fpgaViewBlock.BackColor = System.Drawing.SystemColors.ControlDark;
            this.m_fpgaViewBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_fpgaViewBlock.Location = new System.Drawing.Point(0, 0);
            this.m_fpgaViewBlock.Name = "m_fpgaViewBlock";
            this.m_fpgaViewBlock.PointToSelection = false;
            this.m_fpgaViewBlock.RectanglePenWidth = 1F;
            this.m_fpgaViewBlock.RectangleSelect = false;
            this.m_fpgaViewBlock.Size = new System.Drawing.Size(1195, 410);
            this.m_fpgaViewBlock.TabIndex = 1;
            this.m_fpgaViewBlock.Tag = "BlockView";
            this.m_fpgaViewBlock.ZoomSelectOngoing = false;
            this.m_fpgaViewBlock.ZoomSelectStart = false;
            // 
            // m_tabAll
            // 
            this.m_tabAll.Controls.Add(this.m_fpgaViewAll);
            this.m_tabAll.Location = new System.Drawing.Point(4, 22);
            this.m_tabAll.Name = "m_tabAll";
            this.m_tabAll.Size = new System.Drawing.Size(1195, 410);
            this.m_tabAll.TabIndex = 2;
            this.m_tabAll.Text = "All Tiles View";
            this.m_tabAll.UseVisualStyleBackColor = true;
            // 
            // m_fpgaViewAll
            // 
            this.m_fpgaViewAll.BackColor = System.Drawing.SystemColors.ControlDark;
            this.m_fpgaViewAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_fpgaViewAll.Location = new System.Drawing.Point(0, 0);
            this.m_fpgaViewAll.Name = "m_fpgaViewAll";
            this.m_fpgaViewAll.PointToSelection = false;
            this.m_fpgaViewAll.RectanglePenWidth = 1F;
            this.m_fpgaViewAll.RectangleSelect = false;
            this.m_fpgaViewAll.Size = new System.Drawing.Size(1195, 410);
            this.m_fpgaViewAll.TabIndex = 1;
            this.m_fpgaViewAll.Tag = "TilesView";
            this.m_fpgaViewAll.ZoomSelectOngoing = false;
            this.m_fpgaViewAll.ZoomSelectStart = false;
            
            // 
            // m_menuFile
            // 
            this.m_menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuFileOpenRaw,
            this.m_menuFileOpenXDL,
            this.m_menuFileOpenVivado,
            this.m_menuFileDebugVivado,
            this.m_menuFileOpenDesign,
            this.m_menuAddTimingData,
            this.toolStripSeparator2,
            this.m_menuFileSave,
            this.m_menuFileSaveRaw,
            this.m_menuFileSaveBitmap,
            this.toolStripSeparator3,
            this.m_menuFileResetAll,
            this.m_menuFileDeviceInfo,
            this.m_menuFileBUFGInfo,
            this.m_menuFilePartGenAll,
            this.toolStripSeparator8,
            this.m_menuFileExit});
            this.m_menuFile.Name = "m_menuFile";
            this.m_menuFile.Size = new System.Drawing.Size(37, 20);
            this.m_menuFile.Text = "File";
            // 
            // m_menuFileOpenRaw
            // 
            this.m_menuFileOpenRaw.Name = "m_menuFileOpenRaw";
            this.m_menuFileOpenRaw.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.m_menuFileOpenRaw.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileOpenRaw.Text = "Open binFPGA";
            this.m_menuFileOpenRaw.Click += new System.EventHandler(this.m_menuFPGAOpenRaw_Click);
            // 
            // m_menuFileOpenXDL
            // 
            this.m_menuFileOpenXDL.Name = "m_menuFileOpenXDL";
            this.m_menuFileOpenXDL.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.O)));
            this.m_menuFileOpenXDL.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileOpenXDL.Text = "Open XDL Device Description";
            this.m_menuFileOpenXDL.Click += new System.EventHandler(this.m_menuFPGAOpenXDL_Click);
            // 
            // m_menuFileOpenVivado
            // 
            this.m_menuFileOpenVivado.Name = "m_menuFileOpenVivado";
            this.m_menuFileOpenVivado.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.V)));
            this.m_menuFileOpenVivado.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileOpenVivado.Text = "Open Vivado Device Description";
            this.m_menuFileOpenVivado.Click += new System.EventHandler(this.m_menuFileOpenVivado_Click);
            // 
            // m_menuFileDebugVivado
            // 
            this.m_menuFileDebugVivado.Name = "m_menuFileDebugVivado";
            this.m_menuFileDebugVivado.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileDebugVivado.Text = "Debug Vivado Device Description";
            this.m_menuFileDebugVivado.Click += new System.EventHandler(this.m_menuFileDebugVivado_Click);
            // 
            // m_menuFileOpenDesign
            // 
            this.m_menuFileOpenDesign.Name = "m_menuFileOpenDesign";
            this.m_menuFileOpenDesign.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.m_menuFileOpenDesign.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileOpenDesign.Text = "Open Design";
            this.m_menuFileOpenDesign.Click += new System.EventHandler(this.m_menuFileOpenDesign_Click);
            // 
            // m_menuAddTimingData
            // 
            this.m_menuAddTimingData.Name = "m_menuFileOpenDesign";
            //this.m_menuAddTimingData.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
            //| System.Windows.Forms.Keys.D)));
            this.m_menuAddTimingData.Size = new System.Drawing.Size(301, 22);
            this.m_menuAddTimingData.Text = "Add Timing Data";
            this.m_menuAddTimingData.Click += new System.EventHandler(this.m_menuAddTimingData_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(298, 6);
            // 
            // m_menuFileSave
            // 
            this.m_menuFileSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuFileSaveAsDesign,
            this.m_menuFileSaveAsMacro,
            this.m_menuFileSaveAsBlocker});
            this.m_menuFileSave.Name = "m_menuFileSave";
            this.m_menuFileSave.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileSave.Text = "Save";
            // 
            // m_menuFileSaveAsDesign
            // 
            this.m_menuFileSaveAsDesign.Name = "m_menuFileSaveAsDesign";
            this.m_menuFileSaveAsDesign.Size = new System.Drawing.Size(156, 22);
            this.m_menuFileSaveAsDesign.Text = "Save As Design";
            this.m_menuFileSaveAsDesign.Click += new System.EventHandler(this.m_menuFileSaveAsDesign_Click);
            // 
            // m_menuFileSaveAsMacro
            // 
            this.m_menuFileSaveAsMacro.Name = "m_menuFileSaveAsMacro";
            this.m_menuFileSaveAsMacro.Size = new System.Drawing.Size(156, 22);
            this.m_menuFileSaveAsMacro.Text = "Save As Macro";
            this.m_menuFileSaveAsMacro.Click += new System.EventHandler(this.m_menuFileSaveAsMacro_Click);
            // 
            // m_menuFileSaveAsBlocker
            // 
            this.m_menuFileSaveAsBlocker.Name = "m_menuFileSaveAsBlocker";
            this.m_menuFileSaveAsBlocker.Size = new System.Drawing.Size(156, 22);
            this.m_menuFileSaveAsBlocker.Text = "Save As Blocker";
            this.m_menuFileSaveAsBlocker.Click += new System.EventHandler(this.m_menuFileSaveAsBlocker_Click);
            // 
            // m_menuFileSaveRaw
            // 
            this.m_menuFileSaveRaw.Name = "m_menuFileSaveRaw";
            this.m_menuFileSaveRaw.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.m_menuFileSaveRaw.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileSaveRaw.Text = "Save binFPGA";
            this.m_menuFileSaveRaw.Click += new System.EventHandler(this.m_menuFPGASaveRaw_Click);
            // 
            // m_menuFileSaveBitmap
            // 
            this.m_menuFileSaveBitmap.Name = "m_menuFileSaveBitmap";
            this.m_menuFileSaveBitmap.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileSaveBitmap.Text = "Save Block View as PNG";
            this.m_menuFileSaveBitmap.ToolTipText = "Save the Block View as PNG";
            this.m_menuFileSaveBitmap.Click += new System.EventHandler(this.m_menuFileSaveBitmap_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(298, 6);
            // 
            // m_menuFileResetAll
            // 
            this.m_menuFileResetAll.Name = "m_menuFileResetAll";
            this.m_menuFileResetAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.m_menuFileResetAll.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileResetAll.Text = "Reset All";
            this.m_menuFileResetAll.Click += new System.EventHandler(this.m_menuFPGAResetAll_Click);
            // 
            // m_menuFileDeviceInfo
            // 
            this.m_menuFileDeviceInfo.Name = "m_menuFileDeviceInfo";
            this.m_menuFileDeviceInfo.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileDeviceInfo.Text = "Device Info";
            this.m_menuFileDeviceInfo.Click += new System.EventHandler(this.m_menuFPGADeviceInfo_Click);
            // 
            // m_menuFileBUFGInfo
            // 
            this.m_menuFileBUFGInfo.Name = "m_menuFileBUFGInfo";
            this.m_menuFileBUFGInfo.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileBUFGInfo.Text = "BUFG Info";
            this.m_menuFileBUFGInfo.Click += new System.EventHandler(this.bUFGInfoToolStripMenuItem_Click);
            // 
            // m_menuFilePartGenAll
            // 
            this.m_menuFilePartGenAll.Name = "m_menuFilePartGenAll";
            this.m_menuFilePartGenAll.Size = new System.Drawing.Size(301, 22);
            this.m_menuFilePartGenAll.Text = "Generate binFPGAs";
            this.m_menuFilePartGenAll.Click += new System.EventHandler(this.m_menuFilePartGenAll_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(298, 6);
            // 
            // m_menuFileExit
            // 
            this.m_menuFileExit.Name = "m_menuFileExit";
            this.m_menuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.m_menuFileExit.Size = new System.Drawing.Size(301, 22);
            this.m_menuFileExit.Text = "Exit";
            this.m_menuFileExit.Click += new System.EventHandler(this.m_menuFileExit_Click);
            // 
            // m_menuEdit
            // 
            this.m_menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuEditClearSelection,
            this.m_menuEditInvertSelection,
            this.m_menuEditExpandSelection,
            this.m_menuEditPreferncesExpandLeftRight,
            this.m_menuEditPreferncesExpandSpecialTiles,
            this.m_menuEditSelectAll,
            this.toolStripSeparator7,
            this.m_menuEditPrefernces});
            this.m_menuEdit.Name = "m_menuEdit";
            this.m_menuEdit.Size = new System.Drawing.Size(39, 20);
            this.m_menuEdit.Text = "Edit";
            // 
            // m_menuEditClearSelection
            // 
            this.m_menuEditClearSelection.Name = "m_menuEditClearSelection";
            this.m_menuEditClearSelection.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.C)));
            this.m_menuEditClearSelection.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditClearSelection.Text = "Clear Selection";
            this.m_menuEditClearSelection.Click += new System.EventHandler(this.m_menuEditClearSelection_Click);
            // 
            // m_menuEditInvertSelection
            // 
            this.m_menuEditInvertSelection.Name = "m_menuEditInvertSelection";
            this.m_menuEditInvertSelection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.m_menuEditInvertSelection.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditInvertSelection.Text = "Invert Selection";
            this.m_menuEditInvertSelection.Click += new System.EventHandler(this.m_menuInvertSelection_Click);
            // 
            // m_menuEditExpandSelection
            // 
            this.m_menuEditExpandSelection.Name = "m_menuEditExpandSelection";
            this.m_menuEditExpandSelection.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.m_menuEditExpandSelection.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditExpandSelection.Text = "Expand Selection";
            this.m_menuEditExpandSelection.ToolTipText = "Expand the current selection to pairs of CLB/CLEXLM and INT tiles";
            this.m_menuEditExpandSelection.Click += new System.EventHandler(this.m_menuEditExpandSelection_Click);
            // 
            // m_menuEditPreferncesExpandLeftRight
            // 
            this.m_menuEditPreferncesExpandLeftRight.Name = "m_menuEditPreferncesExpandLeftRight";
            this.m_menuEditPreferncesExpandLeftRight.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.E)));
            this.m_menuEditPreferncesExpandLeftRight.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditPreferncesExpandLeftRight.Text = "Expand Selection Left Right";
            this.m_menuEditPreferncesExpandLeftRight.Click += new System.EventHandler(this.m_menuEditPreferncesExpandLeftRight_Click);
            // 
            // m_menuEditPreferncesExpandSpecialTiles
            // 
            this.m_menuEditPreferncesExpandSpecialTiles.Name = "m_menuEditPreferncesExpandSpecialTiles";
            this.m_menuEditPreferncesExpandSpecialTiles.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditPreferncesExpandSpecialTiles.Text = "Expand Selection Special Tiles";
            this.m_menuEditPreferncesExpandSpecialTiles.Click += new System.EventHandler(this.m_menuEditPreferncesExpandSpecialTiles_Click);
            // 
            // m_menuEditSelectAll
            // 
            this.m_menuEditSelectAll.Name = "m_menuEditSelectAll";
            this.m_menuEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.m_menuEditSelectAll.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditSelectAll.Text = "Select All";
            this.m_menuEditSelectAll.Click += new System.EventHandler(this.m_menuEditSelectAll_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(277, 6);
            // 
            // m_menuEditPrefernces
            // 
            this.m_menuEditPrefernces.Name = "m_menuEditPrefernces";
            this.m_menuEditPrefernces.Size = new System.Drawing.Size(280, 22);
            this.m_menuEditPrefernces.Text = "Preferences";
            this.m_menuEditPrefernces.Click += new System.EventHandler(this.m_menuEditPrefernces_Click);
            // 
            // m_menuCommandsCutOffFromDesign
            // 
            this.m_menuCommandsCutOffFromDesign.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuCommandsRunScript,
            this.m_menuCommandsSingleStepScript,
            this.m_menuCommandsSingleCommand,
            this.toolStripSeparator1,
            this.m_menuCommandsBlock,
            this.toolStripSeparator6,
            this.m_menuCommandsSaveSelection,
            this.m_menuCommandsTunnel});
            this.m_menuCommandsCutOffFromDesign.Name = "m_menuCommandsCutOffFromDesign";
            this.m_menuCommandsCutOffFromDesign.Size = new System.Drawing.Size(81, 20);
            this.m_menuCommandsCutOffFromDesign.Text = "Commands";
            // 
            // m_menuCommandsRunScript
            // 
            this.m_menuCommandsRunScript.Name = "m_menuCommandsRunScript";
            this.m_menuCommandsRunScript.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S)));
            this.m_menuCommandsRunScript.Size = new System.Drawing.Size(197, 22);
            this.m_menuCommandsRunScript.Text = "Run Script";
            this.m_menuCommandsRunScript.Click += new System.EventHandler(this.m_menuCommandsRunScript_Click);
            // 
            // m_menuCommandsSingleStepScript
            // 
            this.m_menuCommandsSingleStepScript.Name = "m_menuCommandsSingleStepScript";
            this.m_menuCommandsSingleStepScript.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.m_menuCommandsSingleStepScript.Size = new System.Drawing.Size(197, 22);
            this.m_menuCommandsSingleStepScript.Text = "Script Debugger";
            this.m_menuCommandsSingleStepScript.Click += new System.EventHandler(this.m_menuCommandsSingleStepScript_Click);
            // 
            // m_menuCommandsSingleCommand
            // 
            this.m_menuCommandsSingleCommand.Name = "m_menuCommandsSingleCommand";
            this.m_menuCommandsSingleCommand.Size = new System.Drawing.Size(197, 22);
            this.m_menuCommandsSingleCommand.Text = "Single Command";
            this.m_menuCommandsSingleCommand.Click += new System.EventHandler(this.m_menuCommandsSingleCommand_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // m_menuCommandsBlock
            // 
            this.m_menuCommandsBlock.Name = "m_menuCommandsBlock";
            this.m_menuCommandsBlock.Size = new System.Drawing.Size(197, 22);
            this.m_menuCommandsBlock.Text = "Block Selection";
            this.m_menuCommandsBlock.Click += new System.EventHandler(this.m_menuCommandsBlock_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(194, 6);
            // 
            // m_menuCommandsSaveSelection
            // 
            this.m_menuCommandsSaveSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuCommandsSaveSelectionExternalFile,
            this.m_menuCommandsSaveSelectionFromCurrentDesign});
            this.m_menuCommandsSaveSelection.Name = "m_menuCommandsSaveSelection";
            this.m_menuCommandsSaveSelection.Size = new System.Drawing.Size(197, 22);
            this.m_menuCommandsSaveSelection.Text = "Save Module";
            // 
            // m_menuCommandsSaveSelectionExternalFile
            // 
            this.m_menuCommandsSaveSelectionExternalFile.Name = "m_menuCommandsSaveSelectionExternalFile";
            this.m_menuCommandsSaveSelectionExternalFile.Size = new System.Drawing.Size(183, 22);
            this.m_menuCommandsSaveSelectionExternalFile.Text = "From external Netlist";
            this.m_menuCommandsSaveSelectionExternalFile.Click += new System.EventHandler(this.m_menuCommandsSaveSelectionExternalFile_Click);
            // 
            // m_menuCommandsSaveSelectionFromCurrentDesign
            // 
            this.m_menuCommandsSaveSelectionFromCurrentDesign.Name = "m_menuCommandsSaveSelectionFromCurrentDesign";
            this.m_menuCommandsSaveSelectionFromCurrentDesign.Size = new System.Drawing.Size(183, 22);
            this.m_menuCommandsSaveSelectionFromCurrentDesign.Text = "From current Design";
            this.m_menuCommandsSaveSelectionFromCurrentDesign.Click += new System.EventHandler(this.m_menuCommandsSaveSelectionFromCurrentDesign_Click);
            // 
            // m_menuCommandsTunnel
            // 
            this.m_menuCommandsTunnel.Name = "m_menuCommandsTunnel";
            this.m_menuCommandsTunnel.Size = new System.Drawing.Size(197, 22);
            this.m_menuCommandsTunnel.Text = "Define Tunnel Wires";
            this.m_menuCommandsTunnel.Click += new System.EventHandler(this.m_menuCommandsTunnel_Click);
            // 
            // m_menuMacro
            // 
            this.m_menuMacro.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuMacroMana,
            this.toolStripSeparator9,
            this.m_menuMacroGnerateXDLCode,
            this.m_menuMacroLaunchFE});
            this.m_menuMacro.Name = "m_menuMacro";
            this.m_menuMacro.Size = new System.Drawing.Size(53, 20);
            this.m_menuMacro.Text = "Macro";
            // 
            // m_menuMacroMana
            // 
            this.m_menuMacroMana.Name = "m_menuMacroMana";
            this.m_menuMacroMana.Size = new System.Drawing.Size(253, 22);
            this.m_menuMacroMana.Text = "Macro Manager";
            this.m_menuMacroMana.Click += new System.EventHandler(this.m_menuMacroView_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(250, 6);
            // 
            // m_menuMacroGnerateXDLCode
            // 
            this.m_menuMacroGnerateXDLCode.Name = "m_menuMacroGnerateXDLCode";
            this.m_menuMacroGnerateXDLCode.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.X)));
            this.m_menuMacroGnerateXDLCode.Size = new System.Drawing.Size(253, 22);
            this.m_menuMacroGnerateXDLCode.Text = "Generate XDL Code";
            this.m_menuMacroGnerateXDLCode.Click += new System.EventHandler(this.m_menuMacroGnerateXDLCode_Click);
            // 
            // m_menuMacroLaunchFE
            // 
            this.m_menuMacroLaunchFE.Name = "m_menuMacroLaunchFE";
            this.m_menuMacroLaunchFE.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
            this.m_menuMacroLaunchFE.Size = new System.Drawing.Size(253, 22);
            this.m_menuMacroLaunchFE.Text = "Launch FPGA-Editor";
            this.m_menuMacroLaunchFE.Click += new System.EventHandler(this.m_menuMacroLaunchFE_Click);
            // 
            // m_menuUCF
            // 
            this.m_menuUCF.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuUCFProhibits,
            this.m_menuUCFLocations,
            this.m_menuUCFArea});
            this.m_menuUCF.Name = "m_menuUCF";
            this.m_menuUCF.Size = new System.Drawing.Size(41, 20);
            this.m_menuUCF.Text = "UCF";
            // 
            // m_menuUCFProhibits
            // 
            this.m_menuUCFProhibits.Name = "m_menuUCFProhibits";
            this.m_menuUCFProhibits.Size = new System.Drawing.Size(120, 22);
            this.m_menuUCFProhibits.Text = "Prohibit";
            this.m_menuUCFProhibits.ToolTipText = "Generate Prohibit Constraints for all Primivtes in Selection";
            this.m_menuUCFProhibits.Click += new System.EventHandler(this.m_menuUCFProhibits_Click);
            // 
            // m_menuUCFLocations
            // 
            this.m_menuUCFLocations.Name = "m_menuUCFLocations";
            this.m_menuUCFLocations.Size = new System.Drawing.Size(120, 22);
            this.m_menuUCFLocations.Text = "Location";
            this.m_menuUCFLocations.ToolTipText = "Generate Location Constraints for Places Macros";
            this.m_menuUCFLocations.Click += new System.EventHandler(this.m_menuUCFLocations_Click);
            // 
            // m_menuUCFArea
            // 
            this.m_menuUCFArea.Name = "m_menuUCFArea";
            this.m_menuUCFArea.Size = new System.Drawing.Size(120, 22);
            this.m_menuUCFArea.Text = "Area";
            this.m_menuUCFArea.ToolTipText = "Generate Area Group Constraints";
            this.m_menuUCFArea.Click += new System.EventHandler(this.m_menuUCFArea_Click);
            // 
            // m_menuHelp
            // 
            this.m_menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuHelpAbout,
            this.m_menuHelpPrintCommandHelp});
            this.m_menuHelp.Name = "m_menuHelp";
            this.m_menuHelp.Size = new System.Drawing.Size(44, 20);
            this.m_menuHelp.Text = "Help";
            // 
            // m_menuHelpAbout
            // 
            this.m_menuHelpAbout.Name = "m_menuHelpAbout";
            this.m_menuHelpAbout.Size = new System.Drawing.Size(187, 22);
            this.m_menuHelpAbout.Text = "About GoAhead";
            this.m_menuHelpAbout.Click += new System.EventHandler(this.m_menuHelpAbout_Click);
            // 
            // m_menuHelpPrintCommandHelp
            // 
            this.m_menuHelpPrintCommandHelp.Name = "m_menuHelpPrintCommandHelp";
            this.m_menuHelpPrintCommandHelp.Size = new System.Drawing.Size(187, 22);
            this.m_menuHelpPrintCommandHelp.Text = "Print Command Help";
            this.m_menuHelpPrintCommandHelp.Click += new System.EventHandler(this.m_menuHelpPrintCommandHelp_Click);
            // 
            // m_menuStrip
            // 
            this.m_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuFile,
            this.m_menuEdit,
            this.m_menuCommandsCutOffFromDesign,
            this.m_menuMacro,
            this.m_menuUCF,
            this.m_menuExtrasWatch,
            this.m_menuUserMenu,
            this.m_menuHelp});
            this.m_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.m_menuStrip.Name = "m_menuStrip";
            this.m_menuStrip.Size = new System.Drawing.Size(1203, 24);
            this.m_menuStrip.TabIndex = 2;
            this.m_menuStrip.Text = "menuStrip1";
            // 
            // m_menuExtrasWatch
            // 
            this.m_menuExtrasWatch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_menuExtrasExportAG,
            this.m_menuExtrasPrintCommandNames,
            this.m_menuExtrasPrintCommandParameter,
            this.m_menuExtrasRedraw,
            this.watchToolStripMenuItem,
            this.m_menuExtrasDebug,
            this.m_menuExtrasHookScripts,
            this.m_menuExtrasGOAHEAD_HOME});
            this.m_menuExtrasWatch.Name = "m_menuExtrasWatch";
            this.m_menuExtrasWatch.Size = new System.Drawing.Size(49, 20);
            this.m_menuExtrasWatch.Text = "Extras";
            // 
            // m_menuExtrasPrintCommandNames
            // 
            this.m_menuExtrasPrintCommandNames.Name = "m_menuExtrasPrintCommandNames";
            this.m_menuExtrasPrintCommandNames.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasPrintCommandNames.Text = "Print Command Names";
            this.m_menuExtrasPrintCommandNames.Click += new System.EventHandler(this.m_menuExtrasPrintCommandNames_Click);
            // 
            // m_menuExtrasExportAG
            // 
            this.m_menuExtrasExportAG.Name = "m_menuExtrasExportAG";
            this.m_menuExtrasExportAG.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasExportAG.Text = "Export Architecture";
            this.m_menuExtrasExportAG.Click += new System.EventHandler(this.m_menuExtrasExportAG_Click);
            // 
            // m_menuExtrasPrintCommandParameter
            // 
            this.m_menuExtrasPrintCommandParameter.Name = "m_menuExtrasPrintCommandParameter";
            this.m_menuExtrasPrintCommandParameter.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasPrintCommandParameter.Text = "Print Command Parameter";
            this.m_menuExtrasPrintCommandParameter.Click += new System.EventHandler(this.m_menuPrintCommandParameter_Click);
            // 
            // m_menuExtrasRedraw
            // 
            this.m_menuExtrasRedraw.Name = "m_menuExtrasRedraw";
            this.m_menuExtrasRedraw.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasRedraw.Text = "Redraw";
            this.m_menuExtrasRedraw.Click += new System.EventHandler(this.m_menuExtrasRedraw_Click);
            // 
            // watchToolStripMenuItem
            // 
            this.watchToolStripMenuItem.Name = "watchToolStripMenuItem";
            this.watchToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.watchToolStripMenuItem.Text = "Variable Watch Window";
            this.watchToolStripMenuItem.ToolTipText = "Open a watch window for all variables";
            this.watchToolStripMenuItem.Click += new System.EventHandler(this.watchToolStripMenuItem_Click);
            // 
            // m_menuExtrasDebug
            // 
            this.m_menuExtrasDebug.Name = "m_menuExtrasDebug";
            this.m_menuExtrasDebug.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasDebug.Text = "Debug";
            // 
            // m_menuExtrasHookScripts
            // 
            this.m_menuExtrasHookScripts.Name = "m_menuExtrasHookScripts";
            this.m_menuExtrasHookScripts.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasHookScripts.Text = " Hook Scripts";
            this.m_menuExtrasHookScripts.Click += new System.EventHandler(this.m_menuExtrasHookScripts_Click);
            // 
            // m_menuExtrasGOAHEAD_HOME
            // 
            this.m_menuExtrasGOAHEAD_HOME.Name = "m_menuExtrasGOAHEAD_HOME";
            this.m_menuExtrasGOAHEAD_HOME.Size = new System.Drawing.Size(216, 22);
            this.m_menuExtrasGOAHEAD_HOME.Text = " GOAHEAD_HOME";
            this.m_menuExtrasGOAHEAD_HOME.Click += new System.EventHandler(this.m_menuExtrasGOAHEAD_HOME_Click);
            // 
            // m_menuUserMenu
            // 
            this.m_menuUserMenu.Name = "m_menuUserMenu";
            this.m_menuUserMenu.Size = new System.Drawing.Size(76, 20);
            this.m_menuUserMenu.Text = "User Menu";
            this.m_menuUserMenu.Visible = false;
            // 
            // m_toolStripUserButtons
            // 
            this.m_toolStripUserButtons.Location = new System.Drawing.Point(0, 24);
            this.m_toolStripUserButtons.Name = "m_toolStripUserButtons";
            this.m_toolStripUserButtons.Size = new System.Drawing.Size(1203, 25);
            this.m_toolStripUserButtons.TabIndex = 7;
            this.m_toolStripUserButtons.Text = "User defiined Toolbar";
            this.m_toolStripUserButtons.Visible = false;
            // 
            // m_console
            // 
            this.m_console.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_console.Location = new System.Drawing.Point(0, 488);
            this.m_console.Name = "m_console";
            this.m_console.Size = new System.Drawing.Size(1203, 146);
            this.m_console.TabIndex = 5;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 656);
            this.Controls.Add(this.m_console);
            this.Controls.Add(this.m_toolStripUserButtons);
            this.Controls.Add(this.m_tabView);
            this.Controls.Add(this.m_statusStrip);
            this.Controls.Add(this.m_menuStrip);
            this.MainMenuStrip = this.m_menuStrip;
            this.Name = "GUI";
            this.Text = "GoAhead";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GUI_FormClosed);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GUI_Paint);
            this.Resize += new System.EventHandler(this.GUI_Resize);
            this.m_tabView.ResumeLayout(false);
            this.m_tabBlock.ResumeLayout(false);
            this.m_tabAll.ResumeLayout(false);
            this.m_menuStrip.ResumeLayout(false);
            this.m_menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            
            //Sync views.
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GUI_OnMouseMove);
            this.SharedZoom = 0.0f;
            this.SharedAutoScroll = m_fpgaViewBlock.ZoomPictureBox.AutoScrollPosition;

        }

        #endregion

        private System.Windows.Forms.StatusStrip m_statusStrip;
        private System.Windows.Forms.TabControl m_tabView;
        private System.Windows.Forms.TabPage m_tabAll;
        private System.Windows.Forms.TabPage m_tabBlock;
        private FPGAViewCtrl m_fpgaViewBlock;
        private ConsoleCtrl m_console;
        private System.Windows.Forms.ToolStripMenuItem m_menuFile;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileOpenRaw;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileOpenXDL;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileSaveRaw;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileSaveBitmap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileResetAll;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileDeviceInfo;
        private System.Windows.Forms.ToolStripMenuItem m_menuFilePartGenAll;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileBUFGInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem m_menuEdit;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditInvertSelection;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditClearSelection;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditPrefernces;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsCutOffFromDesign;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsRunScript;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsSingleStepScript;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsSingleCommand;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsBlock;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsTunnel;
        private System.Windows.Forms.ToolStripMenuItem m_menuMacro;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem m_menuMacroGnerateXDLCode;
        private System.Windows.Forms.ToolStripMenuItem m_menuMacroLaunchFE;
        private System.Windows.Forms.ToolStripMenuItem m_menuUCF;
        private System.Windows.Forms.ToolStripMenuItem m_menuUCFProhibits;
        private System.Windows.Forms.ToolStripMenuItem m_menuUCFLocations;
        private System.Windows.Forms.ToolStripMenuItem m_menuUCFArea;
        private System.Windows.Forms.ToolStripMenuItem m_menuHelp;
        private System.Windows.Forms.ToolStripMenuItem m_menuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem m_menuHelpPrintCommandHelp;
        private System.Windows.Forms.MenuStrip m_menuStrip;
        private FPGAViewCtrl m_fpgaViewAll;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasWatch;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasExportAG;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasPrintCommandNames;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasPrintCommandParameter;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasRedraw;
        private System.Windows.Forms.ToolStripMenuItem m_menuMacroMana;
        private System.Windows.Forms.ToolStrip m_toolStripUserButtons;
        private System.Windows.Forms.ToolStripMenuItem m_menuUserMenu;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileOpenDesign;
        private System.Windows.Forms.ToolStripMenuItem m_menuAddTimingData;
        private System.Windows.Forms.ToolStripMenuItem watchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditPreferncesExpandLeftRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditExpandSelection;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileSave;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileSaveAsDesign;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileSaveAsMacro;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileSaveAsBlocker;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsSaveSelection;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsSaveSelectionExternalFile;
        private System.Windows.Forms.ToolStripMenuItem m_menuCommandsSaveSelectionFromCurrentDesign;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasDebug;
        private System.Windows.Forms.ToolStripMenuItem m_menuEditPreferncesExpandSpecialTiles;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasGOAHEAD_HOME;
        private System.Windows.Forms.ToolStripMenuItem m_menuExtrasHookScripts;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileOpenVivado;
        private System.Windows.Forms.ToolStripMenuItem m_menuFileDebugVivado;

        private float m_SharedZoom;
        private Point m_SharedAutoScroll;
    }
}