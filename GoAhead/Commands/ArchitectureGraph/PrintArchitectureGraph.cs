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
        private Dictionary<int, WireList> interconnectWirelist = new Dictionary<int, WireList>();
        private Dictionary<int, WireList> clbConnectionsWirelist = new Dictionary<int, WireList>();

        private Dictionary<Tile, int> tileToInterconnectWirelistMapping = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> tileToClbConnectionsWirelistMapping = new Dictionary<Tile, int>();

        private Dictionary<Tile, List<Tile>> clbTilesForInterconnectTile = new Dictionary<Tile, List<Tile>>();

        protected override void DoCommandAction()
        {
            bool checkForSubinterconnects = IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect);
            RAMSelectionManager.Instance.UpdateMapping();
            foreach (Tile intTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                if (IdentifierManager.Instance.HasRegexp(IdentifierManager.RegexTypes.SubInterconnect) && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    continue;

                WireList thisTileWL = new WireList();
                WireList primitivesConnectionsWirelist = new WireList();

                Dictionary<Tile, int> primtivesForInterconnect = GetPrimitivesForInterconnect(intTile , checkForSubinterconnects);
                if (primtivesForInterconnect.Count() > 2)
                    OutputManager.WriteOutput("Error: More than 2 primitives found for " + intTile.Location + ": ");

                clbTilesForInterconnectTile.Add(intTile, primtivesForInterconnect.OrderBy(p => p.Value).Select(x=>x.Key).ToList());

                Tile subInterconnect = null;

                foreach (Wire w in intTile.WireList)
                {
                    Tile target = Navigator.GetDestinationByWire(intTile, w);
                    short xIncr = (short)(target.LocationX - intTile.LocationX);
                    short yIncr = (short)(target.LocationY - intTile.LocationY);


                    if (xIncr == 0 && yIncr == 0 && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.CLB) && primtivesForInterconnect.ContainsKey(target))
                    { 
                        primitivesConnectionsWirelist.Add(new PrimitiveWire(w.LocalPipKey, w.PipOnOtherTileKey, w.LocalPipIsDriver, w.XIncr, w.YIncr, primtivesForInterconnect[target])); 
                    }
                    else if (xIncr == 0 && yIncr == 0 && checkForSubinterconnects && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    {
                        subInterconnect = target;
                        bool foundOneMatchingPortOnRam = false;

                        foreach(Port otherPortOnSubInterconnect in subInterconnect.SwitchMatrix.GetDrivenPorts(new Port(w.PipOnOtherTile)))
                        {
                            foreach(Wire intermediateWire in subInterconnect.WireList.GetAllWires(otherPortOnSubInterconnect))
                            {
                                Tile ramTile = Navigator.GetDestinationByWire(subInterconnect, intermediateWire);
                                xIncr = (short)(ramTile.LocationX - intTile.LocationX);
                                yIncr = (short)(ramTile.LocationY - intTile.LocationY);

                                if (xIncr != 0 || yIncr != 0 || !primtivesForInterconnect.ContainsKey(ramTile))
                                {
                                    OutputManager.WriteOutput("Error: Following subinterconnect wires, did not reach expected ram tile " + ramTile.Location + " interconnect tile " + intTile.Location + " interconnect port = " + w.LocalPip);
                                    break;
                                }

                                if (foundOneMatchingPortOnRam)
                                {
                                    OutputManager.WriteOutput("More than one matching ports found on ram block for interconnect " + intTile.Location + " and interconnect port " + w.LocalPip);
                                    break;
                                }
                                primitivesConnectionsWirelist.Add(new PrimitiveWire(w.LocalPipKey, intermediateWire.PipOnOtherTileKey, w.LocalPipIsDriver, xIncr, yIncr, primtivesForInterconnect[ramTile]));
                            }
                        }
                    }
                    else
                    {
                        thisTileWL.Add(new Wire(w.LocalPipKey, w.PipOnOtherTileKey, w.LocalPipIsDriver, xIncr, yIncr));
                    }
                }

                StoreInterconnectWirelist(thisTileWL, interconnectWirelist);
                tileToInterconnectWirelistMapping.Add(intTile, thisTileWL.Key);

                foreach (KeyValuePair<Tile, int> pair in primtivesForInterconnect)
                {
                    Tile primitive = pair.Key;
                    foreach(Wire w in primitive.WireList)
                    {
                        Tile target = Navigator.GetDestinationByWire(primitive, w);
                        
                        if (target.Location == intTile.Location && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.Interconnect))
                        {
                            primitivesConnectionsWirelist.Add(new PrimitiveWire(w.PipOnOtherTileKey, w.LocalPipKey, !w.LocalPipIsDriver, w.XIncr, w.YIncr, pair.Value));
                        }
                        else if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.SubInterconnect))
                        {
                            subInterconnect = target;
                            bool foundOneMatchingPortOnInterconnect = false;

                            foreach (Port otherPortOnSubInterconnect in subInterconnect.SwitchMatrix.GetDrivenPorts(new Port(w.PipOnOtherTile)))
                            {
                                foreach (Wire intermediateWire in subInterconnect.WireList.GetAllWires(otherPortOnSubInterconnect))
                                {
                                    Tile interconnectTile = Navigator.GetDestinationByWire(subInterconnect, intermediateWire);
                                    short xIncr = (short)(interconnectTile.LocationX - intTile.LocationX);
                                    short yIncr = (short)(interconnectTile.LocationY - intTile.LocationY);

                                    if (interconnectTile.Location != intTile.Location)
                                    {
                                        OutputManager.WriteOutput("Error: Following subinterconnect wires, did not reach expected interconnect tile " + intTile.Location + " primtive tile " + primitive.Location + " primitive port = " + w.LocalPip);
                                        break;
                                    }
                                    if (foundOneMatchingPortOnInterconnect)
                                    {
                                        OutputManager.WriteOutput("More than one matching ports found on interconnect block for ram " + primitive.Location + " and primitive port " + w.LocalPip);
                                        break;
                                    }
                                    primitivesConnectionsWirelist.Add(new PrimitiveWire(intermediateWire.PipOnOtherTileKey, w.LocalPipKey, !w.LocalPipIsDriver, xIncr, yIncr, pair.Value));
                                }
                            }
                        }
                    }
                }

                StoreInterconnectWirelist(primitivesConnectionsWirelist, clbConnectionsWirelist);
                tileToClbConnectionsWirelistMapping.Add(intTile, primitivesConnectionsWirelist.Key);
            }

            PrintAllSwitchMatrixWirelists printWirelists = new PrintAllSwitchMatrixWirelists();
            printWirelists.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\wirelists";
            printWirelists.InterconnectWirelists = interconnectWirelist;
            CommandExecuter.Instance.Execute(printWirelists);


            PrintAllTiles printTiles = new PrintAllTiles();
            printTiles.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\tiles";
            printTiles.CLBTilesForInterconnectTile = clbTilesForInterconnectTile;
            printTiles.WirelistHashcodeForInterconnectTiles = tileToInterconnectWirelistMapping;
            printTiles.PrimitiveConnectionsWirelistHashcodeForInterconnectTile = tileToClbConnectionsWirelistMapping;
            CommandExecuter.Instance.Execute(printTiles);

            PrintAllPrimitveConnectionsWirelists printPrimitiveConnections = new PrintAllPrimitveConnectionsWirelists();
            printPrimitiveConnections.FileName = "C:\\Users\\prabh\\OneDrive\\Documents\\Uni\\Internship\\GoAhead\\primitiveConnections";
            printPrimitiveConnections.PrimitiveWirelists = clbConnectionsWirelist;
            CommandExecuter.Instance.Execute(printPrimitiveConnections);
        }

        public static Dictionary<Tile, int> GetPrimitivesForInterconnect(Tile t, bool checkForSubinterconnects)
        {
            Dictionary<Tile, int> primitives = new Dictionary<Tile, int>();
            foreach (Tile clbTile in FPGATypes.GetCLTile(t).Where(c => c.LocationX == t.LocationX && c.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.CLB)))
                primitives.Add(clbTile, primitives.Count + 1);

            if (checkForSubinterconnects)
            {
                foreach (Tile subInterconnectTile in FPGATypes.GetSubInterconnectTile(t).Where(c => c.LocationX == t.LocationX && c.LocationY == t.LocationY && IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.SubInterconnect)))
                {
                    Tile ramTile = null;
                    if (RAMSelectionManager.Instance.HasMapping(FPGA.FPGA.Instance.GetTile(subInterconnectTile.TileKey.X - 1, subInterconnectTile.TileKey.Y)))
                    {
                        ramTile = FPGA.FPGA.Instance.GetTile(subInterconnectTile.TileKey.X - 1, subInterconnectTile.TileKey.Y);
                    }
                    else if (RAMSelectionManager.Instance.HasMapping(FPGA.FPGA.Instance.GetTile(subInterconnectTile.TileKey.X + 1, subInterconnectTile.TileKey.Y)))
                    {
                        ramTile = FPGA.FPGA.Instance.GetTile(subInterconnectTile.TileKey.X + 1, subInterconnectTile.TileKey.Y);
                    }

                    if (ramTile != null)
                    {
                        foreach (Tile mainRamTile in RAMSelectionManager.Instance.GetRamBlockMembers(ramTile).Where(c => IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.BRAM) || IdentifierManager.Instance.IsMatch(c.Location, IdentifierManager.RegexTypes.DSP)))
                        {
                            primitives.Add(mainRamTile, primitives.Count + 1);
                        }
                    }
                }
            }
            return primitives;
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
