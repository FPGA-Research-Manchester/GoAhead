namespace GoAhead.GUI.UCF
{
    partial class ProhibitStatementForm
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
            this.m_btnPrint = new System.Windows.Forms.Button();
            this.m_chkExclude = new System.Windows.Forms.CheckBox();
            this.m_fileSelUCF = new GoAhead.GUI.FileSelectionCtrl();
            this.SuspendLayout();
            // 
            // m_btnPrint
            // 
            this.m_btnPrint.Location = new System.Drawing.Point(110, 110);
            this.m_btnPrint.Name = "m_btnPrint";
            this.m_btnPrint.Size = new System.Drawing.Size(75, 23);
            this.m_btnPrint.TabIndex = 5;
            this.m_btnPrint.Text = "Print";
            this.m_btnPrint.UseVisualStyleBackColor = true;
            this.m_btnPrint.Click += new System.EventHandler(this.m_btnGenerate_Click);
            // 
            // m_chkExclude
            // 
            this.m_chkExclude.AutoSize = true;
            this.m_chkExclude.Checked = true;
            this.m_chkExclude.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkExclude.Location = new System.Drawing.Point(14, 16);
            this.m_chkExclude.Name = "m_chkExclude";
            this.m_chkExclude.Size = new System.Drawing.Size(121, 17);
            this.m_chkExclude.TabIndex = 8;
            this.m_chkExclude.Text = "Exclude used Slices";
            this.m_chkExclude.UseVisualStyleBackColor = true;
            // 
            // m_fileSelUCF
            // 
            this.m_fileSelUCF.Append = true;
            this.m_fileSelUCF.FileName = "";
            this.m_fileSelUCF.Filter = "All files|*.UCF";
            this.m_fileSelUCF.Label = "UCF File";
            this.m_fileSelUCF.Location = new System.Drawing.Point(13, 43);
            this.m_fileSelUCF.Name = "m_fileSelUCF";
            this.m_fileSelUCF.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelUCF.TabIndex = 9;
            // 
            // ProhibitStatementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 143);
            this.Controls.Add(this.m_fileSelUCF);
            this.Controls.Add(this.m_chkExclude);
            this.Controls.Add(this.m_btnPrint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProhibitStatementForm";
            this.Text = "Prohibit Statement";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProhibitStatementForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnPrint;
        private System.Windows.Forms.CheckBox m_chkExclude;
        private FileSelectionCtrl m_fileSelUCF;
    }
}