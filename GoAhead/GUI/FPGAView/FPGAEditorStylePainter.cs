using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GoAhead.FPGA;
using GoAhead.Objects;

namespace GoAhead.GUI.FPGAView
{
    class FPGAEditorStylePainter : Painter, Interfaces.IObserver, Interfaces.IResetable
    {
        public FPGAEditorStylePainter(FPGAViewCtrl view)
            :base(view)
        {
            TileSelectionManager.Instance.Add(this);
            Commands.Reset.ObjectsToReset.Add(this);
        }

        public void Notify(object obj)
        {

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
                            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
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

            graphicsObj.Clear(Color.Black);

            foreach (Tile tile in GetAllTiles().Where(t => m_view.TileRegex.IsMatch(t.Location)))
            {
                DrawTile(tile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }

            graphicsObj.Dispose();
        }

        public override void DrawTile(FPGA.Tile tile, System.Drawing.Graphics graphicsObj, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            int upperLeftX = tile.TileKey.X * m_view.TileSize;
            int upperLeftY = tile.TileKey.Y * m_view.TileSize;

            m_rect.X = upperLeftX - 3;
            m_rect.Y = upperLeftY - 3;
            m_rect.Width = m_view.TileSize - 6;
            m_rect.Height = m_view.TileSize - 6;

            m_sb.Color = m_view.GetColor(tile, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);

            if (addIncrementForSelectedTiles && TileSelectionManager.Instance.IsSelected(tile.TileKey))
            {
                graphicsObj.FillRectangle(m_sb, m_rect);
            }
            else if (addIncrementForUserSelectedTiles && TileSelectionManager.Instance.IsUserSelected(tile.TileKey))
            {
                graphicsObj.FillRectangle(m_sb, m_rect);
            }
            else
            {
                if (
                    IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB) ||
                    IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect) ||
                    IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.BRAM) ||
                    IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.DSP)
                    )
                {
                    graphicsObj.FillRectangle(Brushes.DarkGray, m_rect);
                }
                else
                {
                    graphicsObj.FillRectangle(Brushes.Black, m_rect);
                }
                graphicsObj.DrawRectangle(new Pen(m_sb.Color), m_rect);
            }
            
        }

        void Interfaces.IObserver.Notify(object obj)
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

        void Interfaces.IResetable.Reset()
        {
        }
    }
}
