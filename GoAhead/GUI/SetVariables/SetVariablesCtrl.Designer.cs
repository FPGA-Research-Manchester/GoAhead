namespace GoAhead.GUI.SetVariables
{
    partial class SetVariablesCtrl
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
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_groupBoxCtrl = new System.Windows.Forms.GroupBox();
            this.m_txtTileIdentifier = new System.Windows.Forms.TextBox();
            this.m_lblTileIdentifier = new System.Windows.Forms.Label();
            this.m_groupBoxCtrl.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_btnOk
            // 
            this.m_btnOk.Location = new System.Drawing.Point(428, 28);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(75, 23);
            this.m_btnOk.TabIndex = 1;
            this.m_btnOk.Text = "Ok";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
            // 
            // m_groupBoxCtrl
            // 
            this.m_groupBoxCtrl.Controls.Add(this.m_txtTileIdentifier);
            this.m_groupBoxCtrl.Controls.Add(this.m_lblTileIdentifier);
            this.m_groupBoxCtrl.Controls.Add(this.m_btnOk);
            this.m_groupBoxCtrl.Location = new System.Drawing.Point(3, 3);
            this.m_groupBoxCtrl.Name = "m_groupBoxCtrl";
            this.m_groupBoxCtrl.Size = new System.Drawing.Size(518, 60);
            this.m_groupBoxCtrl.TabIndex = 2;
            this.m_groupBoxCtrl.TabStop = false;
            this.m_groupBoxCtrl.Text = "Control";
            // 
            // m_txtTileIdentifier
            // 
            this.m_txtTileIdentifier.Location = new System.Drawing.Point(12, 31);
            this.m_txtTileIdentifier.Name = "m_txtTileIdentifier";
            this.m_txtTileIdentifier.Size = new System.Drawing.Size(135, 20);
            this.m_txtTileIdentifier.TabIndex = 4;
            // 
            // m_lblTileIdentifier
            // 
            this.m_lblTileIdentifier.AutoSize = true;
            this.m_lblTileIdentifier.Location = new System.Drawing.Point(9, 15);
            this.m_lblTileIdentifier.Name = "m_lblTileIdentifier";
            this.m_lblTileIdentifier.Size = new System.Drawing.Size(67, 13);
            this.m_lblTileIdentifier.TabIndex = 3;
            this.m_lblTileIdentifier.Text = "Tile Identifier";
            // 
            // SetVariablesCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_groupBoxCtrl);
            this.Name = "SetVariablesCtrl";
            this.Size = new System.Drawing.Size(524, 428);
            this.m_groupBoxCtrl.ResumeLayout(false);
            this.m_groupBoxCtrl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.GroupBox m_groupBoxCtrl;
        private System.Windows.Forms.TextBox m_txtTileIdentifier;
        private System.Windows.Forms.Label m_lblTileIdentifier;

    }
}
