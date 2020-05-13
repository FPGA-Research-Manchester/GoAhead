namespace GoAhead.GUI
{
    partial class ObjectViewCtrl
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
            this.m_txtToString = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // m_txtToString
            // 
            this.m_txtToString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtToString.Location = new System.Drawing.Point(0, 0);
            this.m_txtToString.Name = "m_txtToString";
            this.m_txtToString.ReadOnly = true;
            this.m_txtToString.Size = new System.Drawing.Size(404, 328);
            this.m_txtToString.TabIndex = 0;
            this.m_txtToString.Text = "";
            // 
            // ObjectView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_txtToString);
            this.Name = "ObjectView";
            this.Size = new System.Drawing.Size(404, 328);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox m_txtToString;
    }
}
