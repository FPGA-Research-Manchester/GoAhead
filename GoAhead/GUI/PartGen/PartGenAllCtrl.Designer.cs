namespace GoAhead.GUI.PartGen
{
    partial class PartGenAllCtrl
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
            this.m_chkBoxSpartan3 = new System.Windows.Forms.CheckBox();
            this.m_chkBoxSpartan6 = new System.Windows.Forms.CheckBox();
            this.m_chkBoxVirtex4 = new System.Windows.Forms.CheckBox();
            this.m_chkBoxKintex7 = new System.Windows.Forms.CheckBox();
            this.m_chkBoxVirtex6 = new System.Windows.Forms.CheckBox();
            this.m_chkBoxVirtex5 = new System.Windows.Forms.CheckBox();
            this.m_txtPath = new System.Windows.Forms.RichTextBox();
            this.m_lblPath = new System.Windows.Forms.Label();
            this.m_chkBoxKeepXDL = new System.Windows.Forms.CheckBox();
            this.m_chkBoxKeepBinFPGAs = new System.Windows.Forms.CheckBox();
            this.m_btnBrowse = new System.Windows.Forms.Button();
            this.m_btnOk = new System.Windows.Forms.Button();
            this.m_chkBoxAllConns = new System.Windows.Forms.CheckBox();
            this.m_numDrpMaxGregreeofParallelism = new System.Windows.Forms.NumericUpDown();
            this.m_lblMaxParallel = new System.Windows.Forms.Label();
            this.m_tabCtrl = new System.Windows.Forms.TabControl();
            this.m_tabFamily = new System.Windows.Forms.TabPage();
            this.m_tabDevices = new System.Windows.Forms.TabPage();
            this.m_txtDeviceFilter = new System.Windows.Forms.TextBox();
            this.m_lblDeviceFilter = new System.Windows.Forms.Label();
            this.m_lstBoxDevices = new System.Windows.Forms.ListBox();
            this.m_lblFilter = new System.Windows.Forms.Label();
            this.m_txtFilter = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_chkBoxExcludeBirectionalWires = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_numDrpMaxGregreeofParallelism)).BeginInit();
            this.m_tabCtrl.SuspendLayout();
            this.m_tabFamily.SuspendLayout();
            this.m_tabDevices.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_chkBoxSpartan3
            // 
            this.m_chkBoxSpartan3.AutoSize = true;
            this.m_chkBoxSpartan3.Location = new System.Drawing.Point(6, 6);
            this.m_chkBoxSpartan3.Name = "m_chkBoxSpartan3";
            this.m_chkBoxSpartan3.Size = new System.Drawing.Size(69, 17);
            this.m_chkBoxSpartan3.TabIndex = 0;
            this.m_chkBoxSpartan3.Text = "Spartan3";
            this.m_chkBoxSpartan3.UseVisualStyleBackColor = true;
            this.m_chkBoxSpartan3.CheckedChanged += new System.EventHandler(this.m_chkBoxSpartan3_CheckedChanged);
            // 
            // m_chkBoxSpartan6
            // 
            this.m_chkBoxSpartan6.AutoSize = true;
            this.m_chkBoxSpartan6.Location = new System.Drawing.Point(6, 29);
            this.m_chkBoxSpartan6.Name = "m_chkBoxSpartan6";
            this.m_chkBoxSpartan6.Size = new System.Drawing.Size(69, 17);
            this.m_chkBoxSpartan6.TabIndex = 1;
            this.m_chkBoxSpartan6.Text = "Spartan6";
            this.m_chkBoxSpartan6.UseVisualStyleBackColor = true;
            this.m_chkBoxSpartan6.CheckedChanged += new System.EventHandler(this.m_chkBoxSpartan6_CheckedChanged);
            // 
            // m_chkBoxVirtex4
            // 
            this.m_chkBoxVirtex4.AutoSize = true;
            this.m_chkBoxVirtex4.Location = new System.Drawing.Point(6, 52);
            this.m_chkBoxVirtex4.Name = "m_chkBoxVirtex4";
            this.m_chkBoxVirtex4.Size = new System.Drawing.Size(58, 17);
            this.m_chkBoxVirtex4.TabIndex = 2;
            this.m_chkBoxVirtex4.Text = "Virtex4";
            this.m_chkBoxVirtex4.UseVisualStyleBackColor = true;
            this.m_chkBoxVirtex4.CheckedChanged += new System.EventHandler(this.m_chkBoxVirtex4_CheckedChanged);
            // 
            // m_chkBoxKintex7
            // 
            this.m_chkBoxKintex7.AutoSize = true;
            this.m_chkBoxKintex7.Location = new System.Drawing.Point(6, 121);
            this.m_chkBoxKintex7.Name = "m_chkBoxKintex7";
            this.m_chkBoxKintex7.Size = new System.Drawing.Size(61, 17);
            this.m_chkBoxKintex7.TabIndex = 5;
            this.m_chkBoxKintex7.Text = "Kintex7";
            this.m_chkBoxKintex7.UseVisualStyleBackColor = true;
            this.m_chkBoxKintex7.CheckedChanged += new System.EventHandler(this.m_chkBoxKintex7_CheckedChanged);
            // 
            // m_chkBoxVirtex6
            // 
            this.m_chkBoxVirtex6.AutoSize = true;
            this.m_chkBoxVirtex6.Location = new System.Drawing.Point(6, 98);
            this.m_chkBoxVirtex6.Name = "m_chkBoxVirtex6";
            this.m_chkBoxVirtex6.Size = new System.Drawing.Size(58, 17);
            this.m_chkBoxVirtex6.TabIndex = 4;
            this.m_chkBoxVirtex6.Text = "Virtex6";
            this.m_chkBoxVirtex6.UseVisualStyleBackColor = true;
            this.m_chkBoxVirtex6.CheckedChanged += new System.EventHandler(this.m_chkBoxVirtex6_CheckedChanged);
            // 
            // m_chkBoxVirtex5
            // 
            this.m_chkBoxVirtex5.AutoSize = true;
            this.m_chkBoxVirtex5.Location = new System.Drawing.Point(6, 75);
            this.m_chkBoxVirtex5.Name = "m_chkBoxVirtex5";
            this.m_chkBoxVirtex5.Size = new System.Drawing.Size(58, 17);
            this.m_chkBoxVirtex5.TabIndex = 3;
            this.m_chkBoxVirtex5.Text = "Virtex5";
            this.m_chkBoxVirtex5.UseVisualStyleBackColor = true;
            this.m_chkBoxVirtex5.CheckedChanged += new System.EventHandler(this.m_chkBoxVirtex5_CheckedChanged);
            // 
            // m_txtPath
            // 
            this.m_txtPath.Location = new System.Drawing.Point(7, 312);
            this.m_txtPath.Multiline = false;
            this.m_txtPath.Name = "m_txtPath";
            this.m_txtPath.Size = new System.Drawing.Size(249, 19);
            this.m_txtPath.TabIndex = 6;
            this.m_txtPath.Text = "";
            // 
            // m_lblPath
            // 
            this.m_lblPath.AutoSize = true;
            this.m_lblPath.Location = new System.Drawing.Point(7, 293);
            this.m_lblPath.Name = "m_lblPath";
            this.m_lblPath.Size = new System.Drawing.Size(191, 13);
            this.m_lblPath.TabIndex = 7;
            this.m_lblPath.Text = "Where to store XDL and binFPGA Files";
            // 
            // m_chkBoxKeepXDL
            // 
            this.m_chkBoxKeepXDL.AutoSize = true;
            this.m_chkBoxKeepXDL.Checked = true;
            this.m_chkBoxKeepXDL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkBoxKeepXDL.Location = new System.Drawing.Point(7, 339);
            this.m_chkBoxKeepXDL.Name = "m_chkBoxKeepXDL";
            this.m_chkBoxKeepXDL.Size = new System.Drawing.Size(96, 17);
            this.m_chkBoxKeepXDL.TabIndex = 8;
            this.m_chkBoxKeepXDL.Text = "Keep XDL files";
            this.m_chkBoxKeepXDL.UseVisualStyleBackColor = true;
            // 
            // m_chkBoxKeepBinFPGAs
            // 
            this.m_chkBoxKeepBinFPGAs.AutoSize = true;
            this.m_chkBoxKeepBinFPGAs.Location = new System.Drawing.Point(7, 362);
            this.m_chkBoxKeepBinFPGAs.Name = "m_chkBoxKeepBinFPGAs";
            this.m_chkBoxKeepBinFPGAs.Size = new System.Drawing.Size(142, 17);
            this.m_chkBoxKeepBinFPGAs.TabIndex = 9;
            this.m_chkBoxKeepBinFPGAs.Text = "Keep Existing binFPGAS";
            this.m_chkBoxKeepBinFPGAs.UseVisualStyleBackColor = true;
            // 
            // m_btnBrowse
            // 
            this.m_btnBrowse.Location = new System.Drawing.Point(181, 337);
            this.m_btnBrowse.Name = "m_btnBrowse";
            this.m_btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.m_btnBrowse.TabIndex = 10;
            this.m_btnBrowse.Text = "Browse";
            this.m_btnBrowse.UseVisualStyleBackColor = true;
            this.m_btnBrowse.Click += new System.EventHandler(this.m_btnBrowse_Click);
            // 
            // m_btnOk
            // 
            this.m_btnOk.Location = new System.Drawing.Point(95, 538);
            this.m_btnOk.Name = "m_btnOk";
            this.m_btnOk.Size = new System.Drawing.Size(75, 23);
            this.m_btnOk.TabIndex = 11;
            this.m_btnOk.Text = "Ok";
            this.m_btnOk.UseVisualStyleBackColor = true;
            this.m_btnOk.Click += new System.EventHandler(this.m_btnOk_Click);
            // 
            // m_chkBoxAllConns
            // 
            this.m_chkBoxAllConns.AutoSize = true;
            this.m_chkBoxAllConns.Checked = true;
            this.m_chkBoxAllConns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkBoxAllConns.Location = new System.Drawing.Point(7, 386);
            this.m_chkBoxAllConns.Name = "m_chkBoxAllConns";
            this.m_chkBoxAllConns.Size = new System.Drawing.Size(190, 17);
            this.m_chkBoxAllConns.TabIndex = 12;
            this.m_chkBoxAllConns.Text = "Use -all_conns for XDL Generation";
            this.m_chkBoxAllConns.UseVisualStyleBackColor = true;
            // 
            // m_numDrpMaxGregreeofParallelism
            // 
            this.m_numDrpMaxGregreeofParallelism.Location = new System.Drawing.Point(6, 456);
            this.m_numDrpMaxGregreeofParallelism.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numDrpMaxGregreeofParallelism.Name = "m_numDrpMaxGregreeofParallelism";
            this.m_numDrpMaxGregreeofParallelism.Size = new System.Drawing.Size(61, 20);
            this.m_numDrpMaxGregreeofParallelism.TabIndex = 13;
            this.m_numDrpMaxGregreeofParallelism.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_numDrpMaxGregreeofParallelism.Visible = false;
            // 
            // m_lblMaxParallel
            // 
            this.m_lblMaxParallel.AutoSize = true;
            this.m_lblMaxParallel.Location = new System.Drawing.Point(3, 440);
            this.m_lblMaxParallel.Name = "m_lblMaxParallel";
            this.m_lblMaxParallel.Size = new System.Drawing.Size(153, 13);
            this.m_lblMaxParallel.TabIndex = 14;
            this.m_lblMaxParallel.Text = "Maximum Degree of Parallelism";
            this.m_lblMaxParallel.Visible = false;
            // 
            // m_tabCtrl
            // 
            this.m_tabCtrl.Controls.Add(this.m_tabFamily);
            this.m_tabCtrl.Controls.Add(this.m_tabDevices);
            this.m_tabCtrl.Location = new System.Drawing.Point(3, 3);
            this.m_tabCtrl.Name = "m_tabCtrl";
            this.m_tabCtrl.SelectedIndex = 0;
            this.m_tabCtrl.Size = new System.Drawing.Size(253, 278);
            this.m_tabCtrl.TabIndex = 15;
            // 
            // m_tabFamily
            // 
            this.m_tabFamily.Controls.Add(this.m_chkBoxSpartan3);
            this.m_tabFamily.Controls.Add(this.m_chkBoxSpartan6);
            this.m_tabFamily.Controls.Add(this.m_chkBoxVirtex4);
            this.m_tabFamily.Controls.Add(this.m_chkBoxVirtex5);
            this.m_tabFamily.Controls.Add(this.m_chkBoxVirtex6);
            this.m_tabFamily.Controls.Add(this.m_chkBoxKintex7);
            this.m_tabFamily.Location = new System.Drawing.Point(4, 22);
            this.m_tabFamily.Name = "m_tabFamily";
            this.m_tabFamily.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabFamily.Size = new System.Drawing.Size(245, 252);
            this.m_tabFamily.TabIndex = 0;
            this.m_tabFamily.Text = "Family";
            this.m_tabFamily.UseVisualStyleBackColor = true;
            // 
            // m_tabDevices
            // 
            this.m_tabDevices.Controls.Add(this.m_txtDeviceFilter);
            this.m_tabDevices.Controls.Add(this.m_lblDeviceFilter);
            this.m_tabDevices.Controls.Add(this.m_lstBoxDevices);
            this.m_tabDevices.Location = new System.Drawing.Point(4, 22);
            this.m_tabDevices.Name = "m_tabDevices";
            this.m_tabDevices.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabDevices.Size = new System.Drawing.Size(245, 252);
            this.m_tabDevices.TabIndex = 1;
            this.m_tabDevices.Text = "Devices";
            this.m_tabDevices.UseVisualStyleBackColor = true;
            // 
            // m_txtDeviceFilter
            // 
            this.m_txtDeviceFilter.Location = new System.Drawing.Point(6, 221);
            this.m_txtDeviceFilter.Name = "m_txtDeviceFilter";
            this.m_txtDeviceFilter.Size = new System.Drawing.Size(233, 20);
            this.m_txtDeviceFilter.TabIndex = 2;
            this.m_txtDeviceFilter.TextChanged += new System.EventHandler(this.m_txtDeviceFilter_TextChanged);
            // 
            // m_lblDeviceFilter
            // 
            this.m_lblDeviceFilter.AutoSize = true;
            this.m_lblDeviceFilter.Location = new System.Drawing.Point(6, 205);
            this.m_lblDeviceFilter.Name = "m_lblDeviceFilter";
            this.m_lblDeviceFilter.Size = new System.Drawing.Size(29, 13);
            this.m_lblDeviceFilter.TabIndex = 1;
            this.m_lblDeviceFilter.Text = "Filter";
            // 
            // m_lstBoxDevices
            // 
            this.m_lstBoxDevices.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_lstBoxDevices.FormattingEnabled = true;
            this.m_lstBoxDevices.Location = new System.Drawing.Point(3, 3);
            this.m_lstBoxDevices.Name = "m_lstBoxDevices";
            this.m_lstBoxDevices.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.m_lstBoxDevices.Size = new System.Drawing.Size(239, 199);
            this.m_lstBoxDevices.TabIndex = 0;
            this.m_lstBoxDevices.SelectedIndexChanged += new System.EventHandler(this.m_lstBoxDevices_SelectedIndexChanged);
            // 
            // m_lblFilter
            // 
            this.m_lblFilter.AutoSize = true;
            this.m_lblFilter.Location = new System.Drawing.Point(7, 479);
            this.m_lblFilter.Name = "m_lblFilter";
            this.m_lblFilter.Size = new System.Drawing.Size(163, 13);
            this.m_lblFilter.TabIndex = 16;
            this.m_lblFilter.Text = "Positive Filter for XDL-Generation";
            // 
            // m_txtFilter
            // 
            this.m_txtFilter.Location = new System.Drawing.Point(7, 511);
            this.m_txtFilter.Multiline = false;
            this.m_txtFilter.Name = "m_txtFilter";
            this.m_txtFilter.Size = new System.Drawing.Size(246, 19);
            this.m_txtFilter.TabIndex = 17;
            this.m_txtFilter.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 493);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "passed to the PartGenAll Command";
            // 
            // m_chkBoxExcludeBirectionalWires
            // 
            this.m_chkBoxExcludeBirectionalWires.AutoSize = true;
            this.m_chkBoxExcludeBirectionalWires.Location = new System.Drawing.Point(6, 409);
            this.m_chkBoxExcludeBirectionalWires.Name = "m_chkBoxExcludeBirectionalWires";
            this.m_chkBoxExcludeBirectionalWires.Size = new System.Drawing.Size(242, 17);
            this.m_chkBoxExcludeBirectionalWires.TabIndex = 19;
            this.m_chkBoxExcludeBirectionalWires.Text = "Exclude bidirectional Wires from Blocking (S6)";
            this.m_chkBoxExcludeBirectionalWires.UseVisualStyleBackColor = true;
            // 
            // PartGenAllCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_chkBoxExcludeBirectionalWires);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_txtFilter);
            this.Controls.Add(this.m_lblFilter);
            this.Controls.Add(this.m_tabCtrl);
            this.Controls.Add(this.m_lblMaxParallel);
            this.Controls.Add(this.m_numDrpMaxGregreeofParallelism);
            this.Controls.Add(this.m_chkBoxAllConns);
            this.Controls.Add(this.m_btnOk);
            this.Controls.Add(this.m_btnBrowse);
            this.Controls.Add(this.m_chkBoxKeepBinFPGAs);
            this.Controls.Add(this.m_chkBoxKeepXDL);
            this.Controls.Add(this.m_lblPath);
            this.Controls.Add(this.m_txtPath);
            this.Name = "PartGenAllCtrl";
            this.Size = new System.Drawing.Size(262, 569);
            ((System.ComponentModel.ISupportInitialize)(this.m_numDrpMaxGregreeofParallelism)).EndInit();
            this.m_tabCtrl.ResumeLayout(false);
            this.m_tabFamily.ResumeLayout(false);
            this.m_tabFamily.PerformLayout();
            this.m_tabDevices.ResumeLayout(false);
            this.m_tabDevices.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_chkBoxSpartan3;
        private System.Windows.Forms.CheckBox m_chkBoxSpartan6;
        private System.Windows.Forms.CheckBox m_chkBoxVirtex4;
        private System.Windows.Forms.CheckBox m_chkBoxKintex7;
        private System.Windows.Forms.CheckBox m_chkBoxVirtex6;
        private System.Windows.Forms.CheckBox m_chkBoxVirtex5;
        private System.Windows.Forms.RichTextBox m_txtPath;
        private System.Windows.Forms.Label m_lblPath;
        private System.Windows.Forms.CheckBox m_chkBoxKeepXDL;
        private System.Windows.Forms.CheckBox m_chkBoxKeepBinFPGAs;
        private System.Windows.Forms.Button m_btnBrowse;
        private System.Windows.Forms.Button m_btnOk;
        private System.Windows.Forms.CheckBox m_chkBoxAllConns;
        private System.Windows.Forms.NumericUpDown m_numDrpMaxGregreeofParallelism;
        private System.Windows.Forms.Label m_lblMaxParallel;
        private System.Windows.Forms.TabControl m_tabCtrl;
        private System.Windows.Forms.TabPage m_tabFamily;
        private System.Windows.Forms.TabPage m_tabDevices;
        private System.Windows.Forms.ListBox m_lstBoxDevices;
        private System.Windows.Forms.Label m_lblFilter;
        private System.Windows.Forms.RichTextBox m_txtFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox m_chkBoxExcludeBirectionalWires;
        private System.Windows.Forms.TextBox m_txtDeviceFilter;
        private System.Windows.Forms.Label m_lblDeviceFilter;
    }
}
