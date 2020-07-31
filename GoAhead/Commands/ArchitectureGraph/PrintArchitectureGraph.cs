using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintArchitectureGraph : CommandWithFileOutput
    {
        private Dictionary<Tile, int> interconnectTiles = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> primitiveTiles = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> uncommonTiles = new Dictionary<Tile, int>();

        private Dictionary<int, List<int>> interconnectToPrimitiveTiles = new Dictionary<int, List<int>>();
        //private Dictionary<int, Tile> primtiveToInterconnectTiles = new Dictionary<int, Tile>();

        private Dictionary<int, int> interconnectToWirelistHashcode = new Dictionary<int, int>();
        private Dictionary<int, int> primitiveToWirelistHashcode = new Dictionary<int, int>();
        private Dictionary<int, int> uncommonTilesToWirelistHashcode = new Dictionary<int, int>();

        private Dictionary<int, WireList> wirelistHashcodeToWirelist = new Dictionary<int, WireList>();

        private const int uncommonTilesHashcodeShift = 10;
        private static string RESOURCE_STRING = "VsL MsL RsL MsD MsL LsL MsL LsL RsL MsmP msL MsL RsL MsD MsL LsL MsL LsL MsD MsL MsL RsL MsL RsL MsmP msL MsD MsL MsD MsL MsL RsL MsmFmI";
        bool checkForSubinterconnects = RESOURCE_STRING.Contains("m");
        string[] resourceStringChopped = RESOURCE_STRING.Split(' ');

        Regex hdio = new Regex("^HDIO_X");
        Regex cm_final_identifier = new Regex("^CM^");

        protected override void DoCommandAction()
        {
            RAMSelectionManager.Instance.UpdateMapping();

            foreach (Tile intTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    continue;

                interconnectTiles.Add(intTile, interconnectTiles.Count());
                interconnectToPrimitiveTiles.Add(interconnectTiles[intTile], new List<int>());
                Tile subInterconnect = null;

                Tile leftPrimitive = GetPrimitive(intTile, checkForSubinterconnects, left:true);
                Tile rightPrimitive = GetPrimitive(intTile, checkForSubinterconnects, left: false);

                if (leftPrimitive != null)
                {
                    if(!primitiveTiles.Keys.Contains(leftPrimitive))
                        primitiveTiles.Add(leftPrimitive, primitiveTiles.Count());
                    
                    interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(primitiveTiles[leftPrimitive]);
                }
                else
                    interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(-999);
                if (rightPrimitive != null)
                {
                    if (!primitiveTiles.Keys.Contains(rightPrimitive))
                        primitiveTiles.Add(rightPrimitive, primitiveTiles.Count());

                    interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(primitiveTiles[rightPrimitive]);
                }
                else
                    interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(-999);


                if (interconnectToPrimitiveTiles[interconnectTiles[intTile]].Count() > 2)
                {
                    OutputManager.WriteOutput("OOOOH ERROR BOI");
                }
            }

            foreach(Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => !interconnectTiles.ContainsKey(t) && !primitiveTiles.ContainsKey(t))) 
            {
                if(checkForSubinterconnects && !IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    if(tile.WireList.Count() > 0)
                        uncommonTiles.Add(tile, uncommonTiles.Count() + uncommonTilesHashcodeShift);
                }
            }

            OutputManager.WriteOutput("Primitives found.");

            foreach (KeyValuePair<Tile, int> pair in interconnectTiles)
            {
                Tile intTile = pair.Key;
                int intTileHashcode = pair.Value;
                WireList interconnectTileWireList = new WireList();
                processWirelistForTile(checkForSubinterconnects, intTile, out interconnectTileWireList);

                StoreInterconnectWirelist(interconnectTileWireList, wirelistHashcodeToWirelist);
                interconnectToWirelistHashcode.Add(intTileHashcode, interconnectTileWireList.Key);
            }
            OutputManager.WriteOutput("interconnect wirelists made.");

            foreach (KeyValuePair<Tile, int> pair in primitiveTiles)
            {
                Tile primitiveTile = pair.Key;
                int primtiveTileHashcode = pair.Value;
                WireList primitiveTileWireList = new WireList();
                processWirelistForTile(checkForSubinterconnects, primitiveTile, out primitiveTileWireList);

                StoreInterconnectWirelist(primitiveTileWireList, wirelistHashcodeToWirelist);
                primitiveToWirelistHashcode.Add(primitiveTiles[primitiveTile], primitiveTileWireList.Key);
            }

            foreach (KeyValuePair<Tile, int> pair in uncommonTiles.Reverse())
            {
                Tile uncommonTile = pair.Key;
                int uncommonTileHashcode = pair.Value;
                WireList uncommonTileWireList = new WireList();
                processWirelistForTile(checkForSubinterconnects, uncommonTile, out uncommonTileWireList);

                if (uncommonTileWireList.Count() > 0)
                {
                    StoreInterconnectWirelist(uncommonTileWireList, wirelistHashcodeToWirelist);
                    uncommonTilesToWirelistHashcode.Add(uncommonTiles[uncommonTile], uncommonTileWireList.Key);
                }
                else
                {
                    uncommonTilesToWirelistHashcode.Add(uncommonTiles[uncommonTile], -1);
                }
            }

            foreach(Tile uncommonTile in uncommonTiles.Keys)
            {
                if (!uncommonTilesToWirelistHashcode.ContainsKey(uncommonTiles[uncommonTile]))
                    uncommonTilesToWirelistHashcode.Add(uncommonTiles[uncommonTile], -1);
            }

            OutputManager.WriteOutput("Primitive wirelists made.");

            PrintAllSwitchMatrixWirelists printWirelists = new PrintAllSwitchMatrixWirelists();
            printWirelists.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\AG\\wirelists";
            printWirelists.InterconnectWirelists = wirelistHashcodeToWirelist;
            CommandExecuter.Instance.Execute(printWirelists);


            PrintAllInterconnectTiles printTiles = new PrintAllInterconnectTiles();
            printTiles.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\AG\\tiles";
            printTiles.InterconnectTiles = interconnectTiles;
            printTiles.InterconnectToPrimitiveTiles = interconnectToPrimitiveTiles;
            printTiles.InterconnectToWirelistHashcode = interconnectToWirelistHashcode;
            printTiles.PrimitiveToWirelistHashcode = primitiveToWirelistHashcode;
            CommandExecuter.Instance.Execute(printTiles);


            PrintUncommonTiles printUncommonTiles = new PrintUncommonTiles();
            printUncommonTiles.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\AG\\uncommonTiles";
            printUncommonTiles.UncommonTiles = uncommonTiles;
            printUncommonTiles.UncommonToWirelistHashcode = uncommonTilesToWirelistHashcode;
            CommandExecuter.Instance.Execute(printUncommonTiles);
        }

        private Tile GetPrimitive(Tile intTile, bool checkForSubinterconnects, bool left)
        {
            Tile tile = FPGA.FPGA.Instance.GetTile(intTile.TileKey.X + (left ? -1 : 1), intTile.TileKey.Y);

            if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                tile = FPGA.FPGA.Instance.GetTile(tile.TileKey.X + (left ? -1 : 1), tile.TileKey.Y);
            }

            if (getCoreRamTile(tile) != null)
            {
                tile = getCoreRamTile(tile);
            }
            
            if(IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB) || IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.BRAM) || IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.DSP) || hdio.IsMatch(tile.Location) || cm_final_identifier.IsMatch(tile.Location))
                return tile;

            return null;
        }

        private void AddWireToWirelist(Tile startTile, WireList wirelist, Wire startWire, short xIncr, short yIncr, Wire endWire, Tile endTile)
        {
            string blockString = resourceStringChopped[endTile.LocationX];
            int primitiveNumber = -999;

            int index = 0;
            foreach(char c in blockString)
            {
                switch(c)
                {
                    case 's':
                        {
                            index++;
                            if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.Interconnect))
                                primitiveNumber = 1;
                            break;
                        }
                    case 'L':
                        {
                            index++;
                            if (FPGATypes.IsOrientedMatch(endTile.Location, IdentifierManager.RegexTypes.CLB_left))
                                primitiveNumber = 0;
                            else if (FPGATypes.IsOrientedMatch(endTile.Location, IdentifierManager.RegexTypes.CLB_right))
                                primitiveNumber = 2;
                            break;
                        }
                    case 'R':
                        {
                            index++;
                            if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.BRAM))
                                primitiveNumber = index;
                            break;
                        }
                    case 'D':
                        {
                            index++;
                            if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.DSP))
                                primitiveNumber = index;
                            break;
                        }
                    case 'M':
                        {
                            index++;
                            if (FPGATypes.IsOrientedMatch(endTile.Location, IdentifierManager.RegexTypes.CLB_left))
                                primitiveNumber = 0;
                            else if (FPGATypes.IsOrientedMatch(endTile.Location, IdentifierManager.RegexTypes.CLB_right))
                                primitiveNumber = 2;
                            break;
                        }
                    case 'P':
                        {
                            index++;
                            if (hdio.IsMatch(endTile.Location))
                                primitiveNumber = index;
                            break;
                        }
                    case 'F':
                        {
                            index++;
                            if (cm_final_identifier.IsMatch(endTile.Location))
                                primitiveNumber = index;
                            break;
                        }
                    case 'm':
                        break;
                    default:
                        {
                            index++;
                            primitiveNumber = -999;
                            break;
                        }
                }

                if (primitiveNumber != -999)
                    break;
            }

            if (primitiveNumber == -999)
            {
                if (!uncommonTiles.ContainsKey(endTile))
                    uncommonTiles.Add(endTile, uncommonTiles.Count() + uncommonTilesHashcodeShift);
                
                primitiveNumber = uncommonTiles[endTile];
            }

            wirelist.Add(new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, xIncr, yIncr, primitiveNumber));
        }

        private void processWirelistForTile(bool checkForSubinterconnects, Tile tile, out WireList wirelist)
        {
            Tile subInterconnect = null;
            wirelist = new WireList();
            foreach (Wire w in tile.WireList)
            {
                Tile target = Navigator.GetDestinationByWire(tile, w);

                short xIncr = (short)(target.LocationX - tile.LocationX);
                short yIncr = (short)(target.LocationY - tile.LocationY);

                if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    subInterconnect = target;

                    foreach (Port otherPortOnSubInterconnect in subInterconnect.SwitchMatrix.GetDrivenPorts(new Port(w.PipOnOtherTile)))
                    {
                        foreach (Wire intermediateWire in subInterconnect.WireList.GetAllWires(otherPortOnSubInterconnect))
                        {
                            Tile extendedTile = Navigator.GetDestinationByWire(subInterconnect, intermediateWire);
                            xIncr = (short)(extendedTile.LocationX - tile.LocationX);
                            yIncr = (short)(extendedTile.LocationY - tile.LocationY);
                            AddWireToWirelist(tile, wirelist, w, xIncr, yIncr, intermediateWire, extendedTile);
                        }
                    }
                }
                else
                {
                    AddWireToWirelist(tile, wirelist, w, xIncr, yIncr, w, target);
                }

            }
        }

        private Tile getCoreRamTile(Tile ramBlockTile)
        {
            if (RAMSelectionManager.Instance.HasMapping(ramBlockTile))
            {
                foreach (Tile mainRamTile in RAMSelectionManager.Instance.GetRamBlockMembers(ramBlockTile).Where(c => IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.BRAM) || IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.DSP)))
                {
                    return mainRamTile;
                }
            }

            return null;
        }

        private void StoreInterconnectWirelist(WireList wirelistToStore, Dictionary<int, WireList> wirelistCollectionToCheckIn)
        {
            bool equalWLFound = false;

            foreach (WireList other in wirelistCollectionToCheckIn.Values.Where(wl => wl.Count == wirelistToStore.Count))
            {
                if (other.Equals(wirelistToStore))
                {
                    wirelistToStore.Key = other.Key;
                    equalWLFound = true;
                    break;
                }
            }

            if (!equalWLFound)
            {
                wirelistToStore.Key = wirelistCollectionToCheckIn.Count;
            }


            // now share common wire list
            if (!wirelistCollectionToCheckIn.ContainsKey(wirelistToStore.Key))
            {
                wirelistCollectionToCheckIn.Add(wirelistToStore.Key, wirelistToStore);
            }
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }
    }

    class PrimitiveWire : Wire
    {
        public int primitiveNumber;

        public PrimitiveWire(uint localPipKey, uint pipOnOtherTileKey, bool localPipIsDriver, int xIncr, int yIncr, int primitiveNumber) : base(localPipKey, pipOnOtherTileKey, localPipIsDriver, xIncr, yIncr)
        {
            this.primitiveNumber = primitiveNumber;
        }
    }

}
