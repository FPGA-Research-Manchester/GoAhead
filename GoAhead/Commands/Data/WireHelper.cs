using GoAhead.Code;
using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GoAhead.Commands.Data
{
    class WireHelper
    {
        #region Include Flags
        public enum IncludeFlag { UTurnWires, SingleStopoverArcs, BlockStopoverPorts, BELOutWires, BELInWires, WiresTrajectoriesData, IncomingWires }

        private static Dictionary<IncludeFlag, Dictionary<string, bool>> includeFlags =
            new Dictionary<IncludeFlag, Dictionary<string, bool>>();

        public static void AddIncludeFlag(IncludeFlag flagType, string family, bool flag)
        {
            if (!includeFlags.ContainsKey(flagType)) includeFlags.Add(flagType, new Dictionary<string, bool>());
            includeFlags[flagType][family] = flag;
        }

        public static bool GetIncludeFlag(IncludeFlag flagType)
        {
            if (!includeFlags.ContainsKey(flagType)) return false;

            string family = FPGA.FPGA.Instance.Family.ToString();
            if (!includeFlags[flagType].ContainsKey(family)) return false;

            return includeFlags[flagType][family];
        }
        #endregion

        private enum WireType { Out, In }

        public static string keyword_outpin = "outpin";
        public static string keyword_inpin = "inpin";

        public bool IsBELOutPip(Tile tile, string pip)
        {
            return IsBELPiP(tile, pip, WireType.Out);
        }

        public bool IsBELInPip(Tile tile, string pip)
        {
            return IsBELPiP(tile, pip, WireType.In);
        }

        private bool IsBELPiP(Tile tile, string pip, WireType wireType)
        {
            PrepareRegexPairs(wireType);

            if (!FindMatchingRegexPairsForPip(pip, wireType)) return false;

            PrepareTile(tile);

            return ExistsMatchingBELForPip(tile, pip, wireType);
        }

        private readonly Tuple<IdentifierManager.RegexTypes, Dictionary<WireType, IdentifierManager.RegexTypes>> typePair =
            new Tuple<IdentifierManager.RegexTypes, Dictionary<WireType, IdentifierManager.RegexTypes>>
            (IdentifierManager.RegexTypes.BEL, new Dictionary<WireType, IdentifierManager.RegexTypes>
            {
                { WireType.Out, IdentifierManager.RegexTypes.BEL_outwire },
                { WireType.In, IdentifierManager.RegexTypes.BEL_inwire }
            });

        private readonly Dictionary<WireType, List<Tuple<Regex, Regex>>> regexPairs =
            new Dictionary<WireType, List<Tuple<Regex, Regex>>>
            {
                { WireType.Out, new List<Tuple<Regex, Regex>>() },
                { WireType.In, new List<Tuple<Regex, Regex>>() }
            };

        // Initialize regex pairs the first time we call this for each wire type
        private void PrepareRegexPairs(WireType wireType)
        {
            if (regexPairs[wireType].Count() == 0)
            {
                foreach (var pair in IdentifierManager.Instance.GetRegexMultipairList(typePair.Item1, typePair.Item2[wireType]))
                {
                    regexPairs[wireType].Add(new Tuple<Regex, Regex>(
                        new Regex(pair.Item1, RegexOptions.Compiled),
                        new Regex(pair.Item2, RegexOptions.Compiled)));
                }
            }
        }
        
        private List<Dictionary<string, Dictionary<WireType, HashSet<string>>>> BELsAndTheirPips =
            new List<Dictionary<string, Dictionary<WireType, HashSet<string>>>>();
        private Dictionary<Tile, int> TilesAndIndicesForBELs =
            new Dictionary<Tile, int>();

        // Find all BELs and their pips for tile the first time we come accross this tile
        private void PrepareTile(Tile tile)
        {
            if (!TilesAndIndicesForBELs.ContainsKey(tile))
            {
                Dictionary<string, Dictionary<WireType, HashSet<string>>> belsAndPips =
                    new Dictionary<string, Dictionary<WireType, HashSet<string>>>();
                foreach (Slice s in tile.Slices)
                {
                    foreach (string bel in s.PortMapping.SliceElements)
                    {
                        if (belsAndPips.ContainsKey(bel)) continue;

                        Dictionary<WireType, HashSet<string>> pips = new Dictionary<WireType, HashSet<string>>
                        {
                            { WireType.Out, new HashSet<string>() },
                            { WireType.In, new HashSet<string>() }
                        };
                        foreach (Port p in s.PortMapping.GetPorts(bel))
                        {
                            if (s.PortMapping.IsSliceOutPort(p)) pips[WireType.Out].Add(p.Name);
                            else pips[WireType.In].Add(p.Name);
                        }
                        belsAndPips.Add(bel, pips);
                    }
                }
                int i = GetIndexOfBelsAndPipsDictionary(belsAndPips);
                if (i == -1)
                {
                    BELsAndTheirPips.Add(belsAndPips);
                    i = BELsAndTheirPips.Count - 1;
                }
                TilesAndIndicesForBELs.Add(tile, i);
            }
        }

        private int GetIndexOfBelsAndPipsDictionary(Dictionary<string, Dictionary<WireType, HashSet<string>>> dic)
        {
            for(int i=0; i<BELsAndTheirPips.Count; i++)
            {
                if (!BELsAndTheirPips[i].Keys.SequenceEqual(dic.Keys)) continue;
                bool equals = true;
                foreach(var bel in BELsAndTheirPips[i])
                {
                    if(!bel.Value[WireType.In].SetEquals(dic[bel.Key][WireType.In]) ||
                       !bel.Value[WireType.Out].SetEquals(dic[bel.Key][WireType.Out]))
                    {
                        equals = false;
                        break;
                    }
                }
                if (!equals) continue;

                return i;
            }

            return -1;
        }

        // <regex pair index <group name, <bel group index, wire group index>>>
        private Dictionary<int, Dictionary<string, Tuple<int, int>>> matchedRegexPairsAndGroupIndices;

        // Find the regex pairs we can match the pip to
        private bool FindMatchingRegexPairsForPip(string pip, WireType wireType)
        {
            matchedRegexPairsAndGroupIndices = new Dictionary<int, Dictionary<string, Tuple<int, int>>>();
            
            for (int i = 0; i < regexPairs[wireType].Count; i++)
            {
                if (regexPairs[wireType][i].Item2.Match(pip).Success)
                {
                    string keyword = wireType == WireType.Out ? keyword_outpin : keyword_inpin;
                    bool keywordPresent = regexPairs[wireType][i].Item2.GetGroupNames().Contains(keyword);

                    List<string> belGroupNames = new List<string>(regexPairs[wireType][i].Item1.GetGroupNames());
                    belGroupNames.Remove(regexPairs[wireType][i].Item1.GroupNameFromNumber(0));

                    List<string> wireGroupNames = new List<string>(regexPairs[wireType][i].Item2.GetGroupNames());
                    wireGroupNames.Remove(regexPairs[wireType][i].Item2.GroupNameFromNumber(0));
                    if (keywordPresent) wireGroupNames.Remove(keyword);               

                    if(HelperMethodsLibrary.CompareListsIgnoringOrder(belGroupNames, wireGroupNames))
                    {
                        matchedRegexPairsAndGroupIndices.Add(i, new Dictionary<string, Tuple<int, int>>());
                        foreach(var groupName in belGroupNames)
                        {
                            Tuple<int, int> tuple = new Tuple<int, int>(
                                regexPairs[wireType][i].Item1.GroupNumberFromName(groupName),
                                regexPairs[wireType][i].Item2.GroupNumberFromName(groupName));
                            matchedRegexPairsAndGroupIndices[i].Add(groupName, tuple);
                        }
                    }                    
                }
            }

            return matchedRegexPairsAndGroupIndices.Count > 0;
        }

        // Returns true as soon as a matching BEL is found, false if not found
        private bool ExistsMatchingBELForPip(Tile tile, string pip, WireType wireType)
        {
            foreach(var regexPair in matchedRegexPairsAndGroupIndices)
            {
                string keyword = wireType == WireType.Out ? keyword_outpin : keyword_inpin;
                int pinGroupIndex = regexPairs[wireType][regexPair.Key].Item2.GroupNumberFromName(keyword);
                foreach (var belData in BELsAndTheirPips[TilesAndIndicesForBELs[tile]])
                {
                    Match belMatch = regexPairs[wireType][regexPair.Key].Item1.Match(belData.Key);
                    if (!belMatch.Success) continue;
                    Match wireMatch = regexPairs[wireType][regexPair.Key].Item2.Match(pip);
                    if (!wireMatch.Success) continue; // This continue should never happen !
                    if (pinGroupIndex != -1) if (!belData.Value[wireType].Contains(wireMatch.Groups[pinGroupIndex].Value)) continue;

                    bool allGroupsMatch = true;
                    foreach(var group in regexPair.Value)
                    {
                        allGroupsMatch = allGroupsMatch &&
                            (belMatch.Groups[group.Value.Item1].Value == wireMatch.Groups[group.Value.Item2].Value);
                    }
                    if (allGroupsMatch)
                    {
                        AddStopoverArcData(tile, belData.Key, pip, wireType);
                        return true;
                    }
                }
            }

            return false;
        }

        // <tile, <driver bel, {<inpins>, <outpins>}>>
        private Dictionary<Tile, Dictionary<string, List<string>[]>> stopoverArcs =
            new Dictionary<Tile, Dictionary<string, List<string>[]>>();
        private List<Tuple<Regex, Regex>> BELsArcsRegex = null;

        private void AddStopoverArcData(Tile tile, string bel, string pip, WireType wireType)
        {
            int index = 0;
            List<string> driverBELs = new List<string>();

            // Initialize regex pairs the first time we come here
            if(BELsArcsRegex == null)
            {
                BELsArcsRegex = new List<Tuple<Regex, Regex>>();
                try
                {
                    foreach (var belsPair in IdentifierManager.Instance.GetRegexMultipairList(
                    IdentifierManager.RegexTypes.BEL, IdentifierManager.RegexTypes.BEL))
                    {
                        BELsArcsRegex.Add(new Tuple<Regex, Regex>(
                            new Regex(belsPair.Item1, RegexOptions.Compiled),
                            new Regex(belsPair.Item2, RegexOptions.Compiled)));
                    }
                }
                catch (ArgumentException) { };
            }

            // If it's an outwire then we need to find the corresponding driver BELs
            if(wireType == WireType.Out)
            {
                index = 1;
                foreach (var regexPair in BELsArcsRegex)
                {
                    Match outM = regexPair.Item2.Match(bel);
                    if (!outM.Success) continue;

                    foreach (string otherBEL in BELsAndTheirPips[TilesAndIndicesForBELs[tile]].Keys)
                    {
                        Match inM = regexPair.Item1.Match(otherBEL);
                        if (inM.Success) driverBELs.Add(otherBEL);
                    }
                }
            }

            if (driverBELs.Count == 0 && GetIncludeFlag(IncludeFlag.SingleStopoverArcs)) driverBELs.Add(bel);

            // Store the pip under each driver bel
            foreach(string dbel in driverBELs)
            {
                if (!stopoverArcs.ContainsKey(tile))
                    stopoverArcs.Add(tile, new Dictionary<string, List<string>[]>());
                if (!stopoverArcs[tile].ContainsKey(dbel))
                    stopoverArcs[tile].Add(dbel, new List<string>[2] { new List<string>(), new List<string>() });

                stopoverArcs[tile][dbel][index].Add(pip);
            }            
        }

        public void ProcessStopoverArcs()
        {
            foreach(KeyValuePair<Tile, Dictionary<string, List<string>[]>> tile in stopoverArcs)
            {
                foreach(KeyValuePair<string, List<string>[]> bel in tile.Value)
                {
                    foreach(string inpip in bel.Value[0])
                    {
                        foreach(string outpip in bel.Value[1])
                        {
                            Port inPort = new Port(inpip);
                            Port outPort = new Port(outpip);

                            // Add to switch matrix
                            tile.Key.SwitchMatrix.Add(inPort, outPort);

                            // Fix wires for new sm entry
                            foreach (Wire wire in tile.Key.WireList.Where(w => w.LocalPip == outpip)) 
                            {
                                wire.m_localPipIsDriver = true;
                            }

                            if (GetIncludeFlag(IncludeFlag.BlockStopoverPorts))
                            {
                                tile.Key.BlockPort(inPort, Tile.BlockReason.Stopover);
                                tile.Key.BlockPort(outPort, Tile.BlockReason.Stopover);
                            }
                        }
                    }
                }
            }
        }
    }
}
