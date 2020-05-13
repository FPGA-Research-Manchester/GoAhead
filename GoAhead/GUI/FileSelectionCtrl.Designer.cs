namespace GoAhead.GUI
{
    public partial class FileSelectionCtrl
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
            this.m_lblFile = new System.Windows.Forms.Label();
            this.m_btnBrowse = new System.Windows.Forms.Button();
            this.m_txtFile = new System.Windows.Forms.RichTextBox();
            this.m_chkBoxAppend = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // m_lblFile
            // 
            this.m_lblFile.AutoSize = true;
            this.m_lblFile.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_lblFile.Location = new System.Drawing.Point(0, 0);
            this.m_lblFile.Name = "m_lblFile";
            this.m_lblFile.Size = new System.Drawing.Size(26, 13);
            this.m_lblFile.TabIndex = 0;
            this.m_lblFile.Text = "File ";
            // 
            // m_btnBrowse
            // 
            this.m_btnBrowse.Location = new System.Drawing.Point(210, 37);
            this.m_btnBrowse.Name = "m_btnBrowse";
            this.m_btnBrowse.Size = new System.Drawing.Size(46, 21);
            this.m_btnBrowse.TabIndex = 1;
            this.m_btnBrowse.Text = "...";
            this.m_btnBrowse.UseVisualStyleBackColor = true;
            this.m_btnBrowse.Click += new System.EventHandler(this.m_btnBrowse_Click);
            // 
            // m_txtFile
            // 
            this.m_txtFile.Location = new System.Drawing.Point(0, 15);
            this.m_txtFile.Multiline = false;
            this.m_txtFile.Name = "m_txtFile";
            this.m_txtFile.Size = new System.Drawing.Size(256, 19);
            this.m_txtFile.TabIndex = 2;
            this.m_txtFile.Text = "";
            this.m_txtFile.TextChanged += new System.EventHandler(this.m_txtFile_TextChanged);
            // 
            // m_chkBoxAppend
            // 
            this.m_chkBoxAppend.AutoSize = true;
            this.m_chkBoxAppend.Checked = true;
            this.m_chkBoxAppend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkBoxAppend.Location = new System.Drawing.Point(4, 39);
            this.m_chkBoxAppend.Name = "m_chkBoxAppend";
            this.m_chkBoxAppend.Size = new System.Drawing.Size(63, 17);
            this.m_chkBoxAppend.TabIndex = 3;
            this.m_chkBoxAppend.Text = "Append";
            this.m_chkBoxAppend.UseVisualStyleBackColor = true;
            // 
            // FileSelectionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_chkBoxAppend);
            this.Controls.Add(this.m_txtFile);
            this.Controls.Add(this.m_btnBrowse);
            this.Controls.Add(this.m_lblFile);
            this.Name = "FileSelectionCtrl";
            this.Size = new System.Drawing.Size(260, 61);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblFile;
        private System.Windows.Forms.Button m_btnBrowse;
        private System.Windows.Forms.RichTextBox m_txtFile;
        private System.Windows.Forms.CheckBox m_chkBoxAppend;
    }
}
