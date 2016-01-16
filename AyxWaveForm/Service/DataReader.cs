using AyxWaveForm.Format;
using AyxWaveForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyxWaveForm.Service
{
    public static class DataReader
    {
        public static WaveData Read(WavFile wavfile, int pixelPerSec=0)
        {
            return null;
        }

        public static WaveData Read(string filename, int pixelPerSec= 0)
        {
            return Read(WavFile.Read(filename));
        }

        public static Task<WaveData> ReadAsync(string filename, int pixelPerSec=0)
        {
            return Task.Factory.StartNew<WaveData>(() =>
            {
                return Read(filename, pixelPerSec);
            });
        }

        public static Task<WaveData> ReadAsync(WavFile wavfile, int pixelPerSec=0)
        {
            return Task.Factory.StartNew<WaveData>(() =>
            {
                return Read(wavfile, pixelPerSec);
            });
        }

        private static WaveData Read2Channels16Bit(WavFile wavfile)
        {
            return null;
        }

        private static WaveData Read2Channels32Bit(WavFile wavfile)
        {
            return null;
        }

        private static WaveData Read2Channels64Bit(WavFile wavfile)
        {
            return null;
        }

        private static WaveData Read1Channel8Bit(WavFile wavfile)
        {
            return null;
        }

        private static WaveData Read1Channel16Bit(WavFile wavfile)
        {
            return null;
        }

        private static WaveData Read1Channel32Bit(WavFile wavfile)
        {
            return null;
        }

        private static WaveData Read1Channel64Bit(WavFile wavfile)
        {
            return null;
        }
    }
}
