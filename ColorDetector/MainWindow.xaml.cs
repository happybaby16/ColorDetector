using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorDetector.Model;

namespace ColorDetector
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Bitmap BM = new Bitmap(Screen.PrimaryScreen.Bounds.Width / 320, Screen.PrimaryScreen.Bounds.Height / 180);
        public void WindowMoveToCursor(object sender, EventArgs e)
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            var p = GetCursorPosition();
            this.Top = (p.Y+10);
            this.Left = (p.X+10);
            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(Convert.ToInt32(p.X)-2, Convert.ToInt32(p.Y)-2, 0, 0, BM.Size);
            imgPixcelColor.Source = BitmapToImageSource(BM);
            var pixelColor = new System.Drawing.Color();
            pixelColor = BM.GetPixel(BM.Width / 2, BM.Height / 2);
            rctEndColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(pixelColor.R,pixelColor.G,pixelColor.B));
      
        }

        public void IsMouseLClick(object sender, EventArgs e)
        {
            var pixelColor = new System.Drawing.Color();
            pixelColor = BM.GetPixel(BM.Width/2, BM.Height/2);
            System.Windows.Clipboard.SetText($"{pixelColor.R},{pixelColor.G},{pixelColor.B}");

        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            MouseHook.Start();
            MouseHook.MouseMove += WindowMoveToCursor;
            MouseHook.MouseLClick += IsMouseLClick;
        }
        



    }
    
}
