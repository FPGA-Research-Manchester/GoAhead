using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
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
    class BlockPainter : Painter, Interfaces.IObserver, Interfaces.IResetable
    {
        public BlockPainter(FPGAViewCtrl view)
            :base(view)
        {
            TileSelectionManager.Instance.Add(this);
            Commands.Reset.ObjectsToReset.Add(this);
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
                m_graphicsObj = Graphics.FromImage(TileBitmap);
                SelectionUpdate((SelectionUpdate)obj, m_graphicsObj);
            } 
        }

        private void SelectionUpdate(SelectionUpdate selUpdate, Graphics graphicsObj)
        {
            // ram drawing 
            if (!RAMSelectionManager.Instance.HasMappings)
            {
                RAMSelectionManager.Instance.UpdateMapping();
            }
        
            switch (selUpdate.Action)
            {
                case (SelectionUpdateAction.AddCurrentSelectionToUserSelection):
                    {
                        foreach (Tile t in TileSelectionManager.Instance.GetUserSelection(selUpdate.UserSelectionType, m_view.TileRegex))
                        {
                            //if (Regex.IsMatch(t.Location, Objects.IdentifierManager.Instance.GetRegex(IdentifierManager.RegexTypes.CLBRegex)))
                            if (IdentifierManager.Instance.IsMatch(t.Location, IdentifierManager.RegexTypes.CLB))
                            {
                                Tile intTile = FPGATypes.GetInterconnectTile(t);
                                DrawTile(intTile, graphicsObj, true, true);
                            }
                            DrawTile(t, graphicsObj, true, true);
                        }
                        break;
                    }
                case (SelectionUpdateAction.AddToSelection) :
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
                        foreach(Tile tile in TileSelectionManager.Instance.GetSelectedTiles().Where(t => m_view.TileRegex.IsMatch(t.Location)))
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
                                DrawTile(intTile, graphicsObj,true, false);
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
            TileBitmap = new Bitmap((x + 1) * m_view.TileSize, (y + 1) * m_view.TileSize, m_pixelFormat);

            Graphics graphicsObj = Graphics.FromImage(TileBitmap);

            graphicsObj.Clear(Color.Gray);

            foreach (Tile tile in GetAllTiles())
            {
                DrawTile(tile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }

            // get ram block data            
            if (!m_ramDataValid)
            {
                m_ramDataValid = FPGATypes.GetRamBlockSize(m_view.TileRegex, out m_ramBlockWidth, out m_ramBlockHeight, out m_ramTiles);
            }

            foreach (Tile ramTile in m_ramTiles)
            {
                if(!m_view.TileRegex.IsMatch(ramTile.Location))
                {
                    continue;
                }

                DrawRAMTile(ramTile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }

            graphicsObj.Dispose();
        }

        public override void DrawTile(Tile tile, Graphics graphicsObj, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            if(!m_view.TileRegex.IsMatch(tile.Location))
            {
                return;
            }
            bool ramTile = false;
            bool hasMapping = false;

            if (m_ramTiles.Contains(tile))
            {
                ramTile = true;
            }
            else if (!RAMSelectionManager.Instance.HasMapping(tile))
            {
                hasMapping = true;
            }

            if (ramTile)
            {
                DrawRAMTile(tile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }
            else if (hasMapping)
            {
                DrawNonRAMTile(tile, graphicsObj, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
            }
        }

        private void DrawRAMTile(Tile ramTile, Graphics graphicsObj, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            int x = -1;
            int y = -1;
            switch (FPGA.FPGA.Instance.Family)
            {
                case FPGATypes.FPGAFamily.Artix7:
                case FPGATypes.FPGAFamily.Kintex7:
                case FPGATypes.FPGAFamily.Virtex7:
                case FPGATypes.FPGAFamily.Zynq:
                    {
                        if (FPGATypes.IsOrientedMatch(ramTile.Location, IdentifierManager.RegexTypes.BRAM_left))
                        {
                            // ram tile are in the bottom middle
                            x = ramTile.TileKey.X;
                            y = ramTile.TileKey.Y - (m_ramBlockHeight - 1);
                        }
                        else //if (Regex.IsMatch(ramTile.Location, "_R_"))
                        {
                            // ram tile are in the bottom middle
                            x = ramTile.TileKey.X - (m_ramBlockWidth - 1);
                            y = ramTile.TileKey.Y - (m_ramBlockHeight - 1);
                        }
                        break;
                    }
                default:
                    {
                        // ram tile are in the bottom right
                        x = ramTile.TileKey.X - (m_ramBlockWidth - 1);
                        y = ramTile.TileKey.Y - (m_ramBlockHeight - 1);
                        break;
                    }
            }

            if (FPGA.FPGA.Instance.Contains(x, y))
            {
                Tile upperLeft = FPGA.FPGA.Instance.GetTile(x, y);

                int upperLeftX = upperLeft.TileKey.X * m_view.TileSize;
                int upperLeftY = upperLeft.TileKey.Y * m_view.TileSize;

                Rectangle rect = new Rectangle();
                rect.X = upperLeftX-1;
                rect.Y = upperLeftY-1;
                rect.Width = (m_ramBlockWidth * (m_view.TileSize - 1) + m_ramBlockWidth - 1) - 2;
                rect.Height = (m_ramBlockHeight * (m_view.TileSize - 1) + m_ramBlockHeight) - 2;

                m_sb.Color = m_view.GetColor(ramTile, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);
                graphicsObj.FillRectangle(m_sb, rect);
            }
        }

        private void DrawNonRAMTile(Tile tile, Graphics graphicsObj, bool addIncrementForSelectedTiles, bool addIncrementForUserSelectedTiles)
        {
            int upperLeftX = 0;
            int upperLeftY = 0;
            int widthScale = 1;
            int heightScale = 1;

            if (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.CLB))
            {
                Tile intTile = FPGATypes.GetInterconnectTile(tile);

                switch (FPGA.FPGA.Instance.Family)                
                {
                
                    case FPGATypes.FPGAFamily.Artix7:
                    case FPGATypes.FPGAFamily.Kintex7:
                    case FPGATypes.FPGAFamily.Virtex7:
                    case FPGATypes.FPGAFamily.Zynq:
                        {
                            if (FPGATypes.IsOrientedMatch(tile.Location, IdentifierManager.RegexTypes.CLB_left))
                            {
                                // CLBLL_L_X INT_L_X
                                upperLeftX = tile.TileKey.X * m_view.TileSize;
                                upperLeftY = tile.TileKey.Y * m_view.TileSize;
                                widthScale = 2;
                            }
                            else
                            {
                                upperLeftX = intTile.TileKey.X * m_view.TileSize;
                                upperLeftY = intTile.TileKey.Y * m_view.TileSize;
                                // double size of the rectangle
                                widthScale = 2;
                            }
                            break;
                        }
                    case FPGATypes.FPGAFamily.UltraScale:
                        {
                            widthScale = FPGATypes.GetCLTile(intTile).Count() + 2;
                            if (FPGATypes.IsOrientedMatch(tile.Location, IdentifierManager.RegexTypes.CLB_left))
                            {
                                // CLBLL_L_X INT_L_X
                                upperLeftX = tile.TileKey.X * m_view.TileSize;
                                upperLeftY = tile.TileKey.Y * m_view.TileSize;
                            }
                            else
                            {
                                //If CLEL_R is paired with a subinterconnect, keep its colour.
                                Tile subinterconnect = FPGA.FPGA.Instance.GetTile(tile.TileKey.X - 2, tile.TileKey.Y);

                                if (IdentifierManager.Instance.IsMatch(subinterconnect.Location, IdentifierManager.RegexTypes.SubInterconnect))
                                {
                                    upperLeftX = (intTile.TileKey.X - (widthScale == 2 ? 0 : 1)) * m_view.TileSize;
                                    upperLeftY = intTile.TileKey.Y * m_view.TileSize;

                                }
                                
                                // double size of the rectangle
                            } 
                            break;
                        }
                default:
                    {
                        upperLeftX = intTile.TileKey.X * m_view.TileSize;
                        upperLeftY = intTile.TileKey.Y * m_view.TileSize;
                        // double size of the rectangle
                        widthScale = 2;
                        break;
                    }
                }
            }
            else if (IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.Interconnect))
            {
                // interconnect tiles for CLB have no tiles
                return;
            }
            else if(IdentifierManager.Instance.IsMatch(tile.Location, IdentifierManager.RegexTypes.SubInterconnect))
            {
                // Sub-interconnect tiles for CLB have no tiles
                return;

            }
            else
            {
                upperLeftX = tile.TileKey.X * m_view.TileSize;
                upperLeftY = tile.TileKey.Y * m_view.TileSize;
            }


            m_rect.X = upperLeftX - 1;
            m_rect.Y = upperLeftY - 1;
            m_rect.Width = (widthScale * (m_view.TileSize - 1) + widthScale) - 2;
            m_rect.Height = (heightScale * (m_view.TileSize - 1) + heightScale) - 2;

            // default color maybde overwritten

            //For CLEM tiles, preserve their tile colour along the whole block.
            if(tile.Location.Contains("CLEM"))
            {
                m_sb.Color = ColorSettings.Instance.GetColor(tile);
            }
            m_sb.Color = m_view.GetColor(tile, addIncrementForSelectedTiles, addIncrementForUserSelectedTiles);

            
            graphicsObj.FillRectangle(m_sb, m_rect);
        
        }

        public override void Reset()
        {
            m_ramDataValid = false;
        }

        private Graphics m_graphicsObj = null;
        private bool m_ramDataValid = false;
        private TileSet m_ramTiles = new TileSet();
        private int m_ramBlockWidth = 0;
        private int m_ramBlockHeight = 0;       
    }
}
