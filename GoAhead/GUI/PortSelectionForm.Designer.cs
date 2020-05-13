namespace GoAhead.GUI
{
    partial class PortSelectionForm
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_lstAvailablePorts = new System.Windows.Forms.ListBox();
            this.m_lstSelectedPorts = new System.Windows.Forms.ListBox();
            this.m_btnAdd = new System.Windows.Forms.Button();
            this.m_btnRemove = new System.Windows.Forms.Button();
            this.m_txtFilter = new System.Windows.Forms.TextBox();
            this.m_btnReset = new System.Windows.Forms.Button();
            this.m_btnBlock = new System.Windows.Forms.Button();
            this.m_lblAvailabePorts = new System.Windows.Forms.Label();
            this.m_lblSelectedPorts = new System.Windows.Forms.Label();
            this.m_chkkInvert = new System.Windows.Forms.CheckBox();
            this.m_btnRemovePortsFromNets = new System.Windows.Forms.Button();
            this.m_lblRegexpError = new System.Windows.Forms.Label();
            this.m_chkIncludeAllPorts = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_lstAvailablePorts
            // 
            this.m_lstAvailablePorts.FormattingEnabled = true;
            this.m_lstAvailablePorts.ItemHeight = 16;
            this.m_lstAvailablePorts.Location = new System.Drawing.Point(4, 27);
            this.m_lstAvailablePorts.Margin = new System.Windows.Forms.Padding(4);
            this.m_lstAvailablePorts.Name = "m_lstAvailablePorts";
            this.m_lstAvailablePorts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.m_lstAvailablePorts.Size = new System.Drawing.Size(264, 468);
            this.m_lstAvailablePorts.TabIndex = 0;
            // 
            // m_lstSelectedPorts
            // 
            this.m_lstSelectedPorts.FormattingEnabled = true;
            this.m_lstSelectedPorts.ItemHeight = 16;
            this.m_lstSelectedPorts.Location = new System.Drawing.Point(416, 27);
            this.m_lstSelectedPorts.Margin = new System.Windows.Forms.Padding(4);
            this.m_lstSelectedPorts.Name = "m_lstSelectedPorts";
            this.m_lstSelectedPorts.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.m_lstSelectedPorts.Size = new System.Drawing.Size(264, 468);
            this.m_lstSelectedPorts.TabIndex = 1;
            // 
            // m_btnAdd
            // 
            this.m_btnAdd.Location = new System.Drawing.Point(287, 27);
            this.m_btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(100, 28);
            this.m_btnAdd.TabIndex = 2;
            this.m_btnAdd.Text = "Add";
            this.m_btnAdd.UseVisualStyleBackColor = true;
            this.m_btnAdd.Click += new System.EventHandler(this.m_btnAdd_Click);
            // 
            // m_btnRemove
            // 
            this.m_btnRemove.Location = new System.Drawing.Point(287, 70);
            this.m_btnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(100, 28);
            this.m_btnRemove.TabIndex = 3;
            this.m_btnRemove.Text = "Remove";
            this.m_btnRemove.UseVisualStyleBackColor = true;
            this.m_btnRemove.Click += new System.EventHandler(this.m_btnRemove_Click);
            // 
            // m_txtFilter
            // 
            this.m_txtFilter.Location = new System.Drawing.Point(8, 530);
            this.m_txtFilter.Margin = new System.Windows.Forms.Padding(4);
            this.m_txtFilter.Name = "m_txtFilter";
            this.m_txtFilter.Size = new System.Drawing.Size(195, 22);
            this.m_txtFilter.TabIndex = 4;
            this.m_txtFilter.TextChanged += new System.EventHandler(this.m_txtFilter_TextChanged);
            // 
            // m_btnReset
            // 
            this.m_btnReset.Location = new System.Drawing.Point(287, 468);
            this.m_btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnReset.Name = "m_btnReset";
            this.m_btnReset.Size = new System.Drawing.Size(100, 28);
            this.m_btnReset.TabIndex = 6;
            this.m_btnReset.Text = "Reset";
            this.m_btnReset.UseVisualStyleBackColor = true;
            this.m_btnReset.Click += new System.EventHandler(this.m_btnReset_Click);
            // 
            // m_btnBlock
            // 
            this.m_btnBlock.Location = new System.Drawing.Point(496, 510);
            this.m_btnBlock.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnBlock.Name = "m_btnBlock";
            this.m_btnBlock.Size = new System.Drawing.Size(184, 30);
            this.m_btnBlock.TabIndex = 7;
            this.m_btnBlock.Text = "Set Ports Usage";
            this.m_btnBlock.UseVisualStyleBackColor = true;
            this.m_btnBlock.Click += new System.EventHandler(this.m_btnBlock_Click);
            // 
            // m_lblAvailabePorts
            // 
            this.m_lblAvailabePorts.AutoSize = true;
            this.m_lblAvailabePorts.Location = new System.Drawing.Point(4, 4);
            this.m_lblAvailabePorts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblAvailabePorts.Name = "m_lblAvailabePorts";
            this.m_lblAvailabePorts.Size = new System.Drawing.Size(102, 17);
            this.m_lblAvailabePorts.TabIndex = 8;
            this.m_lblAvailabePorts.Text = "Available Ports";
            // 
            // m_lblSelectedPorts
            // 
            this.m_lblSelectedPorts.AutoSize = true;
            this.m_lblSelectedPorts.Location = new System.Drawing.Point(412, 4);
            this.m_lblSelectedPorts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblSelectedPorts.Name = "m_lblSelectedPorts";
            this.m_lblSelectedPorts.Size = new System.Drawing.Size(100, 17);
            this.m_lblSelectedPorts.TabIndex = 9;
            this.m_lblSelectedPorts.Text = "Selected Ports";
            // 
            // m_chkkInvert
            // 
            this.m_chkkInvert.AutoSize = true;
            this.m_chkkInvert.Location = new System.Drawing.Point(8, 564);
            this.m_chkkInvert.Margin = new System.Windows.Forms.Padding(4);
            this.m_chkkInvert.Name = "m_chkkInvert";
            this.m_chkkInvert.Size = new System.Drawing.Size(192, 21);
            this.m_chkkInvert.TabIndex = 10;
            this.m_chkkInvert.Text = "Invert Regular Expression";
            this.m_chkkInvert.UseVisualStyleBackColor = true;
            // 
            // m_btnRemovePortsFromNets
            // 
            this.m_btnRemovePortsFromNets.Location = new System.Drawing.Point(245, 537);
            this.m_btnRemovePortsFromNets.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnRemovePortsFromNets.Name = "m_btnRemovePortsFromNets";
            this.m_btnRemovePortsFromNets.Size = new System.Drawing.Size(193, 48);
            this.m_btnRemovePortsFromNets.TabIndex = 11;
            this.m_btnRemovePortsFromNets.Text = "Remove Ports From all existing Nets (Expert)";
            this.m_btnRemovePortsFromNets.UseVisualStyleBackColor = true;
            this.m_btnRemovePortsFromNets.Click += new System.EventHandler(this.m_btnRemovePortsFromNets_Click);
            // 
            // m_lblRegexpError
            // 
            this.m_lblRegexpError.AutoSize = true;
            this.m_lblRegexpError.Location = new System.Drawing.Point(177, 510);
            this.m_lblRegexpError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_lblRegexpError.Name = "m_lblRegexpError";
            this.m_lblRegexpError.Size = new System.Drawing.Size(0, 17);
            this.m_lblRegexpError.TabIndex = 13;
            // 
            // m_chkIncludeAllPorts
            // 
            this.m_chkIncludeAllPorts.AutoSize = true;
            this.m_chkIncludeAllPorts.Checked = true;
            this.m_chkIncludeAllPorts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_chkIncludeAllPorts.Location = new System.Drawing.Point(496, 585);
            this.m_chkIncludeAllPorts.Margin = new System.Windows.Forms.Padding(4);
            this.m_chkIncludeAllPorts.Name = "m_chkIncludeAllPorts";
            this.m_chkIncludeAllPorts.Size = new System.Drawing.Size(184, 21);
            this.m_chkIncludeAllPorts.TabIndex = 14;
            this.m_chkIncludeAllPorts.Text = "Include Reachable Ports";
            this.m_chkIncludeAllPorts.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(496, 547);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(184, 24);
            this.comboBox1.TabIndex = 15;
            // 
            // PortSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 619);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.m_chkIncludeAllPorts);
            this.Controls.Add(this.m_lblRegexpError);
            this.Controls.Add(this.m_btnRemovePortsFromNets);
            this.Controls.Add(this.m_chkkInvert);
            this.Controls.Add(this.m_lblSelectedPorts);
            this.Controls.Add(this.m_lblAvailabePorts);
            this.Controls.Add(this.m_btnBlock);
            this.Controls.Add(this.m_btnReset);
            this.Controls.Add(this.m_txtFilter);
            this.Controls.Add(this.m_btnRemove);
            this.Controls.Add(this.m_btnAdd);
            this.Controls.Add(this.m_lstSelectedPorts);
            this.Controls.Add(this.m_lstAvailablePorts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PortSelectionForm";
            this.Text = "Define Tunnel Wires";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PortSelectionForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox m_lstAvailablePorts;
        private System.Windows.Forms.ListBox m_lstSelectedPorts;
        private System.Windows.Forms.Button m_btnAdd;
        private System.Windows.Forms.Button m_btnRemove;
        private System.Windows.Forms.TextBox m_txtFilter;
        private System.Windows.Forms.Button m_btnReset;
        private System.Windows.Forms.Button m_btnBlock;
        private System.Windows.Forms.Label m_lblAvailabePorts;
        private System.Windows.Forms.Label m_lblSelectedPorts;
        private System.Windows.Forms.CheckBox m_chkkInvert;
        private System.Windows.Forms.Button m_btnRemovePortsFromNets;
        private System.Windows.Forms.Label m_lblRegexpError;
        private System.Windows.Forms.CheckBox m_chkIncludeAllPorts;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}