using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintIrregularTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Tile Hashcode,Tile name,wirelist hashcode");

            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<Tile, int> pair in IrregularTiles)
            {
                Tile uncommonTile = pair.Key;
                int uncommonTileHashcode = pair.Value;
                int irregularTileWirelistHashcode = IrregularTilesToWirelistHashcodes[uncommonTileHashcode];

                buffer.AppendLine(uncommonTileHashcode + "," + uncommonTile.Location + "," + irregularTileWirelistHashcode);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Uncommon Tiles")]
        public Dictionary<Tile, int> IrregularTiles = new Dictionary<Tile, int>();

        [Parameter(Comment = "Uncommon Tiles Wirelists")]
        public Dictionary<int, int> IrregularTilesToWirelistHashcodes = new Dictionary<int, int>();
    }
}
