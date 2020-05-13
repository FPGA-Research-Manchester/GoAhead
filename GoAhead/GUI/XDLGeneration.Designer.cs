namespace GoAhead.GUI
{
    partial class XDLGeneration
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
            this.m_chkListMacros = new System.Windows.Forms.CheckedListBox();
            this.m_btnGenerate = new System.Windows.Forms.Button();
            this.m_lblSelectedMacros = new System.Windows.Forms.Label();
            this.m_lblXDLFile = new System.Windows.Forms.Label();
            this.m_txtFileName = new System.Windows.Forms.TextBox();
            this.m_btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_chkListMacros
            // 
            this.m_chkListMacros.FormattingEnabled = true;
            this.m_chkListMacros.Location = new System.Drawing.Point(12, 25);
            this.m_chkListMacros.Name = "m_chkListMacros";
            this.m_chkListMacros.Size = new System.Drawing.Size(199, 169);
            this.m_chkListMacros.TabIndex = 0;
            // 
            // m_btnGenerate
            // 
            this.m_btnGenerate.Location = new System.Drawing.Point(60, 304);
            this.m_btnGenerate.Name = "m_btnGenerate";
            this.m_btnGenerate.Size = new System.Drawing.Size(93, 23);
            this.m_btnGenerate.TabIndex = 1;
            this.m_btnGenerate.Text = "Generate XDL";
            this.m_btnGenerate.UseVisualStyleBackColor = true;
            this.m_btnGenerate.Click += new System.EventHandler(this.m_btnGenerate_Click);
            // 
            // m_lblSelectedMacros
            // 
            this.m_lblSelectedMacros.AutoSize = true;
            this.m_lblSelectedMacros.Location = new System.Drawing.Point(9, 9);
            this.m_lblSelectedMacros.Name = "m_lblSelectedMacros";
            this.m_lblSelectedMacros.Size = new System.Drawing.Size(75, 13);
            this.m_lblSelectedMacros.TabIndex = 2;
            this.m_lblSelectedMacros.Text = "Select Macros";
            // 
            // m_lblXDLFile
            // 
            this.m_lblXDLFile.AutoSize = true;
            this.m_lblXDLFile.Location = new System.Drawing.Point(12, 203);
            this.m_lblXDLFile.Name = "m_lblXDLFile";
            this.m_lblXDLFile.Size = new System.Drawing.Size(47, 13);
            this.m_lblXDLFile.TabIndex = 3;
            this.m_lblXDLFile.Text = "XDL File";
            // 
            // m_txtFileName
            // 
            this.m_txtFileName.Location = new System.Drawing.Point(12, 219);
            this.m_txtFileName.Name = "m_txtFileName";
            this.m_txtFileName.Size = new System.Drawing.Size(198, 20);
            this.m_txtFileName.TabIndex = 4;
            // 
            // m_btnBrowse
            // 
            this.m_btnBrowse.Location = new System.Drawing.Point(12, 245);
            this.m_btnBrowse.Name = "m_btnBrowse";
            this.m_btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.m_btnBrowse.TabIndex = 5;
            this.m_btnBrowse.Text = "Browse";
            this.m_btnBrowse.UseVisualStyleBackColor = true;
            this.m_btnBrowse.Click += new System.EventHandler(this.m_btnBrowse_Click);
            // 
            // XDLGeneration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 339);
            this.Controls.Add(this.m_chkListMacros);
            this.Controls.Add(this.m_txtFileName);
            this.Controls.Add(this.m_btnBrowse);
            this.Controls.Add(this.m_btnGenerate);
            this.Controls.Add(this.m_lblXDLFile);
            this.Controls.Add(this.m_lblSelectedMacros);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XDLGeneration";
            this.ShowInTaskbar = false;
            this.Text = "XDLGeneration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox m_chkListMacros;
        private System.Windows.Forms.Button m_btnGenerate;
        private System.Windows.Forms.Label m_lblSelectedMacros;
        private System.Windows.Forms.Label m_lblXDLFile;
        private System.Windows.Forms.TextBox m_txtFileName;
        private System.Windows.Forms.Button m_btnBrowse;
    }
}