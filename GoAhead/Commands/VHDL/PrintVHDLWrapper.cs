using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.Objects;
using GoAhead.Code.VHDL;

namespace GoAhead.Commands.VHDL
{
    [CommandDescription(Description = "Generate the VHDL wrapper for the partial subsystem", Wrapper = false)]
    class PrintVHDLWrapper : PrintVHDLWrapperCommand
    {
        protected override void PrintVHDLCode(VHDLFile vhdlFile)
        {
            // onle for ISE insert component declarations, in Vivado we use LUT6 primitves, and do not need component declartation
            OutputManager.WriteVHDLOutput(vhdlFile.GetSubsystem(FPGA.FPGA.Instance.BackendType == FPGA.FPGATypes.BackendType.ISE));
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

    }
}
