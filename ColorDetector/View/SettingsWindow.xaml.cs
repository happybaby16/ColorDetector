using ColorDetector.Model.Settings;
using System.Windows;
using System.Windows.Forms;

namespace ColorDetector.View
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        SettingsApplication currentSettigns;
        int width = Screen.PrimaryScreen.Bounds.Width;
        int height = Screen.PrimaryScreen.Bounds.Height;
        public SettingsWindow(SettingsApplication stgsApp)
        {
            InitializeComponent();
            DataContext = stgsApp;
            currentSettigns = stgsApp;
           
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            currentSettigns.SaveSettings();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = height - this.ActualHeight;
            this.Left = width - this.ActualWidth;
        }
    }
}
