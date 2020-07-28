using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllInterconnectTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // header line
            OutputManager.WriteOutput("Tile hashcode,Tile name,primitives,wirelist hashcode,switchmatrix hashcode\n");

            StringBuilder buffer = new StringBuilder();
            foreach(KeyValuePair<Tile, int> pair in InterconnectTiles)
            {
                Tile intTile = pair.Key;
                int intTileHashcode = pair.Value;
                int intTileWirelistHashcode = InterconnectToWirelistHashcode[intTileHashcode];
                List<int> primitiveTiles = InterconnectToPrimitiveTiles[intTileHashcode];

                buffer.AppendLine(intTileHashcode + "," + intTile.Location + ",(" + string.Join(",", primitiveTiles) + ")," + intTileWirelistHashcode + "," + intTile.SwitchMatrixHashCode);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Interconnect Tiles")]
        public Dictionary<Tile, int> InterconnectTiles = new Dictionary<Tile, int>();

        [Parameter(Comment = "Interconnect to primitve mappings")]
        public Dictionary<int, List<int>> InterconnectToPrimitiveTiles = new Dictionary<int, List<int>>();

        [Parameter(Comment = "Interconnect Tiles Wirelists")]
        public Dictionary<int, int> InterconnectToWirelistHashcode = new Dictionary<int, int>();

    }
}
