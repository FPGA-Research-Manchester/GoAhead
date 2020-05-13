using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Commands.DeviceInfo
{
    [CommandDescription(Description="Print all slice type found on those tiles that match TileFilter", Wrapper=false)]
    class PrintAllSliceTypes : Command
    {
       protected override void DoCommandAction()
        {
            Dictionary<string, bool> sliceTypes = new Dictionary<string, bool>();

            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, TileFilter)))
            {
                foreach (Slice slice in tile.Slices)
                {
                    if (!sliceTypes.ContainsKey(slice.SliceType))
                    {
                        sliceTypes.Add(slice.SliceType, false);
                    }
                }
            }


            // print
            foreach(KeyValuePair<string, bool> tupel in sliceTypes)
            {
                OutputManager.WriteOutput(tupel.Key);
            }
        }

        public override void Undo()
        {
        }

        [Parameter(Comment = "Only consider tile that match this pattern. Leave the string empty to print all slice types")]
        public string TileFilter = "^CL";
    }
}
