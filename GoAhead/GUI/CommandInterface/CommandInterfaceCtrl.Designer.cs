namespace GoAhead.GUI
{
    partial class CommandInterfaceCtrl
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
            this.m_cmdBoxCommands = new System.Windows.Forms.ComboBox();
            this.m_lblCommand = new System.Windows.Forms.Label();
            this.m_dataGrdArguments = new System.Windows.Forms.DataGridView();
            this.m_colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_lblArgs = new System.Windows.Forms.Label();
            this.m_btnExecute = new System.Windows.Forms.Button();
            this.m_lblCommandAction = new System.Windows.Forms.Label();
            this.m_txtAction = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrdArguments)).BeginInit();
            this.SuspendLayout();
            // 
            // m_cmdBoxCommands
            // 
            this.m_cmdBoxCommands.FormattingEnabled = true;
            this.m_cmdBoxCommands.Location = new System.Drawing.Point(13, 21);
            this.m_cmdBoxCommands.MaxDropDownItems = 16;
            this.m_cmdBoxCommands.Name = "m_cmdBoxCommands";
            this.m_cmdBoxCommands.Size = new System.Drawing.Size(1217, 21);
            this.m_cmdBoxCommands.Sorted = true;
            this.m_cmdBoxCommands.TabIndex = 0;
            this.m_cmdBoxCommands.SelectedIndexChanged += new System.EventHandler(this.m_cmdBoxCommands_SelectedIndexChanged);
            this.m_cmdBoxCommands.TextChanged += new System.EventHandler(this.m_cmdBoxCommands_TextChanged);
            // 
            // m_lblCommand
            // 
            this.m_lblCommand.AutoSize = true;
            this.m_lblCommand.Location = new System.Drawing.Point(10, 5);
            this.m_lblCommand.Name = "m_lblCommand";
            this.m_lblCommand.Size = new System.Drawing.Size(59, 13);
            this.m_lblCommand.TabIndex = 1;
            this.m_lblCommand.Text = "Commands";
            // 
            // m_dataGrdArguments
            // 
            this.m_dataGrdArguments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_dataGrdArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataGrdArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_colName,
            this.m_value,
            this.m_colType});
            this.m_dataGrdArguments.Location = new System.Drawing.Point(15, 123);
            this.m_dataGrdArguments.Name = "m_dataGrdArguments";
            this.m_dataGrdArguments.Size = new System.Drawing.Size(1217, 412);
            this.m_dataGrdArguments.TabIndex = 2;
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
            // m_colType
            // 
            this.m_colType.HeaderText = "Type";
            this.m_colType.Name = "m_colType";
            // 
            // m_lblArgs
            // 
            this.m_lblArgs.AutoSize = true;
            this.m_lblArgs.Location = new System.Drawing.Point(12, 256);
            this.m_lblArgs.Name = "m_lblArgs";
            this.m_lblArgs.Size = new System.Drawing.Size(57, 13);
            this.m_lblArgs.TabIndex = 3;
            this.m_lblArgs.Text = "Arguments";
            // 
            // m_btnExecute
            // 
            this.m_btnExecute.Location = new System.Drawing.Point(1155, 553);
            this.m_btnExecute.Name = "m_btnExecute";
            this.m_btnExecute.Size = new System.Drawing.Size(75, 23);
            this.m_btnExecute.TabIndex = 4;
            this.m_btnExecute.Text = "Execute";
            this.m_btnExecute.UseVisualStyleBackColor = true;
            this.m_btnExecute.Click += new System.EventHandler(this.m_btnExecute_Click);
            // 
            // m_lblCommandAction
            // 
            this.m_lblCommandAction.AutoSize = true;
            this.m_lblCommandAction.Location = new System.Drawing.Point(12, 45);
            this.m_lblCommandAction.Name = "m_lblCommandAction";
            this.m_lblCommandAction.Size = new System.Drawing.Size(37, 13);
            this.m_lblCommandAction.TabIndex = 5;
            this.m_lblCommandAction.Text = "Action";
            // 
            // m_txtAction
            // 
            this.m_txtAction.Location = new System.Drawing.Point(15, 62);
            this.m_txtAction.Name = "m_txtAction";
            this.m_txtAction.ReadOnly = true;
            this.m_txtAction.Size = new System.Drawing.Size(1215, 41);
            this.m_txtAction.TabIndex = 6;
            this.m_txtAction.Text = "";
            // 
            // CommandInterfaceCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_txtAction);
            this.Controls.Add(this.m_lblCommandAction);
            this.Controls.Add(this.m_btnExecute);
            this.Controls.Add(this.m_dataGrdArguments);
            this.Controls.Add(this.m_cmdBoxCommands);
            this.Controls.Add(this.m_lblArgs);
            this.Controls.Add(this.m_lblCommand);
            this.Name = "CommandInterfaceCtrl";
            this.Size = new System.Drawing.Size(1241, 591);
            this.Load += new System.EventHandler(this.CommandInterfaceCtrl_Load);
            this.Resize += new System.EventHandler(this.CommandInterfaceCtrl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGrdArguments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox m_cmdBoxCommands;
        private System.Windows.Forms.Label m_lblCommand;
        private System.Windows.Forms.DataGridView m_dataGrdArguments;
        private System.Windows.Forms.Label m_lblArgs;
        private System.Windows.Forms.Button m_btnExecute;
        private System.Windows.Forms.Label m_lblCommandAction;
        private System.Windows.Forms.RichTextBox m_txtAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_colType;
    }
}
