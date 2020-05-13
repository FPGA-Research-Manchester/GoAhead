using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Commands;

namespace GoAhead.Commands.Debug
{
    [CommandDescription(Description = "Print the identifiers of all currently selected tiles with blocked ports", Wrapper = false, Publish = true)]
    class PrintSelectedTilesWithBlockedPorts : Command
    {
        [Parameter(Comment = "Should GoAhead also consider the stopover tag? Default=false")]
        public bool IncludeStopoverTag = false;

        protected override void DoCommandAction()
        {
            var tiles = TileSelectionManager.Instance.GetSelectedTiles();

            if(IncludeStopoverTag)
            {
                foreach(Tile tile in tiles.Where(t=>t.HasBlockedPorts))
                {
                    OutputManager.WriteOutput(tile.Location);
                }
            }
            else
            {
                foreach (Tile tile in tiles.Where(t => t.HasNonstopoverBlockedPorts))
                {
                    OutputManager.WriteOutput(tile.Location);
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
