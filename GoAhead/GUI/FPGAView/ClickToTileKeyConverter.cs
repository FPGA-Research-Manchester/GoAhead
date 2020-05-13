using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoAhead.FPGA;

namespace GoAhead.GUI.FPGAView
{
    public abstract class ClickToTileKeyConverter
    {
        public ClickToTileKeyConverter(ZoomPicBox picBox)
        {
            m_zoomPictBox = picBox;
        }

        public abstract TileKey GetClickedKey(int x, int y);

        protected ZoomPicBox m_zoomPictBox;
    }

    public class ConvertPosition : ClickToTileKeyConverter
    {
        public ConvertPosition(ZoomPicBox picBox)
            :base(picBox)
        {
        }

        public override TileKey GetClickedKey(int x, int y)
        {
            double maxX = (double)FPGA.FPGA.Instance.MaxX;
            double maxY = (double)FPGA.FPGA.Instance.MaxY;

            double tileWidth = (double)m_zoomPictBox.Image.Width * m_zoomPictBox.Zoom / (maxX + 1);
            double tileHeight = (double)m_zoomPictBox.Image.Height * m_zoomPictBox.Zoom / (maxY + 1);

            int scrolledX = x + m_zoomPictBox.HorizontalScroll.Value;
            int scrolledY = y + m_zoomPictBox.VerticalScroll.Value;

            double dx = scrolledX / tileWidth;
            double dy = scrolledY / tileHeight;

            int upperLeftX = (int)Math.Floor(dx);
            int upperLeftY = (int)Math.Floor(dy);

            return new TileKey(upperLeftX, upperLeftY);
        }
    }
}
