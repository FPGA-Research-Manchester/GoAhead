namespace GoAhead.GUI
{
    partial class NetlistContainerCtrl
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
            this.m_txtMacroName = new System.Windows.Forms.TextBox();
            this.m_btnAdd = new System.Windows.Forms.Button();
            this.m_lblAddMacro = new System.Windows.Forms.Label();
            this.m_grpBoxNetlistContainerCtrl = new System.Windows.Forms.GroupBox();
            this.m_grpBoxNetlistContainerCommands = new System.Windows.Forms.GroupBox();
            this.m_lblDesignBrowser = new System.Windows.Forms.Label();
            this.m_btnDesignBrowser = new System.Windows.Forms.Button();
            this.m_lblFuse = new System.Windows.Forms.Label();
            this.m_lblCutOff = new System.Windows.Forms.Label();
            this.m_lblStatistics = new System.Windows.Forms.Label();
            this.m_lblRead = new System.Windows.Forms.Label();
            this.m_btnFuse = new System.Windows.Forms.Button();
            this.m_btnReadDesign = new System.Windows.Forms.Button();
            this.m_netlistContainerSelector = new GoAhead.GUI.Macros.NetlistContainerManager.NetlistContainerSelectorCtrl();
            this.m_btnMacroStatistics = new System.Windows.Forms.Button();
            this.m_btnCutOff = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.m_grpBoxNetlistContainerCtrl.SuspendLayout();
            this.m_grpBoxNetlistContainerCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_txtMacroName
            // 
            this.m_txtMacroName.Location = new System.Drawing.Point(9, 32);
            this.m_txtMacroName.Name = "m_txtMacroName";
            this.m_txtMacroName.Size = new System.Drawing.Size(285, 20);
            this.m_txtMacroName.TabIndex = 1;
            // 
            // m_btnAdd
            // 
            this.m_btnAdd.Location = new System.Drawing.Point(312, 32);
            this.m_btnAdd.Name = "m_btnAdd";
            this.m_btnAdd.Size = new System.Drawing.Size(91, 22);
            this.m_btnAdd.TabIndex = 2;
            this.m_btnAdd.Text = "Add";
            this.m_btnAdd.UseVisualStyleBackColor = true;
            this.m_btnAdd.Click += new System.EventHandler(this.m_btnAdd_Click);
            // 
            // m_lblAddMacro
            // 
            this.m_lblAddMacro.AutoSize = true;
            this.m_lblAddMacro.Location = new System.Drawing.Point(6, 17);
            this.m_lblAddMacro.Name = "m_lblAddMacro";
            this.m_lblAddMacro.Size = new System.Drawing.Size(106, 13);
            this.m_lblAddMacro.TabIndex = 8;
            this.m_lblAddMacro.Text = "Add Netlist Container";
            // 
            // m_grpBoxNetlistContainerCtrl
            // 
            this.m_grpBoxNetlistContainerCtrl.Controls.Add(this.m_grpBoxNetlistContainerCommands);
            this.m_grpBoxNetlistContainerCtrl.Controls.Add(this.m_txtMacroName);
            this.m_grpBoxNetlistContainerCtrl.Controls.Add(this.m_btnAdd);
            this.m_grpBoxNetlistContainerCtrl.Controls.Add(this.m_lblAddMacro);
            this.m_grpBoxNetlistContainerCtrl.Location = new System.Drawing.Point(3, 3);
            this.m_grpBoxNetlistContainerCtrl.Name = "m_grpBoxNetlistContainerCtrl";
            this.m_grpBoxNetlistContainerCtrl.Size = new System.Drawing.Size(411, 362);
            this.m_grpBoxNetlistContainerCtrl.TabIndex = 12;
            this.m_grpBoxNetlistContainerCtrl.TabStop = false;
            this.m_grpBoxNetlistContainerCtrl.Text = "Netlist Container Control";
            // 
            // m_grpBoxNetlistContainerCommands
            // 
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_btnDesignBrowser);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_lblFuse);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_lblCutOff);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_lblStatistics);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_lblRead);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_btnFuse);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.button3);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_btnReadDesign);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.button2);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_netlistContainerSelector);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.button1);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_btnMacroStatistics);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_btnCutOff);
            this.m_grpBoxNetlistContainerCommands.Controls.Add(this.m_lblDesignBrowser);
            this.m_grpBoxNetlistContainerCommands.Location = new System.Drawing.Point(9, 60);
            this.m_grpBoxNetlistContainerCommands.Name = "m_grpBoxNetlistContainerCommands";
            this.m_grpBoxNetlistContainerCommands.Size = new System.Drawing.Size(394, 296);
            this.m_grpBoxNetlistContainerCommands.TabIndex = 14;
            this.m_grpBoxNetlistContainerCommands.TabStop = false;
            this.m_grpBoxNetlistContainerCommands.Text = "Netlist Container Commands";
            // 
            // m_lblDesignBrowser
            // 
            this.m_lblDesignBrowser.AutoSize = true;
            this.m_lblDesignBrowser.Location = new System.Drawing.Point(11, 241);
            this.m_lblDesignBrowser.Name = "m_lblDesignBrowser";
            this.m_lblDesignBrowser.Size = new System.Drawing.Size(110, 13);
            this.m_lblDesignBrowser.TabIndex = 21;
            this.m_lblDesignBrowser.Text = "Open Design Browser";
            // 
            // m_btnDesignBrowser
            // 
            this.m_btnDesignBrowser.Location = new System.Drawing.Point(12, 258);
            this.m_btnDesignBrowser.Name = "m_btnDesignBrowser";
            this.m_btnDesignBrowser.Size = new System.Drawing.Size(100, 23);
            this.m_btnDesignBrowser.TabIndex = 20;
            this.m_btnDesignBrowser.Text = "Design Browser";
            this.m_btnDesignBrowser.UseVisualStyleBackColor = true;
            this.m_btnDesignBrowser.Click += new System.EventHandler(this.m_btnDesignBrowser_Click);
            // 
            // m_lblFuse
            // 
            this.m_lblFuse.AutoSize = true;
            this.m_lblFuse.Location = new System.Drawing.Point(12, 197);
            this.m_lblFuse.Name = "m_lblFuse";
            this.m_lblFuse.Size = new System.Drawing.Size(35, 13);
            this.m_lblFuse.TabIndex = 19;
            this.m_lblFuse.Text = "label4";
            // 
            // m_lblCutOff
            // 
            this.m_lblCutOff.AutoSize = true;
            this.m_lblCutOff.Location = new System.Drawing.Point(12, 154);
            this.m_lblCutOff.Name = "m_lblCutOff";
            this.m_lblCutOff.Size = new System.Drawing.Size(35, 13);
            this.m_lblCutOff.TabIndex = 18;
            this.m_lblCutOff.Text = "label3";
            // 
            // m_lblStatistics
            // 
            this.m_lblStatistics.AutoSize = true;
            this.m_lblStatistics.Location = new System.Drawing.Point(12, 111);
            this.m_lblStatistics.Name = "m_lblStatistics";
            this.m_lblStatistics.Size = new System.Drawing.Size(35, 13);
            this.m_lblStatistics.TabIndex = 17;
            this.m_lblStatistics.Text = "label2";
            // 
            // m_lblRead
            // 
            this.m_lblRead.AutoSize = true;
            this.m_lblRead.Location = new System.Drawing.Point(12, 67);
            this.m_lblRead.Name = "m_lblRead";
            this.m_lblRead.Size = new System.Drawing.Size(35, 13);
            this.m_lblRead.TabIndex = 16;
            this.m_lblRead.Text = "label1";
            // 
            // m_btnFuse
            // 
            this.m_btnFuse.Location = new System.Drawing.Point(12, 214);
            this.m_btnFuse.Name = "m_btnFuse";
            this.m_btnFuse.Size = new System.Drawing.Size(100, 23);
            this.m_btnFuse.TabIndex = 15;
            this.m_btnFuse.Text = "Fuse";
            this.m_btnFuse.UseVisualStyleBackColor = true;
            this.m_btnFuse.Click += new System.EventHandler(this.m_btnFuse_Click);
            // 
            // m_btnReadDesign
            // 
            this.m_btnReadDesign.Location = new System.Drawing.Point(12, 84);
            this.m_btnReadDesign.Name = "m_btnReadDesign";
            this.m_btnReadDesign.Size = new System.Drawing.Size(91, 23);
            this.m_btnReadDesign.TabIndex = 14;
            this.m_btnReadDesign.Text = "Open Design";
            this.m_btnReadDesign.UseVisualStyleBackColor = true;
            this.m_btnReadDesign.Click += new System.EventHandler(this.m_btnReadDesign_Click);
            // 
            // m_netlistContainerSelector
            // 
            this.m_netlistContainerSelector.Label = "Netlist Container";
            this.m_netlistContainerSelector.Location = new System.Drawing.Point(6, 19);
            this.m_netlistContainerSelector.Name = "m_netlistContainerSelector";
            this.m_netlistContainerSelector.Size = new System.Drawing.Size(165, 45);
            this.m_netlistContainerSelector.TabIndex = 13;
            // 
            // m_btnMacroStatistics
            // 
            this.m_btnMacroStatistics.Location = new System.Drawing.Point(12, 128);
            this.m_btnMacroStatistics.Name = "m_btnMacroStatistics";
            this.m_btnMacroStatistics.Size = new System.Drawing.Size(91, 22);
            this.m_btnMacroStatistics.TabIndex = 11;
            this.m_btnMacroStatistics.Text = "Statistics";
            this.m_btnMacroStatistics.UseVisualStyleBackColor = true;
            this.m_btnMacroStatistics.Click += new System.EventHandler(this.m_btnMacroStatistics_Click);
            // 
            // m_btnCutOff
            // 
            this.m_btnCutOff.Location = new System.Drawing.Point(12, 171);
            this.m_btnCutOff.Name = "m_btnCutOff";
            this.m_btnCutOff.Size = new System.Drawing.Size(91, 22);
            this.m_btnCutOff.TabIndex = 12;
            this.m_btnCutOff.Text = "Cut Off";
            this.m_btnCutOff.UseVisualStyleBackColor = true;
            this.m_btnCutOff.Click += new System.EventHandler(this.m_btnCutOff_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 22);
            this.button1.TabIndex = 12;
            this.button1.Text = "Cut Off";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.m_btnCutOff_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 128);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 22);
            this.button2.TabIndex = 11;
            this.button2.Text = "Statistics";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.m_btnMacroStatistics_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 84);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 23);
            this.button3.TabIndex = 14;
            this.button3.Text = "Open Design";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.m_btnReadDesign_Click);
            // 
            // NetlistContainerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_grpBoxNetlistContainerCtrl);
            this.Name = "NetlistContainerCtrl";
            this.Size = new System.Drawing.Size(420, 368);
            this.m_grpBoxNetlistContainerCtrl.ResumeLayout(false);
            this.m_grpBoxNetlistContainerCtrl.PerformLayout();
            this.m_grpBoxNetlistContainerCommands.ResumeLayout(false);
            this.m_grpBoxNetlistContainerCommands.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox m_txtMacroName;
        private System.Windows.Forms.Button m_btnAdd;
        private System.Windows.Forms.Label m_lblAddMacro;
        private System.Windows.Forms.GroupBox m_grpBoxNetlistContainerCtrl;
        private System.Windows.Forms.Button m_btnMacroStatistics;
        private System.Windows.Forms.Button m_btnCutOff;
        private System.Windows.Forms.GroupBox m_grpBoxNetlistContainerCommands;
        private Macros.NetlistContainerManager.NetlistContainerSelectorCtrl m_netlistContainerSelector;
        private System.Windows.Forms.Button m_btnReadDesign;
        private System.Windows.Forms.Button m_btnFuse;
        private System.Windows.Forms.Label m_lblFuse;
        private System.Windows.Forms.Label m_lblCutOff;
        private System.Windows.Forms.Label m_lblStatistics;
        private System.Windows.Forms.Label m_lblRead;
        private System.Windows.Forms.Label m_lblDesignBrowser;
        private System.Windows.Forms.Button m_btnDesignBrowser;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}
