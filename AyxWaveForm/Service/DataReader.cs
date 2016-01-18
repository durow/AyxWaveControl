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
                    case 8: return Read2Channels(stream, file.SampleNumber/2, file.MaxWidth,Read8Bit);
                    case 16: return Read2Channels(stream, file.SampleNumber/2, file.MaxWidth,Read16Bit);
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
            var samplesPerPixel = (double)sampleNumber / (double)width;
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
                        drawSample = (int)((i + 1) * samplesPerPixel - drawedSample);
                    }
                    var lInfo = new PixelInfo();
                    var rInfo = new PixelInfo();
                    for (int j = 0; j < drawSample; j++)
                    {
                        try
                        {
                            lInfo.Push((short)(reader(br)/2));
                            rInfo.Push((short)(reader(br)/2));
                        }
                        catch
                        {
                            drawedSample += j;
                        }
                    }
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
            var samplesPerPixel = (double)sampleNumber / (double)width;
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
                        drawSample = (int)((i+1)*samplesPerPixel - drawedSample);
                    }
                    var pixInfo = new PixelInfo();
                    for (int j = 0; j < drawSample; j++)
                    {
                        try
                        {
                            pixInfo.Push(reader(br));
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
            var value = br.ReadByte();
            return (short)(WavFile.MinHeight - value * WavFile.MinHeight / 256);
        }

        private static short Read16Bit(BinaryReader br)
        {
            var value = 32768 + br.ReadInt16();
            return (short)(WavFile.MinHeight - value*WavFile.MinHeight / 65536);
        }

        private static short[] Read2Channels16Bit(BinaryReader br)
        {
            var left = Read16Bit(br);
            left = (short)((WavFile.MinHeight - left) / 2);
            var right = Read16Bit(br);
            right = (short)((WavFile.MinHeight - right) / 2);

            var result = new short[2];
            result[0] = left;
            result[1] = right;
            return result;
        }
        private static short[] Read2Channels8Bit(BinaryReader br)
        {
            var left = Read8Bit(br);
            left = (short)((WavFile.MinHeight - left) / 2);
            var right = Read8Bit(br);
            right = (short)((WavFile.MinHeight - right) / 2);

            var result = new short[2];
            result[0] = left;
            result[1] = right;
            return result;
        }
    }
}
