using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.Objects;

namespace GoAhead.FPGA
{
    public enum SelectionUpdateAction { None, ClearSelection, AddToSelection, RemoveFromSelection, ClearAllUserSelections, ClearUserSelection, AddToUserSelection, AddCurrentSelectionToUserSelection, InversionComplete }

    public class SelectionUpdate
    {
        public SelectionUpdate()
        {
            Action = SelectionUpdateAction.None;
            AffecetedTileKey = null;
            UserSelectionType = "";
        }

        public SelectionUpdateAction Action { get; set; }
        public TileKey AffecetedTileKey { get; set; }
        public string UserSelectionType { get; set; }
    }

    public class TileSelectionManager : Interfaces.Subject, Interfaces.IResetable
    {
        bool m_ongoingIncrementalSelection = false;

        public bool OngoingIncrementalSelection
        {
            get { return m_ongoingIncrementalSelection; }
            set { m_ongoingIncrementalSelection = value; }       
        }
        
        private SelectionUpdate m_updateAction = new SelectionUpdate();

        private TileSelectionManager()
        {
            Commands.Debug.PrintGoAheadInternals.ObjectsToPrint.Add(this);
        }

        public void Reset()
        {
            ClearSelection();
            ClearUserSelections();
        }

        /// <summary>
        /// The class FPGA is a Singelton
        /// </summary>
        public static TileSelectionManager Instance = new TileSelectionManager();

        public void ClearSelection()
        {
            // notify before clear
            m_updateAction.Action = SelectionUpdateAction.ClearSelection;
            m_updateAction.AffecetedTileKey = null;

            Notfiy(m_updateAction);

            // now clear
            m_selectedTiles.Clear();
        }

        public void ClearUserSelections()
        {
            // notify before clear
            m_updateAction.Action = SelectionUpdateAction.ClearAllUserSelections;
            m_updateAction.AffecetedTileKey = null;

            Notfiy(m_updateAction);

            m_userSelectedTileKeys.Clear();
            m_userSelectionTypes.Clear();
        }

        /// <summary>
        /// Clears the user selection (if any)
        /// </summary>
        /// <param name="type"></param>
        public void ClearUserSelection(string type)
        {
            // notify before clear
            m_updateAction.Action = SelectionUpdateAction.ClearUserSelection;
            m_updateAction.AffecetedTileKey = null;
            m_updateAction.UserSelectionType = type;

            Notfiy(m_updateAction);

            if (m_userSelectedTileKeys.ContainsKey(type))
            {
                m_userSelectedTileKeys[type].Clear();
            }
            if (m_userSelectionTypes.Contains(type))
            {
                m_userSelectionTypes.Remove(type);
            }
        }

        public int NumberOfSelectedTiles
        {
            get { return m_selectedTiles.Count; }
        }

        public bool HasUserSelection(string userSelName)
        {
            return m_userSelectedTileKeys.ContainsKey(userSelName);
        }

        /// <summary>
        /// Add tileKey to selection if it is not contained already
        /// </summary>
        /// <param name="tileKey"></param>
        public void AddToSelection(TileKey tileKey)
        {
            if (!m_selectedTiles.Contains(tileKey))
            {
                m_selectedTiles.Add(tileKey);
                // call after add
                m_updateAction.Action = SelectionUpdateAction.AddToSelection;
                m_updateAction.AffecetedTileKey = tileKey;

                Notfiy(m_updateAction);
            }
        }

        /// <summary>
        /// Add tileKey to selection if it is not contained already
        /// </summary>
        /// <param name="tileKey"></param>
        public void AddToSelection(TileKey tileKey, bool notify)
        {
            if (!m_selectedTiles.Contains(tileKey))
            {
                m_selectedTiles.Add(tileKey);
                if (notify)
                {
                    // call after add
                    m_updateAction.Action = SelectionUpdateAction.AddToSelection;
                    m_updateAction.AffecetedTileKey = tileKey;

                    Notfiy(m_updateAction);
                }
            }
        }

        /// <summary>
        /// Send an inversion complete event to all observers
        /// </summary>
        /// <param name="tileKey"></param>
        public void SelectionChanged()
        {
            // call after add
            m_updateAction.Action = SelectionUpdateAction.InversionComplete;
            m_updateAction.AffecetedTileKey = null;

            Notfiy(m_updateAction);
        }

        public void RemoveFromSelection(int x, int y)
        {
            if (m_selectedTiles.Contains(x, y))
            {
                m_selectedTiles.Remove(x, y);

                m_updateAction.Action = SelectionUpdateAction.RemoveFromSelection;
                m_updateAction.AffecetedTileKey = new TileKey(x, y);

                Notfiy(m_updateAction);
            }
        }

