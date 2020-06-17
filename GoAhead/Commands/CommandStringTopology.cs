using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoAhead.Commands
{
    public class CommandStringTopology
    {
        public enum TopologyType { CompleteCommand, CommandTag, ArgumentNames, ArgumentValues, Comment };

        public void Add(TopologyType type, int from, int to)
        {
            if (!this.m_borders.ContainsKey(type))
            {
                this.m_borders.Add(type, new List<Tuple<int, int>>());
            }
            this.m_borders[type].Add(new Tuple<int, int>(from, to));
        }

        public IEnumerable<Tuple<int, int>> GetBorders(TopologyType type)
        {
            if (this.m_borders.ContainsKey(type))
            {
                foreach (Tuple<int, int> range in this.m_borders[type])
                {
                    yield return range;
                }
            }
        }

        public IEnumerable<Tuple<TopologyType, Tuple<int, int>>> GetBorders()
        {
            foreach(KeyValuePair<TopologyType, List<Tuple<int, int>>> t in this.m_borders)
            {
                foreach (Tuple<int, int> range in t.Value)
                {
                    Tuple<TopologyType, Tuple<int, int>> next = new Tuple<TopologyType, Tuple<int, int>>(t.Key, range);
                    yield return next;
                }
            }
        }


        private Dictionary<TopologyType, List<Tuple<int, int>>> m_borders = new Dictionary<TopologyType, List<Tuple<int, int>>>();
    }
}
