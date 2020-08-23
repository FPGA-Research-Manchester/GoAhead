using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.Settings;
using GoAhead.Objects;

namespace GoAhead.GUI
{
    class AllTilesPainter : Painter, Interfaces.IObserver, Interfaces.IResetable
    {
        public AllTilesPainter(FPGAViewCtrl view)
            : base(view)
        {
            TileSelectionManager.Instance.Add(this);
        }

        public void Notify(object obj)
        {
            if (obj == null)
            {
                return;
            }

            if (TileBitmap == null)
            {
                return;
            }

            if (obj is SelectionUpdate)
            {
                SelectionUpdate((SelectionUpdate)obj);
            }
            //this.Invalidate();
        }
        
        private void SelectionUpdate(SelectionUpdate selUpdate)
        {
            // ram drawing 
            if (!RAMSelectionManager.Instance.HasMappings)
            {
                RAMSelectionManager.Instance.UpdateMapping();
            }
            
            Graphics graphicsObj = Graphics.FromImage(TileBitmap);

            switch (selUpdate.Action)
            {
                case (SelectionUpdateAction.AddCurrentSelectionToUserSelection):
                    {
                        foreach (Tile t in TileSelectionManager.Instance.GetUserSelection(selUpdate.UserSelectionType, m_view.TileRegex))
                        {
                            if(IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                            {
                                Tile intTile = FPGATypes.GetInterconnectTile(t);
                                DrawTile(intTile, graphicsObj, true, true);
                            }
                            DrawTile(t, graphicsObj, true, true);
                        }
                        break;
                    }
                case (SelectionUpdateAction.AddToSelection):
                case (SelectionUpdateAction.AddToUserSelection):
                    {
                        Tile t = FPGA.FPGA.Instance.GetTile(selUpdate.AffecetedTileKey);
                        DrawTile(t, graphicsObj, true, true);
                        break;
                    }
                case (SelectionUpdateAction.ClearAllUserSelections):
                    {
                        foreach (Tile t in TileSelectionManager.Instance.GetAllUserSelectedTiles())
                        {
                            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                            {
                                Tile intTile = FPGATypes.GetInterconnectTile(t);
                                DrawTile(intTile, graphicsObj, true, false);
                            }
                            DrawTile(t, graphicsObj, true, false);
                        }
                        break;
                    }
                case (SelectionUpdateAction.ClearSelection):
                    {
                        foreach (Tile tile in TileSelectionManager.Instance.GetSelectedTiles().Where(t => m_view.TileRegex.IsMatch(t.Location)))
                        {
                            if (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB))
                            {
                                Tile intTile = FPGATypes.GetInterconnectTile(tile);
                                DrawTile(intTile, graphicsObj, false, true);
                            }
                            DrawTile(tile, graphicsObj, false, true);
                        }
                        break;
                    }
                case (SelectionUpdateAction.ClearUserSelection):
                    {
                        foreach (Tile t in TileSelectionManager.Instance.GetAllUserSelectedTiles(selUpdate.UserSelectionType))
                        {
                            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                            {
                                Tile intTile = FPGATypes.GetInterconnectTile(t);
                                DrawTile(intTile, graphicsObj, true, false);
                            }
                            DrawTile(t, graphicsObj, true, false);
                        }
                        break;
                    }
                case (SelectionUpdateAction.RemoveFromSelection):
                    {
                        Tile t = FPGA.FPGA.Instance.GetTile(selUpdate.AffecetedTileKey);
                        if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                        {
                            Tile intTile = FPGATypes.GetInterconnectTile(t);
                            DrawTile(intTile, graphicsObj, false, true);
                        }
                        DrawTile(t, graphicsObj, false, true);
                        break;
                    }
                case (SelectionUpdateAction.InversionComplete):
                    {
                        foreach (Tile t in FPGA.FPGA.Instance.GetAllTiles())
                        {
                            DrawTile(t, graphicsObj, true, true);
                        }
                        break;
                    }

                default:
                    {
                        throw new ArgumentException("Unexpected UpdateAction: " + selUpdate.Action);
                    }
            }
        }

        public override void DrawTiles(bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            int x = FPGA.FPGA.Instance.MaxX;
            int y = FPGA.FPGA.Instance.MaxY;

            // add 1 due to zero based indeces
            TileBitmap = new Bitmap(
                (int)((x + 1) * m_view.TileSize), 
                (int)((y + 1) * m_view.TileSize),
                m_pixelFormat);

            Graphics graphicsObj = Graphics.FromImage(TileBitmap);

            graphicsObj.Clear(Color.Gray);

            foreach (Tile tile in GetAllTiles())
            {
                DrawTile(tile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }

            // get ram block data            
            if (!m_ramDataValid)
            {
                m_ramDataValid = FPGATypes.GetRamBlockSize(m_view.TileRegex, out m_ramBlockWidth, out m_ramBlockHeight, out m_ramTiles, out m_BRAM_right, out m_DSP_left, out m_connectsRAM);
            }

            foreach (Tile ramTile in m_ramTiles)
            {
                if (m_view.TileRegex.IsMatch(ramTile.Location))
                {
                    continue;
                }

                DrawTile(ramTile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }

            graphicsObj.Dispose();
        }

        public override void DrawTile(Tile tile, Graphics graphicsObj, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            if (!m_view.TileRegex.IsMatch(tile.Location))
            {
                return;
            }

            int upperLeftX = tile.TileKey.X * m_view.TileSize;
            int upperLeftY = tile.TileKey.Y * m_view.TileSize;

            m_rect.X = upperLeftX-1;
            m_rect.Y = upperLeftY-1;
            m_rect.Width = m_view.TileSize - 2;
            m_rect.Height = m_view.TileSize - 2;

            m_sb.Color = m_view.GetColor(tile, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            graphicsObj.FillRectangle(m_sb, m_rect);
        }

        public override void Reset()
        {
            m_ramDataValid = false;
        }
        
        private bool m_ramDataValid = false;
        private TileSet m_ramTiles = new TileSet();
        private TileSet m_BRAM_right = new TileSet();
        private TileSet m_DSP_left = new TileSet();
        private TileSet m_connectsRAM = new TileSet();
        private int m_ramBlockWidth = 0;
        private int m_ramBlockHeight = 0;
    }
}
