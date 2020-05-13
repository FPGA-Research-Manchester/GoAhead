using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Generate XDL for the selecteded macros, convert the XDL code, and show the result in the FPGA Editor", Wrapper=true)]
    class LanchNetlistInFE : Command
    {
        public LanchNetlistInFE()
        {
        }

        public LanchNetlistInFE(List<string> macroNames, string nmcFile, bool includePorts, bool includeDummyNets, bool runFEScript, bool launchFE)
        {
            MacroNames = macroNames;
            NMCFile = nmcFile;
            IncludePorts = includePorts;
            RunFEScript = runFEScript;
            LaunchFE = launchFE;
            IncludeDummyNets = includeDummyNets;
        }

        protected override void DoCommandAction()
        {
            // read env
            string xilinxDir = Environment.GetEnvironmentVariable("XILINX");
            if (xilinxDir[xilinxDir.Length - 1].Equals(Path.DirectorySeparatorChar))
            {
                xilinxDir += Path.DirectorySeparatorChar;
            }

            string tempXDLFile = Path.ChangeExtension(Path.GetTempFileName(), "xdl");
            string tempNCDFile = Path.ChangeExtension(Path.GetTempFileName(), "ncd");
            string tempSCRFile = Path.ChangeExtension(Path.GetTempFileName(), "scr");
            string tempBatchFile = Path.ChangeExtension(Path.GetTempFileName(), "bat");
            string tempLogFile = Path.ChangeExtension(Path.GetTempFileName(), "log");        

            // create xdl
            // CommandExecuter.Instance.Execute(new GenerateXDL(tempXDLFile, this.MacroNames, this.IncludePorts, this.IncludeDummyNets, true, true, true));

            GenerateXDL genXDLCmd = new GenerateXDL();
            genXDLCmd.DesignName = "blocker";
            genXDLCmd.FileName = tempXDLFile;
            genXDLCmd.IncludeDesignStatement = true;
            genXDLCmd.IncludeDummyNets = IncludeDummyNets;
            genXDLCmd.IncludeDesignStatement = true;
            genXDLCmd.IncludeModuleHeader= false;
            genXDLCmd.IncludeModuleFooter = false;
            genXDLCmd.IncludePorts = IncludePorts;
            genXDLCmd.NetlistContainerNames = MacroNames;
            genXDLCmd.SortInstancesBySliceName = false;
            CommandExecuter.Instance.Execute(genXDLCmd);

            if (RunFEScript)
            {
                // create fe
                CommandExecuter.Instance.Execute(new GenerateFEScript(tempSCRFile, tempNCDFile, MacroNames));
            }

            // convert xdl2ncd, run fe script and launch fe in from batch file
            StreamWriter batchFile = new StreamWriter(tempBatchFile);
            string conversionCmd = "xdl -xdl2ncd \"" + tempXDLFile + "\" \"" + tempNCDFile + "\" -nodrc";
            batchFile.WriteLine(conversionCmd);
            // no need for fe script when including port statements
            if (RunFEScript)
            {
                batchFile.WriteLine("fpga_edline -p \"" + tempSCRFile + "\" >> \"" + tempLogFile + "\"");
            }
            if (!string.IsNullOrEmpty(NMCFile))
            {
                batchFile.WriteLine("copy \"" + tempNCDFile + "\" \"" + NMCFile + "\" /Y");
            }
            if (LaunchFE)
            {
                batchFile.WriteLine("start fpga_editor \"" + tempNCDFile + "\"");
            }

            batchFile.WriteLine("del \"" + tempXDLFile + "\"");
            batchFile.WriteLine("del \"" + tempSCRFile  + "\"");
            batchFile.WriteLine("del \"" + tempLogFile + "\""); 
            batchFile.Close();

            Process.Start(tempBatchFile);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "A list of macro names that will be considered for this script")]
        public List<string> MacroNames = new List<string>();

        [Parameter(Comment = "File in which to save the NMC")]
        public string NMCFile = "";

        [Parameter(Comment = "Wheter to include XDL ports or not")]
        public bool IncludePorts = false;

        [Parameter(Comment = "Wheter to include run the FE script or not")]
        public bool RunFEScript = false;

        [Parameter(Comment = "Wheter to launch the macro in FPGA-Editor")]
        public bool LaunchFE = false;

        [Parameter(Comment = "Wheter to include a dummy net for each XDL port or not")]
        public bool IncludeDummyNets = false;

    }
}
