namespace GoAhead.GUI
{
    partial class UserSelectionForm
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
            this.m_txtName = new System.Windows.Forms.RichTextBox();
            this.m_lblName = new System.Windows.Forms.Label();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_txtName
            // 
            this.m_txtName.Location = new System.Drawing.Point(12, 25);
            this.m_txtName.Multiline = false;
            this.m_txtName.Name = "m_txtName";
            this.m_txtName.Size = new System.Drawing.Size(220, 23);
            this.m_txtName.TabIndex = 0;
            this.m_txtName.Text = "";
            // 
            // m_lblName
            // 
            this.m_lblName.AutoSize = true;
            this.m_lblName.Location = new System.Drawing.Point(9, 9);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(107, 13);
            this.m_lblName.TabIndex = 1;
            this.m_lblName.Text = "User Selection Name";
            // 
            // m_btnOK
            // 
            this.m_btnOK.Location = new System.Drawing.Point(238, 25);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(42, 23);
            this.m_btnOK.TabIndex = 2;
            this.m_btnOK.Text = "OK";
            this.m_btnOK.UseVisualStyleBackColor = true;
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // UserSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 55);
            this.Controls.Add(this.m_txtName);
            this.Controls.Add(this.m_btnOK);
            this.Controls.Add(this.m_lblName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserSelectionForm";
            this.Text = "User Selection";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserSelectionForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox m_txtName;
        private System.Windows.Forms.Label m_lblName;
        private System.Windows.Forms.Button m_btnOK;
    }
}