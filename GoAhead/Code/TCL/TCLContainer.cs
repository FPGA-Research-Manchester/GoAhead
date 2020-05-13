using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands.Data;

namespace GoAhead.Code.TCL
{
    class TCLContainer : NetlistContainer
    {
        public TCLContainer(string name)
        {
            Name = name;
        }

        public void Add(TCLPin pin)
        {
            if (m_pins.ContainsKey(pin.Name))
            {
                return;
                throw new ArgumentException("A pin named " + pin.Name + " has already been added");
            }

            m_pins.Add(pin.Name, pin);
        }

        public void Add(TCLDesignHierarchy hier)
        {
            if (m_hier.ContainsKey(hier.Name))
            {
                throw new ArgumentException("A hierarchy named " + hier.Name + " has already been added");
            }

            m_hier.Add(hier.Name, hier);
        }

        public bool AddGndPrimitive(Net net)
        {
            TCLInstance gnd = new TCLInstance();
            gnd.Name = "gnd_for_" + net.Name;
            Tile any = TileSelectionManager.Instance.GetSelectedTiles().Where(t => t.Slices.Count > 0).First();
            gnd.Location = any.Location;
            gnd.SliceName = any.Slices[0].SliceName;
            gnd.BELType = "GND";
            gnd.OmitPlaceCommand = true;
            Add(gnd);

            NetOutpin outpin = new NetOutpin();
            outpin.InstanceName = gnd.Name;
            outpin.SlicePort = "G";
            net.Add(outpin);

            return true;
            /*
            TCLInstance vcc = new TCLInstance();
            vcc.Name = "vcc_for_" + blockerNet.Name;
            Tile any = FPGA.TileSelectionManager.Instance.GetSelectedTiles().First();
            vcc.Location = any.Location;
            vcc.BELType = "VCC";
            vcc.OmitPlaceCommand = true;
            nlc.Add(vcc);

            NetOutpin outpin = new NetOutpin();
            outpin.InstanceName = vcc.Name;
            outpin.SlicePort = "P";
            blockerNet.Add(outpin);

            return true;
             * */
        }

        public IEnumerable<TCLPin> Pins
        {
            get { return m_pins.Values; }
        }

        public IEnumerable<TCLPort> Ports
        {
            get { return m_ports.Values; }
        }

        public IEnumerable<TCLDesignHierarchy> Hierarchies
        {
            get { return m_hier.Values; }
        }

        public override NetlistContainer GetSelectedDesignElements()
        {
            // resulting design
            TCLContainer result = new TCLContainer("GetSelectedDesignElements");
            result.Name = Name;

            // filter instances
            foreach (Instance inst in Instances)
            {
                if (TileSelectionManager.Instance.IsSelected(inst.TileKey))
                {
                    result.Add(inst);
                }
            }
             
            // filter nets
            foreach (TCLNet inNet in Nets)
            {
                // create copy
                TCLNet outNet = TCLNet.Copy(inNet);
                outNet.NodeNet = true;

                // cleat net pins and readd only he selected ones in the for each loop
                outNet.ClearNetPins();
                 
                foreach (NetPin pin in inNet.NetPins)
                {
                    if (!m_instances.ContainsKey(pin.InstanceName))
                    {
                        if (TileSelectionManager.Instance.IsSelected(FPGA.FPGA.Instance.GetTile(pin.TileName)))
                        {
                            outNet.Add(pin);
                        }
                    }
                    else
                    {
                        if (TileSelectionManager.Instance.IsSelected( GetInstanceByName(pin.InstanceName).TileKey))
                        {
                            outNet.Add(pin);
                        }
                    }
                }

                if (outNet.RoutingTree.GetAllRoutingNodes().Any(n => !n.VirtualNode && !TileSelectionManager.Instance.IsSelected(n.Tile)))
                {
                    outNet.FlattenNet();
                    outNet.Remove(node => !node.VirtualNode && !TileSelectionManager.Instance.IsSelected(node.Tile));
                }

                if (outNet.RoutingTree.GetAllRoutingNodes().Any(n => !n.VirtualNode))
                {
                    result.Add(outNet);
                }
            }
            return result;
        }

        public TCLInstance GetInstanceBySliceName(string sliceName, string belName)
        {
            if (!m_sliceInstanceMapping.ContainsKey(sliceName))
            {
                return null;                    
                //throw new ArgumentException("No instance with slice name" + sliceName + " known in " + this.ToString());
            }
            else
            {
                if (m_sliceInstanceMapping[sliceName].Count != 1)
                {
                    List<Instance> instances = m_sliceInstanceMapping[sliceName];
                }
                return (TCLInstance)m_sliceInstanceMapping[sliceName].FirstOrDefault(i => ((TCLInstance)i).Properties.GetValue("BEL").EndsWith(belName));
            }
        }

        public void RemoveAllPins()
        {
            m_pins.Clear();
        }

        public override void WriteCodeToFile(StreamWriter sw)
        {
            sw.WriteLine("##########################################");
            sw.WriteLine("# vivado netlist description for GoAhead #");
            sw.WriteLine("##########################################");
            foreach (TCLInstance cell in Instances)
            {
                sw.Write("Cell=" + cell.Name);
                foreach(TCLProperty prop in cell.Properties)
                {
                    sw.Write(";" + prop.Name + "=" + prop.Value + "(" + (prop.ReadOnly ? "1" : "0" ) + ")");
                }
                sw.WriteLine("");
            }

            foreach (TCLNet net in Nets)
            {
                sw.Write("Net=" + net.Name);
                foreach (NetPin np in net.NetPins)
                {
                    // no (readonly) forBelPins
                    sw.Write(";BelPin=" + np.Code);

                }
                if (net.NodeNet)
                {
                    string nodes = string.Join(" ", net.RoutingTree.Root.Children.Select(n => n.ToString()));
                    sw.Write(";Nodes=" + nodes);
                }
                foreach (TCLProperty prop in net.Properties)
                {
                    sw.Write(";" + prop.Name + "=" + prop.Value + "(" + (prop.ReadOnly ? "1" : "0") + ")");
                }
                sw.WriteLine("");
            }


            sw.WriteLine("# end of file");
        }

        protected Dictionary<string, TCLPin> m_pins = new Dictionary<string, TCLPin>();
        protected Dictionary<string, TCLPort> m_ports = new Dictionary<string, TCLPort>();
        protected Dictionary<string, TCLDesignHierarchy> m_hier = new Dictionary<string, TCLDesignHierarchy>();
    }
}
