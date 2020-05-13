namespace GoAhead.GUI.ExtractModules
{
    partial class ExtractModuleCtrl
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
            this.m_fileSelXDLInFile = new GoAhead.GUI.FileSelectionCtrl();
            this.m_fileSelXDLOutFile = new GoAhead.GUI.FileSelectionCtrl();
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_fileSelBinMacro = new GoAhead.GUI.FileSelectionCtrl();
            this.SuspendLayout();
            // 
            // m_fileSelXDLInFile
            // 
            this.m_fileSelXDLInFile.Append = false;
            this.m_fileSelXDLInFile.FileName = "";
            this.m_fileSelXDLInFile.Filter = "All XDL files|*.XDL";
            this.m_fileSelXDLInFile.Label = "XDL Infile";
            this.m_fileSelXDLInFile.Location = new System.Drawing.Point(3, 8);
            this.m_fileSelXDLInFile.Name = "m_fileSelXDLInFile";
            this.m_fileSelXDLInFile.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelXDLInFile.TabIndex = 0;
            // 
            // m_fileSelXDLOutFile
            // 
            this.m_fileSelXDLOutFile.Append = false;
            this.m_fileSelXDLOutFile.FileName = "";
            this.m_fileSelXDLOutFile.Filter = "All XDL files|*.XDL";
            this.m_fileSelXDLOutFile.Label = "XDL Outfile";
            this.m_fileSelXDLOutFile.Location = new System.Drawing.Point(3, 75);
            this.m_fileSelXDLOutFile.Name = "m_fileSelXDLOutFile";
            this.m_fileSelXDLOutFile.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelXDLOutFile.TabIndex = 1;
            // 
            // m_btnOk
            // 
            this.m_btnOk.Location = new System.Drawing.Point(93, 209);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(75, 23);
            this.m_btnOk.TabIndex = 2;
            this.m_btnOk.Text = "OK";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
            // 
            // m_fileSelBinMacro
            // 
            this.m_fileSelBinMacro.Append = false;
            this.m_fileSelBinMacro.FileName = "";
            this.m_fileSelBinMacro.Filter = "All binNetlist files|*.binNetlist";
            this.m_fileSelBinMacro.Label = "Binary Element (Optional Outfile)";
            this.m_fileSelBinMacro.Location = new System.Drawing.Point(3, 142);
            this.m_fileSelBinMacro.Name = "m_fileSelBinMacro";
            this.m_fileSelBinMacro.Size = new System.Drawing.Size(260, 61);
            this.m_fileSelBinMacro.TabIndex = 3;
            // 
            // CutOffFromDesignCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_fileSelBinMacro);
            this.Controls.Add(this.m_btnOk);
            this.Controls.Add(this.m_fileSelXDLOutFile);
            this.Controls.Add(this.m_fileSelXDLInFile);
            this.Name = "CutOffFromDesignCtrl";
            this.Size = new System.Drawing.Size(265, 241);
            this.ResumeLayout(false);

        }

        #endregion

        private FileSelectionCtrl m_fileSelXDLInFile;
        private FileSelectionCtrl m_fileSelXDLOutFile;
        private System.Windows.Forms.Button m_btnOk;
        private FileSelectionCtrl m_fileSelBinMacro;
    }
}
