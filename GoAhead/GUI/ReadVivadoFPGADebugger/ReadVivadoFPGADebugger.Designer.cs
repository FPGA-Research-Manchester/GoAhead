using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoAhead.GUI.ReadVivadoFPGADebugger
{
    partial class ReadVivadoFPGADebugger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadVivadoFPGADebugger));
            this.lbl1_exportFormat = new System.Windows.Forms.Label();
            this.txtbox_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.formatPanel = new System.Windows.Forms.Panel();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_general = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBox_excludePips = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage_wires = new System.Windows.Forms.TabPage();
            this.table_wiresTab = new System.Windows.Forms.TableLayoutPanel();
            this.flow_wireIgnoresExpand = new System.Windows.Forms.FlowLayoutPanel();
            this.lbl_wireIgnoresExpand = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.table_ignores = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.table_includes = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel_ignoreIdenticalWires = new System.Windows.Forms.Panel();
            this.checkBox_IIW = new System.Windows.Forms.CheckBox();
            this.flow_wireIncludesExpand = new System.Windows.Forms.FlowLayoutPanel();
            this.lbl_wireIncludesExpand = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_disable = new System.Windows.Forms.Button();
            this.btn_proceed = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_path = new System.Windows.Forms.Button();
            this.btn_wireIgnoresExpand = new System.Windows.Forms.Button();
            this.btn_addWireIgnore = new System.Windows.Forms.Button();
            this.btn_addWireInclude = new System.Windows.Forms.Button();
            this.btn_wireIncludesExpand = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.formatPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_general.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.tabPage_wires.SuspendLayout();
            this.table_wiresTab.SuspendLayout();
            this.flow_wireIgnoresExpand.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.table_ignores.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.table_includes.SuspendLayout();
            this.panel_ignoreIdenticalWires.SuspendLayout();
            this.flow_wireIncludesExpand.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl1_exportFormat
            // 
            this.lbl1_exportFormat.AutoSize = true;
            this.lbl1_exportFormat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.lbl1_exportFormat.Location = new System.Drawing.Point(8, 5);
            this.lbl1_exportFormat.Name = "lbl1_exportFormat";
            this.lbl1_exportFormat.Size = new System.Drawing.Size(62, 18);
            this.lbl1_exportFormat.TabIndex = 2;
            this.lbl1_exportFormat.Text = "Format";
            // 
            // txtbox_path
            // 
            this.txtbox_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbox_path.BackColor = System.Drawing.SystemColors.Control;
            this.txtbox_path.Location = new System.Drawing.Point(48, 69);
            this.txtbox_path.Name = "txtbox_path";
            this.txtbox_path.Size = new System.Drawing.Size(623, 24);
            this.txtbox_path.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(8, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 18);
            this.label1.TabIndex = 13;
            this.label1.Text = "Path";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 18);
            this.label4.TabIndex = 15;
            this.label4.Text = "Set output text file:";
            // 
            // formatPanel
            // 
            this.formatPanel.BackColor = System.Drawing.Color.Transparent;
            this.formatPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.formatPanel.Controls.Add(this.radioButton3);
            this.formatPanel.Controls.Add(this.button2);
            this.formatPanel.Controls.Add(this.lbl1_exportFormat);
            this.formatPanel.Controls.Add(this.radioButton4);
            this.formatPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formatPanel.Location = new System.Drawing.Point(4, 121);
            this.formatPanel.Name = "formatPanel";
            this.formatPanel.Padding = new System.Windows.Forms.Padding(5);
            this.formatPanel.Size = new System.Drawing.Size(679, 110);
            this.formatPanel.TabIndex = 17;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(11, 34);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(93, 22);
            this.radioButton3.TabIndex = 11;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Complete";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(11, 62);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(90, 22);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Compact";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_general);
            this.tabControl1.Controls.Add(this.tabPage_wires);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(8, 5);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(715, 393);
            this.tabControl1.TabIndex = 24;
            // 
            // tabPage_general
            // 
            this.tabPage_general.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_general.Controls.Add(this.tableLayoutPanel1);
            this.tabPage_general.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_general.Location = new System.Drawing.Point(4, 33);
            this.tabPage_general.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage_general.Name = "tabPage_general";
            this.tabPage_general.Padding = new System.Windows.Forms.Padding(10);
            this.tabPage_general.Size = new System.Drawing.Size(707, 356);
            this.tabPage_general.TabIndex = 0;
            this.tabPage_general.Text = "General";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.formatPanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel10, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(687, 336);
            this.tableLayoutPanel1.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtbox_path);
            this.panel1.Controls.Add(this.btn_path);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(679, 110);
            this.panel1.TabIndex = 25;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.label14);
            this.panel10.Controls.Add(this.checkBox_excludePips);
            this.panel10.Controls.Add(this.label5);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(4, 238);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(5);
            this.panel10.Size = new System.Drawing.Size(679, 94);
            this.panel10.TabIndex = 26;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(8, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(563, 18);
            this.label14.TabIndex = 16;
            this.label14.Text = "Disabling this option will greatly lower the runtime of the ReadVivadoFPGA comman" +
    "d";
            // 
            // checkBox_excludePips
            // 
            this.checkBox_excludePips.AutoSize = true;
            this.checkBox_excludePips.Location = new System.Drawing.Point(11, 38);
            this.checkBox_excludePips.Name = "checkBox_excludePips";
            this.checkBox_excludePips.Size = new System.Drawing.Size(341, 22);
            this.checkBox_excludePips.TabIndex = 24;
            this.checkBox_excludePips.Text = "ExcludePipsToBidirectionalWiresFromBlocking";
            this.checkBox_excludePips.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(8, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(205, 18);
            this.label5.TabIndex = 23;
            this.label5.Text = "ReadVivadoFPGA Options";
            // 
            // tabPage_wires
            // 
            this.tabPage_wires.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_wires.Controls.Add(this.panel2);
            this.tabPage_wires.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_wires.Location = new System.Drawing.Point(4, 33);
            this.tabPage_wires.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage_wires.Name = "tabPage_wires";
            this.tabPage_wires.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_wires.Size = new System.Drawing.Size(707, 356);
            this.tabPage_wires.TabIndex = 1;
            this.tabPage_wires.Text = "Wires";
            // 
            // table_wiresTab
            // 
            this.table_wiresTab.AutoSize = true;
            this.table_wiresTab.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.table_wiresTab.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.table_wiresTab.ColumnCount = 1;
            this.table_wiresTab.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_wiresTab.Controls.Add(this.flow_wireIgnoresExpand, 0, 3);
            this.table_wiresTab.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.table_wiresTab.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.table_wiresTab.Controls.Add(this.panel_ignoreIdenticalWires, 0, 0);
            this.table_wiresTab.Controls.Add(this.flow_wireIncludesExpand, 0, 1);
            this.table_wiresTab.Dock = System.Windows.Forms.DockStyle.Top;
            this.table_wiresTab.Location = new System.Drawing.Point(0, 0);
            this.table_wiresTab.Name = "table_wiresTab";
            this.table_wiresTab.RowCount = 6;
            this.table_wiresTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table_wiresTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table_wiresTab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.table_wiresTab.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table_wiresTab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.table_wiresTab.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table_wiresTab.Size = new System.Drawing.Size(701, 153);
            this.table_wiresTab.TabIndex = 16;
            // 
            // flow_wireIgnoresExpand
            // 
            this.flow_wireIgnoresExpand.AutoSize = true;
            this.flow_wireIgnoresExpand.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flow_wireIgnoresExpand.Controls.Add(this.btn_wireIgnoresExpand);
            this.flow_wireIgnoresExpand.Controls.Add(this.lbl_wireIgnoresExpand);
            this.flow_wireIgnoresExpand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flow_wireIgnoresExpand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow_wireIgnoresExpand.Location = new System.Drawing.Point(4, 109);
            this.flow_wireIgnoresExpand.Name = "flow_wireIgnoresExpand";
            this.flow_wireIgnoresExpand.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.flow_wireIgnoresExpand.Size = new System.Drawing.Size(693, 38);
            this.flow_wireIgnoresExpand.TabIndex = 19;
            this.flow_wireIgnoresExpand.Click += new System.EventHandler(this.OnExpandRegexTableClicked);
            // 
            // lbl_wireIgnoresExpand
            // 
            this.lbl_wireIgnoresExpand.AutoSize = true;
            this.lbl_wireIgnoresExpand.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_wireIgnoresExpand.Enabled = false;
            this.lbl_wireIgnoresExpand.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_wireIgnoresExpand.Location = new System.Drawing.Point(35, 6);
            this.lbl_wireIgnoresExpand.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_wireIgnoresExpand.Name = "lbl_wireIgnoresExpand";
            this.lbl_wireIgnoresExpand.Size = new System.Drawing.Size(101, 26);
            this.lbl_wireIgnoresExpand.TabIndex = 1;
            this.lbl_wireIgnoresExpand.Text = "Ignores (0)";
            this.lbl_wireIgnoresExpand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btn_addWireIgnore, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.table_ignores, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(4, 154);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(693, 1);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // table_ignores
            // 
            this.table_ignores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.table_ignores.AutoSize = true;
            this.table_ignores.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.table_ignores.ColumnCount = 5;
            this.table_ignores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_ignores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_ignores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_ignores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_ignores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.table_ignores.Controls.Add(this.label9, 3, 0);
            this.table_ignores.Controls.Add(this.label10, 0, 0);
            this.table_ignores.Controls.Add(this.label11, 2, 0);
            this.table_ignores.Controls.Add(this.label12, 1, 0);
            this.table_ignores.Controls.Add(this.label13, 4, 0);
            this.table_ignores.Location = new System.Drawing.Point(3, 3);
            this.table_ignores.Name = "table_ignores";
            this.table_ignores.RowCount = 1;
            this.table_ignores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.table_ignores.Size = new System.Drawing.Size(687, 29);
            this.table_ignores.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(497, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(156, 25);
            this.label9.TabIndex = 7;
            this.label9.Text = "End Port";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(156, 25);
            this.label10.TabIndex = 0;
            this.label10.Text = "Start Tile";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(333, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(156, 25);
            this.label11.TabIndex = 6;
            this.label11.Text = "End Tile";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(169, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(156, 25);
            this.label12.TabIndex = 3;
            this.label12.Text = "Start Port";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(661, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(21, 25);
            this.label13.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.table_includes, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btn_addWireInclude, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(4, 108);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(693, 1);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // table_includes
            // 
            this.table_includes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.table_includes.AutoSize = true;
            this.table_includes.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.table_includes.ColumnCount = 5;
            this.table_includes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_includes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_includes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_includes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_includes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.table_includes.Controls.Add(this.label8, 3, 0);
            this.table_includes.Controls.Add(this.label3, 0, 0);
            this.table_includes.Controls.Add(this.label7, 2, 0);
            this.table_includes.Controls.Add(this.label6, 1, 0);
            this.table_includes.Controls.Add(this.label15, 4, 0);
            this.table_includes.Location = new System.Drawing.Point(3, 3);
            this.table_includes.Name = "table_includes";
            this.table_includes.RowCount = 1;
            this.table_includes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.table_includes.Size = new System.Drawing.Size(687, 29);
            this.table_includes.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(497, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(156, 25);
            this.label8.TabIndex = 7;
            this.label8.Text = "End Port";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Start Tile";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(333, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "End Tile";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(169, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 25);
            this.label6.TabIndex = 3;
            this.label6.Text = "Start Port";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(661, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(21, 25);
            this.label15.TabIndex = 8;
            // 
            // panel_ignoreIdenticalWires
            // 
            this.panel_ignoreIdenticalWires.AutoSize = true;
            this.panel_ignoreIdenticalWires.Controls.Add(this.checkBox_IIW);
            this.panel_ignoreIdenticalWires.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ignoreIdenticalWires.Location = new System.Drawing.Point(4, 4);
            this.panel_ignoreIdenticalWires.Name = "panel_ignoreIdenticalWires";
            this.panel_ignoreIdenticalWires.Padding = new System.Windows.Forms.Padding(6, 15, 6, 15);
            this.panel_ignoreIdenticalWires.Size = new System.Drawing.Size(693, 52);
            this.panel_ignoreIdenticalWires.TabIndex = 17;
            // 
            // checkBox_IIW
            // 
            this.checkBox_IIW.AutoSize = true;
            this.checkBox_IIW.Checked = true;
            this.checkBox_IIW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_IIW.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBox_IIW.Location = new System.Drawing.Point(6, 15);
            this.checkBox_IIW.Margin = new System.Windows.Forms.Padding(0);
            this.checkBox_IIW.Name = "checkBox_IIW";
            this.checkBox_IIW.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.checkBox_IIW.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkBox_IIW.Size = new System.Drawing.Size(681, 22);
            this.checkBox_IIW.TabIndex = 2;
            this.checkBox_IIW.Text = "Ignore wires with identical start and end locations";
            this.checkBox_IIW.UseVisualStyleBackColor = true;
            // 
            // flow_wireIncludesExpand
            // 
            this.flow_wireIncludesExpand.AutoSize = true;
            this.flow_wireIncludesExpand.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flow_wireIncludesExpand.Controls.Add(this.btn_wireIncludesExpand);
            this.flow_wireIncludesExpand.Controls.Add(this.lbl_wireIncludesExpand);
            this.flow_wireIncludesExpand.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flow_wireIncludesExpand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow_wireIncludesExpand.Location = new System.Drawing.Point(4, 63);
            this.flow_wireIncludesExpand.Name = "flow_wireIncludesExpand";
            this.flow_wireIncludesExpand.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.flow_wireIncludesExpand.Size = new System.Drawing.Size(693, 38);
            this.flow_wireIncludesExpand.TabIndex = 18;
            this.flow_wireIncludesExpand.Click += new System.EventHandler(this.OnExpandRegexTableClicked);
            // 
            // lbl_wireIncludesExpand
            // 
            this.lbl_wireIncludesExpand.AutoSize = true;
            this.lbl_wireIncludesExpand.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbl_wireIncludesExpand.Enabled = false;
            this.lbl_wireIncludesExpand.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_wireIncludesExpand.Location = new System.Drawing.Point(35, 6);
            this.lbl_wireIncludesExpand.Margin = new System.Windows.Forms.Padding(3);
            this.lbl_wireIncludesExpand.Name = "lbl_wireIncludesExpand";
            this.lbl_wireIncludesExpand.Size = new System.Drawing.Size(109, 26);
            this.lbl_wireIncludesExpand.TabIndex = 1;
            this.lbl_wireIncludesExpand.Text = "Includes (0)";
            this.lbl_wireIncludesExpand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel4.Controls.Add(this.btn_disable, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.btn_proceed, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 396);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(715, 57);
            this.tableLayoutPanel4.TabIndex = 25;
            // 
            // btn_disable
            // 
            this.btn_disable.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_disable.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_disable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_disable.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_disable.FlatAppearance.BorderSize = 0;
            this.btn_disable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btn_disable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_disable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_disable.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_disable.Image = ((System.Drawing.Image)(resources.GetObject("btn_disable.Image")));
            this.btn_disable.Location = new System.Drawing.Point(464, 1);
            this.btn_disable.Margin = new System.Windows.Forms.Padding(0);
            this.btn_disable.Name = "btn_disable";
            this.btn_disable.Size = new System.Drawing.Size(250, 55);
            this.btn_disable.TabIndex = 1;
            this.btn_disable.Text = "Reset and Disable Debugger";
            this.btn_disable.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_disable.UseVisualStyleBackColor = false;
            this.btn_disable.Click += new System.EventHandler(this.OnCancel);
            // 
            // btn_proceed
            // 
            this.btn_proceed.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btn_proceed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_proceed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_proceed.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_proceed.FlatAppearance.BorderSize = 0;
            this.btn_proceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_proceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_proceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_proceed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_proceed.Image = ((System.Drawing.Image)(resources.GetObject("btn_proceed.Image")));
            this.btn_proceed.Location = new System.Drawing.Point(1, 1);
            this.btn_proceed.Margin = new System.Windows.Forms.Padding(0);
            this.btn_proceed.Name = "btn_proceed";
            this.btn_proceed.Size = new System.Drawing.Size(462, 55);
            this.btn_proceed.TabIndex = 0;
            this.btn_proceed.Text = "Apply Debug Settings and proceed to ReadVivadoFPGA";
            this.btn_proceed.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_proceed.UseVisualStyleBackColor = false;
            this.btn_proceed.Click += new System.EventHandler(this.OnProceed);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::GoAhead.Properties.Resources.icons8_help_24;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Cursor = System.Windows.Forms.Cursors.Help;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(76, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(20, 20);
            this.button2.TabIndex = 22;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_path
            // 
            this.btn_path.BackgroundImage = global::GoAhead.Properties.Resources.icons8_browse_folder_30;
            this.btn_path.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_path.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_path.FlatAppearance.BorderSize = 0;
            this.btn_path.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_path.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_path.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_path.Location = new System.Drawing.Point(11, 67);
            this.btn_path.Name = "btn_path";
            this.btn_path.Size = new System.Drawing.Size(26, 26);
            this.btn_path.TabIndex = 6;
            this.btn_path.Text = "...";
            this.btn_path.UseVisualStyleBackColor = true;
            this.btn_path.Click += new System.EventHandler(this.pathBtn_Click);
            // 
            // btn_wireIgnoresExpand
            // 
            this.btn_wireIgnoresExpand.BackgroundImage = global::GoAhead.Properties.Resources.icons8_collapse_arrow_64;
            this.btn_wireIgnoresExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_wireIgnoresExpand.Enabled = false;
            this.btn_wireIgnoresExpand.FlatAppearance.BorderSize = 0;
            this.btn_wireIgnoresExpand.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btn_wireIgnoresExpand.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btn_wireIgnoresExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_wireIgnoresExpand.Location = new System.Drawing.Point(3, 6);
            this.btn_wireIgnoresExpand.Name = "btn_wireIgnoresExpand";
            this.btn_wireIgnoresExpand.Size = new System.Drawing.Size(26, 26);
            this.btn_wireIgnoresExpand.TabIndex = 0;
            this.btn_wireIgnoresExpand.UseVisualStyleBackColor = true;
            // 
            // btn_addWireIgnore
            // 
            this.btn_addWireIgnore.BackgroundImage = global::GoAhead.Properties.Resources.icons8_add_48;
            this.btn_addWireIgnore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_addWireIgnore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_addWireIgnore.FlatAppearance.BorderSize = 0;
            this.btn_addWireIgnore.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btn_addWireIgnore.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btn_addWireIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_addWireIgnore.Location = new System.Drawing.Point(3, 38);
            this.btn_addWireIgnore.Name = "btn_addWireIgnore";
            this.btn_addWireIgnore.Size = new System.Drawing.Size(36, 36);
            this.btn_addWireIgnore.TabIndex = 1;
            this.btn_addWireIgnore.UseVisualStyleBackColor = true;
            this.btn_addWireIgnore.Click += new System.EventHandler(this.btn_addWireIgnore_Click);
            // 
            // btn_addWireInclude
            // 
            this.btn_addWireInclude.BackgroundImage = global::GoAhead.Properties.Resources.icons8_add_48;
            this.btn_addWireInclude.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_addWireInclude.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_addWireInclude.FlatAppearance.BorderSize = 0;
            this.btn_addWireInclude.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btn_addWireInclude.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btn_addWireInclude.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_addWireInclude.Location = new System.Drawing.Point(3, 38);
            this.btn_addWireInclude.Name = "btn_addWireInclude";
            this.btn_addWireInclude.Size = new System.Drawing.Size(36, 36);
            this.btn_addWireInclude.TabIndex = 1;
            this.btn_addWireInclude.UseVisualStyleBackColor = true;
            this.btn_addWireInclude.Click += new System.EventHandler(this.btn_addWireInclude_Click);
            // 
            // btn_wireIncludesExpand
            // 
            this.btn_wireIncludesExpand.BackgroundImage = global::GoAhead.Properties.Resources.icons8_collapse_arrow_64;
            this.btn_wireIncludesExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_wireIncludesExpand.Enabled = false;
            this.btn_wireIncludesExpand.FlatAppearance.BorderSize = 0;
            this.btn_wireIncludesExpand.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btn_wireIncludesExpand.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btn_wireIncludesExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_wireIncludesExpand.Location = new System.Drawing.Point(3, 6);
            this.btn_wireIncludesExpand.Name = "btn_wireIncludesExpand";
            this.btn_wireIncludesExpand.Size = new System.Drawing.Size(26, 26);
            this.btn_wireIncludesExpand.TabIndex = 0;
            this.btn_wireIncludesExpand.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.table_wiresTab);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(701, 350);
            this.panel2.TabIndex = 20;
            // 
            // ReadVivadoFPGADebugger
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(715, 453);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(0, 500);
            this.Name = "ReadVivadoFPGADebugger";
            this.Text = "ReadVivadoFPGA Debugger";
            this.formatPanel.ResumeLayout(false);
            this.formatPanel.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_general.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.tabPage_wires.ResumeLayout(false);
            this.table_wiresTab.ResumeLayout(false);
            this.table_wiresTab.PerformLayout();
            this.flow_wireIgnoresExpand.ResumeLayout(false);
            this.flow_wireIgnoresExpand.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.table_ignores.ResumeLayout(false);
            this.table_ignores.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.table_includes.ResumeLayout(false);
            this.table_includes.PerformLayout();
            this.panel_ignoreIdenticalWires.ResumeLayout(false);
            this.panel_ignoreIdenticalWires.PerformLayout();
            this.flow_wireIncludesExpand.ResumeLayout(false);
            this.flow_wireIncludesExpand.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        private void Flow_Expand_MouseHover(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
        }

        private void Flow_Expand_MouseLeave(object sender, EventArgs e)
        {
            ((Control)sender).BackColor = System.Drawing.SystemColors.ControlLight;
        }

        #endregion

        private System.Windows.Forms.Button btn_proceed;
        private System.Windows.Forms.Label lbl1_exportFormat;
        private System.Windows.Forms.TextBox txtbox_path;
        private System.Windows.Forms.Button btn_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel formatPanel;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_general;
        private System.Windows.Forms.TabPage tabPage_wires;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_IIW;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.CheckBox checkBox_excludePips;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btn_disable;
        private System.Windows.Forms.Button btn_addWireInclude;
        private System.Windows.Forms.TableLayoutPanel table_includes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TableLayoutPanel table_wiresTab;
        private System.Windows.Forms.Panel panel_ignoreIdenticalWires;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btn_addWireIgnore;
        private System.Windows.Forms.TableLayoutPanel table_ignores;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.FlowLayoutPanel flow_wireIncludesExpand;
        private System.Windows.Forms.Button btn_wireIncludesExpand;
        private System.Windows.Forms.Label lbl_wireIncludesExpand;
        private System.Windows.Forms.FlowLayoutPanel flow_wireIgnoresExpand;
        private System.Windows.Forms.Button btn_wireIgnoresExpand;
        private System.Windows.Forms.Label lbl_wireIgnoresExpand;
        private Panel panel2;
    }
}
