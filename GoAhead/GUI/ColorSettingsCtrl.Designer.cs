namespace GoAhead.GUI.Help
{
    partial class ColorSettingsCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_lxtBoxRegexps = new System.Windows.Forms.ListBox();
            this.m_btnSelectColor = new System.Windows.Forms.Button();
            this.m_tabCtrl = new System.Windows.Forms.TabControl();
            this.m_tiles = new System.Windows.Forms.TabPage();
            this.m_increments = new System.Windows.Forms.TabPage();
            this.m_lblBlock = new System.Windows.Forms.Label();
            this.m_lblUserSel = new System.Windows.Forms.Label();
            this.m_lblSel = new System.Windows.Forms.Label();
            this.m_btnBlockedPortsIncr = new System.Windows.Forms.Button();
            this.m_lblBlockedPorts = new System.Windows.Forms.Label();
            this.m_btnUserSelectionIncr = new System.Windows.Forms.Button();
            this.m_lblUserSelection = new System.Windows.Forms.Label();
            this.m_btnSelectionIncr = new System.Windows.Forms.Button();
            this.m_lblSelection = new System.Windows.Forms.Label();
            this.m_tabCtrl.SuspendLayout();
            this.m_tiles.SuspendLayout();
            this.m_increments.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_lxtBoxRegexps
            // 
            this.m_lxtBoxRegexps.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_lxtBoxRegexps.FormattingEnabled = true;
            this.m_lxtBoxRegexps.Location = new System.Drawing.Point(3, 3);
            this.m_lxtBoxRegexps.Name = "m_lxtBoxRegexps";
            this.m_lxtBoxRegexps.Size = new System.Drawing.Size(133, 256);
            this.m_lxtBoxRegexps.TabIndex = 0;
            // 
            // m_btnSelectColor
            // 
            this.m_btnSelectColor.Location = new System.Drawing.Point(142, 6);
            this.m_btnSelectColor.Name = "m_btnSelectColor";
            this.m_btnSelectColor.Size = new System.Drawing.Size(75, 23);
            this.m_btnSelectColor.TabIndex = 1;
            this.m_btnSelectColor.Text = "Select Color";
            this.m_btnSelectColor.UseVisualStyleBackColor = true;
            this.m_btnSelectColor.Click += new System.EventHandler(this.m_btnSelectColor_Click);
            // 
            // m_tabCtrl
            // 
            this.m_tabCtrl.Controls.Add(this.m_tiles);
            this.m_tabCtrl.Controls.Add(this.m_increments);
            this.m_tabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_tabCtrl.Name = "m_tabCtrl";
            this.m_tabCtrl.SelectedIndex = 0;
            this.m_tabCtrl.Size = new System.Drawing.Size(412, 288);
            this.m_tabCtrl.TabIndex = 2;
            // 
            // m_tiles
            // 
            this.m_tiles.Controls.Add(this.m_lxtBoxRegexps);
            this.m_tiles.Controls.Add(this.m_btnSelectColor);
            this.m_tiles.Location = new System.Drawing.Point(4, 22);
            this.m_tiles.Name = "m_tiles";
            this.m_tiles.Padding = new System.Windows.Forms.Padding(3);
            this.m_tiles.Size = new System.Drawing.Size(404, 262);
            this.m_tiles.TabIndex = 0;
            this.m_tiles.Text = "Tile Colors";
            this.m_tiles.UseVisualStyleBackColor = true;
            // 
            // m_increments
            // 
            this.m_increments.Controls.Add(this.m_btnBlockedPortsIncr);
            this.m_increments.Controls.Add(this.m_lblBlockedPorts);
            this.m_increments.Controls.Add(this.m_btnUserSelectionIncr);
            this.m_increments.Controls.Add(this.m_lblUserSelection);
            this.m_increments.Controls.Add(this.m_btnSelectionIncr);
            this.m_increments.Controls.Add(this.m_lblSelection);
            this.m_increments.Controls.Add(this.m_lblBlock);
            this.m_increments.Controls.Add(this.m_lblUserSel);
            this.m_increments.Controls.Add(this.m_lblSel);
            this.m_increments.Location = new System.Drawing.Point(4, 22);
            this.m_increments.Name = "m_increments";
            this.m_increments.Padding = new System.Windows.Forms.Padding(3);
            this.m_increments.Size = new System.Drawing.Size(404, 262);
            this.m_increments.TabIndex = 1;
            this.m_increments.Text = "Increments for Selections";
            this.m_increments.UseVisualStyleBackColor = true;
            // 
            // m_lblBlock
            // 
            this.m_lblBlock.AutoSize = true;
            this.m_lblBlock.Location = new System.Drawing.Point(29, 146);
            this.m_lblBlock.Name = "m_lblBlock";
            this.m_lblBlock.Size = new System.Drawing.Size(123, 13);
            this.m_lblBlock.TabIndex = 8;
            this.m_lblBlock.Text = "Blocked Ports Increment";
            // 
            // m_lblUserSel
            // 
            this.m_lblUserSel.AutoSize = true;
            this.m_lblUserSel.Location = new System.Drawing.Point(29, 92);
            this.m_lblUserSel.Name = "m_lblUserSel";
            this.m_lblUserSel.Size = new System.Drawing.Size(126, 13);
            this.m_lblUserSel.TabIndex = 7;
            this.m_lblUserSel.Text = "User Selection Increment";
            // 
            // m_lblSel
            // 
            this.m_lblSel.AutoSize = true;
            this.m_lblSel.Location = new System.Drawing.Point(29, 35);
            this.m_lblSel.Name = "m_lblSel";
            this.m_lblSel.Size = new System.Drawing.Size(101, 13);
            this.m_lblSel.TabIndex = 6;
            this.m_lblSel.Text = "Selection Increment";
            // 
            // m_btnBlockedPortsIncr
            // 
            this.m_btnBlockedPortsIncr.Location = new System.Drawing.Point(29, 162);
            this.m_btnBlockedPortsIncr.Name = "m_btnBlockedPortsIncr";
            this.m_btnBlockedPortsIncr.Size = new System.Drawing.Size(75, 23);
            this.m_btnBlockedPortsIncr.TabIndex = 5;
            this.m_btnBlockedPortsIncr.Text = "Set Color";
            this.m_btnBlockedPortsIncr.UseVisualStyleBackColor = true;
            this.m_btnBlockedPortsIncr.Click += new System.EventHandler(this.m_btnBlockedPortsIncr_Click);
            // 
            // m_lblBlockedPorts
            // 
            this.m_lblBlockedPorts.AutoSize = true;
            this.m_lblBlockedPorts.BackColor = System.Drawing.Color.Red;
            this.m_lblBlockedPorts.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_lblBlockedPorts.Location = new System.Drawing.Point(182, 148);
            this.m_lblBlockedPorts.Name = "m_lblBlockedPorts";
            this.m_lblBlockedPorts.Size = new System.Drawing.Size(54, 13);
            this.m_lblBlockedPorts.TabIndex = 4;
            this.m_lblBlockedPorts.Text = "Increment";
            // 
            // m_btnUserSelectionIncr
            // 
            this.m_btnUserSelectionIncr.Location = new System.Drawing.Point(29, 108);
            this.m_btnUserSelectionIncr.Name = "m_btnUserSelectionIncr";
            this.m_btnUserSelectionIncr.Size = new System.Drawing.Size(75, 23);
            this.m_btnUserSelectionIncr.TabIndex = 3;
            this.m_btnUserSelectionIncr.Text = "Set Color";
            this.m_btnUserSelectionIncr.UseVisualStyleBackColor = true;
            this.m_btnUserSelectionIncr.Click += new System.EventHandler(this.m_btnUserSelectionIncr_Click);
            // 
            // m_lblUserSelection
            // 
            this.m_lblUserSelection.AutoSize = true;
            this.m_lblUserSelection.BackColor = System.Drawing.Color.Red;
            this.m_lblUserSelection.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_lblUserSelection.Location = new System.Drawing.Point(182, 93);
            this.m_lblUserSelection.Name = "m_lblUserSelection";
            this.m_lblUserSelection.Size = new System.Drawing.Size(54, 13);
            this.m_lblUserSelection.TabIndex = 2;
            this.m_lblUserSelection.Text = "Increment";
            // 
            // m_btnSelectionIncr
            // 
            this.m_btnSelectionIncr.Location = new System.Drawing.Point(29, 55);
            this.m_btnSelectionIncr.Name = "m_btnSelectionIncr";
            this.m_btnSelectionIncr.Size = new System.Drawing.Size(75, 23);
            this.m_btnSelectionIncr.TabIndex = 1;
            this.m_btnSelectionIncr.Text = "Set Color";
            this.m_btnSelectionIncr.UseVisualStyleBackColor = true;
            this.m_btnSelectionIncr.Click += new System.EventHandler(this.m_btnSelectionIncr_Click);
            // 
            // m_lblSelection
            // 
            this.m_lblSelection.AutoSize = true;
            this.m_lblSelection.BackColor = System.Drawing.Color.Red;
            this.m_lblSelection.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_lblSelection.Location = new System.Drawing.Point(182, 35);
            this.m_lblSelection.Name = "m_lblSelection";
            this.m_lblSelection.Size = new System.Drawing.Size(54, 13);
            this.m_lblSelection.TabIndex = 0;
            this.m_lblSelection.Text = "Increment";
            // 
            // ColorSettingsCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabCtrl);
            this.Name = "ColorSettingsCtrl";
            this.Size = new System.Drawing.Size(412, 288);
            this.m_tabCtrl.ResumeLayout(false);
            this.m_tiles.ResumeLayout(false);
            this.m_increments.ResumeLayout(false);
            this.m_increments.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox m_lxtBoxRegexps;
        private System.Windows.Forms.Button m_btnSelectColor;
        private System.Windows.Forms.TabControl m_tabCtrl;
        private System.Windows.Forms.TabPage m_tiles;
        private System.Windows.Forms.TabPage m_increments;
        private System.Windows.Forms.Button m_btnSelectionIncr;
        private System.Windows.Forms.Label m_lblSelection;
        private System.Windows.Forms.Button m_btnUserSelectionIncr;
        private System.Windows.Forms.Label m_lblUserSelection;
        private System.Windows.Forms.Button m_btnBlockedPortsIncr;
        private System.Windows.Forms.Label m_lblBlockedPorts;
        private System.Windows.Forms.Label m_lblSel;
        private System.Windows.Forms.Label m_lblBlock;
        private System.Windows.Forms.Label m_lblUserSel;
    }
}
