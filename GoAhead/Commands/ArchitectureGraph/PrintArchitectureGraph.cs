using GoAhead.Commands.GridStyle;
using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.ArchitectureGraph
{
    // using struct is more readable, but for better performance, convert this struct to a Dict or Lists
    struct intBlock
    {
        public int intTile;
        public int leftPrimitive;
        public int rightPrimitive;
    }

    class PrintArchitectureGraph : CommandWithFileOutput
    {
        // define data structures
        private Dictionary<Tile, int> tiles = new Dictionary<Tile, int>(); // tiles to hashcode mappings
        private List<intBlock> interconnectBlocks = new List<intBlock>();
        private List<int> irregularTiles = new List<int>();
        private Dictionary<int, Tile> tileHashcodes = new Dictionary<int, Tile>(); // hashcodes to tiles mapping

        private Dictionary<int, int> tileToWirelistHashcodes = new Dictionary<int, int>();
        private Dictionary<int, int> tileToIncomingWirelistHashcodes = new Dictionary<int, int>();

        private Dictionary<Tile, WireList> wirelistsToConsider = new Dictionary<Tile, WireList>();
        private Dictionary<Tile, WireList> incomingWirelists = new Dictionary<Tile, WireList>();

        private Dictionary<int, WireList> wirelistHashcodeToWirelist = new Dictionary<int, WireList>();

        private Dictionary<string, int> wireCounts = new Dictionary<string, int>();

        // hard-coded for ZU3EG, should be produced by GoAhead or given as an argument
        // resource string must be space separated
        private static string RESOURCE_STRING = "VsL MsL RsL MsD MsL LsL MsL LsL RsL MsmP msL MsL RsL MsD MsL LsL MsL LsL MsD MsL MsL RsL MsL RsL MsmP msL MsD MsL MsD MsL MsL RsL MsmFmI";

        bool checkForSubinterconnects = RESOURCE_STRING.Contains("m");
        
        private string[] resourceStringChopped = RESOURCE_STRING.Split(' ');

        protected override void DoCommandAction()
        {
            try
            {
                Directory.CreateDirectory(FolderName);
                if (debug && string.IsNullOrEmpty(FileName))
                    FileName = Path.Combine(FolderName, "debug.ag");
            }
            catch (Exception e)
            {
                throw new Exception("Could not create folder " + FolderName, e);
            }

            // before starting, make sure all BRAMs/DSPs are updated
            RAMSelectionManager.Instance.UpdateMapping();

            resolveWiresInOppositeDirections();

            if (debug)
            {
                // debug information collection
                foreach (KeyValuePair<Tile, WireList> pair in wirelistsToConsider)
                {
                    WireList wl = pair.Value;
                    Tile startTile = pair.Key;
                    if (wl.Count() > 0)
                    {
                        foreach (Wire w in wl)
                        {
                            // keep count of each wire
                            Tile endTile = Navigator.GetDestinationByWire(startTile, w);
                            string wireCountKey = startTile.Location + "-" + w.LocalPip + "-" + w.PipOnOtherTile + "-" + endTile.Location;
                            if (wireCounts.ContainsKey(wireCountKey))
                                wireCounts[wireCountKey] += 1;
                            else
                                wireCounts.Add(wireCountKey, 1);
                        }
                    }
                }
            }

            // goal is to create blocks of LsL or MsL. We go through all the interconnect tiles and find the left and the right tile from it, and count these as the interconnect's primitives
            foreach (Tile intTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.Interconnect)))
            {
                if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(intTile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                    continue;

                tiles.Add(intTile, tiles.Count());

                Tile leftPrimitive = GetPrimitive(intTile, checkForSubinterconnects, left: true);
                Tile rightPrimitive = GetPrimitive(intTile, checkForSubinterconnects, left: false);

                intBlock block = GetIntBlock(intTile, leftPrimitive, rightPrimitive);
                interconnectBlocks.Add(block);
            }

            // once all the homogenous blocks have been found, find all the irregular tiles
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => !tiles.ContainsKey(t))) // Dict tiles at this point contains every tile that has been identified as belonging to a block
            {
                if (checkForSubinterconnects && !IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    // don't include tiles with no wirelists, such as NULL_X0Y0, etc.
                    if (tile.WireList.Count() > 0)
                    {
                        tiles.Add(tile, tiles.Count()); // it is important that irregular tiles have the biggest hashcodes.
                        irregularTiles.Add(tiles[tile]);
                    }  
                }
            }

            // process wirelists for all tiles
            foreach (KeyValuePair<Tile, int> pair in tiles.Reverse())
            {
                Tile tile = pair.Key;
                int tileHashcode = pair.Value;
                WireList wl = new WireList();
                processWirelistForTile(checkForSubinterconnects, tile, out wl);

                if (wl.Count() > 0)
                {
                    StoreWirelist(wl, wirelistHashcodeToWirelist);
                    tileToWirelistHashcodes.Add(tileHashcode, wl.Key);
                }
                else
                {
                    // -1 represents empty wirelist
                    tileToWirelistHashcodes.Add(tileHashcode, -1);
                }
            }

            // store incoming wirelists for all tiles
            foreach (KeyValuePair<Tile, WireList> pair in incomingWirelists)
            {
                Tile tile = pair.Key;
                int tileHashcode = tiles[tile];
                WireList incomingWirelist = pair.Value;

                if (incomingWirelist.Count() > 0)
                {
                    StoreWirelist(incomingWirelist, wirelistHashcodeToWirelist);
                    tileToIncomingWirelistHashcodes.Add(tileHashcode, incomingWirelist.Key);
                }
                else
                {
                    tileToIncomingWirelistHashcodes.Add(tileHashcode, -1);
                }
            }

            // make sure there's no tiles left with no wirelist hashcodes
            foreach (Tile tile in tiles.Keys)
            {
                if (!tileToWirelistHashcodes.ContainsKey(tiles[tile]))
                    tileToWirelistHashcodes.Add(tiles[tile], -1);

                if (!tileToIncomingWirelistHashcodes.ContainsKey(tiles[tile]))
                    tileToIncomingWirelistHashcodes.Add(tiles[tile], -1);
            }

            // populate the other direction of tiles dict
            foreach (KeyValuePair<Tile, int> pair in tiles)
                tileHashcodes.Add(pair.Value, pair.Key);

            // output the graph with help of subcommands
            PrintAllWirelists printWirelists = new PrintAllWirelists();
            printWirelists.FileName = Path.Combine(FolderName, "wirelists.ag");
            printWirelists.Wirelists = wirelistHashcodeToWirelist;
            CommandExecuter.Instance.Execute(printWirelists);

            PrintAllInterconnectBlocks printTiles = new PrintAllInterconnectBlocks();
            printTiles.FileName = Path.Combine(FolderName, "tiles.ag");
            printTiles.TileHashcodes = tileHashcodes;
            printTiles.InterconnectBlocks = interconnectBlocks;
            printTiles.WirelistHashcodes = tileToWirelistHashcodes;
            printTiles.IncomingWirelistHashcodes = tileToIncomingWirelistHashcodes;
            CommandExecuter.Instance.Execute(printTiles);

            PrintIrregularTiles printIrregularTiles = new PrintIrregularTiles();
            printIrregularTiles.FileName = Path.Combine(FolderName, "irregularTiles.ag");
            printIrregularTiles.IrregularTiles = irregularTiles;
            printIrregularTiles.WirelistHashcodes = tileToWirelistHashcodes;
            printIrregularTiles.TileHashcodes = tileHashcodes;
            printIrregularTiles.IncomingWirelistHashcodes = tileToIncomingWirelistHashcodes;
            CommandExecuter.Instance.Execute(printIrregularTiles);

            // perhaps this can be used to print the resource string, arch family in a separate file. Currently, it's hardcoded.
            PrintMiscInformation printMiscInformation = new PrintMiscInformation();

            if (debug)
            {
                // print any debug information out
                foreach (KeyValuePair<string, int> pair in wireCounts)
                {
                    if (pair.Value == -999)
                    {
                        string[] wireParts = pair.Key.Split('-');
                        OutputManager.WriteOutput("WARNING: Architecture Graph has a wire from " + wireParts[0] + " " + wireParts[1] + " to " + wireParts[2] + " " + wireParts[3] + " which doesn't exists in the model.");
                    }
                    else if (pair.Value != 0)
                    {
                        string[] wireParts = pair.Key.Split('-');
                        OutputManager.WriteOutput("WARNING: Wire starting from tile " + wireParts[0] + " port " + wireParts[1] + " and ending at tile " + wireParts[2] + " port " + wireParts[3] + " which exists " + pair.Value + " times in the model, was not found in the generated Architecture Graph.");
                    }
                }
            }
        }

        private void resolveWiresInOppositeDirections()
        {
            // consider all wires in correct direction
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles())
            {
                WireList newWirelist = new WireList();
                if (wirelistsToConsider.ContainsKey(tile))
                    newWirelist = wirelistsToConsider[tile];

                foreach (Wire w in tile.WireList)
                {
                    if (!w.LocalPipIsDriver)
                    {
                        Tile target = Navigator.GetDestinationByWire(tile, w);
                        if (wirelistsToConsider.ContainsKey(target))
                        {
                            int xIncr = (tile.TileKey.X - target.TileKey.X);
                            int yIncr = (tile.TileKey.Y - target.TileKey.Y);
                            wirelistsToConsider[target].Add(new Wire(w.PipOnOtherTileKey, w.LocalPipKey, !w.LocalPipIsDriver, xIncr, yIncr));
                        }
                        else
                        {
                            WireList wl = new WireList();
                            int xIncr = (tile.TileKey.X - target.TileKey.X);
                            int yIncr = (tile.TileKey.Y - target.TileKey.Y);
                            wl.Add(new Wire(w.PipOnOtherTileKey, w.LocalPipKey, !w.LocalPipIsDriver, xIncr, yIncr));
                            wirelistsToConsider[tile] = newWirelist;
                        }
                    }
                    else
                    {
                        newWirelist.Add(w);
                    }
                }
                wirelistsToConsider[tile] = newWirelist;
            }
        }

        private intBlock GetIntBlock(Tile intTile, Tile leftPrim, Tile rightPrim)
        {
            // create an intBlock from constituent tiles
            intBlock block = new intBlock();
            block.intTile = tiles[intTile];
            if (leftPrim != null)
            {
                if (!tiles.Keys.Contains(leftPrim))
                    tiles.Add(leftPrim, tiles.Count());

                block.leftPrimitive = tiles[leftPrim];
            }
            else
                // flag to recognize non-existing primitive is -999
                block.leftPrimitive = -999;

            if (rightPrim != null)
            {
                if (!tiles.Keys.Contains(rightPrim))
                    tiles.Add(rightPrim, tiles.Count());

                block.rightPrimitive = tiles[rightPrim];
            }
            else
                block.rightPrimitive = -999;

            return block;
        }

        private Tile GetPrimitive(Tile intTile, bool skipOverSubInterconnects, bool left)
        {
            // get primitive left or right of the intTile. Skip over any subInterconnects, if wanted. If primitive belongs to a BRAM/DSP block, get the core tile of the block.
            Tile tile = FPGA.FPGA.Instance.GetTile(intTile.TileKey.X + (left ? -1 : 1), intTile.TileKey.Y);

            if (skipOverSubInterconnects && IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                tile = FPGA.FPGA.Instance.GetTile(tile.TileKey.X + (left ? -1 : 1), tile.TileKey.Y);
            }

            if (getCoreRamTile(tile) != null)
            {
                tile = getCoreRamTile(tile);
            }
            
            if(IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB) || IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.BRAM) || IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.DSP))
                return tile;

            return null;
        }

        private void AddWireToWirelist(Tile startTile, WireList wirelist, Wire startWire, Wire endWire, Tile endTile)
        {
            // we need to find the primitive number based on the destionation tile
            int primitiveNumber = getPrimitiveNumber(endTile);

            // the XIncr and YIncr is calculated on the basis of the interconnect tile grid
            short xIncr = (short)(endTile.LocationX - startTile.LocationX);
            short yIncr = (short)(endTile.LocationY - startTile.LocationY);

            PrimitiveWire wire = new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, xIncr, yIncr, primitiveNumber);
            wirelist.Add(wire);

            // for each wire, we add a wire in the opposite direction. This helps is create the incoming wirelist for a tile.
            PrimitiveWire fakeWire = new PrimitiveWire(endWire.PipOnOtherTileKey, startWire.LocalPipKey, !startWire.LocalPipIsDriver, (startTile.LocationX - endTile.LocationX), (startTile.LocationY - endTile.LocationY), getPrimitiveNumber(startTile));

            if (!incomingWirelists.ContainsKey(endTile))
                incomingWirelists.Add(endTile, new WireList());

            incomingWirelists[endTile].Add(fakeWire);

            if (debug)
            {
                if (startWire == endWire)
                {
                    // not dealing with subinterconnects
                    updateWireCounts(startTile.Location + "-" + startWire.LocalPip + "-" + endWire.PipOnOtherTile + "-" + endTile.Location);
                }
                else
                {
                    updateWireCounts(startTile.Location + "-" + startWire.LocalPip + "-" + startWire.PipOnOtherTile + "-" + Navigator.GetDestinationByWire(startTile, startWire).Location);
                    updateWireCounts(Navigator.GetDestinationByWire(startTile, startWire).Location + "-" + endWire.LocalPip + "-" + endWire.PipOnOtherTile + "-" + endTile.Location);

                }
            }
        }

        private int getPrimitiveNumber(Tile endTile)
        {
            // the primitive number indicates which tile in an intBlock does the wire end at. If the tile does not belong to an intBlock, then the primitive number points to the hashcode of the irregular tile that the wire ends at. 
            string blockString = resourceStringChopped[endTile.LocationX];
            blockString.Replace("m", string.Empty);
            int primitiveNumber;

            if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.Interconnect))
                primitiveNumber = 1;
            else if (FPGATypes.IsOrientedMatch(endTile.Location, IdentifierManager.RegexTypes.CLB_left))
                primitiveNumber = 0;
            else if (FPGATypes.IsOrientedMatch(endTile.Location, IdentifierManager.RegexTypes.CLB_right))
                primitiveNumber = 2;
            else if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.BRAM))
                primitiveNumber = blockString.IndexOf('R');
            else if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.DSP))
                primitiveNumber = blockString.IndexOf('D');
            else
                primitiveNumber = -999;


            if (primitiveNumber == -999)
            {
                // if primitive not found, check in irregular tiles. If not found, add the irregular tile, because we need an entry to represent where this wire is ending.
                if (!tiles.ContainsKey(endTile))
                {
                    tiles.Add(endTile, tiles.Count());
                    irregularTiles.Add(tiles[endTile]);
                }

                primitiveNumber = tiles[endTile];
            }

            return primitiveNumber;
        }

        private void updateWireCounts(string wireCountKey)
        {
            if (wireCounts.ContainsKey(wireCountKey))
            {
                wireCounts[wireCountKey] = wireCounts[wireCountKey] - 1;
            }
            else
            {
                // -999 flag to decide the debug message
                wireCounts.Add(wireCountKey, -999);
            }
        }

        private void processWirelistForTile(bool checkForSubinterconnects, Tile tile, out WireList wirelist)
        {
            wirelist = new WireList();
            foreach (Wire w in wirelistsToConsider[tile])
            {
                
                Tile target = Navigator.GetDestinationByWire(tile, w);

                if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    Tile subInterconnect = target;

                    foreach (Port otherPortOnSubInterconnect in subInterconnect.SwitchMatrix.GetDrivenPorts(new Port(w.PipOnOtherTile)))
                    {
                        foreach (Wire intermediateWire in subInterconnect.WireList.GetAllWires(otherPortOnSubInterconnect))
                        {
                            Tile extendedTile = Navigator.GetDestinationByWire(subInterconnect, intermediateWire);
                            AddWireToWirelist(tile, wirelist, w, intermediateWire, extendedTile);
                        }
                    }
                }
                else
                {
                    AddWireToWirelist(tile, wirelist, w, w, target);
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

        public static void StoreWirelist(WireList wirelistToStore, Dictionary<int, WireList> wirelistCollectionToCheckIn)
        {
            bool equalWLFound = false;

            foreach (WireList other in wirelistCollectionToCheckIn.Values.Where(wl => wl.Count == wirelistToStore.Count))
            {
                if (WirelistsEqual(wirelistToStore, other))
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

        private static bool WirelistsEqual(WireList a, WireList b)
        {
            if (a.m_wires.Count != b.m_wires.Count)
            {
                return false;
            }

            foreach (PrimitiveWire w in a)
            {
                if (!HasWire(b, w))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HasWire(WireList wl, PrimitiveWire wire)
        {
            // first, find the matching wire. It should be in m_wires.
            int index = wl.m_wires.IndexOf(wire);

            if (index != -1)
            {
                return ((PrimitiveWire)wl.m_wires[index]).primitiveNumber == wire.primitiveNumber;
            }

            return false;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "Print debug information in FileName")]
        public bool debug = false;

        [Parameter(Comment = "Folder where the Architecture Graph files will be outputted")]
        public string FolderName = "";
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
