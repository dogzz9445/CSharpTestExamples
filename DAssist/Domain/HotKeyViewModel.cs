using DAssist.Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DAssist.Domain
{
    public class HotKey : INotifyPropertyChanged
    {
        private KeyModifier _firstKeyModifier;
        private KeyModifier _secondKeyModifier;
        private Keys _actionKey;

        public KeyModifier FirstKeyModifier
        {
            get => _firstKeyModifier;
            set
            {
                _firstKeyModifier = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("FirstKeyModifier"));
            }
        }

        public KeyModifier SecondKeyModifier
        {
            get => _secondKeyModifier;
            set
            {
                _secondKeyModifier = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("SecondKeyModifier"));
            }
        }

        public Keys ActionKey
        {
            get => _actionKey;
            set
            {
                _actionKey = value;
                RaisePropertyChanged(this, new PropertyChangedEventArgs("ActionKey"));
            }
        }

        public KeyModifier GetKeyModifier
        {
            get
            {
                return _firstKeyModifier & _secondKeyModifier;
            }
        }

        public HotKey()
        {
            FirstKeyModifier = KeyModifier.Ctrl;
            SecondKeyModifier = KeyModifier.Shift;
            ActionKey = Keys.NumPad1;
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
