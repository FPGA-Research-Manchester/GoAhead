namespace GoAhead.GUI.ScriptDebugger
{
    partial class ScriptDebuggerForm
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
            this.m_scriptDebuggerCtrl = new GoAhead.GUI.ScriptDebugger.ScriptDebuggerCtrl();
            this.SuspendLayout();
            // 
            // m_scriptDebuggerCtrl
            // 
            this.m_scriptDebuggerCtrl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.m_scriptDebuggerCtrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_scriptDebuggerCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_scriptDebuggerCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_scriptDebuggerCtrl.Name = "m_scriptDebuggerCtrl";
            this.m_scriptDebuggerCtrl.Size = new System.Drawing.Size(566, 509);
            this.m_scriptDebuggerCtrl.TabIndex = 0;
            // 
            // ScriptDebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 509);
            this.Controls.Add(this.m_scriptDebuggerCtrl);
            this.Name = "ScriptDebuggerForm";
            this.Text = "Script Debugger";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScriptDebuggerForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private ScriptDebuggerCtrl m_scriptDebuggerCtrl;
    }
}