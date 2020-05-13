namespace GoAhead.GUI.ExpandSelection
{
    partial class ExpandSelectionForm
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
            this.m_expandSelectionCtrl = new GoAhead.GUI.ExpandSelection.ExpandSelectionCtrl();
            this.SuspendLayout();
            // 
            // m_expandSelectionCtrl
            // 
            this.m_expandSelectionCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_expandSelectionCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_expandSelectionCtrl.Name = "m_expandSelectionCtrl";
            this.m_expandSelectionCtrl.Size = new System.Drawing.Size(193, 152);
            this.m_expandSelectionCtrl.TabIndex = 0;
            // 
            // ExpandSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 152);
            this.Controls.Add(this.m_expandSelectionCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExpandSelectionForm";
            this.Text = "Expand Selection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExpandSelectionForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ExpandSelectionCtrl m_expandSelectionCtrl;
    }
}