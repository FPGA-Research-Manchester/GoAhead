namespace GoAhead.GUI.Macros.XDLGeneration
{
    partial class XDLGenerationCtrl
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
            this.m_fileSel = new GoAhead.GUI.FileSelectionCtrl();
            this.m_checkedListBoxNetlistContainer = new System.Windows.Forms.CheckedListBox();
            this.m_chkIncludePorts = new System.Windows.Forms.CheckBox();
            this.m_chkIncludeDummyNets = new System.Windows.Forms.CheckBox();
            this.m_chkIncludeDesignStatement = new System.Windows.Forms.CheckBox();
            this.m_chkIncludeHeader = new System.Windows.Forms.CheckBox();
            this.m_chkSort = new System.Windows.Forms.CheckBox();
            this.m_chkIncludeFooter = new System.Windows.Forms.CheckBox();
            this.m_txtDesignName = new System.Windows.Forms.TextBox();
            this.m_lblDesignName = new System.Windows.Forms.Label();
            this.m_btnGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_fileSel
            // 
            this.m_fileSel.Append = true;
            this.m_fileSel.FileName = "";
            this.m_fileSel.Filter = "All XDL files|*.xdl";
            this.m_fileSel.Label = "Label";
            this.m_fileSel.Location = new System.Drawing.Point(3, 128);
            this.m_fileSel.Name = "m_fileSel";
            this.m_fileSel.Size = new System.Drawing.Size(260, 61);
            this.m_fileSel.TabIndex = 0;
            // 
            // m_checkedListBoxNetlistContainer
            // 
            this.m_checkedListBoxNetlistContainer.FormattingEnabled = true;
            this.m_checkedListBoxNetlistContainer.Location = new System.Drawing.Point(4, 4);
            this.m_checkedListBoxNetlistContainer.Name = "m_checkedListBoxNetlistContainer";
            this.m_checkedListBoxNetlistContainer.Size = new System.Drawing.Size(259, 124);
            this.m_checkedListBoxNetlistContainer.TabIndex = 1;
            // 
            // m_chkIncludePorts
            // 
            this.m_chkIncludePorts.AutoSize = true;
            this.m_chkIncludePorts.Location = new System.Drawing.Point(4, 209);
            this.m_chkIncludePorts.Name = "m_chkIncludePorts";
            this.m_chkIncludePorts.Size = new System.Drawing.Size(88, 17);
            this.m_chkIncludePorts.TabIndex = 2;
            this.m_chkIncludePorts.Text = "Include Ports";
            this.m_chkIncludePorts.UseVisualStyleBackColor = true;
            // 
            // m_chkIncludeDummyNets
            // 
            this.m_chkIncludeDummyNets.Location = new System.Drawing.Point(4, 232);
            this.m_chkIncludeDummyNets.Name = "m_chkIncludeDummyNets";
            this.m_chkIncludeDummyNets.Size = new System.Drawing.Size(165, 17);
            this.m_chkIncludeDummyNets.TabIndex = 3;
            this.m_chkIncludeDummyNets.Text = "Include Dummy Nets";
            this.m_chkIncludeDummyNets.UseVisualStyleBackColor = true;
            // 
            // m_chkIncludeDesignStatement
            // 
            this.m_chkIncludeDesignStatement.AutoSize = true;
            this.m_chkIncludeDesignStatement.Location = new System.Drawing.Point(4, 255);
            this.m_chkIncludeDesignStatement.Name = "m_chkIncludeDesignStatement";
            this.m_chkIncludeDesignStatement.Size = new System.Drawing.Size(148, 17);
            this.m_chkIncludeDesignStatement.TabIndex = 4;
            this.m_chkIncludeDesignStatement.Text = "Include Design Statement";
            this.m_chkIncludeDesignStatement.UseVisualStyleBackColor = true;
            // 
            // m_chkIncludeHeader
            // 
            this.m_chkIncludeHeader.AutoSize = true;
            this.m_chkIncludeHeader.Checked = true;
            this.m_chkIncludeHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkIncludeHeader.Location = new System.Drawing.Point(4, 278);
            this.m_chkIncludeHeader.Name = "m_chkIncludeHeader";
            this.m_chkIncludeHeader.Size = new System.Drawing.Size(137, 17);
            this.m_chkIncludeHeader.TabIndex = 5;
            this.m_chkIncludeHeader.Text = "Include Module Header";
            this.m_chkIncludeHeader.UseVisualStyleBackColor = true;
            // 
            // m_chkSort
            // 
            this.m_chkSort.AutoSize = true;
            this.m_chkSort.Location = new System.Drawing.Point(4, 324);
            this.m_chkSort.Name = "m_chkSort";
            this.m_chkSort.Size = new System.Drawing.Size(165, 17);
            this.m_chkSort.TabIndex = 7;
            this.m_chkSort.Text = "Sort Instances by Slice Name";
            this.m_chkSort.UseVisualStyleBackColor = true;
            // 
            // m_chkIncludeFooter
            // 
            this.m_chkIncludeFooter.AutoSize = true;
            this.m_chkIncludeFooter.Location = new System.Drawing.Point(4, 301);
            this.m_chkIncludeFooter.Name = "m_chkIncludeFooter";
            this.m_chkIncludeFooter.Size = new System.Drawing.Size(132, 17);
            this.m_chkIncludeFooter.TabIndex = 6;
            this.m_chkIncludeFooter.Text = "Include Module Footer";
            this.m_chkIncludeFooter.UseVisualStyleBackColor = true;
            // 
            // m_txtDesignName
            // 
            this.m_txtDesignName.Location = new System.Drawing.Point(4, 361);
            this.m_txtDesignName.Name = "m_txtDesignName";
            this.m_txtDesignName.Size = new System.Drawing.Size(259, 20);
            this.m_txtDesignName.TabIndex = 8;
            this.m_txtDesignName.Text = "__XILINX_NMC_MACRO";
            // 
            // m_lblDesignName
            // 
            this.m_lblDesignName.AutoSize = true;
            this.m_lblDesignName.Location = new System.Drawing.Point(3, 342);
            this.m_lblDesignName.Name = "m_lblDesignName";
            this.m_lblDesignName.Size = new System.Drawing.Size(71, 13);
            this.m_lblDesignName.TabIndex = 9;
            this.m_lblDesignName.Text = "Design Name";
            // 
            // m_btnGenerate
            // 
            this.m_btnGenerate.Location = new System.Drawing.Point(96, 387);
            this.m_btnGenerate.Name = "m_btnGenerate";
            this.m_btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.m_btnGenerate.TabIndex = 10;
            this.m_btnGenerate.Text = "Generate";
            this.m_btnGenerate.UseVisualStyleBackColor = true;
            this.m_btnGenerate.Click += new System.EventHandler(this.m_btnGenerate_Click);
            // 
            // XDLGenerationCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnGenerate);
            this.Controls.Add(this.m_lblDesignName);
            this.Controls.Add(this.m_txtDesignName);
            this.Controls.Add(this.m_chkSort);
            this.Controls.Add(this.m_chkIncludeFooter);
            this.Controls.Add(this.m_chkIncludeHeader);
            this.Controls.Add(this.m_chkIncludeDesignStatement);
            this.Controls.Add(this.m_chkIncludeDummyNets);
            this.Controls.Add(this.m_chkIncludePorts);
            this.Controls.Add(this.m_checkedListBoxNetlistContainer);
            this.Controls.Add(this.m_fileSel);
            this.Name = "XDLGenerationCtrl";
            this.Size = new System.Drawing.Size(270, 417);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FileSelectionCtrl m_fileSel;
        private System.Windows.Forms.CheckedListBox m_checkedListBoxNetlistContainer;
        private System.Windows.Forms.CheckBox m_chkIncludePorts;
        private System.Windows.Forms.CheckBox m_chkIncludeDummyNets;
        private System.Windows.Forms.CheckBox m_chkIncludeDesignStatement;
        private System.Windows.Forms.CheckBox m_chkIncludeHeader;
        private System.Windows.Forms.CheckBox m_chkSort;
        private System.Windows.Forms.CheckBox m_chkIncludeFooter;
        private System.Windows.Forms.TextBox m_txtDesignName;
        private System.Windows.Forms.Label m_lblDesignName;
        private System.Windows.Forms.Button m_btnGenerate;
    }
}
