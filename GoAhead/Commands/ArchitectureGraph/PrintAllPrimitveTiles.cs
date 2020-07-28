using GoAhead.FPGA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllPrimitveTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // header line
            OutputManager.WriteOutput("Tile Hashcode,Tile name,wirelist hashcode\n");

            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<Tile, int> pair in PrimitiveTiles)
            {
                Tile primitiveTile = pair.Key;
                int primitiveTileHashcode = pair.Value;
                int primitiveTileWirelistHashcode = PrimitiveToWirelistHashcode[primitiveTileHashcode];

                buffer.AppendLine(primitiveTileHashcode + "," + primitiveTile.Location + "," + primitiveTileWirelistHashcode);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Primitive Tiles")]
        public Dictionary<Tile, int> PrimitiveTiles = new Dictionary<Tile, int>();

        [Parameter(Comment = "Primitive Tiles Wirelists")]
        public Dictionary<int, int> PrimitiveToWirelistHashcode = new Dictionary<int, int>();
    }
}
