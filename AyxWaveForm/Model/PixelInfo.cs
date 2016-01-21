using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AyxWaveForm.Model
{
    public class PixelInfo
    {
        /// <summary>
        /// The minimum value of the pixel
        /// </summary>
        public short Min { get; set; }

        /// <summary>
        /// The maximum value of the pixel
        /// </summary>
        public short Max { get; set; }

        public PixelInfo()
        {
            Min = Max = -1;
        }
        public PixelInfo(short a,short b)
        {
            if(a > b)
            {
                Max = a;
                Min = b;
            }
            else
            {
                Max = b;
                Min = a;
            }
        }

        public void Push(short x)
        {
            if (Min == -1)
            {
                Min = x;
                Max = x;
            }
            else
            {
                if (x < Min)
                    Min = x;
                if (x > Max)
                    Max = x;
            }
        }

        public void Push(PixelInfo info)
        {
            if(Min == -1)
            {
                Min = info.Min;
                Max = info.Max;
            }
            else
            {
                if (info.Min < Min)
                    Min = info.Min;
                if (info.Max > Max)
                    Max = info.Max;
            }
        }
    }
}
