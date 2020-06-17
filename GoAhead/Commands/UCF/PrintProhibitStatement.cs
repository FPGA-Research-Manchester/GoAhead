using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.FPGA.Slices;
using GoAhead.Objects;

namespace GoAhead.Commands.UCF
{
    [CommandDescription(Description="Generate a Prohibt Statement for slice given by SliceNumber on the tile given by Location. This command does not print constrains for slices occupied by macros.", Wrapper=false)]
    class PrintProhibitStatement : UCFCommand
    {
        protected override void DoCommandAction()
        {
            foreach (String prohibitStatment in PrintProhibitStatement.GetProhibitStatments(this.Location, this.ExcludeUsedSlices))
            {
                this.OutputManager.WriteUCFOutput(prohibitStatment);
            }
        }

        public static List<String> GetProhibitStatments(String location, bool excludeUsedSlices)
        {
            Tile tile = FPGA.FPGA.Instance.GetTile(location);

            List<String> result = new List<String>();

            if (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB) ||
                IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.BRAM) ||
                IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.DSP))
            {
                foreach (Slice slice in tile.Slices.Where(s => PrintProhibitStatement.ConsiderSlice(s)))
                {
                    if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.ISE)
                    {
                        // only generate prohibit statements for free slices
                        if (excludeUsedSlices && slice.Usage == FPGATypes.SliceUsage.Macro)
                        {
                            continue;
                        }

                        result.Add("CONFIG PROHIBIT = \"" + slice.SliceName + "\";");
                    }
                    else if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
                    {
                        // 1) either generate PROHIBITS for the whole slice if no BEL is used 
                        //    that results in less PROHIBITS statements and therefore faster processing
                        // 2) or create PROHIBITS bell wise if 

                        // set_property PROHIBIT TRUE [get_sites {RAMB18_X0Y* RAMB36_X0Y*}]
                        // ALUT is for connection primitive

                        

                        if (slice.Bels.All(b => slice.GetBelUsage(b) == FPGATypes.SliceUsage.Free))
                        {
                            // 1)
                            result.Add("set_property PROHIBIT TRUE [get_sites " + slice.SliceName + "];");
                        }
                        else
                        {
                            // 2)

                            // in ISE, once a bel within a slice is used, the placer ignores this slice
                            // however, in vivado, this is not true, the placer does use (pre)used slices
                            // --> we need bel wise prohibits
                            foreach (string bel in slice.Bels.Where(b => !excludeUsedSlices || (slice.GetBelUsage(b) != FPGATypes.SliceUsage.Macro)))
                            {
                                result.Add("set_property PROHIBIT TRUE [get_bels " + slice.SliceName + "/" + bel + "];");
                            }
                        }
                    } 
                }
            }
            return result;
        }

        private static bool ConsiderSlice(Slice s)
        {
            if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.ProhibitExcludeFilter))
            {
                return !IdentifierManager.Instance.IsMatch(s.SliceName, IdentifierManager.RegexTypes.ProhibitExcludeFilter);
            }
            else
            {
                return true;
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The location string  of the tile to block")]
        public String Location = "CLBLL_X2Y78";

        [Parameter(Comment = "Whether to exclude slices used by user instantiated macros")]
        public bool ExcludeUsedSlices = true;
    }
}
