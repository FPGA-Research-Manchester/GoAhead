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
            Regex BELRegexp = new Regex(this.BELs, RegexOptions.Compiled);
            foreach (Tile tile in FPGA.TileSelectionManager.Instance.GetSelectedTiles())
            {
                foreach (Slice slice in tile.Slices)
                {
                    foreach (string bel in slice.Bels.Where(b => BELRegexp.IsMatch(b)))
                    {
                        string instName = slice.SliceName + "_" + bel;
                        this.OutputManager.WriteOutput("create_cell -reference FDRE	" + instName);
                        this.OutputManager.WriteOutput("place_cell " + instName + " " + slice.SliceName + "/" + bel);
                        this.OutputManager.WriteOutput("create_pin -direction IN " + instName + "/" + this.ClockPin);
                        this.OutputManager.WriteOutput("connect_net -hier -net " + this.ClockNetName + " -objects {" + instName + "/" + this.ClockPin + "}");
                    }
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The name of the clock pin to connect")]
        public String ClockPin = "CK";

        [Parameter(Comment = "The slice bels")]
        public String BELs = "[A-D]FF";

        [Parameter(Comment = "The name of the clock net that the clock pins will be added to")]
        public String ClockNetName = "clk";
    }
}
