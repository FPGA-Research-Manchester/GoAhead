using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;

namespace GoAhead.Objects
{
    public class BlockerOrderElement
    {
        public string DriverRegexp = "";
        public string SinkRegexp = "";
        public string VivadoPipConnector = "";
        public bool ConnectAll = true;
        public bool EndPip = true;

        public override string ToString()
        {
            return "DriverRegexp=" + DriverRegexp + " SinkRegexp=" + SinkRegexp + " ConnectAll=" + ConnectAll;
        }
    }

    public class BlockerPath
    {
        public string DriverRegexp = "";
        public string HopRegexp = "";
        public string SinkRegexp = "";
    }

    public class BlockerSettings
    {
        private class ReplacementInfo
        {
            public string PrimitveRegexp = "";
            public string SliceNumberPattern = "";
            public string Template = "";
        }

        private BlockerSettings()
        {
        }

        /// <summary>
        /// BlockerSettings is a singelton
        /// </summary>
        public static BlockerSettings Instance = new BlockerSettings();

        public void AddBlockerPath(string family, string fromRegexp, string hopRegexp, string sinkRegexp)
        {
            BlockerPath p = new BlockerPath();
            p.DriverRegexp = fromRegexp;
            p.HopRegexp = hopRegexp;
            p.SinkRegexp = sinkRegexp;

            if (!m_blockerPaths.ContainsKey(family))
            {
                m_blockerPaths.Add(family, new List<BlockerPath>());
            }

            m_blockerPaths[family].Add(p);
        }

        public IEnumerable<BlockerPath> GetAllBlockerPaths()
        {
            string family = FPGA.FPGA.Instance.Family.ToString();

            foreach (KeyValuePair<string, List<BlockerPath>> tupel in m_blockerPaths.Where(tupel => Regex.IsMatch(family, tupel.Key)))
            {
                foreach (BlockerPath bp in tupel.Value)
                {
                    yield return bp;
                }
            }
        }

        public void AddPortFilter(string family, string regexp)
        {
            if (!m_portFilter.ContainsKey(family))
            {
                m_portFilter.Add(family, new List<string>());
            }

            m_portFilter[family].Add(regexp);
        }

        public bool SkipPort(Port p)
        {
            string family = FPGA.FPGA.Instance.Family.ToString();

            if (!m_portFilterRegexps.ContainsKey(family))
            {
                m_portFilterRegexps.Add(family, new List<Regex>());
                foreach (KeyValuePair<string, List<string>> tupel in m_portFilter.Where(tupel => Regex.IsMatch(family, tupel.Key)))
                {
                    foreach (string filter in tupel.Value)
                    {
                        Regex regexp = new Regex(filter, RegexOptions.Compiled);
                        m_portFilterRegexps[family].Add(regexp);
                    }
                }
            }

            foreach (Regex r in m_portFilterRegexps[family])
            {
                if (r.IsMatch(p.Name))
                {
                    return true;
                }
            }

            // no filter matches
            return false;
        }

        public void AddTileFilter(string family, string regexp)
        {
            if (!m_tileFilter.ContainsKey(family))
            {
                m_tileFilter.Add(family, new List<string>());
            }

            m_tileFilter[family].Add(regexp);
        }

        public bool SkipTile(Tile t)
        {
            string family = FPGA.FPGA.Instance.Family.ToString();

            if (!m_tileFilterRegexps.ContainsKey(family))
            {
                m_tileFilterRegexps.Add(family, new List<Regex>());
                foreach (KeyValuePair<string, List<string>> tupel in m_tileFilter.Where(tupel => Regex.IsMatch(family, tupel.Key)))
                {
                    foreach (string filter in tupel.Value)
                    {
                        Regex regexp = new Regex(filter, RegexOptions.Compiled);
                        m_tileFilterRegexps[family].Add(regexp);
                    }
                }
            }

            foreach (Regex r in m_tileFilterRegexps[family])
            {
                if (r.IsMatch(t.Location))
                {
                    return true;
                }
            }

            // no filter matches
            return false;
        }

