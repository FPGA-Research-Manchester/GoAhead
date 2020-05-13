namespace GoAhead.GUI.BUFGInfo
{
    partial class BUFGInfoCtrl
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
            this.m_dataGrd = new System.Windows.Forms.DataGridView();
            this.m_tile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_BUFGInstance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrd)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataGrd
            // 
            this.m_dataGrd.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_dataGrd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataGrd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_tile,
            this.m_BUFGInstance});
            this.m_dataGrd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dataGrd.Location = new System.Drawing.Point(0, 0);
            this.m_dataGrd.Name = "m_dataGrd";
            this.m_dataGrd.Size = new System.Drawing.Size(461, 371);
            this.m_dataGrd.TabIndex = 0;
            // 
            // m_tile
            // 
            this.m_tile.HeaderText = "Tile";
            this.m_tile.Name = "m_tile";
            this.m_tile.ReadOnly = true;
            // 
            // m_BUFGInstance
            // 
            this.m_BUFGInstance.HeaderText = "BUFGInstance";
            this.m_BUFGInstance.Name = "m_BUFGInstance";
            this.m_BUFGInstance.ReadOnly = true;
            // 
            // BUFGInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_dataGrd);
            this.Name = "BUFGInfo";
            this.Size = new System.Drawing.Size(461, 371);
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView m_dataGrd;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_tile;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_BUFGInstance;
    }
}
