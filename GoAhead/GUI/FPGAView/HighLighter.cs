using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.GUI
{
    public abstract class HighLighter
    {
        public HighLighter(FPGAViewCtrl view)
        {
            m_view = view;
        }

        public abstract void HighLight(Graphics graphicsObj);

        protected FPGAViewCtrl m_view = null;
    }

    public class SelectionHighLighter : HighLighter, Interfaces.IResetable
    {
        public SelectionHighLighter(FPGAViewCtrl view)
            : base(view)
        {
            m_pen = new Pen(Color.Red, 3);// view.TileSize / 2);

            Commands.Reset.ObjectsToReset.Add(this);
        }

        public override void HighLight(Graphics graphicsObj)
        {
            if (!Settings.StoredPreferences.Instance.HighLightSelection)
            {
                return;
            }

            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                return;
            }

            foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles())
            {
                int upperLeftX = tile.TileKey.X * m_view.TileSize;
                int upperLeftY = tile.TileKey.Y * m_view.TileSize;

                Rectangle rect = new Rectangle(upperLeftX, upperLeftY, m_view.TileSize - 1, m_view.TileSize - 1);
                graphicsObj.DrawEllipse(m_pen, rect);
            }

        }

        public void Reset()
        {
            m_lines.Clear();
        }

        private Pen m_pen = null;
        private Dictionary<int, bool> m_lines = new Dictionary<int, bool>();
    }

    public class RAMHighLighter : HighLighter, Interfaces.IResetable
    {
        public RAMHighLighter(FPGAViewCtrl view)
            : base(view)
        {
            m_pen = new Pen(Color.Black, 2);// view.TileSize / 2);

            Commands.Reset.ObjectsToReset.Add(this);
        }

        public override void HighLight(Graphics graphicsObj)
        {
            if (!Settings.StoredPreferences.Instance.HighLightRAMS)
            {
                return;
            }

            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                return;
            }

            if (m_lines.Count == 0)
            {
                foreach (Tile currentTile in FPGA.FPGA.Instance.GetAllTiles().Where(t => Regex.IsMatch(t.Location, IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.BRAM, IdentifierManager.RegexTypes.DSP))))
                {
                    if (!m_lines.ContainsKey(currentTile.TileKey.Y))
                    {
                        m_lines.Add(currentTile.TileKey.Y, false);
                    }
                }
            }

            int maxX = FPGA.FPGA.Instance.GetMaxTileKeyX();

            foreach (int y in m_lines.Keys)
            {
                //Point from = new Point(0, (y+1) * this.m_view.TileSize);
                //Point to = new Point((maxX + 1) * this.m_view.TileSize, (y+1) * this.m_view.TileSize);
                graphicsObj.DrawLine(m_pen, 0, (y + 1) * m_view.TileSize, (maxX + 1) * m_view.TileSize, (y + 1) * m_view.TileSize);
            }
        }

        public void Reset()
        {
            m_lines.Clear();
        }

        private Pen m_pen = null;
        private Dictionary<int, bool> m_lines = new Dictionary<int, bool>();
    }

    public class ClockRegionHighlighter : HighLighter, Interfaces.IResetable
    {
        public ClockRegionHighlighter(FPGAViewCtrl view)
            : base(view)
        {
            m_pen = new Pen(Color.Red, m_view.TileSize / 2);

            Commands.Reset.ObjectsToReset.Add(this);
        }

        public override void HighLight(Graphics graphicsObj)
        {
            if (!Settings.StoredPreferences.Instance.HighLightClockRegions)
            {
                return;
            }

            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                return;
            }

            if (FPGA.FPGA.Instance.BackendType == FPGATypes.BackendType.Vivado)
            {
                DrawArchDefinedClockRegions(graphicsObj);
            }
            else
            {
                DrawUsedDefinedClockRegions(graphicsObj);
            }
        }

        private void DrawArchDefinedClockRegions(Graphics graphicsObj)
        {
            // sweep horizontal
            Tile upperLeftCLB = null;
            for (int x = 0; x < FPGA.FPGA.Instance.MaxY; x++)
            {
                for (int y = 0; y < FPGA.FPGA.Instance.MaxY; y++)
                {
                    Tile t = FPGA.FPGA.Instance.GetTile(x, y);
                    if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                    {
                        upperLeftCLB = t;
                        break;
                    }
                }
                if (upperLeftCLB != null)
                {
                    break;
                }
            }

            // sweep horizontally
            string cr = upperLeftCLB.ClockRegion;
            for (int x = upperLeftCLB.TileKey.X; x < FPGA.FPGA.Instance.MaxX; x++)
            {
                Tile t = FPGA.FPGA.Instance.GetTile(x, upperLeftCLB.TileKey.Y);
                if (!cr.Equals(t.ClockRegion) && !string.IsNullOrEmpty(t.ClockRegion))
                {
                    cr = t.ClockRegion;
                    Point from = new Point(x * m_view.TileSize, 0);
                    Point to = new Point(x * m_view.TileSize, (FPGA.FPGA.Instance.MaxY + 1) * m_view.TileSize);
                    graphicsObj.DrawLine(m_pen, from, to);
                }
            }
            // sweep vertically
            cr = upperLeftCLB.ClockRegion;
            for (int y = upperLeftCLB.TileKey.Y; y < FPGA.FPGA.Instance.MaxY; y++)
            {
                Tile t = FPGA.FPGA.Instance.GetTile(upperLeftCLB.TileKey.X, y);
                if (!cr.Equals(t.ClockRegion) && !string.IsNullOrEmpty(t.ClockRegion))
                {
                    cr = t.ClockRegion;

                    Point from = new Point((int)0, (int)(y * m_view.TileSize));
                    Point to = new Point((int)((FPGA.FPGA.Instance.MaxX+1) * m_view.TileSize), (int)(y * m_view.TileSize));
                    graphicsObj.DrawLine(m_pen, from, to);
                }
            }
        }

        private void DrawUsedDefinedClockRegions(Graphics graphicsObj)
        {
            int maxX = FPGA.FPGA.Instance.GetMaxTileKeyX();
            int maxY = FPGA.FPGA.Instance.GetMaxTileKeyY();

            if (m_anchors.Count == 0)
            {
                foreach (LineParameter lp in LineManager.Instance.GetLineParameter())
                {
                    Tuple<LineParameter, List<Tile>> tuple = new Tuple<LineParameter, List<Tile>>(lp, new List<Tile>());
                    tuple.Item2.AddRange(FPGA.FPGA.Instance.GetAllTiles().Where(t => lp.TileIdentifierRegexp.IsMatch(t.Location)));
                    m_anchors.Add(tuple);
                }
            }

            foreach (Tuple<LineParameter, List<Tile>> t in m_anchors)
            {
                switch (t.Item1.Orientation)
                {
                    case LineManager.Orienation.Horizontal:
                        {
                            t.Item2.ForEach(tile =>
                            {
                                int y = (int)((tile.TileKey.Y + t.Item1.Offset) * m_view.TileSize);
                                Point from = new Point(0,  y);
                                Point to = new Point((maxX + 1) * m_view.TileSize, y);
                                graphicsObj.DrawLine(m_pen, from, to);
                            }
                            );
                            break;
                        }
                    case LineManager.Orienation.Vertical:
                        {
                            t.Item2.ForEach(tile =>
                            {
                                int x = (tile.TileKey.X + t.Item1.Offset) * m_view.TileSize;
                                Point from = new Point(x, 0);
                                Point to = new Point(x, (maxY + 1) * m_view.TileSize);
                                graphicsObj.DrawLine(m_pen, from, to);
                            }
                            );
                            break;
                        }
                }
            }
        }

        public void Reset()
        {
            m_anchors.Clear();
        }

        private List<Tuple<LineParameter, List<Tile>>> m_anchors = new List<Tuple<LineParameter, List<Tile>>>();

        private Pen m_pen;
    }

    public class MacroHighLighter : HighLighter
    {
        public MacroHighLighter(FPGAViewCtrl view)
            : base(view)
        {
        }

        public override void HighLight(Graphics graphicsObj)
        {
            if (!Settings.StoredPreferences.Instance.HighLightPlacedMacros)
            {
                return;
            }

            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                return;
            }

            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(t => t.Slices.Any(s => s.Usage == FPGATypes.SliceUsage.Macro)))
            {
                int upperLeftX = tile.TileKey.X * m_view.TileSize;
                int upperLeftY = tile.TileKey.Y * m_view.TileSize;

                Rectangle rect = new Rectangle(upperLeftX, upperLeftY, m_view.TileSize - 1, m_view.TileSize - 1);

                graphicsObj.DrawRectangle(m_pen, rect);
            }
        }

        private readonly Pen m_pen = new Pen(Settings.ColorSettings.Instance.MacroColor);
    }

    public class PossibleMacroPlacementHighLighter : HighLighter, Interfaces.IResetable
    {
        public PossibleMacroPlacementHighLighter(FPGAViewCtrl view)
            : base(view)
        {
            Commands.Reset.ObjectsToReset.Add(this);
        }

        public string LibrayElementName
        {
            get { return m_libraryElement; }
            set
            {
                if (value.Equals(m_libraryElement))
                {
                    return;
                }
                m_libraryElement = value;
                m_tiles.Clear();
            }
        }

        public override void HighLight(Graphics graphicsObj)
        {
            if (!Settings.StoredPreferences.Instance.HighLightPossibleMacroPlacements)
            {
                return;
            }

            if (FPGA.FPGA.Instance.Family.Equals(FPGATypes.FPGAFamily.Undefined))
            {
                return;
            }

            if (!Library.Instance.Contains(m_libraryElement))
            {
                return;
            }

            if (m_tiles.Count == 0)
            {
                m_tiles = DesignRuleChecker.GetPossibleLibraryElementPlacements(m_libraryElement);
            }

            foreach (Tile tile in m_tiles)
            {
                int upperLeftX = tile.TileKey.X * m_view.TileSize;
                int upperLeftY = tile.TileKey.Y * m_view.TileSize;

                Rectangle rect = new Rectangle(upperLeftX, upperLeftY, m_view.TileSize - 1, m_view.TileSize - 1);

                graphicsObj.DrawRectangle(m_pen, rect);
            }
        }

        public void Reset()
        {
            m_tiles.Clear();
        }

        private string m_libraryElement = "";
        private List<Tile> m_tiles = new List<Tile>();
        private readonly Pen m_pen = new Pen(Settings.ColorSettings.Instance.MacroColor);
    }
}