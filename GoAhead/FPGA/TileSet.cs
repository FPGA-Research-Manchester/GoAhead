using GoAhead.Objects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GoAhead.FPGA
{
    [Serializable]
    public class TileSet : IEnumerable<Tile>
    {
        public void Add(TileKey tileKey)
        {
            Tile t = FPGA.Instance.GetTile(tileKey);
            Add(t);
        }

        public void Add(IEnumerable<Tile> tiles)
        {
            foreach (Tile t in tiles)
            {
                Add(t);
            }
        }

        public void Add(Tile tile)
        {
            if (!m_tiles.ContainsKey(tile.TileKey.X))
            {
                m_tiles.Add(tile.TileKey.X, new Dictionary<int, Tile>());
            }
            m_tiles[tile.TileKey.X].Add(tile.TileKey.Y, tile);
        }

        public void Clear()
        {
            m_tiles.Clear();
        }

        public bool Contains(Tile t)
        {
            return Contains(t.TileKey);
        }

        public bool Contains(TileKey key)
        {
            return Contains(key.X, key.Y);
        }

        public bool Contains(int x, int y)
            {
            if (!m_tiles.ContainsKey(x))
            {
                return false;
            }
            else if (!m_tiles[x].ContainsKey(y))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (int x in m_tiles.Keys)
                {
                    count += m_tiles[x].Count;
                }
                return count;
            }
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            foreach (int x in m_tiles.Keys)
            {
                foreach (int y in m_tiles[x].Keys)
                {
                    yield return m_tiles[x][y];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Tile GetTile(TileKey key)
        {
            return GetTile(key.X, key.Y);
        }

        public Tile GetTile(int x, int y)
        {
            if (!m_tiles.ContainsKey(x))
            {
                return null;
            }
            else if (!m_tiles[x].ContainsKey(y))
            {
                return null;
            }
            else
            {
                return m_tiles[x][y];
            }
        }

        public void Remove(TileKey key)
        {
            Remove(key.X, key.Y);
        }

        public void Remove(int x, int y)
        {
            if (m_tiles.ContainsKey(x))
            {
                if (m_tiles[x].ContainsKey(y))
                {
                    m_tiles[x].Remove(y);
                }
                if (m_tiles[x].Count == 0)
                {
                    m_tiles.Remove(x);
                }
            }
        }

        public void AddConnection(Location from, Location to, int cost)
        {
            if (m_dijkstraWires == null)
            {
                m_dijkstraWires = new Dictionary<Tuple<Location, Location>, int>();
            }
            try
            {
                m_dijkstraWires.Add(new Tuple<Location, Location>(from, to), cost);
            }
            catch (ArgumentException ex)
            {
                SetConnectionCost(from, to, cost);
            }
        }
        public int? GetConnectionCost(Location from, Location to)
        {
            try
            {
                return m_dijkstraWires[new Tuple<Location, Location>(from, to)];
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
        }
        private void SetConnectionCost(Location from, Location to, int newCost)
        {
            m_dijkstraWires[new Tuple<Location, Location>(from, to)] = newCost;
        }

        private Dictionary<int, Dictionary<int, Tile>> m_tiles = new Dictionary<int, Dictionary<int, Tile>>();

        private Dictionary<Tuple<Location, Location>, int> m_dijkstraWires = new Dictionary<Tuple<Location, Location>, int>();
    }
}