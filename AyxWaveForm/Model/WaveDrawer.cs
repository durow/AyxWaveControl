using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AyxWaveForm.Model
{
    public class WaveDrawer
    {
        public PixelInfo[] LeftChannel { get; private set; }
        public PixelInfo[] RightChannel { get; private set; }
        public PixelInfo[] Channel { get; private set; }
    }
}
