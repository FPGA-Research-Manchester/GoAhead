using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    class PrintConditionalModuleInstantiation : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            if (this.PrintBegin)
            {
                this.OutputManager.WriteVHDLOutput("begin");
                this.OutputManager.WriteVHDLOutput("");
            }

            String componentPrefix = "static_placeholder_";

            // out parameters to UpdateSignalData
            Dictionary<String, List<int>> signalWidths;
            Dictionary<String, String> directions;
            List<Tuple<String, List<int>>> interfaces;
            List<String> ifSignals;
            List<String> signalsForInterface;
            List<String> signalsDeclarationsForMappingAndKeep;
            this.GetSignalList(this.PartialAreaName, false, out signalWidths, out directions, out interfaces, out ifSignals, out signalsForInterface,out signalsDeclarationsForMappingAndKeep);

            // TODO move to parser
            VHDLParser vhdlParser = new VHDLParser(this.VHDLModule);
            foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
            {
                this.OutputManager.WriteVHDLOutput("-- conditional instantiation of module " + ent.EntityName + " in partial area " + this.PartialAreaName);
                String enumType = "build_" + ent.EntityName + "_in_" + this.PartialAreaName;
                enumType = enumType.ToUpper();
                this.OutputManager.WriteVHDLOutput("mod_sel_build_" + ent.EntityName + "_in_" + this.PartialAreaName + ": if module_selector = " + enumType + " generate");
                this.OutputManager.WriteVHDLOutput("");

                String prefix = ent.EntityName + "_";
                //String module_name = this.ModulePrefix + this.PartialAreaNames;
                String placeHolderName = componentPrefix + this.PartialAreaName;

                this.OutputManager.WriteVHDLOutput("\t" + "-- the instantiation of the placeholder for the static system");
                this.OutputManager.WriteVHDLOutput("\t" + "inst_" + componentPrefix + this.PartialAreaName + " : " + placeHolderName + " port map (");
                for (int j = 0; j < interfaces.Count; j++)
                {
                    String mapping = "\t" + "\t" + interfaces[j].Item1 + " => " + prefix + interfaces[j].Item1 + "_" + this.PartialAreaName + (j < interfaces.Count - 1 ? "," : "");
                    this.OutputManager.WriteVHDLOutput(mapping);
                }
                this.OutputManager.WriteVHDLOutput("\t" + ");");

                this.OutputManager.WriteVHDLOutput("\t" + "-- the instantiation of module " + ent.EntityName);
                this.OutputManager.WriteVHDLOutput("\t" + "inst_" + this.PartialAreaName + "_" + ent.EntityName + " : " + ent.EntityName + " port map (");

                List<String> mappings = new List<String>();
                for (int j = 0; j < ent.InterfaceSignals.Count; j++)
                {
                    HDLEntitySignal s = ent.InterfaceSignals[j];
                    bool signalWillBeMappedToPlaceHolderSignal = interfaces.Any(tupel => tupel.Item1.Equals(s.SignalName));
                    if (signalWillBeMappedToPlaceHolderSignal)
                    {
                        Tuple<String, List<int>> ifElement = interfaces.FirstOrDefault(tupel => tupel.Item1.Equals(s.SignalName));
                        String mapping = "\t" + "\t" + s.SignalName + " => " + prefix + ifElement.Item1 + "_" + this.PartialAreaName + s.Range;
                        mappings.Add(mapping);                        
                    }
                    else
                    {
                        // TODO #
                        String mapping = "\t" + "\t" + s.SignalName + " => " + s.SignalName;
                        mappings.Add(mapping);   
                    }
                }
                for(int j=0;j<mappings.Count;j++)
                {
                    this.OutputManager.WriteVHDLOutput(mappings[j] + (j < mappings.Count - 1 ? "," : ""));
                }
                this.OutputManager.WriteVHDLOutput("\t" + ");");
                               
                this.OutputManager.WriteVHDLOutput("");
                this.OutputManager.WriteVHDLOutput("end generate;");
                this.OutputManager.WriteVHDLOutput("");
            }

            if (this.CloseArchitecture)
            {
                this.OutputManager.WriteVHDLOutput("end architecture;");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Wheter to open an architecture with begin")]
        public bool PrintBegin = false;

        [Parameter(Comment = "Wheter to close an architecture with end architecture")]
        public bool CloseArchitecture = false;

        [Parameter(Comment = "The VHDL module to instantiate")]
        public String VHDLModule = "module.vhd";

        [Parameter(Comment = "The name of the partial area this module will be built in")]
        public String PartialAreaName = "pr0";
    }
}
