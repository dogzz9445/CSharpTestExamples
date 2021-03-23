using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAssist.Model
{
    public class Display : INotifyPropertyChanged
    {
        private bool primary;
        private string deviceName;
        private string resolution;
        private double brightness;
        private double gamma;

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<object, PropertyChangedEventArgs> RaisePropertyChanged => (sender, args) => PropertyChanged?.Invoke(sender, args);

        public bool Primary
        {
            get => primary;
            set
            {
                primary = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Primary)));
            }
        }

        public string Name
        {
            get
            {
                if (primary)
                {
                    return "주모니터";
                }
                string[] astr = DeviceName.Split('\\');
                return astr[astr.Length - 1];
            }
        }

        public string DeviceName
        {
            get => deviceName;
            set
            {
                deviceName = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(DeviceName)));
            }
        }

        public string Resolution
        {
            get => resolution;
            set
            {
                resolution = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Resolution)));
            }
        }

        public double Brightness
        {
            get => brightness;
            set
            {
                brightness = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Brightness)));
            }
        }
        public double Gamma
        {
            get => gamma;
            set
            {
                gamma = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs(nameof(Gamma)));
            }
        }
    }
}
