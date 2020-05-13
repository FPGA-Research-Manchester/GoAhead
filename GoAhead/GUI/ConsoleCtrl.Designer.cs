using System.Drawing;

namespace GoAhead.GUI
{
    partial class ConsoleCtrl
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.m_tabCtrl = new System.Windows.Forms.TabControl();
            this.m_tabCmdTrace = new System.Windows.Forms.TabPage();
            this.m_txtCommandTrace = new System.Windows.Forms.RichTextBox();
            this.m_tabOutput = new System.Windows.Forms.TabPage();
            this.m_txtOutputTrace = new System.Windows.Forms.RichTextBox();
            this.m_tabWarnings = new System.Windows.Forms.TabPage();
            this.m_txtWarningsTrace = new System.Windows.Forms.RichTextBox();
            this.m_tabErrorTrace = new System.Windows.Forms.TabPage();
            this.m_txtErrorTrace = new System.Windows.Forms.RichTextBox();
            this.m_tabUCF = new System.Windows.Forms.TabPage();
            this.m_txtUCF = new System.Windows.Forms.RichTextBox();
            this.m_tabVHDL = new System.Windows.Forms.TabPage();
            this.m_txtVHDL = new System.Windows.Forms.RichTextBox();
            this.m_tabTCL = new System.Windows.Forms.TabPage();
            this.m_txtTCL = new System.Windows.Forms.RichTextBox();
            this.m_tabInput = new System.Windows.Forms.TabPage();
            this.m_txtInput = new System.Windows.Forms.RichTextBox();
            this.m_tabTCLInput = new System.Windows.Forms.TabPage();
            this.m_panelTCLInput = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            TCL_input = new System.Windows.Forms.RichTextBox();
            this.m_ctxtMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.m_ctxtMenuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ctxtMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ctxtMenuCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ctxtMenuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ctxtMenuPaste = new System.Windows.Forms.ToolStripMenuItem();
            TCL_output = new System.Windows.Forms.RichTextBox();
            this.m_tabCtrl.SuspendLayout();
            this.m_tabCmdTrace.SuspendLayout();
            this.m_tabOutput.SuspendLayout();
            this.m_tabWarnings.SuspendLayout();
            this.m_tabErrorTrace.SuspendLayout();
            this.m_tabUCF.SuspendLayout();
            this.m_tabVHDL.SuspendLayout();
            this.m_tabTCL.SuspendLayout();
            this.m_tabInput.SuspendLayout();
            this.m_tabTCLInput.SuspendLayout();
            this.m_panelTCLInput.SuspendLayout();
            this.m_ctxtMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_tabCtrl
            // 
            this.m_tabCtrl.Controls.Add(this.m_tabCmdTrace);
            this.m_tabCtrl.Controls.Add(this.m_tabOutput);
            this.m_tabCtrl.Controls.Add(this.m_tabWarnings);
            this.m_tabCtrl.Controls.Add(this.m_tabErrorTrace);
            this.m_tabCtrl.Controls.Add(this.m_tabUCF);
            this.m_tabCtrl.Controls.Add(this.m_tabVHDL);
            this.m_tabCtrl.Controls.Add(this.m_tabTCL);
            this.m_tabCtrl.Controls.Add(this.m_tabInput);
            this.m_tabCtrl.Controls.Add(this.m_tabTCLInput);
            this.m_tabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabCtrl.Location = new System.Drawing.Point(0, 0);
            this.m_tabCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabCtrl.Name = "m_tabCtrl";
            this.m_tabCtrl.SelectedIndex = 0;
            this.m_tabCtrl.Size = new System.Drawing.Size(1052, 559);
            this.m_tabCtrl.TabIndex = 0;
            // 
            // m_tabCmdTrace
            // 
            this.m_tabCmdTrace.Controls.Add(this.m_txtCommandTrace);
            this.m_tabCmdTrace.Location = new System.Drawing.Point(4, 25);
            this.m_tabCmdTrace.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabCmdTrace.Name = "m_tabCmdTrace";
            this.m_tabCmdTrace.Size = new System.Drawing.Size(1044, 530);
            this.m_tabCmdTrace.TabIndex = 0;
            this.m_tabCmdTrace.Text = "Command Trace";
            this.m_tabCmdTrace.UseVisualStyleBackColor = true;
            // 
            // m_txtCommandTrace
            // 
            this.m_txtCommandTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtCommandTrace.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_txtCommandTrace.Location = new System.Drawing.Point(0, 0);
            this.m_txtCommandTrace.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtCommandTrace.Name = "m_txtCommandTrace";
            this.m_txtCommandTrace.ReadOnly = true;
            this.m_txtCommandTrace.Size = new System.Drawing.Size(1044, 530);
            this.m_txtCommandTrace.TabIndex = 0;
            this.m_txtCommandTrace.Text = "";
            this.m_txtCommandTrace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtCommandTrace_MouseDown);
            // 
            // m_tabOutput
            // 
            this.m_tabOutput.Controls.Add(this.m_txtOutputTrace);
            this.m_tabOutput.Location = new System.Drawing.Point(4, 25);
            this.m_tabOutput.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabOutput.Name = "m_tabOutput";
            this.m_tabOutput.Size = new System.Drawing.Size(1044, 530);
            this.m_tabOutput.TabIndex = 3;
            this.m_tabOutput.Text = "Output";
            this.m_tabOutput.UseVisualStyleBackColor = true;
            // 
            // m_txtOutputTrace
            // 
            this.m_txtOutputTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtOutputTrace.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.m_txtOutputTrace.Location = new System.Drawing.Point(0, 0);
            this.m_txtOutputTrace.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtOutputTrace.Name = "m_txtOutputTrace";
            this.m_txtOutputTrace.ReadOnly = true;
            this.m_txtOutputTrace.Size = new System.Drawing.Size(1044, 530);
            this.m_txtOutputTrace.TabIndex = 0;
            this.m_txtOutputTrace.Text = "";
            this.m_txtOutputTrace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtOutputTrace_MouseDown);
            // 
            // m_tabWarnings
            // 
            this.m_tabWarnings.Controls.Add(this.m_txtWarningsTrace);
            this.m_tabWarnings.Location = new System.Drawing.Point(4, 25);
            this.m_tabWarnings.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabWarnings.Name = "m_tabWarnings";
            this.m_tabWarnings.Size = new System.Drawing.Size(1044, 530);
            this.m_tabWarnings.TabIndex = 7;
            this.m_tabWarnings.Text = "Warnings";
            this.m_tabWarnings.UseVisualStyleBackColor = true;
            // 
            // m_txtWarningsTrace
            // 
            this.m_txtWarningsTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtWarningsTrace.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.m_txtWarningsTrace.Location = new System.Drawing.Point(0, 0);
            this.m_txtWarningsTrace.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtWarningsTrace.Name = "m_txtWarningsTrace";
            this.m_txtWarningsTrace.ReadOnly = true;
            this.m_txtWarningsTrace.Size = new System.Drawing.Size(1044, 530);
            this.m_txtWarningsTrace.TabIndex = 1;
            this.m_txtWarningsTrace.Text = "";
            this.m_txtWarningsTrace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtWarningsTrace_MouseDown);
            // 
            // m_tabErrorTrace
            // 
            this.m_tabErrorTrace.Controls.Add(this.m_txtErrorTrace);
            this.m_tabErrorTrace.Location = new System.Drawing.Point(4, 25);
            this.m_tabErrorTrace.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabErrorTrace.Name = "m_tabErrorTrace";
            this.m_tabErrorTrace.Size = new System.Drawing.Size(1044, 530);
            this.m_tabErrorTrace.TabIndex = 1;
            this.m_tabErrorTrace.Text = "Error Trace";
            this.m_tabErrorTrace.UseVisualStyleBackColor = true;
            // 
            // m_txtErrorTrace
            // 
            this.m_txtErrorTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtErrorTrace.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_txtErrorTrace.Location = new System.Drawing.Point(0, 0);
            this.m_txtErrorTrace.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtErrorTrace.Name = "m_txtErrorTrace";
            this.m_txtErrorTrace.ReadOnly = true;
            this.m_txtErrorTrace.Size = new System.Drawing.Size(1044, 530);
            this.m_txtErrorTrace.TabIndex = 0;
            this.m_txtErrorTrace.Text = "";
            this.m_txtErrorTrace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtErrorTrace_MouseDown);
            // 
            // m_tabUCF
            // 
            this.m_tabUCF.Controls.Add(this.m_txtUCF);
            this.m_tabUCF.Location = new System.Drawing.Point(4, 25);
            this.m_tabUCF.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabUCF.Name = "m_tabUCF";
            this.m_tabUCF.Size = new System.Drawing.Size(1044, 530);
            this.m_tabUCF.TabIndex = 5;
            this.m_tabUCF.Text = "UCF";
            this.m_tabUCF.UseVisualStyleBackColor = true;
            // 
            // m_txtUCF
            // 
            this.m_txtUCF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtUCF.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.m_txtUCF.Location = new System.Drawing.Point(0, 0);
            this.m_txtUCF.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtUCF.Name = "m_txtUCF";
            this.m_txtUCF.ReadOnly = true;
            this.m_txtUCF.Size = new System.Drawing.Size(1044, 530);
            this.m_txtUCF.TabIndex = 0;
            this.m_txtUCF.Text = "";
            this.m_txtUCF.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtUCF_MouseDown);
            // 
            // m_tabVHDL
            // 
            this.m_tabVHDL.Controls.Add(this.m_txtVHDL);
            this.m_tabVHDL.Location = new System.Drawing.Point(4, 25);
            this.m_tabVHDL.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabVHDL.Name = "m_tabVHDL";
            this.m_tabVHDL.Size = new System.Drawing.Size(1044, 530);
            this.m_tabVHDL.TabIndex = 6;
            this.m_tabVHDL.Text = "VHDL";
            this.m_tabVHDL.UseVisualStyleBackColor = true;
            // 
            // m_txtVHDL
            // 
            this.m_txtVHDL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtVHDL.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.m_txtVHDL.Location = new System.Drawing.Point(0, 0);
            this.m_txtVHDL.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtVHDL.Name = "m_txtVHDL";
            this.m_txtVHDL.ReadOnly = true;
            this.m_txtVHDL.Size = new System.Drawing.Size(1044, 530);
            this.m_txtVHDL.TabIndex = 0;
            this.m_txtVHDL.Text = "";
            this.m_txtVHDL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtVHDL_MouseDown);
            // 
            // m_tabTCL
            // 
            this.m_tabTCL.Controls.Add(this.m_txtTCL);
            this.m_tabTCL.Location = new System.Drawing.Point(4, 25);
            this.m_tabTCL.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabTCL.Name = "m_tabTCL";
            this.m_tabTCL.Size = new System.Drawing.Size(1044, 530);
            this.m_tabTCL.TabIndex = 8;
            this.m_tabTCL.Text = "TCL";
            this.m_tabTCL.UseVisualStyleBackColor = true;
            // 
            // m_txtTCL
            // 
            this.m_txtTCL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtTCL.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.m_txtTCL.Location = new System.Drawing.Point(0, 0);
            this.m_txtTCL.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtTCL.Name = "m_txtTCL";
            this.m_txtTCL.ReadOnly = true;
            this.m_txtTCL.Size = new System.Drawing.Size(1044, 530);
            this.m_txtTCL.TabIndex = 1;
            this.m_txtTCL.Text = "";
            this.m_txtTCL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtTCL_MouseDown);
            // 
            // m_tabInput
            // 
            this.m_tabInput.Controls.Add(this.m_txtInput);
            this.m_tabInput.Location = new System.Drawing.Point(4, 25);
            this.m_tabInput.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabInput.Name = "m_tabInput";
            this.m_tabInput.Size = new System.Drawing.Size(1044, 530);
            this.m_tabInput.TabIndex = 4;
            this.m_tabInput.Text = "Input";
            this.m_tabInput.UseVisualStyleBackColor = true;
            // 
            // m_txtInput
            // 
            this.m_txtInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtInput.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.m_txtInput.Location = new System.Drawing.Point(0, 0);
            this.m_txtInput.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtInput.Name = "m_txtInput";
            this.m_txtInput.Size = new System.Drawing.Size(1044, 530);
            this.m_txtInput.TabIndex = 0;
            this.m_txtInput.Text = "";
            this.m_txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_txtInput_KeyDown);
            this.m_txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_txtInputTrace_KeyPress);
            this.m_txtInput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_txtInput_MouseDown);
            // 
            // m_tabTCLInput
            // 
            this.m_tabTCLInput.Controls.Add(this.m_panelTCLInput);
            this.m_tabTCLInput.Location = new System.Drawing.Point(4, 25);
            this.m_tabTCLInput.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabTCLInput.Name = "m_tabTCLInput";
            this.m_tabTCLInput.Size = new System.Drawing.Size(1044, 530);
            this.m_tabTCLInput.TabIndex = 7;
            this.m_tabTCLInput.Text = "TCL Input";
            this.m_tabTCLInput.UseVisualStyleBackColor = true;
            // 
            // m_panelTCLInput
            // 
            this.m_panelTCLInput.BackColor = System.Drawing.Color.White;
            this.m_panelTCLInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_panelTCLInput.Controls.Add(TCL_output);
            this.m_panelTCLInput.Controls.Add(this.button1);
            this.m_panelTCLInput.Controls.Add(TCL_input);
            this.m_panelTCLInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_panelTCLInput.Location = new System.Drawing.Point(0, 0);
            this.m_panelTCLInput.Margin = new System.Windows.Forms.Padding(4);
            this.m_panelTCLInput.Name = "m_panelTCLInput";
            this.m_panelTCLInput.Size = new System.Drawing.Size(1044, 530);
            this.m_panelTCLInput.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(958, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Source";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_txtTCLInput
            // 
            TCL_input.BackColor = System.Drawing.SystemColors.ControlLightLight;
            TCL_input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TCL_input.Dock = System.Windows.Forms.DockStyle.Bottom;
            TCL_input.Font = new System.Drawing.Font("Consolas", 8.25F);
            TCL_input.Location = new System.Drawing.Point(0, 489);
            TCL_input.Margin = new System.Windows.Forms.Padding(0);
            TCL_input.Name = "m_txtTCLInput";
            TCL_input.Size = new System.Drawing.Size(1040, 37);
            TCL_input.TabIndex = 0;
            TCL_input.Text = "";
            TCL_input.WordWrap = false;
            TCL_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TCL_input_KeyPress);
            // 
            // m_ctxtMenu
            // 
            this.m_ctxtMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.m_ctxtMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ctxtMenuSelectAll,
            this.m_ctxtMenuCopy,
            this.m_ctxtMenuCopyAll,
            this.m_ctxtMenuClear,
            this.m_ctxtMenuPaste});
            this.m_ctxtMenu.Name = "m_ctxtMenu";
            this.m_ctxtMenu.Size = new System.Drawing.Size(178, 124);
            // 
            // m_ctxtMenuSelectAll
            // 
            this.m_ctxtMenuSelectAll.Name = "m_ctxtMenuSelectAll";
            this.m_ctxtMenuSelectAll.Size = new System.Drawing.Size(177, 24);
            this.m_ctxtMenuSelectAll.Text = "Select All";
            this.m_ctxtMenuSelectAll.Click += new System.EventHandler(this.m_ctxtMenuSelectAll_Click);
            // 
            // m_ctxtMenuCopy
            // 
            this.m_ctxtMenuCopy.Name = "m_ctxtMenuCopy";
            this.m_ctxtMenuCopy.Size = new System.Drawing.Size(177, 24);
            this.m_ctxtMenuCopy.Text = "Copy Selection";
            this.m_ctxtMenuCopy.Click += new System.EventHandler(this.m_ctxtMenuCopy_Click);
            // 
            // m_ctxtMenuCopyAll
            // 
            this.m_ctxtMenuCopyAll.Name = "m_ctxtMenuCopyAll";
            this.m_ctxtMenuCopyAll.Size = new System.Drawing.Size(177, 24);
            this.m_ctxtMenuCopyAll.Text = "Copy All";
            this.m_ctxtMenuCopyAll.Click += new System.EventHandler(this.m_ctxtMenuCopyAll_Click);
            // 
            // m_ctxtMenuClear
            // 
            this.m_ctxtMenuClear.Name = "m_ctxtMenuClear";
            this.m_ctxtMenuClear.Size = new System.Drawing.Size(177, 24);
            this.m_ctxtMenuClear.Text = "Clear Window";
            this.m_ctxtMenuClear.Click += new System.EventHandler(this.m_ctxtMenuDelete_Click);
            // 
            // m_ctxtMenuPaste
            // 
            this.m_ctxtMenuPaste.Name = "m_ctxtMenuPaste";
            this.m_ctxtMenuPaste.Size = new System.Drawing.Size(177, 24);
            this.m_ctxtMenuPaste.Text = "Paste";
            this.m_ctxtMenuPaste.Click += new System.EventHandler(this.m_ctxtMenuPaste_Click);
            // 
            // richTextBox1
            // 
            TCL_output.BackColor = System.Drawing.SystemColors.ControlLightLight;
            TCL_output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TCL_output.Dock = System.Windows.Forms.DockStyle.Fill;
            TCL_output.Font = new System.Drawing.Font("Consolas", 8.25F);
            TCL_output.Location = new System.Drawing.Point(0, 0);
            TCL_output.Margin = new System.Windows.Forms.Padding(0);
            TCL_output.Name = "richTextBox1";
            TCL_output.ReadOnly = true;
            TCL_output.Size = new System.Drawing.Size(1040, 489);
            TCL_output.TabIndex = 2;
            TCL_output.Text = "";
            TCL_output.WordWrap = false;
            // 
            // ConsoleCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabCtrl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConsoleCtrl";
            this.Size = new System.Drawing.Size(1052, 559);
            this.m_tabCtrl.ResumeLayout(false);
            this.m_tabCmdTrace.ResumeLayout(false);
            this.m_tabOutput.ResumeLayout(false);
            this.m_tabWarnings.ResumeLayout(false);
            this.m_tabErrorTrace.ResumeLayout(false);
            this.m_tabUCF.ResumeLayout(false);
            this.m_tabVHDL.ResumeLayout(false);
            this.m_tabTCL.ResumeLayout(false);
            this.m_tabInput.ResumeLayout(false);
            this.m_tabTCLInput.ResumeLayout(false);
            this.m_panelTCLInput.ResumeLayout(false);
            this.m_ctxtMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl m_tabCtrl;
        private System.Windows.Forms.TabPage m_tabCmdTrace;
        private System.Windows.Forms.TabPage m_tabErrorTrace;
        private System.Windows.Forms.RichTextBox m_txtCommandTrace;
        private System.Windows.Forms.RichTextBox m_txtErrorTrace;
        private System.Windows.Forms.TabPage m_tabOutput;
        private System.Windows.Forms.RichTextBox m_txtOutputTrace;
        private System.Windows.Forms.TabPage m_tabInput;
        private System.Windows.Forms.Panel m_panelTCLInput;
        private System.Windows.Forms.TabPage m_tabTCLInput;
        private System.Windows.Forms.RichTextBox m_txtInput;
        private System.Windows.Forms.ContextMenuStrip m_ctxtMenu;
        private System.Windows.Forms.ToolStripMenuItem m_ctxtMenuSelectAll;
        private System.Windows.Forms.ToolStripMenuItem m_ctxtMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem m_ctxtMenuClear;
        private System.Windows.Forms.TabPage m_tabUCF;
        private System.Windows.Forms.TabPage m_tabVHDL;
        private System.Windows.Forms.RichTextBox m_txtUCF;
        private System.Windows.Forms.RichTextBox m_txtVHDL;
        private System.Windows.Forms.ToolStripMenuItem m_ctxtMenuCopyAll;
        private System.Windows.Forms.ToolStripMenuItem m_ctxtMenuPaste;
        private System.Windows.Forms.TabPage m_tabWarnings;
        private System.Windows.Forms.RichTextBox m_txtWarningsTrace;
        private System.Windows.Forms.TabPage m_tabTCL;
        private System.Windows.Forms.RichTextBox m_txtTCL;
        private System.Windows.Forms.Button button1;
        private static System.Windows.Forms.RichTextBox TCL_output;
        private static System.Windows.Forms.RichTextBox TCL_input;
    }
}
