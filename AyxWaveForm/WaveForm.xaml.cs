using AyxWaveForm.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using AyxWaveForm.Model;

namespace AyxWaveForm
{
    /// <summary>
    /// WaveForm.xaml 的交互逻辑
    /// </summary>
    public partial class WaveForm : UserControl
    {

        #region DependencyProperties


        public WaveStyle WaveStyle
        {
            get { return (WaveStyle)GetValue(WaveStyleProperty); }
            set { SetValue(WaveStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaveStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaveStyleProperty =
            DependencyProperty.Register("WaveStyle", typeof(WaveStyle), typeof(WaveForm), new PropertyMetadata(null));



        public SliderStyle SliderStyle
        {
            get { return (SliderStyle)GetValue(SliderStyleProperty); }
            set { SetValue(SliderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderStyleProperty =
            DependencyProperty.Register("SliderStyle", typeof(SliderStyle), typeof(WaveForm), new PropertyMetadata(null));



        #endregion

        #region Fields
        private Timer playingTimer = new Timer(50);
        private bool isWorking = false;
        #endregion

        #region Properties
        public double WaveLeftTime { get; private set; }
        public double PosLineTime { get; private set; }
        public double TrackLineTime { get; private set; }
        public WavFile WavFile { get; private set; }
        public Status Status { get; private set; }
        #endregion

        #region Event
        public event EventHandler FileLoaded;
        #endregion

        
        public WaveForm()
        {
            InitializeComponent();
            playingTimer.Elapsed += PlayingTimer_Elapsed;
            playingTimer.Start();
        }

        private void PlayingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isWorking) return;
            if (Status != Status.Playing) return;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                isWorking = true;
                try
                {
                    var time = MyPlayer.Position.TotalSeconds;
                    TimeText.Text = ((int)MyPlayer.Position.TotalMinutes).ToString("D2") + ":" + MyPlayer.Position.Seconds.ToString("D2") +":" + MyPlayer.Position.Milliseconds.ToString("D3");
                    CheckLine(TrackLine, time);
                    isWorking = false;
                }
                catch { isWorking = false; }
            }));
        }

        public void LoadFromFile(string filename)
        {
            if (Status == Status.Playing)
                Stop();
            Reset();
            MainSlider.Reset();
            WavFile = WavFile.Read(filename);
            MainSlider.MinScale = WavFile.MinScale;
            SetChannels();
            RefreshSliderImage();
            DrawWaveImage();
            MyPlayer.Source = new Uri(filename);
            Status = Status.Ready;
            if (FileLoaded != null)
                FileLoaded(this, EventArgs.Empty);
        }
        private void Reset()
        {
            WavFile = null;
            Status = Status.NoFile;
            PosLine.Visibility = Visibility.Visible;
            TrackLine.Visibility = Visibility.Collapsed;
            SetLinesX(0);
            PosLineTime = 0;
            TrackLineTime = 0;
            WaveLeftTime = 0;
        }
        private void SetLinesX(double x)
        {
            PosLine.X1 = PosLine.X2 = x;
            TrackLine.X1 = TrackLine.X2 = x;
        }

        private void SetLinesY(double y)
        {
            PosLine.Y2 = TrackLine.Y2 = y;
        }
        /// <summary>
        /// Play
        /// </summary>
        public void Play()
        {
            if (Status == Status.Stop ||
                Status == Status.Ready)
            {
                try
                {
                    MyPlayer.Position = TimeSpan.FromSeconds(PosLineTime);
                    MyPlayer.Play();
                    Status = Status.Playing;
                }
                catch { }
            }
            if(Status == Status.Pause)
            {
                try
                {
                    MyPlayer.Play();
                }
                catch { }
            }
        }
        /// <summary>
        /// Pause
        /// </summary>
        public void Pause()
        {
            if(Status == Status.Playing)
            {
                try
                {
                    MyPlayer.Pause();
                    Status = Status.Pause;
                }
                catch { }
            }
        }
        /// <summary>
        /// Stop play
        /// </summary>
        public void Stop()
        {
            if(Status == Status.Playing ||
                Status == Status.Pause)
            {
                try
                {
                    MyPlayer.Stop();
                    Status = Status.Stop;
                    TrackLine.Visibility = Visibility.Collapsed;
                }
                catch { }
            }
        }

        //set the channel number when load file
        private void SetChannels()
        {
            if (WavFile.Channels == 1)
            {
                SingleChannelGrid.Visibility = Visibility.Visible;
                DoubleChannelGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                //SingleChannelGrid.Visibility = Visibility.Collapsed;
                //DoubleChannelGrid.Visibility = Visibility.Visible;
            }
        }

        private void RefreshSliderImage()
        {
            MainSlider.SetImage(WavFile.DrawSimple(Brushes.Lime));
        }
        //draw wave when resize and scroll
        private void DrawWaveImage()
        {
            if (WavFile == null) return;
            if (WavFile.Channels == 1)
            {
                SingleChannelImage.Source = WavFile.DrawChannel(MainSlider.StartPercent, MainSlider.Scale, SingleChannelGrid.ActualWidth,SingleChannelGrid.ActualHeight);
            }
            else
            {
                SingleChannelImage.Source = WavFile.DrawChannel(MainSlider.StartPercent, MainSlider.Scale, SingleChannelGrid.ActualWidth,SingleChannelGrid.ActualHeight);
            }
        }
        //check the line's visibility and position when scroll
        private void CheckLine(Line line, double time)
        {
            var waveRightTime = WaveLeftTime + WavFile.TotalSeconds * MainSlider.Scale;
            if (time >= WaveLeftTime && time <= waveRightTime)
            {
                var x = (time - WaveLeftTime) * MainGrid.ActualWidth / (WavFile.TotalSeconds * MainSlider.Scale);
                line.X1 = line.X2 = x;
                line.Visibility = Visibility.Visible;
            }
            else
                line.Visibility = Visibility.Collapsed;
        }
        //slider moved or scale changed
        private void MainSlider_SliderMoved(object sender, Model.SliderMovedEventArgs e)
        {
            if (WavFile == null) return;
            DrawWaveImage();
            WaveLeftTime = MainSlider.StartPercent * WavFile.TotalSeconds;
            CheckLine(PosLine,PosLineTime);
        }

        private void MainBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (WavFile == null) return;
            var x = e.GetPosition(MainGrid).X;
            if (x < 0) return;

            MainSlider.SetScale(e.Delta, x / MainGrid.ActualWidth);
        }

