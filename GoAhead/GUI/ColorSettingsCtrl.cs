using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Settings;

namespace GoAhead.GUI.Help
{
    public partial class ColorSettingsCtrl : UserControl
    {
        public ColorSettingsCtrl()
        {
            InitializeComponent();

            m_lxtBoxRegexps.Items.Clear();

            foreach (string str in ColorSettings.Instance.GetTileRegexps())
            {
                m_lxtBoxRegexps.Items.Add(str);
            }

            UpdateLabelColors();
        }

        private void m_btnSelectColor_Click(object sender, EventArgs e)
        {
            object selectedItem = m_lxtBoxRegexps.SelectedItem;

            if (selectedItem != null)
            {
                string tileRegexp = selectedItem.ToString();
                ColorDialog cDlg = new ColorDialog();
                cDlg.ShowDialog();


                Commands.Identifier.SetColorSetting setCmd = new Commands.Identifier.SetColorSetting();
                setCmd.Color = cDlg.Color.Name;
                setCmd.FamilyRegexp = FPGA.FPGA.Instance.Family.ToString();
                setCmd.IdentifierRegexp = tileRegexp;
                Commands.CommandExecuter.Instance.Execute(setCmd);

                //Preferences.Instance.ColorSettings.SetColor(tileRegexp, cDlg.Color);
            }
        }

        private void m_btnSelectionIncr_Click(object sender, EventArgs e)
        {
            ColorDialog cDlg = new ColorDialog();
            cDlg.Color = ColorSettings.Instance.SelectionIncrement;
            cDlg.ShowDialog();

            ColorSettings.Instance.SelectionIncrement = cDlg.Color;
            UpdateLabelColors();
        }

        private void m_btnUserSelectionIncr_Click(object sender, EventArgs e)
        {
            ColorDialog cDlg = new ColorDialog();
            cDlg.Color = ColorSettings.Instance.UserSelectionIncrement;
            cDlg.ShowDialog();

            ColorSettings.Instance.UserSelectionIncrement = cDlg.Color;
            UpdateLabelColors();
        }

        private void m_btnBlockedPortsIncr_Click(object sender, EventArgs e)
        {
            ColorDialog cDlg = new ColorDialog();
            cDlg.Color = ColorSettings.Instance.BlockedPortsColor;
            cDlg.ShowDialog();

            ColorSettings.Instance.BlockedPortsColor = cDlg.Color;
            UpdateLabelColors();
        }

        private void UpdateLabelColors()
        {
            m_lblBlockedPorts.ForeColor = ColorSettings.Instance.BlockedPortsColor;
            m_lblBlockedPorts.BackColor = ColorSettings.Instance.BlockedPortsColor;
            m_lblSelection.ForeColor = ColorSettings.Instance.SelectionIncrement;
            m_lblSelection.BackColor = ColorSettings.Instance.SelectionIncrement;
            m_lblUserSelection.ForeColor = ColorSettings.Instance.UserSelectionIncrement;
            m_lblUserSelection.BackColor = ColorSettings.Instance.UserSelectionIncrement;
        }
    }
}
