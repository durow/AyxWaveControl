/*
 * Author:durow
 * Date:2016.01.15
 * Description:Read the wavfile head information. 
 */
using AyxWaveForm.Model;
using AyxWaveForm.Service;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AyxWaveForm.Format
{
    public class WavFile
    {
        #region Properties

        /// <summary>
        /// The name of the wav file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// The tag of the file, always 'RIFF' (4 bytes)
        /// </summary>
        public string FileTag { get; private set; }

        /// <summary>
        /// File length withou FileTag(4 bytes)
        /// </summary>
        public int FileLength { get; private set; }
        
        /// <summary>
        /// wave format 1 means PCM (2 bytes)
        /// </summary>
        public short WaveFormat { get; private set; }

        /// <summary>
        /// Channel number (2 bytes)
        /// </summary>
        public short Channels { get; private set; }

        /// <summary>
        /// Sample rate (4 bytes)
        /// </summary>
        public int SampleRate { get; private set; }

        /// <summary>
        /// Bytes per second
        /// </summary>
        public int BytesPerSecond { get; private set; }

        /// <summary>
        /// Sample bits
        /// </summary>
        public short SampleBit { get; private set; }

        /// <summary>
        /// Data chunk offset
        /// </summary>
        public long DataOffset { get; private set; }

        /// <summary>
        /// Data size
        /// </summary>
        public int DataSize { get; private set; }

        /// <summary>
        /// Sample number of the wav file
        /// </summary>
        public int SampleNumber { get; private set; }

        /// <summary>
        /// Total seconds of the wavfile
        /// </summary>
        public double TotalSeconds { get; private set; }

        /// <summary>
        /// Wave data cache
        /// </summary>
        public WaveData CacheData { get; internal set; }

        /// <summary>
        /// pixels per second
        /// </summary>
        public int PixelPerSecond { get; set; }

        public int MaxWidth { get; private set; }
        public static int MinWidth { get; private set; }
        /// <summary>
        /// The miniumum of the wave scale
        /// </summary>
        public double MinScale { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Is closed
        /// </summary>
        private WavFile() { }

        static WavFile()
        {
            MinWidth = 1920;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Read wavfile infomation.
        /// </summary>
        /// <param name="filename">The name of wav file</param>
        /// <returns></returns>
        public static WavFile Read(string filename,int pixelPerSecond = 300,string cacheFile="")
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            using (var stream = new FileStream(filename,FileMode.Open))
            {
                var result = Read(stream,pixelPerSecond,cacheFile);
                result.FileName = filename;
                return result;
            }
        }

        /// <summary>
        /// Read wavfile infomation.
        /// </summary>
        /// <param name="stream">The stream of the file</param>
        /// <returns></returns>
        public static WavFile Read(Stream stream, int pixelPerSecond = 300,string cacheFile = "")
        {
            var file = new WavFile();
            using (var reader = new BinaryReader(stream))
            {
                stream.Position = 0;

                file.FileTag = string.Concat(reader.ReadChars(4));
                if (file.FileTag != "RIFF")
                    throw new Exception("not RIFF file!");
                file.FileLength = reader.ReadInt32();
                stream.Position += 12;
                file.WaveFormat = reader.ReadInt16();
                if (file.WaveFormat != 1)
                    throw new Exception("not PCM format!");
                file.Channels = reader.ReadInt16();
                file.SampleRate = reader.ReadInt32();
                file.BytesPerSecond = reader.ReadInt32();
                stream.Position += 2;
                file.SampleBit = reader.ReadInt16();
                stream.Position = 36;
                var data = string.Concat(reader.ReadChars(4));
                while(data != "data") //find the start of data chunk
                {
                    stream.Position -= 3;
                    data = string.Concat(reader.ReadChars(4));
                }
                file.DataSize = reader.ReadInt32();
                file.DataOffset = stream.Position;
                file.PixelPerSecond = pixelPerSecond;

                file.ComputeFileInfo();
                file.ReadCacheData(stream);
            }
            return file;
        }

        public void ReadCacheData(Stream stream=null,string cacheFile="")
        {
            if (string.IsNullOrEmpty(cacheFile))
            {
                if (stream == null)
                    CacheData = DataReader.Read(this);
                else
                    CacheData = DataReader.Read(this, stream);
            }
            else
            {
                CacheData = DataReader.FromCacheFile(this, cacheFile);
            }
        }

        private void ComputeFileInfo()
        {
            SampleNumber = DataSize * 8 / SampleBit;
            TotalSeconds = (double)DataSize / (double)BytesPerSecond;
            MaxWidth = (int)TotalSeconds * PixelPerSecond + 1;
            if (MaxWidth < MinWidth) MaxWidth = MinWidth;
            MinScale = (double)MinWidth / (double)MaxWidth;
        }

        public ImageSource DrawChannel(double startPer, double scale, double width)
        {
            if (Channels == 1)
                return WaveDrawer.Draw1Channel(CacheData.Channel, Brushes.Lime,0,1);
            else
                return WaveDrawer.Draw2Channel(CacheData.LeftChannel, CacheData.RightChannel, Brushes.Green,0,1);
        }

        public ImageSource DrawLeftChannel()
        {
            return null;
        }

        public ImageSource DrawRightChannel()
        {
            return null;
        }

        #endregion

    }

}
