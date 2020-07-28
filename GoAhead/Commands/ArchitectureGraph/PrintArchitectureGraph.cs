using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands.ArchitectureGraph
{
    class PrintArchitectureGraph : CommandWithFileOutput
    {
        //private Dictionary<int, WireList> interconnectWirelist = new Dictionary<int, WireList>();
        //private Dictionary<int, WireList> clbConnectionsWirelist = new Dictionary<int, WireList>();

        //private Dictionary<Tile, int> tileToInterconnectWirelistMapping = new Dictionary<Tile, int>();
        //private Dictionary<Tile, int> tileToClbConnectionsWirelistMapping = new Dictionary<Tile, int>();

        //private Dictionary<Tile, List<Tile>> clbTilesForInterconnectTile = new Dictionary<Tile, List<Tile>>();



        private Dictionary<Tile, int> interconnectTiles = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> primitiveTiles = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> uncommonTiles = new Dictionary<Tile, int>();

        private Dictionary<int, List<int>> interconnectToPrimitiveTiles = new Dictionary<int, List<int>>();
        private Dictionary<int, Tile> primtiveToInterconnectTiles = new Dictionary<int, Tile>();

        private Dictionary<int, int> interconnectToWirelistHashcode = new Dictionary<int, int>();
        private Dictionary<int, int> primitiveToWirelistHashcode = new Dictionary<int, int>();
        private Dictionary<int, int> uncommonTilesToWirelistHashcode = new Dictionary<int, int>();

        private Dictionary<int, WireList> wirelistHashcodeToWirelist = new Dictionary<int, WireList>();



        protected override void DoCommandAction()
        {
            bool checkForSubinterconnects = IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect);
            RAMSelectionManager.Instance.UpdateMapping();

            foreach (Tile intTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    continue;

                interconnectTiles.Add(intTile, interconnectTiles.Count());
                interconnectToPrimitiveTiles.Add(interconnectTiles[intTile], new List<int>());
                Tile subInterconnect = null;

                foreach (Wire w in intTile.WireList)
                {
                    Tile target = Navigator.GetDestinationByWire(intTile, w);

                    if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    {
                        subInterconnect = target;

                        foreach (Port otherPortOnSubInterconnect in subInterconnect.SwitchMatrix.GetDrivenPorts(new Port(w.PipOnOtherTile)))
                        {
                            foreach (Wire intermediateWire in subInterconnect.WireList.GetAllWires(otherPortOnSubInterconnect))
                            {
                                Tile extendedPrimitiveTile = Navigator.GetDestinationByWire(subInterconnect, intermediateWire);

                                if (getCoreRamTile(extendedPrimitiveTile) != null)
                                {
                                    extendedPrimitiveTile = getCoreRamTile(extendedPrimitiveTile);
                                }

                                if (!primitiveTiles.ContainsKey(extendedPrimitiveTile))
                                {
                                    primitiveTiles.Add(extendedPrimitiveTile, primitiveTiles.Count());
                                    primtiveToInterconnectTiles.Add(primitiveTiles[extendedPrimitiveTile], intTile);
                                }

                                if (!interconnectToPrimitiveTiles[interconnectTiles[intTile]].Contains(primitiveTiles[extendedPrimitiveTile]))
                                {
                                    interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(primitiveTiles[extendedPrimitiveTile]);
                                }
                            }
                        }
                    }
                    else if (IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.Interconnect))
                        continue;
                    else
                    {
                        if (!primitiveTiles.ContainsKey(target))
                        {
                            primitiveTiles.Add(target, primitiveTiles.Count());
                            primtiveToInterconnectTiles.Add(primitiveTiles[target], intTile);
                        }
                        if (!interconnectToPrimitiveTiles[interconnectTiles[intTile]].Contains(primitiveTiles[target]))
                        {
                            interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(primitiveTiles[target]);
                        }
                    }
                }
            }

            foreach(Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => !interconnectTiles.ContainsKey(t) && !primitiveTiles.ContainsKey(t))) 
            {
                if(checkForSubinterconnects && !IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    if(tile.WireList.Count() > 0)
                    {
                        uncommonTiles.Add(tile, uncommonTiles.Count());
                    }
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

            foreach (KeyValuePair<Tile, int> pair in uncommonTiles)
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
            CommandExecuter.Instance.Execute(printTiles);

            PrintAllPrimitveTiles printPrimitiveConnections = new PrintAllPrimitveTiles();
            printPrimitiveConnections.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\AG\\primitiveTiles";
            printPrimitiveConnections.PrimitiveTiles = primitiveTiles;
            printPrimitiveConnections.PrimitiveToWirelistHashcode = primitiveToWirelistHashcode;
            CommandExecuter.Instance.Execute(printPrimitiveConnections);

            PrintUncommonTiles printUncommonTiles = new PrintUncommonTiles();
            printUncommonTiles.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\AG\\uncommonTiles";
            printUncommonTiles.UncommonTiles = uncommonTiles;
            printUncommonTiles.UncommonToWirelistHashcode = uncommonTilesToWirelistHashcode;
            CommandExecuter.Instance.Execute(printUncommonTiles);
        }


        private void AddWireToWirelist(Tile startTile, WireList wirelist, Wire startWire, short xIncr, short yIncr, Wire endWire, Tile endTile)
        {
            if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                wirelist.Add(new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, xIncr, yIncr, -1));
            }
            else if (interconnectTiles.ContainsKey(startTile) && interconnectToPrimitiveTiles[interconnectTiles[startTile]].Contains(primitiveTiles[endTile]))
            {
                int primtiveIndex = interconnectToPrimitiveTiles[interconnectTiles[startTile]].IndexOf(primitiveTiles[endTile]);

                wirelist.Add(new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, xIncr, yIncr, primtiveIndex));
            }
            else
            {

                // super hardcoded. better way?
                try
                {
                    Tile interconnectTileOnOtherXY = primtiveToInterconnectTiles[primitiveTiles[endTile]];

                    if (!interconnectToPrimitiveTiles[interconnectTiles[interconnectTileOnOtherXY]].Contains(primitiveTiles[endTile]))
                    {
                        OutputManager.WriteOutput("BIG ERROR");
                    }
                    else
                    {
                        int primtiveIndex = interconnectToPrimitiveTiles[interconnectTiles[interconnectTileOnOtherXY]].IndexOf(primitiveTiles[endTile]);

                        wirelist.Add(new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, xIncr, yIncr, primtiveIndex));
                    }
                }
                catch(Exception e)
                {
                    if(uncommonTiles.ContainsKey(endTile))
                    {
                        wirelist.Add(new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, -999, -999, uncommonTiles[endTile]));
                    }
                    else
                    {
                        uncommonTiles.Add(endTile, uncommonTiles.Count);
                        wirelist.Add(new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, -999, -999, uncommonTiles[endTile]));
                    }
                }
            }
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


        //public Dictionary<Tile, int> GetPrimitivesForInterconnect(Tile t, bool checkForSubinterconnects)
        //{
        //    Dictionary<Tile, int> primitives = new Dictionary<Tile, int>();

        //    Tile left = FPGA.FPGA.Instance.GetTile(t.TileKey.X - 1, t.TileKey.Y);
        //    Tile right = FPGA.FPGA.Instance.GetTile(t.TileKey.X + 1, t.TileKey.Y);

        //    processPrimitiveTile(left, t, checkForSubinterconnects, primitives);
        //    processPrimitiveTile(right, t, checkForSubinterconnects, primitives);

        //    return primitives;
        //}

        //private void processPrimitiveTile (Tile p, Tile interconnect, bool checkForSubinterconnects, Dictionary<Tile, int> primitives)
        //{
        //    if(p.LocationX == interconnect.LocationX && p.LocationY == interconnect.LocationY && IdentifierManager.Instance.IsMatch(p.Location, IdentifierManager.RegexTypes.CLB))
        //        primitives.Add(p, primitives.Count());
        //    else if(checkForSubinterconnects && p.LocationX == interconnect.LocationX && p.LocationY == interconnect.LocationY && IdentifierManager.Instance.IsMatch(p.Location, IdentifierManager.RegexTypes.SubInterconnect))
        //    {
        //        Tile extendedPrimitiveTile = FPGA.FPGA.Instance.GetTile(p.TileKey.X - 1, p.TileKey.Y).Location != interconnect.Location ? FPGA.FPGA.Instance.GetTile(p.TileKey.X - 1, p.TileKey.Y) : FPGA.FPGA.Instance.GetTile(p.TileKey.X + 1, p.TileKey.Y);
        //        if (!addRAMTile(extendedPrimitiveTile, primitives))
        //        {
        //            // warn
        //            foreach(Tile target in extendedPrimitiveTile.WireList.Where(w => !primitives.Keys.Contains(Navigator.GetDestinationByWire(extendedPrimitiveTile, w))).Select(w => Navigator.GetDestinationByWire(extendedPrimitiveTile, w))) 
        //            {
        //                //warn
        //                primitives.Add(target, primitives.Count());
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //warn
        //        primitives.Add(p, primitives.Count());
        //    }

        //}

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
