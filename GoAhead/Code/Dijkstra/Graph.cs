using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.Code.Dijkstra
{
    internal class Graph<T>
    {
        private List<Node<T>> _nodes;
        private bool _isDirected;
        internal Graph(bool isDirected = true)
        {
            _nodes = new List<Node<T>>();
            _isDirected = isDirected;
            _isInitialised = false;
        }

        public uint AddNode(T node)
        {
            uint index = (uint)_nodes.Count;
            _nodes.Add(new Node<T>(index, node));
            _isInitialised = false;
            return index;
        }

        public void Connect(uint start, uint end, bool isDirectedEdge = true)
        {
            var startNode = _nodes[(int)start];
            var endNode = _nodes[(int)end];

            startNode.AddNeighbour(endNode, 0d);
            if (!isDirectedEdge)
            {
                endNode.AddNeighbour(startNode, 0d);
            }
        }
        public void Connect(uint start, uint end, double distance, bool isDirectedEdge = true)
        {
            var startNode = _nodes[(int)start];
            var endNode = _nodes[(int)end];

            startNode.AddNeighbour(endNode, distance);
            if (!isDirectedEdge)
            {
                endNode.AddNeighbour(startNode, distance);
            }
        }

        public DijkstraResult<T> FindPath(uint start, uint target)
        {
            List<double> distances = new List<double>(_nodes.Count);
            List<uint?> previous = new List<uint?>(_nodes.Count);
            List<uint> unvisited = new List<uint>(_nodes.Count);

            for (int i = 0; i < _nodes.Count; i++)
            {
                unvisited.Add((uint)i);
                distances.Add((uint)i == start ? 0 : Double.PositiveInfinity);
                previous.Add(null);
            }
            while (unvisited.Count > 0)
            {
                DijkstraComparator comparator = new DijkstraComparator(distances);
                unvisited.Sort(comparator);
                uint u = unvisited.First();
                unvisited.Remove(unvisited[0]);
                if (u == target) 
                { 
                    break; 
                }

                foreach (var distTuple in _nodes[(int)u].Neighbours)
                {
                    double alt = distances[(int)u] + distTuple.Item2;
                    if (alt < distances[(int)distTuple.Item1.Index] && !Double.IsInfinity(distances[(int)u]))
                    {
                        distances[(int)distTuple.Item1.Index] = alt;
                        previous[(int)distTuple.Item1.Index] = u;
                    }
                }
            }
            return new DijkstraResult<T>(start, target, distances, previous, _nodes);
        }
    }

    internal class DijkstraComparator : IComparer<uint>
    {
        private List<double> _distances;
        public DijkstraComparator(List<double> distances)
        {
            _distances = distances;
        }
        public int Compare(uint node1, uint node2)
        {
            return _distances[(int)node1].CompareTo(_distances[(int)node2]);
        }
    }

    internal class DijkstraResult<T>
    {
        public String PathString { get; private set; }
        public List<T> Path { get; private set; }
        public double Distance { get; private set; }

        public DijkstraResult(uint start, uint end, List<double> distances, List<uint?> previous, List<Node<T>> nodes)
        {       
            Path = GetPathList(start, end, previous, nodes);
            PathString = GetPathString();
            Distance = distances[(int)end];
        }

        private List<T> GetPathList(uint start, uint end, List<uint?> previous, List<Node<T>> nodes)
        {
            uint index = end;
            List<T> path = new List<T>() { nodes.Where(x => x.Index == index).FirstOrDefault().Value };
            while (index != start)
            {
                if (previous[(int)index] is uint prevIndex)
                {
                    index = prevIndex;
                    path.Add(nodes.Where(x => x.Index == index).FirstOrDefault().Value);
                }
                else
                {
                    Console.WriteLine("No path could be found to the destination."); 
                    break;
                }
            }
            
            path.Reverse();
            return path;
        }

        private String GetPathString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (T node in Path)
            {
                sb.Append($"{node} --> ");
            }
            return sb.ToString().Substring(0, sb.Length - 5);
        }
    }
    
}
