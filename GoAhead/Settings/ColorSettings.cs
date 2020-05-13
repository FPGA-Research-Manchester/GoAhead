using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Interfaces;

namespace GoAhead.Settings
{
    public class ColorSettings : IResetable
    {
        private ColorSettings()
        {
            SelectionColor = Color.Green;
            MacroColor = Color.Yellow;
            BlockedPortsColor = Color.Red;

            Color userSelIncr = Color.FromArgb(230, 0, 0);
            UserSelectionIncrement = userSelIncr;

            Color selIncr = Color.FromArgb(150, 150, 150);
            SelectionIncrement = selIncr;

            Color blockedPorts = Color.FromArgb(200, 200, 200);
            BlockedPortsColor = blockedPorts;

            Commands.Reset.ObjectsToReset.Add(this);
        }

        public void Reset()
        {
            m_bufferedColors.Clear();
        }

        public static ColorSettings Instance = new ColorSettings();

        public Color GetColor(Tile tile)
        {
            if (tile.HasNonstopoverBlockedPorts)
            {
                return BlockedPortsColor;
            }

            if (!ColorBufferContains(tile))
            {
                bool colorFound = false;
                foreach (string str in m_colorSettings.Keys)
                {
                    if (Regex.IsMatch(tile.Location, str))
                    {
                        Add(tile, m_colorSettings[str]);
                        colorFound = true;
                        break;
                    }
                }

                // no match, set default color
                if (!colorFound)
                {
                    Add(tile, Color.DarkGreen);
                }
            }
            else
            {
            }

            return m_bufferedColors[tile.TileKey.X][tile.TileKey.Y];
        }

        private bool ColorBufferContains(Tile t)
        {
            if (!m_bufferedColors.ContainsKey(t.TileKey.X))
            {
                return false;
            }

            return m_bufferedColors[t.TileKey.X].ContainsKey(t.TileKey.Y);
        }

        private void Add(Tile t, Color c)
        {
            if (!m_bufferedColors.ContainsKey(t.TileKey.X))
            {
                m_bufferedColors.Add(t.TileKey.X, new Dictionary<int, Color>());
            }
            m_bufferedColors[t.TileKey.X].Add(t.TileKey.Y, c);
        }

        public IEnumerable<string> GetTileRegexps()
        {
            return m_colorSettings.Keys;
        }

        public void SetColor(string tileRegexp, string colorName)
        {
            m_colorSettings[tileRegexp] = Color.FromName(colorName);
            m_bufferedColors.Clear();
        }

        public Color SelectionColor { get; set; }
        public Color MacroColor { get; set; }
        public Color BlockedPortsColor { get; set; }
        public Color SelectionIncrement { get; set; }
        public Color UserSelectionIncrement { get; set; }

        private Dictionary<string, Color> m_colorSettings = new Dictionary<string, Color>();

        private Dictionary<int, Dictionary<int, Color>> m_bufferedColors = new Dictionary<int, Dictionary<int, Color>>();
    }
}