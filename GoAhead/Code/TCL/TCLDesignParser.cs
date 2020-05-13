using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Commands;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.Code.TCL
{
    public class TCLDesignParser : DesignParser
    {
        public TCLDesignParser(string fileName)
        {
            m_fileName = fileName;
        }

        public override void ParseDesign(NetlistContainer nlc, Command caller)
        {
            TCLContainer container = (TCLContainer)nlc;

            Regex commentRegexp = new Regex(@"^\s*#", RegexOptions.Compiled);

            StreamReader sr = new StreamReader(m_fileName);
            FileInfo fi = new FileInfo(m_fileName);
            long charCount = 0;
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                if (line.Equals("debug"))
                {
                }


                charCount += line.Length;
                if (caller.PrintProgress)
                {
                    caller.ProgressInfo.Progress = (int)((double)charCount / (double)fi.Length * caller.ProgressShare);
                }

                if (commentRegexp.IsMatch(line))
                {
                    continue;
                }

                int length = line.IndexOf('=');
                string linePrefix = line.Substring(0, length);
                string[] atoms = line.Split(';');

                caller.Watch.Start("switch");
                switch (linePrefix)
                {
                    case "Cell":
                        // BEL=IOB33.INBUF_EN(0),CLASS=cell(1),
                        TCLInstance cellInstance = new TCLInstance();
                        cellInstance.AddCode(line);

                        foreach (string atom in atoms.Where(a => !string.IsNullOrEmpty(a)))
                        {
                            string atomPrefix;
                            string atomData;
                            bool readOnly;
                            bool property;
                            Decompose(atom, out atomPrefix, out atomData, out readOnly, out property);

                            // store special properties (i.e. name or properties that need ot modified)
                            switch (atomPrefix)
                            {
                                case "NAME":                                   
                                    cellInstance.Name = atomData;
                                    // flatten hierarchy to ease/enable netlist relocation
                                    cellInstance.Name = cellInstance.Name.Replace('/', '_');
                                    break;
                                case "LOC":
                                    Slice s = FPGA.FPGA.Instance.GetSlice(atomData);
                                    cellInstance.SliceName = s.SliceName;
                                    cellInstance.SliceType = s.SliceType;
                                    cellInstance.SliceNumber = s.ContainingTile.GetSliceNumberByName(s.SliceName);
                                    cellInstance.TileKey = s.ContainingTile.TileKey;
                                    cellInstance.Location = s.ContainingTile.Location;
                                    cellInstance.LocationX = s.ContainingTile.LocationX;
                                    cellInstance.LocationY = s.ContainingTile.LocationY;
                                    // we need a name and a location to add the instance
                                    break;
                                case "BEL":
                                    cellInstance.BELType = atomData.Substring(atomData.LastIndexOf(".")+1);
                                    break;
                            }
                            // Primitive is not a Xilinx primitive property, but denotes a primitive instance in a GoAhead netlist (*.viv_rpt
                            if (property)
                            {
                                cellInstance.Properties.SetProperty(atomPrefix, atomData, readOnly);
                            }
                        }
                        container.Add(cellInstance);

                        break;

                    case "Net":
                        // Net=I_IBUF[0],Routing={},BelPin=SLICE_X20Y6/D6LUT/A5,BelPin=SLICE_X20Y11/C6LUT/A6,BelPin=IOB_X0Y1/INBUF_EN/OUT
                        TCLNet net = new TCLNet("replacedLater");
                        net.AddCode(line);

                        foreach (string atom in atoms.Where(a => !string.IsNullOrEmpty(a)))
                        {
                            string atomPrefix;
                            string atomData;
                            bool readOnly;
                            bool property;
                            Decompose(atom, out atomPrefix, out atomData, out readOnly, out property);

                            switch (atomPrefix)
                            {
                                case "Net":
                                    net.Name = atomData;
                                    // flatten hierarchy to ease/enable netlist relocation
                                    net.Name = net.Name.Replace('/', '_');
                                    // we need a name to add the net 
                                    container.Add(net);
                                    break;
                                case "Nodes":
                                    net.RoutingTree = new TCLRoutingTree();
                                    net.RoutingTree.Root = new TCLRoutingTreeNode(null, null);
                                    net.RoutingTree.Root.VirtualNode = true;
                                    net.NodeNet = true;
                                    string[] nodes = atomData.Split(' ');
                                    foreach(string node in nodes)
                                    {
                                        string nodeTileName = node.Substring(0, node.IndexOf('/'));
                                        string nodePortName = node.Substring(node.IndexOf('/')+1);
                                        TCLRoutingTreeNode routingNode = new TCLRoutingTreeNode(FPGA.FPGA.Instance.GetTile(nodeTileName), new Port(nodePortName));
                                        net.RoutingTree.Root.Children.Add(routingNode);
                                    }
                                    break;
                                case "BelPin":
                                    // BelPin=IOB_X0Y1/INBUF_EN/OUT
                                    string[] hierarchy = atomData.Split('/');
                                    string sliceName = hierarchy[0];
                                    string belName = hierarchy[1];
                                    string portName = hierarchy[2];

                                    Instance instance = container.GetInstanceBySliceName(sliceName, belName);
                                    // inst ist manchmal null,  kommt bei ROUTETHROUGH vor, also zb A6 und es wird durch die LUT durchgeroutet

                                    Slice slice = FPGA.FPGA.Instance.GetSlice(sliceName);
                                    NetPin netPin;
                                    if (slice.PortMapping.IsSliceInPort(portName))
                                    {
                                        netPin = new NetInpin();
                                        netPin.TileName = slice.ContainingTile.Location;
                                    }
                                    else if (slice.PortMapping.IsSliceOutPort(portName))
                                    {
                                        netPin = new NetOutpin();
                                        net.OutpinInstance = instance;
                                        netPin.TileName = slice.ContainingTile.Location;
                                    }
                                    else
                                    {
                                        throw new ArgumentException("Cannot resolve direction of bel pin " + portName + " in line " + line);
                                    }

                                    netPin.InstanceName = instance != null? instance.Name : "unknown instance";
                                    netPin.SliceName = sliceName;
                                    netPin.SlicePort = portName;
                                    netPin.Code = atomData;
                                    net.Add(netPin);

                                    break;

                                case "ROUTE":
                                case "FIXED_ROUTE":
                                    // we need the property TYPE before we can pasrs the net
                                    if (net.Properties.HasProperty("TYPE"))
                                    {
                                        // parse now
                                        net.RoutingTree = new TCLRoutingTree();
                                        if (net.Type == TCLNet.NetType.POWER || net.Type == TCLNet.NetType.GROUND) // TODO more??
                                        {
                                            net.RoutingTree.Root = new TCLRoutingTreeNode(null, null);
                                            net.RoutingTree.Root.VirtualNode = true;
                                        }
                                        ParseRoutingTree(atomData, ref net.RoutingTree.Root);
                                    }
                                    else
                                    {
                                        // parse later
                                        net.SetCode(atomData);
                                    }
                                    // do not set property ROUTE, as the ROUTE property is generated
                                    //net.Properties.SetProperty(atomPrefix, atomData, readOnly);
                                    break;
                                case "TYPE":
                                    net.Properties.SetProperty(atomPrefix, atomData, readOnly);
                                    net.Type = (TCLNet.NetType)Enum.Parse(typeof(TCLNet.NetType), net.Properties.GetValue("TYPE"));
                                    if (net.RoutingTree == null)
                                    {
                                        net.RoutingTree = new TCLRoutingTree();
                                        if (net.Type == TCLNet.NetType.POWER || net.Type == TCLNet.NetType.GROUND)
                                        {
                                            net.RoutingTree.Root = new TCLRoutingTreeNode(null, null);
                                            net.RoutingTree.Root.VirtualNode = true;
                                        }
                                        ParseRoutingTree(net.GetCode(), ref net.RoutingTree.Root);                                        
                                    }
                                    break;
                                default:
                                    net.Properties.SetProperty(atomPrefix, atomData, readOnly);
                                    break;
                            }
                        }
                   
                        break;
                    case "Pin" :
                        TCLPin pin = new TCLPin();
                        pin.AddCode(line);
                        foreach (string atom in atoms.Where(a => !string.IsNullOrEmpty(a)))
                        {
                            string atomPrefix;
                            string atomData;
                            bool readOnly;
                            bool property;
                            Decompose(atom, out atomPrefix, out atomData, out readOnly, out property);

                            switch (atomPrefix)
                            {
                                case "Pin" :
                                    pin.Name = atomData;
                                    container.Add(pin);
                                    break;
                                default:
                                    pin.Properties.SetProperty(atomPrefix, atomData, readOnly);
                                    break;
                            }
                        }

                        break;
                    case "Port" :
                        TCLPort port = new TCLPort();
                        port.AddCode(line);
                        foreach (string atom in atoms.Where(a => !string.IsNullOrEmpty(a)))
                        {
                            string atomPrefix;
                            string atomData;
                            bool readOnly;
                            bool property;
                            Decompose(atom, out atomPrefix, out atomData, out readOnly, out property);
                            
                            switch (atomPrefix)
                            {
                                case "Port":
                                    port.ExternalName = atomData;
                                    container.Add(port);
                                    break;
                                default:
                                    port.Properties.SetProperty(atomPrefix, atomData, readOnly);
                                    break;
                            }
                        }
                        break;
                    case "Hierarchy":
                        TCLDesignHierarchy hier = new TCLDesignHierarchy();
                        hier.AddCode(line);

                        foreach (string atom in atoms.Where(a => !string.IsNullOrEmpty(a)))
                        {
                            string atomPrefix;
                            string atomData;
                            bool readOnly;
                            bool property;
                            Decompose(atom, out atomPrefix, out atomData, out readOnly, out property);

                            switch (atomPrefix)
                            {
                                case "Hierarchy":
                                    hier.Name = atomData;
                                    container.Add(hier);
                                    break;
                                default:
                                    hier.Properties.SetProperty(atomPrefix, atomData, readOnly);
                                    break;
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException("Unexpected netlist element: " + line);
                }
                caller.Watch.Stop("switch");
            }

            foreach (TCLNet n in container.Nets)
            {
                int pinCount = int.Parse(n.Properties.GetValue("PIN_COUNT"));
                int flatPinCount = int.Parse(n.Properties.GetValue("FLAT_PIN_COUNT"));
                if (pinCount != flatPinCount)
                {
                }
                if (pinCount != n.NetPinCount)
                {
                    //caller.OutputManager.WriteWarning("PIN_COUNT should match the number of BelPins in " + n.Name);
                }
            }

            foreach (TCLNet n in container.Nets)
            {
                caller.Watch.Start("SetTiles");
                n.SetTiles(caller);
                caller.Watch.Stop("SetTiles");
                caller.Watch.Start("SetPins");
                n.SetPins();
                caller.Watch.Stop("SetPins");
            }

            sr.Close();
        }

        private void ParseRoutingTree(string line, ref TCLRoutingTreeNode root)
        {
            int i = 0;
            int braces = 0;
            int start = 0;
            string buffer = "";
            TCLRoutingTreeNode current = root;

            while (i < line.Length)
            {
                switch (line[i])
                {
                    case ' ':
                        if (!string.IsNullOrEmpty(buffer))
                        {
                            // buffer is either something like INT_L_X26Y7/VCC_WIRE or EE2BEG0
                            TCLRoutingTreeNode node;
                            string parameter = buffer;
                            int split = buffer.IndexOf('/');
                            if (split > 0)
                            {
                                string tileName= buffer.Substring(0, split);
                                string portName = buffer.Substring(split + 1, buffer.Length - split -1);
                                node = new TCLRoutingTreeNode(FPGA.FPGA.Instance.GetTile(tileName), new Port(portName));
                            }
                            else
                            {
                                node = new TCLRoutingTreeNode(new Port(buffer));
                            }
                            node.Parent = current;
                            // if we create the root node, current is null
                            if (root == null)
                            {
                                root = node;
                            }
                            else
                            {
                                current.Children.Add(node);
                            }
                            current = node;
                            buffer = "";
                        }
                        break;
                    case '(':
                        // read everything up to closing bracket
                        start = i;
                        while (i < line.Length)
                        {
                            if (line[i] == ')')
                            {
                                if (root == null)
                                {
                                    ParseRoutingTree(line.Substring(start + 1, i - start - 1), ref root);
                                }
                                else
                                {
                                    ParseRoutingTree(line.Substring(start + 1, i - start - 1), ref current);
                                }
                                break;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        break;
                    case '{':
                        if (braces != 0)
                        {
                            int subtreeBraces = 0;
                            start = i;
                            // extract substree, e.g., { .. {}}
                            while (i < line.Length)
                            {
                                switch (line[i])
                                {
                                    case '{':
                                        subtreeBraces++;
                                        break;
                                    case '}':
                                        subtreeBraces--;
                                        break;
                                    default:
                                        break;
                                }
                                if (subtreeBraces == 0)
                                {
                                    ParseRoutingTree(line.Substring(start, i - start + 1), ref current);
                                    // i++ done in outer loop
                                    break;
                                }
                                else
                                {
                                    i++;
                                }
                            }
                        }
                        else
                        {
                            braces++;
                        }
                        break;

                    case '}':
                        braces--;
                        break;

                    default:
                        buffer += line[i];
                        break;
                }
                i++;
            }
            if (braces != 0)
            {
                throw new ArgumentException("Incomplete routing tree found: " + line);
            }
        }
        
        private void Decompose(string atom, out string atomPrefix, out string atomData, out bool readOnly, out bool property)
        {
            int length = atom.IndexOf('=');
            if (length < 0)
            {

            }
            atomPrefix = atom.Substring(0, length);
            atomData = atom.Substring(length + 1);

            readOnly = false;
            property = 
                !atomPrefix.Equals("Cell") && 
                !atomPrefix.Equals("Net") &&
                !atomPrefix.Equals("BelPin") &&
                !atomPrefix.Equals("Nodes") &&
                !atomPrefix.Equals("Port") &&
                !atomPrefix.Equals("Pin") &&
                !atomPrefix.Equals("Hierarchy");

            if (property)
            {
                string readOnlyAsString = atomData.Substring(atomData.Length - 2, 1);
                readOnly = readOnlyAsString.Equals("1");
                atomData = atomData.Substring(0, atomData.Length - 3);
            }
        }

    }
}