namespace GoAhead.GUI
{
    partial class PreferencesForm
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
            this.m_tabCtrl = new System.Windows.Forms.TabControl();
            this.m_tabColors = new System.Windows.Forms.TabPage();
            this.m_colorSettings = new GoAhead.GUI.Help.ColorSettingsCtrl();
            this.m_tabMisc = new System.Windows.Forms.TabPage();
            this.m_lblrectangleWidth = new System.Windows.Forms.Label();
            this.m_numDropDownRectangleWidth = new System.Windows.Forms.NumericUpDown();
            this.m_lblConsoleGUIShare = new System.Windows.Forms.Label();
            this.m_numDropDownConsoleGUIShare = new System.Windows.Forms.NumericUpDown();
            this.m_chkPrintSelectionResourceInfo = new System.Windows.Forms.CheckBox();
            this.m_chkShowToolTips = new System.Windows.Forms.CheckBox();
            this.m_chkPrintWrappedCommands = new System.Windows.Forms.CheckBox();
            this.m_chkExpandSelection = new System.Windows.Forms.CheckBox();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_btnCancel = new System.Windows.Forms.Button();
            this.m_btnApply = new System.Windows.Forms.Button();
            this.m_tabCtrl.SuspendLayout();
            this.m_tabColors.SuspendLayout();
            this.m_tabMisc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDropDownRectangleWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDropDownConsoleGUIShare)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tabCtrl
            // 
            this.m_tabCtrl.Controls.Add(this.m_tabColors);
            this.m_tabCtrl.Controls.Add(this.m_tabMisc);
            this.m_tabCtrl.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_tabCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_tabCtrl.Name = "m_tabCtrl";
            this.m_tabCtrl.SelectedIndex = 0;
            this.m_tabCtrl.Size = new System.Drawing.Size(388, 305);
            this.m_tabCtrl.TabIndex = 0;
            // 
            // m_tabColors
            // 
            this.m_tabColors.Controls.Add(this.m_colorSettings);
            this.m_tabColors.Location = new System.Drawing.Point(4, 22);
            this.m_tabColors.Name = "m_tabColors";
            this.m_tabColors.Size = new System.Drawing.Size(380, 279);
            this.m_tabColors.TabIndex = 1;
            this.m_tabColors.Text = "Colors";
            this.m_tabColors.UseVisualStyleBackColor = true;
            // 
            // m_colorSettings
            // 
            this.m_colorSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_colorSettings.Location = new System.Drawing.Point(0, 0);
            this.m_colorSettings.Name = "m_colorSettings";
            this.m_colorSettings.Size = new System.Drawing.Size(380, 279);
            this.m_colorSettings.TabIndex = 1;
            // 
            // m_tabMisc
            // 
            this.m_tabMisc.Controls.Add(this.m_lblrectangleWidth);
            this.m_tabMisc.Controls.Add(this.m_numDropDownRectangleWidth);
            this.m_tabMisc.Controls.Add(this.m_lblConsoleGUIShare);
            this.m_tabMisc.Controls.Add(this.m_numDropDownConsoleGUIShare);
            this.m_tabMisc.Controls.Add(this.m_chkPrintSelectionResourceInfo);
            this.m_tabMisc.Controls.Add(this.m_chkShowToolTips);
            this.m_tabMisc.Controls.Add(this.m_chkPrintWrappedCommands);
            this.m_tabMisc.Controls.Add(this.m_chkExpandSelection);
            this.m_tabMisc.Location = new System.Drawing.Point(4, 22);
            this.m_tabMisc.Name = "m_tabMisc";
            this.m_tabMisc.Size = new System.Drawing.Size(380, 279);
            this.m_tabMisc.TabIndex = 2;
            this.m_tabMisc.Text = "Misc";
            this.m_tabMisc.UseVisualStyleBackColor = true;
            // 
            // m_lblrectangleWidth
            // 
            this.m_lblrectangleWidth.AutoSize = true;
            this.m_lblrectangleWidth.Location = new System.Drawing.Point(18, 173);
            this.m_lblrectangleWidth.Name = "m_lblrectangleWidth";
            this.m_lblrectangleWidth.Size = new System.Drawing.Size(163, 13);
            this.m_lblrectangleWidth.TabIndex = 7;
            this.m_lblrectangleWidth.Text = "Selection Rectangle Line Weight";
            // 
            // m_numDropDownRectangleWidth
            // 
            this.m_numDropDownRectangleWidth.Location = new System.Drawing.Point(21, 192);
            this.m_numDropDownRectangleWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.m_numDropDownRectangleWidth.Name = "m_numDropDownRectangleWidth";
            this.m_numDropDownRectangleWidth.Size = new System.Drawing.Size(120, 20);
            this.m_numDropDownRectangleWidth.TabIndex = 6;
            // 
            // m_lblConsoleGUIShare
            // 
            this.m_lblConsoleGUIShare.AutoSize = true;
            this.m_lblConsoleGUIShare.Location = new System.Drawing.Point(18, 118);
            this.m_lblConsoleGUIShare.Name = "m_lblConsoleGUIShare";
            this.m_lblConsoleGUIShare.Size = new System.Drawing.Size(98, 13);
            this.m_lblConsoleGUIShare.TabIndex = 5;
            this.m_lblConsoleGUIShare.Text = "Console GUI Share";
            // 
            // m_numDropDownConsoleGUIShare
            // 
            this.m_numDropDownConsoleGUIShare.Location = new System.Drawing.Point(21, 137);
            this.m_numDropDownConsoleGUIShare.Name = "m_numDropDownConsoleGUIShare";
            this.m_numDropDownConsoleGUIShare.Size = new System.Drawing.Size(120, 20);
            this.m_numDropDownConsoleGUIShare.TabIndex = 4;
            this.m_numDropDownConsoleGUIShare.ValueChanged += new System.EventHandler(this.m_numDropDownConsoleGUIShare_ValueChanged);
            // 
            // m_chkPrintSelectionResourceInfo
            // 
            this.m_chkPrintSelectionResourceInfo.AutoSize = true;
            this.m_chkPrintSelectionResourceInfo.Checked = true;
            this.m_chkPrintSelectionResourceInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkPrintSelectionResourceInfo.Location = new System.Drawing.Point(21, 89);
            this.m_chkPrintSelectionResourceInfo.Name = "m_chkPrintSelectionResourceInfo";
            this.m_chkPrintSelectionResourceInfo.Size = new System.Drawing.Size(164, 17);
            this.m_chkPrintSelectionResourceInfo.TabIndex = 3;
            this.m_chkPrintSelectionResourceInfo.Text = "Print Selection Resource Info";
            this.m_chkPrintSelectionResourceInfo.UseVisualStyleBackColor = true;
            // 
            // m_chkShowToolTips
            // 
            this.m_chkShowToolTips.AutoSize = true;
            this.m_chkShowToolTips.Location = new System.Drawing.Point(21, 66);
            this.m_chkShowToolTips.Name = "m_chkShowToolTips";
            this.m_chkShowToolTips.Size = new System.Drawing.Size(100, 17);
            this.m_chkShowToolTips.TabIndex = 2;
            this.m_chkShowToolTips.Text = "Show Tool Tips";
            this.m_chkShowToolTips.UseVisualStyleBackColor = true;
            // 
            // m_chkPrintWrappedCommands
            // 
            this.m_chkPrintWrappedCommands.AutoSize = true;
            this.m_chkPrintWrappedCommands.Location = new System.Drawing.Point(21, 43);
            this.m_chkPrintWrappedCommands.Name = "m_chkPrintWrappedCommands";
            this.m_chkPrintWrappedCommands.Size = new System.Drawing.Size(343, 17);
            this.m_chkPrintWrappedCommands.TabIndex = 1;
            this.m_chkPrintWrappedCommands.Text = "Print wrapped Commands as Comments to CommandTrace (Debug)";
            this.m_chkPrintWrappedCommands.UseVisualStyleBackColor = true;
            // 
            // m_chkExpandSelection
            // 
            this.m_chkExpandSelection.AutoSize = true;
            this.m_chkExpandSelection.Checked = true;
            this.m_chkExpandSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkExpandSelection.Location = new System.Drawing.Point(21, 20);
            this.m_chkExpandSelection.Name = "m_chkExpandSelection";
            this.m_chkExpandSelection.Size = new System.Drawing.Size(230, 17);
            this.m_chkExpandSelection.TabIndex = 0;
            this.m_chkExpandSelection.Text = "Run ExpandSelection after a user selection";
            this.m_chkExpandSelection.UseVisualStyleBackColor = true;
            // 
            // m_btnOK
            // 
            this.m_btnOK.Location = new System.Drawing.Point(125, 313);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(75, 23);
            this.m_btnOK.TabIndex = 1;
            this.m_btnOK.Text = "&OK";
            this.m_btnOK.UseVisualStyleBackColor = true;
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // m_btnCancel
            // 
            this.m_btnCancel.Location = new System.Drawing.Point(212, 313);
            this.m_btnCancel.Name = "m_btnCancel";
            this.m_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.m_btnCancel.TabIndex = 2;
            this.m_btnCancel.Text = "&Cancel";
            this.m_btnCancel.UseVisualStyleBackColor = true;
            this.m_btnCancel.Click += new System.EventHandler(this.m_btnCancel_Click);
            // 
            // m_btnApply
            // 
            this.m_btnApply.Location = new System.Drawing.Point(302, 313);
            this.m_btnApply.Name = "m_btnApply";
            this.m_btnApply.Size = new System.Drawing.Size(75, 23);
            this.m_btnApply.TabIndex = 3;
            this.m_btnApply.Text = "&Apply";
            this.m_btnApply.UseVisualStyleBackColor = true;
            this.m_btnApply.Click += new System.EventHandler(this.m_btnApply_Click);
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 346);
            this.Controls.Add(this.m_btnApply);
            this.Controls.Add(this.m_btnCancel);
            this.Controls.Add(this.m_btnOK);
            this.Controls.Add(this.m_tabCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.Text = "Preferences";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PreferencesForm_FormClosed);
            this.m_tabCtrl.ResumeLayout(false);
            this.m_tabColors.ResumeLayout(false);
            this.m_tabMisc.ResumeLayout(false);
            this.m_tabMisc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDropDownRectangleWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDropDownConsoleGUIShare)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl m_tabCtrl;
        private System.Windows.Forms.Button m_btnOK;
        private System.Windows.Forms.Button m_btnCancel;
        private System.Windows.Forms.Button m_btnApply;
        private System.Windows.Forms.TabPage m_tabColors;
        private Help.ColorSettingsCtrl m_colorSettings;
        private System.Windows.Forms.TabPage m_tabMisc;
        private System.Windows.Forms.CheckBox m_chkPrintWrappedCommands;
        private System.Windows.Forms.CheckBox m_chkExpandSelection;
        private System.Windows.Forms.CheckBox m_chkShowToolTips;
        private System.Windows.Forms.CheckBox m_chkPrintSelectionResourceInfo;
        private System.Windows.Forms.NumericUpDown m_numDropDownConsoleGUIShare;
        private System.Windows.Forms.Label m_lblConsoleGUIShare;
        private System.Windows.Forms.Label m_lblrectangleWidth;
        private System.Windows.Forms.NumericUpDown m_numDropDownRectangleWidth;
    }
}