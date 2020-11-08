using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintAllInterconnectBlocks : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // header line
            OutputManager.WriteOutput("Tile name,(wirelist hashcodes),(incoming wirelist hashcodes),switchmatrix hashcode");

            StringBuilder buffer = new StringBuilder();
            foreach (KeyValuePair<int, List<int>> kvp in InterconnectToPrimtivesMappings)
            {
                int interconnectTileHashcode = kvp.Key;
                List<int> primitiveHashcodes = kvp.Value;

                Tile intTile = TileHashcodes[interconnectTileHashcode];
                string intTileWirelistHashcode = WirelistHashcodes[interconnectTileHashcode].ToString();
                string intTileIncomingWirelistHashcode = IncomingWirelistHashcodes[interconnectTileHashcode].ToString();

                string leftPrimitiveWirelistHashcode= "", rightPrimitiveWirelistHashcode = "", leftPrimitiveIncomingWirelistHashcode = "", rightPrimitiveIncomingWirelistHashcode = "";

                // empty primitive flagged using -999
                if (primitiveHashcodes[0] != -999)
                    
                {
                    leftPrimitiveWirelistHashcode = WirelistHashcodes[primitiveHashcodes[0]].ToString();
                    leftPrimitiveIncomingWirelistHashcode = IncomingWirelistHashcodes[primitiveHashcodes[0]].ToString();
                }
                if (primitiveHashcodes[1] != -999)
                {
                    rightPrimitiveWirelistHashcode = WirelistHashcodes[primitiveHashcodes[1]].ToString();
                    rightPrimitiveIncomingWirelistHashcode = IncomingWirelistHashcodes[primitiveHashcodes[1]].ToString();
                }
                

                buffer.AppendLine(intTile.Location + ",(" + leftPrimitiveWirelistHashcode + ";" + intTileWirelistHashcode + ";" + rightPrimitiveWirelistHashcode + ")," + 
                    "(" + leftPrimitiveIncomingWirelistHashcode + ";" + intTileIncomingWirelistHashcode + ";" + rightPrimitiveIncomingWirelistHashcode + ")," + 
                    intTile.SwitchMatrixHashCode);
            }

            OutputManager.WriteOutput(buffer.ToString());
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "All tile hashcodes")]
        public Dictionary<int, Tile> TileHashcodes = new Dictionary<int, Tile>();

        [Parameter(Comment = "Interconnect To Primitives mappings")]
        public Dictionary<int, List<int>> InterconnectToPrimtivesMappings = new Dictionary<int, List<int>>();

        [Parameter(Comment = "Wirelist Hashcodes")]
        public Dictionary<int, int> WirelistHashcodes = new Dictionary<int, int>();

        [Parameter(Comment = "Incoming Wirelist Hashcodes")]
        public Dictionary<int, int> IncomingWirelistHashcodes = new Dictionary<int, int>();

    }
}
