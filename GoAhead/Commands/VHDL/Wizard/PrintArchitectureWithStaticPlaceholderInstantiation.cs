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
            String componentPrefix = "static_placeholder_";
            String entityName = "partial_master";

            this.OutputManager.WriteVHDLOutput("library IEEE;");
            this.OutputManager.WriteVHDLOutput("use IEEE.STD_LOGIC_1164.ALL;");
            this.OutputManager.WriteVHDLOutput("");
            this.OutputManager.WriteVHDLOutput("use work.config.all;");
            this.OutputManager.WriteVHDLOutput("");
            this.OutputManager.WriteVHDLOutput("entity " + entityName + " is port (");
            this.OutputManager.WriteVHDLOutput("\t" + "clk : in std_logic);");
            this.OutputManager.WriteVHDLOutput("end " + entityName + ";");
            this.OutputManager.WriteVHDLOutput("");

            this.OutputManager.WriteVHDLOutput("architecture module_implementation of " + entityName + " is");
            this.OutputManager.WriteVHDLOutput("");

            // out parameters to UpdateSignalData
            Dictionary<String, List<int>> signalWidths;
            Dictionary<String, String> directions;
            List<Tuple<String, List<int>>> interfaces;
            List<String> ifSignals;
            List<String> signalsForInterface;
            List<String> signalsDeclarationsForMappingAndKeep;

            for (int i = 0; i < this.PartialAreaNames.Count; i++)
            {
                this.UpdateSignalData(this.PartialAreaNames[i], out signalWidths, out directions, out interfaces, out ifSignals, out signalsForInterface, out signalsDeclarationsForMappingAndKeep);

                // comp decl
                this.OutputManager.WriteVHDLOutput("-- the declaration of the placeholder for the static system");
                this.OutputManager.WriteVHDLOutput("component " + componentPrefix + this.PartialAreaNames[i] + " is port (");
                foreach (String s in signalsForInterface)
                {
                    this.OutputManager.WriteVHDLOutput("\t" + s);
                }
                this.OutputManager.WriteVHDLOutput("end component;");
                this.OutputManager.WriteVHDLOutput("");
            }
        }
        

        private void UpdateSignalData(String partialAreaName, out Dictionary<String, List<int>> signalWidths, out Dictionary<String, String> directions, out List<Tuple<String, List<int>>> interfaces, out List<String> ifSignals, out List<String> signalsForInterface, out List<String> signalsDeclarationsForMappingAndKeep)
        {
            this.GetSignalList(partialAreaName, true,
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
        public List<String> PartialAreaNames = new List<String>();
    }
}
