namespace GoAhead.GUI
{
    partial class NetlistContainerForm
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
            this.m_lblSelectedNetlistContainer = new System.Windows.Forms.Label();
            this.m_chkXDLIncludePorts = new System.Windows.Forms.CheckBox();
            this.m_chkRunFEScript = new System.Windows.Forms.CheckBox();
            this.m_chkXDLIncludeDummyNets = new System.Windows.Forms.CheckBox();
            this.m_fileSelectionCtrl = new GoAhead.GUI.FileSelectionCtrl();
            this.SuspendLayout();
            // 
            // m_chkListMacros
            // 
            this.m_chkListMacros.CheckOnClick = true;
            this.m_chkListMacros.FormattingEnabled = true;
            this.m_chkListMacros.Location = new System.Drawing.Point(12, 25);
            this.m_chkListMacros.Name = "m_chkListMacros";
            this.m_chkListMacros.Size = new System.Drawing.Size(199, 169);
            this.m_chkListMacros.TabIndex = 0;
            // 
            // m_btnGenerate
            // 
            this.m_btnGenerate.Location = new System.Drawing.Point(59, 422);
            this.m_btnGenerate.Name = "m_btnGenerate";
            this.m_btnGenerate.Size = new System.Drawing.Size(93, 23);
            this.m_btnGenerate.TabIndex = 1;
            this.m_btnGenerate.Text = "&Generate";
            this.m_btnGenerate.UseVisualStyleBackColor = true;
            this.m_btnGenerate.Click += new System.EventHandler(this.m_btnGenerate_Click);
            // 
            // m_lblSelectedNetlistContainer
            // 
            this.m_lblSelectedNetlistContainer.AutoSize = true;
            this.m_lblSelectedNetlistContainer.Location = new System.Drawing.Point(9, 9);
            this.m_lblSelectedNetlistContainer.Name = "m_lblSelectedNetlistContainer";
            this.m_lblSelectedNetlistContainer.Size = new System.Drawing.Size(117, 13);
            this.m_lblSelectedNetlistContainer.TabIndex = 2;
            this.m_lblSelectedNetlistContainer.Text = "Select Netlist Container";
            // 
            // m_chkXDLIncludePorts
            // 
            this.m_chkXDLIncludePorts.AutoSize = true;
            this.m_chkXDLIncludePorts.Checked = true;
            this.m_chkXDLIncludePorts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkXDLIncludePorts.Location = new System.Drawing.Point(15, 280);
            this.m_chkXDLIncludePorts.Name = "m_chkXDLIncludePorts";
            this.m_chkXDLIncludePorts.Size = new System.Drawing.Size(112, 17);
            this.m_chkXDLIncludePorts.TabIndex = 6;
            this.m_chkXDLIncludePorts.Text = "Include XDL Ports";
            this.m_chkXDLIncludePorts.UseVisualStyleBackColor = true;
            // 
            // m_chkRunFEScript
            // 
            this.m_chkRunFEScript.AutoSize = true;
            this.m_chkRunFEScript.Checked = true;
            this.m_chkRunFEScript.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkRunFEScript.Location = new System.Drawing.Point(15, 324);
            this.m_chkRunFEScript.Name = "m_chkRunFEScript";
            this.m_chkRunFEScript.Size = new System.Drawing.Size(137, 17);
            this.m_chkRunFEScript.TabIndex = 7;
            this.m_chkRunFEScript.Text = "Run FPGA-Editor Script";
            this.m_chkRunFEScript.UseVisualStyleBackColor = true;
            // 
            // m_chkXDLIncludeDummyNets
            // 
            this.m_chkXDLIncludeDummyNets.AutoSize = true;
            this.m_chkXDLIncludeDummyNets.Checked = true;
            this.m_chkXDLIncludeDummyNets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkXDLIncludeDummyNets.Location = new System.Drawing.Point(15, 301);
            this.m_chkXDLIncludeDummyNets.Name = "m_chkXDLIncludeDummyNets";
            this.m_chkXDLIncludeDummyNets.Size = new System.Drawing.Size(124, 17);
            this.m_chkXDLIncludeDummyNets.TabIndex = 8;
            this.m_chkXDLIncludeDummyNets.Text = "Include Dummy Nets";
            this.m_chkXDLIncludeDummyNets.UseVisualStyleBackColor = true;
            // 
            // m_fileSelectionCtrl
            // 
            this.m_fileSelectionCtrl.Append = true;
            this.m_fileSelectionCtrl.FileName = "";
            this.m_fileSelectionCtrl.Filter = "All files|*.*";
            this.m_fileSelectionCtrl.Label = "Label";
            this.m_fileSelectionCtrl.Location = new System.Drawing.Point(15, 201);
            this.m_fileSelectionCtrl.Name = "m_fileSelectionCtrl";
            this.m_fileSelectionCtrl.Size = new System.Drawing.Size(196, 61);
            this.m_fileSelectionCtrl.TabIndex = 9;
            // 
            // NetlistContainerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 453);
            this.Controls.Add(this.m_fileSelectionCtrl);
            this.Controls.Add(this.m_chkXDLIncludeDummyNets);
            this.Controls.Add(this.m_chkRunFEScript);
            this.Controls.Add(this.m_chkXDLIncludePorts);
            this.Controls.Add(this.m_chkListMacros);
            this.Controls.Add(this.m_btnGenerate);
            this.Controls.Add(this.m_lblSelectedNetlistContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetlistContainerForm";
            this.ShowInTaskbar = false;
            this.Text = "Generation";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MacroSelectionForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.CheckedListBox m_chkListMacros;
        protected System.Windows.Forms.Button m_btnGenerate;
        protected System.Windows.Forms.Label m_lblSelectedNetlistContainer;
        protected System.Windows.Forms.CheckBox m_chkXDLIncludePorts;
        protected System.Windows.Forms.CheckBox m_chkRunFEScript;
        protected System.Windows.Forms.CheckBox m_chkXDLIncludeDummyNets;
        private FileSelectionCtrl m_fileSelectionCtrl;
    }
}