namespace GoAhead.GUI.MacroLibrary
{
    partial class LilbraryManagerCtrl
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
            this.m_listBoxLibraryElementNames = new System.Windows.Forms.ListBox();
            this.m_lblElements = new System.Windows.Forms.Label();
            this.m_btnAddXDLElement = new System.Windows.Forms.Button();
            this.m_btnSaveLib = new System.Windows.Forms.Button();
            this.m_btnOpenLib = new System.Windows.Forms.Button();
            this.m_btnClear = new System.Windows.Forms.Button();
            this.m_btnRemove = new System.Windows.Forms.Button();
            this.m_btnAddBinaryElement = new System.Windows.Forms.Button();
            this.m_btnSave = new System.Windows.Forms.Button();
            this.m_btnPrintWrapper = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_listBoxLibraryElementNames
            // 
            this.m_listBoxLibraryElementNames.FormattingEnabled = true;
            this.m_listBoxLibraryElementNames.Location = new System.Drawing.Point(5, 20);
            this.m_listBoxLibraryElementNames.Name = "m_listBoxLibraryElementNames";
            this.m_listBoxLibraryElementNames.Size = new System.Drawing.Size(291, 199);
            this.m_listBoxLibraryElementNames.TabIndex = 0;
            this.m_listBoxLibraryElementNames.DoubleClick += new System.EventHandler(this.m_listBoxMacroNames_DoubleClick);
            this.m_listBoxLibraryElementNames.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_listBoxMacroNames_MouseMove);
            // 
            // m_lblElements
            // 
            this.m_lblElements.AutoSize = true;
            this.m_lblElements.Location = new System.Drawing.Point(2, 4);
            this.m_lblElements.Name = "m_lblElements";
            this.m_lblElements.Size = new System.Drawing.Size(84, 13);
            this.m_lblElements.TabIndex = 1;
            this.m_lblElements.Text = "Library Elements";
            // 
            // m_btnAddXDLElement
            // 
            this.m_btnAddXDLElement.Location = new System.Drawing.Point(305, 20);
            this.m_btnAddXDLElement.Name = "m_btnAddXDLElement";
            this.m_btnAddXDLElement.Size = new System.Drawing.Size(119, 21);
            this.m_btnAddXDLElement.TabIndex = 2;
            this.m_btnAddXDLElement.Text = "Add XDL Element";
            this.m_btnAddXDLElement.UseVisualStyleBackColor = true;
            this.m_btnAddXDLElement.Click += new System.EventHandler(this.m_btnAddXDL_Click);
            // 
            // m_btnSaveLib
            // 
            this.m_btnSaveLib.Location = new System.Drawing.Point(203, 225);
            this.m_btnSaveLib.Name = "m_btnSaveLib";
            this.m_btnSaveLib.Size = new System.Drawing.Size(93, 25);
            this.m_btnSaveLib.TabIndex = 3;
            this.m_btnSaveLib.Text = "Save Library";
            this.m_btnSaveLib.UseVisualStyleBackColor = true;
            this.m_btnSaveLib.Click += new System.EventHandler(this.m_btnSaveLib_Click);
            // 
            // m_btnOpenLib
            // 
            this.m_btnOpenLib.Location = new System.Drawing.Point(5, 225);
            this.m_btnOpenLib.Name = "m_btnOpenLib";
            this.m_btnOpenLib.Size = new System.Drawing.Size(93, 25);
            this.m_btnOpenLib.TabIndex = 4;
            this.m_btnOpenLib.Text = "Open Library";
            this.m_btnOpenLib.UseVisualStyleBackColor = true;
            this.m_btnOpenLib.Click += new System.EventHandler(this.m_btnOpenLib_Click);
            // 
            // m_btnClear
            // 
            this.m_btnClear.Location = new System.Drawing.Point(104, 225);
            this.m_btnClear.Name = "m_btnClear";
            this.m_btnClear.Size = new System.Drawing.Size(93, 25);
            this.m_btnClear.TabIndex = 5;
            this.m_btnClear.Text = "Clear Library";
            this.m_btnClear.UseVisualStyleBackColor = true;
            this.m_btnClear.Click += new System.EventHandler(this.m_btnClear_Click);
            // 
            // m_btnRemove
            // 
            this.m_btnRemove.Location = new System.Drawing.Point(305, 106);
            this.m_btnRemove.Name = "m_btnRemove";
            this.m_btnRemove.Size = new System.Drawing.Size(119, 23);
            this.m_btnRemove.TabIndex = 6;
            this.m_btnRemove.Text = "Remove Element";
            this.m_btnRemove.UseVisualStyleBackColor = true;
            this.m_btnRemove.Click += new System.EventHandler(this.m_btnRemove_Click);
            // 
            // m_btnAddBinaryElement
            // 
            this.m_btnAddBinaryElement.Location = new System.Drawing.Point(305, 47);
            this.m_btnAddBinaryElement.Name = "m_btnAddBinaryElement";
            this.m_btnAddBinaryElement.Size = new System.Drawing.Size(119, 23);
            this.m_btnAddBinaryElement.TabIndex = 7;
            this.m_btnAddBinaryElement.Text = "Add binary Element";
            this.m_btnAddBinaryElement.UseVisualStyleBackColor = true;
            this.m_btnAddBinaryElement.Click += new System.EventHandler(this.m_btnAddBinaryMacro_Click);
            // 
            // m_btnSave
            // 
            this.m_btnSave.Location = new System.Drawing.Point(305, 77);
            this.m_btnSave.Name = "m_btnSave";
            this.m_btnSave.Size = new System.Drawing.Size(119, 23);
            this.m_btnSave.TabIndex = 8;
            this.m_btnSave.Text = "Save Element";
            this.m_btnSave.UseVisualStyleBackColor = true;
            this.m_btnSave.Click += new System.EventHandler(this.m_btnSaveLibraryElement_Click);
            // 
            // m_btnPrintWrapper
            // 
            this.m_btnPrintWrapper.Location = new System.Drawing.Point(303, 183);
            this.m_btnPrintWrapper.Name = "m_btnPrintWrapper";
            this.m_btnPrintWrapper.Size = new System.Drawing.Size(121, 36);
            this.m_btnPrintWrapper.TabIndex = 9;
            this.m_btnPrintWrapper.Text = "Print Component Declaration";
            this.m_btnPrintWrapper.UseVisualStyleBackColor = true;
            this.m_btnPrintWrapper.Click += new System.EventHandler(this.m_btnPrintWrapper_Click);
            // 
            // LilbraryManagerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnPrintWrapper);
            this.Controls.Add(this.m_btnSave);
            this.Controls.Add(this.m_btnAddBinaryElement);
            this.Controls.Add(this.m_btnRemove);
            this.Controls.Add(this.m_btnClear);
            this.Controls.Add(this.m_btnOpenLib);
            this.Controls.Add(this.m_btnSaveLib);
            this.Controls.Add(this.m_btnAddXDLElement);
            this.Controls.Add(this.m_listBoxLibraryElementNames);
            this.Controls.Add(this.m_lblElements);
            this.Name = "LilbraryManagerCtrl";
            this.Size = new System.Drawing.Size(431, 254);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox m_listBoxLibraryElementNames;
        private System.Windows.Forms.Label m_lblElements;
        private System.Windows.Forms.Button m_btnAddXDLElement;
        private System.Windows.Forms.Button m_btnSaveLib;
        private System.Windows.Forms.Button m_btnOpenLib;
        private System.Windows.Forms.Button m_btnClear;
        private System.Windows.Forms.Button m_btnRemove;
        private System.Windows.Forms.Button m_btnAddBinaryElement;
        private System.Windows.Forms.Button m_btnSave;
        private System.Windows.Forms.Button m_btnPrintWrapper;
    }
}
