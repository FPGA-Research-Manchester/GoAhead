using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GoAhead.Commands.XDLManipulation;

namespace GoAhead.GUI.ExtractModules
{
    public partial class ExtractModuleCtrl : UserControl
    {
        public ExtractModuleCtrl()
        {
            InitializeComponent();
           
            m_fileSelXDLInFile.DisableAppendCheckBox();
            m_fileSelXDLOutFile.DisableAppendCheckBox();
            m_fileSelBinMacro.DisableAppendCheckBox();
            
            m_fileSelXDLInFile.RestorePreviousSelection();
            m_fileSelXDLOutFile.RestorePreviousSelection();
            m_fileSelBinMacro.RestorePreviousSelection();
        }

        private void m_btnOk_Click(object sender, EventArgs e)
        {
            switch (ModuleSource)
            {
                case ExtractModuleForm.ModuleSourceType.FromNetlist:
                    {
                        Commands.XDLManipulation.ExtractModule cmd = new Commands.XDLManipulation.ExtractModule();
                        cmd.BinaryNetlist = m_fileSelBinMacro.FileName;
                        cmd.XDLInFile = m_fileSelXDLInFile.FileName;
                        cmd.XDLOutFile = m_fileSelXDLOutFile.FileName;
                        Commands.CommandExecuter.Instance.Execute(cmd);
                        break;
                    }
                case ExtractModuleForm.ModuleSourceType.FromSelection:
                    {
                        Commands.NetlistContainerGeneration.SaveSelectionAsModule cmd = new Commands.NetlistContainerGeneration.SaveSelectionAsModule();
                        cmd.BinaryNetlist = m_fileSelBinMacro.FileName;
                        cmd.OutFile = m_fileSelXDLOutFile.FileName;
                        Commands.CommandExecuter.Instance.Execute(cmd);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Unknown source for module: " + ModuleSource);
                    };
            }          
        }

        private void UpdateGUI()
        {
            switch (ModuleSource)
            {
                case ExtractModuleForm.ModuleSourceType.FromNetlist:                                
                    // all three
                    m_fileSelXDLInFile.Visible = true;
                    m_fileSelXDLInFile.Top = 0;
                    m_fileSelXDLOutFile.Top = m_fileSelXDLInFile.Height;
                    m_fileSelBinMacro.Top = m_fileSelXDLOutFile.Top + m_fileSelXDLInFile.Height;
                    break;
                case ExtractModuleForm.ModuleSourceType.FromSelection:                    
                    // no in file
                    m_fileSelXDLInFile.Visible = true;
                    m_fileSelXDLOutFile.Top = 0;
                    m_fileSelBinMacro.Top = m_fileSelXDLOutFile.Height;

                    break;
                default:
                    break;
            }
        }

        public ExtractModuleForm.ModuleSourceType ModuleSource
        {
            get { return m_moduleSource;}
            set { m_moduleSource = value; UpdateGUI(); }

        }

        private ExtractModuleForm.ModuleSourceType m_moduleSource = ExtractModuleForm.ModuleSourceType.FromNetlist;
    }
}
