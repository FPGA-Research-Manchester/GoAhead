using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print all tiles with ports that are excluded from blocking", Wrapper = false, Publish = true)]
    class PrintTilesWithPortsExcludedFromBlocking : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => t.GetAllBlockedPorts(Tile.BlockReason.ExcludedFromBlocking).Any()))
            {
                OutputManager.WriteOutput("Tile " + tile.Location + " contains ports that are excluded from blocking");
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
