using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.VHDL
{
    class PrintArchitectureWithStaticPlaceholderInstantiation : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            string componentPrefix = "static_placeholder_";
            string entityName = "partial_master";

            OutputManager.WriteVHDLOutput("library IEEE;");
            OutputManager.WriteVHDLOutput("use IEEE.STD_LOGIC_1164.ALL;");
            OutputManager.WriteVHDLOutput("");
            OutputManager.WriteVHDLOutput("use work.config.all;");
            OutputManager.WriteVHDLOutput("");
            OutputManager.WriteVHDLOutput("entity " + entityName + " is port (");
            OutputManager.WriteVHDLOutput("\t" + "clk : in std_logic);");
            OutputManager.WriteVHDLOutput("end " + entityName + ";");
            OutputManager.WriteVHDLOutput("");

            OutputManager.WriteVHDLOutput("architecture module_implementation of " + entityName + " is");
            OutputManager.WriteVHDLOutput("");

            // out parameters to UpdateSignalData
            Dictionary<string, List<int>> signalWidths;
            Dictionary<string, string> directions;
            List<Tuple<string, List<int>>> interfaces;
            List<string> ifSignals;
            List<string> signalsForInterface;
            List<string> signalsDeclarationsForMappingAndKeep;

            for (int i = 0; i < PartialAreaNames.Count; i++)
            {
                UpdateSignalData(PartialAreaNames[i], out signalWidths, out directions, out interfaces, out ifSignals, out signalsForInterface, out signalsDeclarationsForMappingAndKeep);

                // comp decl
                OutputManager.WriteVHDLOutput("-- the declaration of the placeholder for the static system");
                OutputManager.WriteVHDLOutput("component " + componentPrefix + PartialAreaNames[i] + " is port (");
                foreach (string s in signalsForInterface)
                {
                    OutputManager.WriteVHDLOutput("\t" + s);
                }
                OutputManager.WriteVHDLOutput("end component;");
                OutputManager.WriteVHDLOutput("");
            }
        }
        

        private void UpdateSignalData(string partialAreaName, out Dictionary<string, List<int>> signalWidths, out Dictionary<string, string> directions, out List<Tuple<string, List<int>>> interfaces, out List<string> ifSignals, out List<string> signalsForInterface, out List<string> signalsDeclarationsForMappingAndKeep)
        {
            GetSignalList(partialAreaName, true,
                out signalWidths,
                out directions,
                out interfaces,
                out ifSignals,
                out signalsForInterface,
                out signalsDeclarationsForMappingAndKeep);
            /*
            this.GetSignalList(partialAreaName, false,
                out signalWidths,
                out directions,
                out interfaces,
                out ifSignals,
                out signalsForInterfaceInverted,
                out signalsDeclarationsForMappingAndKeep);
             * */
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
                
        [Parameter(Comment = "The name of partial areas, e.g. pr0,pr1,pr2")]
        public List<string> PartialAreaNames = new List<string>();
    }
}
