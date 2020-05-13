namespace GoAhead.GUI
{
    partial class CommandInterfaceForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.commandInterfaceCtrl1 = new GoAhead.GUI.CommandInterfaceCtrl();
            this.SuspendLayout();
            // 
            // commandInterfaceCtrl1
            // 
            this.commandInterfaceCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandInterfaceCtrl1.Location = new System.Drawing.Point(0, 0);
            this.commandInterfaceCtrl1.Name = "commandInterfaceCtrl1";
            this.commandInterfaceCtrl1.Size = new System.Drawing.Size(1246, 587);
            this.commandInterfaceCtrl1.TabIndex = 0;
            // 
            // CommandInterfaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 587);
            this.Controls.Add(this.commandInterfaceCtrl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CommandInterfaceForm";
            this.Text = "Command Interface";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CommandInterfaceForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private CommandInterfaceCtrl commandInterfaceCtrl1;
    }
}