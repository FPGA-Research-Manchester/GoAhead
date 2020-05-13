namespace GoAhead.GUI.AddLibraryManager
{
    partial class AddXDLLibraryCtrl
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
            this.m_btnBrowse = new System.Windows.Forms.Button();
            this.m_lblFile = new System.Windows.Forms.Label();
            this.m_btnAdd = new System.Windows.Forms.Button();
            this.m_txtXDLMacro = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_btnBrowse
            // 
            this.m_btnBrowse.Location = new System.Drawing.Point(212, 19);
            this.m_btnBrowse.Name = "m_btnBrowse";
            this.m_btnBrowse.Size = new System.Drawing.Size(75, 20);
            this.m_btnBrowse.TabIndex = 0;
            this.m_btnBrowse.Text = "Browse";
            this.m_btnBrowse.UseVisualStyleBackColor = true;
            this.m_btnBrowse.Click += new System.EventHandler(this.m_btnBrowse_Click);
            // 
            // m_lblFile
            // 
            this.m_lblFile.AutoSize = true;
            this.m_lblFile.Location = new System.Drawing.Point(3, 3);
            this.m_lblFile.Name = "m_lblFile";
            this.m_lblFile.Size = new System.Drawing.Size(78, 13);
            this.m_lblFile.TabIndex = 3;
            this.m_lblFile.Text = "XDL File Name";
            // 
            // m_btnAdd
            // 
            this.m_btnAdd.Location = new System.Drawing.Point(212, 60);
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(75, 23);
            this.m_btnAdd.TabIndex = 5;
            this.m_btnAdd.Text = "Add";
            this.m_btnAdd.UseVisualStyleBackColor = true;
            this.m_btnAdd.Click += new System.EventHandler(this.m_btnAdd_Click);
            // 
            // m_txtXDLMacro
            // 
            this.m_txtXDLMacro.Location = new System.Drawing.Point(6, 19);
            this.m_txtXDLMacro.Name = "m_txtXDLMacro";
            this.m_txtXDLMacro.Size = new System.Drawing.Size(200, 20);
            this.m_txtXDLMacro.TabIndex = 6;
            // 
            // AddXDLLibraryCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnAdd);
            this.Controls.Add(this.m_btnBrowse);
            this.Controls.Add(this.m_txtXDLMacro);
            this.Controls.Add(this.m_lblFile);
            this.Name = "AddXDLLibraryCtrl";
            this.Size = new System.Drawing.Size(298, 90);
            this.Load += new System.EventHandler(this.AddMacroCtrl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btnBrowse;
        private System.Windows.Forms.Label m_lblFile;
        private System.Windows.Forms.Button m_btnAdd;
        private System.Windows.Forms.TextBox m_txtXDLMacro;
    }
}
