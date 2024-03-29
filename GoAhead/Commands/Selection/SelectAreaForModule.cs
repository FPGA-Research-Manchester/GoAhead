﻿using System;   
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Commands.XDLManipulation;
using GoAhead.Code.XDL;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Commands.Selection
{
    [CommandDescription(Description = "Find a placement for the reconfigurable area", Wrapper = true, Publish = true)]
    class FindPlacementForReconfigurableArea : Command
    {
        protected override void DoCommandAction()
        {
            // get resource requirments from module
            ResourceRequirements resourceRequirements = GetResourceRequirements();

            // select a whole line and check wether we found all resource types at least one 
            int sweepLineY = FindSweepLine();

            m_resStringInfo = new ResourceStringInfo();
            m_resStringInfo.InitResourceStrings(sweepLineY);

            List<AreaCandidate> candidates = new List<AreaCandidate>();
            for (int h = 1; h < Height; h++)
            {
                foreach (KeyValuePair<string, SelectionInfo> tupel in m_resStringInfo.EvaluatedResourceStrings)
                {        
                    if (ResourceMatch(resourceRequirements, tupel.Key, h))
                    {
                        AreaCandidate candidate = new AreaCandidate();
                        candidate.SelectionInfo = tupel.Value;
                        candidate.FragmentationInfo = GetFragmentation(tupel.Value, h, resourceRequirements);
                        candidate.Height = h;
                        // weiter: Center of mass ausrechnen
                        // 

                        candidates.Add(candidate);
                    }
                }
            }

            if (candidates.Count == 0)
            {
                throw new ArgumentException("No selection found");
            }

            int topN = 1;
            foreach (AreaCandidate result in candidates.OrderBy(c => c.FragmentationInfo.TotalFragmentation))
            {
                OutputManager.WriteOutput(result.ToString());
                if(topN > TopN)
                {
                    break;
                }
            }
        }

        private ResourceRequirements GetResourceRequirements()
        {           
            // get component wise maximum from files
            PrintComponentWiseMaximalResourceConsumption printCmd = new PrintComponentWiseMaximalResourceConsumption();
            printCmd.Files = XDLModules;
            CommandExecuter.Instance.Execute(printCmd);
            string output = printCmd.OutputManager.GetOutput();
            string[] atoms = output.Split(' ');

            int clbs = int.Parse(atoms[1]);
            int dsp = int.Parse(atoms[3]);
            int bram = int.Parse(atoms[5]);

            // TODO detect chains

            // store result
            ResourceRequirements unionRequirements = new ResourceRequirements();
            foreach (IdentifierManager.RegexTypes resourceType in m_resStringInfo.GetResourceTypes())
            {
                unionRequirements.Chains.Add(resourceType, new List<int>());
            }
            unionRequirements.Chains[IdentifierManager.RegexTypes.CLB].Add(clbs);
            unionRequirements.Chains[IdentifierManager.RegexTypes.DSP].Add(dsp);
            unionRequirements.Chains[IdentifierManager.RegexTypes.BRAM].Add(bram);
            
            /*
            Random rnd = new Random();
            ResourceRequirements unionRequirements = new ResourceRequirements();
            unionRequirements.Name = "union";
            foreach (IdentifierManager.RegexTypes resourceType in this.m_resStringInfo.GetResourceTypes())
            {
                unionRequirements.Chains.Add(resourceType, new List<int>());
                // max four chains of max lenght 3 each
                int numberOfChains = rnd.Next(1,3);
                for(int c=0;c<numberOfChains;c++)
                {
                    unionRequirements.Chains[resourceType].Add(rnd.Next(1,4));
                }
            }
            unionRequirements.Chains[IdentifierManager.RegexTypes.CLBRegex].Clear();
            unionRequirements.Chains[IdentifierManager.RegexTypes.CLBRegex].Add(rnd.Next(30));
            unionRequirements.XCenter = rnd.NextDouble() * (double)FPGA.FPGA.Instance.MaxX;
            unionRequirements.YCenter = rnd.NextDouble() * (double)FPGA.FPGA.Instance.MaxY;
            */
            return unionRequirements;
        }

        private bool ResourceMatch(ResourceRequirements resourceRequirements, string resourceString, int height)
        {
            Dictionary<IdentifierManager.RegexTypes, int> resources = GetResourceInArea(resourceString, height);

            foreach (IdentifierManager.RegexTypes resourceType in m_resStringInfo.GetResourceTypes())
            {
                // there is no such resource
                if (!resources.ContainsKey(resourceType) && resourceRequirements.GetResourceRequirement(resourceType) > 0)
                {
                    return false;
                }               
                // there are too less of them
                else if (resources[resourceType] < resourceRequirements.GetResourceRequirement(resourceType))
                {
                    return false;
                }
            }        

            // prepare bins
            Dictionary<IdentifierManager.RegexTypes, Dictionary<int, int>> bins = new Dictionary<IdentifierManager.RegexTypes, Dictionary<int, int>>();                        
            foreach (IdentifierManager.RegexTypes resourceType in m_resStringInfo.GetResourceTypes())
            {
                if (!bins.ContainsKey(resourceType))
                {
                    bins.Add(resourceType, new Dictionary<int,int>());
                }
                bins[resourceType] = GetBins(resourceType, resourceString, height);
            }
          

            // every chain (except CLBs) has to fit into resources (first fit)
            foreach (IdentifierManager.RegexTypes resourceType in m_resStringInfo.GetResourceTypes().Where(rt => rt != IdentifierManager.RegexTypes.CLB))
            {
                // copy!
                Dictionary<int, int> bin = new Dictionary<int, int>(bins[resourceType]);

                for (int i = 0; i < resourceRequirements.Chains[resourceType].Count; i++)
                {
                    bool binFound = false;
                    for (int b = 0; b < bin.Count; b++)
                    {
                        if (bin[b] - resourceRequirements.Chains[resourceType][i] >= 0)
                        {
                            bin[b] -= resourceRequirements.Chains[resourceType][i];
                            binFound = true;
                            break;
                        }
                    }
                    if (!binFound)
                    {
                        return false;
                    }
                }
            }            
          
            return true;
        }

        private Fragmentation GetFragmentation(SelectionInfo selInfo, int height, ResourceRequirements resourceRequirements)
        {
            Dictionary<IdentifierManager.RegexTypes, int> resources = GetResourceInArea(selInfo.ResourceString, height);

            Fragmentation result = new Fragmentation();
            foreach (IdentifierManager.RegexTypes resourceType in m_resStringInfo.GetResourceTypes())
            {
                result.ResourceTypeFragmentation[resourceType] = resources[resourceType] - resourceRequirements.GetResourceRequirement(resourceType);
            }
            
            return result;
        }

        private int FindSweepLine()
        {
            for (int y = FPGA.FPGA.Instance.MaxY; y >= 0; y--)
            {
                CommandExecuter.Instance.Execute(new ClearSelection());
                AddToSelectionXY addCmd = new AddToSelectionXY();
                addCmd.UpperLeftX = 0;
                addCmd.UpperLeftY = y;
                addCmd.LowerRightX = FPGA.FPGA.Instance.MaxX;
                addCmd.LowerRightY = y;
                CommandExecuter.Instance.Execute(addCmd);
                // no expand selection to end up at the buttom of BRAM block

                List<IdentifierManager.RegexTypes> resTypes = new List<IdentifierManager.RegexTypes>(m_resStringInfo.GetResourceTypes());
                Dictionary<IdentifierManager.RegexTypes, int> resourcesInLine =
                    TileSelectionManager.Instance.GetRessourcesInSelection(TileSelectionManager.Instance.GetSelectedTiles(), resTypes);
                if (resourcesInLine.Keys.Count == resTypes.Count && resourcesInLine[IdentifierManager.RegexTypes.CLB] > 2)
                {
                    // capture the row
                    return y;
                }
            }
            throw new ArgumentException("Could not find a line to sweep");
        }

        private Dictionary<IdentifierManager.RegexTypes, int> GetResourceInArea(string resourceString, int height)
        {
            Dictionary<IdentifierManager.RegexTypes, int> precalculated = m_resStringInfo.EvaluatedResourceStrings[resourceString].Resources;

            Dictionary<IdentifierManager.RegexTypes, int> result = new Dictionary<IdentifierManager.RegexTypes, int>();
            foreach (IdentifierManager.RegexTypes resourceType in precalculated.Keys)
            {
                result.Add(resourceType, precalculated[resourceType] * height);
            }
            return result;
        }

        private Dictionary<int, int> GetBins(IdentifierManager.RegexTypes resourceType, string resourceString, int height)
        {
            Dictionary<int, int> bins = new Dictionary<int, int>();
            char name = m_resStringInfo.GetCharForResourceType(resourceType);
            for(int i=0;i<resourceString.Count(c => c.Equals(name));i++)
            {
                bins[i] = height;
            }
            return bins;
        }

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        private ResourceStringInfo m_resStringInfo = new ResourceStringInfo();

        [Parameter(Comment = "The XDL-File (placed netlist) to read in")]
        public List<string> XDLModules = new List<string>();

        [Parameter(Comment = "The instance prefix that selects all instances for the union module")]
        public string InstancePrefix = "inst_mod";

        [Parameter(Comment = "The first TopN selections will be stored as user selections")]
        public int TopN = 1 ;

        [Parameter(Comment = "The name under which to store the resulting selections")]
        public string UserSelectionPrefix = "min_frag_";

        [Parameter(Comment = "The maximal height of the region in integer mutiples of a BRAM/DSP block height. The height is also limited by the actual FPGA height")]
        public int Height = 1000;
    }

    [Serializable]
    public class ResourceStringInfo
    {
        public Dictionary<string, SelectionInfo> EvaluatedResourceStrings
        {
            get { return m_evaluatedResourceStrings; }
        }

        public void InitResourceStrings(int sweepLineY)
        {
            // get BRAM / DSP heighs
            BRAMDSPSetting dspSetting = BRAMDSPSettingsManager.Instance.GetDSPSetting();
            BRAMDSPSetting bramSetting = BRAMDSPSettingsManager.Instance.GetBRAMSetting();

            if (dspSetting.Heigth != bramSetting.Heigth)
            {
                throw new ArgumentException("Expecting DSP and BRAM to be same height");
            }

            m_dspBRAMHeight = dspSetting.Heigth;

            string resourceString = "";
            List<Tile> tiles = new List<Tile>();
            for (int x = 0; x < FPGA.FPGA.Instance.MaxX; x++)
            {
                Tile t = FPGA.FPGA.Instance.GetTile(x, sweepLineY);
                bool addTile = false;
                foreach (IdentifierManager.RegexTypes resType in GetResourceTypes())
                {
                    if (IdentifierManager.Instance.IsMatch(t.Location, resType))
                    {
                        resourceString += GetCharForResourceType(resType);
                        addTile = true;
                    }
                }
                if(addTile)
                {
                    tiles.Add(t);
                }
            }

            for (int start = 0; start < resourceString.Length; start++)
            {
                for (int length = 1; start + length < resourceString.Length; length++)
                {
                    string subString = resourceString.Substring(start, length);

                    if (!m_evaluatedResourceStrings.ContainsKey(subString))
                    {
                        Dictionary<IdentifierManager.RegexTypes, int> resourcesInSelection = new Dictionary<IdentifierManager.RegexTypes, int>();

                        foreach (IdentifierManager.RegexTypes resType in GetResourceTypes())
                        {                           
                            char resTypeChar = GetCharForResourceType(resType);
                            resourcesInSelection[resType] = subString.Count(c => c == resTypeChar);
                        }
                        // we think in fll BRAM / DSP 
                        resourcesInSelection[IdentifierManager.RegexTypes.CLB] *= m_dspBRAMHeight;
                        
                        SelectionInfo selInfo = new SelectionInfo();
                        selInfo.Left = tiles[start];
                        selInfo.Right = tiles[start+(length-1)];
                        selInfo.ResourceString = subString;
                        selInfo.Resources = resourcesInSelection;
                        m_evaluatedResourceStrings.Add(subString, selInfo);
                    }
                }
            }         
        }

        public int MoveHeight(int currentY, int height)
        {
            for (int i = m_topYOfSelection.Count - 1; i - height >= 0; i--)
            {
                if (m_topYOfSelection[i] == currentY)
                {
                    return (m_topYOfSelection[i - height]);
                }
            }

            throw new ArgumentException("Can not move further up");
        }

        public bool CanMoveUp(int currentY, int height)
        {
            for (int i = m_topYOfSelection.Count - 1; i - height >= 0; i--)
            {
                if (m_topYOfSelection[i] == currentY)
                {
                    return true;
                }
            }

            return false;
        }

        public char GetCharForResourceType(IdentifierManager.RegexTypes resType)
        {
            if(!m_res2StringMapping.ContainsKey(resType))
            {
                Tile any = FPGA.FPGA.Instance.GetAllTiles().FirstOrDefault(t => Regex.IsMatch(t.Location, IdentifierManager.Instance.GetRegex(resType)));
                m_res2StringMapping.Add(resType, any.Location[0]);
            }
            return m_res2StringMapping[resType];
        }

        public string GetResourcePattern()
        {
            string pattern = "";
            foreach (IdentifierManager.RegexTypes resType in GetResourceTypes())
            {
                pattern += (pattern.Length > 0 ? "|" : "") + "(" + IdentifierManager.Instance.GetRegex(resType) + ")";
            }
            return pattern;
        }

        public IEnumerable<IdentifierManager.RegexTypes> GetResourceTypes()
        {
            yield return IdentifierManager.RegexTypes.CLB;
            yield return IdentifierManager.RegexTypes.BRAM;
            yield return IdentifierManager.RegexTypes.DSP;
        }

        private Dictionary<IdentifierManager.RegexTypes, char> m_res2StringMapping = new Dictionary<IdentifierManager.RegexTypes, char>();
        private Dictionary<char, IdentifierManager.RegexTypes> m_string2ResMapping = new Dictionary<char, IdentifierManager.RegexTypes>();
        private List<int> m_topYOfSelection = new List<int>();
        private int m_dspBRAMHeight = 0;
        private Dictionary<string, SelectionInfo> m_evaluatedResourceStrings = new Dictionary<string, SelectionInfo>();
    }

    public class ResourceRequirements
    {
        public string Name = "";
        public double XCenter = 0;
        public double YCenter = 0;
        public Dictionary<IdentifierManager.RegexTypes, List<int>> Chains = new Dictionary<IdentifierManager.RegexTypes, List<int>>();
        public int GetResourceRequirement(IdentifierManager.RegexTypes resType)
        {
            if (!Chains.ContainsKey(resType))
            {
                return 0;
            }
            return Chains[resType].Sum();
        }

        public override string ToString()
        {
            string output = "Resource requirements for " + Name + " @X=" + XCenter + " Y=" + YCenter + Environment.NewLine;
            foreach (IdentifierManager.RegexTypes resourceType in Chains.Keys)
            {
                output += resourceType + ":" + GetResourceRequirement(resourceType) + " with chains ";
                foreach (int chainLength in Chains[resourceType])
                {
                    output += chainLength + " ";
                }
                output += Environment.NewLine;
            }
            return output;
        }
    }

    public class Fragmentation
    {
        public Dictionary<IdentifierManager.RegexTypes, int> ResourceTypeFragmentation = new Dictionary<IdentifierManager.RegexTypes, int>();
        public int TotalFragmentation
        {
            get
            {
                return ResourceTypeFragmentation.Sum(t => t.Value);
            }
        }
    }
    
    public class SelectionInfo
    {
        public Tile Left = null;
        public Tile Right = null;
        public string ResourceString = "";        
        public Dictionary<IdentifierManager.RegexTypes, int> Resources = new Dictionary<IdentifierManager.RegexTypes, int>();
    }

    public class AreaCandidate
    {
        public SelectionInfo SelectionInfo;
        public Fragmentation FragmentationInfo = new Fragmentation();
        public int Height;
        public int CenterOfMass;
    }

}



