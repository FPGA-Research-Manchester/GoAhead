using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.UCF
{
    [CommandDescription(Description = "Output a location constraint")]
    class PrintLocationConstraint : UCFCommand
    {
        public PrintLocationConstraint()
        {
        }

        protected override void DoCommandAction()
        {
            if (IdentifierManager.Instance.IsMatch(this.Location, IdentifierManager.RegexTypes.CLB))
            {
                Tile where = FPGA.FPGA.Instance.GetTile(this.Location);

                if (this.SliceNumber >= where.Slices.Count)
                {
                    throw new ArgumentException("Slice index exceeded on " + where);
                }

                String constraint = "";
                if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.ISE)
                {
                    this.OutputManager.WriteUCFOutput("INST \"" + this.InstanceName + "\" LOC = \"" + where.Slices[this.SliceNumber] + "\"; # generated_by_GoAhead");
                }
                else if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
                {
                    // place_cell inst_PartialSubsystem/inst_3 SLICE_X6Y66/C6LUT
                    this.OutputManager.WriteUCFOutput("place_cell " + this.InstanceName + " " + where.Slices[this.SliceNumber] + "/" + this.BEL + "; # generated_by_GoAhead");
                    // LOCK_PINS can only be applied to LUTs, for UltraScale we used FlipFops as well 
                    if (this.BEL.EndsWith("LUT"))
                    {
                        this.OutputManager.WriteUCFOutput("set_property LOCK_PINS {I0:A1 I1:A2 I2:A3 I3:A4 I4:A5 I5:A6} [get_cells " + this.InstanceName + "]; # generated_by_GoAhead");
                    }
                    this.OutputManager.WriteUCFOutput("set_property DONT_TOUCH TRUE [get_cells " + this.InstanceName + "]; # generated_by_GoAhead");
                    /*
                    // set_property LOC SLICE_X33Y15 [get_cells My_LUT6_inst]
                    this.OutputManager.WriteUCFOutput("set_property LOC " + where.Slices[this.SliceNumber] + " [get_cells " + this.InstanceName + "]; # generated_by_GoAhead");
                    // set_property LOCK_PINS {I0:A1 I1:A2 I2:A3 I3:A4} [get_cells inst_PartialSubsystem/inst_0]
                    this.OutputManager.WriteUCFOutput("set_property LOCK_PINS {I0:A1 I1:A2 I2:A3 I3:A4 I4:A5 I5:A6} [get_cells " + this.InstanceName + "]; # generated_by_GoAhead");
                     * */
                }

                // promt ucf to text box
                this.OutputManager.WriteUCFOutput(constraint);
            }
        }

        public override void Undo()
        {
        }
      
        [Parameter(Comment = "The location of the file where toplace the ucf in")]
        public String Location = "";

        [Parameter(Comment = "The index of the slice the blocker will use")]
        public int SliceNumber = 0;

        [Parameter(Comment = "The instance name")]
        public String InstanceName= "";

        [Parameter(Comment = "The slice element name (Vivado only)")]
        public String BEL = "";
    }
}
