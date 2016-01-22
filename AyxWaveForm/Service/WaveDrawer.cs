/*
 * Author:durow
 * Date:2016.01.16
 * Description:Used to draw wave data to image
*/
using AyxWaveForm.Format;
using AyxWaveForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AyxWaveForm.Service
{
    public static class WaveDrawer
    {
        public static ImageSource Draw1Channel(PixelInfo[] info,Brush waveBrush,double startPer, double scale,double width, double height=-1)
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
            var pen = new Pen(waveBrush, 1);
            pen.Freeze();

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

        public static ImageSource Draw2Channel(PixelInfo[] lInfo, PixelInfo[] rInfo, Brush waveBrush, double startPer, double scale, double width, double height=-1)
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
            var pen = new Pen(waveBrush, 1);
            var linePen = new Pen(waveBrush, 1);
            
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

        public static ImageSource DrawSimple(WaveData data,Brush waveBrush)
        {
            if(data.Channel != null)
            {
                var img = Draw1Channel(data.Channel, waveBrush, 0, 1, WavFile.MinWidth);
                img.Freeze();
                return img;
            }
            else if(data.LeftChannel!=null && data.RightChannel!=null)
            {
                var img = Draw2Channel(data.LeftChannel, data.RightChannel, waveBrush, 0, 1, WavFile.MinWidth);
                img.Freeze();
                return img;
            }
            return null;
        }

        private static double ScaleToHeight(double value,double height)
        {
            return value * height / WavFile.MinHeight;
        }
    }
}
