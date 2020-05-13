namespace GoAhead.GUI.Macros.VHDL
{
    partial class PrintVHDLCtrl
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
            this.m_libElInstSelector = new GoAhead.GUI.Macros.LibraryElementInstantiation.LibraryElementInstantiationSelectorCtrl();
            this.m_grpBoxVHDL = new System.Windows.Forms.GroupBox();
            this.m_grpBoxPrintWrapperInstantiations = new System.Windows.Forms.GroupBox();
            this.m_fileSelVHDLWrapperInstantiation = new GoAhead.GUI.FileSelectionCtrl();
            this.m_btnPrintWrapperInstantiation = new System.Windows.Forms.Button();
            this.m_grpBoxPrintWrapper = new System.Windows.Forms.GroupBox();
            this.m_fileSelVHDLWrapper = new GoAhead.GUI.FileSelectionCtrl();
            this.m_btnPrintWrapper = new System.Windows.Forms.Button();
            this.m_txtEntityName = new System.Windows.Forms.TextBox();
            this.m_lblEntityName = new System.Windows.Forms.Label();
            this.m_grpBoxUCF = new System.Windows.Forms.GroupBox();
            this.m_fileSelUCF = new GoAhead.GUI.FileSelectionCtrl();
            this.m_txtHierarchyPrefix = new System.Windows.Forms.TextBox();
            this.m_lblHierarchyPrefix = new System.Windows.Forms.Label();
            this.m_btnPrintUCF = new System.Windows.Forms.Button();
            this.m_grpBoxVHDL.SuspendLayout();
            this.m_grpBoxPrintWrapperInstantiations.SuspendLayout();
            this.m_grpBoxPrintWrapper.SuspendLayout();
            this.m_grpBoxUCF.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_libElInstSelector
            // 
            this.m_libElInstSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_libElInstSelector.Location = new System.Drawing.Point(0, 0);
            this.m_libElInstSelector.Name = "m_libElInstSelector";
            this.m_libElInstSelector.Size = new System.Drawing.Size(788, 319);
            this.m_libElInstSelector.TabIndex = 16;
            // 
            // m_grpBoxVHDL
            // 
            this.m_grpBoxVHDL.Controls.Add(this.m_grpBoxPrintWrapperInstantiations);
            this.m_grpBoxVHDL.Controls.Add(this.m_grpBoxPrintWrapper);
            this.m_grpBoxVHDL.Controls.Add(this.m_txtEntityName);
            this.m_grpBoxVHDL.Controls.Add(this.m_lblEntityName);
            this.m_grpBoxVHDL.Location = new System.Drawing.Point(292, 328);
            this.m_grpBoxVHDL.Name = "m_grpBoxVHDL";
            this.m_grpBoxVHDL.Size = new System.Drawing.Size(421, 243);
            this.m_grpBoxVHDL.TabIndex = 17;
            this.m_grpBoxVHDL.TabStop = false;
            this.m_grpBoxVHDL.Text = "Print VHDL Wrapper / Instantiations";
            // 
            // m_grpBoxPrintWrapperInstantiations
            // 
            this.m_grpBoxPrintWrapperInstantiations.Controls.Add(this.m_fileSelVHDLWrapperInstantiation);
            this.m_grpBoxPrintWrapperInstantiations.Controls.Add(this.m_btnPrintWrapperInstantiation);
            this.m_grpBoxPrintWrapperInstantiations.Location = new System.Drawing.Point(8, 146);
            this.m_grpBoxPrintWrapperInstantiations.Name = "m_grpBoxPrintWrapperInstantiations";
            this.m_grpBoxPrintWrapperInstantiations.Size = new System.Drawing.Size(407, 88);
            this.m_grpBoxPrintWrapperInstantiations.TabIndex = 19;
            this.m_grpBoxPrintWrapperInstantiations.TabStop = false;
            this.m_grpBoxPrintWrapperInstantiations.Text = "Print Wrapper Instantiations";
            // 
            // m_fileSelVHDLWrapperInstantiation
            // 
            this.m_fileSelVHDLWrapperInstantiation.Append = true;
            this.m_fileSelVHDLWrapperInstantiation.FileName = "";
            this.m_fileSelVHDLWrapperInstantiation.Filter = "All VDHL files|*.vhd";
            this.m_fileSelVHDLWrapperInstantiation.Label = "VHDL Wrapper Instantiation File";
            this.m_fileSelVHDLWrapperInstantiation.Location = new System.Drawing.Point(6, 19);
            this.m_fileSelVHDLWrapperInstantiation.Name = "m_fileSelVHDLWrapperInstantiation";
            this.m_fileSelVHDLWrapperInstantiation.Size = new System.Drawing.Size(261, 61);
            this.m_fileSelVHDLWrapperInstantiation.TabIndex = 17;
            // 
            // m_btnPrintWrapperInstantiation
            // 
            this.m_btnPrintWrapperInstantiation.Location = new System.Drawing.Point(315, 57);
            this.m_btnPrintWrapperInstantiation.Name = "m_btnPrintWrapperInstantiation";
            this.m_btnPrintWrapperInstantiation.Size = new System.Drawing.Size(86, 23);
            this.m_btnPrintWrapperInstantiation.TabIndex = 16;
            this.m_btnPrintWrapperInstantiation.Text = "Print";
            this.m_btnPrintWrapperInstantiation.UseVisualStyleBackColor = true;
            this.m_btnPrintWrapperInstantiation.Click += new System.EventHandler(this.m_btnPrintWrapperInstantiation_Click);
            // 
            // m_grpBoxPrintWrapper
            // 
            this.m_grpBoxPrintWrapper.Controls.Add(this.m_fileSelVHDLWrapper);
            this.m_grpBoxPrintWrapper.Controls.Add(this.m_btnPrintWrapper);
            this.m_grpBoxPrintWrapper.Location = new System.Drawing.Point(8, 59);
            this.m_grpBoxPrintWrapper.Name = "m_grpBoxPrintWrapper";
            this.m_grpBoxPrintWrapper.Size = new System.Drawing.Size(407, 81);
            this.m_grpBoxPrintWrapper.TabIndex = 18;
            this.m_grpBoxPrintWrapper.TabStop = false;
            this.m_grpBoxPrintWrapper.Text = "Print Wrapper";
            // 
            // m_fileSelVHDLWrapper
            // 
            this.m_fileSelVHDLWrapper.Append = true;
            this.m_fileSelVHDLWrapper.FileName = "";
            this.m_fileSelVHDLWrapper.Filter = "All VDHL files|*.vhd";
            this.m_fileSelVHDLWrapper.Label = "VHDL Wrapper File";
            this.m_fileSelVHDLWrapper.Location = new System.Drawing.Point(6, 17);
            this.m_fileSelVHDLWrapper.Name = "m_fileSelVHDLWrapper";
            this.m_fileSelVHDLWrapper.Size = new System.Drawing.Size(261, 61);
            this.m_fileSelVHDLWrapper.TabIndex = 9;
            // 
            // m_btnPrintWrapper
            // 
            this.m_btnPrintWrapper.Location = new System.Drawing.Point(315, 52);
            this.m_btnPrintWrapper.Name = "m_btnPrintWrapper";
            this.m_btnPrintWrapper.Size = new System.Drawing.Size(86, 23);
            this.m_btnPrintWrapper.TabIndex = 4;
            this.m_btnPrintWrapper.Text = "Print";
            this.m_btnPrintWrapper.UseVisualStyleBackColor = true;
            this.m_btnPrintWrapper.Click += new System.EventHandler(this.m_btnPrintWrapper_Click);
            // 
            // m_txtEntityName
            // 
            this.m_txtEntityName.Location = new System.Drawing.Point(8, 33);
            this.m_txtEntityName.Name = "m_txtEntityName";
            this.m_txtEntityName.Size = new System.Drawing.Size(407, 20);
            this.m_txtEntityName.TabIndex = 14;
            this.m_txtEntityName.Text = "PartialSubsystem";
            // 
            // m_lblEntityName
            // 
            this.m_lblEntityName.AutoSize = true;
            this.m_lblEntityName.Location = new System.Drawing.Point(5, 17);
            this.m_lblEntityName.Name = "m_lblEntityName";
            this.m_lblEntityName.Size = new System.Drawing.Size(64, 13);
            this.m_lblEntityName.TabIndex = 15;
            this.m_lblEntityName.Text = "Entity Name";
            // 
            // m_grpBoxUCF
            // 
            this.m_grpBoxUCF.Controls.Add(this.m_fileSelUCF);
            this.m_grpBoxUCF.Controls.Add(this.m_txtHierarchyPrefix);
            this.m_grpBoxUCF.Controls.Add(this.m_lblHierarchyPrefix);
            this.m_grpBoxUCF.Controls.Add(this.m_btnPrintUCF);
            this.m_grpBoxUCF.Location = new System.Drawing.Point(4, 328);
            this.m_grpBoxUCF.Name = "m_grpBoxUCF";
            this.m_grpBoxUCF.Size = new System.Drawing.Size(282, 167);
            this.m_grpBoxUCF.TabIndex = 18;
            this.m_grpBoxUCF.TabStop = false;
            this.m_grpBoxUCF.Text = "Print UCF Placement Constraints";
            // 
            // m_fileSelUCF
            // 
            this.m_fileSelUCF.Append = true;
            this.m_fileSelUCF.FileName = "";
            this.m_fileSelUCF.Filter = "All UCF files|*.ucf";
            this.m_fileSelUCF.Label = "UCF File";
            this.m_fileSelUCF.Location = new System.Drawing.Point(12, 62);
            this.m_fileSelUCF.Name = "m_fileSelUCF";
            this.m_fileSelUCF.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelUCF.TabIndex = 10;
            // 
            // m_txtHierarchyPrefix
            // 
            this.m_txtHierarchyPrefix.Location = new System.Drawing.Point(12, 32);
            this.m_txtHierarchyPrefix.Name = "m_txtHierarchyPrefix";
            this.m_txtHierarchyPrefix.Size = new System.Drawing.Size(147, 20);
            this.m_txtHierarchyPrefix.TabIndex = 5;
            // 
            // m_lblHierarchyPrefix
            // 
            this.m_lblHierarchyPrefix.AutoSize = true;
            this.m_lblHierarchyPrefix.Location = new System.Drawing.Point(9, 16);
            this.m_lblHierarchyPrefix.Name = "m_lblHierarchyPrefix";
            this.m_lblHierarchyPrefix.Size = new System.Drawing.Size(81, 13);
            this.m_lblHierarchyPrefix.TabIndex = 4;
            this.m_lblHierarchyPrefix.Text = "Hierarchy Prefix";
            // 
            // m_btnPrintUCF
            // 
            this.m_btnPrintUCF.Location = new System.Drawing.Point(197, 137);
            this.m_btnPrintUCF.Name = "m_btnPrintUCF";
            this.m_btnPrintUCF.Size = new System.Drawing.Size(75, 23);
            this.m_btnPrintUCF.TabIndex = 1;
            this.m_btnPrintUCF.Text = "Print";
            this.m_btnPrintUCF.UseVisualStyleBackColor = true;
            this.m_btnPrintUCF.Click += new System.EventHandler(this.m_btnPrintUCF_Click);
            // 
            // PrintVHDLCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_grpBoxUCF);
            this.Controls.Add(this.m_grpBoxVHDL);
            this.Controls.Add(this.m_libElInstSelector);
            this.Name = "PrintVHDLCtrl";
            this.Size = new System.Drawing.Size(788, 577);
            this.m_grpBoxVHDL.ResumeLayout(false);
            this.m_grpBoxVHDL.PerformLayout();
            this.m_grpBoxPrintWrapperInstantiations.ResumeLayout(false);
            this.m_grpBoxPrintWrapper.ResumeLayout(false);
            this.m_grpBoxUCF.ResumeLayout(false);
            this.m_grpBoxUCF.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LibraryElementInstantiation.LibraryElementInstantiationSelectorCtrl m_libElInstSelector;
        private System.Windows.Forms.GroupBox m_grpBoxVHDL;
        private System.Windows.Forms.GroupBox m_grpBoxPrintWrapperInstantiations;
        private FileSelectionCtrl m_fileSelVHDLWrapperInstantiation;
        private System.Windows.Forms.Button m_btnPrintWrapperInstantiation;
        private System.Windows.Forms.GroupBox m_grpBoxPrintWrapper;
        private FileSelectionCtrl m_fileSelVHDLWrapper;
        private System.Windows.Forms.Button m_btnPrintWrapper;
        private System.Windows.Forms.TextBox m_txtEntityName;
        private System.Windows.Forms.Label m_lblEntityName;
        private System.Windows.Forms.GroupBox m_grpBoxUCF;
        private FileSelectionCtrl m_fileSelUCF;
        private System.Windows.Forms.TextBox m_txtHierarchyPrefix;
        private System.Windows.Forms.Label m_lblHierarchyPrefix;
        private System.Windows.Forms.Button m_btnPrintUCF;
    }
}
