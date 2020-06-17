using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.NetlistContainerGeneration;

namespace GoAhead.Commands
{
    class PrintResourceUtilizationScript : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OpenDesign openCmd = new OpenDesign();
            openCmd.FileName = this.XDLInFile;
            this.OutputManager.WriteOutput(openCmd.ToString());

            this.OutputManager.WriteOutput("ClearSelection;");            
            foreach (Command cmd in FPGA.TileSelectionManager.Instance.GetListOfAddToSelectionXYCommandsForUserSelection(this.UserSelectionType))
            {
                this.OutputManager.WriteOutput(cmd.ToString());
            }

            Print printTextCmd = new Print();
            printTextCmd.Text = "resource utilization for " + this.XDLInFile;
            this.OutputManager.WriteOutput(printTextCmd.ToString());

            PrintResourceConsumptionInSelection printCmd = new PrintResourceConsumptionInSelection();
            this.OutputManager.WriteOutput(printCmd.ToString());
            this.OutputManager.WriteOutput("Reset;");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The netlist to analyze")]
        public String XDLInFile = "";

        [Parameter(Comment = "The name of the user selection type (= module shape)")]
        public String UserSelectionType = "PartialArea";
    }
}
