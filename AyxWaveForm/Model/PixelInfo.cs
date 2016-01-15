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
        public short Min { get; private set; }

        /// <summary>
        /// The maximum value of the pixel
        /// </summary>
        public short Max { get; private set; }

        public PixelInfo(short a=0,short b=0)
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
    }
}
