/*
 * Author:durow
 * Date:2016.01.19
 * Description:Set the slider's style
*/

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

        #region ThumbBackground


        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region SliderBackground


        public Brush SliderBackground
        {
            get { return (Brush)GetValue(SliderBackgroundProperty); }
            set { SetValue(SliderBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderBackgroundProperty =
            DependencyProperty.Register("SliderBackground", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region SliderWaveBrush


        public Brush SliderWaveBrush
        {
            get { return (Brush)GetValue(SliderWaveBrushProperty); }
            set { SetValue(SliderWaveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderWaveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderWaveBrushProperty =
            DependencyProperty.Register("SliderWaveBrush", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region SliderBorderThickness


        public Thickness SliderBorderThickness
        {
            get { return (Thickness)GetValue(SliderBorderThicknessProperty); }
            set { SetValue(SliderBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderBorderThicknessProperty =
            DependencyProperty.Register("SliderBorderThickness", typeof(Thickness), typeof(SliderStyle), new PropertyMetadata(new Thickness(0)));


        #endregion

        #region SliderBorderBrush


        public Brush SliderBorderBrush
        {
            get { return (Brush)GetValue(SliderBorderBrushProperty); }
            set { SetValue(SliderBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderBorderBrushProperty =
            DependencyProperty.Register("SliderBorderBrush", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region SliderHeight


        public double SliderHeight
        {
            get { return (double)GetValue(SliderHeightProperty); }
            set { SetValue(SliderHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderHeightProperty =
            DependencyProperty.Register("SliderHeight", typeof(double), typeof(SliderStyle), new PropertyMetadata(30.0));


        #endregion

        #region PosLineBrush


        public Brush PosLineBrush
        {
            get { return (Brush)GetValue(PosLineBrushProperty); }
            set { SetValue(PosLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PosLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PosLineBrushProperty =
            DependencyProperty.Register("PosLineBrush", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region TrackLineBrush


        public Brush TrackLineBrush
        {
            get { return (Brush)GetValue(TrackLineBrushProperty); }
            set { SetValue(TrackLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrackLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackLineBrushProperty =
            DependencyProperty.Register("TrackLineBrush", typeof(Brush), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        #region ShowSliderLines


        public bool ShowSliderLines
        {
            get { return (bool)GetValue(ShowSliderLinesProperty); }
            set { SetValue(ShowSliderLinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSliderLines.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSliderLinesProperty =
            DependencyProperty.Register("ShowSliderLines", typeof(bool), typeof(SliderStyle), new PropertyMetadata(null));


        #endregion

        public SliderStyle()
        {
            ThumbBackground = new SolidColorBrush(Color.FromRgb(99,184,239));
            SliderBackground = new SolidColorBrush(Color.FromRgb(0, 17, 33));
            SliderWaveBrush = new SolidColorBrush(Color.FromRgb(80, 148, 211));
            SliderBorderBrush = Brushes.Black;
            PosLineBrush = Brushes.Yellow;
            TrackLineBrush = Brushes.Red;
            ShowSliderLines = true;
        }
    }
}
