namespace GoAhead.GUI
{
    partial class FindTileGUI
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
            this.m_btnFindXY = new System.Windows.Forms.Button();
            this.m_lblX = new System.Windows.Forms.Label();
            this.m_lblY = new System.Windows.Forms.Label();
            this.m_lblSlice = new System.Windows.Forms.Label();
            this.m_btnFindSlice = new System.Windows.Forms.Button();
            this.m_btnFindLocation = new System.Windows.Forms.Button();
            this.m_lblLocation = new System.Windows.Forms.Label();
            this.m_txtLocation = new System.Windows.Forms.TextBox();
            this.m_txtSliceName = new System.Windows.Forms.TextBox();
            this.m_txtX = new System.Windows.Forms.TextBox();
            this.m_txtY = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_btnFindXY
            // 
            this.m_btnFindXY.Location = new System.Drawing.Point(192, 16);
            this.m_btnFindXY.Name = "m_btnFindXY";
            this.m_btnFindXY.Size = new System.Drawing.Size(75, 23);
            this.m_btnFindXY.TabIndex = 3;
            this.m_btnFindXY.Text = "Find";
            this.m_btnFindXY.UseVisualStyleBackColor = true;
            this.m_btnFindXY.Click += new System.EventHandler(this.m_btnFindXY_Click);
            // 
            // m_lblX
            // 
            this.m_lblX.AutoSize = true;
            this.m_lblX.Location = new System.Drawing.Point(2, 2);
            this.m_lblX.Name = "m_lblX";
            this.m_lblX.Size = new System.Drawing.Size(14, 13);
            this.m_lblX.TabIndex = 3;
            this.m_lblX.Text = "X";
            // 
            // m_lblY
            // 
            this.m_lblY.AutoSize = true;
            this.m_lblY.Location = new System.Drawing.Point(58, 2);
            this.m_lblY.Name = "m_lblY";
            this.m_lblY.Size = new System.Drawing.Size(14, 13);
            this.m_lblY.TabIndex = 4;
            this.m_lblY.Text = "Y";
            // 
            // m_lblSlice
            // 
            this.m_lblSlice.AutoSize = true;
            this.m_lblSlice.Location = new System.Drawing.Point(2, 39);
            this.m_lblSlice.Name = "m_lblSlice";
            this.m_lblSlice.Size = new System.Drawing.Size(30, 13);
            this.m_lblSlice.TabIndex = 6;
            this.m_lblSlice.Text = "Slice";
            // 
            // m_btnFindSlice
            // 
            this.m_btnFindSlice.Location = new System.Drawing.Point(192, 56);
            this.m_btnFindSlice.Name = "m_btnFindSlice";
            this.m_btnFindSlice.Size = new System.Drawing.Size(75, 23);
            this.m_btnFindSlice.TabIndex = 5;
            this.m_btnFindSlice.Text = "Find";
            this.m_btnFindSlice.UseVisualStyleBackColor = true;
            this.m_btnFindSlice.Click += new System.EventHandler(this.m_btnFindSlice_Click);
            // 
            // m_btnFindLocation
            // 
            this.m_btnFindLocation.Location = new System.Drawing.Point(190, 97);
            this.m_btnFindLocation.Name = "m_btnFindLocation";
            this.m_btnFindLocation.Size = new System.Drawing.Size(75, 23);
            this.m_btnFindLocation.TabIndex = 7;
            this.m_btnFindLocation.Text = "Find";
            this.m_btnFindLocation.UseVisualStyleBackColor = true;
            this.m_btnFindLocation.Click += new System.EventHandler(this.m_btnFindLocation_Click);
            // 
            // m_lblLocation
            // 
            this.m_lblLocation.AutoSize = true;
            this.m_lblLocation.Location = new System.Drawing.Point(0, 80);
            this.m_lblLocation.Name = "m_lblLocation";
            this.m_lblLocation.Size = new System.Drawing.Size(48, 13);
            this.m_lblLocation.TabIndex = 9;
            this.m_lblLocation.Text = "Location";
            // 
            // m_txtLocation
            // 
            this.m_txtLocation.Location = new System.Drawing.Point(3, 97);
            this.m_txtLocation.Name = "m_txtLocation";
            this.m_txtLocation.Size = new System.Drawing.Size(175, 20);
            this.m_txtLocation.TabIndex = 6;
            // 
            // m_txtSliceName
            // 
            this.m_txtSliceName.Location = new System.Drawing.Point(2, 56);
            this.m_txtSliceName.Name = "m_txtSliceName";
            this.m_txtSliceName.Size = new System.Drawing.Size(178, 20);
            this.m_txtSliceName.TabIndex = 4;
            // 
            // m_txtX
            // 
            this.m_txtX.Location = new System.Drawing.Point(4, 16);
            this.m_txtX.Name = "m_txtX";
            this.m_txtX.Size = new System.Drawing.Size(43, 20);
            this.m_txtX.TabIndex = 1;
            // 
            // m_txtY
            // 
            this.m_txtY.Location = new System.Drawing.Point(57, 16);
            this.m_txtY.Name = "m_txtY";
            this.m_txtY.Size = new System.Drawing.Size(43, 20);
            this.m_txtY.TabIndex = 2;
            // 
            // FindTileGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 126);
            this.Controls.Add(this.m_txtY);
            this.Controls.Add(this.m_txtX);
            this.Controls.Add(this.m_txtSliceName);
            this.Controls.Add(this.m_txtLocation);
            this.Controls.Add(this.m_btnFindLocation);
            this.Controls.Add(this.m_lblLocation);
            this.Controls.Add(this.m_btnFindSlice);
            this.Controls.Add(this.m_lblSlice);
            this.Controls.Add(this.m_lblY);
            this.Controls.Add(this.m_lblX);
            this.Controls.Add(this.m_btnFindXY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindTileGUI";
            this.Text = "Find Tile";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FindTileGUI_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnFindXY;
        private System.Windows.Forms.Label m_lblX;
        private System.Windows.Forms.Label m_lblY;
        private System.Windows.Forms.Label m_lblSlice;
        private System.Windows.Forms.Button m_btnFindSlice;
        private System.Windows.Forms.Button m_btnFindLocation;
        private System.Windows.Forms.Label m_lblLocation;
        private System.Windows.Forms.TextBox m_txtLocation;
        private System.Windows.Forms.TextBox m_txtSliceName;
        private System.Windows.Forms.TextBox m_txtX;
        private System.Windows.Forms.TextBox m_txtY;
    }
}