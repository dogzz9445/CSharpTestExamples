using DAssist.Manager;
using DAssist.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAssist.Domain
{
    public class RampViewModel : INotifyPropertyChanged
    {
        private int _selectedIndex;
        private Display _selectedItem;
        private ObservableCollectionEx<Display> _displays;

        public RampViewModel()
        {
            RefreshScreens();
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

        public void RefreshScreens()
        {
            if (Displays != null)
            {
                Displays.ItemPropertyChanged -= RaisePropertyChanged;
                Displays.Clear();
            }
            Displays = GenerateDisplaySettings();
            Displays.ItemPropertyChanged += RaisePropertyChanged;
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

            Display display = this.SelectedItem;
            if (display != null)
            {
                RampManager.UpdateRamp(SelectedItem.Gamma, SelectedItem.Brightness / 50.0f, SelectedItem.DeviceName);
            }
        }
    }
}
