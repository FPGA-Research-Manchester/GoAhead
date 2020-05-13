using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.Settings;

namespace GoAhead.GUI
{
    public abstract class Painter : Interfaces.IResetable
    {
        public Painter(FPGAViewCtrl view)
        {
            m_view = view;
        }

        protected IEnumerable<Tile> GetAllTiles()
        {
            foreach (Tile tile in FPGA.FPGA.Instance.GetAllTiles().Where(p => m_view.TileRegex.IsMatch(p.Location)))
            {
                yield return tile;
            }
        }

        public void Invalidate()
        {
            if (TileBitmap != null)
            {
                Graphics graphicsObj = Graphics.FromImage(TileBitmap);
                HighLighter.ForEach(hl => hl.HighLight(graphicsObj));
            }
        }
        
        /// <summary>
        /// Redraw all
        /// </summary>
        public abstract void DrawTiles(bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles);

        /// <summary>
        /// Redraw given tile
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="graphicsObj"></param>
        /// <param name="addIncrementForSelectedTiles"></param>
        public abstract void DrawTile(Tile tile, Graphics graphicsObj, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles);

        public virtual void DrawSelection(bool selectionStarts, bool selectionEnds) { }

        public virtual void Reset() { }

        public Bitmap TileBitmap { get; set; }

        protected SolidBrush m_sb = new SolidBrush(Color.Red);
        public FPGAViewCtrl m_view = null;
        protected readonly PixelFormat m_pixelFormat = PixelFormat.Format16bppRgb555;

        protected Rectangle m_rect = new Rectangle();

        public List<HighLighter> HighLighter = new List<HighLighter>();
    }
}
