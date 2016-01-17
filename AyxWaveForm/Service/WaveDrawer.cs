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
        public static ImageSource Draw1Channel(PixelInfo[] info,Brush waveBrush)
        {
            DrawingVisual dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pen = new Pen(waveBrush, 1);
            pen.Freeze();
            for (int i = 1; i <= info.Length; i++)
            {
                var tempInfo = info[i - 1];
                dc.DrawLine(pen, new Point(i, tempInfo.Min), new Point(i, tempInfo.Max));
            }
            dc.Close();
            var bmp = new RenderTargetBitmap(WavFile.MinWidth, 256, 0, 0, PixelFormats.Default);
            bmp.Render(dv);
            return bmp;
        }

        public static ImageSource Draw2Channel(PixelInfo[] lInfo, PixelInfo[] rInfo, Brush waveBrush)
        {
            DrawingVisual dv = new DrawingVisual();
            var dc = dv.RenderOpen();
            var pen = new Pen(waveBrush, 1);
            pen.Freeze();
            for (int i = 1; i <= lInfo.Length; i++)
            {
                var templInfo = lInfo[i - 1];
                dc.DrawLine(pen, new Point(i, templInfo.Min), new Point(i, templInfo.Max));

                var temprInfo = rInfo[i - 1];
                dc.DrawLine(pen, new Point(i, 257 + temprInfo.Min), new Point(i, 257 + temprInfo.Max));
            }
            dc.Close();
            var bmp = new RenderTargetBitmap(WavFile.MinWidth, 514, 0, 0, PixelFormats.Default);
            bmp.Render(dv);
            return bmp;
        }
    }
}
