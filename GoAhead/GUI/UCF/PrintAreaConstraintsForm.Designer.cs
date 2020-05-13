namespace GoAhead.GUI.UCF
{
    partial class PrintAreaConstraintsForm
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
            this.m_btnGenerate = new System.Windows.Forms.Button();
            this.m_txtInstanceName = new System.Windows.Forms.TextBox();
            this.m_lblInstanceName = new System.Windows.Forms.Label();
            this.m_fileSelUCF = new GoAhead.GUI.FileSelectionCtrl();
            this.SuspendLayout();
            // 
            // m_btnGenerate
            // 
            this.m_btnGenerate.Location = new System.Drawing.Point(104, 143);
            this.m_btnGenerate.Name = "m_btnGenerate";
            this.m_btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.m_btnGenerate.TabIndex = 11;
            this.m_btnGenerate.Text = "Generate";
            this.m_btnGenerate.UseVisualStyleBackColor = true;
            this.m_btnGenerate.Click += new System.EventHandler(this.m_btnGenerate_Click);
            // 
            // m_txtInstanceName
            // 
            this.m_txtInstanceName.Location = new System.Drawing.Point(15, 104);
            this.m_txtInstanceName.Name = "m_txtInstanceName";
            this.m_txtInstanceName.Size = new System.Drawing.Size(260, 20);
            this.m_txtInstanceName.TabIndex = 16;
            // 
            // m_lblInstanceName
            // 
            this.m_lblInstanceName.AutoSize = true;
            this.m_lblInstanceName.Location = new System.Drawing.Point(12, 88);
            this.m_lblInstanceName.Name = "m_lblInstanceName";
            this.m_lblInstanceName.Size = new System.Drawing.Size(79, 13);
            this.m_lblInstanceName.TabIndex = 17;
            this.m_lblInstanceName.Text = "Instance Name";
            // 
            // m_fileSelUCF
            // 
            this.m_fileSelUCF.Append = true;
            this.m_fileSelUCF.FileName = "";
            this.m_fileSelUCF.Filter = "All files|*.UCF";
            this.m_fileSelUCF.Label = "UCF File";
            this.m_fileSelUCF.Location = new System.Drawing.Point(15, 13);
            this.m_fileSelUCF.Name = "m_fileSelUCF";
            this.m_fileSelUCF.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelUCF.TabIndex = 18;
            // 
            // PrintAreaConstraintsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 177);
            this.Controls.Add(this.m_fileSelUCF);
            this.Controls.Add(this.m_txtInstanceName);
            this.Controls.Add(this.m_btnGenerate);
            this.Controls.Add(this.m_lblInstanceName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintAreaConstraintsForm";
            this.Text = "Print Area Constraints";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PrintAreaConstraintsForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnGenerate;
        private System.Windows.Forms.TextBox m_txtInstanceName;
        private System.Windows.Forms.Label m_lblInstanceName;
        private FileSelectionCtrl m_fileSelUCF;
    }
}