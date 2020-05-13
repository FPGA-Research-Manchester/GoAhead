using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoAhead.GUI.ExtractModules
{
    public partial class ExtractModuleForm : Form
    {
        public enum ModuleSourceType { FromNetlist = 0, FromSelection= 1 }

        public ExtractModuleForm(ModuleSourceType sourceType)
        {
            InitializeComponent();

            Settings.StoredPreferences.Instance.GUISettings.Open(this);

            m_ectractModule.ModuleSource = sourceType;

            switch (sourceType)
            {
                case ModuleSourceType.FromNetlist:
                    Text = "Extract module from external netlist";
                    break;
                case ModuleSourceType.FromSelection:
                    Text = "Extract module from loaded design";
                    break;
                default:
                    break;
            }

        }

        private void CutOffFromDesignForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.StoredPreferences.Instance.GUISettings.Close(this);
        }
    }
}
