using GoAhead.FPGA;
using GoAhead.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoAhead.Code.Dijkstra
{
    // Graph object used in the custom Dijkstra implementation. Uses a generic type parameter.
    internal class Graph<T>
    {
        // Private readonly reference list of all nodes in the graph.
        private readonly List<Node<T>> _nodes;

        internal Graph()
        {
            _nodes = new List<Node<T>>();
        }

        internal Graph(bool uniquePortNames)
        {
            _nodes = new List<Node<T>>();
            _uniquePortNames = uniquePortNames;
        }

        private bool _uniquePortNames = false;
        /// <summary>
        /// Adds a node to the graph and returns the node's uint representation (index within _nodes).
        /// </summary>
        /// <param name="node"> The object to add to the graph. </param>
        /// <returns> A uint index of the node added to the graph. </returns>
        public uint AddNode(T node)
        {
            uint index = (uint)_nodes.Count;
            _nodes.Add(new Node<T>(index, node));
            return index;
        }

        /// <summary>
        /// Connects two nodes on the graph using their uint indexes.
        /// </summary>
        /// <param name="start"> Index of the node where the edge starts. </param>
        /// <param name="end"> Index of the node where the edge terminates. </param>
        /// <param name="distance"> Cost of traversing the edge. </param>
        /// <param name="isDirectedEdge"> Whether the edge should have an equivalent edge in the opposite direction. </param>
        /// <example> For example:
        /// <code>
        /// Connect(0, 1, 4.3, false);
        /// </code>
        /// would add a reference to node 1 in node 0's list of neighbours with a distance of 4.3, and then do the same for the other way round: adding a reference to node 0 in node 1's list of neighbours.
        /// </example>
        public void Connect(uint start, uint end, int distance = 0, bool isDirectedEdge = true)
        {
            var startNode = _nodes[(int)start];
            var endNode = _nodes[(int)end];

            startNode.AddNeighbour(endNode, distance);
            if (!isDirectedEdge)
            {
                endNode.AddNeighbour(startNode, distance);
            }
        }

        /// <summary>
        /// Applies a slightly modified Dijkstra algorithm to the graph represented by this object.
        /// </summary>
        /// <param name="start"> Index of the node where the Dijkstra algorithm will begin. </param>
        /// <param name="target"> Index of the node that we are trying to reach. </param>
        /// <returns> A <c>DijkstraResult<typeparamref name="T"/></c> object containing information about the path found. </returns>
        public DijkstraResult<T> FindPath(uint start, uint target)
        {
            List<int> distances = new List<int>(_nodes.Count);
            List<uint?> previous = new List<uint?>(_nodes.Count);
            List<uint> unvisited = new List<uint>(_nodes.Count);

            for (int i = 0; i < _nodes.Count; i++)
            {
                unvisited.Add((uint)i);
                distances.Add((uint)i == start ? 0 : int.MaxValue);
                previous.Add(null);
            }
            while (unvisited.Count > 0)
            {
                DijkstraComparator<T> comparator = new DijkstraComparator<T>(distances);
                unvisited.Sort(comparator);
                uint u = unvisited.First();
                unvisited.Remove(unvisited[0]);
                if (u == target) 
                { 
                    break; 
                }

                foreach (var distTuple in _nodes[(int)u].Neighbours)
                {
                    int dist1 = distances[(int)u];
                    int dist2 = distances[(int)distTuple.Item1.Index];
                    int alt = distances[(int)u] + distTuple.Item2;
                    if (alt < distances[(int)distTuple.Item1.Index] && (distances[(int)u] != int.MaxValue))
                    {
                        if (_uniquePortNames)
                        {
                            switch (_nodes[(int)u].Path[0])
                            {
                                case Location l:
                                    List<string> ports = ((IEnumerable<Location>)_nodes[(int)u].Path).Select(p => p.Pip.ToString()).ToList();
                                    string nextPort = (_nodes[(int)distTuple.Item1.Index].Value as Location).Pip.ToString();
                                    if (ports.Contains(nextPort))
                                        continue;
                                    break;
                            }
                        }
                        distances[(int)distTuple.Item1.Index] = alt;
                        previous[(int)distTuple.Item1.Index] = u;
                        if (_uniquePortNames)
                        {
                            _nodes[(int)distTuple.Item1.Index].Path.AddRange(_nodes[(int)u].Path);
                        }
                    }
                }
            }
            return new DijkstraResult<T>(start, target, distances, previous, _nodes);
        }
    }

    /// <summary>
    /// Custom implementation of IComparer for use on a list of uint indexes of nodes. 
    /// </summary>
    internal class DijkstraComparator<T> : IComparer<uint>
    {
        private readonly List<int> _distances;
        public DijkstraComparator(List<int> distances)
        {
            _distances = distances;
        }
        public int Compare(uint node1, uint node2)
        {
            return _distances[(int)node1].CompareTo(_distances[(int)node2]);
        }
    }


    /// <summary>
    /// Class containing information about the path found via the Dijkstra algorithm.
    /// </summary>
    /// <typeparam name="T"> The type associated with the Node objects.</typeparam>
    internal class DijkstraResult<T>
    {
        public String PathString { get; private set; }
        public List<T> Path { get; private set; }
        public double Distance { get; private set; }

        public DijkstraResult(uint start, uint end, List<int> distances, List<uint?> previous, List<Node<T>> nodes)
        {       
            Path = GetPathList(start, end, previous, nodes);
            PathString = GetPathString();
            Distance = distances[(int)end];
        }

        /// <summary>
        /// Creates a List\<<typeparamref name="T"/>\> containing the contents of each Node object in the final path.
        /// </summary>
        /// <param name="start"> Index of the node where the Dijkstra algorithm begins. </param>
        /// <param name="end"> Index of the node where the Dijkstra algorithm ends. </param>
        /// <param name="previous"> List containing the uint key of each node's predecessor node - a value of 1 at index 5 would mean 1 is the predecessor of 5. </param>
        /// <param name="nodes"> List of references to every node in the graph. </param>
        /// <returns> A List<typeparamref name="T"/> containing the path found, or the empty list if no path was found. </returns>
        private List<T> GetPathList(uint start, uint end, List<uint?> previous, List<Node<T>> nodes)
        {
            int count = previous.Where(ind => ind != null).Count();
            uint? prev = previous.Where(ind => ind != null).First();
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
                    path = new List<T>();
                    break;
                }
            }
            
            path.Reverse();
            return path;
        }
        /// <summary>
        /// Creates a string representation of the path found. 
        /// </summary>
        /// <returns> A string object representing the path found. </returns>
        private string GetPathString()
        {
            if(Path.Count == 0)
            {
                return "Null path.";
            }
            StringBuilder sb = new StringBuilder();
            foreach (T node in Path)
            {
                sb.Append($"{node} --> ");
            }
            return sb.ToString().Substring(0, sb.Length - 5);
        }
    }
    
}
