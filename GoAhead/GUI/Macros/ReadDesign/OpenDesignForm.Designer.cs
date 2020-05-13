namespace GoAhead.GUI.AddLibraryManager
{
    partial class OpenDesignForm
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
            this.m_readDesignIntoMacroCtrl = new GoAhead.GUI.AddLibraryManager.ReadDesignCtrl();
            this.SuspendLayout();
            // 
            // m_readDesignIntoMacroCtrl
            // 
            this.m_readDesignIntoMacroCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_readDesignIntoMacroCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_readDesignIntoMacroCtrl.Name = "m_readDesignIntoMacroCtrl";
            this.m_readDesignIntoMacroCtrl.Size = new System.Drawing.Size(265, 234);
            this.m_readDesignIntoMacroCtrl.TabIndex = 0;
            // 
            // ReadDesignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 234);
            this.Controls.Add(this.m_readDesignIntoMacroCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReadDesignForm";
            this.Text = "Read Design";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReadDesignIntoMacroForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ReadDesignCtrl m_readDesignIntoMacroCtrl;
    }
}