        public void RemoveFromSelection(TileKey tileKey, bool notify)
        {
            if (m_selectedTiles.Contains(tileKey))
            {
                m_selectedTiles.Remove(tileKey);

                if (notify)
                {
                    m_updateAction.Action = SelectionUpdateAction.RemoveFromSelection;
                    m_updateAction.AffecetedTileKey = tileKey;

                    Notfiy(m_updateAction);
                }
            }
        }

        public void AddCurrentSelectionToUserSelection(string type)
        {
            if (!m_userSelectedTileKeys.ContainsKey(type))
            {
                m_userSelectedTileKeys.Add(type, new TileSet());
            }

            if (!m_userSelectionTypes.Contains(type))
            {
                m_userSelectionTypes.Add(type);
            }

            // copy current user selection to type
            foreach (Tile t in m_selectedTiles)
            {
                // add new items
                if (!m_userSelectedTileKeys[type].Contains(t.TileKey))
                {
                    m_userSelectedTileKeys[type].Add(t.TileKey);
                }
                // or remove previously added items
                else
                {
                    m_userSelectedTileKeys[type].Remove(t.TileKey);
                }
            }

            m_updateAction.Action = SelectionUpdateAction.AddCurrentSelectionToUserSelection;
            m_updateAction.UserSelectionType = type;

            Notfiy(m_updateAction);
        }

        public IEnumerable<Tile> GetSelectedTiles()
        {
            foreach (Tile next in m_selectedTiles)
            {
                yield return next;
            }
        }

        public bool IsSelected(Tile tile)
        {
            return m_selectedTiles.Contains(tile.TileKey);
        }

        public bool IsSelected(TileKey tileKey)
        {
            return m_selectedTiles.Contains(tileKey);
        }

        public bool IsSelected(string location)
        {
            Tile t = FPGA.Instance.GetTile(location);
            return IsSelected(t.TileKey);
        }

        public bool IsSelected(int x, int y)
        {
            return m_selectedTiles.Contains(x, y);
        }

        public bool IsUserSelected(Tile tile, string type)
        {
            return IsUserSelected(tile.TileKey, type);
        }

        public bool IsUserSelected(TileKey tileKey, string type)
        {
            if (m_userSelectedTileKeys.ContainsKey(type))
            {
                return m_userSelectedTileKeys[type].Contains(tileKey);
            }
            else
            {
                // no tileKey found
                return false;
            }
        }

        public bool IsUserSelected(TileKey tileKey)
        {
            foreach (KeyValuePair<string, TileSet> userSel in m_userSelectedTileKeys)
            {
                if (userSel.Value.Contains(tileKey))
                {
                    return true;
                }
            }

            // no tileKey found
            return false;
        }

        /// <summary>
        /// Get the first Tile in user selection that matches on filter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Tile GetFirstUserSelectedTile(string type, string filter, FPGATypes.Placement placement)
        {
            if (!m_userSelectedTileKeys.ContainsKey(type))
            {
                throw new ArgumentException("No user selection " + type + " found");
            }

            int lowerX = int.MaxValue;
            int lowerY = int.MaxValue;

            int upperX = int.MinValue;
            int upperY = int.MinValue;

            Regex tileFilter = new Regex(filter, RegexOptions.Compiled);

            foreach (Tile tile in m_userSelectedTileKeys[type].Where(t => tileFilter.IsMatch(t.Location)))
            {
                if (tile.TileKey.X < lowerX)
                {
                    lowerX = tile.TileKey.X;
                }
                if (tile.TileKey.X > upperX)
                {
                    upperX = tile.TileKey.X;
                }
                if (tile.TileKey.Y < lowerY)
                {
                    lowerY = tile.TileKey.Y;
                }
                if (tile.TileKey.Y > upperY)
                {
                    upperY = tile.TileKey.Y;
                }
            }

            switch (placement)
            {
                case FPGATypes.Placement.UpperLeft:
                    return FPGA.Instance.GetTile(lowerX, lowerY);
                case FPGATypes.Placement.UpperRight:
                    return FPGA.Instance.GetTile(upperX, lowerY);
                case FPGATypes.Placement.LowerLeft:
                    return FPGA.Instance.GetTile(lowerX, upperY);
                case FPGATypes.Placement.LowerRight:
                    return FPGA.Instance.GetTile(upperX, upperY);
                default:
                    throw new ArgumentException("Placement " + placement + " not implemented");
            }
        }

