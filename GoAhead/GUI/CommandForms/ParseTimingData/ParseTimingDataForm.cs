using GoAhead.Commands;
using GoAhead.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.CommandForms.ParseTimingData
{
    public partial class ParseTimingDataForm : Form
    {
        private static string settingsName = "ParseTimingData_indices";

        public ParseTimingDataForm()
        {
            InitializeComponent();

            if (StoredPreferences.Instance.TextBoxSettings.HasSetting(settingsName))
            {
                string[] settings = StoredPreferences.Instance.TextBoxSettings.GetSetting(settingsName).Split(' ');
                int j=0;

                if (settings.Length == 7) commentSymbol.Text = "";
                else if (settings.Length > 7) { commentSymbol.Text = settings[0]; j++; }
                else return;
                
                bool success = true;
                decimal[] values = new decimal[7];
                for (int i = j; i < settings.Length; i++)
                    success = success && decimal.TryParse(settings[i], out values[i-j]);

                if (success)
                {
                    try
                    {
                        num_tile.Value = values[0];
                        num_spip.Value = values[1];
                        num_epip.Value = values[2];
                        num_att1.Value = values[3];
                        num_att2.Value = values[4];
                        num_att3.Value = values[5];
                        num_att4.Value = values[6];
                    }
                    catch (ArgumentOutOfRangeException) { }
                }
            }

            textBox_att1.Text = FPGA.FPGA.Instance.GetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute1);
            textBox_att2.Text = FPGA.FPGA.Instance.GetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute2);
            textBox_att3.Text = FPGA.FPGA.Instance.GetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute3);
            textBox_att4.Text = FPGA.FPGA.Instance.GetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute4);
        }

        // On Parse
        private void button1_Click(object sender, EventArgs e)
        {
            List<int> indices = new List<int>()
            {
                (int)num_tile.Value,
                (int)num_spip.Value,
                (int)num_epip.Value,
                checkBox_att1.Checked ? (int)num_att1.Value : -1,
                checkBox_att2.Checked ? (int)num_att2.Value : -1,
                checkBox_att3.Checked ? (int)num_att3.Value : -1,
                checkBox_att4.Checked ? (int)num_att4.Value : -1
            };

            Commands.Data.ParseTimingData cmd = new Commands.Data.ParseTimingData
            {
                FileName = FileName,
                CSVIndices = indices,
                IgnoreString = commentSymbol.Text
            };

            CommandExecuter.Instance.Execute(cmd);

            if (checkBox_att1.Checked) FPGA.FPGA.Instance.SetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute1, textBox_att1.Text);
            if (checkBox_att2.Checked) FPGA.FPGA.Instance.SetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute2, textBox_att2.Text);
            if (checkBox_att3.Checked) FPGA.FPGA.Instance.SetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute3, textBox_att3.Text);
            if (checkBox_att4.Checked) FPGA.FPGA.Instance.SetTimeModelAttributeName(FPGA.Tile.TimeAttributes.Attribute4, textBox_att4.Text);

            string newSetting = commentSymbol.Text;
            for (int i = 0; i < indices.Count; i++) newSetting += " " + indices[i];

            StoredPreferences.Instance.TextBoxSettings.AddOrUpdateSetting(settingsName, newSetting);
            Close();
        }

        private void checkBox_att_CheckedChanged(object sender, EventArgs e)
        {
            if (sender.Equals(checkBox_att1))
            {
                textBox_att1.Enabled = checkBox_att1.Checked;
                num_att1.Enabled = checkBox_att1.Checked;
            }
            else if (sender.Equals(checkBox_att2))
            {
                textBox_att2.Enabled = checkBox_att2.Checked;
                num_att2.Enabled = checkBox_att2.Checked;
            }
            else if (sender.Equals(checkBox_att3))
            {
                textBox_att3.Enabled = checkBox_att3.Checked;
                num_att3.Enabled = checkBox_att3.Checked;
            }
            else if (sender.Equals(checkBox_att4))
            {
                textBox_att4.Enabled = checkBox_att4.Checked;
                num_att4.Enabled = checkBox_att4.Checked;
            }
        }
    }
}
