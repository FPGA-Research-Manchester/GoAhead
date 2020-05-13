namespace GoAhead.GUI.Macros.DesignBrowser
{
    partial class DesignBrowserForm
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
            this.m_designBrowserCtrl = new GoAhead.GUI.Macros.DesignBrowser.DesignBrowserCtrl();
            this.SuspendLayout();
            // 
            // m_designBrowserCtrl
            // 
            this.m_designBrowserCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_designBrowserCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_designBrowserCtrl.Name = "m_designBrowserCtrl";
            this.m_designBrowserCtrl.Size = new System.Drawing.Size(292, 273);
            this.m_designBrowserCtrl.TabIndex = 0;
            // 
            // DesignBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.m_designBrowserCtrl);
            this.Name = "DesignBrowserForm";
            this.Text = "Design Browser";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DesignBrowserForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private DesignBrowserCtrl m_designBrowserCtrl;
    }
}