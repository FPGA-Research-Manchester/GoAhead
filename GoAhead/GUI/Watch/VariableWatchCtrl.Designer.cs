namespace GoAhead.GUI.Watch
{
    partial class VariableWatchCtrl
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
            this.m_dataGrdArguments = new System.Windows.Forms.DataGridView();
            this.m_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrdArguments)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataGrdArguments
            // 
            this.m_dataGrdArguments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_dataGrdArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataGrdArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_colName,
            this.m_value});
            this.m_dataGrdArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dataGrdArguments.Location = new System.Drawing.Point(0, 0);
            this.m_dataGrdArguments.Name = "m_dataGrdArguments";
            this.m_dataGrdArguments.Size = new System.Drawing.Size(573, 432);
            this.m_dataGrdArguments.TabIndex = 3;
            // 
            // m_colName
            // 
            this.m_colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.m_colName.HeaderText = "Name";
            this.m_colName.Name = "m_colName";
            this.m_colName.Width = 60;
            // 
            // m_value
            // 
            this.m_value.HeaderText = "Value";
            this.m_value.Name = "m_value";
            // 
            // VariableWatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_dataGrdArguments);
            this.Name = "VariableWatch";
            this.Size = new System.Drawing.Size(573, 432);
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrdArguments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView m_dataGrdArguments;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_value;
    }
}