        public Tile GetFirstSelectedTile(FPGATypes.Placement placement, IdentifierManager.RegexTypes tileIdentifier)
        {
            int lowerX = int.MaxValue;
            int lowerY = int.MaxValue;

            int upperX = int.MinValue;
            int upperY = int.MinValue;

            foreach (Tile tile in m_selectedTiles.Where(t => IdentifierManager.Instance.IsMatch(t.Location, tileIdentifier)))
            {
                TileKey key = tile.TileKey;

                if (key.X < lowerX)
                {
                    lowerX = key.X;
                }
                if (key.X > upperX)
                {
                    upperX = key.X;
                }
                if (key.Y < lowerY)
                {
                    lowerY = key.Y;
                }
                if (key.Y > upperY)
                {
                    upperY = key.Y;
                }
            }
            switch (placement)
            {
                case FPGATypes.Placement.UpperLeft:
                    return FPGA.Instance.GetTile(lowerX, lowerY);
                case FPGATypes.Placement.UpperRight:
                    return FPGA.Instance.GetTile(upperX, lowerY);
                case FPGATypes.Placement.LowerLeft:
                    return FPGA.Instance.GetTile(lowerX, upperY);
                case FPGATypes.Placement.LowerRight:
                    return FPGA.Instance.GetTile(upperX, upperY);
                default:
                    throw new ArgumentException("Placement " + placement + " not implemented");
            }
        }

        public Tile GetSelectedTile(string filter, FPGATypes.Placement placement)
        {
            return FindTileInSelection(filter, placement, m_selectedTiles);
        }

        public Tile GetUserSelectedTile(string filter, string userSelType, FPGATypes.Placement placement)
        {
            return FindTileInSelection(filter, placement, m_userSelectedTileKeys[userSelType]);
        }

        private Tile FindTileInSelection(string filter, FPGATypes.Placement placement, IEnumerable<Tile> selection)
        {
            int lowerX = int.MaxValue;
            int lowerY = int.MaxValue;

            int upperX = int.MinValue;
            int upperY = int.MinValue;

            bool xFound = false;
            bool yFound = false;

            Regex tileFilter = new Regex(filter, RegexOptions.Compiled);

            foreach (Tile tile in selection.Where(t => tileFilter.IsMatch(t.Location)))
            {
                if (tile.TileKey.X < lowerX)
                {
                    lowerX = tile.TileKey.X;
                    xFound = true;
                }
                if (tile.TileKey.X > upperX)
                {
                    upperX = tile.TileKey.X;
                    xFound = true;
                }

                if (tile.TileKey.Y < lowerY)
                {
                    lowerY = tile.TileKey.Y;
                    yFound = true;
                }

                if (tile.TileKey.Y > upperY)
                {
                    upperY = tile.TileKey.Y;
                    yFound = true;
                }
            }
            if (xFound && yFound)
            {
                switch (placement)
                {
                    case FPGATypes.Placement.UpperLeft:
                        return FPGA.Instance.GetTile(lowerX, lowerY);
                    case FPGATypes.Placement.UpperRight:
                        return FPGA.Instance.GetTile(upperX, lowerY);
                    case FPGATypes.Placement.LowerLeft:
                        return FPGA.Instance.GetTile(lowerX, upperY);
                    case FPGATypes.Placement.LowerRight:
                        return FPGA.Instance.GetTile(upperX, upperY);
                    default:
                        throw new ArgumentException("Placement " + placement + " not implemented");
                }
            }
            else
            {
                return null;
                //throw new ArgumentException("No tile found in current selection that matches " + filter);
            }
        }

        public void GetCenterOfSelection(Predicate<Tile> p, out double xCenter, out double yCenter)
        {
            xCenter = 0;
            yCenter = 0;

            int tileCount = 0;
            foreach (Tile t in GetSelectedTiles().Where(tile => p(tile)))
            {
                xCenter += t.TileKey.X;
                yCenter += t.TileKey.Y;
                tileCount++;
            }

            xCenter /= tileCount;
            yCenter /= tileCount;
        }

        public void GetCenterOfTiles(IEnumerable<Tile> tiles, out double xCenter, out double yCenter)
        {
            xCenter = 0;
            yCenter = 0;

            int tileCount = 0;
            foreach (Tile t in tiles)
            {
                xCenter += t.TileKey.X;
                yCenter += t.TileKey.Y;
                tileCount++;
            }

            xCenter /= tileCount;
            yCenter /= tileCount;
        }

        public int GetUserSelectionTileCount(string type)
        {
            if (m_userSelectedTileKeys.ContainsKey(type))
            {
                return m_userSelectedTileKeys[type].Count;
            }
            else
            {
                return 0;
            }
        }

