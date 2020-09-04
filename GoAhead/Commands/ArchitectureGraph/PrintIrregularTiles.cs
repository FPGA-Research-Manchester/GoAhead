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
            OutputManager.WriteOutput("Tile Hashcode,Tile name,wirelist hashcode,incoming wirelist hashcode");

            StringBuilder buffer = new StringBuilder();
            foreach (int irregularTileHashcode in IrregularTiles)
            {
                Tile irregularTile = TileHashcodes[irregularTileHashcode];
                int irregularTileWirelistHashcode = WirelistHashcodes[irregularTileHashcode];
                int irregularTileIncomingWirelistHashcode = IncomingWirelistHashcodes[irregularTileHashcode];

                buffer.AppendLine(irregularTileHashcode + "," + irregularTile.Location + "," + irregularTileWirelistHashcode + "," + irregularTileIncomingWirelistHashcode);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Uncommon Tiles")]
        public List<int> IrregularTiles = new List<int>();

        [Parameter(Comment = "All tile hashcodes")]
        public Dictionary<int, Tile> TileHashcodes = new Dictionary<int, Tile>();

        [Parameter(Comment = "Wirelist Hashcodes")]
        public Dictionary<int, int> WirelistHashcodes = new Dictionary<int, int>();

        [Parameter(Comment = "Incoming Wirelist Hashcodes")]
        public Dictionary<int, int> IncomingWirelistHashcodes = new Dictionary<int, int>();

    }
}
