namespace GoAhead.GUI.MacroForm
{
    partial class MacroManagerForm
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
            this.m_tabCtrlTop = new System.Windows.Forms.TabControl();
            this.m_tabManager = new System.Windows.Forms.TabPage();
            this.macroManagerCtrl1 = new GoAhead.GUI.NetlistContainerCtrl();
            this.m_tabLibrary = new System.Windows.Forms.TabPage();
            this.m_library = new GoAhead.GUI.MacroLibrary.LilbraryManagerCtrl();
            this.m_tabPlacer = new System.Windows.Forms.TabPage();
            this.m_placer = new GoAhead.GUI.MacroForm.LibraryElementPlacerCtrl();
            this.m_tabInstantiations = new System.Windows.Forms.TabPage();
            this.macroInstantiationManagerCtrl1 = new GoAhead.GUI.LibraryElementInstantiation.LibraryElementInstantiationManagerCtrl();
            this.m_tabVHDLUCF = new System.Windows.Forms.TabPage();
            this.m_printVHDLCtrl = new GoAhead.GUI.Macros.VHDL.PrintVHDLCtrl();
            this.m_tabCtrlTop.SuspendLayout();
            this.m_tabManager.SuspendLayout();
            this.m_tabLibrary.SuspendLayout();
            this.m_tabPlacer.SuspendLayout();
            this.m_tabInstantiations.SuspendLayout();
            this.m_tabVHDLUCF.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabCtrlTop
            // 
            this.m_tabCtrlTop.Controls.Add(this.m_tabManager);
            this.m_tabCtrlTop.Controls.Add(this.m_tabLibrary);
            this.m_tabCtrlTop.Controls.Add(this.m_tabPlacer);
            this.m_tabCtrlTop.Controls.Add(this.m_tabInstantiations);
            this.m_tabCtrlTop.Controls.Add(this.m_tabVHDLUCF);
            this.m_tabCtrlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabCtrlTop.Location = new System.Drawing.Point(0, 0);
            this.m_tabCtrlTop.Name = "m_tabCtrlTop";
            this.m_tabCtrlTop.SelectedIndex = 0;
            this.m_tabCtrlTop.Size = new System.Drawing.Size(732, 722);
            this.m_tabCtrlTop.TabIndex = 0;
            // 
            // m_tabManager
            // 
            this.m_tabManager.Controls.Add(this.macroManagerCtrl1);
            this.m_tabManager.Location = new System.Drawing.Point(4, 22);
            this.m_tabManager.Name = "m_tabManager";
            this.m_tabManager.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabManager.Size = new System.Drawing.Size(724, 696);
            this.m_tabManager.TabIndex = 0;
            this.m_tabManager.Text = "Manager";
            this.m_tabManager.UseVisualStyleBackColor = true;
            // 
            // macroManagerCtrl1
            // 
            this.macroManagerCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.macroManagerCtrl1.Location = new System.Drawing.Point(3, 3);
            this.macroManagerCtrl1.Name = "macroManagerCtrl1";
            this.macroManagerCtrl1.Size = new System.Drawing.Size(718, 690);
            this.macroManagerCtrl1.TabIndex = 0;
            // 
            // m_tabLibrary
            // 
            this.m_tabLibrary.Controls.Add(this.m_library);
            this.m_tabLibrary.Location = new System.Drawing.Point(4, 22);
            this.m_tabLibrary.Name = "m_tabLibrary";
            this.m_tabLibrary.Size = new System.Drawing.Size(724, 696);
            this.m_tabLibrary.TabIndex = 3;
            this.m_tabLibrary.Text = "Library";
            this.m_tabLibrary.UseVisualStyleBackColor = true;
            // 
            // m_library
            // 
            this.m_library.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_library.Location = new System.Drawing.Point(0, 0);
            this.m_library.Name = "m_library";
            this.m_library.Size = new System.Drawing.Size(192, 74);
            this.m_library.TabIndex = 0;
            // 
            // m_tabPlacer
            // 
            this.m_tabPlacer.Controls.Add(this.m_placer);
            this.m_tabPlacer.Location = new System.Drawing.Point(4, 22);
            this.m_tabPlacer.Name = "m_tabPlacer";
            this.m_tabPlacer.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabPlacer.Size = new System.Drawing.Size(724, 696);
            this.m_tabPlacer.TabIndex = 1;
            this.m_tabPlacer.Text = "Placer";
            this.m_tabPlacer.UseVisualStyleBackColor = true;
            // 
            // m_placer
            // 
            this.m_placer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_placer.Location = new System.Drawing.Point(3, 3);
            this.m_placer.Name = "m_placer";
            this.m_placer.Size = new System.Drawing.Size(718, 690);
            this.m_placer.TabIndex = 0;
            // 
            // m_tabInstantiations
            // 
            this.m_tabInstantiations.Controls.Add(this.macroInstantiationManagerCtrl1);
            this.m_tabInstantiations.Location = new System.Drawing.Point(4, 22);
            this.m_tabInstantiations.Name = "m_tabInstantiations";
            this.m_tabInstantiations.Size = new System.Drawing.Size(724, 696);
            this.m_tabInstantiations.TabIndex = 2;
            this.m_tabInstantiations.Text = "Instantiations";
            this.m_tabInstantiations.UseVisualStyleBackColor = true;
            // 
            // macroInstantiationManagerCtrl1
            // 
            this.macroInstantiationManagerCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.macroInstantiationManagerCtrl1.Location = new System.Drawing.Point(0, 0);
            this.macroInstantiationManagerCtrl1.Name = "macroInstantiationManagerCtrl1";
            this.macroInstantiationManagerCtrl1.Size = new System.Drawing.Size(192, 74);
            this.macroInstantiationManagerCtrl1.TabIndex = 0;
            // 
            // m_tabVHDLUCF
            // 
            this.m_tabVHDLUCF.Controls.Add(this.m_printVHDLCtrl);
            this.m_tabVHDLUCF.Location = new System.Drawing.Point(4, 22);
            this.m_tabVHDLUCF.Name = "m_tabVHDLUCF";
            this.m_tabVHDLUCF.Size = new System.Drawing.Size(724, 696);
            this.m_tabVHDLUCF.TabIndex = 4;
            this.m_tabVHDLUCF.Text = "VHDL / UCF";
            this.m_tabVHDLUCF.UseVisualStyleBackColor = true;
            // 
            // m_printVHDLCtrl
            // 
            this.m_printVHDLCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_printVHDLCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_printVHDLCtrl.Name = "m_printVHDLCtrl";
            this.m_printVHDLCtrl.Size = new System.Drawing.Size(192, 74);
            this.m_printVHDLCtrl.TabIndex = 0;
            // 
            // MacroManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 722);
            this.Controls.Add(this.m_tabCtrlTop);
            this.Name = "MacroManagerForm";
            this.Text = "Macro Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MacroView_FormClosed);
            this.m_tabCtrlTop.ResumeLayout(false);
            this.m_tabManager.ResumeLayout(false);
            this.m_tabLibrary.ResumeLayout(false);
            this.m_tabPlacer.ResumeLayout(false);
            this.m_tabInstantiations.ResumeLayout(false);
            this.m_tabVHDLUCF.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl m_tabCtrlTop;
        private System.Windows.Forms.TabPage m_tabManager;
        private System.Windows.Forms.TabPage m_tabPlacer;
        private System.Windows.Forms.TabPage m_tabInstantiations;
        private NetlistContainerCtrl macroManagerCtrl1;
        private LibraryElementInstantiation.LibraryElementInstantiationManagerCtrl macroInstantiationManagerCtrl1;
        private LibraryElementPlacerCtrl m_placer;
        private System.Windows.Forms.TabPage m_tabLibrary;
        private MacroLibrary.LilbraryManagerCtrl m_library;
        private System.Windows.Forms.TabPage m_tabVHDLUCF;
        private Macros.VHDL.PrintVHDLCtrl m_printVHDLCtrl;
    }
}