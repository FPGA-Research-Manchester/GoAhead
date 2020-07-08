using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GoAhead.Commands.Identifier;
using GoAhead.FPGA;
using GoAhead.Interfaces;

namespace GoAhead.Objects
{
    public class BRAMDSPSetting
    {
        public BRAMDSPSetting(int width, int height, bool leftRightHandling, string buttomLeft, string buttomRight)
        {
            Width = width;
            Heigth = height;
            LeftRightHandling = leftRightHandling;
            ButtomLeft = buttomLeft;
            ButtomRight = buttomRight;
        }

        public readonly int Width;
        public readonly int Heigth;
        public readonly bool LeftRightHandling;
        public readonly string ButtomLeft;
        public readonly string ButtomRight;
    }

    public class BRAMDSPSettingsManager
    {
        public void SetBRAMParameters(string family, int width, int height, bool leftRightHandling, string buttomLeft, string buttomRight)
        {
            m_bram[family] = new BRAMDSPSetting(width, height, leftRightHandling, buttomLeft, buttomRight);
        }

        public void SetDSPParameters(string family, int width, int height, bool leftRightHandling, string buttomLeft, string buttomRight)
        {
            m_dsp[family] = new BRAMDSPSetting(width, height, leftRightHandling, buttomLeft, buttomRight);
        }

        /// <summary>
        /// IdentifierManager is a singelton
        /// </summary>
        public static BRAMDSPSettingsManager Instance = new BRAMDSPSettingsManager();

        public BRAMDSPSetting GetBRAMSetting()
        {
            string key = FPGA.FPGA.Instance.Family.ToString();
            return m_bram[key];
        }

        public BRAMDSPSetting GetDSPSetting()
        {
            string key = FPGA.FPGA.Instance.Family.ToString();
            return m_dsp[key];
        }

        public void GetBRAMWidthAndHeight(out int width, out int height)
        {
            string key = FPGA.FPGA.Instance.Family.ToString();
            if (!m_bram.ContainsKey(key))
            {
                width = 0;
                height = 0;
            }
            else
            {
                width = m_bram[key].Width;
                height = m_bram[key].Heigth;
            }
        }

        private Dictionary<string, BRAMDSPSetting> m_bram = new Dictionary<string, BRAMDSPSetting>();
        private Dictionary<string, BRAMDSPSetting> m_dsp = new Dictionary<string, BRAMDSPSetting>();
    }

    public class LineParameter
    {
        public LineParameter(LineManager.Orienation orientation, int offset, string tileIdentifierRegexp)
        {
            m_orientation = orientation;
            m_offset = offset;
            m_tileIdentifierRegexp = new Regex(tileIdentifierRegexp, RegexOptions.Compiled);
        }

        public LineManager.Orienation Orientation
        {
            get { return m_orientation; }
        }

        public int Offset
        {
            get { return m_offset; }
        }

        public Regex TileIdentifierRegexp
        {
            get { return m_tileIdentifierRegexp; }
        }

        private readonly LineManager.Orienation m_orientation;
        private readonly int m_offset;
        private readonly Regex m_tileIdentifierRegexp;
    }

    public class LineManager
    {
        public enum Orienation { Undefined, Vertical, Horizontal }

        private LineManager()
        {
        }

        public static LineManager Instance = new LineManager();

        public void AddSetting(string family, Orienation orientation, int offset, string tileIdentifierRegexp)
        {
            if (!m_parameter.ContainsKey(family))
            {
                m_parameter.Add(family, new List<LineParameter>());
            }

            LineParameter lp = new LineParameter(orientation, offset, tileIdentifierRegexp);

            m_parameter[family].Add(lp);
        }

        public IEnumerable<LineParameter> GetLineParameter()
        {
            string key = FPGA.FPGA.Instance.Family.ToString();
            if (m_parameter.ContainsKey(key))
            {
                foreach (LineParameter lp in m_parameter[key])
                {
                    yield return lp;
                }
            }
        }

        private Dictionary<string, List<LineParameter>> m_parameter = new Dictionary<string, List<LineParameter>>();
    }

    public class IdentifierManager : IResetable
    {
        public enum RegexTypes {
            Unknown,
            BRAM,
            BRAM_left,
            BRAM_right,
            DSP,
            CLB,
            CLB_left,
            CLB_right,
            Interconnect,
            SubInterconnect_INTF,
            Interconnect_left,
            Interconnect_right,
            Slice,
            ProhibitExcludeFilter,
            VLineAnchor,
            HLineAnchor,
            BEL,
            BEL_outwire,
            BEL_inwire
        }

