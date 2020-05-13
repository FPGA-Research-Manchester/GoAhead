namespace GoAhead.GUI
{
    partial class TileViewCtrl
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
            this.m_txtBox = new System.Windows.Forms.RichTextBox();
            this.m_tabCtrlTop = new System.Windows.Forms.TabControl();
            this.m_tabText = new System.Windows.Forms.TabPage();
            this.m_tabSwitchMatrix = new System.Windows.Forms.TabPage();
            this.m_grpFilter = new System.Windows.Forms.GroupBox();
            this.m_txtOutFilter = new System.Windows.Forms.TextBox();
            this.m_lblOutFilter = new System.Windows.Forms.Label();
            this.m_txtInFilter = new System.Windows.Forms.TextBox();
            this.m_lblInFilter = new System.Windows.Forms.Label();
            this.m_lblShowTimes = new System.Windows.Forms.Label();
            this.m_cbxShowTimes = new System.Windows.Forms.CheckBox();
            this.m_lblOutFilterValid = new System.Windows.Forms.Label();
            this.m_lblInFilterValid = new System.Windows.Forms.Label();
            this.m_grdViewSwitchMatrix = new System.Windows.Forms.DataGridView();
            this.m_tabWires = new System.Windows.Forms.TabPage();
            this.m_grdViewWires = new System.Windows.Forms.DataGridView();
            this.m_localPip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_locapPipIsDriver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_pipOnOtherTile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_xIncr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_yIncr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_targetTile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_tabLUTRouting = new System.Windows.Forms.TabPage();
            this.m_grpBoxLutRoutingFilter = new System.Windows.Forms.GroupBox();
            this.m_txtLRLUTInFilter = new System.Windows.Forms.TextBox();
            this.m_txtLRBegFilter = new System.Windows.Forms.TextBox();
            this.m_txtLREndFilter = new System.Windows.Forms.TextBox();
            this.m_txtLRLutOutFilter = new System.Windows.Forms.TextBox();
            this.m_grdViewLUTRouting = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_in = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_out = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timing_port1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timing_port2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timing_attribute3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timing_attribute4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timing_attribute2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timing_attribute1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_tabCtrlTop.SuspendLayout();
            this.m_tabText.SuspendLayout();
            this.m_tabSwitchMatrix.SuspendLayout();
            this.m_grpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewSwitchMatrix)).BeginInit();
            this.m_tabWires.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewWires)).BeginInit();
            this.m_tabLUTRouting.SuspendLayout();
            this.m_grpBoxLutRoutingFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewLUTRouting)).BeginInit();
            this.SuspendLayout();
            // 
            // m_txtBox
            // 
            this.m_txtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_txtBox.Location = new System.Drawing.Point(4, 4);
            this.m_txtBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtBox.Name = "m_txtBox";
            this.m_txtBox.ReadOnly = true;
            this.m_txtBox.Size = new System.Drawing.Size(1103, 619);
            this.m_txtBox.TabIndex = 0;
            this.m_txtBox.Text = "";
            // 
            // m_tabCtrlTop
            // 
            this.m_tabCtrlTop.Controls.Add(this.m_tabText);
            this.m_tabCtrlTop.Controls.Add(this.m_tabSwitchMatrix);
            this.m_tabCtrlTop.Controls.Add(this.m_tabWires);
            this.m_tabCtrlTop.Controls.Add(this.m_tabLUTRouting);
            this.m_tabCtrlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabCtrlTop.Location = new System.Drawing.Point(0, 0);
            this.m_tabCtrlTop.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabCtrlTop.Name = "m_tabCtrlTop";
            this.m_tabCtrlTop.SelectedIndex = 0;
            this.m_tabCtrlTop.Size = new System.Drawing.Size(1119, 656);
            this.m_tabCtrlTop.TabIndex = 1;
            // 
            // m_tabText
            // 
            this.m_tabText.Controls.Add(this.m_txtBox);
            this.m_tabText.Location = new System.Drawing.Point(4, 25);
            this.m_tabText.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabText.Name = "m_tabText";
            this.m_tabText.Padding = new System.Windows.Forms.Padding(4);
            this.m_tabText.Size = new System.Drawing.Size(1111, 627);
            this.m_tabText.TabIndex = 0;
            this.m_tabText.Text = "Text";
            this.m_tabText.UseVisualStyleBackColor = true;
            // 
            // m_tabSwitchMatrix
            // 
            this.m_tabSwitchMatrix.Controls.Add(this.m_grpFilter);
            this.m_tabSwitchMatrix.Controls.Add(this.m_lblOutFilterValid);
            this.m_tabSwitchMatrix.Controls.Add(this.m_lblInFilterValid);
            this.m_tabSwitchMatrix.Controls.Add(this.m_grdViewSwitchMatrix);
            this.m_tabSwitchMatrix.Location = new System.Drawing.Point(4, 25);
            this.m_tabSwitchMatrix.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabSwitchMatrix.Name = "m_tabSwitchMatrix";
            this.m_tabSwitchMatrix.Padding = new System.Windows.Forms.Padding(4);
            this.m_tabSwitchMatrix.Size = new System.Drawing.Size(1111, 627);
            this.m_tabSwitchMatrix.TabIndex = 1;
            this.m_tabSwitchMatrix.Text = "Switch Matrix";
            this.m_tabSwitchMatrix.UseVisualStyleBackColor = true;
            this.m_tabSwitchMatrix.Resize += new System.EventHandler(this.m_tabSwitchMatrix_Resize);
            // 
            // m_grpFilter
            // 
            this.m_grpFilter.Controls.Add(this.m_txtOutFilter);
            this.m_grpFilter.Controls.Add(this.m_lblOutFilter);
            this.m_grpFilter.Controls.Add(this.m_txtInFilter);
            this.m_grpFilter.Controls.Add(this.m_lblInFilter);
            this.m_grpFilter.Controls.Add(this.m_lblShowTimes);
            this.m_grpFilter.Controls.Add(this.m_cbxShowTimes);
            this.m_grpFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpFilter.Location = new System.Drawing.Point(4, 4);
            this.m_grpFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_grpFilter.Name = "m_grpFilter";
            this.m_grpFilter.Padding = new System.Windows.Forms.Padding(4);
            this.m_grpFilter.Size = new System.Drawing.Size(1103, 60);
            this.m_grpFilter.TabIndex = 8;
            this.m_grpFilter.TabStop = false;
            this.m_grpFilter.Text = "Filter";
            // 
            // m_txtOutFilter
            // 
            this.m_txtOutFilter.Location = new System.Drawing.Point(657, 23);
            this.m_txtOutFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtOutFilter.Name = "m_txtOutFilter";
            this.m_txtOutFilter.Size = new System.Drawing.Size(207, 22);
            this.m_txtOutFilter.TabIndex = 5;
            this.m_txtOutFilter.Text = ".*";
            this.m_txtOutFilter.TextChanged += new System.EventHandler(this.m_txtOutFilter_TextChanged);
            // 
            // m_lblOutFilter
            // 
            this.m_lblOutFilter.AutoSize = true;
            this.m_lblOutFilter.Location = new System.Drawing.Point(583, 23);
            this.m_lblOutFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblOutFilter.Name = "m_lblOutFilter";
            this.m_lblOutFilter.Size = new System.Drawing.Size(66, 17);
            this.m_lblOutFilter.TabIndex = 2;
            this.m_lblOutFilter.Text = "Out Filter";
            // 
            // m_txtInFilter
            // 
            this.m_txtInFilter.Location = new System.Drawing.Point(69, 23);
            this.m_txtInFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtInFilter.Name = "m_txtInFilter";
            this.m_txtInFilter.Size = new System.Drawing.Size(207, 22);
            this.m_txtInFilter.TabIndex = 3;
            this.m_txtInFilter.Text = ".*";
            this.m_txtInFilter.TextChanged += new System.EventHandler(this.m_txtInFilter_TextChanged);
            // 
            // m_lblInFilter
            // 
            this.m_lblInFilter.AutoSize = true;
            this.m_lblInFilter.Location = new System.Drawing.Point(7, 23);
            this.m_lblInFilter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblInFilter.Name = "m_lblInFilter";
            this.m_lblInFilter.Size = new System.Drawing.Size(54, 17);
            this.m_lblInFilter.TabIndex = 4;
            this.m_lblInFilter.Text = "In Filter";
            // 
            // m_lblShowTimes
            // 
            this.m_lblShowTimes.AutoSize = true;
            this.m_lblShowTimes.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_lblShowTimes.Location = new System.Drawing.Point(964, 19);
            this.m_lblShowTimes.Margin = new System.Windows.Forms.Padding(4);
            this.m_lblShowTimes.Name = "m_lblShowTimes";
            this.m_lblShowTimes.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.m_lblShowTimes.Size = new System.Drawing.Size(111, 25);
            this.m_lblShowTimes.TabIndex = 2;
            this.m_lblShowTimes.Text = "Show Time Data";
            this.m_lblShowTimes.Visible = false;
            // 
            // m_cbxShowTimes
            // 
            this.m_cbxShowTimes.Checked = true;
            this.m_cbxShowTimes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbxShowTimes.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_cbxShowTimes.Location = new System.Drawing.Point(1075, 19);
            this.m_cbxShowTimes.Name = "m_cbxShowTimes";
            this.m_cbxShowTimes.Size = new System.Drawing.Size(24, 37);
            this.m_cbxShowTimes.TabIndex = 6;
            this.m_cbxShowTimes.CheckedChanged += new System.EventHandler(this.CbxShowTimes_CheckedChanged);
            this.m_cbxShowTimes.Visible = false;
            // 
            // m_lblOutFilterValid
            // 
            this.m_lblOutFilterValid.AutoSize = true;
            this.m_lblOutFilterValid.Location = new System.Drawing.Point(875, 7);
            this.m_lblOutFilterValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblOutFilterValid.Name = "m_lblOutFilterValid";
            this.m_lblOutFilterValid.Size = new System.Drawing.Size(0, 17);
            this.m_lblOutFilterValid.TabIndex = 7;
            // 
            // m_lblInFilterValid
            // 
            this.m_lblInFilterValid.AutoSize = true;
            this.m_lblInFilterValid.Location = new System.Drawing.Point(284, 6);
            this.m_lblInFilterValid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblInFilterValid.Name = "m_lblInFilterValid";
            this.m_lblInFilterValid.Size = new System.Drawing.Size(0, 17);
            this.m_lblInFilterValid.TabIndex = 6;
            // 
            // m_grdViewSwitchMatrix
            // 
            this.m_grdViewSwitchMatrix.AllowUserToAddRows = false;
            this.m_grdViewSwitchMatrix.AllowUserToDeleteRows = false;
            this.m_grdViewSwitchMatrix.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewSwitchMatrix.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewSwitchMatrix.Location = new System.Drawing.Point(4, 260);
            this.m_grdViewSwitchMatrix.Margin = new System.Windows.Forms.Padding(4);
            this.m_grdViewSwitchMatrix.Name = "m_grdViewSwitchMatrix";
            this.m_grdViewSwitchMatrix.ReadOnly = true;
            this.m_grdViewSwitchMatrix.Size = new System.Drawing.Size(1100, 364);
            this.m_grdViewSwitchMatrix.TabIndex = 0;
            // 
            // m_tabWires
            // 
            this.m_tabWires.Controls.Add(this.m_grdViewWires);
            this.m_tabWires.Location = new System.Drawing.Point(4, 25);
            this.m_tabWires.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabWires.Name = "m_tabWires";
            this.m_tabWires.Size = new System.Drawing.Size(1111, 627);
            this.m_tabWires.TabIndex = 2;
            this.m_tabWires.Text = "Wires";
            this.m_tabWires.UseVisualStyleBackColor = true;
            // 
            // m_grdViewWires
            // 
            this.m_grdViewWires.AllowUserToAddRows = false;
            this.m_grdViewWires.AllowUserToDeleteRows = false;
            this.m_grdViewWires.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewWires.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewWires.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_localPip,
            this.m_locapPipIsDriver,
            this.m_pipOnOtherTile,
            this.m_xIncr,
            this.m_yIncr,
            this.m_targetTile});
            this.m_grdViewWires.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_grdViewWires.Location = new System.Drawing.Point(0, 0);
            this.m_grdViewWires.Margin = new System.Windows.Forms.Padding(4);
            this.m_grdViewWires.Name = "m_grdViewWires";
            this.m_grdViewWires.ReadOnly = true;
            this.m_grdViewWires.Size = new System.Drawing.Size(1111, 627);
            this.m_grdViewWires.TabIndex = 1;
            // 
            // m_localPip
            // 
            this.m_localPip.HeaderText = "LocalPip";
            this.m_localPip.Name = "m_localPip";
            this.m_localPip.ReadOnly = true;
            // 
            // m_locapPipIsDriver
            // 
            this.m_locapPipIsDriver.HeaderText = "LocapPipIsDriver";
            this.m_locapPipIsDriver.Name = "m_locapPipIsDriver";
            this.m_locapPipIsDriver.ReadOnly = true;
            // 
            // m_pipOnOtherTile
            // 
            this.m_pipOnOtherTile.HeaderText = "PipOnOtherTile";
            this.m_pipOnOtherTile.Name = "m_pipOnOtherTile";
            this.m_pipOnOtherTile.ReadOnly = true;
            // 
            // m_xIncr
            // 
            this.m_xIncr.HeaderText = "XIncr";
            this.m_xIncr.Name = "m_xIncr";
            this.m_xIncr.ReadOnly = true;
            // 
            // m_yIncr
            // 
            this.m_yIncr.HeaderText = "YIncr";
            this.m_yIncr.Name = "m_yIncr";
            this.m_yIncr.ReadOnly = true;
            // 
            // m_targetTile
            // 
            this.m_targetTile.HeaderText = "Target";
            this.m_targetTile.Name = "m_targetTile";
            this.m_targetTile.ReadOnly = true;
            // 
            // m_tabLUTRouting
            // 
            this.m_tabLUTRouting.Controls.Add(this.m_grpBoxLutRoutingFilter);
            this.m_tabLUTRouting.Controls.Add(this.m_grdViewLUTRouting);
            this.m_tabLUTRouting.Location = new System.Drawing.Point(4, 25);
            this.m_tabLUTRouting.Margin = new System.Windows.Forms.Padding(4);
            this.m_tabLUTRouting.Name = "m_tabLUTRouting";
            this.m_tabLUTRouting.Size = new System.Drawing.Size(1111, 627);
            this.m_tabLUTRouting.TabIndex = 3;
            this.m_tabLUTRouting.Text = "LUT Routing";
            this.m_tabLUTRouting.UseVisualStyleBackColor = true;
            // 
            // m_grpBoxLutRoutingFilter
            // 
            this.m_grpBoxLutRoutingFilter.Controls.Add(this.m_txtLRLUTInFilter);
            this.m_grpBoxLutRoutingFilter.Controls.Add(this.m_txtLRBegFilter);
            this.m_grpBoxLutRoutingFilter.Controls.Add(this.m_txtLREndFilter);
            this.m_grpBoxLutRoutingFilter.Controls.Add(this.m_txtLRLutOutFilter);
            this.m_grpBoxLutRoutingFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grpBoxLutRoutingFilter.Location = new System.Drawing.Point(0, 0);
            this.m_grpBoxLutRoutingFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_grpBoxLutRoutingFilter.Name = "m_grpBoxLutRoutingFilter";
            this.m_grpBoxLutRoutingFilter.Padding = new System.Windows.Forms.Padding(4);
            this.m_grpBoxLutRoutingFilter.Size = new System.Drawing.Size(1111, 60);
            this.m_grpBoxLutRoutingFilter.TabIndex = 9;
            this.m_grpBoxLutRoutingFilter.TabStop = false;
            this.m_grpBoxLutRoutingFilter.Text = "Filter";
            // 
            // m_txtLRLUTInFilter
            // 
            this.m_txtLRLUTInFilter.Location = new System.Drawing.Point(845, 18);
            this.m_txtLRLUTInFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtLRLUTInFilter.Name = "m_txtLRLUTInFilter";
            this.m_txtLRLUTInFilter.Size = new System.Drawing.Size(143, 22);
            this.m_txtLRLUTInFilter.TabIndex = 10;
            this.m_txtLRLUTInFilter.Text = ".*";
            this.m_txtLRLUTInFilter.TextChanged += new System.EventHandler(this.m_txtLRLUTInFilter_TextChanged);
            // 
            // m_txtLRBegFilter
            // 
            this.m_txtLRBegFilter.Location = new System.Drawing.Point(587, 18);
            this.m_txtLRBegFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtLRBegFilter.Name = "m_txtLRBegFilter";
            this.m_txtLRBegFilter.Size = new System.Drawing.Size(143, 22);
            this.m_txtLRBegFilter.TabIndex = 8;
            this.m_txtLRBegFilter.Text = ".*";
            this.m_txtLRBegFilter.TextChanged += new System.EventHandler(this.m_txtLRBegFilter_TextChanged);
            // 
            // m_txtLREndFilter
            // 
            this.m_txtLREndFilter.Location = new System.Drawing.Point(321, 18);
            this.m_txtLREndFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtLREndFilter.Name = "m_txtLREndFilter";
            this.m_txtLREndFilter.Size = new System.Drawing.Size(143, 22);
            this.m_txtLREndFilter.TabIndex = 7;
            this.m_txtLREndFilter.Text = ".*";
            this.m_txtLREndFilter.TextChanged += new System.EventHandler(this.m_txtLREndFilter_TextChanged);
            // 
            // m_txtLRLutOutFilter
            // 
            this.m_txtLRLutOutFilter.Location = new System.Drawing.Point(67, 18);
            this.m_txtLRLutOutFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtLRLutOutFilter.Name = "m_txtLRLutOutFilter";
            this.m_txtLRLutOutFilter.Size = new System.Drawing.Size(143, 22);
            this.m_txtLRLutOutFilter.TabIndex = 3;
            this.m_txtLRLutOutFilter.Text = ".*";
            this.m_txtLRLutOutFilter.TextChanged += new System.EventHandler(this.m_txtLRLutOutFilter_TextChanged);
            // 
            // m_grdViewLUTRouting
            // 
            this.m_grdViewLUTRouting.AllowUserToAddRows = false;
            this.m_grdViewLUTRouting.AllowUserToDeleteRows = false;
            this.m_grdViewLUTRouting.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewLUTRouting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewLUTRouting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn0,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.m_grdViewLUTRouting.Location = new System.Drawing.Point(0, 68);
            this.m_grdViewLUTRouting.Margin = new System.Windows.Forms.Padding(4);
            this.m_grdViewLUTRouting.Name = "m_grdViewLUTRouting";
            this.m_grdViewLUTRouting.ReadOnly = true;
            this.m_grdViewLUTRouting.Size = new System.Drawing.Size(1108, 556);
            this.m_grdViewLUTRouting.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn0
            // 
            this.dataGridViewTextBoxColumn0.HeaderText = "LUT-Output";
            this.dataGridViewTextBoxColumn0.Name = "dataGridViewTextBoxColumn0";
            this.dataGridViewTextBoxColumn0.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "In";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Out";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "LUT-Input";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // m_in
            // 
            this.m_in.HeaderText = "In";
            this.m_in.Name = "m_in";
            this.m_in.ReadOnly = true;
            // 
            // m_out
            // 
            this.m_out.HeaderText = "Out";
            this.m_out.Name = "m_out";
            this.m_out.ReadOnly = true;
            // 
            // timing_port1
            // 
            this.timing_port1.HeaderText = "From Port";
            this.timing_port1.Name = "timing_port1";
            this.timing_port1.ReadOnly = true;
            // 
            // timing_port2
            // 
            this.timing_port2.HeaderText = "To Port";
            this.timing_port2.Name = "timing_port2";
            this.timing_port2.ReadOnly = true;
            // 
            // timing_attribute3
            // 
            this.timing_attribute3.Name = "timing_attribute3";
            this.timing_attribute3.ReadOnly = true;
            // 
            // timing_attribute4
            // 
            this.timing_attribute4.Name = "timing_attribute4";
            this.timing_attribute4.ReadOnly = true;
            // 
            // timing_attribute2
            // 
            this.timing_attribute2.Name = "timing_attribute2";
            this.timing_attribute2.ReadOnly = true;
            // 
            // timing_attribute1
            // 
            this.timing_attribute1.Name = "timing_attribute1";
            this.timing_attribute1.ReadOnly = true;
            // 
            // TileViewCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabCtrlTop);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TileViewCtrl";
            this.Size = new System.Drawing.Size(1119, 656);
            this.m_tabCtrlTop.ResumeLayout(false);
            this.m_tabText.ResumeLayout(false);
            this.m_tabSwitchMatrix.ResumeLayout(false);
            this.m_tabSwitchMatrix.PerformLayout();
            this.m_grpFilter.ResumeLayout(false);
            this.m_grpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewSwitchMatrix)).EndInit();
            this.m_tabWires.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewWires)).EndInit();
            this.m_tabLUTRouting.ResumeLayout(false);
            this.m_grpBoxLutRoutingFilter.ResumeLayout(false);
            this.m_grpBoxLutRoutingFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewLUTRouting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox m_txtBox;
        private System.Windows.Forms.TabControl m_tabCtrlTop;
        private System.Windows.Forms.TabPage m_tabText;
        private System.Windows.Forms.TabPage m_tabSwitchMatrix;
        private System.Windows.Forms.DataGridView m_grdViewSwitchMatrix;
        private System.Windows.Forms.TabPage m_tabWires;
        private System.Windows.Forms.DataGridView m_grdViewWires;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_localPip;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_locapPipIsDriver;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_pipOnOtherTile;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_xIncr;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_yIncr;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_targetTile;
        private System.Windows.Forms.TabPage m_tabLUTRouting;
        private System.Windows.Forms.DataGridView m_grdViewLUTRouting;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_in;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_out;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn0;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label m_lblInFilter;
        private System.Windows.Forms.TextBox m_txtInFilter;
        private System.Windows.Forms.Label m_lblOutFilter;
        private System.Windows.Forms.TextBox m_txtOutFilter;
        private System.Windows.Forms.Label m_lblShowTimes;
        private System.Windows.Forms.CheckBox m_cbxShowTimes;
        private System.Windows.Forms.Label m_lblInFilterValid;
        private System.Windows.Forms.Label m_lblOutFilterValid;
        private System.Windows.Forms.GroupBox m_grpFilter;
        private System.Windows.Forms.GroupBox m_grpBoxLutRoutingFilter;
        private System.Windows.Forms.TextBox m_txtLRLUTInFilter;
        private System.Windows.Forms.TextBox m_txtLRBegFilter;
        private System.Windows.Forms.TextBox m_txtLREndFilter;
        private System.Windows.Forms.TextBox m_txtLRLutOutFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn timing_port1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timing_port2;
        private System.Windows.Forms.DataGridViewTextBoxColumn timing_attribute1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timing_attribute2;
        private System.Windows.Forms.DataGridViewTextBoxColumn timing_attribute3;
        private System.Windows.Forms.DataGridViewTextBoxColumn timing_attribute4;        
    }
}
