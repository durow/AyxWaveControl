using AyxWaveForm.Format;
using AyxWaveForm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyxWaveForm.Service
{
    public static class DataReader
    {
        public static WaveData Read(WavFile file)
        {
            var stream = new FileStream(file.FileName, FileMode.Open);
            var result = Read(file, stream);
            stream.Close();
            return result;
        }

        public static WaveData Read(WavFile file, Stream stream)
        {
            stream.Position = file.DataOffset;
            if(file.Channels == 1)
            {
                switch (file.SampleBit)
                {
                    case 8:return Read1Channel(stream, file.SampleNumber, file.MaxWidth,Read8Bit);
                    case 16:return Read1Channel(stream, file.SampleNumber, file.MaxWidth,Read16Bit);
                    default: return null;
                }
            }
            else
            {
                switch(file.SampleBit)
                {
                    case 16: return Read2Channels(stream, file.SampleNumber, file.MaxWidth,Read8Bit);
                    case 32: return Read2Channels(stream, file.SampleNumber, file.MaxWidth,Read16Bit);
                    default: return null;
                }
            }
        }

        public static WaveData FromCacheFile(WavFile file, string filename)
        {
            return null;
        }

        public static Task<WaveData> ReadAsync(WavFile file, Stream stream, int samplesPerPixel)
        {
            return Task.Factory.StartNew<WaveData>(() =>
            {
                return Read(file,stream);
            });
        }

        public static WaveData Read2Channels(Stream stream,int sampleNumber,int width,Func<BinaryReader,short> reader)
        {
            var l = new PixelInfo[width];
            var r = new PixelInfo[width];
            var log = new int[width];

            using (var br = new BinaryReader(stream))
            {
                var drawedSample = 0;
                for (int i = 0; i < width; i++)
                {
                    var drawSample = 0;
                    if (i == width - 1)
                        drawSample = sampleNumber - drawedSample;
                    else
                    {
                        var draw = (double)((sampleNumber - drawedSample) / (double)(width - i));
                        drawSample = (int)Math.Round(draw);
                    }
                    var lInfo = new PixelInfo();
                    var rInfo = new PixelInfo();
                    for (int j = 0; j < drawSample; j++)
                    {
                        try
                        {
                            var lTemp = reader(br);
                            var lHeight = 256 - lTemp;
                            lInfo.Push((short)lHeight);

                            var rTemp = reader(br);
                            var rHeight = 256 - rTemp;
                            rInfo.Push((short)rHeight);
                        }
                        catch
                        {
                            drawedSample += j;
                        }
                    }
                    log[i] = drawSample;
                    l[i] = lInfo;
                    r[i] = rInfo;
                    drawedSample += drawSample;
                }
                return new WaveData(null,l,r);
            }
        }

        private static WaveData Read1Channel(Stream stream,int sampleNumber,int width,Func<BinaryReader,short> reader)
        {
            var result = new PixelInfo[width];
            var log = new int[width];
            using (var br = new BinaryReader(stream))
            {
                var drawedSample = 0;
                for (int i = 0; i < width; i++)
                {
                    var drawSample = 0;
                    if (i == width - 1)
                        drawSample = sampleNumber - drawedSample;
                    else
                    {
                        var draw = (double)((sampleNumber - drawedSample) / (double)(width - i));
                        drawSample = (int)Math.Round(draw);
                    }
                    log[i] = drawSample;
                    var pixInfo = new PixelInfo();
                    for (int j = 0; j < drawSample; j++)
                    {
                        try
                        {
                            var temp = reader(br);
                            var height = 256 - temp;
                            pixInfo.Push((short)height);
                        }
                        catch
                        {
                            drawedSample += j;
                        }
                    }
                    drawedSample += drawSample;
                    result[i] = pixInfo;
                }
                return new WaveData(result);
            }
        }

        private static short Read8Bit(BinaryReader br)
        {
            return (short)br.ReadByte();
        }

        private static short Read16Bit(BinaryReader br)
        {
            var value = 32768 + br.ReadInt16();
            return (short)(value*256 / 65536);
        }
        
    }
}
