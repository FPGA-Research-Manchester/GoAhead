using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Code;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    class DesignRuleChecker
    {
        public static List<Tile> GetPossibleMacroPlacementsInSelection(string macroName)
        {
            List<Tile> possiblePlacements = new List<Tile>();

            LibraryElement libElement = Library.Instance.GetElement(macroName);

            foreach (Tile anchor in TileSelectionManager.Instance.GetSelectedTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
            {
                StringBuilder errorList = null;
                bool validPlacement = CheckLibraryElementPlacement(anchor, libElement, out errorList);
                if (validPlacement)
                {
                    possiblePlacements.Add(anchor);
                }
            }

            return possiblePlacements;
        }

        public static List<Tile> GetPossibleLibraryElementPlacements(string libraryElementName)
        {
            List<Tile> possiblePlacements = new List<Tile>();

            LibraryElement libElement = Library.Instance.GetElement(libraryElementName);

            foreach (Tile anchor in FPGA.FPGA.Instance.GetAllTiles().Where(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB)))
            {
                StringBuilder errorList = null;
                bool validPlacement = CheckLibraryElementPlacement(anchor, libElement, out errorList);
                if (validPlacement)
                {
                    possiblePlacements.Add(anchor);
                }
            }

            return possiblePlacements;
        }

        public static bool CheckLibraryElementPlacement(Tile anchor, LibraryElement libElement, out StringBuilder errorList)
        {
            errorList = new StringBuilder();
            int errorCountLength = 512;

            // check relocation
            bool relocationPossible = libElement.IsRelocationPossible(anchor, out errorList);
            if (!relocationPossible)
            {
                // if relocation fails, stop trying
                errorList.AppendLine("Error during relocation. Can not relocate all tiles. Does the module footprint match");
                return false;
            }

            foreach (Tuple<Instance, Tile> instanceTile in libElement.GetInstanceTiles(anchor, libElement))
            {
                if (errorList.Length > errorCountLength)
                {
                    break;
                }

                if (instanceTile.Item1 == null && instanceTile.Item2 == null)
                {
                    errorList.AppendLine("Left FPGA during macro placement form anchor (error during relocation)");
                    continue;
                }

                Tile targetTile = instanceTile.Item2;

                bool twoCLBS = IdentifierManager.Instance.IsMatch(targetTile.Location, IdentifierManager.RegexTypes.CLB) &&
                   IdentifierManager.Instance.IsMatch(instanceTile.Item1.Location, IdentifierManager.RegexTypes.CLB);

                // target tile type must match: remove digits from location string and expect equality
                string targetLocationWithoutDigits = Regex.Replace(targetTile.Location, @"\d", "");
                string instLocationWithoutDigits = Regex.Replace(instanceTile.Item1.Location, @"\d", "");
                if (!targetLocationWithoutDigits.Equals(instLocationWithoutDigits) && !twoCLBS)
                {
                    errorList.AppendLine("Tile type mismatch. Can not place " + instanceTile.Item1.Location + " at " + targetTile.Location);
                }

                if (instanceTile.Item1.SliceNumber < targetTile.Slices.Count)
                {
                    Slice targetSlice = targetTile.Slices[(int)instanceTile.Item1.SliceNumber];

                    string requiredSliceType = instanceTile.Item1.SliceType;
                    string targetSliceType = targetSlice.SliceType;

                    // slice type must be compatible
                    // compare parameterizable slice types
                    bool sliceTypesAreCompatible = SliceCompare.Instance.Matches(requiredSliceType, targetSliceType);

                    // if not match was found (e.g. due to missing or incomplete init.goa), try the hardcaoded comparsion
                    if (!sliceTypesAreCompatible)
                    {
                        if (requiredSliceType.Equals(targetSliceType)) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEX") && targetSliceType.Equals("SLICEX")) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEX") && targetSliceType.Equals("SLICEM")) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEX") && targetSliceType.Equals("SLICEL")) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEL") && targetSliceType.Equals("SLICEX")) { sliceTypesAreCompatible = false; }
                        else if (requiredSliceType.Equals("SLICEL") && targetSliceType.Equals("SLICEM")) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEL") && targetSliceType.Equals("SLICEL")) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEM") && targetSliceType.Equals("SLICEX")) { sliceTypesAreCompatible = false; }
                        else if (requiredSliceType.Equals("SLICEM") && targetSliceType.Equals("SLICEM")) { sliceTypesAreCompatible = true; }
                        else if (requiredSliceType.Equals("SLICEM") && targetSliceType.Equals("SLICEL")) { sliceTypesAreCompatible = false; }
                        else if (targetSliceType.StartsWith(requiredSliceType)) { sliceTypesAreCompatible = true; }
                    };
                    if (!sliceTypesAreCompatible)
                    {
                        errorList.AppendLine("Incompatible Slice Type found: Required is " + requiredSliceType + " but found " + targetSliceType + " at " + targetTile.Location);
                    }

                    // check if slice is still free
                    if (targetSlice.Usage != FPGATypes.SliceUsage.Free)
                    {
                        errorList.AppendLine("Slice " + targetSlice + " is used already");
                    }
                }
                else
                {
                    errorList.AppendLine("Expecting " + (instanceTile.Item1.SliceNumber + 1) + " slices " + targetTile.Location + ". Only found " + targetTile.Slices.Count);
                }
            }

            try
            {
                // check if all ports are free
                foreach (Tuple<Tile, List<Port>> tilePortListTupel in libElement.GetPortsToBlock(anchor))
                {
                    foreach (Port portToBlock in tilePortListTupel.Item2)
                    {
                        if (errorList.Length > errorCountLength)
                        {
                            break;
                        }

                        // Port may not be blocked and must exist
                        if (tilePortListTupel.Item1.IsPortBlocked(portToBlock))
                        {
                            errorList.AppendLine("Port " + portToBlock + " on tile " + tilePortListTupel.Item1.Location + " is used already");
                        }
                        if (!tilePortListTupel.Item1.SwitchMatrix.Contains(portToBlock))
                        {
                            errorList.AppendLine("Port " + portToBlock + " on tile " + tilePortListTupel.Item1.Location + " does not exist");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorList.AppendLine(e.Message);
            }

            // error
            return (errorList.Length == 0);
        }
    }
}