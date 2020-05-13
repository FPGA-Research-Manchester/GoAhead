namespace GoAhead.GUI.UCF
{
    partial class LocationConstraintsGUI
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
            this.m_btnGenerate = new System.Windows.Forms.Button();
            this.m_numSliecIndex = new System.Windows.Forms.NumericUpDown();
            this.m_txtInstanceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InstanceName = new System.Windows.Forms.Label();
            this.m_fileSelCtrlUCF = new GoAhead.GUI.FileSelectionCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.m_numSliecIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // m_btnGenerate
            // 
            this.m_btnGenerate.Location = new System.Drawing.Point(105, 211);
            this.m_btnGenerate.Name = "m_btnGenerate";
            this.m_btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.m_btnGenerate.TabIndex = 0;
            this.m_btnGenerate.Text = "Generate";
            this.m_btnGenerate.UseVisualStyleBackColor = true;
            this.m_btnGenerate.Click += new System.EventHandler(this.m_btnGenerate_Click);
            // 
            // m_numSliecIndex
            // 
            this.m_numSliecIndex.Location = new System.Drawing.Point(12, 35);
            this.m_numSliecIndex.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numSliecIndex.Name = "m_numSliecIndex";
            this.m_numSliecIndex.Size = new System.Drawing.Size(266, 20);
            this.m_numSliecIndex.TabIndex = 1;
            // 
            // m_txtInstanceName
            // 
            this.m_txtInstanceName.Location = new System.Drawing.Point(12, 87);
            this.m_txtInstanceName.Name = "m_txtInstanceName";
            this.m_txtInstanceName.Size = new System.Drawing.Size(266, 20);
            this.m_txtInstanceName.TabIndex = 3;
            this.m_txtInstanceName.Text = "InstanceLoop[].inst_pass_lut";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Slice Index";
            // 
            // InstanceName
            // 
            this.InstanceName.AutoSize = true;
            this.InstanceName.Location = new System.Drawing.Point(12, 71);
            this.InstanceName.Name = "InstanceName";
            this.InstanceName.Size = new System.Drawing.Size(76, 13);
            this.InstanceName.TabIndex = 6;
            this.InstanceName.Text = "InstanceName";
            // 
            // m_fileSelCtrlUCF
            // 
            this.m_fileSelCtrlUCF.Append = true;
            this.m_fileSelCtrlUCF.FileName = "";
            this.m_fileSelCtrlUCF.Filter = "All files|*.UCF";
            this.m_fileSelCtrlUCF.Label = "Label";
            this.m_fileSelCtrlUCF.Location = new System.Drawing.Point(12, 144);
            this.m_fileSelCtrlUCF.Name = "m_fileSelCtrlUCF";
            this.m_fileSelCtrlUCF.Size = new System.Drawing.Size(266, 61);
            this.m_fileSelCtrlUCF.TabIndex = 7;
            // 
            // LocationConstraintsGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 240);
            this.Controls.Add(this.m_fileSelCtrlUCF);
            this.Controls.Add(this.m_numSliecIndex);
            this.Controls.Add(this.m_txtInstanceName);
            this.Controls.Add(this.InstanceName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LocationConstraintsGUI";
            this.Text = "Location Constraints";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LocationConstraintsGUI_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.m_numSliecIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnGenerate;
        private System.Windows.Forms.NumericUpDown m_numSliecIndex;
        private System.Windows.Forms.TextBox m_txtInstanceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label InstanceName;
        private FileSelectionCtrl m_fileSelCtrlUCF;
    }
}