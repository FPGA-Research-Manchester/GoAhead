using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.Selection
{
    [Serializable]
    [CommandDescription(Description = "Add tiles in the rectangle speciifed by the two slices to the current selection", Wrapper = true)]
    public class AddToSelectionBySlice : AddToSelectionCommand
    {
        protected override void DoCommandAction()
        {
            if(!Range.Contains(":"))
            {
                throw new ArgumentException("Expecting range in foramt SLICE_X8Y55:SLICE_X12Y243, but found " + Range);
            }
            string[] atoms = Range.Split(':');
            Slice s1 = FPGA.FPGA.Instance.GetSlice(atoms[0]);
            Slice s2 = FPGA.FPGA.Instance.GetSlice(atoms[1]);
            if (s1 == null)
            {
                throw new ArgumentException("Could not find Slice1 " + atoms[0]);
            }
            if (s2 == null)
            {
                throw new ArgumentException("Could not find Slice2 " + atoms[1]);
            }

            Tile t1 = s1.ContainingTile;
            Tile t2 = s2.ContainingTile;

            // the sorting (UpperLeft LowerRight) is carried out by AddToSelectionXY
            AddToSelectionXY addCmd = new AddToSelectionXY();
            addCmd.UpperLeftX = t1.TileKey.X;
            addCmd.UpperLeftY = t1.TileKey.Y;
            addCmd.LowerRightX = t2.TileKey.X;
            addCmd.LowerRightY = t2.TileKey.Y;

            CommandExecuter.Instance.Execute(addCmd);
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Only selected those tiles in the given range that match this filter")]
        public string Filter = ".*";

        [Parameter(Comment = "The rectangle, e.g. SLICE_X8Y55:SLICE_X12Y243")]
        public string Range = "SLICE_X8Y55:SLICE_X12Y243";
    }
}