        private IdentifierManager()
        {
            Commands.Reset.ObjectsToReset.Add(this);
        }

        public void Reset()
        {
            m_regularExpressionBuffer.Clear();
        }

        /// <summary>
        /// IdentifierManager is a singelton
        /// </summary>
        public static IdentifierManager Instance = new IdentifierManager();

        public void SetRegex(RegexTypes type, string family, string regexp)
        {
            if (!m_regex.ContainsKey(type))
            {
                m_regex.Add(type, new Dictionary<string, string>());
            }
            m_regex[type][family] = regexp;
        }

        public void AddRegexMultipair(Tuple<RegexTypes, RegexTypes> typePair, string family, Tuple<string, string> regexPair)
        {
            if(!m_regex_multipairs.ContainsKey(typePair))
            {
                m_regex_multipairs.Add(typePair, new Dictionary<string, List<Tuple<string, string>>>());
                m_regex_multipairs[typePair][family] = new List<Tuple<string, string>>();
            }
            m_regex_multipairs[typePair][family].Add(regexPair);
        }

        public string GetRegex(RegexTypes type)
        {
            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                throw new ArgumentException("No FPGA loaded");
            }

            if (!m_regex.ContainsKey(type))
            {
                throw new ArgumentException("No Regexp of type " + type + " found. Use SetRegexp command. Are you missing init.goa?");
            }

            string familiy = FPGA.FPGA.Instance.Family.ToString();
            if (!m_regex[type].ContainsKey(familiy))
            {
                throw new ArgumentException("No Regexp of type " + type + " found for FPGA family " + familiy + ". Use SetRegexp. Are you missing init.goa?");
            }
            return m_regex[type][familiy];
        }

        public List<Tuple<string, string>> GetRegexMultipairList(Tuple<RegexTypes, RegexTypes> typePair)
        {
            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                throw new ArgumentException("No FPGA loaded");
            }

            if (!m_regex_multipairs.ContainsKey(typePair))
            {
                throw new ArgumentException("No Regexp Pair of type <" + typePair.Item1 + ", " + typePair.Item2 + "> found. Use AddRegexp command. Are you missing init.goa?");
            }

            string familiy = FPGA.FPGA.Instance.Family.ToString();
            if (!m_regex_multipairs[typePair].ContainsKey(familiy))
            {
                throw new ArgumentException("No Regexp of type <" + typePair.Item1 + ", " + typePair.Item2 + "> found for FPGA family " + familiy + ". Use AddRegexp. Are you missing init.goa?");
            }

