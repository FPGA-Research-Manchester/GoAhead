namespace GoAhead.GUI.Watch
{
    partial class VariableWatchForm
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
            this.m_watch = new GoAhead.GUI.Watch.VariableWatchCtrl();
            this.SuspendLayout();
            // 
            // m_watch
            // 
            this.m_watch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_watch.Location = new System.Drawing.Point(0, 0);
            this.m_watch.Name = "m_watch";
            this.m_watch.Size = new System.Drawing.Size(292, 273);
            this.m_watch.TabIndex = 0;
            // 
            // VariableWatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.m_watch);
            this.Name = "VariableWatchForm";
            this.Text = "Variable Watch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VariableWatchForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private VariableWatchCtrl m_watch;
    }
}