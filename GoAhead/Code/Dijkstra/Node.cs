using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.Code.Dijkstra
{
    internal class Node<T>
    {
        public T Value { get; }
        public uint Index { get; }
        public List<Tuple<Node<T>, double>> Neighbours { get; }

        public Node(uint index, T value)
        {
            Value = value;
            Index = index;
            Neighbours = new List<Tuple<Node<T>, double>>();
            Neighbours.Add(new Tuple<Node<T>, double>(this, 0d));
        }

        public void AddNeighbour(Node<T> node, double distance)
        {
            Neighbours.Add(new Tuple<Node<T>, double>(node, distance));
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
