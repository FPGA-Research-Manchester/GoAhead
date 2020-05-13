using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    [CommandDescription(Description="Read in an VHDL file and print a component declaration for each entity we find", Wrapper=false, Publish=true)]
    class PrintComponentDeclarationForVHDLModule : VHDLCommand
    {
        protected override void DoCommandAction()
        {
            // TODO move to parser
            VHDLParser vhdlParser = new VHDLParser(VHDLModule);
            foreach (VHDLParserEntity ent in vhdlParser.GetEntities())
            {
                OutputManager.WriteVHDLOutput("-- component declaration for module " + ent.EntityName);
                OutputManager.WriteVHDLOutput("component " + ent.EntityName + " is port (");
                for (int i = 0; i < ent.InterfaceSignals.Count; i++)
                {
                    // weiter: invertieren und dann auch signal mapping bei instasntiierung abgleichen
                    OutputManager.WriteVHDLOutput("\t" + ent.InterfaceSignals[i].WholeSignalDeclaration + (i < ent.InterfaceSignals.Count - 1 ? ";" : ");"));
                }
                OutputManager.WriteVHDLOutput("end component;");
                OutputManager.WriteVHDLOutput("");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The path to VHDL module  file to read")]
        public string VHDLModule = "top.vhd";
    }
}
