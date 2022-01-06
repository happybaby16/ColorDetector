using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ColorDetector.Model.Settings
{
    public class SettingsApplication: INotifyPropertyChanged
    {
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;


        private static double _zoom = 0.005;
        public  double Zoom { get { return _zoom; } set { _zoom = value; NotifyPropertyChanged(); } }//Параметр отвечающий за приближение

        private static bool _isGetMessage = true;
        public bool IsGetMessage { get=> _isGetMessage; set { _isGetMessage = value; NotifyPropertyChanged(); } }//Параметр отвечающий за получение сообщения о скопированном цвете
        
        private static bool _isCopyToClipboard = true;
        public bool IsCopyToClipboard { get => _isCopyToClipboard; set { _isCopyToClipboard = value; NotifyPropertyChanged(); } }//Параметр отвечающий за копирование в буффер обмена

        public void GetSettings()
        {
            try
            {
                using (StreamReader sr = new StreamReader("AppSettings"))
                {
                    var lineSettings = sr.ReadLine();
                    if (lineSettings != string.Empty&&lineSettings!=null)
                    {
                        var dataSettings = lineSettings.Split(';');
                        _zoom = Convert.ToDouble(dataSettings[0]);
                        IsGetMessage = Convert.ToBoolean(dataSettings[1]);
                        IsCopyToClipboard = Convert.ToBoolean(dataSettings[2]);
                    }
                 
                }
            }
            catch
            { }
        }

        public void SaveSettings()
        {
            using (StreamWriter sr = new StreamWriter("AppSettings"))
            {
                string lineSettings = $"{_zoom};{_isGetMessage};{_isCopyToClipboard}";
                sr.WriteLine(lineSettings);
            }
        }

    }
}
