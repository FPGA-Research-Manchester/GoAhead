using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.XDL
{
    [Serializable]
    public class XDLNet : Net
    {
        public XDLNet()
        {
            Name = "undefined";
        }

        public XDLNet(string name)
        {
            Name = name;
        }

        private static int maxHops = 0;

        public XDLNet(List<Location> path)
        {
            int i = 0;
            int limit = path.Count - 1;
            while (i < limit)
            {
                int lookAhead = i + 1;
                int hopsInTile = 0;
                while (lookAhead < limit)
                {
                    bool locationMatch = path[lookAhead].Tile.Location.Equals(path[i].Tile.Location);
                    bool directSwitchMatrxiConnect = path[i].Tile.SwitchMatrix.Contains(path[lookAhead - 1].Pip, path[lookAhead].Pip);

                    if (!locationMatch || !directSwitchMatrxiConnect)
                    {
                        break;
                    }
                    lookAhead++;
                    hopsInTile++;
                }

                switch (hopsInTile)
                {
                    case 0: // last arc
                    case 1:
                        {
                            XDLPip arc = new XDLPip(path[i].Tile.Location, path[i].Pip.Name, "->", path[i + 1].Pip.Name);
                            Add(arc);
                            i += 2;
                            break;
                        }
                    default:
                        {
                            for (int hopIndex = 0; hopIndex < hopsInTile; hopIndex++)
                            {
                                XDLPip arc = new XDLPip(path[i].Tile.Location, path[i + hopIndex].Pip.Name, "->", path[i + hopIndex + 1].Pip.Name);
                                Add(arc);
                            }

                            if (hopsInTile > maxHops)
                            {
                                maxHops = hopsInTile;
                            }
                            i += hopsInTile + 1;
                            break;
                        }
                }
            }
        }

        public XDLNet(XDLNet other)
        {
            Name = other.Name;
            Header = other.Header;
            HeaderExtension = other.HeaderExtension;
            //this.m_xdlCodeList.AddRange(other.m_xdlCodeList);
            m_netPins.AddRange(other.m_netPins);
            m_xdlPips.AddRange(other.m_xdlPips);
        }

        public void ClearPips()
        {
            if (m_xdlPips != null)
            {
                m_xdlPips.Clear();
            }
        }

        public void ClearPins()
        {
            if (m_netPins != null)
            {
                m_netPins.Clear();
            }
        }

        public void ClearPips(bool unblockPorts)
        {
            if (unblockPorts)
            {
                foreach (XDLPip seg in Pips)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(seg.Location);
                    t.UnblockPort(seg.From);
                    t.UnblockPort(seg.To);
                }
            }

            ClearPips();
        }

        public override void BlockUsedResources()
        {
            foreach (XDLPip pip in Pips)
            {
                Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                if (!t.IsPortBlocked(pip.From))
                {
                    t.BlockPort(pip.From, Tile.BlockReason.Blocked);
                }
                if (!t.IsPortBlocked(pip.To))
                {
                    t.BlockPort(pip.To, Tile.BlockReason.Blocked);
                }
            }
        }

        /// <summary>
        /// Returns true if one XDLPip resides on the given tile an contains the given port (as from or to port)
        /// </summary>
        /// <param name="where"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Contains(Tile where, Port port)
        {
            foreach (XDLPip next in Pips)
            {
                if (next.Location.Equals(where.Location) && (next.From.Equals(port.Name) || next.To.Equals(port.Name)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Contains(Predicate<XDLPip> p)
        {
            if (m_xdlPips == null)
            {
                return false;
            }

            return m_xdlPips.FirstOrDefault(i => p(i)) != null;
        }

        public void Add(Tile where, Port from, Port to)
        {
            XDLPip pip = new XDLPip(where.Location, from.Name, "->", to.Name);
            if (m_xdlPips == null)
            {
                m_xdlPips = new List<XDLPip>();
            }
            m_xdlPips.Add(pip);
        }

        public void Add(Tile where, string from, string to)
        {
            XDLPip pip = new XDLPip(where.Location, from, "->", to);
            if (m_xdlPips == null)
            {
                m_xdlPips = new List<XDLPip>();
            }
            m_xdlPips.Add(pip);
        }

        /// <summary>
        /// Insert a pseudo pip that serves as a comment
        /// </summary>
        /// <param name="comment"></param>
        public void Add(string comment)
        {
            XDLPip commentPip = new XDLPip(!comment.StartsWith("#") ? "# " + comment : comment, "", "", "");

            if (m_xdlPips == null)
            {
                m_xdlPips = new List<XDLPip>();
            }
            m_xdlPips.Add(commentPip);
        }

        public void Add(XDLPip pip)
        {
            if (m_xdlPips == null)
            {
                m_xdlPips = new List<XDLPip>();
            }
            m_xdlPips.Add(pip);
        }

        public bool HasIndex()
        {
            return Regex.IsMatch(Header, @"<\d+>");
        }

        public bool HasInpin
        {
            get
            {
                if (m_netPins != null)
                {
                    foreach (NetPin np in m_netPins)
                    {
                        if (np is NetInpin)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public int GetIndex()
        {
            if (!HasIndex())
            {
                throw new ArgumentException("This net has no index");
            }
            string indexString = "";
            int pos = 0;
            bool indexFound = false;
            while (true)
            {
                if (Header[pos] == '<')
                {
                    indexFound = true;
                }
                else if (Header[pos] == '>')
                {
                    break;
                }
                else if (indexFound)
                {
                    indexString += Header[pos];
                }

                pos++;
            }
            return int.Parse(indexString);
        }

        public void Add(XDLNet net, bool checkForExistence = true, string comment = "")
        {
            if (m_xdlPips == null)
            {
                m_xdlPips = new List<XDLPip>();
            }
            if (m_netPins == null)
            {
                m_netPins = new List<NetPin>();
            }

            if (checkForExistence)
            {
                foreach (XDLPip pip in net.Pips)
                {
                    if (!m_xdlPips.Contains(pip))
                    {
                        if (!string.IsNullOrEmpty(comment))
                        {
                            Add(comment);
                        }
                        Add(pip);
                    }
                }
            }
            else
            {
                foreach (XDLPip pip in net.Pips)
                {
                    if (!string.IsNullOrEmpty(comment))
                    {
                        Add(comment);
                    }
                    Add(pip);
                }
            }

            // inpins outpins
            m_netPins.AddRange(net.NetPins);
        }

        public IEnumerable<XDLPip> Pips
        {
            get
            {
                if (m_xdlPips != null)
                {
                    // do not return comment pips
                    foreach (XDLPip pip in m_xdlPips.Where(p => !p.Location.StartsWith("#")))
                    {
                        yield return pip;
                    }
                }
            }
        }

        /// <summary>
        /// Remove all XDLPip that match the predicate and unblock Ports of remove XDLPips and return if any pip has been removed
        /// </summary>
        /// <param name="
        public void Remove(Predicate<XDLPip> predicate)
        {
            if (m_xdlPips == null)
            {
                return;
            }

            foreach (XDLPip pip in m_xdlPips.Where(p => predicate(p)))
            {
                Tile t = FPGA.FPGA.Instance.GetTile(pip.Location);
                if (t.IsPortBlocked(pip.From))
                {
                    t.UnblockPort(pip.From);
                }
                if (t.IsPortBlocked(pip.To))
                {
                    t.UnblockPort(pip.To);
                }
            }

            // before value
            m_xdlPips.RemoveAll(predicate);
        }

        public void RemoveAllPinStatements(Predicate<NetPin> predicate)
        {
            if (m_netPins != null)
            {
                m_netPins.RemoveAll(predicate);
            }
        }

        public bool HasPip(Predicate<XDLPip> filter)
        {
            if (m_xdlPips != null)
            {
                foreach (XDLPip pip in m_xdlPips)
                {
                    if (filter(pip))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override IEnumerable<NetPin> NetPins
        {
            get
            {
                if (m_netPins != null)
                {
                    foreach (NetPin pin in m_netPins)
                    {
                        yield return pin;
                    }
                }
                yield break;
            }
        }

        /// <summary>
        /// An XDLNet describes a macro or module port if
        /// 1) there is one inpin
        /// 2) there are neither outpins nor pips
        /// </summary>
        public bool IsPortNet
        {
            get
            {
                int inPins = InpinCount;
                int outPins = OutpinCount;
                int arcCount = PipCount;
                bool inPin = (inPins > 0) && (outPins + arcCount == 0);
                bool outPin = (outPins > 0) && (inPins + arcCount == 0);

                return inPin || outPin;
            }
        }

        public override int PipCount
        {
            get { return (m_xdlPips == null ? 0 : m_xdlPips.Count); }
        }

        public bool IsAntenna(out Dictionary<string, List<XDLPip>> pipsToRemove)
        {
            pipsToRemove = new Dictionary<string, List<XDLPip>>();

            string outpinName = NetPins.Where(np => np is NetOutpin).First().SlicePort;
            XDLPip firstPipAfterOutpin = Pips.Where(p => p.From.EndsWith(outpinName) && IdentifierManager.Instance.IsMatch(p.Location, IdentifierManager.RegexTypes.CLB)).FirstOrDefault();
            if (firstPipAfterOutpin == null)
            {
                //Console.WriteLine("Could not find the pip right after the outpin " + outpinName + " in net " + this.Name);
                return false;
            }

            Queue<XDLPip> reachablePips = new Queue<XDLPip>();
            reachablePips.Enqueue(firstPipAfterOutpin);

            foreach (XDLPip pip in Pips.Where(p => !p.Equals(firstPipAfterOutpin)))
            {
                if (!pipsToRemove.ContainsKey(pip.Location))
                {
                    pipsToRemove.Add(pip.Location, new List<XDLPip>());
                }
                pipsToRemove[pip.Location].Add(pip);
            }

            while (reachablePips.Count > 0)
            {
                // from the current pip
                XDLPip current = reachablePips.Dequeue();
                // go to all reachable locations
                foreach (Location loc in Navigator.GetDestinations(current.Location, current.To))
                {
                    if (!pipsToRemove.ContainsKey(loc.Tile.Location))
                    {
                        continue;
                    }
                    // for readability we reference the list of pips
                    List<XDLPip> pipsToRemoveOnTile = pipsToRemove[loc.Tile.Location];
                    while (true)
                    {
                        int index = pipsToRemoveOnTile.FindIndex(pip => pip.From.Equals(loc.Pip.Name));
                        if (index == -1)
                        {
                            break;
                        }
                        XDLPip reachable = pipsToRemoveOnTile[index];
                        pipsToRemoveOnTile.RemoveAt(index);
                        reachablePips.Enqueue(reachable);
                        // remove empty lists for readability
                        if (pipsToRemoveOnTile.Count == 0)
                        {
                            pipsToRemove.Remove(loc.Tile.Location);
                        }
                    }
                }
            }
            return pipsToRemove.Values.Any(l => l.Count > 0);
        }

        public override string ToString()
        {
            return Name;
        }

        public void WriteToFile(StreamWriter sw)
        {
            string headerExtension = string.IsNullOrEmpty(HeaderExtension) ? "" : " " + HeaderExtension;
            string config = string.IsNullOrEmpty(Config) ? "" : " " + Config;
            string netDeclaration = "net \"" + Name + "\"" + headerExtension + "," + config;
            sw.WriteLine(netDeclaration);

            foreach (NetPin np in NetPins.Where(x => x is NetOutpin))
            {
                sw.WriteLine("\t" + np.ToString());
            }

            foreach (NetPin np in NetPins.Where(x => x is NetInpin))//.OrderBy(x => x.InstanceName))
            {
                sw.WriteLine("\t" + np.ToString());
            }

            foreach (XDLPip pip in Pips)//.OrderBy(x => x.Location))
            {
                sw.WriteLine("\t" + pip.ToString());
            }

            sw.WriteLine(";");
        }

        [DefaultValue(false)]
        public bool PRLink { get; set; }

        /// <summary>
        /// net "inst_S6_bus_macro/l2r_3" ,
        /// </summary>
        [DefaultValue("undefined header")]
        public string Header { get; set; }

        /// <summary>
        /// vcc
        /// </summary>
        [DefaultValue("undefined header extension")]
        public string HeaderExtension { get; set; }

        /// <summary>
        /// cfg "_MACRO::Inst_connection_macros_right/right_State_1"
        /// </summary>
        [DefaultValue("undefined config ")]
        public string Config { get; set; }

        /// <summary>
        /// all pips strcutured as they can be found in this.m_xdlCodeList in an unstructured way
        /// </summary>
        private List<XDLPip> m_xdlPips = null;
    }
}