namespace GoAhead.GUI.Macros.DesignBrowser
{
    partial class DesignBrowserCtrl
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
            this.m_treeView = new System.Windows.Forms.TreeView();
            this.m_txtInstanceCode = new System.Windows.Forms.RichTextBox();
            this.m_lblResource = new System.Windows.Forms.Label();
            this.m_lblSlices = new System.Windows.Forms.Label();
            this.m_lblBRAM = new System.Windows.Forms.Label();
            this.m_lblDSP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_treeView
            // 
            this.m_treeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_treeView.Location = new System.Drawing.Point(0, 0);
            this.m_treeView.Name = "m_treeView";
            this.m_treeView.Size = new System.Drawing.Size(214, 504);
            this.m_treeView.TabIndex = 0;
            this.m_treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.m_treeView_NodeMouseClick);
            // 
            // m_txtInstanceCode
            // 
            this.m_txtInstanceCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_txtInstanceCode.Location = new System.Drawing.Point(214, 0);
            this.m_txtInstanceCode.Name = "m_txtInstanceCode";
            this.m_txtInstanceCode.Size = new System.Drawing.Size(389, 216);
            this.m_txtInstanceCode.TabIndex = 1;
            this.m_txtInstanceCode.Text = "";
            // 
            // m_lblResource
            // 
            this.m_lblResource.AutoSize = true;
            this.m_lblResource.Location = new System.Drawing.Point(221, 223);
            this.m_lblResource.Name = "m_lblResource";
            this.m_lblResource.Size = new System.Drawing.Size(61, 13);
            this.m_lblResource.TabIndex = 2;
            this.m_lblResource.Text = "Resources:";
            // 
            // m_lblSlices
            // 
            this.m_lblSlices.AutoSize = true;
            this.m_lblSlices.Location = new System.Drawing.Point(283, 226);
            this.m_lblSlices.Name = "m_lblSlices";
            this.m_lblSlices.Size = new System.Drawing.Size(13, 13);
            this.m_lblSlices.TabIndex = 3;
            this.m_lblSlices.Text = "0";
            // 
            // m_lblBRAM
            // 
            this.m_lblBRAM.AutoSize = true;
            this.m_lblBRAM.Location = new System.Drawing.Point(283, 253);
            this.m_lblBRAM.Name = "m_lblBRAM";
            this.m_lblBRAM.Size = new System.Drawing.Size(13, 13);
            this.m_lblBRAM.TabIndex = 4;
            this.m_lblBRAM.Text = "0";
            // 
            // m_lblDSP
            // 
            this.m_lblDSP.AutoSize = true;
            this.m_lblDSP.Location = new System.Drawing.Point(283, 280);
            this.m_lblDSP.Name = "m_lblDSP";
            this.m_lblDSP.Size = new System.Drawing.Size(13, 13);
            this.m_lblDSP.TabIndex = 5;
            this.m_lblDSP.Text = "0";
            // 
            // DesignBrowserCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_lblDSP);
            this.Controls.Add(this.m_lblBRAM);
            this.Controls.Add(this.m_lblSlices);
            this.Controls.Add(this.m_lblResource);
            this.Controls.Add(this.m_txtInstanceCode);
            this.Controls.Add(this.m_treeView);
            this.Name = "DesignBrowserCtrl";
            this.Size = new System.Drawing.Size(603, 504);
            this.Load += new System.EventHandler(this.DesignBrowserCtrl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView m_treeView;
        private System.Windows.Forms.RichTextBox m_txtInstanceCode;
        private System.Windows.Forms.Label m_lblResource;
        private System.Windows.Forms.Label m_lblSlices;
        private System.Windows.Forms.Label m_lblBRAM;
        private System.Windows.Forms.Label m_lblDSP;
    }
}
