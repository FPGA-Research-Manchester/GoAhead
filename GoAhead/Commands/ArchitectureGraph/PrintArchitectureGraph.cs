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
    class PrintArchitectureGraph : CommandWithFileOutput
    {
        // define data structures
        private Dictionary<Tile, int> interconnectTiles = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> primitiveTiles = new Dictionary<Tile, int>();
        private Dictionary<Tile, int> irregularTiles = new Dictionary<Tile, int>();

        private Dictionary<int, List<int>> interconnectToPrimitiveTiles = new Dictionary<int, List<int>>();
        private Dictionary<int, int> interconnectToWirelistHashcode = new Dictionary<int, int>();
        
        private Dictionary<int, int> primitiveToWirelistHashcode = new Dictionary<int, int>();
        private Dictionary<int, int> irregularTilesToWirelistHashcode = new Dictionary<int, int>();

        private Dictionary<int, WireList> wirelistHashcodeToWirelist = new Dictionary<int, WireList>();

        private Dictionary<string, int> wireCounts = new Dictionary<string, int>();

        private const int irregularTilesHashcodeShift = 10;

        // hard-coded, should be produced by GoAhead
        private static string RESOURCE_STRING = "VsL MsL RsL MsD MsL LsL MsL LsL RsL MsmP msL MsL RsL MsD MsL LsL MsL LsL MsD MsL MsL RsL MsL RsL MsmP msL MsD MsL MsD MsL MsL RsL MsmFmI";

        // maybe a flag?
        bool checkForSubinterconnects = RESOURCE_STRING.Contains("m");
        
        private string[] resourceStringChopped = RESOURCE_STRING.Split(' ');

        // move to init.goa
        Regex hdio = new Regex("^HDIO_X");
        Regex cm_final_identifier = new Regex("^CM^");

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

            if (debug)
            {
                // grab all available wires
                foreach (Tile startTile in FPGA.FPGA.Instance.GetAllTiles())
                {
                    if (startTile.WireList.Count > 0)
                    {
                        foreach (Wire w in startTile.WireList)
                        {
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

                interconnectTiles.Add(intTile, interconnectTiles.Count());
                interconnectToPrimitiveTiles.Add(interconnectTiles[intTile], new List<int>());

                Tile leftPrimitive = GetPrimitive(intTile, checkForSubinterconnects, left: true);
                Tile rightPrimitive = GetPrimitive(intTile, checkForSubinterconnects, left: false);

                updateDictionariesWithPrimitive(intTile, leftPrimitive);
                updateDictionariesWithPrimitive(intTile, rightPrimitive);

                if (interconnectToPrimitiveTiles[interconnectTiles[intTile]].Count() > 2)
                {
                    OutputManager.WriteOutput("ERROR: found more than 2 primitives - hashcodes " +  string.Join(",", interconnectToPrimitiveTiles[interconnectTiles[intTile]]) + " - for interconnect tile with hashcode " + interconnectTiles[intTile]);
                }
            }

            // once all the homogenous blocks have been found, find all the irregular tiles
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => !interconnectTiles.ContainsKey(t) && !primitiveTiles.ContainsKey(t))) 
            {
                if(checkForSubinterconnects && !IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    // don't include tiles with no wirelists, such as NULL_X0Y0, etc.
                    if(tile.WireList.Count() > 0)
                        // the irregular tiles hashcode enumeration have a predefined shift. This is so that while creating wires, the numbers from 0 to irregularTilesHashcodeShift can be used to refer back to normal primitives. While parsing, if any wire connects to a primitive greater than irregularTilesHashcodeShift, we should look into the irregular tiles to find the destination tile. 
                        irregularTiles.Add(tile, irregularTiles.Count() + irregularTilesHashcodeShift);
                }
            }

            // process wirelists for all interconnect tiles
            foreach (KeyValuePair<Tile, int> pair in interconnectTiles)
            {
                Tile intTile = pair.Key;
                int intTileHashcode = pair.Value;
                WireList interconnectTileWireList = new WireList();
                processWirelistForTile(checkForSubinterconnects, intTile, out interconnectTileWireList);

                StoreWirelist(interconnectTileWireList, wirelistHashcodeToWirelist);
                interconnectToWirelistHashcode.Add(intTileHashcode, interconnectTileWireList.Key);
            }

            // process wirelists for all primitive tiles
            foreach (KeyValuePair<Tile, int> pair in primitiveTiles)
            {
                Tile primitiveTile = pair.Key;
                int primtiveTileHashcode = pair.Value;
                WireList primitiveTileWireList = new WireList();
                processWirelistForTile(checkForSubinterconnects, primitiveTile, out primitiveTileWireList);

                StoreWirelist(primitiveTileWireList, wirelistHashcodeToWirelist);
                primitiveToWirelistHashcode.Add(primitiveTiles[primitiveTile], primitiveTileWireList.Key);
            }

            // process wirelists for all irregular tiles. Since irregular tiles may be found during processing of the wirelists, we create a copy of the irregularTiles data structure, and then iterate through it.
            foreach (KeyValuePair<Tile, int> pair in irregularTiles.Reverse())
            {
                Tile uncommonTile = pair.Key;
                int uncommonTileHashcode = pair.Value;
                WireList uncommonTileWireList = new WireList();
                processWirelistForTile(checkForSubinterconnects, uncommonTile, out uncommonTileWireList);

                if (uncommonTileWireList.Count() > 0)
                {
                    StoreWirelist(uncommonTileWireList, wirelistHashcodeToWirelist);
                    irregularTilesToWirelistHashcode.Add(irregularTiles[uncommonTile], uncommonTileWireList.Key);
                }
                else
                {
                    // if this irregular tile does not have a wirelist, flag it as -1.
                    irregularTilesToWirelistHashcode.Add(irregularTiles[uncommonTile], -1);
                }
            }

            // make sure to update the wirelist hashcodes for irregular tiles found in the previous loop.
            foreach(Tile uncommonTile in irregularTiles.Keys)
            {
                // we know that new irregular tiles found will have an empty wirelist because if not, they would be found when collecting primitives
                if (!irregularTilesToWirelistHashcode.ContainsKey(irregularTiles[uncommonTile]))
                    irregularTilesToWirelistHashcode.Add(irregularTiles[uncommonTile], -1);
            }

            // output the graph with help of subcommands
            PrintAllWirelists printWirelists = new PrintAllWirelists();
            printWirelists.FileName = Path.Combine(FolderName, "wirelists.ag");
            printWirelists.Wirelists = wirelistHashcodeToWirelist;
            CommandExecuter.Instance.Execute(printWirelists);

            PrintAllInterconnectTiles printTiles = new PrintAllInterconnectTiles();
            printTiles.FileName = Path.Combine(FolderName, "tiles.ag");
            printTiles.InterconnectTiles = interconnectTiles;
            printTiles.InterconnectToPrimitiveTiles = interconnectToPrimitiveTiles;
            printTiles.InterconnectToWirelistHashcode = interconnectToWirelistHashcode;
            printTiles.PrimitiveToWirelistHashcode = primitiveToWirelistHashcode;
            CommandExecuter.Instance.Execute(printTiles);

            PrintIrregularTiles printIrregularTiles = new PrintIrregularTiles();
            printIrregularTiles.FileName = Path.Combine(FolderName, "irregularTiles.ag");
            printIrregularTiles.IrregularTiles = irregularTiles;
            printIrregularTiles.IrregularTilesToWirelistHashcodes = irregularTilesToWirelistHashcode;
            CommandExecuter.Instance.Execute(printIrregularTiles);

            if (debug)
            {
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
                        OutputManager.WriteOutput("WARNING: Wire starting from tile " + wireParts[0] + " port " + wireParts[1] + " and ending at tile " + wireParts[2] + " port " + wireParts[3] +  " which exists " + pair.Value + " times in the model, was not found in the generated Architecture Graph.");
                    }
                }
            }
        }

        private void updateDictionariesWithPrimitive(Tile intTile, Tile primitive)
        {
            if (primitive != null)
            {
                if (!primitiveTiles.Keys.Contains(primitive))
                    primitiveTiles.Add(primitive, primitiveTiles.Count());

                interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(primitiveTiles[primitive]);
            }
            else
                // flag to recognize non-existing primitive is -999
                interconnectToPrimitiveTiles[interconnectTiles[intTile]].Add(-999);
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
            
            if(IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB) || IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.BRAM) || IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.DSP) || hdio.IsMatch(tile.Location) || cm_final_identifier.IsMatch(tile.Location))
                return tile;

            return null;
        }

        private void AddWireToWirelist(Tile startTile, WireList wirelist, Wire startWire, short xIncr, short yIncr, Wire endWire, Tile endTile)
        {
            // we need to find the primitive number based on the endTile and the column resource string it is ending on
            string blockString = resourceStringChopped[endTile.LocationX];
            int primitiveNumber = -999;

            int index = 0;
            // some of these cases are logically hardcoded for ultrascale. 
            foreach(char c in blockString)
            {
                switch(c)
                {
                    case 's':
                        {
                            // interconnect is always in the middle (0, 1, 2) of a column
                            index++;
                            if (IdentifierManager.Instance.IsMatch(endTile.Location, IdentifierManager.RegexTypes.Interconnect))
                                primitiveNumber = 1;
                            break;
                        }
                    case 'L':
                        {
                            // check if CLB is left or right - it will always be adacent, so 0 or 2.
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
                            // check if CLB is left or right - it will always be adacent, so 0 or 2.
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
                        // skip over subinterconnect tiles
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
                // if primitive not found, check in irregular tiles. If not found, add the irregular tile, because we need an entry to represent where this wire is ending.
                if (!irregularTiles.ContainsKey(endTile))
                    irregularTiles.Add(endTile, irregularTiles.Count() + irregularTilesHashcodeShift);
                
                primitiveNumber = irregularTiles[endTile];
            }

            PrimitiveWire wire = new PrimitiveWire(startWire.LocalPipKey, endWire.PipOnOtherTileKey, startWire.LocalPipIsDriver, xIncr, yIncr, primitiveNumber);
            wirelist.Add(wire);

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
            foreach (Wire w in tile.WireList)
            {
                Tile target = Navigator.GetDestinationByWire(tile, w);

                short xIncr = (short)(target.LocationX - tile.LocationX);
                short yIncr = (short)(target.LocationY - tile.LocationY);

                if (checkForSubinterconnects && IdentifierManager.Instance.IsMatch(target.Location, IdentifierManager.RegexTypes.SubInterconnect))
                {
                    Tile subInterconnect = target;

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