        public void AddPrimitveTemplate(string family, string primitiveTypeRegexp, string sliceNumberPattern, string template)
        {
            if (!m_mappings.ContainsKey(family))
            {
                m_mappings.Add(family, new List<ReplacementInfo>());
            }

            ReplacementInfo ri = new ReplacementInfo();
            ri.PrimitveRegexp = primitiveTypeRegexp;
            ri.SliceNumberPattern = sliceNumberPattern;
            ri.Template = template;

            m_mappings[family].Add(ri);
        }

        /// <summary>
        /// Wheter to insert a template considering the SliceNumberPattern
        /// </summary>
        /// <param name="primitiveIdentifier"></param>
        /// <param name="primitiveIndex"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public bool InsertTemplate(string primitiveIdentifier, bool ignoreSliceNumber, int primitiveIndex, out string template)
        {
            template = "";

            string family = FPGA.FPGA.Instance.Family.ToString();

            foreach (KeyValuePair<string, List<ReplacementInfo>> tupel in m_mappings.Where(tupel => Regex.IsMatch(family, tupel.Key)))
            {
                foreach (ReplacementInfo ri in tupel.Value)
                {
                    if (Regex.IsMatch(primitiveIdentifier, ri.PrimitveRegexp) && (ignoreSliceNumber || Regex.IsMatch(primitiveIndex.ToString(), ri.SliceNumberPattern)))
                    {
                        template = ri.Template;
                        return true;
                    }
                }
            }

            // no match
            template = "";
            return false;
        }

        public void AddEndPipRegexp(string familyRegexp, string endPipRegexp)
        {
            if (!m_endPipsRegexps.ContainsKey(familyRegexp))
            {
                m_endPipsRegexps.Add(familyRegexp, new List<string>());
            }

            m_endPipsRegexps[familyRegexp].Add(endPipRegexp);
        }

        public void AddBlockerOrder(string familyRegexp, string driverRegexp, string sinkRegexp, bool connectAll, bool endPip, string vivadoPipConnector)
        {
            if (!m_blockerOrder.ContainsKey(familyRegexp))
            {
                m_blockerOrder.Add(familyRegexp, new List<BlockerOrderElement>());
            }

            BlockerOrderElement el = new BlockerOrderElement();
            el.ConnectAll = connectAll;
            el.DriverRegexp = driverRegexp;
            el.SinkRegexp = sinkRegexp;
            el.EndPip = endPip;
            el.VivadoPipConnector = vivadoPipConnector;
            m_blockerOrder[familyRegexp].Add(el);
        }

        public IEnumerable<BlockerOrderElement> GetBlockerOrder()
        {
            string family = FPGA.FPGA.Instance.Family.ToString();

            foreach (KeyValuePair<string, List<BlockerOrderElement>> tupel in m_blockerOrder.Where(tupel => Regex.IsMatch(family, tupel.Key)))
            {
                foreach (BlockerOrderElement el in tupel.Value)
                {
                    yield return el;
                }
            }
        }

        /// <summary>
        /// map e.g. FPGA family to SLICE and template
        /// </summary>
        private Dictionary<string, List<ReplacementInfo>> m_mappings = new Dictionary<string, List<ReplacementInfo>>();

        private Dictionary<string, List<string>> m_tileFilter = new Dictionary<string, List<string>>();
        private Dictionary<string, List<Regex>> m_tileFilterRegexps = new Dictionary<string, List<Regex>>();

        private Dictionary<string, List<string>> m_portFilter = new Dictionary<string, List<string>>();
        private Dictionary<string, List<Regex>> m_portFilterRegexps = new Dictionary<string, List<Regex>>();

        /// <summary>
        /// map e.g. FPGA family to SLICE and template
        /// </summary>
        private Dictionary<string, List<BlockerPath>> m_blockerPaths = new Dictionary<string, List<BlockerPath>>();

        private Dictionary<string, List<string>> m_endPipsRegexps = new Dictionary<string, List<string>>();

        private Dictionary<string, List<BlockerOrderElement>> m_blockerOrder = new Dictionary<string, List<BlockerOrderElement>>();
    }
}