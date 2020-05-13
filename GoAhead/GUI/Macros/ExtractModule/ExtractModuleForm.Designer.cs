namespace GoAhead.GUI.ExtractModules
{
    partial class ExtractModuleForm
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
            this.m_ectractModule = new GoAhead.GUI.ExtractModules.ExtractModuleCtrl();
            this.SuspendLayout();
            // 
            // m_ectractModule
            // 
            this.m_ectractModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ectractModule.Location = new System.Drawing.Point(0, 0);
            this.m_ectractModule.ModuleSource = GoAhead.GUI.ExtractModules.ExtractModuleForm.ModuleSourceType.FromNetlist;
            this.m_ectractModule.Name = "m_ectractModule";
            this.m_ectractModule.Size = new System.Drawing.Size(267, 241);
            this.m_ectractModule.TabIndex = 0;
            // 
            // ExtractModuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 241);
            this.Controls.Add(this.m_ectractModule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtractModuleForm";
            this.Text = "Extract Module";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CutOffFromDesignForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtractModuleCtrl m_ectractModule;
    }
}