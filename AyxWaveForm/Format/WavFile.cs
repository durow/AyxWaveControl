using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyxWaveForm.Format
{
    public class WavFile
    {
        #region Properties

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
        /// Data block offset
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

        #endregion

        #region Constructor

        /// <summary>
        /// Is closed
        /// </summary>
        private WavFile() { }

        #endregion

        #region Methods

        /// <summary>
        /// Read wav from filename
        /// </summary>
        /// <param name="filename">The name of wav file</param>
        /// <returns></returns>
        public static WavFile Read(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            using (var stream = new FileStream(filename,FileMode.Open))
            {
                return Read(stream);
            }
        }


        public static WavFile Read(Stream stream)
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
                while(data != "data")
                {
                    stream.Position -= 3;
                    data = string.Concat(reader.ReadChars(4));
                }
                file.DataSize = reader.ReadInt32();
                file.DataOffset = stream.Position;
                file.SampleNumber = file.DataSize * 8 / file.SampleBit;
                file.TotalSeconds = (double)file.DataSize / (double)file.BytesPerSecond;
            }
            return file;
        }

        #endregion


    }

}
