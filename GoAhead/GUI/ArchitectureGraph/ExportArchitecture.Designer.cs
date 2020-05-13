using System;

namespace GoAhead.GUI.ArchitectureGraph
{
    partial class ExportArchitecture
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
            this.btn_export = new System.Windows.Forms.Button();
            this.txtbox_exportFormat = new System.Windows.Forms.TextBox();
            this.lbl1_exportFormat = new System.Windows.Forms.Label();
            this.lbl2_exportFormat = new System.Windows.Forms.Label();
            this.txtbox_path = new System.Windows.Forms.TextBox();
            this.btn_path = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.selectionPanel = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.formatPanel = new System.Windows.Forms.Panel();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.columnWidthTextBox = new System.Windows.Forms.TextBox();
            this.columnWidthLabel = new System.Windows.Forms.Label();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.appendCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.selectionPanel.SuspendLayout();
            this.formatPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_export
            // 
            this.btn_export.Location = new System.Drawing.Point(33, 176);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(101, 30);
            this.btn_export.TabIndex = 0;
            this.btn_export.Text = "Export";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtbox_exportFormat
            // 
            this.txtbox_exportFormat.Location = new System.Drawing.Point(33, 148);
            this.txtbox_exportFormat.Name = "txtbox_exportFormat";
            this.txtbox_exportFormat.Size = new System.Drawing.Size(388, 22);
            this.txtbox_exportFormat.TabIndex = 1;
            this.txtbox_exportFormat.Text = "{tile1} {port1} -> {tile2} {port2}";
            // 
            // lbl1_exportFormat
            // 
            this.lbl1_exportFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lbl1_exportFormat.Location = new System.Drawing.Point(3, 14);
            this.lbl1_exportFormat.Name = "lbl1_exportFormat";
            this.lbl1_exportFormat.Size = new System.Drawing.Size(68, 26);
            this.lbl1_exportFormat.TabIndex = 2;
            this.lbl1_exportFormat.Text = "Format";
            // 
            // lbl2_exportFormat
            // 
            this.lbl2_exportFormat.Location = new System.Drawing.Point(30, 123);
            this.lbl2_exportFormat.Name = "lbl2_exportFormat";
            this.lbl2_exportFormat.Size = new System.Drawing.Size(191, 22);
            this.lbl2_exportFormat.TabIndex = 3;
            this.lbl2_exportFormat.Text = "Specify the format of a line:";
            // 
            // txtbox_path
            // 
            this.txtbox_path.BackColor = System.Drawing.SystemColors.Control;
            this.txtbox_path.Location = new System.Drawing.Point(43, 53);
            this.txtbox_path.Name = "txtbox_path";
            this.txtbox_path.Size = new System.Drawing.Size(434, 22);
            this.txtbox_path.TabIndex = 5;
            // 
            // btn_path
            // 
            this.btn_path.Location = new System.Drawing.Point(6, 53);
            this.btn_path.Name = "btn_path";
            this.btn_path.Size = new System.Drawing.Size(31, 23);
            this.btn_path.TabIndex = 6;
            this.btn_path.Text = "...";
            this.btn_path.UseVisualStyleBackColor = true;
            this.btn_path.Click += new System.EventHandler(this.pathBtn_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 53);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(87, 21);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Selection";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(6, 80);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(78, 21);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "All Tiles";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 26);
            this.label1.TabIndex = 13;
            this.label1.Text = "Path";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(3, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 26);
            this.label2.TabIndex = 14;
            this.label2.Text = "Scope";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "Set output text file:";
            // 
            // selectionPanel
            // 
            this.selectionPanel.BackColor = System.Drawing.Color.Transparent;
            this.selectionPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.selectionPanel.Controls.Add(this.button3);
            this.selectionPanel.Controls.Add(this.radioButton2);
            this.selectionPanel.Controls.Add(this.radioButton1);
            this.selectionPanel.Controls.Add(this.label2);
            this.selectionPanel.Location = new System.Drawing.Point(120, 25);
            this.selectionPanel.Name = "selectionPanel";
            this.selectionPanel.Size = new System.Drawing.Size(196, 120);
            this.selectionPanel.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(71, 11);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(22, 22);
            this.button3.TabIndex = 23;
            this.button3.Text = "?";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // formatPanel
            // 
            this.formatPanel.BackColor = System.Drawing.Color.Transparent;
            this.formatPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.formatPanel.Controls.Add(this.radioButton3);
            this.formatPanel.Controls.Add(this.button2);
            this.formatPanel.Controls.Add(this.lbl1_exportFormat);
            this.formatPanel.Controls.Add(this.columnWidthTextBox);
            this.formatPanel.Controls.Add(this.columnWidthLabel);
            this.formatPanel.Controls.Add(this.radioButton4);
            this.formatPanel.Location = new System.Drawing.Point(322, 25);
            this.formatPanel.Name = "formatPanel";
            this.formatPanel.Size = new System.Drawing.Size(276, 120);
            this.formatPanel.TabIndex = 17;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(6, 53);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(84, 21);
            this.radioButton3.TabIndex = 11;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Compact";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(77, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(22, 22);
            this.button2.TabIndex = 22;
            this.button2.Text = "?";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // columnWidthTextBox
            // 
            this.columnWidthTextBox.Enabled = false;
            this.columnWidthTextBox.Location = new System.Drawing.Point(243, 80);
            this.columnWidthTextBox.Name = "columnWidthTextBox";
            this.columnWidthTextBox.Size = new System.Drawing.Size(30, 22);
            this.columnWidthTextBox.TabIndex = 19;
            this.columnWidthTextBox.Text = "40";
            // 
            // columnWidthLabel
            // 
            this.columnWidthLabel.AutoSize = true;
            this.columnWidthLabel.Enabled = false;
            this.columnWidthLabel.Location = new System.Drawing.Point(142, 82);
            this.columnWidthLabel.Name = "columnWidthLabel";
            this.columnWidthLabel.Size = new System.Drawing.Size(95, 17);
            this.columnWidthLabel.TabIndex = 20;
            this.columnWidthLabel.Text = "Column Width";
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(4, 80);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(93, 21);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Formatted";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // appendCheckBox
            // 
            this.appendCheckBox.AutoSize = true;
            this.appendCheckBox.Location = new System.Drawing.Point(398, 29);
            this.appendCheckBox.Name = "appendCheckBox";
            this.appendCheckBox.Size = new System.Drawing.Size(79, 21);
            this.appendCheckBox.TabIndex = 18;
            this.appendCheckBox.Text = "Append";
            this.appendCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(427, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(22, 22);
            this.button1.TabIndex = 21;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(33, 56);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(395, 21);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "Exclude wires from ports not in the switch matrix of the tile.";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(116, 242);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(486, 252);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.lbl2_exportFormat);
            this.tabPage1.Controls.Add(this.btn_export);
            this.tabPage1.Controls.Add(this.txtbox_exportFormat);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(478, 223);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Wires";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(33, 83);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(332, 21);
            this.checkBox2.TabIndex = 25;
            this.checkBox2.Text = "Exclude wires from ports on arcs with stopovers.";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(30, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(409, 22);
            this.label5.TabIndex = 24;
            this.label5.Text = "Exports the outgoing wires for each tile in the scope.";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.checkBox3);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(478, 223);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Switch Matrices";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(33, 56);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(208, 21);
            this.checkBox3.TabIndex = 30;
            this.checkBox3.Text = "Exclude arcs with stopovers.";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(33, 176);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(101, 30);
            this.button5.TabIndex = 29;
            this.button5.Text = "Export";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(427, 147);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(22, 22);
            this.button4.TabIndex = 28;
            this.button4.Text = "?";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(33, 148);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(388, 22);
            this.textBox1.TabIndex = 27;
            this.textBox1.Text = "{tile} : {port1} -> {port2}";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(30, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(191, 22);
            this.label6.TabIndex = 26;
            this.label6.Text = "Specify the format of a line:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(30, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(442, 22);
            this.label3.TabIndex = 25;
            this.label3.Text = "Exports the switch matrix connections for each tile in the scope.";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Transparent;
            this.tabPage3.Controls.Add(this.button7);
            this.tabPage3.Controls.Add(this.button6);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(478, 223);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "BELs";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(33, 176);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(101, 30);
            this.button7.TabIndex = 30;
            this.button7.Text = "Export";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.Location = new System.Drawing.Point(427, 147);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(22, 22);
            this.button6.TabIndex = 29;
            this.button6.Text = "?";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(33, 148);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(388, 22);
            this.textBox2.TabIndex = 28;
            this.textBox2.Text = "{tile} {slice} {slicetype} {bel} {port} {porttype}";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(30, 123);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(191, 22);
            this.label8.TabIndex = 27;
            this.label8.Text = "Specify the format of a line:";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(30, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(442, 22);
            this.label7.TabIndex = 26;
            this.label7.Text = "Exports the BEL descriptions for each tile in the scope.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.appendCheckBox);
            this.panel1.Controls.Add(this.txtbox_path);
            this.panel1.Controls.Add(this.btn_path);
            this.panel1.Location = new System.Drawing.Point(120, 151);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(478, 85);
            this.panel1.TabIndex = 25;
            // 
            // ExportArchitecture
            // 
            this.ClientSize = new System.Drawing.Size(718, 538);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.formatPanel);
            this.Controls.Add(this.selectionPanel);
            this.Name = "ExportArchitecture";
            this.Text = "Export Architecture";
            this.selectionPanel.ResumeLayout(false);
            this.selectionPanel.PerformLayout();
            this.formatPanel.ResumeLayout(false);
            this.formatPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.TextBox txtbox_exportFormat;
        private System.Windows.Forms.Label lbl1_exportFormat;
        private System.Windows.Forms.Label lbl2_exportFormat;
        private System.Windows.Forms.TextBox txtbox_path;
        private System.Windows.Forms.Button btn_path;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel selectionPanel;
        private System.Windows.Forms.Panel formatPanel;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.CheckBox appendCheckBox;
        private System.Windows.Forms.TextBox columnWidthTextBox;
        private System.Windows.Forms.Label columnWidthLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}
