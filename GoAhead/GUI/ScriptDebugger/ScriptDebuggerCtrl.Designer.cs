namespace GoAhead.GUI.ScriptDebugger
{
    partial class ScriptDebuggerCtrl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptDebuggerCtrl));
            this.m_lstViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_setBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_deleteBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_removeAllBreakpointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_grpBoxControls = new System.Windows.Forms.GroupBox();
            this.m_lblCmd = new System.Windows.Forms.Label();
            this.m_btnReload = new System.Windows.Forms.Button();
            this.m_btnRun = new System.Windows.Forms.Button();
            this.m_btnClear = new System.Windows.Forms.Button();
            this.m_btnBrowseForScript = new System.Windows.Forms.Button();
            this.m_btnStop = new System.Windows.Forms.Button();
            this.m_lblScriptFiled = new System.Windows.Forms.Label();
            this.m_btnExecuteNextCmd = new System.Windows.Forms.Button();
            this.m_txtCmds = new GoAhead.GUI.SyncTextBox();
            this.m_txtLineNumber = new GoAhead.GUI.SyncTextBox();
            this.m_lstViewContextMenu.SuspendLayout();
            this.m_grpBoxControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lstViewContextMenu
            // 
            this.m_lstViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_setBreakpointToolStripMenuItem,
            this.m_deleteBreakpointToolStripMenuItem,
            this.m_removeAllBreakpointsToolStripMenuItem});
            this.m_lstViewContextMenu.Name = "m_lstViewContext";
            this.m_lstViewContextMenu.Size = new System.Drawing.Size(186, 70);
            // 
            // m_setBreakpointToolStripMenuItem
            // 
            this.m_setBreakpointToolStripMenuItem.Name = "m_setBreakpointToolStripMenuItem";
            this.m_setBreakpointToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.m_setBreakpointToolStripMenuItem.Text = "Set Breakpoint";
            this.m_setBreakpointToolStripMenuItem.Click += new System.EventHandler(this.m_setBreakpointToolStripMenuItem_Click);
            // 
            // m_deleteBreakpointToolStripMenuItem
            // 
            this.m_deleteBreakpointToolStripMenuItem.Name = "m_deleteBreakpointToolStripMenuItem";
            this.m_deleteBreakpointToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.m_deleteBreakpointToolStripMenuItem.Text = "Delete Breakpoint";
            this.m_deleteBreakpointToolStripMenuItem.Click += new System.EventHandler(this.m_deleteBreakpointToolStripMenuItem_Click);
            // 
            // m_removeAllBreakpointsToolStripMenuItem
            // 
            this.m_removeAllBreakpointsToolStripMenuItem.Name = "m_removeAllBreakpointsToolStripMenuItem";
            this.m_removeAllBreakpointsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.m_removeAllBreakpointsToolStripMenuItem.Text = "Remove all Breakpoints";
            this.m_removeAllBreakpointsToolStripMenuItem.Click += new System.EventHandler(this.m_removeAllBreakpointsToolStripMenuItem_Click);
            // 
            // m_grpBoxControls
            // 
            this.m_grpBoxControls.Controls.Add(this.m_lblCmd);
            this.m_grpBoxControls.Controls.Add(this.m_btnReload);
            this.m_grpBoxControls.Controls.Add(this.m_btnRun);
            this.m_grpBoxControls.Controls.Add(this.m_btnClear);
            this.m_grpBoxControls.Controls.Add(this.m_btnBrowseForScript);
            this.m_grpBoxControls.Controls.Add(this.m_btnStop);
            this.m_grpBoxControls.Controls.Add(this.m_lblScriptFiled);
            this.m_grpBoxControls.Controls.Add(this.m_btnExecuteNextCmd);
            this.m_grpBoxControls.Location = new System.Drawing.Point(8, 402);
            this.m_grpBoxControls.Name = "m_grpBoxControls";
            this.m_grpBoxControls.Size = new System.Drawing.Size(440, 107);
            this.m_grpBoxControls.TabIndex = 14;
            this.m_grpBoxControls.TabStop = false;
            this.m_grpBoxControls.Text = "Control";
            // 
            // m_lblCmd
            // 
            this.m_lblCmd.AutoSize = true;
            this.m_lblCmd.Location = new System.Drawing.Point(10, 82);
            this.m_lblCmd.Name = "m_lblCmd";
            this.m_lblCmd.Size = new System.Drawing.Size(113, 13);
            this.m_lblCmd.TabIndex = 10;
            this.m_lblCmd.Text = "No command selected";
            // 
            // m_btnReload
            // 
            this.m_btnReload.Location = new System.Drawing.Point(146, 46);
            this.m_btnReload.Name = "m_btnReload";
            this.m_btnReload.Size = new System.Drawing.Size(62, 23);
            this.m_btnReload.TabIndex = 9;
            this.m_btnReload.Text = "Reload";
            this.m_btnReload.UseVisualStyleBackColor = true;
            this.m_btnReload.Click += new System.EventHandler(this.m_btnReload_Click);
            // 
            // m_btnRun
            // 
            this.m_btnRun.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("m_btnRun.BackgroundImage")));
            this.m_btnRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.m_btnRun.Location = new System.Drawing.Point(240, 46);
            this.m_btnRun.Name = "m_btnRun";
            this.m_btnRun.Size = new System.Drawing.Size(39, 23);
            this.m_btnRun.TabIndex = 8;
            this.m_btnRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.m_btnRun.UseVisualStyleBackColor = true;
            this.m_btnRun.Click += new System.EventHandler(this.m_btnRun_Click);
            // 
            // m_btnClear
            // 
            this.m_btnClear.Location = new System.Drawing.Point(78, 46);
            this.m_btnClear.Name = "m_btnClear";
            this.m_btnClear.Size = new System.Drawing.Size(62, 23);
            this.m_btnClear.TabIndex = 7;
            this.m_btnClear.Text = "Clear";
            this.m_btnClear.UseVisualStyleBackColor = true;
            this.m_btnClear.Click += new System.EventHandler(this.m_btnClear_Click);
            // 
            // m_btnBrowseForScript
            // 
            this.m_btnBrowseForScript.Location = new System.Drawing.Point(10, 46);
            this.m_btnBrowseForScript.Name = "m_btnBrowseForScript";
            this.m_btnBrowseForScript.Size = new System.Drawing.Size(62, 23);
            this.m_btnBrowseForScript.TabIndex = 2;
            this.m_btnBrowseForScript.Text = "Browse";
            this.m_btnBrowseForScript.UseVisualStyleBackColor = true;
            this.m_btnBrowseForScript.Click += new System.EventHandler(this.m_btnBrowseForScript_Click_1);
            // 
            // m_btnStop
            // 
            this.m_btnStop.Image = ((System.Drawing.Image)(resources.GetObject("m_btnStop.Image")));
            this.m_btnStop.Location = new System.Drawing.Point(331, 46);
            this.m_btnStop.Name = "m_btnStop";
            this.m_btnStop.Size = new System.Drawing.Size(39, 23);
            this.m_btnStop.TabIndex = 5;
            this.m_btnStop.UseVisualStyleBackColor = true;
            this.m_btnStop.Click += new System.EventHandler(this.m_btnStop_Click);
            // 
            // m_lblScriptFiled
            // 
            this.m_lblScriptFiled.AutoSize = true;
            this.m_lblScriptFiled.Location = new System.Drawing.Point(10, 21);
            this.m_lblScriptFiled.Name = "m_lblScriptFiled";
            this.m_lblScriptFiled.Size = new System.Drawing.Size(105, 13);
            this.m_lblScriptFiled.TabIndex = 3;
            this.m_lblScriptFiled.Text = "No Script File loaded";
            // 
            // m_btnExecuteNextCmd
            // 
            this.m_btnExecuteNextCmd.Image = ((System.Drawing.Image)(resources.GetObject("m_btnExecuteNextCmd.Image")));
            this.m_btnExecuteNextCmd.Location = new System.Drawing.Point(285, 46);
            this.m_btnExecuteNextCmd.Name = "m_btnExecuteNextCmd";
            this.m_btnExecuteNextCmd.Size = new System.Drawing.Size(39, 23);
            this.m_btnExecuteNextCmd.TabIndex = 0;
            this.m_btnExecuteNextCmd.UseVisualStyleBackColor = true;
            this.m_btnExecuteNextCmd.Click += new System.EventHandler(this.m_btnExecuteNextCmd_Click);
            // 
            // m_txtCmds
            // 
            this.m_txtCmds.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.m_txtCmds.Buddy = this.m_txtLineNumber;
            this.m_txtCmds.Location = new System.Drawing.Point(90, 16);
            this.m_txtCmds.Name = "m_txtCmds";
            this.m_txtCmds.ReadOnly = true;
            this.m_txtCmds.Size = new System.Drawing.Size(328, 307);
            this.m_txtCmds.TabIndex = 17;
            this.m_txtCmds.Text = "";
            this.m_txtCmds.MouseClick += new System.Windows.Forms.MouseEventHandler(this.m_txtCmds_MouseClick);
            this.m_txtCmds.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtCmds_MouseDown);
            this.m_txtCmds.MouseEnter += new System.EventHandler(this.m_txtCmds_MouseEnter);
            this.m_txtCmds.MouseLeave += new System.EventHandler(this.m_txtCmds_MouseLeave);
            this.m_txtCmds.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_txtCmds_MouseMove);
            // 
            // m_txtLineNumber
            // 
            this.m_txtLineNumber.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.m_txtLineNumber.Buddy = this.m_txtCmds;
            this.m_txtLineNumber.Location = new System.Drawing.Point(18, 16);
            this.m_txtLineNumber.Name = "m_txtLineNumber";
            this.m_txtLineNumber.ReadOnly = true;
            this.m_txtLineNumber.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.m_txtLineNumber.Size = new System.Drawing.Size(66, 307);
            this.m_txtLineNumber.TabIndex = 18;
            this.m_txtLineNumber.Text = "";
            // 
            // ScriptDebuggerCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.m_txtCmds);
            this.Controls.Add(this.m_txtLineNumber);
            this.Controls.Add(this.m_grpBoxControls);
            this.Name = "ScriptDebuggerCtrl";
            this.Size = new System.Drawing.Size(460, 519);
            this.Load += new System.EventHandler(this.ScriptDebugger_Load);
            this.Resize += new System.EventHandler(this.ScriptDebuggerCtrl_Resize);
            this.m_lstViewContextMenu.ResumeLayout(false);
            this.m_grpBoxControls.ResumeLayout(false);
            this.m_grpBoxControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip m_lstViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem m_setBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_deleteBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_removeAllBreakpointsToolStripMenuItem;
        private System.Windows.Forms.GroupBox m_grpBoxControls;
        private System.Windows.Forms.Button m_btnRun;
        private System.Windows.Forms.Button m_btnClear;
        private System.Windows.Forms.Button m_btnBrowseForScript;
        private System.Windows.Forms.Button m_btnStop;
        private System.Windows.Forms.Label m_lblScriptFiled;
        private System.Windows.Forms.Button m_btnExecuteNextCmd;
        private SyncTextBox m_txtCmds;
        private SyncTextBox m_txtLineNumber;
        private System.Windows.Forms.Button m_btnReload;
        private System.Windows.Forms.Label m_lblCmd;

    }
}