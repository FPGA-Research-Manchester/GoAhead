namespace GoAhead.GUI.AddLibraryManager.Save
{
    partial class SaveCtrl
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
            this.m_chkListBoxMacros = new System.Windows.Forms.CheckedListBox();
            this.m_lblMacro = new System.Windows.Forms.Label();
            this.m_fileSelCtrl = new GoAhead.GUI.FileSelectionCtrl();
            this.SuspendLayout();
            // 
            // m_btnOk
            // 
            this.m_btnOk.Location = new System.Drawing.Point(97, 232);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(75, 23);
            this.m_btnOk.TabIndex = 3;
            this.m_btnOk.Text = "Save";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
            // 
            // m_chkListBoxMacros
            // 
            this.m_chkListBoxMacros.CheckOnClick = true;
            this.m_chkListBoxMacros.FormattingEnabled = true;
            this.m_chkListBoxMacros.Location = new System.Drawing.Point(4, 25);
            this.m_chkListBoxMacros.Name = "m_chkListBoxMacros";
            this.m_chkListBoxMacros.Size = new System.Drawing.Size(260, 124);
            this.m_chkListBoxMacros.TabIndex = 4;
            // 
            // m_lblMacro
            // 
            this.m_lblMacro.AutoSize = true;
            this.m_lblMacro.Location = new System.Drawing.Point(3, 4);
            this.m_lblMacro.Name = "m_lblMacro";
            this.m_lblMacro.Size = new System.Drawing.Size(84, 13);
            this.m_lblMacro.TabIndex = 5;
            this.m_lblMacro.Text = "Netlist Container";
            // 
            // m_fileSelCtrl
            // 
            this.m_fileSelCtrl.Append = false;
            this.m_fileSelCtrl.FileName = "";
            this.m_fileSelCtrl.Filter = "All XDL files|*.xdl";
            this.m_fileSelCtrl.Label = "Output File";
            this.m_fileSelCtrl.Location = new System.Drawing.Point(4, 165);
            this.m_fileSelCtrl.Name = "m_fileSelCtrl";
            this.m_fileSelCtrl.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelCtrl.TabIndex = 0;
            // 
            // SaveCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_lblMacro);
            this.Controls.Add(this.m_chkListBoxMacros);
            this.Controls.Add(this.m_btnOk);
            this.Controls.Add(this.m_fileSelCtrl);
            this.Name = "SaveCtrl";
            this.Size = new System.Drawing.Size(267, 262);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FileSelectionCtrl m_fileSelCtrl;
        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.CheckedListBox m_chkListBoxMacros;
        private System.Windows.Forms.Label m_lblMacro;
    }
}
