namespace GoAhead.GUI
{
    partial class TileViewForm
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_txtBox = new System.Windows.Forms.RichTextBox();
            this.m_tabCtrlTop = new System.Windows.Forms.TabControl();
            this.m_tabText = new System.Windows.Forms.TabPage();
            this.m_tabSwitchMatrix = new System.Windows.Forms.TabPage();
            this.m_grdViewSwitchMatrix = new System.Windows.Forms.DataGridView();
            this.m_in = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_out = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_lut = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_tabWires = new System.Windows.Forms.TabPage();
            this.m_grdViewWires = new System.Windows.Forms.DataGridView();
            this.m_localPip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_locapPipIsDriver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_pipOnOtherTile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_xIncr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_yIncr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_targetTile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_tabCtrlTop.SuspendLayout();
            this.m_tabText.SuspendLayout();
            this.m_tabSwitchMatrix.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewSwitchMatrix)).BeginInit();
            this.m_tabWires.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewWires)).BeginInit();
            this.SuspendLayout();
            // 
            // m_txtBox
            // 
            this.m_txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtBox.Location = new System.Drawing.Point(3, 3);
            this.m_txtBox.Name = "m_txtBox";
            this.m_txtBox.ReadOnly = true;
            this.m_txtBox.Size = new System.Drawing.Size(825, 501);
            this.m_txtBox.TabIndex = 0;
            this.m_txtBox.Text = "";
            // 
            // m_tabCtrlTop
            // 
            this.m_tabCtrlTop.Controls.Add(this.m_tabText);
            this.m_tabCtrlTop.Controls.Add(this.m_tabSwitchMatrix);
            this.m_tabCtrlTop.Controls.Add(this.m_tabWires);
            this.m_tabCtrlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabCtrlTop.Location = new System.Drawing.Point(0, 0);
            this.m_tabCtrlTop.Name = "m_tabCtrlTop";
            this.m_tabCtrlTop.SelectedIndex = 0;
            this.m_tabCtrlTop.Size = new System.Drawing.Size(839, 533);
            this.m_tabCtrlTop.TabIndex = 1;
            // 
            // m_tabText
            // 
            this.m_tabText.Controls.Add(this.m_txtBox);
            this.m_tabText.Location = new System.Drawing.Point(4, 22);
            this.m_tabText.Name = "m_tabText";
            this.m_tabText.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabText.Size = new System.Drawing.Size(831, 507);
            this.m_tabText.TabIndex = 0;
            this.m_tabText.Text = "Text";
            this.m_tabText.UseVisualStyleBackColor = true;
            // 
            // m_tabSwitchMatrix
            // 
            this.m_tabSwitchMatrix.Controls.Add(this.m_grdViewSwitchMatrix);
            this.m_tabSwitchMatrix.Location = new System.Drawing.Point(4, 22);
            this.m_tabSwitchMatrix.Name = "m_tabSwitchMatrix";
            this.m_tabSwitchMatrix.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabSwitchMatrix.Size = new System.Drawing.Size(831, 507);
            this.m_tabSwitchMatrix.TabIndex = 1;
            this.m_tabSwitchMatrix.Text = "Switch Matrix";
            this.m_tabSwitchMatrix.UseVisualStyleBackColor = true;
            // 
            // m_grdViewSwitchMatrix
            // 
            this.m_grdViewSwitchMatrix.AllowUserToAddRows = false;
            this.m_grdViewSwitchMatrix.AllowUserToDeleteRows = false;
            this.m_grdViewSwitchMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewSwitchMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewSwitchMatrix.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_in,
            this.m_out,
            this.m_lut});
            this.m_grdViewSwitchMatrix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_grdViewSwitchMatrix.Location = new System.Drawing.Point(3, 3);
            this.m_grdViewSwitchMatrix.Name = "m_grdViewSwitchMatrix";
            this.m_grdViewSwitchMatrix.ReadOnly = true;
            this.m_grdViewSwitchMatrix.Size = new System.Drawing.Size(825, 501);
            this.m_grdViewSwitchMatrix.TabIndex = 0;
            // 
            // m_in
            // 
            this.m_in.HeaderText = "In";
            this.m_in.Name = "m_in";
            this.m_in.ReadOnly = true;
            // 
            // m_out
            // 
            this.m_out.HeaderText = "Out";
            this.m_out.Name = "m_out";
            this.m_out.ReadOnly = true;
            // 
            // m_lut
            // 
            this.m_lut.HeaderText = "LUT-Input";
            this.m_lut.Name = "m_lut";
            this.m_lut.ReadOnly = true;
            // 
            // m_tabWires
            // 
            this.m_tabWires.Controls.Add(this.m_grdViewWires);
            this.m_tabWires.Location = new System.Drawing.Point(4, 22);
            this.m_tabWires.Name = "m_tabWires";
            this.m_tabWires.Size = new System.Drawing.Size(831, 507);
            this.m_tabWires.TabIndex = 2;
            this.m_tabWires.Text = "Wires";
            this.m_tabWires.UseVisualStyleBackColor = true;
            // 
            // m_grdViewWires
            // 
            this.m_grdViewWires.AllowUserToAddRows = false;
            this.m_grdViewWires.AllowUserToDeleteRows = false;
            this.m_grdViewWires.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewWires.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewWires.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_localPip,
            this.m_locapPipIsDriver,
            this.m_pipOnOtherTile,
            this.m_xIncr,
            this.m_yIncr,
            this.m_targetTile});
            this.m_grdViewWires.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_grdViewWires.Location = new System.Drawing.Point(0, 0);
            this.m_grdViewWires.Name = "m_grdViewWires";
            this.m_grdViewWires.ReadOnly = true;
            this.m_grdViewWires.Size = new System.Drawing.Size(831, 507);
            this.m_grdViewWires.TabIndex = 1;
            // 
            // m_localPip
            // 
            this.m_localPip.HeaderText = "LocalPip";
            this.m_localPip.Name = "m_localPip";
            this.m_localPip.ReadOnly = true;
            // 
            // m_locapPipIsDriver
            // 
            this.m_locapPipIsDriver.HeaderText = "LocapPipIsDriver";
            this.m_locapPipIsDriver.Name = "m_locapPipIsDriver";
            this.m_locapPipIsDriver.ReadOnly = true;
            // 
            // m_pipOnOtherTile
            // 
            this.m_pipOnOtherTile.HeaderText = "PipOnOtherTile";
            this.m_pipOnOtherTile.Name = "m_pipOnOtherTile";
            this.m_pipOnOtherTile.ReadOnly = true;
            // 
            // m_xIncr
            // 
            this.m_xIncr.HeaderText = "XIncr";
            this.m_xIncr.Name = "m_xIncr";
            this.m_xIncr.ReadOnly = true;
            // 
            // m_yIncr
            // 
            this.m_yIncr.HeaderText = "YIncr";
            this.m_yIncr.Name = "m_yIncr";
            this.m_yIncr.ReadOnly = true;
            // 
            // m_targetTile
            // 
            this.m_targetTile.HeaderText = "Target";
            this.m_targetTile.Name = "m_targetTile";
            this.m_targetTile.ReadOnly = true;
            // 
            // TileViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 533);
            this.Controls.Add(this.m_tabCtrlTop);
            this.Name = "TileViewForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TileView_FormClosed);
            this.m_tabCtrlTop.ResumeLayout(false);
            this.m_tabText.ResumeLayout(false);
            this.m_tabSwitchMatrix.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewSwitchMatrix)).EndInit();
            this.m_tabWires.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewWires)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox m_txtBox;
        private System.Windows.Forms.TabControl m_tabCtrlTop;
        private System.Windows.Forms.TabPage m_tabText;
        private System.Windows.Forms.TabPage m_tabSwitchMatrix;
        private System.Windows.Forms.DataGridView m_grdViewSwitchMatrix;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_in;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_out;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_lut;
        private System.Windows.Forms.TabPage m_tabWires;
        private System.Windows.Forms.DataGridView m_grdViewWires;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_localPip;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_locapPipIsDriver;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_pipOnOtherTile;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_xIncr;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_yIncr;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_targetTile;
    }
}
