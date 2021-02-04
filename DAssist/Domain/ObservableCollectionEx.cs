using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DAssist.Domain
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public ObservableCollectionEx() : base()
        {
            this.CollectionChanged += new NotifyCollectionChangedEventHandler(OnPropertyCollectionChagned);
        }

        private void OnPropertyCollectionChagned(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += new PropertyChangedEventHandler(RaisePropertyChanged);
                }
            }
            if (e.OldItems != null)
            {
                foreach (Object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= new PropertyChangedEventHandler(RaisePropertyChanged);
                }
            }
        }

        public event PropertyChangedEventHandler ItemPropertyChanged;

        private Action<object, PropertyChangedEventArgs> RaisePropertyChanged => (sender, args) => ItemPropertyChanged?.Invoke(sender, args);
    }
}
