using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL;
using GoAhead.Objects;

namespace GoAhead.FPGA
{
    [Serializable]
    public partial class FPGATypes
    {
        public enum BackendType { ISE = 0, Vivado = 1, Altera = 2, All = 10 }

        public enum FPGAFamily
        {
            Undefined = 0,
            Virtex4 = 1, Virtex5 = 2, Virtex6 = 3, Spartan6 = 4, Kintex7 = 5, Spartan3 = 6, Artix7 = 7, Zynq = 8, Virtex2 = 9,
            StratixV = 10, CycloneIVE = 11, UltraScale = 12, Virtex7 = 13
        }

        public enum PortDirection { In = 0, Out = 1 }

        public enum Direction { North = 0, South = 1, East = 2, West = 3 }

        public enum Placement { UpperLeft = 0, UpperRight = 1, LowerLeft = 2, LowerRight = 3 }

        public enum SliceUsage { Free = 0, Macro = 1, Blocker = 2 }

        public enum InterfaceDirection { East = 0, West = 1 }

        public static Direction GetDirectionFromString(string direction)
        {
            switch (direction.ToLower())
            {
                case "south": { return Direction.South; }
                case "north": { return Direction.North; }
                case "east": { return Direction.East; }
                case "west": { return Direction.West; }
                default: { throw new ArgumentException("Can not hanlde direction " + direction); }
            }
        }

