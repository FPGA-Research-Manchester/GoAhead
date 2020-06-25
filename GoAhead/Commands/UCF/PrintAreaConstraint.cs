using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.UCF
{
    [CommandDescription(Description="Print an area constraint for the current selection")]
    class PrintAreaConstraint : UCFCommand
    {
        protected override void DoCommandAction()
        {
            FPGATypes.AssertBackendType(FPGATypes.BackendType.ISE, FPGATypes.BackendType.Vivado);
            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    PrintAreaGroupForISE();
                    break;
                case FPGATypes.BackendType.Vivado:
                    PrintAreaGroupForVivado();
                    break;
                default:
                    break;
            }
        }

        private void PrintAreaGroupForVivado()
        {
            //create_pblock ag1;
            //resize_pblock [get_pblocks ag1] -add {SLICE_X0Y44:SLICE_X27Y20

            string pBlockName = Regex.Replace(InstanceName, @"\*", "");
            pBlockName = "pb_" + pBlockName;
            OutputManager.WriteTCLOutput("create_pblock " + pBlockName + "; # generated_by_GoAhead");

            //this.PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.CLB, new int[] { 0, 1 }, pBlockName);
            //this.PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.BRAM, new int[] { 0, 0, 1, 1 }, pBlockName);
            //this.PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.DSP, new int[] { 0, 0 }, pBlockName);
            PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.CLB, new int[] { 0, 0 }, pBlockName);
            PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.BRAM, new int[] { 0, 0, 1, 1 }, pBlockName);
            PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.DSP, new int[] { 0, 0, 1, 1 }, pBlockName);

            OutputManager.WriteTCLOutput("add_cells_to_pblock [get_pblocks " + pBlockName + "] [get_cells " + InstanceName + "]; # generated_by_GoAhead");

        }
        private void PrintAreaGroupForISE()
        {
            //INST “*reconfig_blue*” AREA_GROUP = "pblock_reconfig_blue";
            //AREA_GROUP "pblock_reconfig_blue" RANGE = SLICE_X28Y64:SLICE_X33Y67;
            string groupName = Regex.Replace(InstanceName, @"\*", "");
            groupName = "AG_" + groupName;

            string firstLine = "INST \"" + InstanceName + "\" AREA_GROUP = \"" + groupName + "\"; # generated_by_GoAhead";
            OutputManager.WriteUCFOutput(firstLine);

            PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.CLB, new int[] { 0, 1 }, groupName);
            PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.BRAM, new int[] { 0, 0, 1, 1 }, groupName);
            PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes.DSP, new int[] { 0, 0 }, groupName);

            OutputManager.WriteUCFOutput("AREA_GROUP \"" + groupName + "\" GROUP=CLOSED; # generated_by_GoAhead");
            OutputManager.WriteUCFOutput("AREA_GROUP \"" + groupName + "\" PLACE=CLOSED; # generated_by_GoAhead");
        }

        private void PrintAreaConstraintForResourceType(IdentifierManager.RegexTypes filterType, int[] sliceIndeces, string groupName)
        {
            int incr1 = 0;
            int incr2 = 1;    // why = 1?
            //int incr2 = 0;

            bool success = Print(incr1, incr2, FPGATypes.Placement.LowerLeft, FPGATypes.Placement.UpperRight, filterType, sliceIndeces, groupName);

            if (!success)
            {
                incr1 = 1;
                incr2 = 0;
                success = Print(incr1, incr2, FPGATypes.Placement.LowerLeft, FPGATypes.Placement.UpperRight, filterType, sliceIndeces, groupName);

                if (!success)
                {
                    //String filter = IdentifierManager.Instance.GetRegex(filterType);
                    //this.OutputManager.WriteUCFOutput("# could not find (LowerLeft and UpperRight) nor (UpperLeft nor LowerRight) for tile type " + filter + " in current selection. # generated_by_GoAhead");
                }
            }
        }

        private bool Print(int incr1, int incr2, FPGATypes.Placement placement1, FPGATypes.Placement placement2, IdentifierManager.RegexTypes filterType, int[] sliceIndeces, string groupName)
        {
            string filter = IdentifierManager.Instance.GetRegex(filterType);
            Tile tile1 = TileSelectionManager.Instance.GetSelectedTile(filter, placement1);
            Tile tile2 = TileSelectionManager.Instance.GetSelectedTile(filter, placement2);

            if (tile1 == null || tile2 == null)
            {
                return false;
            }
            if (tile1.Slices.Count == 0 || tile2.Slices.Count == 0)
            {
                return false;
            }

            switch (FPGA.FPGA.Instance.BackendType)
            {
                case FPGATypes.BackendType.ISE:
                    // Virtex 6 hard coded
                    if (FPGA.FPGA.Instance.Family == FPGATypes.FPGAFamily.Virtex6 && filterType == IdentifierManager.RegexTypes.BRAM)
                    {
                        if (tile1.Slices.Count != 3 || tile2.Slices.Count != 3)
                        {
                            throw new ArgumentException("Unexpected number of slices in Virtex6 RAM tile");
                        }
                        string lowerLeftSlice = tile1.Slices[0].ToString();
                        string upperRightSlice = tile2.Slices[1].ToString();
                        string firstLine = "AREA_GROUP \"" + groupName + "\"" + " RANGE = " + lowerLeftSlice + ":" + upperRightSlice + "; # generated_by_GoAhead";
                        OutputManager.WriteUCFOutput(firstLine);

                        string lowerLeftRAMB36Slice = tile1.Slices[2].ToString();
                        string upperRightRAMB36Slice = tile2.Slices[2].ToString();
                        string secondLine = "AREA_GROUP \"" + groupName + "\"" + " RANGE = " + lowerLeftRAMB36Slice + ":" + upperRightRAMB36Slice + "; # generated_by_GoAhead";
                        OutputManager.WriteUCFOutput(secondLine);
                    }
                    else if (FPGA.FPGA.Instance.Family == FPGATypes.FPGAFamily.Virtex6 && filterType == IdentifierManager.RegexTypes.DSP)
                    {
                        if (tile1.Slices.Count != 3 || tile2.Slices.Count != 3)
                        {
                            throw new ArgumentException("Unexpected number of slices in Virtex6 DSP tile");
                        }
                        string lowerLeftSlice = tile1.Slices[0].ToString();
                        string upperRightSlice = tile2.Slices[1].ToString();
                        string firstLine = "AREA_GROUP \"" + groupName + "\"" + " RANGE = " + lowerLeftSlice + ":" + upperRightSlice + "; # generated_by_GoAhead";
                        OutputManager.WriteUCFOutput(firstLine);
                    }
                    else
                    {
                        // other devices "genericly"
                        for (int i = 0; i < sliceIndeces.Length; i += 2)
                        {
                            string lowerLeftSlice = tile1.Slices[sliceIndeces[i + incr1]].ToString();
                            string upperRightSlice = tile2.Slices[sliceIndeces[i + incr2]].ToString();

                            string secondLine = "AREA_GROUP \"" + groupName + "\"" + " RANGE = " + lowerLeftSlice + ":" + upperRightSlice + "; # generated_by_GoAhead";

                            OutputManager.WriteUCFOutput(secondLine);
                        }
                    }
                    break;
                case FPGATypes.BackendType.Vivado:
                    for (int i = 0; i < sliceIndeces.Length; i += 2) // why < Length?
                    {
                        string lowerLeftSlice = tile1.Slices[sliceIndeces[i + incr1]].ToString();
                        string upperRightSlice = tile2.Slices[sliceIndeces[i + incr2]].ToString();

                        string secondLine = "resize_pblock [get_pblocks " + groupName + "] -add {" + lowerLeftSlice + ":" + upperRightSlice + "}; # generated_by_GoAhead";
                        OutputManager.WriteTCLOutput(secondLine);
                    }
                    break;
            }
            
            return true;
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "The name of the instance. You may add '*' as a suffix or prefix. The instance name will become the group name of the AREA constraint, however the '*' will be removed")]
        public string InstanceName = "*inst_mod*";
    }
}
