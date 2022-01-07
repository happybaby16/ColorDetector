using ColorDetector.Model.Settings;
using System;
using System.Threading.Tasks;
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
        private bool IsVisible { get; set; }=false;//Показывает скрыто ли сейчас окно меню настроек приложения
        public SettingsWindow(SettingsApplication stgsApp)
        {
            InitializeComponent();
            DataContext = stgsApp;
            currentSettigns = stgsApp;
            this.IsVisibleChanged+= DrawMenuSettings;
            this.Left = width - this.Width;
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            currentSettigns.SaveSettings();
        }


        public async void DrawMenuSettings(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible)//Показывает меню
            {
                this.Visibility = Visibility.Visible;
                for (int i = 0; i < this.Height; i++)
                {
                    this.Top = height - i;
                    if (i % 3 == 0)
                    {
                        await Task.Delay(1);
                    }
                }
                IsVisible = true;
            }
            else//скрывает меню
            {
                double actualHeight = this.Top;
                for (int i=0;i<this.Height;i++)
                {
                    this.Top = actualHeight +i;
                    if (i % 3 == 0)
                    {
                        await Task.Delay(1);
                    }
                }
                this.Visibility = Visibility.Hidden;
                IsVisible = false;
            }
        }
    }
}
