namespace GoAhead.GUI.Macros.NetlistContainerManager
{
    partial class NetlistContainerSelectorCtrl
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
            this.m_lblNetlistContainer = new System.Windows.Forms.Label();
            this.m_cmbBoxNetlistContainerNames = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_lblNetlistContainer
            // 
            this.m_lblNetlistContainer.AutoSize = true;
            this.m_lblNetlistContainer.Location = new System.Drawing.Point(0, 4);
            this.m_lblNetlistContainer.Name = "m_lblNetlistContainer";
            this.m_lblNetlistContainer.Size = new System.Drawing.Size(84, 13);
            this.m_lblNetlistContainer.TabIndex = 5;
            this.m_lblNetlistContainer.Text = "Netlist Container";
            // 
            // m_cmbBoxNetlistContainerNames
            // 
            this.m_cmbBoxNetlistContainerNames.FormattingEnabled = true;
            this.m_cmbBoxNetlistContainerNames.Location = new System.Drawing.Point(3, 20);
            this.m_cmbBoxNetlistContainerNames.Name = "m_cmbBoxNetlistContainerNames";
            this.m_cmbBoxNetlistContainerNames.Size = new System.Drawing.Size(160, 21);
            this.m_cmbBoxNetlistContainerNames.TabIndex = 6;
            // 
            // NetlistContainerSelectorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cmbBoxNetlistContainerNames);
            this.Controls.Add(this.m_lblNetlistContainer);
            this.Name = "NetlistContainerSelectorCtrl";
            this.Size = new System.Drawing.Size(165, 45);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblNetlistContainer;
        private System.Windows.Forms.ComboBox m_cmbBoxNetlistContainerNames;
    }
}
