namespace GoAhead.GUI.Blocker
{
    partial class BlockerForm
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
            this.m_blockerCtrl = new GoAhead.GUI.Blocker.BlockerCtrl();
            this.SuspendLayout();
            // 
            // m_blockerCtrl
            // 
            this.m_blockerCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_blockerCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_blockerCtrl.Name = "m_blockerCtrl";
            this.m_blockerCtrl.Size = new System.Drawing.Size(176, 222);
            this.m_blockerCtrl.TabIndex = 0;
            // 
            // BlockerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 222);
            this.Controls.Add(this.m_blockerCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlockerForm";
            this.Text = "Block Selection";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BlockerForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private BlockerCtrl m_blockerCtrl;
    }
}