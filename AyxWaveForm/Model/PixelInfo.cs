/*
 * Author:durow
 * Date:2016.01.21
 * Description:The information of the pixel.Used to draw wave fast.
*/

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

        /// <summary>
        /// New instance with no data,Min and Max are both -1
        /// </summary>
        public PixelInfo()
        {
            Min = Max = -1;
        }

        /// <summary>
        /// New instance width two values.
        /// The smaller one is Min.
        /// The bigger one is Max.
        /// </summary>
        /// <param name="a">one value</param>
        /// <param name="b">another value</param>
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

        /// <summary>
        /// Push a value to PixelInfo
        /// If x is bigger than Max,Max=x
        /// If x is smaller than Min,Min=x
        /// If PixelInfo has no data,Max=Min=x.
        /// </summary>
        /// <param name="x"></param>
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

        /// <summary>
        /// Push a PixelInfo into this
        /// If info.Max is bigger than Max,Max=info.Max.
        /// If info.Min is smaller than Min,Min=info.Min.
        /// If has no data,Min=info.Min and Max=info.Max
        /// </summary>
        /// <param name="info">The PixelInfo pushed into</param>
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
