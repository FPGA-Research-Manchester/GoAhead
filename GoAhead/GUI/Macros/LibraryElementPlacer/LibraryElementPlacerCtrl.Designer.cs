namespace GoAhead.GUI.MacroForm
{
    partial class LibraryElementPlacerCtrl
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
            this.m_chkBoxAutoclearMulti = new System.Windows.Forms.CheckBox();
            this.m_drpDownVerticallOrder = new System.Windows.Forms.ComboBox();
            this.m_lblVertical = new System.Windows.Forms.Label();
            this.m_drpDownHorizontalOrder = new System.Windows.Forms.ComboBox();
            this.m_drpDownMode = new System.Windows.Forms.ComboBox();
            this.m_btnPlaceInSelection = new System.Windows.Forms.Button();
            this.m_btnCheckPlacementForSelection = new System.Windows.Forms.Button();
            this.m_txtMultiInstanceName = new System.Windows.Forms.RichTextBox();
            this.m_lblMode = new System.Windows.Forms.Label();
            this.m_lblHorizontalOrder = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_chkBoxAutoclearSingle = new System.Windows.Forms.CheckBox();
            this.m_btnPlaceBySlice = new System.Windows.Forms.Button();
            this.m_txtAnchorSlice = new System.Windows.Forms.RichTextBox();
            this.m_txtAnchorLocation = new System.Windows.Forms.RichTextBox();
            this.m_btnCheck = new System.Windows.Forms.Button();
            this.m_txtSingleInstanceName = new System.Windows.Forms.RichTextBox();
            this.m_btnPlaceMacro = new System.Windows.Forms.Button();
            this.m_lblInstanceName = new System.Windows.Forms.Label();
            this.m_lblAnchor = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_tabSingleMultiPlacement = new System.Windows.Forms.TabControl();
            this.m_tabPageSinglePlacement = new System.Windows.Forms.TabPage();
            this.m_tabPageMultiPlacement = new System.Windows.Forms.TabPage();
            this.m_btnPrintPossiblePlacements = new System.Windows.Forms.Button();
            this.m_libElementSelector = new GoAhead.GUI.Macros.LibraryManager.LibraryElementSelectorCtrl();
            this.m_netlistContainerSelector = new GoAhead.GUI.Macros.NetlistContainerManager.NetlistContainerSelectorCtrl();
            this.m_tabSingleMultiPlacement.SuspendLayout();
            this.m_tabPageSinglePlacement.SuspendLayout();
            this.m_tabPageMultiPlacement.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_chkBoxAutoclearMulti
            // 
            this.m_chkBoxAutoclearMulti.AutoSize = true;
            this.m_chkBoxAutoclearMulti.Location = new System.Drawing.Point(269, 69);
            this.m_chkBoxAutoclearMulti.Name = "m_chkBoxAutoclearMulti";
            this.m_chkBoxAutoclearMulti.Size = new System.Drawing.Size(134, 17);
            this.m_chkBoxAutoclearMulti.TabIndex = 13;
            this.m_chkBoxAutoclearMulti.Text = "Auto Clear Module Slot";
            this.m_chkBoxAutoclearMulti.UseVisualStyleBackColor = true;
            // 
            // m_drpDownVerticallOrder
            // 
            this.m_drpDownVerticallOrder.FormattingEnabled = true;
            this.m_drpDownVerticallOrder.Items.AddRange(new object[] {
            "bottom-up",
            "top-down"});
            this.m_drpDownVerticallOrder.Location = new System.Drawing.Point(171, 68);
            this.m_drpDownVerticallOrder.Name = "m_drpDownVerticallOrder";
            this.m_drpDownVerticallOrder.Size = new System.Drawing.Size(77, 21);
            this.m_drpDownVerticallOrder.TabIndex = 18;
            this.m_drpDownVerticallOrder.Text = "bottom-up";
            // 
            // m_lblVertical
            // 
            this.m_lblVertical.AutoSize = true;
            this.m_lblVertical.Location = new System.Drawing.Point(168, 52);
            this.m_lblVertical.Name = "m_lblVertical";
            this.m_lblVertical.Size = new System.Drawing.Size(42, 13);
            this.m_lblVertical.TabIndex = 17;
            this.m_lblVertical.Text = "Vertical";
            // 
            // m_drpDownHorizontalOrder
            // 
            this.m_drpDownHorizontalOrder.FormattingEnabled = true;
            this.m_drpDownHorizontalOrder.Items.AddRange(new object[] {
            "left-to-right",
            "right-to-left"});
            this.m_drpDownHorizontalOrder.Location = new System.Drawing.Point(87, 68);
            this.m_drpDownHorizontalOrder.Name = "m_drpDownHorizontalOrder";
            this.m_drpDownHorizontalOrder.Size = new System.Drawing.Size(77, 21);
            this.m_drpDownHorizontalOrder.TabIndex = 16;
            this.m_drpDownHorizontalOrder.Text = "left-to-right";
            // 
            // m_drpDownMode
            // 
            this.m_drpDownMode.FormattingEnabled = true;
            this.m_drpDownMode.Items.AddRange(new object[] {
            "row-wise",
            "column-wise"});
            this.m_drpDownMode.Location = new System.Drawing.Point(12, 67);
            this.m_drpDownMode.Name = "m_drpDownMode";
            this.m_drpDownMode.Size = new System.Drawing.Size(70, 21);
            this.m_drpDownMode.TabIndex = 15;
            this.m_drpDownMode.Text = "row-wise";
            // 
            // m_btnPlaceInSelection
            // 
            this.m_btnPlaceInSelection.Location = new System.Drawing.Point(276, 92);
            this.m_btnPlaceInSelection.Name = "m_btnPlaceInSelection";
            this.m_btnPlaceInSelection.Size = new System.Drawing.Size(118, 23);
            this.m_btnPlaceInSelection.TabIndex = 11;
            this.m_btnPlaceInSelection.Text = "Place by Selection";
            this.m_btnPlaceInSelection.UseVisualStyleBackColor = true;
            this.m_btnPlaceInSelection.Click += new System.EventHandler(this.m_btnPlaceInSelection_Click);
            // 
            // m_btnCheckPlacementForSelection
            // 
            this.m_btnCheckPlacementForSelection.Location = new System.Drawing.Point(270, 21);
            this.m_btnCheckPlacementForSelection.Name = "m_btnCheckPlacementForSelection";
            this.m_btnCheckPlacementForSelection.Size = new System.Drawing.Size(118, 23);
            this.m_btnCheckPlacementForSelection.TabIndex = 1;
            this.m_btnCheckPlacementForSelection.Text = "Check Placement";
            this.m_btnCheckPlacementForSelection.UseVisualStyleBackColor = true;
            this.m_btnCheckPlacementForSelection.Click += new System.EventHandler(this.m_btnCheckPlacementForSelection_Click);
            // 
            // m_txtMultiInstanceName
            // 
            this.m_txtMultiInstanceName.Location = new System.Drawing.Point(7, 21);
            this.m_txtMultiInstanceName.Multiline = false;
            this.m_txtMultiInstanceName.Name = "m_txtMultiInstanceName";
            this.m_txtMultiInstanceName.Size = new System.Drawing.Size(250, 23);
            this.m_txtMultiInstanceName.TabIndex = 5;
            this.m_txtMultiInstanceName.Text = "";
            // 
            // m_lblMode
            // 
            this.m_lblMode.AutoSize = true;
            this.m_lblMode.Location = new System.Drawing.Point(9, 51);
            this.m_lblMode.Name = "m_lblMode";
            this.m_lblMode.Size = new System.Drawing.Size(34, 13);
            this.m_lblMode.TabIndex = 14;
            this.m_lblMode.Text = "Mode";
            // 
            // m_lblHorizontalOrder
            // 
            this.m_lblHorizontalOrder.AutoSize = true;
            this.m_lblHorizontalOrder.Location = new System.Drawing.Point(84, 52);
            this.m_lblHorizontalOrder.Name = "m_lblHorizontalOrder";
            this.m_lblHorizontalOrder.Size = new System.Drawing.Size(54, 13);
            this.m_lblHorizontalOrder.TabIndex = 13;
            this.m_lblHorizontalOrder.Text = "Horizontal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "InstanceName";
            // 
            // m_chkBoxAutoclearSingle
            // 
            this.m_chkBoxAutoclearSingle.AutoSize = true;
            this.m_chkBoxAutoclearSingle.Location = new System.Drawing.Point(269, 69);
            this.m_chkBoxAutoclearSingle.Name = "m_chkBoxAutoclearSingle";
            this.m_chkBoxAutoclearSingle.Size = new System.Drawing.Size(134, 17);
            this.m_chkBoxAutoclearSingle.TabIndex = 12;
            this.m_chkBoxAutoclearSingle.Text = "Auto Clear Module Slot";
            this.m_chkBoxAutoclearSingle.UseVisualStyleBackColor = true;
            // 
            // m_btnPlaceBySlice
            // 
            this.m_btnPlaceBySlice.Location = new System.Drawing.Point(177, 95);
            this.m_btnPlaceBySlice.Name = "m_btnPlaceBySlice";
            this.m_btnPlaceBySlice.Size = new System.Drawing.Size(85, 23);
            this.m_btnPlaceBySlice.TabIndex = 10;
            this.m_btnPlaceBySlice.Text = "Place by Slice";
            this.m_btnPlaceBySlice.UseVisualStyleBackColor = true;
            this.m_btnPlaceBySlice.Click += new System.EventHandler(this.m_btnPlaceBySlice_Click);
            // 
            // m_txtAnchorSlice
            // 
            this.m_txtAnchorSlice.Location = new System.Drawing.Point(141, 68);
            this.m_txtAnchorSlice.Multiline = false;
            this.m_txtAnchorSlice.Name = "m_txtAnchorSlice";
            this.m_txtAnchorSlice.Size = new System.Drawing.Size(123, 21);
            this.m_txtAnchorSlice.TabIndex = 8;
            this.m_txtAnchorSlice.Text = "";
            // 
            // m_txtAnchorLocation
            // 
            this.m_txtAnchorLocation.Location = new System.Drawing.Point(8, 68);
            this.m_txtAnchorLocation.Multiline = false;
            this.m_txtAnchorLocation.Name = "m_txtAnchorLocation";
            this.m_txtAnchorLocation.Size = new System.Drawing.Size(124, 21);
            this.m_txtAnchorLocation.TabIndex = 7;
            this.m_txtAnchorLocation.Text = "";
            // 
            // m_btnCheck
            // 
            this.m_btnCheck.Location = new System.Drawing.Point(270, 21);
            this.m_btnCheck.Name = "m_btnCheck";
            this.m_btnCheck.Size = new System.Drawing.Size(118, 23);
            this.m_btnCheck.TabIndex = 1;
            this.m_btnCheck.Text = "Check Placement";
            this.m_btnCheck.UseVisualStyleBackColor = true;
            this.m_btnCheck.Click += new System.EventHandler(this.m_btnCheck_Click);
            // 
            // m_txtSingleInstanceName
            // 
            this.m_txtSingleInstanceName.Location = new System.Drawing.Point(7, 21);
            this.m_txtSingleInstanceName.Multiline = false;
            this.m_txtSingleInstanceName.Name = "m_txtSingleInstanceName";
            this.m_txtSingleInstanceName.Size = new System.Drawing.Size(250, 23);
            this.m_txtSingleInstanceName.TabIndex = 5;
            this.m_txtSingleInstanceName.Text = "";
            // 
            // m_btnPlaceMacro
            // 
            this.m_btnPlaceMacro.Location = new System.Drawing.Point(49, 95);
            this.m_btnPlaceMacro.Name = "m_btnPlaceMacro";
            this.m_btnPlaceMacro.Size = new System.Drawing.Size(81, 23);
            this.m_btnPlaceMacro.TabIndex = 2;
            this.m_btnPlaceMacro.Text = "Place by Tile";
            this.m_btnPlaceMacro.UseVisualStyleBackColor = true;
            this.m_btnPlaceMacro.Click += new System.EventHandler(this.m_btnPlaceMacro_Click);
            // 
            // m_lblInstanceName
            // 
            this.m_lblInstanceName.AutoSize = true;
            this.m_lblInstanceName.Location = new System.Drawing.Point(5, 5);
            this.m_lblInstanceName.Name = "m_lblInstanceName";
            this.m_lblInstanceName.Size = new System.Drawing.Size(76, 13);
            this.m_lblInstanceName.TabIndex = 4;
            this.m_lblInstanceName.Text = "InstanceName";
            // 
            // m_lblAnchor
            // 
            this.m_lblAnchor.AutoSize = true;
            this.m_lblAnchor.Location = new System.Drawing.Point(9, 51);
            this.m_lblAnchor.Name = "m_lblAnchor";
            this.m_lblAnchor.Size = new System.Drawing.Size(61, 13);
            this.m_lblAnchor.TabIndex = 6;
            this.m_lblAnchor.Text = "Anchor Tile";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Anchor Slice";
            // 
            // m_tabSingleMultiPlacement
            // 
            this.m_tabSingleMultiPlacement.Controls.Add(this.m_tabPageSinglePlacement);
            this.m_tabSingleMultiPlacement.Controls.Add(this.m_tabPageMultiPlacement);
            this.m_tabSingleMultiPlacement.Location = new System.Drawing.Point(0, 64);
            this.m_tabSingleMultiPlacement.Name = "m_tabSingleMultiPlacement";
            this.m_tabSingleMultiPlacement.SelectedIndex = 0;
            this.m_tabSingleMultiPlacement.Size = new System.Drawing.Size(483, 160);
            this.m_tabSingleMultiPlacement.TabIndex = 17;
            // 
            // m_tabPageSinglePlacement
            // 
            this.m_tabPageSinglePlacement.Controls.Add(this.m_chkBoxAutoclearSingle);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_btnPlaceBySlice);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_txtSingleInstanceName);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_txtAnchorSlice);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_txtAnchorLocation);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_btnCheck);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_btnPlaceMacro);
            this.m_tabPageSinglePlacement.Controls.Add(this.label1);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_lblAnchor);
            this.m_tabPageSinglePlacement.Controls.Add(this.m_lblInstanceName);
            this.m_tabPageSinglePlacement.Location = new System.Drawing.Point(4, 22);
            this.m_tabPageSinglePlacement.Name = "m_tabPageSinglePlacement";
            this.m_tabPageSinglePlacement.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabPageSinglePlacement.Size = new System.Drawing.Size(475, 134);
            this.m_tabPageSinglePlacement.TabIndex = 0;
            this.m_tabPageSinglePlacement.Text = "Single Placement";
            this.m_tabPageSinglePlacement.UseVisualStyleBackColor = true;
            // 
            // m_tabPageMultiPlacement
            // 
            this.m_tabPageMultiPlacement.Controls.Add(this.m_chkBoxAutoclearMulti);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_drpDownVerticallOrder);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_btnCheckPlacementForSelection);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_drpDownHorizontalOrder);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_drpDownMode);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_btnPlaceInSelection);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_txtMultiInstanceName);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_lblVertical);
            this.m_tabPageMultiPlacement.Controls.Add(this.label2);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_lblHorizontalOrder);
            this.m_tabPageMultiPlacement.Controls.Add(this.m_lblMode);
            this.m_tabPageMultiPlacement.Location = new System.Drawing.Point(4, 22);
            this.m_tabPageMultiPlacement.Name = "m_tabPageMultiPlacement";
            this.m_tabPageMultiPlacement.Padding = new System.Windows.Forms.Padding(3);
            this.m_tabPageMultiPlacement.Size = new System.Drawing.Size(475, 134);
            this.m_tabPageMultiPlacement.TabIndex = 1;
            this.m_tabPageMultiPlacement.Text = "Multi Placement";
            this.m_tabPageMultiPlacement.UseVisualStyleBackColor = true;
            // 
            // m_btnPrintPossiblePlacements
            // 
            this.m_btnPrintPossiblePlacements.Location = new System.Drawing.Point(360, 32);
            this.m_btnPrintPossiblePlacements.Name = "m_btnPrintPossiblePlacements";
            this.m_btnPrintPossiblePlacements.Size = new System.Drawing.Size(97, 48);
            this.m_btnPrintPossiblePlacements.TabIndex = 18;
            this.m_btnPrintPossiblePlacements.Text = "Print Possible Placements in Selection";
            this.m_btnPrintPossiblePlacements.UseVisualStyleBackColor = true;
            this.m_btnPrintPossiblePlacements.Click += new System.EventHandler(this.m_btnPrintPossiblePlacements_Click);
            // 
            // m_libElementSelector
            // 
            this.m_libElementSelector.Label = "Library Elements";
            this.m_libElementSelector.Location = new System.Drawing.Point(180, 13);
            this.m_libElementSelector.Name = "m_libElementSelector";
            this.m_libElementSelector.Size = new System.Drawing.Size(174, 46);
            this.m_libElementSelector.TabIndex = 14;
            // 
            // m_netlistContainerSelector
            // 
            this.m_netlistContainerSelector.Label = "Netlist Container";
            this.m_netlistContainerSelector.Location = new System.Drawing.Point(9, 13);
            this.m_netlistContainerSelector.Name = "m_netlistContainerSelector";
            this.m_netlistContainerSelector.Size = new System.Drawing.Size(165, 45);
            this.m_netlistContainerSelector.TabIndex = 13;
            // 
            // LibraryElementPlacerCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnPrintPossiblePlacements);
            this.Controls.Add(this.m_tabSingleMultiPlacement);
            this.Controls.Add(this.m_libElementSelector);
            this.Controls.Add(this.m_netlistContainerSelector);
            this.Name = "LibraryElementPlacerCtrl";
            this.Size = new System.Drawing.Size(486, 225);
            this.Load += new System.EventHandler(this.MacroPlacerCtrl_Load);
            this.m_tabSingleMultiPlacement.ResumeLayout(false);
            this.m_tabPageSinglePlacement.ResumeLayout(false);
            this.m_tabPageSinglePlacement.PerformLayout();
            this.m_tabPageMultiPlacement.ResumeLayout(false);
            this.m_tabPageMultiPlacement.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox m_drpDownHorizontalOrder;
        private System.Windows.Forms.ComboBox m_drpDownMode;
        private System.Windows.Forms.Button m_btnPlaceInSelection;
        private System.Windows.Forms.Button m_btnCheckPlacementForSelection;
        private System.Windows.Forms.RichTextBox m_txtMultiInstanceName;
        private System.Windows.Forms.Label m_lblMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_btnPlaceBySlice;
        private System.Windows.Forms.RichTextBox m_txtAnchorSlice;
        private System.Windows.Forms.RichTextBox m_txtAnchorLocation;
        private System.Windows.Forms.Button m_btnCheck;
        private System.Windows.Forms.RichTextBox m_txtSingleInstanceName;
        private System.Windows.Forms.Button m_btnPlaceMacro;
        private System.Windows.Forms.Label m_lblInstanceName;
        private System.Windows.Forms.Label m_lblAnchor;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label m_lblHorizontalOrder;
        private System.Windows.Forms.ComboBox m_drpDownVerticallOrder;
        public System.Windows.Forms.Label m_lblVertical;
        private System.Windows.Forms.CheckBox m_chkBoxAutoclearSingle;
        private System.Windows.Forms.CheckBox m_chkBoxAutoclearMulti;
        private Macros.NetlistContainerManager.NetlistContainerSelectorCtrl m_netlistContainerSelector;
        private Macros.LibraryManager.LibraryElementSelectorCtrl m_libElementSelector;
        private System.Windows.Forms.TabControl m_tabSingleMultiPlacement;
        private System.Windows.Forms.TabPage m_tabPageSinglePlacement;
        private System.Windows.Forms.TabPage m_tabPageMultiPlacement;
        private System.Windows.Forms.Button m_btnPrintPossiblePlacements;
    }
}
