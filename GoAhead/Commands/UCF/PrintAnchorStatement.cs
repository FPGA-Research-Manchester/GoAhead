using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.UCF
{
    class PrintAnchorStatement : UCFCommand
    {
        public PrintAnchorStatement()
        {
        }

        public PrintAnchorStatement(string fileName, List<string> macroNames)
        {
            FileName = fileName;
            NetlistContainerNames = macroNames;
        }

        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("################ GoAhead ################ GoAhead ################");

            foreach (string netlistContainerName in NetlistContainerNames)
            {
                string anchor;
                List<XDLContainer> nlcs = new List<XDLContainer>();
                nlcs.Add((XDLContainer) NetlistContainerManager.Instance.Get(netlistContainerName));
                bool anchorFound = XDLContainer.GetAnchor(nlcs, out anchor);

                buffer.AppendLine("INST \"*inst_" + netlistContainerName + "\" LOC = \"" + anchor + "\"; # generated_by_GoAhead");
            }
        
            // write to file
            if (File.Exists(FileName))
            {
                TextWriter tw = new StreamWriter(FileName);
                tw.Write(buffer.ToString());
                tw.Close();
            }

            // write to gui
            OutputManager.WriteUCFOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "A list of netlist container names for those anchor statements will be printed")]
        public List<string> NetlistContainerNames = new List<string>();
    }
}
