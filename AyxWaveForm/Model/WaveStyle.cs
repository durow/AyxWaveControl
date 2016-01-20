using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AyxWaveForm.Model
{
    public class WaveStyle:DependencyObject
    {
        #region WaveBackground


        public Brush WaveBackground
        {
            get { return (Brush)GetValue(WaveBackgroundProperty); }
            set { SetValue(WaveBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaveBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaveBackgroundProperty =
            DependencyProperty.Register("WaveBackground", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region WaveBrush


        public Brush WaveBrush
        {
            get { return (Brush)GetValue(WaveBrushProperty); }
            set { SetValue(WaveBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaveBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaveBrushProperty =
            DependencyProperty.Register("WaveBrush", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region PosLineBrush


        public Brush PosLineBrush
        {
            get { return (Brush)GetValue(PosLineBrushProperty); }
            set { SetValue(PosLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PosLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PosLineBrushProperty =
            DependencyProperty.Register("PosLineBrush", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region TrackLineBrush


        public Brush TrackLineBrush
        {
            get { return (Brush)GetValue(TrackLineBrushProperty); }
            set { SetValue(TrackLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrackLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackLineBrushProperty =
            DependencyProperty.Register("TrackLineBrush", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region MiddleLineBrush


        public Brush MiddleLineBrush
        {
            get { return (Brush)GetValue(MiddleLineBrushProperty); }
            set { SetValue(MiddleLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MiddleLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MiddleLineBrushProperty =
            DependencyProperty.Register("MiddleLineBrush", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region GridBrush


        public Brush GridBrush
        {
            get { return (Brush)GetValue(GridBrushProperty); }
            set { SetValue(GridBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GridBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register("GridBrush", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region ShowMiddleLine


        public bool ShowMiddleLine
        {
            get { return (bool)GetValue(ShowMiddleLineProperty); }
            set { SetValue(ShowMiddleLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowMiddleLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowMiddleLineProperty =
            DependencyProperty.Register("ShowMiddleLine", typeof(bool), typeof(WaveStyle), new PropertyMetadata(true));


        #endregion

        #region ShowGrid


        public bool ShowGrid
        {
            get { return (bool)GetValue(ShowGridProperty); }
            set { SetValue(ShowGridProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowGrid.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowGridProperty =
            DependencyProperty.Register("ShowGrid", typeof(bool), typeof(WaveStyle), new PropertyMetadata(true));


        #endregion

        #region TimeTextBrush


        public Brush TimeTextBrush
        {
            get { return (Brush)GetValue(TimeTextBrushProperty); }
            set { SetValue(TimeTextBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeTextBrushProperty =
            DependencyProperty.Register("TimeTextBrush", typeof(Brush), typeof(WaveStyle), new PropertyMetadata(null));


        #endregion

        #region ShowTimeText


        public bool ShowTimeText
        {
            get { return (bool)GetValue(ShowTimeTextProperty); }
            set { SetValue(ShowTimeTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowTimeText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTimeTextProperty =
            DependencyProperty.Register("ShowTimeText", typeof(bool), typeof(WaveStyle), new PropertyMetadata(true));


        #endregion

        public WaveStyle()
        {
            WaveBackground = new SolidColorBrush(Color.FromRgb(26,61,91));
            WaveBrush = new SolidColorBrush(Color.FromRgb(93, 175, 235));
            PosLineBrush = new SolidColorBrush(Color.FromRgb(110, 197, 114));
            TrackLineBrush = new SolidColorBrush(Color.FromRgb(240, 228, 159));
            MiddleLineBrush = new SolidColorBrush(Color.FromRgb(93, 175, 235));
            GridBrush = Brushes.DarkGreen;
            TimeTextBrush = Brushes.White;
        }
    }
}
