/*
 * Author:durow
 * Date:2016.01.20
*/

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

        #region WaveStyle

        public WaveStyle WaveStyle
        {
            get { return (WaveStyle)GetValue(WaveStyleProperty); }
            set { SetValue(WaveStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WaveStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WaveStyleProperty =
            DependencyProperty.Register("WaveStyle", typeof(WaveStyle), typeof(WaveForm), new PropertyMetadata(null));

        #endregion

        #region SliderStyle

        public SliderStyle SliderStyle
        {
            get { return (SliderStyle)GetValue(SliderStyleProperty); }
            set { SetValue(SliderStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SliderStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SliderStyleProperty =
            DependencyProperty.Register("SliderStyle", typeof(SliderStyle), typeof(WaveForm), new PropertyMetadata(null));

        #endregion

        #region PosLineTime


        public double PosLineTime
        {
            get { return (double)GetValue(PosLineTimeProperty); }
            private set { SetValue(PosLineTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PosLineTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PosLineTimeProperty =
            DependencyProperty.Register("PosLineTime", typeof(double), typeof(WaveForm), new PropertyMetadata(0.0));


        #endregion

        #region TrackLineTime


        public double TrackLineTime
        {
            get { return (double)GetValue(TrackLineTimeProperty); }
            private set { SetValue(TrackLineTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TrackLineTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TrackLineTimeProperty =
            DependencyProperty.Register("TrackLineTime", typeof(double), typeof(WaveForm), new PropertyMetadata(0.0));


        #endregion

        #endregion

        #region Fields
        private Timer playingTimer = new Timer(25);
        private bool isWorking = false;
        #endregion

        #region Properties
        public double WaveLeftTime { get; private set; }
        public WavFile WavFile { get; private set; }
        public Status Status { get; private set; }
        public bool AutoStart { get; set; }
        #endregion

        #region Event
        public event EventHandler FileLoaded;
        public event EventHandler PlayEnded;
        public event EventHandler PlayStarted;
        public event EventHandler PlayPaused;
        public event EventHandler PlayStopped;
        #endregion

        #region Constructor
        public WaveForm()
        {
            InitializeComponent();
            WaveStyle = new WaveStyle();
            SliderStyle = new SliderStyle();
            playingTimer.Elapsed += PlayingTimer_Elapsed;
        }
        #endregion

        #region PrivateMethods

        //Refresh the TrackLine,TrackLineTime and TimeText when playing
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
                    TimeText.Text = ((int)MyPlayer.Position.TotalMinutes).ToString("D2") + ":" + MyPlayer.Position.Seconds.ToString("D2") + ":" + MyPlayer.Position.Milliseconds.ToString("D3");
                    MainSlider.TrackLine.X1 = MainSlider.TrackLine.X2 = MainSlider.ActualWidth * time / WavFile.TotalSeconds;
                    CheckLine(TrackLine, time);
                    isWorking = false;
                }
                catch { isWorking = false; }
            }));
        }
        //Reset the control
        private void Reset()
        {
            WavFile = null;
            MyPlayer.Close();
            MyPlayer.Source = null;
            Status = Status.NoFile;
            PosLine.Visibility = Visibility.Visible;
            TrackLine.Visibility = Visibility.Collapsed;
            MainSlider.PosLine.X2 = MainSlider.PosLine.X1 = 0;
            MainSlider.TrackLine.Visibility = Visibility.Collapsed;
            SetLinesX(0);
            PosLineTime = 0;
            TrackLineTime = 0;
            WaveLeftTime = 0;
            TimeText.Text = "00:00.000";
        }
        //Set the X1 and X2 property of PosLine and TrackLine
        private void SetLinesX(double x)
        {
            PosLine.X1 = PosLine.X2 = x;
            TrackLine.X1 = TrackLine.X2 = x;
        }
        //Set the Y2 property of PosLine and TrackLine
        private void SetLinesY(double y)
        {
            PosLine.Y2 = TrackLine.Y2 = y;
        }
        //Set the visibility of grid when load file
        private void SetChannels()
        {
            if (WavFile.Channels == 1)
            {
                SingleChannel.Visibility = Visibility.Visible;
                DoubleChannel.Visibility = Visibility.Collapsed;
            }
            else
            {
                SingleChannel.Visibility = Visibility.Collapsed;
                DoubleChannel.Visibility = Visibility.Visible;
            }
        }
        //Refresh the slider's background image
        private void RefreshSliderImage()
        {
            MainSlider.SetImage(WavFile.DrawSimple(WaveStyle));
        }
        //Refresh MiddleLines
        private void RefreshMiddleLines()
        {
            if (WavFile == null)
                return;

            if (WavFile.Channels == 1)
            {
                SingleMiddleLine.Y1 = SingleMiddleLine.Y2 = WaveGrid.ActualHeight / 2;
                SingleMiddleLine.X2 = WaveGrid.ActualWidth;
            }
            else
            {
                LeftMiddleLine.Y1 = LeftMiddleLine.Y2 = LeftChannelGrid.ActualHeight / 2;
                LeftMiddleLine.X2 = LeftChannelGrid.ActualWidth;
                RightMiddleLine.Y1 = RightMiddleLine.Y2 = RightChannelGrid.ActualHeight / 2;
                RightMiddleLine.X2 = RightChannelGrid.ActualWidth;
            }
        }
        //Draw wave when resize and scroll
        private void DrawWaveImage()
        {
            if (WavFile == null) return;
            if (WavFile.Channels == 1)
            {
                SingleChannelImage.Source = WavFile.DrawChannel(WaveStyle,MainSlider.StartPercent, MainSlider.Scale, SingleChannel.ActualWidth, SingleChannel.ActualHeight);
            }
            else
            {
                LeftChannelImage.Source = WavFile.DrawLeftChannel(WaveStyle, MainSlider.StartPercent, MainSlider.Scale, LeftChannel.ActualWidth, LeftChannel.ActualHeight);
                RightChannelImage.Source = WavFile.DrawRightChannel(WaveStyle, MainSlider.StartPercent, MainSlider.Scale, RightChannel.ActualWidth, RightChannel.ActualHeight);
            }
        }
        //Check the line's visibility and position when scroll
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
        //Slider moved or scale changed
        private void MainSlider_SliderMoved(object sender, Model.SliderMovedEventArgs e)
        {
            if (WavFile == null) return;
            DrawWaveImage();
            WaveLeftTime = MainSlider.StartPercent * WavFile.TotalSeconds;
            CheckLine(PosLine, PosLineTime);
        }
        //Scale the wave
        private void MainBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (WavFile == null) return;
            var x = e.GetPosition(MainGrid).X;
            if (x < 0) return;

            MainSlider.SetScale(e.Delta, x / MainGrid.ActualWidth);
        }
        //When wave border size changed,need to redraw the wave
        private void WaveBorder_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.PreviousSize.Width != e.NewSize.Width)
                    DrawWaveImage();
                //refresh PosLine
                PosLine.Y2 = TrackLine.Y2 = WaveBorder.ActualHeight;
                var newX = PosLine.X1 * e.NewSize.Width / e.PreviousSize.Width;
                PosLine.X1 = PosLine.X2 = newX;
                //refresh TrackLine
                if (Status == Status.Pause)
                {
                    var newTrack = TrackLine.X1 * e.NewSize.Width / e.PreviousSize.Width;
                    TrackLine.X1 = TrackLine.X2 = newTrack;
                }
                //refresh MiddleLine
                RefreshMiddleLines();
            }
            catch { }
        }
        private void MovePosLine(double x)
        {
            PosLine.X1 = PosLine.X2 = x;
            PosLine.Visibility = Visibility.Visible;
            PosLineTime = WaveLeftTime + x * MainSlider.Scale * WavFile.TotalSeconds / MainGrid.ActualWidth;
            if (Status != Status.Playing)
            {
                var ts = TimeSpan.FromSeconds(PosLineTime);
                MyPlayer.Position = ts;
                TimeText.Text = ((int)ts.TotalMinutes).ToString("D2") + ":" + ts.Seconds.ToString("D2") + "." + ts.Milliseconds.ToString("D3");
            }
        }
        //Mouse left button down to set the PosLine and PosLineTime 
        private void MainGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (WavFile == null) return;
            var x = e.GetPosition(MainGrid).X;
            MovePosLine(x);
            var sliderPosLine = PosLineTime * MainSlider.ActualWidth / WavFile.TotalSeconds;
            MainSlider.PosLine.X1 = MainSlider.PosLine.X2 = sliderPosLine;
        }
        //Mouse right button down to stop play
        private void MainBorder_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Status == Status.Playing)
                Stop();
            else
                Play();
        }
        //When play ended
        private void MyPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Load a wav file and draw the waveform
        /// </summary>
        /// <param name="filename">Filename</param>
        public void LoadFromFile(string filename)
        {
            if (Status == Status.Playing)
                Stop();
            if (WavFile != null && WavFile.FileName == filename)
                return;
            Reset();
            MainSlider.Reset();
            WavFile = WavFile.Read(filename);
            MainSlider.MinScale = WavFile.MinScale;
            SetChannels();
            RefreshSliderImage();
            DrawWaveImage();
            RefreshMiddleLines();
            MyPlayer.Source = new Uri(filename);
            MyPlayer.Play();
            if (!AutoStart) MyPlayer.Stop();
            Status = Status.Ready;
            if (FileLoaded != null)
                FileLoaded(this, EventArgs.Empty);
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
                    playingTimer.Start();
                    MyPlayer.Play();
                    MainSlider.TrackLine.Visibility = Visibility.Visible;
                    Status = Status.Playing;
                }
                catch { }
            }
            if (Status == Status.Pause)
            {
                try
                {
                    playingTimer.Start();
                    MyPlayer.Play();
                    Status = Status.Playing;
                }
                catch { }
            }
        }

        /// <summary>
        /// Pause
        /// </summary>
        public void Pause()
        {
            if (Status == Status.Playing)
            {
                try
                {
                    MyPlayer.Pause();
                    Status = Status.Pause;
                    playingTimer.Stop();
                }
                catch { }
            }
        }

        /// <summary>
        /// Stop play
        /// </summary>
        public void Stop()
        {
            if (Status == Status.Playing ||
                Status == Status.Pause)
            {
                try
                {
                    MyPlayer.Stop();
                    Status = Status.Stop;
                    TrackLine.Visibility = Visibility.Collapsed;
                    MainSlider.TrackLine.Visibility = Visibility.Collapsed;
                    MainSlider.TrackLine.X1 = MainSlider.TrackLine.X2 = MainSlider.PosLine.X1;
                    playingTimer.Stop();
                }
                catch { }
            }
        }

        #endregion

        private void WaveBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void MainSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var x = e.GetPosition(MainSlider).X;
            MainSlider.MoveThumb(x);
            if (MainSlider.MySlider.Value == 1)
            {
                var pos = (x/MainSlider.ActualWidth - MainSlider.StartPercent) * ActualWidth / MainSlider.Scale;
                MovePosLine(pos);
            }
            else
                MovePosLine(0);
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
