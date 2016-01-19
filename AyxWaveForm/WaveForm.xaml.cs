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

namespace AyxWaveForm
{
    /// <summary>
    /// WaveForm.xaml 的交互逻辑
    /// </summary>
    public partial class WaveForm : UserControl
    {
        #region Properties
        public double LeftTime { get; private set; }
        public double PosLineTime { get; private set; }
        public double TrackLineTime { get; private set; }
        #endregion
        public WavFile WavFile { get; private set; }
        public WaveForm()
        {
            InitializeComponent();
        }

        public void LoadFromFile(string filename)
        {
            WavFile = WavFile.Read(filename);
            MainSlider.MinScale = WavFile.MinScale;
            SetChannels();
            DrawWaveImage(MainSlider.StartPercent, MainSlider.Scale);
        }
        public void Play()
        { }

        public void Pause()
        { }

        public void Stop()
        { }

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
        private void DrawWaveImage(double startPer, double scale)
        {
            if (WavFile.Channels == 1)
            {
                SingleChannelImage.Source = WavFile.DrawChannel(startPer, scale);
                MainSlider.SetImage(WavFile.DrawSimple(MainSlider.ActualHeight));
            }
            else
            {
                //SingleChannelGrid.Visibility = Visibility.Collapsed;
                //DoubleChannelGrid.Visibility = Visibility.Visible;
                SingleChannelImage.Source = WavFile.DrawChannel(startPer, scale);
                MainSlider.SetImage(WavFile.DrawSimple(MainSlider.ActualHeight));
            }
        }

        private void MainSlider_SliderMoved(object sender, Model.SliderMovedEventArgs e)
        {
            DrawWaveImage(e.StartPercent, e.Scale);
        }

        private void MainBorder_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var x = e.GetPosition(MainGrid).X;
            if (x < 0) return;

            MainSlider.SetScale(e.Delta, x / MainGrid.ActualWidth);
        }
    }
}
