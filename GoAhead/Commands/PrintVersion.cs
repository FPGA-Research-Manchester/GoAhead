using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    class PrintVersion : Command
    {
        private const string version = "2.0.7a";

        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("GoAhead Version " + version);
            OutputManager.WriteOutput("Latest changes:");

            OutputManager.WriteOutput("Override arguments for alias commands, e.g. MySelection UpperLeftX=34000;");
            OutputManager.WriteOutput("AddArcs for Vivado");
            OutputManager.WriteOutput("Enrich PrintSelection with more tile info, add PrintSelectionSimple doing the old print out");
            OutputManager.WriteOutput("Execute use cmd /c command");
            OutputManager.WriteOutput("Blockers are generate like TILE/FROM -> TILE/TO");
            OutputManager.WriteOutput("Minimal support for UltraScale");
            OutputManager.WriteOutput("Fix AddBlockToSelection for Virtex7");
            OutputManager.WriteOutput("Introduce Prefix in PreRoutePRLink for supprting hierarchical nets");
            OutputManager.WriteOutput("Introduce StepWidth for AnnotateSignalNames for precise mapping (to prevent pins swaps)");
            OutputManager.WriteOutput("Supported for newer devices US,V7,...");
            OutputManager.WriteOutput("Support for 2016.2");
            OutputManager.WriteOutput("Resource reporting");
            OutputManager.WriteOutput("Vivado end pip blocking");
            OutputManager.WriteOutput("ISE commands deprecated");
            OutputManager.WriteOutput("Parser bugfix");
            OutputManager.WriteOutput("Add first Vivado commands");
            OutputManager.WriteOutput("Remove concurrent XDL generation");
            OutputManager.WriteOutput("Blanks in identifiers");
            OutputManager.WriteOutput("Extended routing model");
            OutputManager.WriteOutput("BitWiseOr");
            OutputManager.WriteOutput("CopyNetListContainerContent");
            OutputManager.WriteOutput("...");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
