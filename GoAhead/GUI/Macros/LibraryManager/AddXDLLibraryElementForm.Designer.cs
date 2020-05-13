namespace GoAhead.GUI.AddLibraryManager.AddMacro
{
    partial class AddXDLLibraryElementForm
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
            this.m_addMacroCtrl = new GoAhead.GUI.AddLibraryManager.AddXDLLibraryCtrl();
            this.SuspendLayout();
            // 
            // m_addMacroCtrl
            // 
            this.m_addMacroCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_addMacroCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_addMacroCtrl.Name = "m_addMacroCtrl";
            this.m_addMacroCtrl.Size = new System.Drawing.Size(294, 91);
            this.m_addMacroCtrl.TabIndex = 0;
            // 
            // AddMacroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 91);
            this.Controls.Add(this.m_addMacroCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddMacroForm";
            this.Text = "Add XDL Library Element";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddMacroForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private AddXDLLibraryCtrl m_addMacroCtrl;
    }
}