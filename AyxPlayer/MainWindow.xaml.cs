using Microsoft.Win32;
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

namespace AyxPlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != true) return;
            try
            {
                MyWaveForm.LoadFromFile(ofd.FileName);
                var file = MyWaveForm.WavFile;
                var type = file.GetType();
                var result = "";
                foreach (var prop in type.GetProperties())
                {
                    result += prop.Name + ":" + prop.GetValue(file,null) + "\n";
                }
                InfoText.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                InfoText.Text = "";
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
