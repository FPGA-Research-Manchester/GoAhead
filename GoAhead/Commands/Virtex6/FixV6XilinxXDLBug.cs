using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.Code.XDL;

namespace GoAhead.Commands.Virtex6
{
    [CommandDescription(Description = "Remove instance identifier from slice configurations (fix a Xilinx bug that inserts commas)", Wrapper = false)]
    class FixV6XilinxXDLBug : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            FPGA.FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

            XDLContainer nlc = (XDLContainer)this.GetNetlistContainer();

            List<String> fixedSliceConfigurations = new List<String>();
            foreach (XDLInstance inst in nlc.Instances)
            { 
                // A5LUT:Inst_PE/Mmult_OPA[31]_OPB[31]_MuLt_17_OUT_Madd10_cy<6>:#LUT -> A5LUT::#LUT
                String originalCode = inst.ToString();
                String fixedCode = originalCode;
                if (!fixedCode.Contains("A5LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " A5LUT:(.+?):#LUT:", " A5LUT::#LUT:"); }
                if (!fixedCode.Contains("B5LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " B5LUT:(.+?):#LUT:", " B5LUT::#LUT:"); }
                if (!fixedCode.Contains("C5LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " C5LUT:(.+?):#LUT:", " C5LUT::#LUT:"); }
                if (!fixedCode.Contains("D5LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " D5LUT:(.+?):#LUT:", " D5LUT::#LUT:"); }
                if (!fixedCode.Contains("A6LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " A6LUT:(.+?):#LUT:", " A6LUT::#LUT:"); }
                if (!fixedCode.Contains("B6LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " B6LUT:(.+?):#LUT:", " B6LUT::#LUT:"); }
                if (!fixedCode.Contains("C6LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " C6LUT:(.+?):#LUT:", " C6LUT::#LUT:"); }
                if (!fixedCode.Contains("D6LUT::#OFF")) { fixedCode = Regex.Replace(fixedCode, " D6LUT:(.+?):#LUT:", " D6LUT::#LUT:"); }
                // neu
                // A5FF:Inst_PE/Mmult_OPA[31]_OPB[31]_MuLt_18_OUT_OPA,OPB<20>_x_OPA,OPB<62>_mand1_FRB: -> A5FF::
                if (!fixedCode.Contains("A5FF::")) { fixedCode = Regex.Replace(fixedCode, "A5FF:(.+?):", "A5FF::"); }
                if (!fixedCode.Contains("B5FF::")) { fixedCode = Regex.Replace(fixedCode, "B5FF:(.+?):", "B5FF::"); }
                if (!fixedCode.Contains("C5FF::")) { fixedCode = Regex.Replace(fixedCode, "C5FF:(.+?):", "C5FF::"); }
                if (!fixedCode.Contains("D5FF::")) { fixedCode = Regex.Replace(fixedCode, "D5FF:(.+?):", "D5FF::"); }

                // BFF:Inst_PE/config_data_en_0:#FF --> BFF::#FF
                if (!fixedCode.Contains("AFF::#OFF") && !fixedCode.Contains("AFF::#FF")) { fixedCode = Regex.Replace(fixedCode, "AFF:(.+?):", "AFF::"); }
                if (!fixedCode.Contains("BFF::#OFF") && !fixedCode.Contains("BFF::#FF")) { fixedCode = Regex.Replace(fixedCode, "BFF:(.+?):", "BFF::"); }
                if (!fixedCode.Contains("CFF::#OFF") && !fixedCode.Contains("CFF::#FF")) { fixedCode = Regex.Replace(fixedCode, "CFF:(.+?):", "CFF::"); }
                if (!fixedCode.Contains("DFF::#OFF") && !fixedCode.Contains("DFF::#FF")) { fixedCode = Regex.Replace(fixedCode, "DFF:(.+?):", "DFF::"); }

                fixedSliceConfigurations.Add(fixedCode);
            }

            nlc.Remove(delegate(Instance i) {return true;});
            foreach(String xdlCode in fixedSliceConfigurations)
            {
                nlc.AddSliceCodeBlock(xdlCode);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
