using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.GUI
{
    public partial class DeviceInfoForm : Form
    {
        public DeviceInfoForm()
        {
            InitializeComponent();

            m_lblFamiliyInfoString.Text = FPGA.FPGA.Instance.Family.ToString() + " (Backend " + FPGA.FPGA.Instance.BackendType + ")";
            m_lblDeviceInfoString.Text = FPGA.FPGA.Instance.DeviceName.ToString();

            int clbCount = FPGA.FPGA.Instance.GetAllTiles().Count(t => IdentifierManager.Instance.IsMatch(t.Location,IdentifierManager.RegexTypes.CLB));
            Tile clb = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location,IdentifierManager.RegexTypes.CLB));

            if (clb != null)
            {
                m_lblCLBCount.Text = "CLBs: " + clbCount + " (" + clbCount * clb.Slices.Count + " Slices)";
            }
            else
            {
                m_lblCLBCount.Text = "No CLB found, use SetCLBIdentifierRegexp to tell GoAhead how to identify a CLB";
            }
            m_lblDSPCount.Text = "DSP: " + FPGA.FPGA.Instance.GetAllTiles().Count(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.DSP));
            m_lblBRAMCount.Text = "BRAM: " + (FPGA.FPGA.Instance.GetAllTiles().Count(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.BRAM))).ToString();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);
        }

        private void DeviceInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }

    }
}
