using System;
using System.Linq;
using GoAhead.Code;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Code.TCL;

namespace GoAhead.Commands.NetlistContainerGeneration
{
    [CommandDescription(Description = "Read in a design netlist (xdl or viv_nl) into a netlist container", Wrapper = false, Publish = true)]
    class OpenDesign : NetlistContainerCommand
    {
        protected override void DoCommandAction()
        {
            NetlistContainer nlc = GetNetlistContainer();

            DesignParser parser = DesignParser.CreateDesignParser(FileName);

            try
            {
                parser.ParseDesign(nlc, this);
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error during parsing the design " + FileName + ": " + e.Message + ". Are you trying to open the design on the correct device?");
            }

            foreach (Instance inst in nlc.Instances)
            {
                Tile t = FPGA.FPGA.Instance.GetTile(inst.Location);
                if (!t.HasSlice(inst.SliceName))
                {
                    OutputManager.WriteWarning("Can not find primitve " + inst.SliceName + " on tile " + t.Location);
                }
                else
                {
                    Slice s = t.GetSliceByName(inst.SliceName);
                    s.Usage = FPGATypes.SliceUsage.Macro;
                    if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
                    {
                        TCLInstance tclInst = (TCLInstance)inst;
                        if (!string.IsNullOrEmpty(tclInst.BELType))
                        {
                            s.SetBelUsage(tclInst.BELType, FPGATypes.SliceUsage.Macro);
                        }
                    }
                }
            }

            foreach (Net n in nlc.Nets)
            {
                n.BlockUsedResources();
            }

            if (AutoFixXDLBugs)
            {
                FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE);

                if (FPGA.FPGA.Instance.Family == FPGATypes.FPGAFamily.Spartan6)
                {
                    foreach (XDLInstance inst in nlc.Instances.Where(i => i.Location.Contains("IOB") || i.SliceType.Equals("IOB")))
                    {
                        string code = inst.ToString();
                        if (!code.Contains("OUTBUF:"))
                        {
                            code = code.Replace("PRE_EMPHASIS::#OFF", "");
                            inst.SetCode(code);
                            OutputManager.WriteWarning("Fixed XDL code for instance " + inst.Name);
                        }
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The netlist to read in (either .xdl oder .viv_nl)")]
        public string FileName = "design.xdl";

        [Parameter(Comment = "Whether or not ot automatically fix known XDL bugs")]
        public bool AutoFixXDLBugs = false;
    }
}