        public IEnumerable<Tile> GetAllUserSelectedTiles()
        {
            foreach (string type in m_userSelectionTypes)
            {
                foreach (Tile tile in GetAllUserSelectedTiles(type))
                {
                    yield return tile;
                }
            }
        }

        public IEnumerable<Tile> GetAllUserSelectedTiles(string type)
        {
            if (m_userSelectedTileKeys.ContainsKey(type))
            {
                foreach (Tile t in m_userSelectedTileKeys[type])
                {
                    yield return t;
                }
            }
            else
            {
                throw new ArgumentException("No user selection " + type + " found");
            }
        }

        public List<Command> GetListOfAddToSelectionXYCommandsForUserSelection(string type)
        {
            List<Command> result = new List<Command>();

            Tile upperLeft = FindTileInSelection("", FPGATypes.Placement.UpperLeft, m_userSelectedTileKeys[type]);
            Tile lowerRight = FindTileInSelection("", FPGATypes.Placement.LowerRight, m_userSelectedTileKeys[type]);

            if (upperLeft == null || lowerRight == null)
            {
                throw new ArgumentException("Nothing selected, but GetListOfAddToSelectionXYCommandsForUserSelection was called");
            }

            AddToSelectionXY addCmd = new AddToSelectionXY();
            addCmd.LowerRightX = lowerRight.TileKey.X;
            addCmd.LowerRightY = lowerRight.TileKey.Y;
            addCmd.UpperLeftX = upperLeft.TileKey.X;
            addCmd.UpperLeftY = upperLeft.TileKey.Y;

            result.Add(addCmd);
            result.Add(new ExpandSelection());
            return result;
        }

        public BindingList<string> UserSelectionTypes
        {
            get { return m_userSelectionTypes; }
        }

        /// <summary>
        /// Return those tiles in user selection that match posFilter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="posFilter"></param>
        /// <returns></returns>
        public IEnumerable<Tile> GetUserSelection(string type, Regex posFilter)
        {
            foreach (Tile t in GetAllUserSelectedTiles(type))
            {
                if (posFilter.IsMatch(t.Location))
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection">the current selection or a user selection</param>
        /// <param name="clbs"></param>
        /// <param name="slices"></param>
        /// <param name="brams"></param>
        /// <param name="dsps"></param>
        public void GetRessourcesInSelection(IEnumerable<Tile> selection, out int clbs, out int brams, out int dsps)
        {
            clbs = selection.Count(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB));
            brams = selection.Count(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.BRAM));
            dsps = selection.Count(t => IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.DSP));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selection">the current selection or a user selection</param>
        /// <param name="clbs"></param>
        /// <param name="slices"></param>
        /// <param name="brams"></param>
        /// <param name="dsps"></param>
        public Dictionary<IdentifierManager.RegexTypes, int> GetRessourcesInSelection(IEnumerable<Tile> selection, IEnumerable<IdentifierManager.RegexTypes> types)
        {
            Dictionary<IdentifierManager.RegexTypes, int> result = new Dictionary<IdentifierManager.RegexTypes, int>();
            foreach (Tile t in selection)
            {
                foreach (IdentifierManager.RegexTypes resourceTpye in types)
                {
                    if (IdentifierManager.Instance.IsMatch(t.Location, resourceTpye))
                    {
                        if (!result.ContainsKey(resourceTpye))
                        {
                            result.Add(resourceTpye, 0);
                        }
                        result[resourceTpye]++;
                    }
                    foreach (Slice s in t.Slices)
                    {
                        if (IdentifierManager.Instance.IsMatch(s.SliceName, resourceTpye))
                        {
                            if (!result.ContainsKey(resourceTpye))
                            {
                                result.Add(resourceTpye, 0);
                            }
                            result[resourceTpye]++;
                        }
                    }
                }
            }

            return result;
        }