            return m_regex_multipairs[typePair][familiy];
        }

        public List<Tuple<string, string>> GetRegexMultipairList(RegexTypes type1, RegexTypes type2)
        {
            return GetRegexMultipairList(new Tuple<RegexTypes, RegexTypes>(type1, type2));
        }

        public bool HasRegexp(RegexTypes type)
        {
            if (!m_regex.ContainsKey(type))
            {
                return false;
            }

            string familiy = FPGA.FPGA.Instance.Family.ToString();
            if (!m_regex[type].ContainsKey(familiy))
            {
                return false;
            }
            return true;
        }

        public string GetRegex(RegexTypes type1, RegexTypes type2)
        {
            string regexp1 = GetRegex(type1);
            string regexp2 = GetRegex(type2);

            return "(" + regexp1 + ")|(" + regexp2 + ")";
        }

        public bool IsMatch(string input, RegexTypes regexpType)
        {
            if (!m_regularExpressionBuffer.ContainsKey(regexpType))
            {
                string pattern = GetRegex(regexpType);
                Regex regex = new Regex(pattern, RegexOptions.Compiled);
                m_regularExpressionBuffer.Add(regexpType, regex);
            }

            return m_regularExpressionBuffer[regexpType].IsMatch(input);
        }

        public RegexTypes GetRegexpType(Tile t)
        {
            foreach (KeyValuePair<RegexTypes, Dictionary<string, string>> tupel in m_regex)
            {
                if (IsMatch(t.Location, tupel.Key))
                {
                    return tupel.Key;
                }
            }
            return RegexTypes.Unknown;
            //throw new ArgumentException("Could not find a resource type for " + t.Location);
        }

        private Dictionary<RegexTypes, Regex> m_regularExpressionBuffer = new Dictionary<RegexTypes, Regex>();
        // <regex, <family, pattern>>
        private Dictionary<RegexTypes, Dictionary<string, string>> m_regex = new Dictionary<RegexTypes, Dictionary<string, string>>();

        /// <summary>
        /// This one can be used for pairs of regex - two expressions that are related
        /// There may be multiple possible pattern pairs for each regex pair
        /// 
        /// Occupied usages:
        /// (BELRegex, BELOutwireRegex) - AddBELOutwireIdentifierRegexp
        /// (BELRegex, BELInwireRegex) - AddBELInwireIdentifierRegexp
        /// (BELRegex, BELRegex) - AddSwitchMatrixStopoverArc
        /// </summary>
        /// 
        // <<regex1, regex2>, <family, list<pattern1, pattern2>>>
        private Dictionary<Tuple<RegexTypes, RegexTypes>, Dictionary<string, List<Tuple<string, string>>>> m_regex_multipairs =
            new Dictionary<Tuple<RegexTypes, RegexTypes>, Dictionary<string, List<Tuple<string, string>>>>();
    }

    public class IdentifierPrefixManager
    {
        public static IdentifierPrefixManager Instance = new IdentifierPrefixManager();

        public List<string> Prefices
        {
            get { return m_prefices; }
            set { m_prefices = value; }
        }

        private List<string> m_prefices = new List<string>();
    }

    public class ColumnTypeNameManager
    {
        public static ColumnTypeNameManager Instance = new ColumnTypeNameManager();

        public string GetColumnTypeNameByResource(string resource, out SetColumnTypeNames addCmd)
        {
            if (resource.Contains("SLICEL"))
            {
            }

            addCmd = null;
            if (string.IsNullOrEmpty(resource))
            {
                return "EMPTY";
            }
            foreach (KeyValuePair<string, string> t in m_nameByResources)
            {
                string candidate = t.Value;
                while (resource.Length >= candidate.Length)
                {
                    if (resource.Equals(candidate))
                    {
                        return t.Key;
                    }
                    candidate += "," + t.Value;
                }
            }
            // nothing found -> define new type
            int i = 0;
            while (m_nameByResources.ContainsKey("unknown" + i))
            {
                i++;
            }
            // return how to set this unknown type name
            addCmd = new SetColumnTypeNames();
            addCmd.ColumnTypeName = "unknown" + i;
            addCmd.Resources = resource;


            AddTypeNameByResource("unknown" + i, resource);
            return "unknown" + i;
        }

        public void AddTypeNameByResource(string typeName, string resource)
        {
            if (m_nameByResources.ContainsKey(typeName))
            {
                throw new ArgumentException("Type name " + typeName + " already exists");
            }
            m_nameByResources.Add(typeName, resource);
        }

        private Dictionary<string, string> m_nameByResources = new Dictionary<string, string>();

    }

    public class SliceCompare
    {
        private SliceCompare()
        {
            //Commands.Reset.ObjectsToReset.Add(this);
        }

        public void Add(string familiy, string requiredSliceType, string possibleTargetSliceType)
        {
            if (!m_rules.ContainsKey(familiy))
            {
                m_rules.Add(familiy, new Dictionary<string, List<string>>());
            }

            if (!m_rules[familiy].ContainsKey(requiredSliceType))
            {
                m_rules[familiy].Add(requiredSliceType, new List<string>());
            }

            m_rules[familiy][requiredSliceType].Add(possibleTargetSliceType);
        }

        public bool Matches(string requiredSliceType, string possibleTargetSliceType)
        {
            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                throw new ArgumentException("No FPGA loaded");
            }

            string familiy = FPGA.FPGA.Instance.Family.ToString();
            if (!m_rules.ContainsKey(familiy))
            {
                return false;
            }
            if (!m_rules[familiy].ContainsKey(requiredSliceType))
            {
                return false;
            }
            if (!m_rules[familiy][requiredSliceType].Contains(possibleTargetSliceType))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// IdentifierManager is a singelton
        /// </summary>
        public static SliceCompare Instance = new SliceCompare();

        private Dictionary<string, Dictionary<string, List<string>>> m_rules = new Dictionary<string, Dictionary<string, List<string>>>();
    }
}