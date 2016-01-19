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
        public static ImageSource Draw1Channel(PixelInfo[] info,Brush waveBrush,double startPer, double scale,double height=-1)
        {
            if (info.Length < WavFile.MinWidth)
                return null;
            if (height == -1)
                height = WavFile.MinHeight;
            var startPos = (int)(info.Length * startPer);
            var sampleNumber = (int)(info.Length * scale);
            if (startPos + sampleNumber > info.Length)
                startPos = info.Length - sampleNumber;
            var samplesPerPixel = (double)sampleNumber / (double)WavFile.MinWidth;

            DrawingVisual dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pen = new Pen(waveBrush, 1);
            
            pen.Freeze();

            //draw background
            dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, WavFile.MinWidth, height));
            
            //draw wave
            var drawedSample = 0;
            for (int i = 0; i < WavFile.MinWidth; i++)
            {
                var drawSample = 0;
                if (i == WavFile.MinWidth - 1)
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
                drawedSample += drawSample;
            }
            dc.Close();
            var bmp = new RenderTargetBitmap(WavFile.MinWidth, (int)height, 0, 0, PixelFormats.Default);
            bmp.Render(dv);
            return bmp;
        }

        public static ImageSource Draw2Channel(PixelInfo[] lInfo, PixelInfo[] rInfo, Brush waveBrush, double startPer, double scale, double height=-1)
        {
            if (lInfo.Length < WavFile.MinWidth)
                return null;
            
            var startPos = (int)(lInfo.Length * startPer);
            var sampleNumber = (int)(lInfo.Length * scale);
            if (startPos + sampleNumber > lInfo.Length)
                startPos = lInfo.Length - sampleNumber;
            var samplesPerPixel = (double)sampleNumber / (double)WavFile.MinWidth;

            if (height == -1)
                height = WavFile.MinHeight;
            var singleHeight = height / 2;

            DrawingVisual dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pen = new Pen(waveBrush, 1);
            pen.Freeze();

            //draw background
            dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, WavFile.MinWidth, height));
            //draw wave
            var drawedSample = 0;
            for (int i = 0; i < WavFile.MinWidth; i++)
            {
                var drawSample = 0;
                if (i == WavFile.MinWidth - 1)
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
                dc.DrawLine(pen, new Point(i , lmin), new Point(i , lmax));
                dc.DrawLine(pen, new Point(i , rmin + singleHeight), new Point(i , rmax + singleHeight));
                drawedSample += drawSample;
            }
            dc.Close();
            var bmp = new RenderTargetBitmap(WavFile.MinWidth, (int)height, 0, 0, PixelFormats.Default);
            bmp.Render(dv);
            return bmp;
        }

        private static double ScaleToHeight(double value,double height)
        {
            return value * height / WavFile.MinHeight;
        }
    }
}
