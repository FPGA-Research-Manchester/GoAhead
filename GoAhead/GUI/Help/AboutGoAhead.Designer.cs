namespace GoAhead.GUI
{
    partial class AboutGoAhead
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutGoAhead));
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_lblLink = new System.Windows.Forms.LinkLabel();
            this.m_lblVersion = new System.Windows.Forms.Label();
            this.m_lblGoAhead = new System.Windows.Forms.Label();
            this.m_lblSlogan1 = new System.Windows.Forms.Label();
            this.m_lblSlogan2 = new System.Windows.Forms.Label();
            this.m_lblSlogan3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_btnOk
            // 
            this.m_btnOk.Location = new System.Drawing.Point(251, 521);
            this.m_btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(100, 28);
            this.m_btnOk.TabIndex = 0;
            this.m_btnOk.Text = "OK";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
            // 
            // m_lblLink
            // 
            this.m_lblLink.AutoSize = true;
            this.m_lblLink.Location = new System.Drawing.Point(127, 530);
            this.m_lblLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblLink.Name = "m_lblLink";
            this.m_lblLink.Size = new System.Drawing.Size(110, 17);
            this.m_lblLink.TabIndex = 0;
            this.m_lblLink.TabStop = true;
            this.m_lblLink.Text = "www.recobus.de";
            this.m_lblLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_lblLink_LinkClicked);
            // 
            // m_lblVersion
            // 
            this.m_lblVersion.AutoSize = true;
            this.m_lblVersion.Location = new System.Drawing.Point(43, 103);
            this.m_lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblVersion.Name = "m_lblVersion";
            this.m_lblVersion.Size = new System.Drawing.Size(116, 17);
            this.m_lblVersion.TabIndex = 2;
            this.m_lblVersion.Text = "Unknown version";
            // 
            // m_lblGoAhead
            // 
            this.m_lblGoAhead.AutoSize = true;
            this.m_lblGoAhead.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblGoAhead.Location = new System.Drawing.Point(25, 25);
            this.m_lblGoAhead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblGoAhead.Name = "m_lblGoAhead";
            this.m_lblGoAhead.Size = new System.Drawing.Size(289, 69);
            this.m_lblGoAhead.TabIndex = 3;
            this.m_lblGoAhead.Text = "GoAhead";
            // 
            // m_lblSlogan1
            // 
            this.m_lblSlogan1.AutoSize = true;
            this.m_lblSlogan1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblSlogan1.Location = new System.Drawing.Point(57, 306);
            this.m_lblSlogan1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblSlogan1.Name = "m_lblSlogan1";
            this.m_lblSlogan1.Size = new System.Drawing.Size(233, 29);
            this.m_lblSlogan1.TabIndex = 4;
            this.m_lblSlogan1.Text = "The Simple Formula";
            // 
            // m_lblSlogan2
            // 
            this.m_lblSlogan2.AutoSize = true;
            this.m_lblSlogan2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblSlogan2.Location = new System.Drawing.Point(45, 351);
            this.m_lblSlogan2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblSlogan2.Name = "m_lblSlogan2";
            this.m_lblSlogan2.Size = new System.Drawing.Size(257, 29);
            this.m_lblSlogan2.TabIndex = 5;
            this.m_lblSlogan2.Text = "for Building Bus-based";
            // 
            // m_lblSlogan3
            // 
            this.m_lblSlogan3.AutoSize = true;
            this.m_lblSlogan3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblSlogan3.Location = new System.Drawing.Point(39, 396);
            this.m_lblSlogan3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblSlogan3.Name = "m_lblSlogan3";
            this.m_lblSlogan3.Size = new System.Drawing.Size(274, 29);
            this.m_lblSlogan3.TabIndex = 6;
            this.m_lblSlogan3.Text = "Reconfigurable Systems";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(27, 160);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(355, 112);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 460);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Icons Credits";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(24, 477);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(122, 17);
            this.linkLabel1.TabIndex = 9;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://icons8.com";
            // 
            // AboutGoAhead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 558);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.m_lblSlogan3);
            this.Controls.Add(this.m_lblSlogan2);
            this.Controls.Add(this.m_lblSlogan1);
            this.Controls.Add(this.m_lblVersion);
            this.Controls.Add(this.m_lblLink);
            this.Controls.Add(this.m_btnOk);
            this.Controls.Add(this.m_lblGoAhead);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutGoAhead";
            this.Text = "About GoAhead";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AboutGoAhead_FormClosed);
            this.Load += new System.EventHandler(this.AboutGoAhead_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.LinkLabel m_lblLink;
        private System.Windows.Forms.Label m_lblVersion;
        private System.Windows.Forms.Label m_lblGoAhead;
        private System.Windows.Forms.Label m_lblSlogan1;
        private System.Windows.Forms.Label m_lblSlogan2;
        private System.Windows.Forms.Label m_lblSlogan3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}