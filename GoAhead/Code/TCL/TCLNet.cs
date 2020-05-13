using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.Commands.BlockingShared;
using GoAhead.FPGA;
using GoAhead.Objects;
using GoAhead.Commands;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLNet : Net
    {
        public enum NetType { UNDEFINED = 0, SIGNAL = 1, GLOBAL_CLOCK = 2, LOCAL_CLOCK = 3, REGIONAL_CLOCK = 4, POWER = 5, GROUND = 6, DONT_CARE = 7}

        /// <summary>
        /// copy and relocate the net
        /// </summary>
        /// <param name="other"></param>
        public static TCLNet Relocate(TCLNet other, LibraryElement libElement, Tile anchor)
        {
            TCLNet copiedNet = new TCLNet(other.Name);
            copiedNet.RoutingTree = new TCLRoutingTree(other.RoutingTree);
            foreach (TCLRoutingTreeNode node in copiedNet.RoutingTree.GetAllRoutingNodes().Where(n => n.Tile != null))
            {
                string targetLocation = "";
                bool success = libElement.GetTargetLocation(node.Tile.Location, anchor, out targetLocation);
                Tile targetTile = FPGA.FPGA.Instance.GetTile(targetLocation);
                node.Tile = targetTile;
            }


            copiedNet.m_footerComment.Append(other.FooterComment);
            copiedNet.Properties = new TCLProperties(other.Properties);

            copiedNet.m_netPins = new List<NetPin>();
            foreach (NetPin pin in other.NetPins)
            {
                NetPin copiedPin = NetPin.Copy(pin);
                copiedNet.m_netPins.Add(copiedPin);
                string targetLocation = "";
                bool success = libElement.GetTargetLocation(pin.TileName, anchor, out targetLocation);
                Tile targetTile = FPGA.FPGA.Instance.GetTile(targetLocation);
                pin.TileName = targetTile.Location;
            }
            copiedNet.IsBlockerNet = other.IsBlockerNet;
            copiedNet.NodeNet = other.NodeNet;
            if (other.OutpinInstance != null)
            {
                copiedNet.OutpinInstance = new TCLInstance((TCLInstance)other.OutpinInstance);
            }
            return copiedNet;
        }

        /// <summary>
        /// Copy TODO use Clone
        /// </summary>
        /// <param name="other"></param>
        public static TCLNet Copy(TCLNet other)
        {
            TCLNet copy = new TCLNet(other.Name);
            copy.RoutingTree = new TCLRoutingTree(other.RoutingTree);
            copy.m_footerComment.Append(other.FooterComment);
            copy.Properties = new TCLProperties(other.Properties);

            copy.m_netPins = new List<NetPin>();
            foreach (NetPin pin in other.NetPins)
            {
                copy.m_netPins.Add(NetPin.Copy(pin));
            }
            copy.IsBlockerNet = other.IsBlockerNet;
            copy.NodeNet = other.NodeNet;
            if (other.OutpinInstance != null)
            {
                copy.OutpinInstance = new TCLInstance((TCLInstance)other.OutpinInstance);
            }
            return copy;
        }

        public TCLNet(string name)
        {
            Name = name;
            IsBlockerNet = false;
        }

        public bool IsBlockerNet { get; set; }

        public override int PipCount
        {
            get { return RoutingTree.GetAllRoutingNodes().Count(n => !n.VirtualNode); }
        }

        public override void BlockUsedResources()
        {
            foreach (TCLRoutingTreeNode node in RoutingTree.GetAllRoutingNodes().Where(n => !n.VirtualNode))
            {
                node.Tile.BlockPort(node.Port, Tile.BlockReason.OccupiedByMacro);
            }
        }


        public void UnflattenNet()
        {
            List<TCLRoutingTreeNode> nodeQueue = new List<TCLRoutingTreeNode>(RoutingTree.Root.Children);
            NetOutpin outPin = (NetOutpin) NetPins.First(np => np is NetOutpin);
            Tile rootTile = FPGA.FPGA.Instance.GetTile(outPin.TileName);
            TCLRoutingTreeNode newRoot = nodeQueue.FirstOrDefault(n =>
                n.Tile.Location.Equals(rootTile.Location) &&
                rootTile.GetSliceByName(outPin.SliceName).PortMapping.IsSliceOutPort(outPin.SlicePort));

            nodeQueue.Remove(newRoot);

            NodeNet = false;
            RoutingTree.Root = newRoot;
            // try to assign each node to parent
            while (nodeQueue.Count > 0)
            {
                foreach (TCLRoutingTreeNode node in RoutingTree.GetAllRoutingNodes())
                {
                    bool addedNodes = false;
                    // stay on tile
                    TCLRoutingTreeNode nodeOnThisTile = nodeQueue.Find(n => 
                        n.Tile.Location.Equals(node.Tile.Location) &&
                        n.Tile.SwitchMatrix.Contains(node.Port, n.Port));
                    if(nodeOnThisTile != null)
                    {
                        node.Children.Add(nodeOnThisTile);
                        nodeQueue.Remove(nodeOnThisTile);
                        addedNodes = true;
                        break;
                    }

                    // leave tile
                    foreach (Location loc in Navigator.GetDestinations(node.Tile, node.Port))
                    {
                        // look for next hop on loc.Tile
                        TCLRoutingTreeNode nodeOnOtherTile = nodeQueue.Find(n => 
                            n.Tile.Location.Equals(loc.Tile.Location) && 
                            n.Tile.SwitchMatrix.Contains(loc.Pip, n.Port));
                        if (nodeOnOtherTile != null)
                        {
                            node.Children.Add(nodeOnOtherTile);
                            nodeQueue.Remove(nodeOnOtherTile);
                            addedNodes = true;
                            break;
                        }
                    }
                    if (addedNodes)
                    {
                        break;
                    }
                }

            }
        }
           
        public void FlattenNet()
        {
            // we do not need to overwrite TCLRoutingTree (which is only s very flat class)
            TCLRoutingTreeNode newRoot = new TCLRoutingTreeNode(null, null);
            newRoot.VirtualNode = true;
            // we can ignore virtual nodes as the new root will be the virtual new node
            foreach(TCLRoutingTreeNode node in RoutingTree.GetAllRoutingNodes().Where(n => !n.VirtualNode))
            {
                newRoot.Children.Add(node);
                node.Parent = newRoot;
            }

            // clear children after iterating over GetAllRoutingNodes, otherwise this will change the result fromGetAllRoutingNodes
            foreach (TCLRoutingTreeNode node in newRoot.Children)
            {
                node.Children.Clear();
            }
            
            NodeNet = true;
            RoutingTree.Root = newRoot;
        }

        public void SetTiles(Command caller)
        {
            // no routing
            if (m_routingTree == null)
            {
                return;
            }
            // no routing
            if (m_routingTree.Root == null)
            {
                return;
            }

            if (m_routingTree.Root.VirtualNode)
            {
                foreach (TCLRoutingTreeNode child in m_routingTree.Root.Children)
                {
                    if (child.Tile == null)
                    {
                        throw new ArgumentException("Can not set tiles for virtual node with child " + child + " in " + Name);
                    }
                    SetTiles(child, child.Tile,caller);
                }
            }
            else
            {
                // no start point
                if (OutpinInstance == null)
                {
                    throw new ArgumentException("Can not set tiles due to missing geometrical information in " + Name);
                }
                else
                {

                    Tile t = FPGA.FPGA.Instance.GetTile(OutpinInstance.TileKey);
                    TCLRoutingTreeNode n = m_routingTree.Root;

                    n.Tile = t;
                    SetTiles(n, t,caller);
                }
            }
        }

        private void SetTiles(TCLRoutingTreeNode node, Tile tile, Command caller)
        {
            foreach (TCLRoutingTreeNode child in node.Children)
            {
                child.Tile = tile;
                bool destinationsFound = false;
                IEnumerable<Location> locs = Navigator.GetDestinations(tile.Location, child.Port.Name);
                
                if(locs.Count() == 1)
                {
                    SetTiles(child, locs.First().Tile, caller);
                    destinationsFound = true;
                }
                else if (locs.Count() > 1)
                {
                    foreach (Location l in locs)
                    {
                        bool pathCanContinue = true;
                        foreach (TCLRoutingTreeNode c in node.Children)
                        {
                            if (!l.Tile.SwitchMatrix.GetDrivenPorts(l.Pip).Contains(c.Port))
                            {
                                pathCanContinue = false;
                            }

                        }
                        if (pathCanContinue)
                        {
                            SetTiles(child, l.Tile, caller);
                            destinationsFound = true;
                            break;
                        }
                    }

                    /*
                     * Wenn es mehrere loc gibt, dann nimm die, von dessen loc.Pip aus die/der child.children erreichbar sind
                     * Wenn es davon mehrer gibt --> Fehler
                    if (!child.Port.Name.StartsWith("IMUX") && !child.Port.Name.StartsWith("CLK"))
                    { 
                    
                    }
                    caller.OutputManager.WriteWarning("Path is not unique, branching from node " + child);
                     * */
                }

                if (!destinationsFound)
                {
                    SetTiles(child, tile, caller);
                }
            }
        }
        
        public void SetPins()
        {
            foreach (TCLRoutingTreeNode node in RoutingTree.GetAllRoutingNodes().Where(n => !n.VirtualNode))
            {
                Tile t = node.Tile;
                foreach (Slice s in t.Slices)
                {
                    bool inport = s.PortMapping.IsSliceInPort(node.Port);
                    bool outport = s.PortMapping.IsSliceOutPort(node.Port);
                    if ((inport || outport) && node.Port.Name.Contains('_'))
                    {
                        NetPin pin = null;
                        if (inport)
                        {
                            pin = new NetInpin();
                        }
                        else
                        {
                            pin = new NetOutpin();
                        }

                        pin.SlicePort = node.Port.Name.Substring(node.Port.Name.LastIndexOf('_'));
                        pin.InstanceName = s.SliceName;

                        bool pinExistsAlready = NetPins.FirstOrDefault(np => np.InstanceName.Equals(pin.InstanceName) && np.SlicePort.Equals(pin.SlicePort)) != null;
                        if (!pinExistsAlready)
                        {
                            Add(pin);
                        }
                    }
                }
            }
        }

        public string GetTCLRouting()
        {
            StringBuilder buffer = new StringBuilder();

            bool hasRouting = RoutingTree == null ? false : RoutingTree.Root != null;

            if (hasRouting && RoutingTree.Root.VirtualNode)
            {
                buffer.AppendLine("set_property ROUTE \"( \\");
                foreach (TCLRoutingTreeNode child in RoutingTree.Root.Children)
                {
                    foreach (TCLRoutingTreeNode grandChld in child.Children)
                    {
                        //buffer.AppendLine("\t{ " + child.ToString() + " " + child.VivadoPipConnector + grandChld.Port.Name + @" } \");
                        // bugfix after input from Edson
                        buffer.AppendLine("\t{ " + child.ToString() + " " + child.VivadoPipConnector + grandChld.ToString() + @" } \");
                    }
                }
                buffer.Append(")\" [get_nets " + Name + "]");
            }
            else if(hasRouting)
            {
                // pretty tree like code
                buffer.AppendLine(@"set_property ROUTE { \");
                ToTCL(RoutingTree.Root, buffer);
                buffer.AppendLine(@" \");
                buffer.Append("} [get_nets " + Name + "]");
            }
            else
            {
                // one liner, Viviado doet not allow linebreaks in empty routing
                buffer.Append(@"set_property ROUTE {} [get_nets " + Name + "]");
            }
            return buffer.ToString();
        }

        public void Remove(Predicate<TCLRoutingTreeNode> p)
        {
            if (!NodeNet)
            {
                throw new ArgumentException("Can only rmoev nodes form a node net (i.e. flat net)");
            }

            if (RoutingTree.Root != null)
            {
                RoutingTree.Root.Children.Remove(n => p(n));
            }
        }


        private void ToTCL(TCLRoutingTreeNode node, StringBuilder buffer)
        {
            if (node == null)
            {
                buffer.Append("{}");
                return;
            }

            // the first two node do need any tabs 
            if (node.Depth >= 2)
            {
                buffer.AppendLine(@" \");
                buffer.Append(string.Concat(Enumerable.Repeat("\t", node.Depth)));
            }

            // do not insert braces on last branch
            bool openBranch = node.Parent == null ? 
                false : node.Parent.Children.Count > 1 && !node.Equals(node.Parent.Children[node.Parent.Children.Count-1]);

            if (openBranch)
            {
                buffer.Append("{");
            }

            // the node itself
            buffer.Append(node.Port.Name + (node.Children.Count == 0 ? "" : " "));

            // the node has leaves only -> dump them without recursion
            if (node.Children.Count > 0 && node.Children.All(n => n.Children.Count == 0))
            {
                for (int i = 0; i < node.Children.Count-1; i++)
                {
                    buffer.Append("{" + node.Children[i].Port.Name + "}");
                    if (i % 5 == 0 && i > 0)
                    {
                        buffer.AppendLine(@"\");
                        buffer.Append(string.Concat(Enumerable.Repeat("\t", node.Depth + 2)));
                    }
                }
                // last branch goes without braces
                buffer.Append(node.Children[node.Children.Count-1].Port.Name);
            }
            else
            {
                // the node has children which have children in turn -> recursion
                foreach (TCLRoutingTreeNode child in node.Children)
                {
                    ToTCL(child, buffer);
                }
            }

            if (openBranch)
            {
                buffer.Append("}");
            }
        }

        public void AddFooterComment(string line)
        {
            m_footerComment.AppendLine(line);
        }

        public string FooterComment
        {
            get { return m_footerComment.ToString(); }
        }

        public TCLRoutingTree RoutingTree
        {
            get { return m_routingTree; }
            set { m_routingTree = value; }
        }

        public Instance OutpinInstance
        {
            get { return m_outpinInstance; }
            set { m_outpinInstance = value; }
        }

        public NetType Type
        {
            get { return m_netType; }
            set { m_netType = value; }
        }

        public bool NodeNet
        {
            get { return m_nodeNet; }
            set { m_nodeNet = value; }
        }

        public TCLProperties Properties = new TCLProperties();

        private bool m_nodeNet = false;
        private TCLRoutingTree m_routingTree = null;
        private Instance m_outpinInstance = null;
        private StringBuilder m_footerComment = new StringBuilder();
        private NetType m_netType = NetType.UNDEFINED;
    }
}