        private void WaveBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.PreviousSize.Width != e.NewSize.Width)
                    DrawWaveImage();
                PosLine.Y2 = TrackLine.Y2 = WaveBorder.ActualHeight;
                var newX = PosLine.X1 * e.NewSize.Width / e.PreviousSize.Width;
                PosLine.X1 = PosLine.X2 = newX;
                if(Status == Status.Pause)
                {
                    var newTrack = TrackLine.X1 * e.NewSize.Width / e.PreviousSize.Width;
                    TrackLine.X1 = TrackLine.X2 = newTrack;
                }
            }
            catch { }
        }
        //set the PosLine
        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WavFile == null) return;
            var x = e.GetPosition(MainGrid).X;
            SetLinesX(x);
            PosLine.Visibility = Visibility.Visible;
            PosLineTime = WaveLeftTime + x * MainSlider.Scale * WavFile.TotalSeconds / MainGrid.ActualWidth;
            if (Status == Status.Playing)
                MyPlayer.Position = TimeSpan.FromSeconds(PosLineTime);
            var ts = TimeSpan.FromSeconds(PosLineTime);
            TimeText.Text = ((int)ts.TotalMinutes).ToString("D2") + ":" + ts.Seconds.ToString("D2") + ":" + ts.Milliseconds.ToString("D3");
        }

        private void MainBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Status == Status.Playing)
                Stop();
            else
                Play();
        }

        private void MyPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            Stop();
        }
    }

    public enum Status
    {
        NoFile,
        Ready,
        Playing,
        Stop,
        Pause,
    }
}
