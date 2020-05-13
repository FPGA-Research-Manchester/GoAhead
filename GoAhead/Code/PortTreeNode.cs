using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoAhead.FPGA;
using GoAhead.Code.TCL;

namespace GoAhead.Code
{
    [Serializable]
    public class Children : IEnumerable<TCLRoutingTreeNode>
    {
        public Children()
        {
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public Children(Children other)
        {
            foreach (TCLRoutingTreeNode n in other)
            {
                m_children.Add(new TCLRoutingTreeNode(n.Tile, n.Port));
            }
        }

        public void Add(TCLRoutingTreeNode node)
        {
            m_children.Add(node);
            node.Parent = Parent;

            AddToIndex(node);
        }

        private void AddToIndex(TCLRoutingTreeNode node)
        {
            if (node.Tile != null)
            {
                if (!m_index.ContainsKey(node.Tile.Location))
                {
                    m_index.Add(node.Tile.Location, new Dictionary<string, TCLRoutingTreeNode>());
                }
                m_index[node.Tile.Location][node.Port.Name] = node;
            }
        }

        public bool Contains(Predicate<TCLRoutingTreeNode> p)
        {
            return m_children.Exists(n => p(n));
        }

        public TCLRoutingTreeNode FirstOrDefault(Predicate<TCLRoutingTreeNode> p)
        {
            return m_children.FirstOrDefault(n => p(n));
        }

        public TCLRoutingTreeNode this[int i]
        {
            get { return m_children[i]; }
            set { m_children[i] = value; }
        }

        public int Count
        {
            get { return m_children.Count; }
        }

        public void Clear()
        {
            m_children.Clear();
        }

        public void Remove(Predicate<TCLRoutingTreeNode> p)
        {
            // unblock removed ports
            foreach (TCLRoutingTreeNode node in this.Where(n => p(n)))
            {
                node.Tile.UnblockPort(node.Port);
            }

            m_children.RemoveAll(p);
        }

        public override string ToString()
        {
            return m_children.Count + " children";
        }

        public TCLRoutingTreeNode FirstOrDefault(string location, string portName)
        {
            if (!m_index.ContainsKey(location))
            {
                TCLRoutingTreeNode result = m_children.FirstOrDefault(n => n.Tile.Location.Equals(location) && n.Port.Name.Equals(portName));
                if (result != null)
                {
                    AddToIndex(result);
                }
                return result;
            }
            else if (m_index[location].ContainsKey(portName))
            {
                return m_index[location][portName];
            }
            else
            {
                return null;
            }
        }

        private List<TCLRoutingTreeNode> m_children = new List<TCLRoutingTreeNode>();
        public TCLRoutingTreeNode Parent = null;
        private Dictionary<string, Dictionary<string, TCLRoutingTreeNode>> m_index = new Dictionary<string, Dictionary<string, TCLRoutingTreeNode>>();

        public IEnumerator<TCLRoutingTreeNode> GetEnumerator()
        {
            return m_children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
