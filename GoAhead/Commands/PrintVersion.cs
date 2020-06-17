using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    class PrintVersion : Command
    {
        protected override void DoCommandAction()
        {
            this.OutputManager.WriteOutput("GoAhead Version 1.810");
            this.OutputManager.WriteOutput("Latest changes:");

            this.OutputManager.WriteOutput("Override arguments for alias commands, e.g. MySelection UpperLeftX=34000;");
            this.OutputManager.WriteOutput("AddArcs for Vivado");
            this.OutputManager.WriteOutput("Enrich PrintSelection with more tile info, add PrintSelectionSimple doing the old print out");
            this.OutputManager.WriteOutput("Execute use cmd /c command");
            this.OutputManager.WriteOutput("Blockers are generate like TILE/FROM -> TILE/TO");
            this.OutputManager.WriteOutput("Minimal support for UltraScale");
            this.OutputManager.WriteOutput("Fix AddBlockToSelection for Virtex7");
            this.OutputManager.WriteOutput("Introduce Prefix in PreRoutePRLink for supprting hierarchical nets");
            this.OutputManager.WriteOutput("Introduce StepWidth for AnnotateSignalNames for precise mapping (to prevent pins swaps)");
            this.OutputManager.WriteOutput("Supported for newer devices US,V7,...");
            this.OutputManager.WriteOutput("Support for 2016.2");
            this.OutputManager.WriteOutput("Resource reporting");
            this.OutputManager.WriteOutput("Vivado end pip blocking");
            this.OutputManager.WriteOutput("ISE commands deprecated");
            this.OutputManager.WriteOutput("Parser bugfix");
            this.OutputManager.WriteOutput("Add first Vivado commands");
            this.OutputManager.WriteOutput("Remove concurrent XDL generation");
            this.OutputManager.WriteOutput("Blanks in identifiers");
            this.OutputManager.WriteOutput("Extended routing model");
            this.OutputManager.WriteOutput("BitWiseOr");
            this.OutputManager.WriteOutput("CopyNetListContainerContent");
            this.OutputManager.WriteOutput("...");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
