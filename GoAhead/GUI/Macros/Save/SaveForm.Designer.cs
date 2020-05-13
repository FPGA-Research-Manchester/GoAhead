namespace GoAhead.GUI.AddLibraryManager.Save
{
    partial class SaveForm
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
            this.m_saveCtrl = new GoAhead.GUI.AddLibraryManager.Save.SaveCtrl();
            this.SuspendLayout();
            // 
            // m_saveCtrl
            // 
            this.m_saveCtrl.CurrentSaveType = GoAhead.GUI.AddLibraryManager.Save.SaveForm.SaveType.SaveAsDesign;
            this.m_saveCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_saveCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_saveCtrl.Name = "m_saveCtrl";
            this.m_saveCtrl.Size = new System.Drawing.Size(271, 260);
            this.m_saveCtrl.TabIndex = 0;
            // 
            // SaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 260);
            this.Controls.Add(this.m_saveCtrl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveForm";
            this.Text = "Save as Design or Macro";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SaveForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private GoAhead.GUI.AddLibraryManager.Save.SaveCtrl m_saveCtrl;
    }
}