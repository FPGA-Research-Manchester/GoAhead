using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    class PrintModuleSelectionPackage : VHDLCommand
    {
        public IEnumerable<string> GetEnumTypeValues()
        {
            foreach (string pr in PartialAreaNames)
            {
                foreach (string vhdlFile in GetVHDLFilePaths(pr))
                {
                    VHDLParser vhdlParser = new VHDLParser(vhdlFile);
                    foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
                    {
                        string nextEnumValue = "build_" + ent.EntityName + "_in_" + pr;
                        yield return nextEnumValue;
                    }
                }
            }
        }

       protected override void DoCommandAction()
        {
            OutputManager.WriteVHDLOutput("library IEEE;");
            OutputManager.WriteVHDLOutput("use IEEE.STD_LOGIC_1164.all;");
            OutputManager.WriteVHDLOutput("");   
            OutputManager.WriteVHDLOutput("package config is");
            OutputManager.WriteVHDLOutput("");
            string enumType = "";
            string initValue = "";

            foreach (string enumValue in GetEnumTypeValues())
            {
                if (initValue.Length == 0)
                {
                    initValue = enumValue;
                }
                enumType += (enumType.Length > 0 ? "," : "") + enumValue;
            }

            initValue = "NONE";// MinitValue.ToUpper();
            enumType = enumType.ToUpper();

            OutputManager.WriteVHDLOutput("\t" + "type module_selector_t is (" + enumType + ");");
            OutputManager.WriteVHDLOutput("\t" + "constant module_selector : module_selector_t := " + initValue + ";");
            OutputManager.WriteVHDLOutput("");
            OutputManager.WriteVHDLOutput("end config;");
        }

        private IEnumerable<string> GetVHDLFilePaths(string partialArea)
        {
            foreach (string mapping in ModulesPerArea.Where(str => str.StartsWith(partialArea + ":")))
            {
                string[] tupel = Regex.Split(mapping, partialArea + ":");
                yield return tupel[1];
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }


        [Parameter(Comment = @"pr0:c:\temp\m1.vhd,pr0:c:\temp\m2.vhd,pr1:c:\temp\m3.vhd")]
        public List<string> ModulesPerArea = new List<string>();

        [Parameter(Comment = "The name of partial areas, e.g. pr0,pr1,pr2")]
        public List<string> PartialAreaNames = new List<string>();
    }
}
