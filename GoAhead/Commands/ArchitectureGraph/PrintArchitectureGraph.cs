using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.DeviceInfo
{
    class PrintArchitectureGraph : CommandWithFileOutput
    {
        private Dictionary<int, WireList> interconnectWirelist = new Dictionary<int, WireList>();
        private Dictionary<Tile, int> tileToInterconnectWirelistMapping = new Dictionary<Tile, int>();
        private Dictionary<Tile, List<Tile>> clbTilesForInterconnectTile = new Dictionary<Tile, List<Tile>>();

        protected override void DoCommandAction()
        {
            foreach (Tile intTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect) && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    continue;

                WireList thisTileWL = new WireList();
                foreach (Wire w in intTile.WireList)
                {
                    Tile target = Navigator.GetDestinationByWire(intTile, w);
                    short xIncr = (short)(target.LocationX - intTile.LocationX);
                    short yIncr = (short)(target.LocationY - intTile.LocationY);


                    if (xIncr != 0 || yIncr != 0)
                        thisTileWL.Add(new Wire(w.LocalPipKey, w.PipOnOtherTileKey, w.LocalPipIsDriver, xIncr, yIncr));

                }

                StoreInterconnectWirelist(thisTileWL);
                tileToInterconnectWirelistMapping.Add(intTile, thisTileWL.Key);
                clbTilesForInterconnectTile.Add(intTile, GetCLBTilesForInterconnect(intTile));
            }

            PrintAllSwitchMatrixWirelists printWirelists = new PrintAllSwitchMatrixWirelists();
            printWirelists.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\wirelists";
            printWirelists.InterconnectWirelists = interconnectWirelist;
            CommandExecuter.Instance.Execute(printWirelists);


            PrintAllTiles printTiles = new PrintAllTiles();
            printTiles.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\tiles";
            printTiles.CLBTilesForInterconnectTile = clbTilesForInterconnectTile;
            printTiles.WirelistHashcodeForInterconnectTiles = tileToInterconnectWirelistMapping;
            CommandExecuter.Instance.Execute(printTiles);
        }

        public static List<Tile> GetCLBTilesForInterconnect(Tile t)
        {
            List<Tile> tilesToPrint = new List<Tile>();
            foreach (Tile clbTile in FPGATypes.GetCLTile(t).Where(c => c.LocationX == t.LocationX && c.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.CLB)))
                tilesToPrint.Add(clbTile);

            return tilesToPrint;
        }

        private void StoreInterconnectWirelist(WireList intWL)
        {
            bool equalWLFound = false;

            foreach (WireList other in interconnectWirelist.Values.Where(wl => wl.Count == intWL.Count))
            {
                if (other.Equals(intWL))
                {
                    intWL.Key = other.Key;
                    equalWLFound = true;
                    break;
                }
            }

            if (!equalWLFound)
            {
                intWL.Key = interconnectWirelist.Count;
            }


            // now share common wire list
            if (!interconnectWirelist.ContainsKey(intWL.Key))
            {
                interconnectWirelist.Add(intWL.Key, intWL);
            }
            else
            {
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }

}
