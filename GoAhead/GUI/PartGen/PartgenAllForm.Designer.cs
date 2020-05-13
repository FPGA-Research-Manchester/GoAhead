namespace GoAhead.GUI.PartGen
{
    partial class PartgenAllForm
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
            this.m_partGenAllCtrl = new GoAhead.GUI.PartGen.PartGenAllCtrl();
            this.SuspendLayout();
            // 
            // m_partGenAllCtrl
            // 
            this.m_partGenAllCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_partGenAllCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_partGenAllCtrl.Name = "m_partGenAllCtrl";
            this.m_partGenAllCtrl.Size = new System.Drawing.Size(259, 561);
            this.m_partGenAllCtrl.TabIndex = 0;
            // 
            // PartgenAllForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 561);
            this.Controls.Add(this.m_partGenAllCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PartgenAllForm";
            this.Text = "Generate binFGPAs";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PartgenAllForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private PartGenAllCtrl m_partGenAllCtrl;
    }
}