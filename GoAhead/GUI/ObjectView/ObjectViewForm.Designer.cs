namespace GoAhead.GUI.ObjectView
{
    partial class ObjectViewForm
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
            this.m_objView = new GoAhead.GUI.ObjectViewCtrl();
            this.SuspendLayout();
            // 
            // m_objView
            // 
            this.m_objView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_objView.Location = new System.Drawing.Point(0, 0);
            this.m_objView.Name = "m_objView";
            this.m_objView.Object = null;
            this.m_objView.Size = new System.Drawing.Size(444, 377);
            this.m_objView.TabIndex = 0;
            // 
            // ObjectViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 377);
            this.Controls.Add(this.m_objView);
            this.Name = "ObjectViewForm";
            this.Text = "Object View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ObjectViewForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ObjectViewCtrl m_objView;
    }
}