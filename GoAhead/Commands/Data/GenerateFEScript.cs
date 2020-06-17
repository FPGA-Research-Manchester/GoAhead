using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Data
{
    class GenerateFEScript : Command
    {
        public GenerateFEScript()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">the name of the file to store the script in</param>
        /// <param name="nmcFileName">the path of the macro the script will open, modify and save</param>
        /// <param name="m_macroNames"></param>
        public GenerateFEScript(String SCRfileName, String nmcFileName, List<String> macroNames)
        {
            this.FileName = SCRfileName;
            this.MacroNames = macroNames;
            this.NMCFileName = nmcFileName;
        }

        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGA.FPGATypes.BackendType.ISE);

            TextWriter tw = new StreamWriter(FileName);

            FEScript script = new FEScript(NMCFileName);

            foreach (String macroName in this.MacroNames)
            {
                foreach (XDLMacroPort port in ((XDLContainer)NetlistContainerManager.Instance.Get(macroName)).MacroPorts)
                {
                    script.AddCreatePinCommand(port);
                }
            }

            tw.Write(script.ToString());

            tw.Close();
        }

        public override void Undo()
        {
            throw new ArgumentException("The method or operation is not implemented.");
        }

        [Parameter(Comment = "The name of the file to save the script in")]
        public String FileName;
        [Parameter(Comment = "A list of macro names that will be considered for this script")]
        public List<String> MacroNames;
        [Parameter(Comment = "The name of the macro the scripz will try to open")]
        public String NMCFileName;
    }
}


