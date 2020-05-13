namespace GoAhead.GUI
{
    partial class DeviceInfoForm
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
            this.m_lblDeviceInfoHeadline = new System.Windows.Forms.Label();
            this.m_lblDeviceInfoString = new System.Windows.Forms.Label();
            this.m_lblFamiliyInfoString = new System.Windows.Forms.Label();
            this.m_lblCLBCount = new System.Windows.Forms.Label();
            this.m_lblDSPCount = new System.Windows.Forms.Label();
            this.m_lblBRAMCount = new System.Windows.Forms.Label();
            this.m_lblResources = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_lblDeviceInfoHeadline
            // 
            this.m_lblDeviceInfoHeadline.AutoSize = true;
            this.m_lblDeviceInfoHeadline.Location = new System.Drawing.Point(27, 13);
            this.m_lblDeviceInfoHeadline.Name = "m_lblDeviceInfoHeadline";
            this.m_lblDeviceInfoHeadline.Size = new System.Drawing.Size(62, 13);
            this.m_lblDeviceInfoHeadline.TabIndex = 0;
            this.m_lblDeviceInfoHeadline.Text = "Device Info";
            // 
            // m_lblDeviceInfoString
            // 
            this.m_lblDeviceInfoString.AutoSize = true;
            this.m_lblDeviceInfoString.Location = new System.Drawing.Point(147, 36);
            this.m_lblDeviceInfoString.Name = "m_lblDeviceInfoString";
            this.m_lblDeviceInfoString.Size = new System.Drawing.Size(87, 13);
            this.m_lblDeviceInfoString.TabIndex = 1;
            this.m_lblDeviceInfoString.Text = "No FPGA loaded";
            // 
            // m_lblFamiliyInfoString
            // 
            this.m_lblFamiliyInfoString.AutoSize = true;
            this.m_lblFamiliyInfoString.Location = new System.Drawing.Point(147, 13);
            this.m_lblFamiliyInfoString.Name = "m_lblFamiliyInfoString";
            this.m_lblFamiliyInfoString.Size = new System.Drawing.Size(87, 13);
            this.m_lblFamiliyInfoString.TabIndex = 2;
            this.m_lblFamiliyInfoString.Text = "No FPGA loaded";
            // 
            // m_lblCLBCount
            // 
            this.m_lblCLBCount.AutoSize = true;
            this.m_lblCLBCount.Location = new System.Drawing.Point(147, 67);
            this.m_lblCLBCount.Name = "m_lblCLBCount";
            this.m_lblCLBCount.Size = new System.Drawing.Size(87, 13);
            this.m_lblCLBCount.TabIndex = 4;
            this.m_lblCLBCount.Text = "No FPGA loaded";
            // 
            // m_lblDSPCount
            // 
            this.m_lblDSPCount.AutoSize = true;
            this.m_lblDSPCount.Location = new System.Drawing.Point(147, 90);
            this.m_lblDSPCount.Name = "m_lblDSPCount";
            this.m_lblDSPCount.Size = new System.Drawing.Size(87, 13);
            this.m_lblDSPCount.TabIndex = 5;
            this.m_lblDSPCount.Text = "No FPGA loaded";
            // 
            // m_lblBRAMCount
            // 
            this.m_lblBRAMCount.AutoSize = true;
            this.m_lblBRAMCount.Location = new System.Drawing.Point(147, 112);
            this.m_lblBRAMCount.Name = "m_lblBRAMCount";
            this.m_lblBRAMCount.Size = new System.Drawing.Size(87, 13);
            this.m_lblBRAMCount.TabIndex = 6;
            this.m_lblBRAMCount.Text = "No FPGA loaded";
            // 
            // m_lblResources
            // 
            this.m_lblResources.AutoSize = true;
            this.m_lblResources.Location = new System.Drawing.Point(27, 67);
            this.m_lblResources.Name = "m_lblResources";
            this.m_lblResources.Size = new System.Drawing.Size(58, 13);
            this.m_lblResources.TabIndex = 3;
            this.m_lblResources.Text = "Resources";
            // 
            // DeviceInfoForm
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 142);
            this.Controls.Add(this.m_lblBRAMCount);
            this.Controls.Add(this.m_lblDSPCount);
            this.Controls.Add(this.m_lblCLBCount);
            this.Controls.Add(this.m_lblResources);
            this.Controls.Add(this.m_lblFamiliyInfoString);
            this.Controls.Add(this.m_lblDeviceInfoString);
            this.Controls.Add(this.m_lblDeviceInfoHeadline);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceInfoForm";
            this.Text = "FPGA Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeviceInfo_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblDeviceInfoHeadline;
        private System.Windows.Forms.Label m_lblDeviceInfoString;
        private System.Windows.Forms.Label m_lblFamiliyInfoString;
        private System.Windows.Forms.Label m_lblCLBCount;
        private System.Windows.Forms.Label m_lblDSPCount;
        private System.Windows.Forms.Label m_lblBRAMCount;
        private System.Windows.Forms.Label m_lblResources;
    }
}