using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using GoAhead.Objects;
using GoAhead.FPGA;

namespace GoAhead.Commands.Misc
{
    class SetEdgeCosts
    {
        [CommandDescription(Description = "Sets the cost for a wire using the tiles and ports it goes to and from.")]
        class SetPortCosts : Command
        {
            [Parameter(Comment = "The start tile to set the port cost for")]
            public string StartTile = "";

            [Parameter(Comment = "The start port to set the cost for")]
            public string StartPort = "";

            [Parameter(Comment = "The end tile to set the port cost for")]
            public string EndTile = "";

            [Parameter(Comment = "The end port to set the cost for")]
            public string EndPort = "";

            [Parameter(Comment = "The latency value")]
            public double NewCost = 0;

            protected override void DoCommandAction()
            {
                int wiresUpdated = 0;
                List<Tile> tileList = FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, StartTile)).OrderBy(t => t.Location).ToList();
                foreach (Tile t in tileList)
                {
                    WireList wl = FPGA.FPGA.Instance.GetWireList(t.WireListHashCode);
                    foreach (Wire w in wl)
                    {
                        if (w.LocalPip.Equals(StartPort) && w.PipOnOtherTile.Equals(EndPort) && t.GetTileAtWireEnd(w).TileKey.ToString().Equals(EndTile))
                        {
                            w.Cost = NewCost;
                            wiresUpdated++;
                        }
                    }
                }
                if (wiresUpdated == 0)
                    Console.WriteLine("No wire was found from start location to target location.");
                else
                    Console.WriteLine($"{wiresUpdated} wires were updated with the new cost.");
            }
            public override void Undo()
            {
                throw new ArgumentException("The method or operation is not implemented.");
            }
        }
    }
}