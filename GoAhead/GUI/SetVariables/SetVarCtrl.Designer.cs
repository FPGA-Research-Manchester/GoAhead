namespace GoAhead.GUI.SetVariables
{
    partial class SetVarCtrl
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
            this.m_lblVar = new System.Windows.Forms.Label();
            this.m_txtValue = new System.Windows.Forms.TextBox();
            this.m_lblValue = new System.Windows.Forms.Label();
            this.m_lblName = new System.Windows.Forms.Label();
            this.m_cmbBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_lblVar
            // 
            this.m_lblVar.AutoSize = true;
            this.m_lblVar.Location = new System.Drawing.Point(5, 25);
            this.m_lblVar.Name = "m_lblVar";
            this.m_lblVar.Size = new System.Drawing.Size(0, 13);
            this.m_lblVar.TabIndex = 12;
            // 
            // m_txtValue
            // 
            this.m_txtValue.Location = new System.Drawing.Point(126, 22);
            this.m_txtValue.Name = "m_txtValue";
            this.m_txtValue.Size = new System.Drawing.Size(300, 20);
            this.m_txtValue.TabIndex = 10;
            this.m_txtValue.TextChanged += new System.EventHandler(this.m_txtValue_TextChanged_1);
            // 
            // m_lblValue
            // 
            this.m_lblValue.AutoSize = true;
            this.m_lblValue.Location = new System.Drawing.Point(123, 6);
            this.m_lblValue.Name = "m_lblValue";
            this.m_lblValue.Size = new System.Drawing.Size(34, 13);
            this.m_lblValue.TabIndex = 11;
            this.m_lblValue.Text = "Value";
            // 
            // m_lblName
            // 
            this.m_lblName.AutoSize = true;
            this.m_lblName.Location = new System.Drawing.Point(5, 6);
            this.m_lblName.Name = "m_lblName";
            this.m_lblName.Size = new System.Drawing.Size(76, 13);
            this.m_lblName.TabIndex = 9;
            this.m_lblName.Text = "Variable Name";
            // 
            // m_cmbBox
            // 
            this.m_cmbBox.FormattingEnabled = true;
            this.m_cmbBox.Location = new System.Drawing.Point(305, 0);
            this.m_cmbBox.Name = "m_cmbBox";
            this.m_cmbBox.Size = new System.Drawing.Size(121, 21);
            this.m_cmbBox.TabIndex = 13;
            this.m_cmbBox.SelectedIndexChanged += new System.EventHandler(this.m_cmbBox_SelectedIndexChanged);
            // 
            // SetVarCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cmbBox);
            this.Controls.Add(this.m_lblVar);
            this.Controls.Add(this.m_txtValue);
            this.Controls.Add(this.m_lblValue);
            this.Controls.Add(this.m_lblName);
            this.Name = "SetVarCtrl";
            this.Size = new System.Drawing.Size(432, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblVar;
        private System.Windows.Forms.TextBox m_txtValue;
        private System.Windows.Forms.Label m_lblValue;
        private System.Windows.Forms.Label m_lblName;
        private System.Windows.Forms.ComboBox m_cmbBox;
    }
}
