using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // header line
            OutputManager.WriteOutput("Tile name,primitive 1,primitive 2,wirelist hashcode,primitive connections wirelist hashcode,switchmatrix hashcode\n");

            foreach(KeyValuePair<Tile, int> interconnectTileWithHashcode in WirelistHashcodeForInterconnectTiles)
            {
                Tile t = interconnectTileWithHashcode.Key;
                int interconnectWirelistHashcode = interconnectTileWithHashcode.Value;
                int primitiveConnectionsWirelistHashcode = PrimitiveConnectionsWirelistHashcodeForInterconnectTile[t];

                List<Tile> clbTiles = CLBTilesForInterconnectTile[t];
                if (clbTiles.Count < 1)
                { 
                    // OutputManager.WriteOutput("Warning: No CLB tiles found for tile " + t.Location + ". Skipping."); 
                    continue; 
                }

                if (clbTiles.Count > 2)
                {
                    //OutputManager.WriteOutput("Warning: more than 2 CLB tiles found for tile " + t.Location + ". Skipping."); 
                    continue;
                }

                if (clbTiles.Count == 1)
                    clbTiles.Add(null);

                OutputManager.WriteOutput(string.Join(",", t.Location, clbTiles[0]?.Location ?? "", clbTiles[1]?.Location ?? "", interconnectWirelistHashcode, primitiveConnectionsWirelistHashcode, t.SwitchMatrixHashCode));
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Interconnect Tiles with their interconnect wirelist hashcodes")]
        public Dictionary<Tile, int> WirelistHashcodeForInterconnectTiles = null;

        [Parameter(Comment = "List of Interconnect Tiles with their primitive connections wirelist hashcodes")]
        public Dictionary<Tile, int> PrimitiveConnectionsWirelistHashcodeForInterconnectTile = null;

        [Parameter(Comment = "List of Interconnect Tiles with their primitives")]
        public Dictionary<Tile, List<Tile>> CLBTilesForInterconnectTile = null;

    }
}
