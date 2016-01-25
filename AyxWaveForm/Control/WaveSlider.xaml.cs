using AyxWaveForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AyxWaveForm.Control
{
    /// <summary>
    /// WaveSlider.xaml 的交互逻辑
    /// </summary>
    public partial class WaveSlider : UserControl
    {
        private bool isReseting = false;

        #region Properties
        public double Scale { get; private set; }
        public double StartPercent { get; private set; }
        public double MinScale { get; set; }
        #endregion

        #region DependencyProperties

        #region ThumbBackground


        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register("ThumbBackground", typeof(Brush), typeof(WaveSlider), new PropertyMetadata(null));


        #endregion

        #region ThumbWidth


        public double ThumbWidth
        {
            get { return (double)GetValue(ThumbWidthProperty); }
            internal set { SetValue(ThumbWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbWidthProperty =
            DependencyProperty.Register("ThumbWidth", typeof(double), typeof(WaveSlider), new PropertyMetadata(0.0));


        #endregion

        #region PosLineBrush


        public Brush PosLineBrush
        {
            get { return (Brush)GetValue(PosLineBrushProperty); }
            set { SetValue(PosLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PosLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PosLineBrushProperty =
            DependencyProperty.Register("PosLineBrush", typeof(Brush), typeof(WaveSlider), new PropertyMetadata(null));


        #endregion

        #region TrackLineBrush


        public Brush TrackLineBrush
        {
            get { return (Brush)GetValue(TrackLineBrushProperty); }
            set { SetValue(TrackLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrackLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackLineBrushProperty =
            DependencyProperty.Register("TrackLineBrush", typeof(Brush), typeof(WaveSlider), new PropertyMetadata(null));


        #endregion



        public bool ShowLines
        {
            get { return (bool)GetValue(ShowLinesProperty); }
            set { SetValue(ShowLinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowLines.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowLinesProperty =
            DependencyProperty.Register("ShowLines", typeof(bool), typeof(WaveSlider), new PropertyMetadata(true));


        #endregion

        #region Event

        public event EventHandler<SliderMovedEventArgs> SliderMoved;

        #endregion

        public WaveSlider()
        {
            InitializeComponent();
            Reset();
        }

        public void SetScale(double delta, double mousePer)
        {
            var oldScale = Scale;
            if (delta < 0)
            {
                if (Scale == 1) return;
                Scale += 0.05;
                if (Scale > 1) Scale = 1;
            }
            else if (delta > 0)
            {
                if (Scale == MinScale) return;
                Scale -= 0.05;
                if (Scale < MinScale) Scale = MinScale;
            }
            if (Scale == 1)
            {
                MySlider.Value = 1;
                MySlider_ValueChanged(null, null);
            }
            else
            {
                var newValue = (StartPercent + (oldScale - Scale) * mousePer) / (1 - Scale);
                if (newValue < 0)
                    newValue = 0;
                if (newValue > 1)
                    newValue = 1;
                if (newValue != MySlider.Value)
                    MySlider.Value = newValue;
                else
                    MySlider_ValueChanged(null, null);
            }
            Self_SizeChanged(null, null);
        }
        public void Reset()
        {
            isReseting = true;
            Scale = 1;
            StartPercent = 0;
            MySlider.Value = 1;
            Self_SizeChanged(null, null);
            isReseting = false;
        }
        public void SetImage(ImageSource img)
        {
            SliderImage.Source = img;
        }
        public void MoveThumb(double x)
        {
            if (Scale == 1) return;
            MySlider.Value = x / (ActualWidth * (1 - Scale));
            PosLine.X2 = PosLine.X1 = x;
        }

        private void MySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StartPercent = (1 - Scale) * MySlider.Value;
            if(!isReseting && SliderMoved != null)
                SliderMoved(this, new SliderMovedEventArgs(StartPercent,Scale));
        }

        private void Self_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                ThumbWidth = this.ActualWidth * Scale;
                PosLine.Y2 = TrackLine.Y2 = this.ActualHeight;

                if (e == null) return;
                var k = e.NewSize.Width / e.PreviousSize.Width;
                PosLine.X1 = PosLine.X2 = PosLine.X1 * k;
                TrackLine.X1 = TrackLine.X2 = TrackLine.X1 * k;
            }
            catch { }
        }
    }
}
