namespace GoAhead.GUI.CommandForms.ParseTimingData
{
    partial class ParseTimingDataForm
    {
        public string FileName = "";
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.num_att3 = new System.Windows.Forms.NumericUpDown();
            this.num_att4 = new System.Windows.Forms.NumericUpDown();
            this.num_att2 = new System.Windows.Forms.NumericUpDown();
            this.num_att1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.num_tile = new System.Windows.Forms.NumericUpDown();
            this.num_spip = new System.Windows.Forms.NumericUpDown();
            this.num_epip = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.commentSymbol = new System.Windows.Forms.TextBox();
            this.textBox_att1 = new System.Windows.Forms.TextBox();
            this.checkBox_att1 = new System.Windows.Forms.CheckBox();
            this.textBox_att2 = new System.Windows.Forms.TextBox();
            this.textBox_att3 = new System.Windows.Forms.TextBox();
            this.textBox_att4 = new System.Windows.Forms.TextBox();
            this.checkBox_att2 = new System.Windows.Forms.CheckBox();
            this.checkBox_att3 = new System.Windows.Forms.CheckBox();
            this.checkBox_att4 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.num_att3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_att4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_att2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_att1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_tile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_spip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_epip)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(537, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Configure GoAhead to recognize which field is expected at which index in the csv " +
    "file";
            // 
            // num_att3
            // 
            this.num_att3.Location = new System.Drawing.Point(527, 178);
            this.num_att3.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.num_att3.Name = "num_att3";
            this.num_att3.Size = new System.Drawing.Size(50, 22);
            this.num_att3.TabIndex = 5;
            this.num_att3.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // num_att4
            // 
            this.num_att4.Location = new System.Drawing.Point(527, 218);
            this.num_att4.Name = "num_att4";
            this.num_att4.Size = new System.Drawing.Size(50, 22);
            this.num_att4.TabIndex = 6;
            this.num_att4.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // num_att2
            // 
            this.num_att2.Location = new System.Drawing.Point(527, 138);
            this.num_att2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.num_att2.Name = "num_att2";
            this.num_att2.Size = new System.Drawing.Size(50, 22);
            this.num_att2.TabIndex = 7;
            this.num_att2.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // num_att1
            // 
            this.num_att1.Location = new System.Drawing.Point(527, 98);
            this.num_att1.Name = "num_att1";
            this.num_att1.Size = new System.Drawing.Size(50, 22);
            this.num_att1.TabIndex = 8;
            this.num_att1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(298, 284);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Parse File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(100, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Tile";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(100, 180);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Start PIP";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(100, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "End PIP";
            // 
            // num_tile
            // 
            this.num_tile.Location = new System.Drawing.Point(253, 138);
            this.num_tile.Name = "num_tile";
            this.num_tile.Size = new System.Drawing.Size(50, 22);
            this.num_tile.TabIndex = 13;
            // 
            // num_spip
            // 
            this.num_spip.Location = new System.Drawing.Point(253, 178);
            this.num_spip.Name = "num_spip";
            this.num_spip.Size = new System.Drawing.Size(50, 22);
            this.num_spip.TabIndex = 14;
            this.num_spip.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // num_epip
            // 
            this.num_epip.Location = new System.Drawing.Point(253, 218);
            this.num_epip.Name = "num_epip";
            this.num_epip.Size = new System.Drawing.Size(50, 22);
            this.num_epip.TabIndex = 15;
            this.num_epip.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(100, 100);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(117, 17);
            this.label9.TabIndex = 16;
            this.label9.Text = "Comment Symbol";
            // 
            // commentSymbol
            // 
            this.commentSymbol.Location = new System.Drawing.Point(253, 97);
            this.commentSymbol.Name = "commentSymbol";
            this.commentSymbol.Size = new System.Drawing.Size(50, 22);
            this.commentSymbol.TabIndex = 17;
            this.commentSymbol.Text = "#";
            // 
            // textBox_att1
            // 
            this.textBox_att1.Location = new System.Drawing.Point(421, 98);
            this.textBox_att1.Name = "textBox_att1";
            this.textBox_att1.Size = new System.Drawing.Size(100, 22);
            this.textBox_att1.TabIndex = 18;
            this.textBox_att1.Text = "Attribute 1";
            // 
            // checkBox_att1
            // 
            this.checkBox_att1.AutoSize = true;
            this.checkBox_att1.Checked = true;
            this.checkBox_att1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_att1.Location = new System.Drawing.Point(397, 100);
            this.checkBox_att1.Name = "checkBox_att1";
            this.checkBox_att1.Size = new System.Drawing.Size(18, 17);
            this.checkBox_att1.TabIndex = 19;
            this.checkBox_att1.UseVisualStyleBackColor = true;
            this.checkBox_att1.CheckedChanged += new System.EventHandler(this.checkBox_att_CheckedChanged);
            // 
            // textBox_att2
            // 
            this.textBox_att2.Location = new System.Drawing.Point(421, 138);
            this.textBox_att2.Name = "textBox_att2";
            this.textBox_att2.Size = new System.Drawing.Size(100, 22);
            this.textBox_att2.TabIndex = 20;
            this.textBox_att2.Text = "Attribute 2";
            // 
            // textBox_att3
            // 
            this.textBox_att3.Location = new System.Drawing.Point(421, 178);
            this.textBox_att3.Name = "textBox_att3";
            this.textBox_att3.Size = new System.Drawing.Size(100, 22);
            this.textBox_att3.TabIndex = 21;
            this.textBox_att3.Text = "Attribute 3";
            // 
            // textBox_att4
            // 
            this.textBox_att4.Location = new System.Drawing.Point(421, 218);
            this.textBox_att4.Name = "textBox_att4";
            this.textBox_att4.Size = new System.Drawing.Size(100, 22);
            this.textBox_att4.TabIndex = 22;
            this.textBox_att4.Text = "Attribute 4";
            // 
            // checkBox_att2
            // 
            this.checkBox_att2.AutoSize = true;
            this.checkBox_att2.Checked = true;
            this.checkBox_att2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_att2.Location = new System.Drawing.Point(397, 141);
            this.checkBox_att2.Name = "checkBox_att2";
            this.checkBox_att2.Size = new System.Drawing.Size(18, 17);
            this.checkBox_att2.TabIndex = 23;
            this.checkBox_att2.UseVisualStyleBackColor = true;
            this.checkBox_att2.CheckedChanged += new System.EventHandler(this.checkBox_att_CheckedChanged);
            // 
            // checkBox_att3
            // 
            this.checkBox_att3.AutoSize = true;
            this.checkBox_att3.Checked = true;
            this.checkBox_att3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_att3.Location = new System.Drawing.Point(397, 181);
            this.checkBox_att3.Name = "checkBox_att3";
            this.checkBox_att3.Size = new System.Drawing.Size(18, 17);
            this.checkBox_att3.TabIndex = 24;
            this.checkBox_att3.UseVisualStyleBackColor = true;
            this.checkBox_att3.CheckedChanged += new System.EventHandler(this.checkBox_att_CheckedChanged);
            // 
            // checkBox_att4
            // 
            this.checkBox_att4.AutoSize = true;
            this.checkBox_att4.Checked = true;
            this.checkBox_att4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_att4.Location = new System.Drawing.Point(397, 222);
            this.checkBox_att4.Name = "checkBox_att4";
            this.checkBox_att4.Size = new System.Drawing.Size(18, 17);
            this.checkBox_att4.TabIndex = 25;
            this.checkBox_att4.UseVisualStyleBackColor = true;
            this.checkBox_att4.CheckedChanged += new System.EventHandler(this.checkBox_att_CheckedChanged);
            // 
            // ParseTimingDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 351);
            this.Controls.Add(this.checkBox_att4);
            this.Controls.Add(this.checkBox_att3);
            this.Controls.Add(this.checkBox_att2);
            this.Controls.Add(this.textBox_att4);
            this.Controls.Add(this.textBox_att3);
            this.Controls.Add(this.textBox_att2);
            this.Controls.Add(this.checkBox_att1);
            this.Controls.Add(this.textBox_att1);
            this.Controls.Add(this.commentSymbol);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.num_epip);
            this.Controls.Add(this.num_spip);
            this.Controls.Add(this.num_tile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.num_att1);
            this.Controls.Add(this.num_att2);
            this.Controls.Add(this.num_att4);
            this.Controls.Add(this.num_att3);
            this.Controls.Add(this.label1);
            this.Name = "ParseTimingDataForm";
            this.Text = "Add Timing Data";
            ((System.ComponentModel.ISupportInitialize)(this.num_att3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_att4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_att2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_att1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_tile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_spip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_epip)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown num_att1;
        private System.Windows.Forms.NumericUpDown num_att2;
        private System.Windows.Forms.NumericUpDown num_att3;
        private System.Windows.Forms.NumericUpDown num_att4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown num_tile;
        private System.Windows.Forms.NumericUpDown num_spip;
        private System.Windows.Forms.NumericUpDown num_epip;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox commentSymbol;
        private System.Windows.Forms.TextBox textBox_att1;
        private System.Windows.Forms.CheckBox checkBox_att1;
        private System.Windows.Forms.TextBox textBox_att2;
        private System.Windows.Forms.TextBox textBox_att3;
        private System.Windows.Forms.TextBox textBox_att4;
        private System.Windows.Forms.CheckBox checkBox_att2;
        private System.Windows.Forms.CheckBox checkBox_att3;
        private System.Windows.Forms.CheckBox checkBox_att4;
    }
}