        public static string GetSringFromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.East: { return "East"; }
                case Direction.North: { return "Borth"; }
                case Direction.South: { return "South"; }
                case Direction.West: { return "West"; }
                default: { throw new ArgumentException("Can not hanlde direction " + direction); }
            }
        }

        public static PortDirection GetPortDirectionFromString(string direction)
        {
            switch (direction.ToLower())
            {
                case "in": { return PortDirection.In; }
                case "out": { return PortDirection.Out; }
                default: { throw new ArgumentException("Can not hanlde direction " + direction); }
            }
        }

        public static void AssertBackendType(params FPGATypes.BackendType[] expectedBackendTypes)
        {
            if (!expectedBackendTypes.Contains(FPGA.Instance.BackendType))
            {
                throw new ArgumentException("Backend type of the currently loaded FPGA must be in " + expectedBackendTypes + ", but it is " + FPGA.Instance.BackendType);
            }
        }

        /// <summary>
        /// Get X Y values from e.g CLB_X3Y6
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static bool GetXYFromIdentifier(string identifier, out int x, out int y)
        {
            if (m_sliceMatch.IsMatch(identifier))
            {
                string[] atoms = identifier.Split('X', 'Y');
                x = int.Parse(atoms[atoms.Length - 2]);
                y = int.Parse(atoms[atoms.Length - 1]);
                return true;
            }
            else
            {
                x = -1;
                y = -1;
                return false;
            }
        }

        /// <summary>
        /// Some tile are named CLBLL, others CLBLM. During relocation we derive by modifieing the x and y values
        /// of a location string tile names that requirer LL->LM or vice versa.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="continueReplacment"></param>
        /// <returns></returns>
        public static bool ResolveLMIdentifier(string identifier, Predicate<string> continueReplacment, out string result)
        {
            result = "";
            // CL naming fun
            string resolvedIdentifier = identifier;
            int replacements = 0;

            //!FPGA.Instance.Contains(resolvedLocation)
            while (continueReplacment(resolvedIdentifier))
            {
                switch (FPGA.Instance.Family)
                {
                    case (FPGAFamily.Spartan6):
                        {
                            // locations and some ports
                            if (Regex.IsMatch(resolvedIdentifier, "L_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "L_", "M_"); }
                            else if (Regex.IsMatch(resolvedIdentifier, "M_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "M_", "L_"); }
                            // ports only
                            if (Regex.IsMatch(resolvedIdentifier, "^X_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "^X_", "XX_"); }
                            else if (Regex.IsMatch(resolvedIdentifier, "^XL_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "^XL_", "M_"); }
                            else if (Regex.IsMatch(resolvedIdentifier, "^M_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "^M_", "XL_"); }
                            else if (Regex.IsMatch(resolvedIdentifier, "^XX_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "^XX_", "X_"); }
                            replacements++;
                            break;
                        }
                    case (FPGAFamily.Virtex5):
                    case (FPGAFamily.Virtex6):
                        {
                            if (resolvedIdentifier.Contains("_LL_")) { resolvedIdentifier = resolvedIdentifier.Replace("_LL_", "_L_"); }
                            else if (resolvedIdentifier.Contains("BLM_M_")) { resolvedIdentifier = resolvedIdentifier.Replace("BLM_M_", "BLL_L_"); }
                            else if (resolvedIdentifier.Contains("BLM_")) { resolvedIdentifier = resolvedIdentifier.Replace("BLM_", "BLL_"); }
                            else if (resolvedIdentifier.Contains("BLL_")) { resolvedIdentifier = resolvedIdentifier.Replace("BLL_", "BLM_"); }
                            replacements++;
                            break;
                        }
                    case (FPGAFamily.Artix7):
                        {
                            if (Regex.IsMatch(resolvedIdentifier, "_L_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "_L_", "_R_"); }
                            else if (Regex.IsMatch(resolvedIdentifier, "_R_")) { resolvedIdentifier = Regex.Replace(resolvedIdentifier, "_R_", "_L_"); }
                            replacements++;
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException("ResolveLMLocation not implemented for FPGA family " + FPGA.Instance.Family);
                        }
                }
                // replacements without success, return useless tile
                if (replacements > 5)
                {
                    return false;
                    //return "";
                    //throw new NotImplementedException("Could not relocated " + identifier);
                }
            }
            result = resolvedIdentifier;
            return true;
            //return resolvedIdentifier;
        }

        public static XDLPip RelocatePip(Tile targetLocation, XDLPip pip, XDLNet targetNet)
        {
            if (!IdentifierManager.Instance.IsMatch(targetLocation.Location, IdentifierManager.RegexTypes.CLB))
            {
                throw new ArgumentException("Expecting CLB");
            }

            Tile referenceTile = FPGA.Instance.GetAllTiles().First(t => t.SwitchMatrix.Contains(pip.From, pip.To));
            if (referenceTile == null)
            {
                throw new ArgumentException("Could not relocate " + pip.ToString() + " to tile " + targetLocation.Location);
            }

            int fromSliceIndex = -1;
            int toSliceIndex = -1;
            foreach (Slice s in referenceTile.Slices)
            {
                if (s.PortMapping.Contains(new Port(pip.From)))
                {
                    fromSliceIndex = referenceTile.GetSliceNumberByName(s.SliceName);
                }
                if (s.PortMapping.Contains(new Port(pip.To)))
                {
                    toSliceIndex = referenceTile.GetSliceNumberByName(s.SliceName);
                }
            }

            if (fromSliceIndex != toSliceIndex)
            {
            }

            Tuple<Port, Port> drivingArc = null;

            // route into slice
            if (fromSliceIndex == -1 && toSliceIndex != -1)
            {
                int lastUnderscore = pip.To.LastIndexOf("_");
                string suffix = pip.To.Substring(lastUnderscore, pip.To.Length - lastUnderscore);

                Port sliceInPort = targetLocation.Slices[toSliceIndex].PortMapping.Ports.FirstOrDefault(p => p.Name.EndsWith(suffix));
                drivingArc = targetLocation.SwitchMatrix.GetAllArcs().FirstOrDefault(t => t.Item2.Name.Equals(sliceInPort.Name));
            }
            else if (fromSliceIndex != -1 && toSliceIndex == -1)
            {
                int lastUnderscore = pip.From.LastIndexOf("_");
                string suffix = pip.From.Substring(lastUnderscore, pip.From.Length - lastUnderscore);

                Port sliceOutPort = targetLocation.Slices[fromSliceIndex].PortMapping.Ports.FirstOrDefault(p => p.Name.EndsWith(suffix));
                // IsSlicePort: toSliceIndex is not found in slice, prevent using arcs that route in SLICE, i.e., prevent that CLBLL_L_AA -> IMUX get mapped to CLBLL_L_AA -> AMUX
                foreach (Tuple<Port, Port> candicate in targetLocation.SwitchMatrix.GetAllArcs().Where(t => t.Item1.Name.Equals(sliceOutPort.Name)))
                {
                    string prefix = candicate.Item2.Name.Contains(" ") ? candicate.Item2.Name.Substring(0, candicate.Item2.Name.IndexOf(" ")) : candicate.Item2.Name;
                    if (!targetLocation.IsSlicePort(prefix))
                    {
                        drivingArc = candicate;
                        break;
                    }
                }

                //drivingArc = targetLocation.SwitchMatrix.GetAllArcs().FirstOrDefault(t => t.Item1.Name.Equals(sliceOutPort.Name) && !referenceTile.IsSlicePort(t.Item2.Name));
            }
            else if (fromSliceIndex != -1 && toSliceIndex != -1)
            {
                int lastUnderscoreTo = pip.To.LastIndexOf("_");
                string toSuffix = pip.To.Substring(lastUnderscoreTo, pip.To.Length - lastUnderscoreTo);
                Port toPort = targetLocation.Slices[toSliceIndex].PortMapping.Ports.FirstOrDefault(p => p.Name.EndsWith(toSuffix));

                int lastUnderscoreFrom = pip.From.LastIndexOf("_");
                string fromSuffix = pip.From.Substring(lastUnderscoreFrom, pip.From.Length - lastUnderscoreFrom);
                Port fromPort = targetLocation.Slices[fromSliceIndex].PortMapping.Ports.FirstOrDefault(p => p.Name.EndsWith(fromSuffix));

                drivingArc = targetLocation.SwitchMatrix.GetAllArcs().FirstOrDefault(t => t.Item1.Name.Equals(fromPort.Name) && t.Item2.Name.StartsWith(toPort.Name));
            }
            else
            {
                throw new ArgumentException("Could not relocate " + pip.ToString() + " to tile " + targetLocation.Location);
            }

            // cut of routethrough
            string relocatedToPortName = drivingArc.Item2.Name;
            if (relocatedToPortName.Contains(" "))
            {
                relocatedToPortName = relocatedToPortName.Substring(0, relocatedToPortName.IndexOf(" "));
            }
            XDLPip result = new XDLPip(targetLocation.Location, drivingArc.Item1.Name, pip.Operator, relocatedToPortName);
            return result;

            /*
             * int indexOfFirstUnderScore = pip.From.IndexOf("_");
            String fromSuffix = pip.From.Substring(indexOfFirstUnderScore, pip.From.Length - indexOfFirstUnderScore);

            indexOfFirstUnderScore = pip.To.LastIndexOf("_");
            String toSuffix = pip.To.Substring(indexOfFirstUnderScore, pip.To.Length - indexOfFirstUnderScore);

            int tries = 0;
            while (tries < 10)
            {
                foreach (Port from in targetLocation.SwitchMatrix.Ports.Where(p => p.Name.EndsWith(fromSuffix) && targetLocation.GetSliceNumberByPortName(p) == fromSliceIndex))
                {
                    foreach (Port p in targetLocation.SwitchMatrix.GetDrivenPorts(from).Where(p => targetLocation.GetSliceNumberByPortName(p) == toSliceIndex))
                    {
                        String portNameWithoutRouteThrough = p.Name;
                        if (p.Name.Contains(" "))
                        {
                            portNameWithoutRouteThrough = p.Name.Substring(0, p.Name.IndexOf(" "));
                        }
                        if (portNameWithoutRouteThrough.EndsWith(toSuffix))
                        {
                            if (targetLocation.SwitchMatrix.Contains(from.Name, p.Name))
                            {
                                XDLPip result = new XDLPip(targetLocation.Location, from.Name, pip.Operator, portNameWithoutRouteThrough);
                                return result;
                            }
                        }
                    }
                }
                if (fromSuffix.Length > 1)
                {
                    fromSuffix = fromSuffix.Substring(1, fromSuffix.Length - 1);
                }
                tries++;
            }
                         */
            throw new ArgumentException("Could not relocate " + pip.ToString() + " to tile " + targetLocation.Location);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tileFilter"></param>
        /// <param name="width">the width of a ram block</param>
        /// <param name="height">the height of a ram block</param>
        /// <returns>valididate the out data</returns>
        public static bool GetRamBlockSize(string tileFilter, out int width, out int height, out TileSet ramTiles)
        {
            ramTiles = new TileSet();

            BRAMDSPSettingsManager.Instance.GetBRAMWidthAndHeight(out width, out height);

            Regex filter = new Regex(tileFilter, RegexOptions.Compiled);

            foreach (Tile ram in FPGA.Instance.GetAllTiles().Where(t => filter.IsMatch(t.Location)))
            {
                if (IdentifierManager.Instance.IsMatch(ram.Location, IdentifierManager.RegexTypes.BRAM) || IdentifierManager.Instance.IsMatch(ram.Location, IdentifierManager.RegexTypes.DSP))
                {
                    ramTiles.Add(ram);
                }
            }

            // no drawing if we find less than 2 tles
            return ramTiles.Count >= 2;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tileFilter"></param>
        /// <param name="width">the width of a ram block</param>
        /// <param name="height">the height of a ram block</param>
        /// <returns>valididate the out data</returns>
        public static bool GetRamBlockSize(Regex tileFilter, out int width, out int height, out TileSet ramTiles)
        {
            ramTiles = new TileSet();

            BRAMDSPSettingsManager.Instance.GetBRAMWidthAndHeight(out width, out height);

            foreach (Tile ram in FPGA.Instance.GetAllTiles().Where(t => tileFilter.IsMatch(t.Location)))
            {
                if (Regex.IsMatch(ram.Location, IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.BRAM, IdentifierManager.RegexTypes.DSP)))
                {
                    ramTiles.Add(ram);
                }
            }

            // no drawing if we find less than 2 tles
            return ramTiles.Count >= 2;
        }

        /// <summary>
        /// Get the Interconnect Tile that belongs to the clbConnectLocation
        /// For UltraScale, also get Interconnect tile for the INT_INTF type of tiles.
        /// </summary>
        /// <param name="clbLocatioin"></param>
        /// <returns></returns>
        public static Tile GetInterconnectTile(Tile clb)
        {
            switch (FPGA.Instance.Family)
            {
                case FPGAFamily.Spartan6:
                case FPGAFamily.Virtex4:
                case FPGAFamily.Virtex5:
                case FPGAFamily.Virtex6:
                    {
                        // go one left
                        return FPGA.Instance.GetTile(clb.TileKey.X - 1, clb.TileKey.Y);
                    }
                case FPGAFamily.Spartan3:
                case FPGAFamily.Virtex2:
                    {
                        // dont move
                        return clb;
                    }
                case FPGAFamily.UltraScale:
                    {
                        if (IsOrientedMatch(clb.Location, IdentifierManager.RegexTypes.CLB_left))
                        {
                            return FPGA.Instance.GetTile(clb.TileKey.X + 1, clb.TileKey.Y);
                        }
                        else if (IsOrientedMatch(clb.Location, IdentifierManager.RegexTypes.CLB_right))
                        {
                            return FPGA.Instance.GetTile(clb.TileKey.X - 1, clb.TileKey.Y);
                        }
                        //Check to the right of the INT_INTF.
                        else if (IdentifierManager.Instance.IsMatch(clb.Location, IdentifierManager.RegexTypes.SubInterconnect)
                                 && IdentifierManager.Instance.IsMatch(FPGA.Instance.GetTile(clb.TileKey.X + 1, clb.TileKey.Y).Location, IdentifierManager.RegexTypes.Interconnect))
                        {
                            //Move to the right interconnect.
                            return FPGA.Instance.GetTile(clb.TileKey.X + 1, clb.TileKey.Y);
                        }
                        else if (IdentifierManager.Instance.IsMatch(clb.Location, IdentifierManager.RegexTypes.SubInterconnect)
                                 && IdentifierManager.Instance.IsMatch(FPGA.Instance.GetTile(clb.TileKey.X - 1, clb.TileKey.Y).Location, IdentifierManager.RegexTypes.Interconnect))
                        {
                            //Move to the right interconnect.
                            return FPGA.Instance.GetTile(clb.TileKey.X - 1, clb.TileKey.Y);
                        }
                        else
                        {
                            throw new ArgumentException("GetInterconnectTile not implemented for tile " + clb.Location + " in " + FPGA.Instance.Family);
                        }
                    }
                case FPGAFamily.Kintex7:
                case FPGAFamily.Artix7:
                case FPGAFamily.Zynq:
                case FPGAFamily.Virtex7:
                    {
                        if (IsOrientedMatch(clb.Location, IdentifierManager.RegexTypes.CLB_left))
                        {
                            return FPGA.Instance.GetTile(clb.TileKey.X + 1, clb.TileKey.Y);
                        }
                        else if (IsOrientedMatch(clb.Location, IdentifierManager.RegexTypes.CLB_right))
                        {
                            return FPGA.Instance.GetTile(clb.TileKey.X - 1, clb.TileKey.Y);
                        }
                        else
                        {
                            throw new ArgumentException("GetInterconnectTile not implemented for tile " + clb.Location + " in " + FPGA.Instance.Family);
                        }
                    }
                case FPGAFamily.StratixV:
                case FPGAFamily.CycloneIVE:
                    {
                        // TODO
                        return clb;
                    }
                default:
                    {
                        throw new ArgumentException("GetInterconnectTile not implemented for " + FPGA.Instance.Family);
                    }
            }
        }

        /// <summary>
        /// Get the CLB that belongs to the interConnectLocation
        /// </summary>
        /// <param name="clbLocatioin"></param>
        /// <returns></returns>
        public static IEnumerable<Tile> GetCLTile(Tile interconnectTile)
        {
            switch (FPGA.Instance.Family)
            {
                case FPGAFamily.Spartan6:
                case FPGAFamily.Virtex4:
                case FPGAFamily.Virtex5:
                case FPGAFamily.Virtex6:
                    {
                        yield return FPGA.Instance.GetTile(interconnectTile.TileKey.X + 1, interconnectTile.TileKey.Y);
                        break;
                    }
                case FPGAFamily.UltraScale:
                    {
                        Tile left = FPGA.Instance.GetTile(interconnectTile.TileKey.X - 1, interconnectTile.TileKey.Y);
                        Tile right= FPGA.Instance.GetTile(interconnectTile.TileKey.X + 1, interconnectTile.TileKey.Y);
                        if(IdentifierManager.Instance.IsMatch(left.Location, IdentifierManager.RegexTypes.CLB))
                        {
                            yield return left;
                        }
                        if(IdentifierManager.Instance.IsMatch(right.Location, IdentifierManager.RegexTypes.CLB))
                        {
                            yield return right;
                        }
                        break;
                    }
                case FPGAFamily.Artix7:
                case FPGAFamily.Kintex7:
                case FPGAFamily.Virtex7:
                case FPGAFamily.Zynq:
                    {
                        if (IsOrientedMatch(interconnectTile.Location, IdentifierManager.RegexTypes.Interconnect_left))
                        {
                            yield return FPGA.Instance.GetTile(interconnectTile.TileKey.X - 1, interconnectTile.TileKey.Y);
                        }
                        else if (IsOrientedMatch(interconnectTile.Location, IdentifierManager.RegexTypes.Interconnect_right))
                        {
                            yield return FPGA.Instance.GetTile(interconnectTile.TileKey.X + 1, interconnectTile.TileKey.Y);
                        }
                        else
                        {
                            throw new ArgumentException("GetCLTile not implemented for tile " + interconnectTile.Location + " in " + FPGA.Instance.Family);
                        }
                        break;
                    }
                case FPGAFamily.StratixV:
                case FPGAFamily.CycloneIVE:
                    {
                        // TODO
                        yield return interconnectTile;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("GetCBTile not implemented for " + FPGA.Instance.Family);
                    }
            }
        }

        /// <summary>
        /// Get the INT_INTF that belongs to the interConnectLocation
        /// </summary>
        /// <param name="clbLocatioin"></param>
        /// <returns></returns>
        public static IEnumerable<Tile> GetSubInterconnectTile(Tile interconnectTile)
        {
            switch (FPGA.Instance.Family)
            {
                case FPGAFamily.Spartan6:
                case FPGAFamily.Virtex4:
                case FPGAFamily.Virtex5:
                case FPGAFamily.Virtex6:
                case FPGAFamily.Artix7:
                case FPGAFamily.Kintex7:
                case FPGAFamily.Virtex7:
                case FPGAFamily.Zynq:
                case FPGAFamily.StratixV:
                case FPGAFamily.CycloneIVE:
                    {
                        // TODO
                        yield return interconnectTile;
                        break;
                    }
                case FPGAFamily.UltraScale:
                    {
                        Tile left = FPGA.Instance.GetTile(interconnectTile.TileKey.X - 1, interconnectTile.TileKey.Y);
                        Tile right = FPGA.Instance.GetTile(interconnectTile.TileKey.X + 1, interconnectTile.TileKey.Y);
                        if (IdentifierManager.Instance.IsMatch(left.Location, IdentifierManager.RegexTypes.SubInterconnect))
                        {
                            yield return left;
                        }
                        if (IdentifierManager.Instance.IsMatch(right.Location, IdentifierManager.RegexTypes.SubInterconnect))
                        {
                            yield return right;
                        }
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("GetSubInterconnectTile not implemented for " + FPGA.Instance.Family);
                    }
            }
        }

        public static IEnumerable<LUTRoutingInfo> GetLUTRouting(Tile clb)
        {
            // go to left interconnet tile
            if (!IdentifierManager.Instance.IsMatch(clb.Location, IdentifierManager.RegexTypes.CLB))
            {
                throw new ArgumentException("GetPortTriplets may only be called for CLB, found: " + clb.Location);
            }
            Tile interConnect = GetInterconnectTile(clb);

            // "", in, out, lutIn
            foreach (Tuple<Port, Port> arc in interConnect.SwitchMatrix.GetAllArcs())
            {
                foreach (Location location in Navigator.GetDestinations(interConnect, arc.Item2).Where(loc => loc.Tile.Location.Equals(clb.Location)))
                {
                    LUTRoutingInfo info = new LUTRoutingInfo();
                    info.Port2 = arc.Item1;
                    info.Port3 = arc.Item2;
                    try
                    {
                        info.Port4 = clb.SwitchMatrix.GetFirstDrivenPort(location.Pip);
                    }
                    catch (ArgumentException) { }
                    
                    yield return info;
                }
            }

            foreach (Tuple<Port, Port> arc in clb.SwitchMatrix.GetAllArcs())
            {
                foreach (Location location in Navigator.GetDestinations(clb, arc.Item2).Where(loc => loc.Tile.Location.Equals(interConnect.Location)))
                {
                    foreach (Port p in interConnect.SwitchMatrix.GetDrivenPorts(location.Pip))
                    {
                        LUTRoutingInfo info = new LUTRoutingInfo();
                        info.Port1 = arc.Item1;
                        info.Port2 = location.Pip;
                        info.Port3 = p;
                        yield return info;
                    }
                }
            }

            // lutout, in, out,
        }

        /// <summary>
        /// Get the direction (in or our) from the given slice port
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static PortDirection GetDirection(Port slicePort)
        {
            Tile anyCLB = FPGA.Instance.GetAllTiles().FirstOrDefault(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB) && t.Slices.Count >= 2);

            foreach (Slice s in anyCLB.Slices)
            {
                if (s.PortMapping.IsSliceInPort(slicePort.Name))
                {
                    return PortDirection.In;
                }
                else if (s.PortMapping.IsSliceOutPort(slicePort.Name))
                {
                    return PortDirection.Out;
                }
            }

            throw new ArgumentException("Could not determine the direction of the slice port " + slicePort + " on tile " + anyCLB.Location);
        }

        // Global default values, used if an identifier is missing
        #region Default Values
        public static Regex ClbLeft = new Regex("(^CLB(L|M){2,2}_L)|(^CLE_M_X)", RegexOptions.Compiled);
        public static Regex ClbRight = new Regex("(^CLB(L|M){2,2}_R)|(^CLEL_R_X)", RegexOptions.Compiled);

        public static Regex IntLeft = new Regex("^INT_L", RegexOptions.Compiled);
        public static Regex IntRight = new Regex("^INT_R", RegexOptions.Compiled);

        public static Regex BRAMLeft = new Regex("^BRAM_L", RegexOptions.Compiled);
        public static Regex BRAMRight = new Regex("^BRAM_R", RegexOptions.Compiled);
        #endregion

        public static bool IsOrientedMatch(string block, IdentifierManager.RegexTypes type)
        {
            if (IdentifierManager.Instance.HasRegexp(type))
            {
                return IdentifierManager.Instance.IsMatch(block, type);
            }

            Regex defaultRegex = new Regex("");
            switch(type)
            {
                case IdentifierManager.RegexTypes.CLB_left:
                    defaultRegex = ClbLeft;
                    break;
                case IdentifierManager.RegexTypes.CLB_right:
                    defaultRegex = ClbRight;
                    break;
                case IdentifierManager.RegexTypes.Interconnect_left:
                    defaultRegex = IntLeft;
                    break;
                case IdentifierManager.RegexTypes.Interconnect_right:
                    defaultRegex = IntRight;
                    break;
                case IdentifierManager.RegexTypes.BRAM_left:
                    defaultRegex = BRAMLeft;
                    break;
                case IdentifierManager.RegexTypes.BRAM_right:
                    defaultRegex = BRAMRight;
                    break;
            }

            return defaultRegex.IsMatch(block);
        }

        public static bool fpgaTypeSupportsEastWestSwitchboxes(FPGAFamily family)
        {
            if (family == FPGAFamily.UltraScale)
                return true;

            return false;
        }

        /// <summary>
        /// SLICE_X0Y0 or RAMB8_X1Y7
        /// </summary>
        private static Regex m_sliceMatch = new Regex(@"_X\d+Y\d+", RegexOptions.Compiled);
    }

    public class LUTRoutingInfo
    {
        public Port Port1;
        public Port Port2;
        public Port Port3;
        public Port Port4;
    }
}