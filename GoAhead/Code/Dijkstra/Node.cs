using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.Code.Dijkstra
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Node<T>
    {
        public T Value { get; }
        public uint Index { get; }
        public List<Tuple<Node<T>, int>> Neighbours { get; }

        public Node(uint index, T value)
        {
            Value = value;
            Index = index;
            Neighbours = new List<Tuple<Node<T>, int>>();
            Neighbours.Add(new Tuple<Node<T>, int>(this, 0));
        }

        /// <summary>
        /// Connect a node to this node by adding the destination node and the distance as a tuple object to the list of neighbours.
        /// </summary>
        /// <param name="node"> The destination node to connect this node to. </param>
        /// <param name="distance"> The distance associated with the edge. </param>
        public void AddNeighbour(Node<T> node, int distance)
        {
            Neighbours.Add(new Tuple<Node<T>, int>(node, distance));
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
