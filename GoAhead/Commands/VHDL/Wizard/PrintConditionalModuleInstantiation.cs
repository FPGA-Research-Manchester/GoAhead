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
            if (PrintBegin)
            {
                OutputManager.WriteVHDLOutput("begin");
                OutputManager.WriteVHDLOutput("");
            }

            string componentPrefix = "static_placeholder_";

            // out parameters to UpdateSignalData
            Dictionary<string, List<int>> signalWidths;
            Dictionary<string, string> directions;
            List<Tuple<string, List<int>>> interfaces;
            List<string> ifSignals;
            List<string> signalsForInterface;
            List<string> signalsDeclarationsForMappingAndKeep;
            GetSignalList(PartialAreaName, false, out signalWidths, out directions, out interfaces, out ifSignals, out signalsForInterface,out signalsDeclarationsForMappingAndKeep);

            // TODO move to parser
            VHDLParser vhdlParser = new VHDLParser(VHDLModule);
            foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
            {
                OutputManager.WriteVHDLOutput("-- conditional instantiation of module " + ent.EntityName + " in partial area " + PartialAreaName);
                string enumType = "build_" + ent.EntityName + "_in_" + PartialAreaName;
                enumType = enumType.ToUpper();
                OutputManager.WriteVHDLOutput("mod_sel_build_" + ent.EntityName + "_in_" + PartialAreaName + ": if module_selector = " + enumType + " generate");
                OutputManager.WriteVHDLOutput("");

                string prefix = ent.EntityName + "_";
                //String module_name = this.ModulePrefix + this.PartialAreaNames;
                string placeHolderName = componentPrefix + PartialAreaName;

                OutputManager.WriteVHDLOutput("\t" + "-- the instantiation of the placeholder for the static system");
                OutputManager.WriteVHDLOutput("\t" + "inst_" + componentPrefix + PartialAreaName + " : " + placeHolderName + " port map (");
                for (int j = 0; j < interfaces.Count; j++)
                {
                    string mapping = "\t" + "\t" + interfaces[j].Item1 + " => " + prefix + interfaces[j].Item1 + "_" + PartialAreaName + (j < interfaces.Count - 1 ? "," : "");
                    OutputManager.WriteVHDLOutput(mapping);
                }
                OutputManager.WriteVHDLOutput("\t" + ");");

                OutputManager.WriteVHDLOutput("\t" + "-- the instantiation of module " + ent.EntityName);
                OutputManager.WriteVHDLOutput("\t" + "inst_" + PartialAreaName + "_" + ent.EntityName + " : " + ent.EntityName + " port map (");

                List<string> mappings = new List<string>();
                for (int j = 0; j < ent.InterfaceSignals.Count; j++)
                {
                    HDLEntitySignal s = ent.InterfaceSignals[j];
                    bool signalWillBeMappedToPlaceHolderSignal = interfaces.Any(tupel => tupel.Item1.Equals(s.SignalName));
                    if (signalWillBeMappedToPlaceHolderSignal)
                    {
                        Tuple<string, List<int>> ifElement = interfaces.FirstOrDefault(tupel => tupel.Item1.Equals(s.SignalName));
                        string mapping = "\t" + "\t" + s.SignalName + " => " + prefix + ifElement.Item1 + "_" + PartialAreaName + s.Range;
                        mappings.Add(mapping);                        
                    }
                    else
                    {
                        // TODO #
                        string mapping = "\t" + "\t" + s.SignalName + " => " + s.SignalName;
                        mappings.Add(mapping);   
                    }
                }
                for(int j=0;j<mappings.Count;j++)
                {
                    OutputManager.WriteVHDLOutput(mappings[j] + (j < mappings.Count - 1 ? "," : ""));
                }
                OutputManager.WriteVHDLOutput("\t" + ");");
                               
                OutputManager.WriteVHDLOutput("");
                OutputManager.WriteVHDLOutput("end generate;");
                OutputManager.WriteVHDLOutput("");
            }

            if (CloseArchitecture)
            {
                OutputManager.WriteVHDLOutput("end architecture;");
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
        public string VHDLModule = "module.vhd";

        [Parameter(Comment = "The name of the partial area this module will be built in")]
        public string PartialAreaName = "pr0";
    }
}
