namespace GoAhead.GUI.LibraryElementInstantiation
{
    partial class LibraryElementInstantiationManagerCtrl
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
            this.m_btnAnnotateSignals = new System.Windows.Forms.Button();
            this.m_grdViewMapping = new System.Windows.Forms.DataGridView();
            this.m_port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_signal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_mapping = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.m_grpAnoteSignals = new System.Windows.Forms.GroupBox();
            this.m_libElInstSelector = new GoAhead.GUI.Macros.LibraryElementInstantiation.LibraryElementInstantiationSelectorCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewMapping)).BeginInit();
            this.m_grpAnoteSignals.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_btnAnnotateSignals
            // 
            this.m_btnAnnotateSignals.Location = new System.Drawing.Point(605, 123);
            this.m_btnAnnotateSignals.Name = "m_btnAnnotateSignals";
            this.m_btnAnnotateSignals.Size = new System.Drawing.Size(106, 25);
            this.m_btnAnnotateSignals.TabIndex = 11;
            this.m_btnAnnotateSignals.Text = "Annotate Signals";
            this.m_btnAnnotateSignals.UseVisualStyleBackColor = true;
            this.m_btnAnnotateSignals.Click += new System.EventHandler(this.m_btnAnnotateSignals_Click);
            // 
            // m_grdViewMapping
            // 
            this.m_grdViewMapping.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewMapping.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.m_grdViewMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewMapping.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_port,
            this.m_signal,
            this.m_mapping});
            this.m_grdViewMapping.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grdViewMapping.Location = new System.Drawing.Point(3, 16);
            this.m_grdViewMapping.Name = "m_grdViewMapping";
            this.m_grdViewMapping.Size = new System.Drawing.Size(717, 101);
            this.m_grdViewMapping.TabIndex = 12;
            // 
            // m_port
            // 
            this.m_port.HeaderText = "Port";
            this.m_port.Name = "m_port";
            this.m_port.ReadOnly = true;
            // 
            // m_signal
            // 
            this.m_signal.HeaderText = "Signal";
            this.m_signal.Name = "m_signal";
            // 
            // m_mapping
            // 
            this.m_mapping.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.m_mapping.HeaderText = "Mapping";
            this.m_mapping.Items.AddRange(new object[] {
            "external",
            "internal",
            "no_vector"});
            this.m_mapping.Name = "m_mapping";
            // 
            // m_grpAnoteSignals
            // 
            this.m_grpAnoteSignals.Controls.Add(this.m_grdViewMapping);
            this.m_grpAnoteSignals.Controls.Add(this.m_btnAnnotateSignals);
            this.m_grpAnoteSignals.Location = new System.Drawing.Point(0, 325);
            this.m_grpAnoteSignals.Name = "m_grpAnoteSignals";
            this.m_grpAnoteSignals.Size = new System.Drawing.Size(723, 162);
            this.m_grpAnoteSignals.TabIndex = 13;
            this.m_grpAnoteSignals.TabStop = false;
            this.m_grpAnoteSignals.Text = "Annotate Signals";
            // 
            // m_libElInstSelector
            // 
            this.m_libElInstSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_libElInstSelector.Location = new System.Drawing.Point(0, 0);
            this.m_libElInstSelector.Name = "m_libElInstSelector";
            this.m_libElInstSelector.Size = new System.Drawing.Size(784, 319);
            this.m_libElInstSelector.TabIndex = 15;
            // 
            // LibraryElementInstantiationManagerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_libElInstSelector);
            this.Controls.Add(this.m_grpAnoteSignals);
            this.Name = "LibraryElementInstantiationManagerCtrl";
            this.Size = new System.Drawing.Size(784, 494);
            this.Resize += new System.EventHandler(this.LibraryElementInstantiationManagerCtrl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewMapping)).EndInit();
            this.m_grpAnoteSignals.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_btnAnnotateSignals;
        private System.Windows.Forms.DataGridView m_grdViewMapping;
        private System.Windows.Forms.GroupBox m_grpAnoteSignals;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_port;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_signal;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_mapping;
        private Macros.LibraryElementInstantiation.LibraryElementInstantiationSelectorCtrl m_libElInstSelector;
    }
}
