using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAssist.Domain
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
                return astr[astr.Length-1];
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

    public class RampViewModel : INotifyPropertyChanged
    {
        private int _selectedIndex;
        private Display _selectedItem;
        private ObservableCollectionEx<Display> _displays;

        public RampViewModel()
        {
            Displays = GenerateDisplaySettings();
            Displays.ItemPropertyChanged += RaisePropertyChanged;
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        public Display SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null || value.Equals(_selectedItem)) return;

                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
            }
        }

        public ObservableCollectionEx<Display> Displays
        {
            get => _displays;
            set
            {
                _displays = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Displays)));
            }
        }

        private ObservableCollectionEx<Display> GenerateDisplaySettings()
        {
            ObservableCollectionEx<Display> displays = new ObservableCollectionEx<Display>();

            // 주모니터 생성
            displays.Add(new Display()
            {
                Primary = Screen.PrimaryScreen.Primary,
                DeviceName = Screen.PrimaryScreen.DeviceName,
                Resolution = Screen.PrimaryScreen.WorkingArea.ToString(),
                Gamma = 1.0f,
                Brightness = 50.0f
            });

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    displays.Add(new Display()
                    {
                        Primary = screen.Primary,
                        DeviceName = screen.DeviceName,
                        Resolution = screen.WorkingArea.ToString(),
                        Gamma = 1.0f,
                        Brightness = 50.0f
                    });
                }
            }

            return displays;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
    }
}
