using System;
using System.Collections.Generic;
using System.IO;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.Wizards
{
    public class BuildIslandStyleModuleRunBatch : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("@REM build all modules");
            string config = "config.vhd";
            string configNone = "config_none.vhd";
            OutputManager.WriteOutput("mv " + config + " " + configNone);
            OutputManager.WriteOutput("");

            foreach (string t in VHDLModules)
            {
                int index = t.IndexOf(':');
                string partialArea = t.Substring(0, index);
                string vhdlModule = t.Substring(index + 1, t.Length - (index + 1));

                string ucfFile = partialArea + ".ucf"; ;
                string blockerFile = partialArea + "_blocker.xdl"; ;

                VHDLParser p = new VHDLParser(vhdlModule);
                foreach (VHDLParserEntity entity in p.GetEntities())
                {
                    string enumType = ("build_" + entity.EntityName + "_in_" + partialArea).ToUpper();
                    string moduleNCD = partialArea + entity.EntityName + ".ncd";

                    OutputManager.WriteOutput("xcopy /Y " + ucfFile + " partial_region.ucf");
                    OutputManager.WriteOutput("cat " + configNone + " | sed 's/NONE/" + enumType + "/g' > " + config);
                    OutputManager.WriteOutput("call partial top " + Path.GetFileName(blockerFile) + " instBUFG25");
                    OutputManager.WriteOutput("mv top_routed.ncd " + moduleNCD);
                    OutputManager.WriteOutput("xdl -ncd2xdl " + moduleNCD);
                    OutputManager.WriteOutput("");
                }
            }
            OutputManager.WriteOutput("mv " + configNone + " " + config);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = @"c:\temp\m1.vhd,c:\temp\m2.vhd,c:\temp\m3.vhd")]
        public List<string> VHDLModules = new List<string>();
    }
}