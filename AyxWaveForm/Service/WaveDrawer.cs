/*
 * Author:durow
 * Date:2016.01.16
 * Description:Used to draw wave data to ImageSource
*/

using AyxWaveForm.Format;
using AyxWaveForm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AyxWaveForm.Service
{
    public static class WaveDrawer
    {
        /// <summary>
        /// Draw one channel.
        /// For single channel wave, it draws the channel and wave grid.
        /// For two channel wave,use this method to draw one channel of two.
        /// </summary>
        /// <param name="info">chached sample info</param>
        /// <param name="style">style of the wave</param>
        /// <param name="startPer">the percent of start position</param>
        /// <param name="scale">scale</param>
        /// <param name="width">width of the return bitmap</param>
        /// <param name="height">height of the return bitmap</param>
        /// <returns>bitmap that the wave drawed on</returns>
        public static ImageSource Draw1Channel(PixelInfo[] info,WaveStyle style, double startPer, double scale,double width, double height=-1)
        {
            if (width <= 0) return null;
            if (info.Length < width)
                return null;
            if (height == -1)
                height = WavFile.MinHeight;
            var startPos = (int)(info.Length * startPer);
            var sampleNumber = (int)(info.Length * scale);
            if (startPos + sampleNumber > info.Length)
                startPos = info.Length - sampleNumber;
            var samplesPerPixel = (double)sampleNumber / width;

            DrawingVisual dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pen = new Pen(style.WaveBrush, 1);
            var gridPen = new Pen(style.GridBrush, 0.2);
            gridPen.Freeze();
            pen.Freeze();
            //draw wave grid
            var hNumber = GetGridNumber(height / 2, 50);
            var k = height / (2 * hNumber);
            for (int i = 1; i < hNumber; i++)
            {
                dc.DrawLine(gridPen, new Point(0, i * k), new Point(width, i * k));
                dc.DrawLine(gridPen, new Point(0, height/2 + i * k), new Point(width, height/2 + i * k));
            }
            var vNumber = GetGridNumber(width, 100);
            var vGridSpace = width / vNumber;
            for (int i = 1; i < vNumber; i++)
            {
                dc.DrawLine(gridPen, new Point(i * vGridSpace, 0), new Point(i * vGridSpace, height));
            }
            //draw wave
            var drawedSample = 0;
            PixelInfo prePixel = null;
            for (int i = 0; i < width; i++)
            {
                var drawSample = 0;
                if (i == width - 1)
                    drawSample = sampleNumber - drawedSample;
                else
                {
                    drawSample = (int)((i + 1) * samplesPerPixel - drawedSample);
                }
                var tempInfo = new PixelInfo();
                for (int j = 0; j < drawSample; j++)
                {
                    tempInfo.Push(info[startPos + drawedSample + j]);
                }
                var min = tempInfo.Min;
                var max = tempInfo.Max;
                if(height != WavFile.MinHeight)
                {
                    min = (short)ScaleToHeight(min, height);
                    max = (short)ScaleToHeight(max, height);
                }
                dc.DrawLine(pen, new Point(i, min), new Point(i, max));
                if (prePixel == null)
                {
                    prePixel = new PixelInfo
                    {
                        Min = min,
                        Max = max,
                    };
                }
                else
                {
                    if(prePixel.Max < min)
                    {
                        dc.DrawLine(pen, new Point(i - 1, prePixel.Max), new Point(i, min));
                    }
                    if (prePixel.Min > max)
                    {
                        dc.DrawLine(pen, new Point(i - 1, prePixel.Min), new Point(i, max));
                    }
                    prePixel.Max = max;
                    prePixel.Min = min;
                }
                drawedSample += drawSample;
            }
            dc.Close();
            var bmp = new RenderTargetBitmap((int)width, (int)height, 0, 0, PixelFormats.Default);
            bmp.Render(dv);
            return bmp;
        }

        /// <summary>
        /// Draw two channels
        /// </summary>
        /// <param name="lInfo">cached samples of the left channel</param>
        /// <param name="rInfo">cached samples of the right channel</param>
        /// <param name="style">style of the wave</param>
        /// <param name="startPer">the percent of the start position</param>
        /// <param name="scale">scale</param>
        /// <param name="width">the width of the return bitmap</param>
        /// <param name="height">the height of the return bitmap</param>
        /// <returns>bitmap that the wave drawed on</returns>
        public static ImageSource Draw2Channel(PixelInfo[] lInfo, PixelInfo[] rInfo, WaveStyle style, double startPer, double scale, double width, double height=-1)
        {
            if (width <= 0) return null;

            if (lInfo.Length < width)
                return null;
            
            var startPos = (int)(lInfo.Length * startPer);
            var sampleNumber = (int)(lInfo.Length * scale);
            if (startPos + sampleNumber > lInfo.Length)
                startPos = lInfo.Length - sampleNumber;
            var samplesPerPixel = (double)sampleNumber / width;

            if (height == -1)
                height = WavFile.MinHeight;
            var singleHeight = height / 2;

            DrawingVisual dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pen = new Pen(style.WaveBrush, 1);
            var linePen = new Pen(style.WaveBrush, 1);
            
            pen.Freeze();

            //draw wave
            var drawedSample = 0;
            PixelInfo lprePixel = null;
            PixelInfo rprePixel = null;
            for (int i = 0; i < width; i++)
            {
                var drawSample = 0;
                if (i == width - 1)
                    drawSample = sampleNumber - drawedSample;
                else
                {
                    drawSample = (int)((i + 1) * samplesPerPixel - drawedSample);
                }
                var lTemp = new PixelInfo();
                var rTemp = new PixelInfo();
                for (int j = 0; j < drawSample; j++)
                {
                    lTemp.Push(lInfo[startPos + drawedSample + j]);
                    rTemp.Push(rInfo[startPos + drawedSample + j]);
                }
                var lmin = lTemp.Min;
                var lmax = lTemp.Max;
                var rmin = rTemp.Min;
                var rmax = rTemp.Max;
                if (height != -1)
                {
                    lmin = (short)ScaleToHeight(lmin, height);
                    lmax = (short)ScaleToHeight(lmax, height);
                    rmin = (short)ScaleToHeight(rmin, height);
                    rmax = (short)ScaleToHeight(rmax, height);
                }
                dc.DrawLine(pen, new Point(i , lmin/2), new Point(i , lmax/2));
                dc.DrawLine(pen, new Point(i , rmin/2 + singleHeight), new Point(i , rmax/2 + singleHeight));
                if(lprePixel == null)
                {
                    lprePixel = new PixelInfo
                    {
                        Max = lmax,
                        Min = lmin,
                    };
                }
                else
                {
                    if(lprePixel.Max < lmin)
                        dc.DrawLine(pen, new Point(i-1, lprePixel.Max / 2), new Point(i, lmin / 2));
                    if(lprePixel.Min > lmax)
                        dc.DrawLine(pen, new Point(i-1, lprePixel.Min / 2), new Point(i, lmax / 2));
                }
                if(rprePixel == null)
                {
                    rprePixel = new PixelInfo
                    {
                        Max = rmax,
                        Min = rmin,
                    };
                }
                else
                {
                    if (rprePixel.Max < rmin)
                        dc.DrawLine(pen, new Point(i - 1, rprePixel.Max / 2), new Point(i, rmin / 2));
                    if (rprePixel.Min > rmax)
                        dc.DrawLine(pen, new Point(i - 1, rprePixel.Min / 2), new Point(i, rmax / 2));
                }
                drawedSample += drawSample;
            }
            dc.Close();
            var bmp = new RenderTargetBitmap((int)width, (int)height, 0, 0, PixelFormats.Default);
            bmp.Render(dv);
            return bmp;
        }

        public static ImageSource Draw1Channel(WavFile wavFile, WaveStyle style, double startPer,double scale, double width, double height)
        {
            using (var stream = File.Open(wavFile.FileName, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {

                }
            }
            return null;
        }
        /// <summary>
        /// draw a bitmap for slider background
        /// </summary>
        /// <param name="data">cached samples of the wave</param>
        /// <param name="style">style of the wave</param>
        /// <returns></returns>
        public static ImageSource DrawSimple(WaveData data,WaveStyle style)
        {
            if(data.Channel != null)
            {
                var img = Draw1Channel(data.Channel, style, 0, 1, WavFile.MinWidth);
                img.Freeze();
                return img;
            }
            else if(data.LeftChannel!=null && data.RightChannel!=null)
            {
                var img = Draw2Channel(data.LeftChannel, data.RightChannel, style, 0, 1, WavFile.MinWidth);
                img.Freeze();
                return img;
            }
            return null;
        }

        /// <summary>
        /// scale from WavFile's MinHeight property to the height of the bitmap
        /// </summary>
        /// <param name="value">oldValue</param>
        /// <param name="height">height of the bitmap</param>
        /// <returns>new value that scaled</returns>
        private static double ScaleToHeight(double value,double height)
        {
            return value * height / WavFile.MinHeight;
        }

        /// <summary>
        /// compute the number of grid lines
        /// </summary>
        /// <param name="height">height of width</param>
        /// <param name="minSpace">minimum space between two grid lines</param>
        /// <returns></returns>
        private static int GetGridNumber(double height, double minSpace)
        {
            var max = 10;
            for (int i = 2; i < max; i++)
            {
                var h = height / i;
                if (h <= minSpace)
                    return i;
            }
            return max;
        }
    }
}
