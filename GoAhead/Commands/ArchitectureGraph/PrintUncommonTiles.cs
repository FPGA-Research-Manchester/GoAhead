using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintUncommonTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Tile Hashcode,Tile name,wirelist hashcode\n");

            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<Tile, int> pair in UncommonTiles)
            {
                Tile uncommonTile = pair.Key;
                int uncommonTileHashcode = pair.Value;
                int uncommonTileWirelistHashcode = UncommonToWirelistHashcode[uncommonTileHashcode];

                buffer.AppendLine(uncommonTileHashcode + "," + uncommonTile.Location + "," + uncommonTileWirelistHashcode);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Uncommon Tiles")]
        public Dictionary<Tile, int> UncommonTiles = new Dictionary<Tile, int>();

        [Parameter(Comment = "Uncommon Tiles Wirelists")]
        public Dictionary<int, int> UncommonToWirelistHashcode = new Dictionary<int, int>();
    }
}
