namespace GoAhead.GUI.Macros.BusInterface
{
    partial class PrintBusInterfaceCtrl
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
            this.m_grpBox = new System.Windows.Forms.GroupBox();
            this.m_drpDwnSignals = new System.Windows.Forms.NumericUpDown();
            this.m_drpDwnStartIndex = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.m_drpDwnWires = new System.Windows.Forms.ComboBox();
            this.m_drpDwnPips = new System.Windows.Forms.ComboBox();
            this.m_drpDwnBorder = new System.Windows.Forms.ComboBox();
            this.m_labelBorder = new System.Windows.Forms.Label();
            this.m_labelImport = new System.Windows.Forms.Label();
            this.m_labelPips = new System.Windows.Forms.Label();
            this.m_labelStartIndex = new System.Windows.Forms.Label();
            this.m_labelWiresType = new System.Windows.Forms.Label();
            this.m_btnPrintBusInterface = new System.Windows.Forms.Button();
            this.m_labelExport = new System.Windows.Forms.Label();
            this.m_labelTCLPath = new System.Windows.Forms.Label();
            this.m_txtBoxTCLPath = new System.Windows.Forms.TextBox();

            this.m_fileSelect = new GoAhead.GUI.FileSelectionCtrl();
            this.m_fileSelectOut = new GoAhead.GUI.FileSelectionCtrl();
            this.m_grpBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_drpDwnSignals)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_drpDwnStartIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // m_grpBox
            // 
            this.m_grpBox.Controls.Add(this.m_drpDwnSignals);
            this.m_grpBox.Controls.Add(this.m_drpDwnStartIndex);
            this.m_grpBox.Controls.Add(this.label1);
            this.m_grpBox.Controls.Add(this.m_drpDwnWires);
            this.m_grpBox.Controls.Add(this.m_drpDwnPips);
            this.m_grpBox.Controls.Add(this.m_drpDwnBorder);
            this.m_grpBox.Controls.Add(this.m_labelBorder);
            this.m_grpBox.Controls.Add(this.m_labelImport);
            this.m_grpBox.Controls.Add(this.m_labelPips);
            this.m_grpBox.Controls.Add(this.m_labelStartIndex);
            this.m_grpBox.Controls.Add(this.m_labelWiresType);
            this.m_grpBox.Controls.Add(this.m_btnPrintBusInterface);
            this.m_grpBox.Controls.Add(this.m_labelExport);
            this.m_grpBox.Controls.Add(this.m_labelTCLPath);
            this.m_grpBox.Controls.Add(this.m_txtBoxTCLPath);
            this.m_grpBox.Controls.Add(this.m_fileSelect);
            this.m_grpBox.Controls.Add(this.m_fileSelectOut);
            this.m_grpBox.Location = new System.Drawing.Point(4, 4);
            this.m_grpBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_grpBox.Name = "m_grpBox";
            this.m_grpBox.Padding = new System.Windows.Forms.Padding(4);
            this.m_grpBox.Size = new System.Drawing.Size(1047, 2464);
            this.m_grpBox.TabIndex = 18;
            this.m_grpBox.TabStop = false;
            this.m_grpBox.Text = "Print Bus Interface Constraints";
            this.m_grpBox.Enter += new System.EventHandler(this.m_grpBox_Enter);
            // 
            // m_drpDwnSignals
            // 
            this.m_drpDwnSignals.Location = new System.Drawing.Point(142, 405);
            this.m_drpDwnSignals.Name = "m_drpDwnSignals";
            this.m_drpDwnSignals.Size = new System.Drawing.Size(149, 22);
            this.m_drpDwnSignals.TabIndex = 17;
            this.m_drpDwnSignals.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.m_drpDwnSignals.Minimum = 1;
            // 
            // m_drpDwnStartIndex
            // 
            this.m_drpDwnStartIndex.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.m_drpDwnStartIndex.Location = new System.Drawing.Point(142, 353);
            this.m_drpDwnStartIndex.Name = "m_drpDwnStartIndex";
            this.m_drpDwnStartIndex.Size = new System.Drawing.Size(149, 22);
            this.m_drpDwnStartIndex.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 407);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "Signals per Tile :";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // m_drpDwnWires
            // 
            this.m_drpDwnWires.Location = new System.Drawing.Point(142, 307);
            this.m_drpDwnWires.Name = "m_drpDwnWires";
            this.m_drpDwnWires.Size = new System.Drawing.Size(149, 22);
            this.m_drpDwnWires.TabIndex = 13;
            this.m_drpDwnWires.Items.Add("2");
            this.m_drpDwnWires.Items.Add("4");
            this.m_drpDwnWires.SelectedItem = "2";
            this.m_drpDwnWires.FormattingEnabled = false;
            // 
            // m_drpDwnPips
            // 
            this.m_drpDwnPips.Location = new System.Drawing.Point(142, 258);
            this.m_drpDwnPips.Name = "m_drpDwnPip";
            this.m_drpDwnPips.Size = new System.Drawing.Size(149, 22);
            this.m_drpDwnPips.TabIndex = 13;
            this.m_drpDwnPips.Items.Add("W");
            this.m_drpDwnPips.Items.Add("E");
            this.m_drpDwnPips.SelectedItem = "W";
            this.m_drpDwnPips.FormattingEnabled = false;
            // 
            // m_drpDwnBorder
            // 
            this.m_drpDwnBorder.Location = new System.Drawing.Point(142, 211);
            this.m_drpDwnBorder.Name = "m_drpDwnBorder";
            this.m_drpDwnBorder.Size = new System.Drawing.Size(149, 22);
            this.m_drpDwnBorder.TabIndex = 13;
            this.m_drpDwnBorder.Items.Add("West");
            this.m_drpDwnBorder.Items.Add("East");
            this.m_drpDwnBorder.SelectedItem = "West";
            this.m_drpDwnBorder.FormattingEnabled = false;
            // 
            // m_labelBorder
            // 
            this.m_labelBorder.Location = new System.Drawing.Point(13, 213);
            this.m_labelBorder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelBorder.Name = "m_labelBorder";
            this.m_labelBorder.Size = new System.Drawing.Size(231, 28);
            this.m_labelBorder.TabIndex = 4;
            this.m_labelBorder.Text = "Border:\r\n";
            this.m_labelBorder.Click += new System.EventHandler(this.m_labelBorder_Click);
            // 
            // m_labelImport
            // 
            this.m_labelImport.Location = new System.Drawing.Point(13, 35);
            this.m_labelImport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelImport.Name = "m_labelImport";
            this.m_labelImport.Size = new System.Drawing.Size(133, 28);
            this.m_labelImport.TabIndex = 5;
            this.m_labelImport.Text = "Import a CSV file";
            // 
            // m_labelPips
            // 
            this.m_labelPips.Location = new System.Drawing.Point(13, 260);
            this.m_labelPips.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelPips.Name = "m_labelPips";
            this.m_labelPips.Size = new System.Drawing.Size(119, 31);
            this.m_labelPips.TabIndex = 6;
            this.m_labelPips.Text = "Group of pips:";
            // 
            // m_labelStartIndex
            // 
            this.m_labelStartIndex.Location = new System.Drawing.Point(13, 355);
            this.m_labelStartIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelStartIndex.Name = "m_labelStartIndex";
            this.m_labelStartIndex.Size = new System.Drawing.Size(226, 28);
            this.m_labelStartIndex.TabIndex = 7;
            this.m_labelStartIndex.Text = "Start index:";
            // 
            // m_labelWiresType
            // 
            this.m_labelWiresType.Location = new System.Drawing.Point(12, 309);
            this.m_labelWiresType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelWiresType.Name = "m_labelWiresType";
            this.m_labelWiresType.Size = new System.Drawing.Size(228, 28);
            this.m_labelWiresType.TabIndex = 8;
            this.m_labelWiresType.Text = "Type of wires: \r\n";
            this.m_labelWiresType.Click += new System.EventHandler(this.m_labelWiresType_Click);
            // 
            // m_btnPrintBusInterface
            // 
            this.m_btnPrintBusInterface.Location = new System.Drawing.Point(19, 590);
            this.m_btnPrintBusInterface.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnPrintBusInterface.Name = "m_btnPrintBusInterface";
            this.m_btnPrintBusInterface.Size = new System.Drawing.Size(223, 28);
            this.m_btnPrintBusInterface.TabIndex = 1;
            this.m_btnPrintBusInterface.Text = "Export";
            this.m_btnPrintBusInterface.UseVisualStyleBackColor = true;
            this.m_btnPrintBusInterface.Click += new System.EventHandler(this.m_btnPrintBusInterface_Click);
            // 
            // m_labelExport
            // 
            this.m_labelExport.Location = new System.Drawing.Point(16, 458);
            this.m_labelExport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelExport.Name = "m_labelExport";
            this.m_labelExport.Size = new System.Drawing.Size(133, 28);
            this.m_labelExport.TabIndex = 5;
            this.m_labelExport.Text = "Export in a txt file";
            // 
            // m_labelTCLPath
            // 
            this.m_labelTCLPath.Location = new System.Drawing.Point(12, 170);
            this.m_labelTCLPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelTCLPath.Name = "m_labelTCLPath";
            this.m_labelTCLPath.Size = new System.Drawing.Size(100, 23);
            this.m_labelTCLPath.TabIndex = 9;
            this.m_labelTCLPath.Text = "TCL file Path: ";
            // 
            // m_txtBoxTCLPath
            // 
            this.m_txtBoxTCLPath.Location = new System.Drawing.Point(142, 167);
            this.m_txtBoxTCLPath.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBoxTCLPath.Name = "m_txtBoxTCLPath";
            this.m_txtBoxTCLPath.Size = new System.Drawing.Size(267, 22);
            this.m_txtBoxTCLPath.TabIndex = 3;
            this.m_txtBoxTCLPath.Text = "..\\..\\static_interface_constraints.tcl";
            // 
            // m_fileSelect
            // 
            this.m_fileSelect.Append = false;
            this.m_fileSelect.FileName = "";
            this.m_fileSelect.Filter = "All CSV files|*.csv";
            this.m_fileSelect.Label = "CSV File";
            this.m_fileSelect.Location = new System.Drawing.Point(12, 62);
            this.m_fileSelect.Name = "m_fileSelect";
            this.m_fileSelect.Size = new System.Drawing.Size(400, 100);
            this.m_fileSelect.TabIndex = 10;
            // 
            // m_fileSelectOut
            // 
            this.m_fileSelectOut.Append = true;
            this.m_fileSelectOut.FileName = "";
            this.m_fileSelectOut.Filter = "All Txt files|*.txt";
            this.m_fileSelectOut.Label = "Txt File";
            this.m_fileSelectOut.Location = new System.Drawing.Point(12, 470);
            this.m_fileSelectOut.Name = "m_fileSelectOut";
            this.m_fileSelectOut.Size = new System.Drawing.Size(400, 100);
            this.m_fileSelectOut.TabIndex = 10;
            // 
            // PrintBusInterfaceCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_grpBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrintBusInterfaceCtrl";
            this.Size = new System.Drawing.Size(1051, 710);
            this.m_grpBox.ResumeLayout(false);
            this.m_grpBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_drpDwnSignals)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_drpDwnStartIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_grpBox;
        private FileSelectionCtrl m_fileSelect;
        private FileSelectionCtrl m_fileSelectOut;
        private System.Windows.Forms.Button m_btnPrintBusInterface;
        private System.Windows.Forms.TextBox m_txtBoxTCLPath;

        private System.Windows.Forms.Label m_labelImport;
        private System.Windows.Forms.Label m_labelExport;
        private System.Windows.Forms.Label m_labelBorder;
        private System.Windows.Forms.Label m_labelPips;
        private System.Windows.Forms.Label m_labelWiresType;
        private System.Windows.Forms.Label m_labelStartIndex;
      
        private System.Windows.Forms.Label m_labelTCLPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_drpDwnStartIndex;
        private System.Windows.Forms.NumericUpDown m_drpDwnSignals;

        private System.Windows.Forms.ComboBox m_drpDwnBorder;
        private System.Windows.Forms.ComboBox m_drpDwnWires;
        private System.Windows.Forms.ComboBox m_drpDwnPips;
    }
}
