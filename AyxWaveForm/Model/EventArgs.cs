using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AyxWaveForm.Model
{
    public class SliderMovedEventArgs
    {
        public double StartPercent { get; private set; }

        public SliderMovedEventArgs(double startPer)
        {
            StartPercent = startPer;
        }
    }
}
