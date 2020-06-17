using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    class PrintModuleSelectionPackage : VHDLCommand
    {
        public IEnumerable<String> GetEnumTypeValues()
        {
            foreach (String pr in this.PartialAreaNames)
            {
                foreach (String vhdlFile in this.GetVHDLFilePaths(pr))
                {
                    VHDLParser vhdlParser = new VHDLParser(vhdlFile);
                    foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
                    {
                        String nextEnumValue = "build_" + ent.EntityName + "_in_" + pr;
                        yield return nextEnumValue;
                    }
                }
            }
        }

       protected override void DoCommandAction()
        {
            this.OutputManager.WriteVHDLOutput("library IEEE;");
            this.OutputManager.WriteVHDLOutput("use IEEE.STD_LOGIC_1164.all;");
            this.OutputManager.WriteVHDLOutput("");   
            this.OutputManager.WriteVHDLOutput("package config is");
            this.OutputManager.WriteVHDLOutput("");
            String enumType = "";
            String initValue = "";

            foreach (String enumValue in this.GetEnumTypeValues())
            {
                if (initValue.Length == 0)
                {
                    initValue = enumValue;
                }
                enumType += (enumType.Length > 0 ? "," : "") + enumValue;
            }

            initValue = "NONE";// MinitValue.ToUpper();
            enumType = enumType.ToUpper();

            this.OutputManager.WriteVHDLOutput("\t" + "type module_selector_t is (" + enumType + ");");
            this.OutputManager.WriteVHDLOutput("\t" + "constant module_selector : module_selector_t := " + initValue + ";");
            this.OutputManager.WriteVHDLOutput("");
            this.OutputManager.WriteVHDLOutput("end config;");
        }

        private IEnumerable<String> GetVHDLFilePaths(String partialArea)
        {
            foreach (String mapping in this.ModulesPerArea.Where(str => str.StartsWith(partialArea + ":")))
            {
                String[] tupel = Regex.Split(mapping, partialArea + ":");
                yield return tupel[1];
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = @"pr0:c:\temp\m1.vhd,pr0:c:\temp\m2.vhd,pr1:c:\temp\m3.vhd")]
        public List<String> ModulesPerArea = new List<String>();

        [Parameter(Comment = "The name of partial areas, e.g. pr0,pr1,pr2")]
        public List<String> PartialAreaNames = new List<String>();
    }
}
