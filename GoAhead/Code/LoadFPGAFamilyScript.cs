using System;
using System.IO;

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Check whether next to the GoAhead binary an FPGA familiy specific script (e.g. Spartan6.goa) exists and execute it", Wrapper = true)]
    public class LoadFPGAFamilyScript : Command
    {
        protected override void DoCommandAction()
        {
            string familyScript = Program.AssemblyDirectory + Path.DirectorySeparatorChar + FPGA.FPGA.Instance.Family + ".goa";

            if (File.Exists(familyScript))
            {
                OutputManager.WriteOutput("Reading hook script " + familyScript);
                RunScript runCmd = new RunScript();
                runCmd.FileName = familyScript;
                CommandExecuter.Instance.Execute(runCmd);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}