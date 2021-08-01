using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace EmguCvMemoryLeak
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoCapture videoCapture;
        private bool flag;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                flag = false;
                videoCapture = new VideoCapture(@"./example.mp4");
                if (!videoCapture.IsOpened)
                {
                    throw new Exception("file not found.copy the example.mp4 to bin/x86/debug/");
                }

                videoCapture.ImageGrabbed += VideoCapture_ImageGrabbed;
                videoCapture.Start();

                SpinWait.SpinUntil(() => flag);

                videoCapture.Stop();
                videoCapture.ImageGrabbed -= VideoCapture_ImageGrabbed;
                videoCapture.Dispose();
            }
        }

        private void VideoCapture_ImageGrabbed(object sender, EventArgs e)
        {
            Mat mat = new Mat();
            if (!videoCapture.Retrieve(mat) || mat.IsEmpty)
            {
                flag = true;
            }

            mat.Dispose();
        }
    }
}
