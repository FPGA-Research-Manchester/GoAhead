using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintAllTiles : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            // header line
            OutputManager.WriteOutput("Tile name,primitive 1,primitive 2,wirelist hashcode,switchmatrix hashcode\n");

            //foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            //{
            //    // filter out interconnects
            //    if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect) && IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.SubInterconnect))
            //        continue;

            //    List<Tile> clbTiles = PrintArchitectureGraph.GetCLBTilesForInterconnect(t);

            //    if (clbTiles.Count < 1)
            //    { 
            //        // OutputManager.WriteOutput("Warning: No CLB tiles found for tile " + t.Location + ". Skipping."); 
            //        continue; 
            //    }

            //    if (clbTiles.Count > 2)
            //    { 
            //        //OutputManager.WriteOutput("Warning: more than 2 CLB tiles found for tile " + t.Location + ". Skipping."); 
            //        continue; 
            //    }

            //    if (clbTiles.Count == 1)
            //        clbTiles.Add(null);

            //    OutputManager.WriteOutput(string.Join(",", t.Location, clbTiles[0]?.Location ?? "", clbTiles[1]?.Location ?? "", t.WireListHashCode, t.SwitchMatrixHashCode));
            //}

            foreach(KeyValuePair<Tile, int> interconnectTileWithHashcode in WirelistHashcodeForInterconnectTiles)
            {
                Tile t = interconnectTileWithHashcode.Key;
                int interconnectWirelistHashcode = interconnectTileWithHashcode.Value;

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

                OutputManager.WriteOutput(string.Join(",", t.Location, clbTiles[0]?.Location ?? "", clbTiles[1]?.Location ?? "", interconnectWirelistHashcode, t.SwitchMatrixHashCode));
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Interconnect Tiles with their interconnect wirelist hashcodes")]
        public Dictionary<Tile, int> WirelistHashcodeForInterconnectTiles = null;
        
        [Parameter(Comment = "List of Interconnect Tiles with their primitives")]
        public Dictionary<Tile, List<Tile>> CLBTilesForInterconnectTile = null;

    }
}
