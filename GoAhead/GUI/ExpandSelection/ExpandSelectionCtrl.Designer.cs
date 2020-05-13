namespace GoAhead.GUI.ExpandSelection
{
    partial class ExpandSelectionCtrl
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
            this.m_cmbBoxUserSel = new System.Windows.Forms.ComboBox();
            this.m_btnOK = new System.Windows.Forms.Button();
            this.m_lblUserSelection = new System.Windows.Forms.Label();
            this.m_rbBtnLeft = new System.Windows.Forms.RadioButton();
            this.m_rbBtnRight = new System.Windows.Forms.RadioButton();
            this.m_rbBtnLeftRight = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // m_cmbBoxUserSel
            // 
            this.m_cmbBoxUserSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cmbBoxUserSel.FormattingEnabled = true;
            this.m_cmbBoxUserSel.Location = new System.Drawing.Point(6, 25);
            this.m_cmbBoxUserSel.Name = "m_cmbBoxUserSel";
            this.m_cmbBoxUserSel.Size = new System.Drawing.Size(181, 21);
            this.m_cmbBoxUserSel.TabIndex = 0;
            // 
            // m_btnOK
            // 
            this.m_btnOK.Location = new System.Drawing.Point(62, 118);
            this.m_btnOK.Name = "m_btnOK";
            this.m_btnOK.Size = new System.Drawing.Size(75, 23);
            this.m_btnOK.TabIndex = 1;
            this.m_btnOK.Text = "Expand";
            this.m_btnOK.UseVisualStyleBackColor = true;
            this.m_btnOK.Click += new System.EventHandler(this.m_btnOK_Click);
            // 
            // m_lblUserSelection
            // 
            this.m_lblUserSelection.AutoSize = true;
            this.m_lblUserSelection.Location = new System.Drawing.Point(3, 9);
            this.m_lblUserSelection.Name = "m_lblUserSelection";
            this.m_lblUserSelection.Size = new System.Drawing.Size(76, 13);
            this.m_lblUserSelection.TabIndex = 2;
            this.m_lblUserSelection.Text = "User Selection";
            // 
            // m_rbBtnLeft
            // 
            this.m_rbBtnLeft.AutoSize = true;
            this.m_rbBtnLeft.Checked = true;
            this.m_rbBtnLeft.Location = new System.Drawing.Point(6, 52);
            this.m_rbBtnLeft.Name = "m_rbBtnLeft";
            this.m_rbBtnLeft.Size = new System.Drawing.Size(82, 17);
            this.m_rbBtnLeft.TabIndex = 3;
            this.m_rbBtnLeft.TabStop = true;
            this.m_rbBtnLeft.Text = "Expand Left";
            this.m_rbBtnLeft.UseVisualStyleBackColor = true;
            // 
            // m_rbBtnRight
            // 
            this.m_rbBtnRight.AutoSize = true;
            this.m_rbBtnRight.Location = new System.Drawing.Point(6, 72);
            this.m_rbBtnRight.Name = "m_rbBtnRight";
            this.m_rbBtnRight.Size = new System.Drawing.Size(89, 17);
            this.m_rbBtnRight.TabIndex = 4;
            this.m_rbBtnRight.TabStop = true;
            this.m_rbBtnRight.Text = "Expand Right";
            this.m_rbBtnRight.UseVisualStyleBackColor = true;
            // 
            // m_rbBtnLeftRight
            // 
            this.m_rbBtnLeftRight.AutoSize = true;
            this.m_rbBtnLeftRight.Location = new System.Drawing.Point(6, 95);
            this.m_rbBtnLeftRight.Name = "m_rbBtnLeftRight";
            this.m_rbBtnLeftRight.Size = new System.Drawing.Size(131, 17);
            this.m_rbBtnLeftRight.TabIndex = 5;
            this.m_rbBtnLeftRight.TabStop = true;
            this.m_rbBtnLeftRight.Text = "Expand Left and Right";
            this.m_rbBtnLeftRight.UseVisualStyleBackColor = true;
            // 
            // ExpandSelectionCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_rbBtnLeftRight);
            this.Controls.Add(this.m_rbBtnRight);
            this.Controls.Add(this.m_rbBtnLeft);
            this.Controls.Add(this.m_btnOK);
            this.Controls.Add(this.m_cmbBoxUserSel);
            this.Controls.Add(this.m_lblUserSelection);
            this.Name = "ExpandSelectionCtrl";
            this.Size = new System.Drawing.Size(203, 146);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_cmbBoxUserSel;
        private System.Windows.Forms.Button m_btnOK;
        private System.Windows.Forms.Label m_lblUserSelection;
        private System.Windows.Forms.RadioButton m_rbBtnLeft;
        private System.Windows.Forms.RadioButton m_rbBtnRight;
        private System.Windows.Forms.RadioButton m_rbBtnLeftRight;
    }
}
