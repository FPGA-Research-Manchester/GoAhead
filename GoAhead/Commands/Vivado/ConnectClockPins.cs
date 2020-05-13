using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands.NetlistContainerGeneration;
using GoAhead.Objects;
using GoAhead.Code;
using GoAhead.FPGA;

namespace GoAhead.Commands.Vivado
{
    [CommandDescription(Description = "Generate TCL code to connect all clock pins of all currently selected tiles to the given clock net", Wrapper = false, Publish = true)]
    class ConnectClockPins : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            Regex BELRegexp = new Regex(BELs, RegexOptions.Compiled);
            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles())
            {
                foreach (Slice slice in tile.Slices)
                {
                    foreach (string bel in slice.Bels.Where(b => BELRegexp.IsMatch(b)))
                    {
                        string instName = slice.SliceName + "_" + bel;
                        OutputManager.WriteOutput("create_cell -reference FDRE	" + instName);
                        OutputManager.WriteOutput("place_cell " + instName + " " + slice.SliceName + "/" + bel);
                        OutputManager.WriteOutput("create_pin -direction IN " + instName + "/" + ClockPin);
                        OutputManager.WriteOutput("connect_net -hier -net " + ClockNetName + " -objects {" + instName + "/" + ClockPin + "}");
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the clock pin to connect")]
        public string ClockPin = "CK";

        [Parameter(Comment = "The slice bels")]
        public string BELs = "[A-D]FF";

        [Parameter(Comment = "The name of the clock net that the clock pins will be added to")]
        public string ClockNetName = "clk";
    }
}
