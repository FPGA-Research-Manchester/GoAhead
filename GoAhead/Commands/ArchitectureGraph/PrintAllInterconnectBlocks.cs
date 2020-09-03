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
            foreach(intBlock block in InterconnectBlocks)
            {
                Tile intTile = TileHashcodes[block.intTile];
                string intTileWirelistHashcode = WirelistHashcodes[block.intTile].ToString();
                string intTileIncomingWirelistHashcode = IncomingWirelistHashcodes[block.intTile].ToString();

                string leftPrimitiveWirelistHashcode= "", rightPrimitiveWirelistHashcode = "", leftPrimitiveIncomingWirelistHashcode = "", rightPrimitiveIncomingWirelistHashcode = "";

                // empty primitive flagged using -999
                if (block.leftPrimitive != -999)
                    
                {
                    leftPrimitiveWirelistHashcode = WirelistHashcodes[block.leftPrimitive].ToString();
                    leftPrimitiveIncomingWirelistHashcode = IncomingWirelistHashcodes[block.leftPrimitive].ToString();
                }
                if (block.rightPrimitive != -999)
                {
                    rightPrimitiveWirelistHashcode = WirelistHashcodes[block.rightPrimitive].ToString();
                    rightPrimitiveIncomingWirelistHashcode = IncomingWirelistHashcodes[block.rightPrimitive].ToString();
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

        [Parameter(Comment = "Interconnect Tiles Wirelists")]
        public List<intBlock> InterconnectBlocks = new List<intBlock>();

        [Parameter(Comment = "Wirelist Hashcodes")]
        public Dictionary<int, int> WirelistHashcodes = new Dictionary<int, int>();

        [Parameter(Comment = "Incoming Wirelist Hashcodes")]
        public Dictionary<int, int> IncomingWirelistHashcodes = new Dictionary<int, int>();

    }
}
