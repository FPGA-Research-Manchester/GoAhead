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
            openCmd.FileName = XDLInFile;
            OutputManager.WriteOutput(openCmd.ToString());

            OutputManager.WriteOutput("ClearSelection;");            
            foreach (Command cmd in FPGA.TileSelectionManager.Instance.GetListOfAddToSelectionXYCommandsForUserSelection(UserSelectionType))
            {
                OutputManager.WriteOutput(cmd.ToString());
            }

            Print printTextCmd = new Print();
            printTextCmd.Text = "resource utilization for " + XDLInFile;
            OutputManager.WriteOutput(printTextCmd.ToString());

            PrintResourceConsumptionInSelection printCmd = new PrintResourceConsumptionInSelection();
            OutputManager.WriteOutput(printCmd.ToString());
            OutputManager.WriteOutput("Reset;");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The netlist to analyze")]
        public string XDLInFile = "";

        [Parameter(Comment = "The name of the user selection type (= module shape)")]
        public string UserSelectionType = "PartialArea";
    }
}
