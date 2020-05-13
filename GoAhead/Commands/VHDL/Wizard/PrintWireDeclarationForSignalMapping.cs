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
            Dictionary<string, List<int>> signalWidths;
            Dictionary<string, string> directions;
            List<Tuple<string, List<int>>> interfaces;
            List<string> ifSignals;
            List<string> signalsForInterface;
            List<string> signalsDeclarationsForMappingAndKeep;
            GetSignalList(PartialAreaName, false,
                out signalWidths,
                out directions,
                out interfaces,
                out ifSignals,
                out signalsForInterface,
                out signalsDeclarationsForMappingAndKeep);

            if (PrintAttributeDeclaration)
            {
                OutputManager.WriteVHDLOutput("attribute s : string;");
                OutputManager.WriteVHDLOutput("attribute keep : string;");
            }

            List<string> declaredSignalNames = new List<string>();
            VHDLParser vhdlParser = new VHDLParser(VHDLModule);
            foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
            {
                OutputManager.WriteVHDLOutput("-- declaration of signals to connect module " + ent.EntityName);
                for (int i = 0; i < ent.InterfaceSignals.Count; i++)
                {
                    string signalName = ent.InterfaceSignals[i].SignalName;
                    if (!signalWidths.ContainsKey(signalName))
                    {
                        OutputManager.WriteVHDLOutput("-- could not map the module signal " + ent.InterfaceSignals[i].SignalName + " to a signal in the partial area");
                    }
                    else
                    {
                        int signalWidthOfSignalNameInPartialArea = signalWidths[signalName].Count;
                        string signalDecl = "signal " + ent.EntityName + "_" + signalName + "_" + PartialAreaName + " : std_logic_vector(" + (signalWidthOfSignalNameInPartialArea - 1) + " downto 0) := (others => '1');";
                        OutputManager.WriteVHDLOutput(signalDecl);
                        declaredSignalNames.Add(ent.EntityName + "_" + signalName + "_" + PartialAreaName);
                    }
                }
                OutputManager.WriteVHDLOutput("");
            }
          
            foreach (string signalName in declaredSignalNames)
            {
                OutputManager.WriteVHDLOutput("attribute s of " + signalName + " : signal is \"yes\";");
                OutputManager.WriteVHDLOutput("attribute keep of " + signalName + " : signal is \"yes\";");
            }
            OutputManager.WriteVHDLOutput("");
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the partial area this module will be built in")]
        public string PartialAreaName = "pr0";

        [Parameter(Comment = "The path to VHDL module  file to read")]
        public string VHDLModule = "top.vhd";
                
        [Parameter(Comment = "The path to VHDL module  file to read")]
        public bool PrintAttributeDeclaration = true;
        
    }
}
