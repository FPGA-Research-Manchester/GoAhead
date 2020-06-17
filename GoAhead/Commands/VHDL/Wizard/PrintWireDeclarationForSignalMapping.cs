using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    class PrintWireDeclarationForSignalMapping : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            Dictionary<String, List<int>> signalWidths;
            Dictionary<String, String> directions;
            List<Tuple<String, List<int>>> interfaces;
            List<String> ifSignals;
            List<String> signalsForInterface;
            List<String> signalsDeclarationsForMappingAndKeep;
            this.GetSignalList(this.PartialAreaName, false,
                out signalWidths,
                out directions,
                out interfaces,
                out ifSignals,
                out signalsForInterface,
                out signalsDeclarationsForMappingAndKeep);

            if (this.PrintAttributeDeclaration)
            {
                this.OutputManager.WriteVHDLOutput("attribute s : string;");
                this.OutputManager.WriteVHDLOutput("attribute keep : string;");
            }

            List<String> declaredSignalNames = new List<String>();
            VHDLParser vhdlParser = new VHDLParser(this.VHDLModule);
            foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
            {
                this.OutputManager.WriteVHDLOutput("-- declaration of signals to connect module " + ent.EntityName);
                for (int i = 0; i < ent.InterfaceSignals.Count; i++)
                {
                    String signalName = ent.InterfaceSignals[i].SignalName;
                    if (!signalWidths.ContainsKey(signalName))
                    {
                        this.OutputManager.WriteVHDLOutput("-- could not map the module signal " + ent.InterfaceSignals[i].SignalName + " to a signal in the partial area");
                    }
                    else
                    {
                        int signalWidthOfSignalNameInPartialArea = signalWidths[signalName].Count;
                        String signalDecl = "signal " + ent.EntityName + "_" + signalName + "_" + this.PartialAreaName + " : std_logic_vector(" + (signalWidthOfSignalNameInPartialArea - 1) + " downto 0) := (others => '1');";
                        this.OutputManager.WriteVHDLOutput(signalDecl);
                        declaredSignalNames.Add(ent.EntityName + "_" + signalName + "_" + this.PartialAreaName);
                    }
                }
                this.OutputManager.WriteVHDLOutput("");
            }
          
            foreach (String signalName in declaredSignalNames)
            {
                this.OutputManager.WriteVHDLOutput("attribute s of " + signalName + " : signal is \"yes\";");
                this.OutputManager.WriteVHDLOutput("attribute keep of " + signalName + " : signal is \"yes\";");
            }
            this.OutputManager.WriteVHDLOutput("");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the partial area this module will be built in")]
        public String PartialAreaName = "pr0";

        [Parameter(Comment = "The path to VHDL module  file to read")]
        public String VHDLModule = "top.vhd";
                
        [Parameter(Comment = "The path to VHDL module  file to read")]
        public bool PrintAttributeDeclaration = true;
        
    }
}
