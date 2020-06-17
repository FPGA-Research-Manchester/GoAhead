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
            this.OutputManager.WriteOutput("@REM build all modules");
            String config = "config.vhd";
            String configNone = "config_none.vhd";
            this.OutputManager.WriteOutput("mv " + config + " " + configNone);
            this.OutputManager.WriteOutput("");

            foreach (String t in this.VHDLModules)
            {
                int index = t.IndexOf(':');
                String partialArea = t.Substring(0, index);
                String vhdlModule = t.Substring(index + 1, t.Length - (index + 1));

                String ucfFile = partialArea + ".ucf"; ;
                String blockerFile = partialArea + "_blocker.xdl"; ;

                VHDLParser p = new VHDLParser(vhdlModule);
                foreach (VHDLParserEntity entity in p.GetEntities())
                {
                    String enumType = ("build_" + entity.EntityName + "_in_" + partialArea).ToUpper();
                    String moduleNCD = partialArea + entity.EntityName + ".ncd";

                    this.OutputManager.WriteOutput("xcopy /Y " + ucfFile + " partial_region.ucf");
                    this.OutputManager.WriteOutput("cat " + configNone + " | sed 's/NONE/" + enumType + "/g' > " + config);
                    this.OutputManager.WriteOutput("call partial top " + Path.GetFileName(blockerFile) + " instBUFG25");
                    this.OutputManager.WriteOutput("mv top_routed.ncd " + moduleNCD);
                    this.OutputManager.WriteOutput("xdl -ncd2xdl " + moduleNCD);
                    this.OutputManager.WriteOutput("");
                }
            }
            this.OutputManager.WriteOutput("mv " + configNone + " " + config);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = @"c:\temp\m1.vhd,c:\temp\m2.vhd,c:\temp\m3.vhd")]
        public List<String> VHDLModules = new List<String>();
    }
}