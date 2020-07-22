using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintCarryPipsInAllCLBs : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            foreach(Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
            {
                foreach (LUTRoutingInfo info in FPGATypes.GetLUTRouting(tile))
                {
                    if ((info.Port1 != null && info.Port1.ToString().StartsWith("CARRY")) || (info.Port2 != null && info.Port2.ToString().StartsWith("CARRY")) || (info.Port3 != null && info.Port3.ToString().StartsWith("CARRY")) || (info.Port4 != null && info.Port4.ToString().StartsWith("CARRY")))
                        OutputManager.WriteOutput(tile.TileKey + "," + tile.Location + "," + info.Port1 + "," + info.Port2 + "," + info.Port3 + "," + info.Port4 + "\n");
                }
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

    }
}
