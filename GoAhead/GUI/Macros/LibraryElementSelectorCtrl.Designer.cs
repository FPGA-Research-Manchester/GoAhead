namespace GoAhead.GUI.Macros.LibraryManager
{
    partial class LibraryElementSelectorCtrl
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
            this.m_lblElements = new System.Windows.Forms.Label();
            this.m_cmbBoxLibraryElementNames = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_lblElements
            // 
            this.m_lblElements.AutoSize = true;
            this.m_lblElements.Location = new System.Drawing.Point(0, 5);
            this.m_lblElements.Name = "m_lblElements";
            this.m_lblElements.Size = new System.Drawing.Size(84, 13);
            this.m_lblElements.TabIndex = 3;
            this.m_lblElements.Text = "Library Elements";
            // 
            // m_cmbBoxLibraryElementNames
            // 
            this.m_cmbBoxLibraryElementNames.FormattingEnabled = true;
            this.m_cmbBoxLibraryElementNames.Location = new System.Drawing.Point(3, 20);
            this.m_cmbBoxLibraryElementNames.Name = "m_cmbBoxLibraryElementNames";
            this.m_cmbBoxLibraryElementNames.Size = new System.Drawing.Size(165, 21);
            this.m_cmbBoxLibraryElementNames.TabIndex = 4;
            // 
            // LibraryElementSelectorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cmbBoxLibraryElementNames);
            this.Controls.Add(this.m_lblElements);
            this.Name = "LibraryElementSelectorCtrl";
            this.Size = new System.Drawing.Size(174, 46);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_lblElements;
        private System.Windows.Forms.ComboBox m_cmbBoxLibraryElementNames;

    }
}
