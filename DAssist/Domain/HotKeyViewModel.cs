using DAssist.Manager;
using DAssist.Model;
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
