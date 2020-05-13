namespace GoAhead.GUI.BUFGInfo
{
    partial class BUFGInfoForm
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
            this.m_bufgInfo = new GoAhead.GUI.BUFGInfo.BUFGInfoCtrl();
            this.SuspendLayout();
            // 
            // m_bufgInfo
            // 
            this.m_bufgInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_bufgInfo.Location = new System.Drawing.Point(0, 0);
            this.m_bufgInfo.Name = "m_bufgInfo";
            this.m_bufgInfo.Size = new System.Drawing.Size(555, 472);
            this.m_bufgInfo.TabIndex = 0;
            // 
            // BUFGInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 472);
            this.Controls.Add(this.m_bufgInfo);
            this.Name = "BUFGInfoForm";
            this.Text = "BUFG Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BUFGInfoForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private GoAhead.GUI.BUFGInfo.BUFGInfoCtrl m_bufgInfo;
    }
}