using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Code.TCL
{
    [Serializable]
    public class TCLRoutingTree
    {
        public TCLRoutingTree()
        { 
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="other"></param>
        public TCLRoutingTree(TCLRoutingTree other)
        {
            if (other == null)
            {
                return;
            }
            if (other.Root == null)
            {
                return;
            }

            Root = new TCLRoutingTreeNode(other.Root);

            Queue<Tuple<TCLRoutingTreeNode, TCLRoutingTreeNode>> pairs = new Queue<Tuple<TCLRoutingTreeNode, TCLRoutingTreeNode>>();
            pairs.Enqueue(new Tuple<TCLRoutingTreeNode,TCLRoutingTreeNode>(Root, other.Root));

            while(pairs.Count > 0)
            {
                Tuple<TCLRoutingTreeNode, TCLRoutingTreeNode> next = pairs.Dequeue();
                for (int i = 0; i < next.Item2.Children.Count; i++)
                {
                    next.Item1.Children[i] = new TCLRoutingTreeNode(next.Item2.Children[i]);
                    pairs.Enqueue(new Tuple<TCLRoutingTreeNode,TCLRoutingTreeNode>(next.Item1.Children[i], next.Item2.Children[i]));
                }
            }
        }

        public TCLRoutingTreeNode Root = null;

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            foreach (TCLRoutingTreeNode node in GetAllRoutingNodes())
            {
                buffer.AppendLine(node.ToString() + " ");
            }
            return buffer.ToString();
        }

        public IEnumerable<TCLRoutingTreeNode> GetAllRoutingNodes()
        {
            return GetAllRoutingNodes(Root);
        }

        public IEnumerable<TCLRoutingTreeNode> GetAllRoutingNodes(TCLRoutingTreeNode root)
        {
            // nothing to return in case of empty routing
            if (root != null)
            {
                yield return root;
                foreach (TCLRoutingTreeNode child in root.Children)
                {
                    foreach (TCLRoutingTreeNode other in GetAllRoutingNodes(child))
                    {
                        yield return other;
                    }
                }
            }
        }
    }

}
