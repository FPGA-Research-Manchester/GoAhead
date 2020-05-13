namespace GoAhead.GUI.Blocker
{
    partial class BlockerCtrl
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
            this.m_chkPrintUnblockedPorts = new System.Windows.Forms.CheckBox();
            this.m_chkBlockMarkedPortsOnly = new System.Windows.Forms.CheckBox();
            this.m_chkUseEndPips = new System.Windows.Forms.CheckBox();
            this.m_lblPrefix = new System.Windows.Forms.Label();
            this.m_txtPrefix = new System.Windows.Forms.TextBox();
            this.m_numDrpSliceNumber = new System.Windows.Forms.NumericUpDown();
            this.m_lblSliceNumber = new System.Windows.Forms.Label();
            this.m_btnBlock = new System.Windows.Forms.Button();
            this.m_grpBoxBlockParamter = new System.Windows.Forms.GroupBox();
            this.m_netlistContainerSelector = new GoAhead.GUI.Macros.NetlistContainerManager.NetlistContainerSelectorCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDrpSliceNumber)).BeginInit();
            this.m_grpBoxBlockParamter.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_chkPrintUnblockedPorts
            // 
            this.m_chkPrintUnblockedPorts.AutoSize = true;
            this.m_chkPrintUnblockedPorts.Location = new System.Drawing.Point(6, 16);
            this.m_chkPrintUnblockedPorts.Name = "m_chkPrintUnblockedPorts";
            this.m_chkPrintUnblockedPorts.Size = new System.Drawing.Size(129, 17);
            this.m_chkPrintUnblockedPorts.TabIndex = 0;
            this.m_chkPrintUnblockedPorts.Text = "Print Unblocked Ports";
            this.m_chkPrintUnblockedPorts.UseVisualStyleBackColor = true;
            // 
            // m_chkUseEndPips
            // 
            this.m_chkUseEndPips.AutoSize = true;
            this.m_chkUseEndPips.Checked = true;
            this.m_chkUseEndPips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkUseEndPips.Location = new System.Drawing.Point(6, 115);
            this.m_chkUseEndPips.Name = "m_chkUseEndPips";
            this.m_chkUseEndPips.Size = new System.Drawing.Size(123, 17);
            this.m_chkUseEndPips.TabIndex = 1;
            this.m_chkUseEndPips.Text = "Block With End Pips";
            this.m_chkUseEndPips.UseVisualStyleBackColor = true;
            // 
            // m_chkBlockMarkedPortsOnly
            // 
            this.m_chkBlockMarkedPortsOnly.AutoSize = true;
            this.m_chkBlockMarkedPortsOnly.Checked = false;
            this.m_chkBlockMarkedPortsOnly.Location = new System.Drawing.Point(6, 135);
            this.m_chkBlockMarkedPortsOnly.Name = "m_chkBlockMarkedPortsOnly";
            this.m_chkBlockMarkedPortsOnly.Size = new System.Drawing.Size(123, 17);
            this.m_chkBlockMarkedPortsOnly.TabIndex = 1;
            this.m_chkBlockMarkedPortsOnly.Text = "Block only if 'ToBeBlocked'";
            this.m_chkBlockMarkedPortsOnly.UseVisualStyleBackColor = true;
            // 
            // m_lblPrefix
            // 
            this.m_lblPrefix.AutoSize = true;
            this.m_lblPrefix.Location = new System.Drawing.Point(3, 50);
            this.m_lblPrefix.Name = "m_lblPrefix";
            this.m_lblPrefix.Size = new System.Drawing.Size(33, 13);
            this.m_lblPrefix.TabIndex = 2;
            this.m_lblPrefix.Text = "Prefix";
            // 
            // m_txtPrefix
            // 
            this.m_txtPrefix.Location = new System.Drawing.Point(6, 50);
            this.m_txtPrefix.Name = "m_txtPrefix";
            this.m_txtPrefix.Size = new System.Drawing.Size(154, 20);
            this.m_txtPrefix.TabIndex = 3;
            this.m_txtPrefix.Text = "RBB_Blocker";
            // 
            // m_numDrpSliceNumber
            // 
            this.m_numDrpSliceNumber.Location = new System.Drawing.Point(6, 89);
            this.m_numDrpSliceNumber.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numDrpSliceNumber.Name = "m_numDrpSliceNumber";
            this.m_numDrpSliceNumber.Size = new System.Drawing.Size(154, 20);
            this.m_numDrpSliceNumber.TabIndex = 4;
            // 
            // m_lblSliceNumber
            // 
            this.m_lblSliceNumber.AutoSize = true;
            this.m_lblSliceNumber.Location = new System.Drawing.Point(3, 73);
            this.m_lblSliceNumber.Name = "m_lblSliceNumber";
            this.m_lblSliceNumber.Size = new System.Drawing.Size(70, 13);
            this.m_lblSliceNumber.TabIndex = 5;
            this.m_lblSliceNumber.Text = "Slice Number";
            // 
            // m_btnBlock
            // 
            this.m_btnBlock.Location = new System.Drawing.Point(49, 215);
            this.m_btnBlock.Name = "m_btnBlock";
            this.m_btnBlock.Size = new System.Drawing.Size(75, 23);
            this.m_btnBlock.TabIndex = 6;
            this.m_btnBlock.Text = "Block";
            this.m_btnBlock.UseVisualStyleBackColor = true;
            this.m_btnBlock.Click += new System.EventHandler(this.m_btnBlock_Click);
            // 
            // m_grpBoxBlockParamter
            // 
            this.m_grpBoxBlockParamter.Controls.Add(this.m_chkPrintUnblockedPorts);
            this.m_grpBoxBlockParamter.Controls.Add(this.m_chkBlockMarkedPortsOnly);
            this.m_grpBoxBlockParamter.Controls.Add(this.m_chkUseEndPips);
            this.m_grpBoxBlockParamter.Controls.Add(this.m_numDrpSliceNumber);
            this.m_grpBoxBlockParamter.Controls.Add(this.m_txtPrefix);
            this.m_grpBoxBlockParamter.Controls.Add(this.m_lblSliceNumber);
            this.m_grpBoxBlockParamter.Controls.Add(this.m_lblPrefix);
            this.m_grpBoxBlockParamter.Location = new System.Drawing.Point(3, 3);
            this.m_grpBoxBlockParamter.Name = "m_grpBoxBlockParamter";
            this.m_grpBoxBlockParamter.Size = new System.Drawing.Size(171, 160);
            this.m_grpBoxBlockParamter.TabIndex = 8;
            this.m_grpBoxBlockParamter.TabStop = false;
            this.m_grpBoxBlockParamter.Text = "Blocker Parameter";
            // 
            // m_netlistContainerSelector
            // 
            this.m_netlistContainerSelector.Location = new System.Drawing.Point(5, 165);
            this.m_netlistContainerSelector.Name = "m_netlistContainerSelector";
            this.m_netlistContainerSelector.Size = new System.Drawing.Size(165, 45);
            this.m_netlistContainerSelector.TabIndex = 9;
            // 
            // BlockerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_netlistContainerSelector);
            this.Controls.Add(this.m_grpBoxBlockParamter);
            this.Controls.Add(this.m_btnBlock);
            this.Name = "BlockerCtrl";
            this.Size = new System.Drawing.Size(180, 270);
            ((System.ComponentModel.ISupportInitialize)(this.m_numDrpSliceNumber)).EndInit();
            this.m_grpBoxBlockParamter.ResumeLayout(false);
            this.m_grpBoxBlockParamter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox m_chkPrintUnblockedPorts;
        private System.Windows.Forms.CheckBox m_chkBlockMarkedPortsOnly;
        private System.Windows.Forms.CheckBox m_chkUseEndPips;
        private System.Windows.Forms.Label m_lblPrefix;
        private System.Windows.Forms.TextBox m_txtPrefix;
        private System.Windows.Forms.NumericUpDown m_numDrpSliceNumber;
        private System.Windows.Forms.Label m_lblSliceNumber;
        private System.Windows.Forms.Button m_btnBlock;
        private System.Windows.Forms.GroupBox m_grpBoxBlockParamter;
        private Macros.NetlistContainerManager.NetlistContainerSelectorCtrl m_netlistContainerSelector;
    }
}
