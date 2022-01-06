using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;


namespace ColorDetector.View
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        Timer timer = new Timer();
        int width = Screen.PrimaryScreen.Bounds.Width;
        int height = Screen.PrimaryScreen.Bounds.Height;
        public MessageBoxWindow(System.Drawing.Color selectedColor)
        {
            InitializeComponent();
            rctColor.Fill =  new SolidColorBrush(System.Windows.Media.Color.FromRgb(selectedColor.R, selectedColor.G, selectedColor.B));
            DataContext = new object[] { selectedColor};
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseApp(null, new EventArgs());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = height - this.ActualHeight;
            this.Left = width - this.ActualWidth;
            timer.Interval = 4000;
            timer.Tick += CloseApp;
            timer.Start();
        }

        //Метод закрываюший приложение
        public void CloseApp(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
