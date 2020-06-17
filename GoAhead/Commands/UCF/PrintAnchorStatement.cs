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

        public PrintAnchorStatement(String fileName, List<String> macroNames)
        {
            this.FileName = fileName;
            this.NetlistContainerNames = macroNames;
        }

        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine("################ GoAhead ################ GoAhead ################");

            foreach (String netlistContainerName in this.NetlistContainerNames)
            {
                String anchor;
                List<XDLContainer> nlcs = new List<XDLContainer>();
                nlcs.Add((XDLContainer) NetlistContainerManager.Instance.Get(netlistContainerName));
                bool anchorFound = XDLContainer.GetAnchor(nlcs, out anchor);

                buffer.AppendLine("INST \"*inst_" + netlistContainerName + "\" LOC = \"" + anchor + "\"; # generated_by_GoAhead");
            }
        
            // write to file
            if (File.Exists(this.FileName))
            {
                TextWriter tw = new StreamWriter(FileName);
                tw.Write(buffer.ToString());
                tw.Close();
            }

            // write to gui
            this.OutputManager.WriteUCFOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "A list of netlist container names for those anchor statements will be printed")]
        public List<String> NetlistContainerNames = new List<String>();
    }
}
