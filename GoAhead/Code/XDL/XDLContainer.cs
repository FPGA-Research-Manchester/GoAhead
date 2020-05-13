using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.XDL
{
    [Serializable]
    public class XDLContainer : NetlistContainer
    {
        public XDLContainer()
        {
            Name = "undefined";
            Anchor = "undefined";
        }

        public XDLContainer(string name)
        {
            Name = name;
            Anchor = "undefined";
        }

        ~XDLContainer()
        {
            if (m_netCodeBlockSW != null)
            {
                try
                {
                    m_netCodeBlockSW.Close();
                }
                catch
                {
                    // dont care
                }
            }

            if (File.Exists(m_netCodeBlockFile))
            {
                File.Delete(m_netCodeBlockFile);
            }
        }

        public bool ExplicitAnchorFound { get; set; }

        /// <summary>
        /// The name of the design (if any) that parsed to fill up the netlist container
        /// </summary>
        public string DesignName { get; set; }

        /// <summary>
        /// The FPGA familiy for which this design was implemented
        /// </summary>
        public string Family { get; set; }

        /// <summary>
        /// The anchor as given in the module delcaration line
        /// e.g. module "S6_bus_macro_no_vec" "left" , cfg "_SYSTEM_MACRO::FALSE" ;
        /// declares the instance named left as the anchor (usually identical with the IslandStyleNMC.Anchor)
        /// </summary>
        public string Anchor { get; set; }

        public void Add(XDLModule module)
        {
            m_modules.Add(module.Name, module);
        }

        public void AddDesignConfig(string headerCode)
        {
            m_designConfig.AppendLine(headerCode);
        }

        public void AddSliceCodeBlock(string sliceCode)
        {
            XDLInstance inst = XDLDesignParser.ExtractInstance(sliceCode);
            Add(inst);
            //this.m_sliceCodeBlocks.Add(inst.Name, inst);
        }

        /// <summary>
        /// Add plain XDL net code
        /// </summary>
        /// <param name="netCode"></param>
        public void AddNetCodeBlock(string netCode)
        {
            if (m_netCodeBlockSW == null)
            {
                m_netCodeBlockFile = Path.GetTempFileName();
                m_netCodeBlockSW = new StreamWriter(m_netCodeBlockFile, true);
            }
            m_netCodeBlockSW.WriteLine(netCode);
        }

        public void Add(XDLMacroPort port)
        {
            if (!m_portIndeces.ContainsKey(port.PortName))
            {
                m_portIndeces.Add(port.PortName, 0);
            }
            uint index = m_portIndeces[port.PortName];
            m_portIndeces[port.PortName]++;
            port.PortName = port.PortName + "<" + index + ">";
            m_macroPorts.Add(port.PortName, port);
        }

        public string GetDesignConfig()
        {
            return m_designConfig.ToString();
        }

        public string GetNetPortsBlocks()
        {
            return m_portCodeBlock.ToString();
        }

        public static bool GetAnchor(List<XDLContainer> netlistContainer, out string result)
        {
            Slice currentAnchor = null;
            foreach (NetlistContainer n in netlistContainer)
            {
                Slice probe = n.GetTopLeftSlice();

                if (currentAnchor == null && probe != null)
                {
                    currentAnchor = probe;
                }
                else if (currentAnchor != null && probe != null)
                {
                    if (probe.ContainingTile.TileKey.X < currentAnchor.ContainingTile.TileKey.X)
                    {
                        probe = currentAnchor;
                    }
                    else if (probe.ContainingTile.TileKey.X == currentAnchor.ContainingTile.TileKey.X && probe.ContainingTile.TileKey.Y < currentAnchor.ContainingTile.TileKey.Y)
                    {
                        probe = currentAnchor;
                    }
                }
            }

            result = (currentAnchor != null ? currentAnchor.SliceName : "ERROR:could_not_find_a_slice_with_ports_on");
            return currentAnchor != null;
        }

        public void WriteNetCodeBlocks(StreamWriter sw)
        {
            if (m_netCodeBlockSW != null)
            {
                m_netCodeBlockSW.Close();
                TextReader tr = new StreamReader(m_netCodeBlockFile);
                string line = "";
                while ((line = tr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
                tr.Close();
            }
        }

        protected override void UnblockResourcesAfterRemoval(string netName)
        {
            foreach (XDLPip seg in ((XDLNet)m_nets[netName]).Pips)
            {
                Tile t = FPGA.FPGA.Instance.GetTile(seg.Location);
                t.UnblockPort(seg.From);
                t.UnblockPort(seg.To);
            }
        }

        public IEnumerable<XDLModule> Modules
        {
            get { return m_modules.Values; }
        }

        public XDLInstance GetInstance(NetPin np)
        {
            if (HasInstanceByName(np.InstanceName))
            {
                //XDLInstance result = this.m_instances[np.InstanceName];
                //return result;
                return (XDLInstance)m_instances[np.InstanceName];
            }
            else
            {
                throw new ArgumentNullException("Can not resolve " + np.InstanceName);
            }
        }

        /// <summary>
        /// Get the name of the first net. Throws Exception if no net exists .
        /// </summary>
        /// <returns></returns>
        public override Net GetAnyNet()
        {
            if (m_nets.Count == 0)
            {
                throw new ArgumentException("GetAnyNet: No nets found in netlist container " + Name);
            }
            else
            {
                // return a net with a driver
                XDLNet netWithOutpin = (XDLNet)m_nets.Values.FirstOrDefault(n => n.OutpinCount == 1 && string.IsNullOrEmpty(((XDLNet)n).HeaderExtension));
                if (netWithOutpin != null)
                {
                    return netWithOutpin;
                }
                // if there is no net wiht a driver, return a net with an inpin
                XDLNet netWithInpin = (XDLNet)m_nets.Values.FirstOrDefault(t => t.InpinCount > 0);
                if (netWithInpin != null)
                {
                    return netWithInpin;
                }

                XDLNet anynet = (XDLNet)m_nets.Values.FirstOrDefault();
                return anynet;
            }
        }

        public override NetlistContainer GetSelectedDesignElements()
        {
            // resulting design
            XDLContainer result = new XDLContainer();

            result.Name = Name;
            result.AddDesignConfig(GetDesignConfig());

            // copy modules
            foreach (XDLModule mod in Modules)
            {
                result.Add(mod);
            }

            // filter ports
            foreach (XDLPort p in Ports)
            {
                XDLInstance inst = (XDLInstance)GetInstanceByName(p.InstanceName);
                if (TileSelectionManager.Instance.IsSelected(inst.TileKey))
                {
                    result.Add(p);
                }
            }

            // filter instances
            foreach (XDLInstance inst in Instances)
            {
                if (TileSelectionManager.Instance.IsSelected(inst.TileKey))
                {
                    result.Add(inst);
                }
            }

            // filter nets
            foreach (XDLNet inNet in Nets)
            {
                XDLNet outNet = new XDLNet();
                outNet.Name = inNet.Name;
                outNet.Header = inNet.Header;
                outNet.HeaderExtension = inNet.HeaderExtension;
                outNet.Config = inNet.Config;
                bool arcAdded = false;

                //outNet.AddCode(inNet.Header);

                // add/remove pins
                foreach (NetPin pin in inNet.NetPins)
                {
                    TileKey where = GetInstanceByName(pin.InstanceName).TileKey;
                    if (TileSelectionManager.Instance.IsSelected(where))
                    {
                        outNet.Add(pin);
                        arcAdded = true;
                    }
                }

                // add/remoce pips (arcs)
                foreach (XDLPip pip in inNet.Pips)
                {
                    bool addPip = true;

                    if (HasMappedInstances(pip.Location))
                    {
                        // exclude via instance
                        foreach (XDLInstance inst in GetInstancesByLocation(pip.Location))
                        {
                            if (!TileSelectionManager.Instance.IsSelected(inst.TileKey))
                            {
                                addPip = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // exclude via TileKey (eg for tile without instances INT_X8Y36)
                        Tile where = FPGA.FPGA.Instance.GetTile(pip.Location);
                        if (!TileSelectionManager.Instance.IsSelected(where.TileKey))
                        {
                            addPip = false;
                        }
                    }

                    if (addPip)
                    {
                        // we keep this net
                        arcAdded = true;
                        outNet.Add(pip);
                    }
                }

                // only add non empty nets
                if (arcAdded)
                {
                    result.Add(outNet);
                }
            }
            return result;
        }

        /// <summary>
        /// Get instances by location string. There may be several instances mapped in one location
        /// </summary>
        /// <param name="location">e.g. CLEXL_X18Y82</param>
        /// <returns></returns>
        private IEnumerable<XDLInstance> GetInstancesByLocation(string location)
        {
            if (m_locationInstanceMapping.ContainsKey(location))
            {
                foreach (XDLInstance inst in m_locationInstanceMapping[location])
                {
                    yield return inst;
                }
            }
        }

        public XDLInstance GetInstancesByLocation(string tileIdentifier, string sliceName)
        {
            if (m_locationInstanceMapping.ContainsKey(tileIdentifier))
            {
                foreach (XDLInstance inst in m_locationInstanceMapping[tileIdentifier])
                {
                    if (inst.SliceName.Equals(sliceName))
                    {
                        return inst;
                    }
                }
            }
            throw new ArgumentException("Could not find an instance on tile " + tileIdentifier + "." + sliceName);
        }

        public XDLInstance LeftMostInstance
        {
            get
            {
                if (m_leftMostInstance == null)
                {
                    FindUpperLeftLowerRight();
                }
                return m_leftMostInstance;
            }
            set { m_leftMostInstance = value; }
        }

        public XDLInstance RightMostInstance
        {
            get
            {
                if (m_rightMostInstance == null)
                {
                    FindUpperLeftLowerRight();
                }
                return m_rightMostInstance;
            }
            set { m_rightMostInstance = value; }
        }

        public int ModuleCount
        {
            get { return m_modules.Count; }
        }

        private void FindUpperLeftLowerRight()
        {
            // get module extension
            int lowerX = int.MaxValue;
            int upperX = int.MinValue;

            foreach (XDLInstance inst in Instances)
            {
                //Tile tile = FPGA.FPGA.Instance.GetTile(inst.TileKey);
                if (inst.LocationX < lowerX)
                {
                    lowerX = inst.LocationX;
                    LeftMostInstance = inst;
                }
                if (inst.LocationX > upperX)
                {
                    upperX = inst.LocationX;
                    RightMostInstance = inst;
                }
            }
        }

        public IEnumerable<XDLMacroPort> MacroPorts
        {
            get { return m_macroPorts.Values; }
        }

        public bool HasInstanceByName(string instanceName)
        {
            //return this.m_instances.Any(t => t.Value.Name.Equals(instanceName));
            return m_instances.ContainsKey(instanceName);
        }

        /// <summary>
        /// Write XDL design file
        /// </summary>
        /// <param name="sw"></param>
        public override void WriteCodeToFile(StreamWriter sw)
        {
            sw.WriteLine(m_designConfig.ToString());

            foreach (XDLModule mod in Modules)
            {
                sw.WriteLine(mod.ToString());
            }

            foreach (XDLPort p in Ports)
            {
                sw.WriteLine(p.ToString());
            }

            foreach (XDLInstance inst in Instances)
            {
                sw.WriteLine(inst.ToString());
            }

            foreach (XDLNet net in Nets)
            {
                net.WriteToFile(sw);
                //sw.WriteLine(net.ToString());
            }
        }

        private StringBuilder m_designConfig = new StringBuilder();
        private StreamWriter m_netCodeBlockSW = null;
        private string m_netCodeBlockFile = "";
        private StringBuilder m_portCodeBlock = new StringBuilder();
        private Dictionary<string, XDLModule> m_modules = new Dictionary<string, XDLModule>();
        private XDLInstance m_rightMostInstance = null;
        private XDLInstance m_leftMostInstance = null;

        private Dictionary<string, uint> m_portIndeces = new Dictionary<string, uint>();
        private Dictionary<string, XDLMacroPort> m_macroPorts = new Dictionary<string, XDLMacroPort>();
    }
}