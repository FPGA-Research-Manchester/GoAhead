namespace GoAhead.GUI.TileView
{
    partial class SingleTileViewForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_tabTop = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // m_tabTop
            // 
            this.m_tabTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabTop.Location = new System.Drawing.Point(0, 0);
            this.m_tabTop.Name = "m_tabTop";
            this.m_tabTop.SelectedIndex = 0;
            this.m_tabTop.Size = new System.Drawing.Size(711, 498);
            this.m_tabTop.TabIndex = 0;
            // 
            // TileViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 498);
            this.Controls.Add(this.m_tabTop);
            this.Name = "TileViewForm";
            this.Text = "Tile View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TileViewForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl m_tabTop;

    }
}