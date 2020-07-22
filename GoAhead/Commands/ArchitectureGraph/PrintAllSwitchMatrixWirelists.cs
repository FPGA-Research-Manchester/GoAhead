using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintAllSwitchMatrixWirelists : CommandWithFileOutput
    {
        protected override void DoCommandAction()
        {
            OutputManager.WriteOutput("Local Pip, Target Pip,LocalPipIsDriver,RelativeX,RelativeY,Wirelist Hashcode\n");
            //foreach (Tile intTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            //{
            //    if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect) && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.SubInterconnect))
            //        continue;

            //    List<Tile> clbTiles = PrintAllTiles.GetCLBTilesForInterconnect(intTile);

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

                
            //    foreach (Wire w in intTile.WireList.OrderBy(wire => wire.LocalPip))
            //    {
            //        OutputManager.WriteOutput(string.Join(",", w.LocalPip, w.PipOnOtherTile, (w.LocalPipIsDriver ? "1" : "0"), w.XIncr, w.YIncr, intTile.WireListHashCode));
            //    }

            //    foreach(Tile t in clbTiles)
            //    {
            //        foreach(Wire w in t.WireList.OrderBy(wire => wire.LocalPip))
            //        {
            //            if(w.PipOnOtherTile.StartsWith("LOGIC_OUTS") && (Navigator.GetDestinationByWire(t, w).Location == intTile.Location))
            //                OutputManager.WriteOutput(string.Join(",", w.LocalPip, w.PipOnOtherTile, "0", w.XIncr, w.YIncr, intTile.WireListHashCode));
            //        }
            //    }

            foreach (WireList wl in InterconnectWirelists.Values)
            {
                foreach (Wire w in wl)
                    OutputManager.WriteOutput(string.Join(",", w.LocalPip, w.PipOnOtherTile, w.LocalPipIsDriver, w.XIncr, w.YIncr, wl.Key));

                OutputManager.WriteOutput("\n");

            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "List of Interconnect Wirelists")]
        public Dictionary<int, WireList> InterconnectWirelists = null;
    }
}
