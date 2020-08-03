using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GoAhead.FPGA;
using GoAhead.Commands;
using GoAhead.Commands.Selection;
using GoAhead.Settings;

namespace GoAhead.GUI
{

    /// <summary>
    /// ZoomPicBox does what it says on the wrapper.
    /// </summary>
    /// <remarks>
    /// PictureBox doesn't lend itself well to overriding. Why not start with something basic and do the job properly?
    /// </remarks>

    public class ZoomPicBox : ScrollableControl
    {
        Image _image;
        [
            Category("Appearance"),
            Description("The image to be displayed")
        ]

        public Image Image
        {
            get{return _image;}
            set
            {
                _image=value;
                UpdateScaleFactor(false);
                Invalidate();

                InterpolationMode = InterpolationMode.NearestNeighbor;
            }
        }

        float _zoom=1.0f;
        [
            Category("Appearance"),
            Description("The zoom factor. Less than 1 to reduce. More than 1 to magnify.")
        ]

        public float Zoom
        {
            get{return _zoom;}
            set
            {
                if (value < 0 || value < 0.00001)
                {
                    value = 0.00001f;
                }

                _zoom=value;
                UpdateScaleFactor(true);
                Invalidate();
            }
        }


        float _prevZoom = 1.0f;
        [
            Category("Appearance"),
            Description("The previous zoom factor.")
        ]

        public float PrevZoom
        {
            get { return _prevZoom; }
            set
            {
                if (value < 0 || value < 0.00001)
                {
                    value = 0.00001f;
                }

                _prevZoom = value;
                
            }
        }

        Point _zoomPoint = new Point(1, 1);
        [
            Category("Appearance"),
            Description("The zoom point. Used to zoom relative to the mouse cursor.")
        ]

        public Point ZoomPoint
        {
            get { return new Point(_zoomPoint.X, _zoomPoint.Y); }
            set
            {
                _zoomPoint = value;
            }
        }


        /// <summary>
        /// Calculates the effective size of the image
        /// after zooming and updates the AutoScrollSize accordingly
        /// </summary>
        private void UpdateScaleFactor(bool onZoom)
        {
            if (_image == null)
            {
                AutoScrollMinSize = Size;
            }
            else
            {
                if (onZoom && _zoomPoint.X == 0 && _zoomPoint.Y == 0)
                {
                    float scrollX = (-1) * (AutoScrollPosition.X * _zoom / _prevZoom - 0.5f);
                    float scrollY = (-1) * (AutoScrollPosition.Y * _zoom / _prevZoom - 0.5f);
                    AutoScrollPosition = new Point((int)(scrollX), (int)(scrollY));
                }
                else if (onZoom)
                {
                    float x = _zoomPoint.X * _prevZoom / _zoom;
                    float y = _zoomPoint.Y * _prevZoom / _zoom;

                    AutoScrollPosition = new Point((int) x , (int) y);
                }
                 
                AutoScrollMinSize = new Size((int)(_image.Width * _zoom + 0.5f), (int)(_image.Height * _zoom + 0.5f));

            }            
        }

        InterpolationMode _interpolationMode=InterpolationMode.High;
        [
            Category("Appearance"),
            Description("The interpolation mode used to smooth the drawing")
        ]
        public InterpolationMode InterpolationMode
        {
            get { return _interpolationMode; }
            set { _interpolationMode=value; }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // do nothing.
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            //if no image, don't bother
            if(_image==null)
            {
                base.OnPaintBackground(e);
                return;
            }


            //Set up a zoom matrix
            Matrix mx = new Matrix(_zoom, 0, 0, _zoom, 0, 0);
            //now translate the matrix into position for the scrollbars
            mx.Translate(AutoScrollPosition.X /_zoom, AutoScrollPosition.Y /_zoom);
            
            //use the transform
            e.Graphics.Transform=mx;
            //and the desired interpolation mode
            e.Graphics.InterpolationMode=_interpolationMode;
            //Draw the image ignoring the images resolution settings.
            e.Graphics.DrawImage(_image,new Rectangle(0,0,_image.Width,_image.Height),0,0,_image.Width, _image.Height,GraphicsUnit.Pixel);
          //e.Graphics.DrawImage(_image,new Rectangle(0,0,_image.Width,_image.Height), new Rectangle(0, 0, (int)_zoom, (int)_zoom), GraphicsUnit.Pixel);
            base.OnPaint(e);

           

        }

        public ZoomPicBox()
        {
            //Double buffer the control
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.DoubleBuffer, true);
            AutoScroll=true;
        }




    }
}

 
