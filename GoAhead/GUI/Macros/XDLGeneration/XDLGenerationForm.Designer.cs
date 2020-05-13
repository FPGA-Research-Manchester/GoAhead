namespace GoAhead.GUI.Macros.XDLGeneration
{
    partial class XDLGenerationForm
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
            this.xdlGenerationCtrl1 = new GoAhead.GUI.Macros.XDLGeneration.XDLGenerationCtrl();
            this.SuspendLayout();
            // 
            // xdlGenerationCtrl1
            // 
            this.xdlGenerationCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xdlGenerationCtrl1.Location = new System.Drawing.Point(0, 0);
            this.xdlGenerationCtrl1.Name = "xdlGenerationCtrl1";
            this.xdlGenerationCtrl1.Size = new System.Drawing.Size(267, 416);
            this.xdlGenerationCtrl1.TabIndex = 0;
            // 
            // XDLGenerationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 416);
            this.Controls.Add(this.xdlGenerationCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XDLGenerationForm";
            this.Text = "XDL Generation";
            this.ResumeLayout(false);

        }

        #endregion

        private XDLGenerationCtrl xdlGenerationCtrl1;
    }
}