namespace GoAhead.GUI.Macros.LibraryElementInstantiation
{
    partial class LibraryElementInstantiationSelectorCtrl
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
            this.m_grdViewInstances = new System.Windows.Forms.DataGridView();
            this.m_lblOverride = new System.Windows.Forms.Label();
            this.m_lblAutofilter = new System.Windows.Forms.Label();
            this.m_lblManualFilter = new System.Windows.Forms.Label();
            this.m_txtManualFilter = new System.Windows.Forms.TextBox();
            this.m_txtAutoFilter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewInstances)).BeginInit();
            this.SuspendLayout();
            // 
            // m_grdViewInstances
            // 
            this.m_grdViewInstances.AllowUserToAddRows = false;
            this.m_grdViewInstances.AllowUserToDeleteRows = false;
            this.m_grdViewInstances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_grdViewInstances.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.m_grdViewInstances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_grdViewInstances.Dock = System.Windows.Forms.DockStyle.Top;
            this.m_grdViewInstances.Location = new System.Drawing.Point(0, 0);
            this.m_grdViewInstances.Name = "m_grdViewInstances";
            this.m_grdViewInstances.ReadOnly = true;
            this.m_grdViewInstances.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_grdViewInstances.Size = new System.Drawing.Size(777, 239);
            this.m_grdViewInstances.TabIndex = 1;
            this.m_grdViewInstances.SelectionChanged += new System.EventHandler(this.m_grdViewInstances_SelectionChanged);
            // 
            // m_lblOverride
            // 
            this.m_lblOverride.AutoSize = true;
            this.m_lblOverride.Location = new System.Drawing.Point(5, 285);
            this.m_lblOverride.Name = "m_lblOverride";
            this.m_lblOverride.Size = new System.Drawing.Size(106, 13);
            this.m_lblOverride.TabIndex = 23;
            this.m_lblOverride.Text = "(overrides Auto Filter)";
            // 
            // m_lblAutofilter
            // 
            this.m_lblAutofilter.AutoSize = true;
            this.m_lblAutofilter.Location = new System.Drawing.Point(6, 248);
            this.m_lblAutofilter.Name = "m_lblAutofilter";
            this.m_lblAutofilter.Size = new System.Drawing.Size(54, 13);
            this.m_lblAutofilter.TabIndex = 22;
            this.m_lblAutofilter.Text = "Auto Filter";
            // 
            // m_lblManualFilter
            // 
            this.m_lblManualFilter.AutoSize = true;
            this.m_lblManualFilter.Location = new System.Drawing.Point(5, 273);
            this.m_lblManualFilter.Name = "m_lblManualFilter";
            this.m_lblManualFilter.Size = new System.Drawing.Size(67, 13);
            this.m_lblManualFilter.TabIndex = 21;
            this.m_lblManualFilter.Text = "Manual Filter";
            // 
            // m_txtManualFilter
            // 
            this.m_txtManualFilter.Location = new System.Drawing.Point(117, 273);
            this.m_txtManualFilter.Name = "m_txtManualFilter";
            this.m_txtManualFilter.Size = new System.Drawing.Size(656, 20);
            this.m_txtManualFilter.TabIndex = 20;
            this.m_txtManualFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_txtManualFilter_KeyUp);
            // 
            // m_txtAutoFilter
            // 
            this.m_txtAutoFilter.Location = new System.Drawing.Point(118, 245);
            this.m_txtAutoFilter.Name = "m_txtAutoFilter";
            this.m_txtAutoFilter.Size = new System.Drawing.Size(656, 20);
            this.m_txtAutoFilter.TabIndex = 19;
            // 
            // LibraryElementInstantiationSelectorCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_txtManualFilter);
            this.Controls.Add(this.m_txtAutoFilter);
            this.Controls.Add(this.m_grdViewInstances);
            this.Controls.Add(this.m_lblOverride);
            this.Controls.Add(this.m_lblAutofilter);
            this.Controls.Add(this.m_lblManualFilter);
            this.Name = "LibraryElementInstantiationSelectorCtrl";
            this.Size = new System.Drawing.Size(777, 305);
            this.Resize += new System.EventHandler(this.LibraryElementInstantiationSelectorCtrl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.m_grdViewInstances)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_grdViewInstances;
        private System.Windows.Forms.Label m_lblOverride;
        private System.Windows.Forms.Label m_lblAutofilter;
        private System.Windows.Forms.Label m_lblManualFilter;
        private System.Windows.Forms.TextBox m_txtManualFilter;
        private System.Windows.Forms.TextBox m_txtAutoFilter;
    }
}
