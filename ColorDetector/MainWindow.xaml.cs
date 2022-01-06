using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorDetector.Model;
using ColorDetector.View;
using ColorDetector.Model.Settings;

namespace ColorDetector
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SettingsWindow stgsAppWindow;
        SettingsApplication stgsApp;
        public static Bitmap BM;

        /// <summary>
        /// Отвечает за движение окна приложения за курсором и за отрисовку контенка внутри окна (загрузка приближенной картинки)
        /// </summary>
        public void WindowMoveToCursor(object sender, EventArgs e)
        {
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            System.Windows.Point p = GetCursorPosition();
            //Небольшое изменение координат для помещение окна прилоежния правее курсора
            this.Top = (p.Y+10);
            this.Left = (p.X+10);
            DrawImage(p);
        }

        /// <summary>
        /// Отвечает за отрисовку контента в приложении (загрузка картинки)
        /// </summary>
        /// <param name="points">Координаты курсора</param>
        private void DrawImage(System.Windows.Point points)
        {
            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(Convert.ToInt32(points.X) - 2, Convert.ToInt32(points.Y) - 2, 0, 0, BM.Size);
            imgPixcelColor.Source = BitmapToImage.BitmapToImageSource(BM);
            var pixelColor = SearchColor;
            //Меняем цвет квадрата в правом нижнем углу, чтобы показать конечный цвет, который будет скопирован в буффер обмена
            rctEndColor.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(pixelColor.R, pixelColor.G, pixelColor.B));
        }

        /// <summary>
        /// Возвращает цвет пикселя, наведённого на курсор
        /// </summary>
        private System.Drawing.Color SearchColor
        {
            get=>BM.GetPixel(BM.Width / 2, BM.Height / 2);
        }

        /// <summary>
        /// Отвечает за копирование цвета в буффер обмена
        /// </summary>
        public void IsMouseLClick(object sender, EventArgs e)
        {
            var pixelColor = SearchColor;
            if (stgsApp.IsCopyToClipboard)
            {
                System.Windows.Clipboard.SetText($"{pixelColor.R},{pixelColor.G},{pixelColor.B}");
            }
            if (stgsApp.IsGetMessage)
            {
                new MessageBoxWindow(pixelColor).Show();
            }
            
        }
        /// <summary>
        /// Устанавливает настройки в ходе выполнения программы
        /// </summary>
        private void UpdateSettings(object sender, EventArgs e)
        {
            BM = new Bitmap(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * stgsApp.Zoom), Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * stgsApp.Zoom));
        }

        private void StartApplication(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
            MouseHook.Start();
        }

        private void StopApplication(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            MouseHook.Stop();
        }

        public MainWindow()
        {
            InitializeComponent();
            AppLoad();

        }
        public void AppLoad()
        {
            stgsApp = new SettingsApplication();
            stgsApp.GetSettings();
            stgsApp.PropertyChanged += UpdateSettings;
            BM = new Bitmap(Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width * stgsApp.Zoom), Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height * stgsApp.Zoom));
            
            MouseHook.MouseMove += WindowMoveToCursor;
            MouseHook.MouseLClick += IsMouseLClick;
            KeyboardHook.Start();
            KeyboardHook.KeyboardStartApplication += StartApplication;
            KeyboardHook.KeyboardStopApplication += StopApplication;
            stgsAppWindow = new SettingsWindow(stgsApp);
            stgsAppWindow.Visibility = Visibility.Visible;
        }

    }
    
}
