using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.Code.XDL.ResourceDescription;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead
{      
    [Serializable]
    public class UnresolvedWires : IEnumerable<Tuple<Tile, WireList>>
    {
        public UnresolvedWires()
        {             
            m_identifer = new SortedList<string, uint>();
            foreach (SwitchMatrix sm in FPGA.FPGA.Instance.GetAllSwitchMatrices())
            {
                foreach (Port p in sm.Ports)
                {
                    if (!m_identifer.ContainsKey(p.Name))
                    {
                        m_identifer.Add(p.Name, p.NameKey);
                    }
                }
            }
        }
        public void Add(Tile tile, Wire w)
        {
            if (!m_objects.ContainsKey(tile.TileKey.X))
            {
                m_objects.Add(tile.TileKey.X, new Dictionary<int, WireList>());
            }
            if (!m_objects[tile.TileKey.X].ContainsKey(tile.TileKey.Y))
            {
                m_objects[tile.TileKey.X].Add(tile.TileKey.Y, new WireList());
            }

            m_objects[tile.TileKey.X][tile.TileKey.Y].Add(w, false);
        }

        public void Set(Tile tile, WireList wl)
        {
            m_objects[tile.TileKey.X][tile.TileKey.Y].Clear();
            m_objects[tile.TileKey.X][tile.TileKey.Y] = wl;
        }

        public bool Contains(int x, int y)
        {
            if (!m_objects.ContainsKey(x))
            {
                return false;
            }
            else if (!m_objects[x].ContainsKey(y))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Remove(int x, int y)
        {
            m_objects[x].Remove(y);
        }

        public void ClearCache()
        {
            foreach (int x in m_objects.Keys)
            {
                foreach (int y in m_objects[x].Keys)
                {
                    m_objects[x][y].ClearWireKeys();
                    m_objects[x][y].ClearCache();
                }
            }
        }

        public bool ValidIdentifier(string identifier)
        {
            return m_identifer.ContainsKey(identifier);

        }
        /*
        public int Count(int x, int y)
        {
            if (!this.Contains(x, y))
            {
                return -1;
            }
            else
            {
                return this.m_objects[x][y].Count;
            }
        }*/

        public WireList Get(Tile key)
        {
            if (!m_objects.ContainsKey(key.TileKey.X))
            {
                return null;
            }
            else if (!m_objects[key.TileKey.X].ContainsKey(key.TileKey.Y))
            {
                return null;
            }

            return m_objects[key.TileKey.X][key.TileKey.Y];
        }

        public IEnumerator<Tuple<Tile, WireList>> GetEnumerator()
        {
            foreach (int x in m_objects.Keys)
            {
                foreach (int y in m_objects[x].Keys)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(x, y);
                    Tuple<Tile, WireList> result = new Tuple<Tile, WireList>(t, m_objects[x][y]);
                    yield return result;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private SortedList<string, uint> m_identifer = new SortedList<string, uint>();
        private Dictionary<int, Dictionary<int, WireList>> m_objects = new Dictionary<int, Dictionary<int, WireList>>();
    }
}

namespace GoAhead.Commands.Data
{
    [CommandDescription(Description = "Read an XDL resource file and build uop the routing model (incremental to a prior ReadXDL commmand)", Wrapper = true)]
    class ReadWireStatements : Command
    {
        protected override void DoCommandAction()
        {
            if (!HandleUnresolvedWires)
            {
                FPGA.FPGA.Instance.ClearWireList();
            }
            m_loopWires = 0;

            // create reader & open file
            XDLStreamReaderWithUndo sr = new XDLStreamReaderWithUndo(FileName);
            XDLTileParser tp = new XDLTileParser();

            UnresolvedWires unresWires = null;
            
            if (HandleUnresolvedWires)
            {
                unresWires = new UnresolvedWires();               
            }

            try
            {
                Regex tileFilter = new Regex(@"\t+\(tile", RegexOptions.Compiled);
                string line = "";
                int tileCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    // only consider tiles
                    if (tileFilter.IsMatch(line))
                    {
                        ProgressInfo.Progress = ProgressStart + ((int)((double)tileCount++ / (double)FPGA.FPGA.Instance.TileCount * ProgressShare));

                        int yPos;
                        int xPos;
                        string location;
                        XDLTileParser.GetTileHeaderData(line, out yPos, out xPos, out location);
                        Tile tile = FPGA.FPGA.Instance.GetTile(location);

                        Watch.Start("ParseWire");
                        tp.ParseWire(tile, sr, unresWires);
                        Watch.Stop("ParseWire");

                        if (HandleUnresolvedWires && tileCount % 10000 == 0)
                        {                         
                            ResolveWires(unresWires);
                            unresWires.ClearCache();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                throw error;
            }
            finally
            {
                sr.Close();
            }            

            if (!HandleUnresolvedWires)
            {
                // clean up data structures that were used for fast comparing wire list and are now no longer needed
                foreach (WireList wl in FPGA.FPGA.Instance.GetAllWireLists())
                {
                    wl.ClearWireKeys();
                }
            }
            else
            {
                ResolveWires(unresWires);
                OutputManager.WriteOutput("Added " + m_loopWires + " loop wires");
            }
        }

        private void ResolveWires(UnresolvedWires unresWires)
        {
            Watch.Start("ResolveWires");
            if (unresWires == null)
            {
                return;
            }

            List<Tuple<Tile, WireList>> updates = new List<Tuple<Tile, WireList>>();
            foreach (Tuple<Tile, WireList> tuple in unresWires.Where(t => t.Item1.SwitchMatrix.ArcCount != 0))
            {
                //Console.WriteLine(tuple.Item1.Location + " with " + tuple.Item2.Count);
                WireList wiresToNonExistingTiles = new WireList();
                foreach (Wire srcWire in tuple.Item2)
                {
                    Watch.Start("GetDestinationByWire 1");
                    Tile destTile = Navigator.GetDestinationByWire(tuple.Item1, srcWire);
                    Watch.Stop("GetDestinationByWire 1");

                    Watch.Start("contains");
                    bool contains = unresWires.Contains(destTile.TileKey.X, destTile.TileKey.Y);
                    Watch.Stop("contains");
                    if (!contains || !tuple.Item1.SwitchMatrix.Contains(new Port(srcWire.LocalPip)))
                    {
                        wiresToNonExistingTiles.Add(srcWire, false);
                        continue;
                    }                   

                    WireList dstWireList = unresWires.Get(destTile);
                    Watch.Start("inner loop");
                    foreach (Wire dstWire in dstWireList.Where(d => d.LocalPipKey.Equals(srcWire.PipOnOtherTileKey) && !d.PipOnOtherTile.Equals(srcWire.LocalPip)))
                    {
                        Watch.Start("GetDestinationByWire 2");
                        Tile otherTile = Navigator.GetDestinationByWire(destTile, dstWire);
                        Watch.Stop("GetDestinationByWire 2");

                        // tile a -> wire -> tile b -> wire -> tile a , but tile b has no switch matrix. only add wires to tiles with non empty switch matrices
                        // as otherwise the wire can not used
                        Watch.Start("Match");
                        bool match = otherTile.Location.Equals(tuple.Item1.Location) &&
                            //!dstWire.PipOnOtherTile.Equals(srcWire.LocalPip) &&
                            //tuple.Item1.SwitchMatrix.ArcCount > 0 &&
                            //tuple.Item1.SwitchMatrix.Contains(new Port(srcWire.LocalPip)) &&
                            tuple.Item1.SwitchMatrix.Contains(new Port(dstWire.PipOnOtherTile));
                        Watch.Stop("Match");

                        if (match)
                        {
                            //this.OutputManager.WriteOutput
                            //Console.WriteLine("Add loop wire on " + tuple.Item1.Location + " connecting " + srcWire.LocalPip + " with " + dstWire.PipOnOtherTile + " (via " + destTile.Location + ")");
                            Watch.Start("Parse");
                            Wire w = new Wire(srcWire.LocalPipKey, dstWire.PipOnOtherTileKey, true, 0, 0);
                            Watch.Stop("Parse");

                            // no duplicates
                            Watch.Start("HasWire");
                            if (!tuple.Item1.WireList.HasWire(w))
                            {
                                tuple.Item1.WireList.Add(w, false);
                                m_loopWires++;
                            }
                            Watch.Stop("HasWire");
                        }
                    }
                    Watch.Stop("inner loop");
                }

                if (wiresToNonExistingTiles.Count < tuple.Item2.Count)
                {
                    updates.Add(new Tuple<Tile, WireList>(tuple.Item1, wiresToNonExistingTiles));
                }
            }

            Watch.Start("Clean");
            foreach (Tuple<Tile, WireList> tuple in updates)
            {
                //Console.WriteLine(tuple.Item1.Location + " reduction from " + unresWires.Get(tuple.Item1).Count + " to " + tuple.Item2.Count);
                unresWires.Set(tuple.Item1, tuple.Item2);
                if (tuple.Item2.Count == 0)
                {
                    unresWires.Remove(tuple.Item1.TileKey.X, tuple.Item1.TileKey.Y);
                }
            }
            Watch.Stop("Clean");

            Watch.Stop("ResolveWires");
        }

        private int m_loopWires = 0;

        public override void Undo()
        {
            throw new NotImplementedException();
        }

        [Parameter(Comment = "The XDL file to read")]
        public string FileName = "";

        [Parameter(Comment = "")]
        public bool HandleUnresolvedWires = false;
       
    }
}
