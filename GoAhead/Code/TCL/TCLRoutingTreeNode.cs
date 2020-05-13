using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLRoutingTreeNode 
    {
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public TCLRoutingTreeNode(TCLRoutingTreeNode other)
        {
            Port = other.Port;
            Children = new Children(other.Children);
            foreach (TCLRoutingTreeNode child in Children)
            {
                child.Parent = this;
            }
            Children.Parent = this;
            Parent = other.Parent == null? null : new TCLRoutingTreeNode((TCLRoutingTreeNode)other.Parent);
            Tile = other.Tile;
            VirtualNode = other.VirtualNode;
        }

        public TCLRoutingTreeNode(Port node)
        {
            Port = node;
            Children = new Children();
            Children.Parent = this;
        }

        public TCLRoutingTreeNode(Tile tile, Port port)
        {
            Port = port;
            Tile = tile;
            Children = new Children();
            Children.Parent = this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TCLRoutingTreeNode))
            {
                return false;
            }
            TCLRoutingTreeNode other = (TCLRoutingTreeNode)obj;
            // TODO compare more?
            return
                other.Tile.Location.Equals(Tile.Location) &&
                other.Port.NameKey == Port.NameKey;
        }

        public int Depth
        {
            get
            {
                int depth = 0;
                TCLRoutingTreeNode current = this;
                while (current.Parent != null)
                {
                    depth++;
                    current = current.Parent;
                }
                return depth;
            }
        }

        public readonly Port Port;
        public Children Children;
        public Tile Tile = null;
        public TCLRoutingTreeNode Parent = null;
        public string VivadoPipConnector = "";
        
        /// <summary>
        /// For virtual node, we do not emit any code. Virtual node are used as root in const0/1 nets
        /// </summary>
        public bool VirtualNode = false;

        public override string ToString()
        {
            return (Tile == null ? "" : Tile.Location + "/") + Port.Name;
        }
    }
}
