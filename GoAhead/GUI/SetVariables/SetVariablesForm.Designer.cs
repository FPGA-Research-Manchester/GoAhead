namespace GoAhead.GUI.SetVariables
{
    partial class SetVariablesForm
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
            this.m_setVariables = new GoAhead.GUI.SetVariables.SetVariablesCtrl();
            this.SuspendLayout();
            // 
            // m_setVariables
            // 
            this.m_setVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_setVariables.Inputs = null;
            this.m_setVariables.Location = new System.Drawing.Point(0, 0);
            this.m_setVariables.Name = "m_setVariables";
            this.m_setVariables.Size = new System.Drawing.Size(526, 273);
            this.m_setVariables.TabIndex = 0;
            this.m_setVariables.Load += new System.EventHandler(this.m_setVariables_Load);
            // 
            // SetVariablesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 273);
            this.Controls.Add(this.m_setVariables);
            this.Name = "SetVariablesForm";
            this.Text = "Set Variables";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetVariablesForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private SetVariablesCtrl m_setVariables;
    }
}