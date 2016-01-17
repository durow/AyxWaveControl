using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AyxWaveForm.Model
{
    public class WaveData
    {
        public PixelInfo[] LeftChannel { get; private set; }
        public PixelInfo[] RightChannel { get; private set; }
        public PixelInfo[] Channel { get; private set; }

        public WaveData(PixelInfo[] channel = null,PixelInfo[] leftChannel = null,PixelInfo[] rightChannel = null)
        {
            Channel = channel;
            LeftChannel = leftChannel;
            RightChannel = rightChannel;
        }
    }
}
