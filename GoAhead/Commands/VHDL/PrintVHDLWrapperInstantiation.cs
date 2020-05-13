using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    [CommandDescription(Description = "Generate the instantiation of the VHDL wrapper for the partial subsystem", Wrapper = false)]
    class PrintVHDLWrapperInstantiation : PrintVHDLWrapperCommand
    {
        protected override void PrintVHDLCode(VHDLFile vhdlFile)
        {
            OutputManager.WriteVHDLOutput(vhdlFile.GetSubsystemInstantiation());
        }
        
        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
