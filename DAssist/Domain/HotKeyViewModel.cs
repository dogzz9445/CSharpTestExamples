using DAssist.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DAssist.Domain
{
    public class HotKey : INotifyPropertyChanged
    {
        private string _firstKeyModifier;
        private string _secondKeyModifier;
        private string _actionKey;

        public string FirstKeyModifier
        {
            get => _firstKeyModifier;
            set
            {
                _firstKeyModifier = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("FirstKeyModifier"));
            }
        }

        public string SecondKeyModifier
        {
            get => _secondKeyModifier;
            set
            {
                _secondKeyModifier = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("SecondKeyModifier"));
            }
        }

        public string ActionKey
        {
            get => _actionKey;
            set
            {
                _actionKey = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("ActionKey"));
            }
        }

        public HotKey()
        {
            FirstKeyModifier = Enum.GetName(typeof(KeyModifier), KeyModifier.Ctrl);
            SecondKeyModifier = Enum.GetName(typeof(KeyModifier), KeyModifier.Shift);
            ActionKey = Enum.GetName(typeof(Key), Key.NumPad1);
        }

        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class HotKeyViewModel : INotifyPropertyChanged
    {
        private HotKey _propertyHotKey;

        public HotKeyViewModel()
        {
            PropertyHotKey = new HotKey();
            PropertyHotKey.PropertyChanged += RaisePropertyChanged;
        }

        public HotKey PropertyHotKey
        {
            get => _propertyHotKey;
            set
            {
                _propertyHotKey = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("PropertyHotKey"));
            }
        }

        private void RaisePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