        public void RemoveUserSelection(string type)
        {
            m_userSelectedTileKeys.Remove(type);
            m_userSelectionTypes.Remove(type);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("There are currently " + m_selectedTiles.Count + " selected tiles. Use PrintAllSelectedTiles for details");
            if (m_userSelectionTypes.Count == 0)
            {
                result.AppendLine("There are currently no user selections");
            }
            else
            {
                result.AppendLine("The following user selections exist");
                foreach (KeyValuePair<string, TileSet> userSelections in m_userSelectedTileKeys)
                {
                    result.AppendLine(userSelections.Key + " contains " + userSelections.Value.Count + " tiles");
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// current selection
        /// </summary>
        //private Dictionary<TileKey, bool> m_selectedTileKeys = new Dictionary<TileKey, bool>();
        //private Dictionary<int, Dictionary<int, bool>> m_selectedTileKeys = new Dictionary<int, Dictionary<int, bool>>();
        private TileSet m_selectedTiles = new TileSet();

        /// <summary>
        /// saved collections
        /// </summary>
        private Dictionary<string, TileSet> m_userSelectedTileKeys = new Dictionary<string, TileSet>();

        private BindingList<string> m_userSelectionTypes = new BindingList<string>();
    }

    public class RAMSelectionManager : Interfaces.IResetable
    {
        private RAMSelectionManager()
        {
        }

        public static RAMSelectionManager Instance = new RAMSelectionManager();

        /// <summary>
        /// map a tile within a ram block to all tiles that are part of this ram block
        /// </summary>
        //private Dictionary<Tile, List<Tile>> m_ramTileMapping = new Dictionary<Tile, List<Tile>>();
        private Dictionary<int, Dictionary<int, TileSet>> m_ramTileMapping = new Dictionary<int, Dictionary<int, TileSet>>();

        public void Reset()
        {
            m_ramTileMapping.Clear();
        }

        public bool HasMappings
        {
            get { return m_ramTileMapping.Count != 0; }
        }

        public bool HasMapping(Tile lowerRightTile)
        {
            if (!m_ramTileMapping.ContainsKey(lowerRightTile.TileKey.X))
            {
                return false;
            }
            else if (!m_ramTileMapping[lowerRightTile.TileKey.X].ContainsKey(lowerRightTile.TileKey.Y))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IEnumerable<Tile> GetRamBlockMembers(Tile lowerRightTile)
        {
            if (!HasMapping(lowerRightTile))
            {
                throw new ArgumentException("No ram block mapping found for " + lowerRightTile);
            }

            return m_ramTileMapping[lowerRightTile.TileKey.X][lowerRightTile.TileKey.Y];
        }

        public void UpdateMapping()
        {
            int width;
            int height;
            TileSet ramTiles;
            bool success = FPGATypes.GetRamBlockSize("", out width, out height, out ramTiles);

            if (!success)
            {
                return;
            }

            BRAMDSPSetting bs = BRAMDSPSettingsManager.Instance.GetBRAMSetting();

            m_ramTileMapping.Clear();

            foreach (Tile ramTile in ramTiles)
            {
                // collect all tiles that are part of the ram block
                List<Tile> ramBlockMembers = new List<Tile>();
                if (!bs.LeftRightHandling)
                {
                    // ramTile is the lower right tile in a ram block
                    for (int x = ramTile.TileKey.X - (width - 1); x <= ramTile.TileKey.X; x++)
                    {
                        for (int y = ramTile.TileKey.Y - (height - 1); y <= ramTile.TileKey.Y; y++)
                        {
                            ramBlockMembers.Add(FPGA.Instance.GetTile(x, y));
                        }
                    }
                }
                else
                {
                    if (Regex.IsMatch(ramTile.Location, bs.ButtomLeft))
                    {
                        for (int x = ramTile.TileKey.X; x <= ramTile.TileKey.X + (width - 1); x++)
                        {
                            for (int y = ramTile.TileKey.Y - (height - 1); y <= ramTile.TileKey.Y; y++)
                            {
                                ramBlockMembers.Add(FPGA.Instance.GetTile(x, y));
                            }
                        }
                    }
                    else if (Regex.IsMatch(ramTile.Location, bs.ButtomRight))
                    {
                        for (int x = ramTile.TileKey.X; x >= ramTile.TileKey.X - (width - 1); x--)
                        {
                            for (int y = ramTile.TileKey.Y - (height - 1); y <= ramTile.TileKey.Y; y++)
                            {
                                ramBlockMembers.Add(FPGA.Instance.GetTile(x, y));
                            }
                        }
                    }
                }

                // ramBlockMembers contains the anchor
                foreach (Tile member in ramBlockMembers)
                {
                    if (!m_ramTileMapping.ContainsKey(member.TileKey.X))
                    {
                        m_ramTileMapping.Add(member.TileKey.X, new Dictionary<int, TileSet>());
                    }
                    if (!m_ramTileMapping[member.TileKey.X].ContainsKey(member.TileKey.Y))
                    {
                        m_ramTileMapping[member.TileKey.X].Add(member.TileKey.Y, new TileSet());
                    }

                    m_ramTileMapping[member.TileKey.X][member.TileKey.Y].Add(ramBlockMembers);
                }
            }
        }
    }
}