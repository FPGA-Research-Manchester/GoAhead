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
            this.m_txtBoxBorder = new System.Windows.Forms.TextBox();
            this.m_txtBoxPips = new System.Windows.Forms.TextBox();
            this.m_txtBoxStartIndex = new System.Windows.Forms.TextBox();
            this.m_txtBoxWiresType = new System.Windows.Forms.TextBox();
            this.m_labelBorder = new System.Windows.Forms.Label();
            this.m_labelImport = new System.Windows.Forms.Label();
            this.m_labelExport = new System.Windows.Forms.Label();
            this.m_labelPips = new System.Windows.Forms.Label();
            this.m_labelStartIndex = new System.Windows.Forms.Label();
            this.m_labelWiresType = new System.Windows.Forms.Label();
            this.m_btnPrintBusInterface = new System.Windows.Forms.Button();
         
            this.m_fileSelect = new GoAhead.GUI.FileSelectionCtrl();
            this.m_fileSelectOut = new GoAhead.GUI.FileSelectionCtrl();
            this.m_txtBoxTCLPath = new System.Windows.Forms.TextBox();
            this.m_labelTCLPath = new System.Windows.Forms.Label();
            this.m_grpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_grpBox
            // 
            this.m_grpBox.Controls.Add(this.m_txtBoxBorder);
            this.m_grpBox.Controls.Add(this.m_txtBoxPips);
            this.m_grpBox.Controls.Add(this.m_txtBoxStartIndex);
            this.m_grpBox.Controls.Add(this.m_txtBoxWiresType);
            this.m_grpBox.Controls.Add(this.m_labelBorder);
            this.m_grpBox.Controls.Add(this.m_labelImport);
            this.m_grpBox.Controls.Add(this.m_labelPips);
            this.m_grpBox.Controls.Add(this.m_labelStartIndex);
            this.m_grpBox.Controls.Add(this.m_labelWiresType);
            this.m_grpBox.Controls.Add(this.m_btnPrintBusInterface);
           
            this.m_grpBox.Controls.Add(this.m_fileSelect);
            this.m_grpBox.Controls.Add(this.m_fileSelectOut);
            this.m_grpBox.Controls.Add(this.m_labelExport);
            this.m_grpBox.Controls.Add(this.m_labelTCLPath);
            this.m_grpBox.Controls.Add(this.m_txtBoxTCLPath);

            this.m_grpBox.Location = new System.Drawing.Point(0, 4);
            this.m_grpBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_grpBox.Name = "m_grpBox";
            this.m_grpBox.Padding = new System.Windows.Forms.Padding(4);
            this.m_grpBox.Size = new System.Drawing.Size(1333, 2464);
            this.m_grpBox.TabIndex = 18;
            this.m_grpBox.TabStop = false;
            this.m_grpBox.Text = "Print Bus Interface Constraints";
            this.m_grpBox.Enter += new System.EventHandler(this.m_grpBox_Enter);
            // 
            // m_fileSelect
            // 
            this.m_fileSelect.Append = true;
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
            this.m_fileSelectOut.Location = new System.Drawing.Point(12, 455);
            this.m_fileSelectOut.Name = "m_fileSelectOut";
            this.m_fileSelectOut.Size = new System.Drawing.Size(400, 100);
            this.m_fileSelectOut.TabIndex = 10;
            // 
            // m_txtBoxBorder
            // 
            this.m_txtBoxBorder.Location = new System.Drawing.Point(247, 213);
            this.m_txtBoxBorder.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBoxBorder.Name = "m_txtBoxBorder";
            this.m_txtBoxBorder.Size = new System.Drawing.Size(132, 22);
            this.m_txtBoxBorder.TabIndex = 0;
            this.m_txtBoxBorder.Text = "West";
            // 
            // m_txtBoxPips
            // 
            this.m_txtBoxPips.Location = new System.Drawing.Point(247, 268);
            this.m_txtBoxPips.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBoxPips.Name = "m_txtBoxPips";
            this.m_txtBoxPips.Size = new System.Drawing.Size(132, 22);
            this.m_txtBoxPips.TabIndex = 1;
            this.m_txtBoxPips.Text = "W";
            this.m_txtBoxPips.TextChanged += new System.EventHandler(this.m_txtBoxPips_TextChanged);
            // 
            // m_txtBoxStartIndex
            // 
            this.m_txtBoxStartIndex.Location = new System.Drawing.Point(247, 378);
            this.m_txtBoxStartIndex.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBoxStartIndex.Name = "m_txtBoxStartIndex";
            this.m_txtBoxStartIndex.Size = new System.Drawing.Size(132, 22);
            this.m_txtBoxStartIndex.TabIndex = 2;
            this.m_txtBoxStartIndex.Text = "0";
            // 
            // m_txtBoxWiresType
            // 
            this.m_txtBoxWiresType.Location = new System.Drawing.Point(247, 326);
            this.m_txtBoxWiresType.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBoxWiresType.Name = "m_txtBoxWiresType";
            this.m_txtBoxWiresType.Size = new System.Drawing.Size(132, 22);
            this.m_txtBoxWiresType.TabIndex = 3;
            this.m_txtBoxWiresType.Text = "2";
            // 
            // m_labelBorder
            // 
            this.m_labelBorder.Location = new System.Drawing.Point(11, 213);
            this.m_labelBorder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelBorder.Name = "m_labelBorder";
            this.m_labelBorder.Size = new System.Drawing.Size(231, 28);
            this.m_labelBorder.TabIndex = 4;
            this.m_labelBorder.Text = "Choose a valid border: West/East";
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
            // m_labelExport
            // 
            this.m_labelExport.Location = new System.Drawing.Point(13, 430);
            this.m_labelExport.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelExport.Name = "m_labelExport";
            this.m_labelExport.Size = new System.Drawing.Size(133, 28);
            this.m_labelExport.TabIndex = 5;
            this.m_labelExport.Text = "Export in a txt file";
            // 
            // m_labelPips
            // 
            this.m_labelPips.Location = new System.Drawing.Point(11, 271);
            this.m_labelPips.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelPips.Name = "m_labelPips";
            this.m_labelPips.Size = new System.Drawing.Size(228, 28);
            this.m_labelPips.TabIndex = 6;
            this.m_labelPips.Text = "Choose a valid Group of pips: W/E";
            this.m_labelPips.Text = "Choose a valid Group of pips: W/E";
            // 
            // m_labelStartIndex
            // 
            this.m_labelStartIndex.Location = new System.Drawing.Point(13, 378);
            this.m_labelStartIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelStartIndex.Name = "m_labelStartIndex";
            this.m_labelStartIndex.Size = new System.Drawing.Size(226, 28);
            this.m_labelStartIndex.TabIndex = 7;
            this.m_labelStartIndex.Text = "Choose a valid Start index >= 0";
            // 
            // m_labelWiresType
            // 
            this.m_labelWiresType.Location = new System.Drawing.Point(11, 329);
            this.m_labelWiresType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_labelWiresType.Name = "m_labelWiresType";
            this.m_labelWiresType.Size = new System.Drawing.Size(228, 28);
            this.m_labelWiresType.TabIndex = 8;
            this.m_labelWiresType.Text = "Choose a valid type of wires: 2/4";
            this.m_labelWiresType.Click += new System.EventHandler(this.m_labelWiresType_Click);
            // 
            // m_btnPrintBusInterface
            // 
            this.m_btnPrintBusInterface.Location = new System.Drawing.Point(19, 570);
            this.m_btnPrintBusInterface.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnPrintBusInterface.Name = "m_btnPrintBusInterface";
            this.m_btnPrintBusInterface.Size = new System.Drawing.Size(223, 28);
            this.m_btnPrintBusInterface.TabIndex = 1;
            this.m_btnPrintBusInterface.Text = "Export";
            this.m_btnPrintBusInterface.UseVisualStyleBackColor = true;
            this.m_btnPrintBusInterface.Click += new System.EventHandler(this.m_btnPrintBusInterface_Click);
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
            this.m_txtBoxTCLPath.Location = new System.Drawing.Point(110, 165);
            this.m_txtBoxTCLPath.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBoxTCLPath.Name = "m_txtBoxTCLPath";
            this.m_txtBoxTCLPath.Size = new System.Drawing.Size(267, 23);
            this.m_txtBoxTCLPath.TabIndex = 3;
            this.m_txtBoxTCLPath.Text = @"..\..\static_interface_constraints.tcl";
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_grpBox;
        private FileSelectionCtrl m_fileSelect;
        private FileSelectionCtrl m_fileSelectOut;
        private System.Windows.Forms.Button m_btnPrintBusInterface;
        private System.Windows.Forms.TextBox m_txtBoxBorder;
        private System.Windows.Forms.TextBox m_txtBoxPips;
        private System.Windows.Forms.TextBox m_txtBoxWiresType;
        private System.Windows.Forms.TextBox m_txtBoxStartIndex;
        private System.Windows.Forms.TextBox m_txtBoxTCLPath;


        private System.Windows.Forms.Label m_labelImport;
        private System.Windows.Forms.Label m_labelExport;
        private System.Windows.Forms.Label m_labelBorder;
        private System.Windows.Forms.Label m_labelPips;
        private System.Windows.Forms.Label m_labelWiresType;
        private System.Windows.Forms.Label m_labelStartIndex;
      
        private System.Windows.Forms.Label m_labelTCLPath;
    }
}
