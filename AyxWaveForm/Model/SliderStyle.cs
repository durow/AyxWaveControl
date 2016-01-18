using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AyxWaveForm.Model
{
    public class SliderStyle:DependencyObject
    {
        #region Background


        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Background.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region ThumbBackground


        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(Brushes.Transparent));


        #endregion

        #region ThumbWidth


        public double ThumbWidth
        {
            get { return (double)GetValue(ThumbWidthProperty); }
            set { SetValue(ThumbWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.Register("ThumbWidth", typeof(double), typeof(SliderStyle), new PropertyMetadata(0.0));


        #endregion
    }
}
