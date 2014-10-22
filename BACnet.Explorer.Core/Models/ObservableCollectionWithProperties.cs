using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core.Models
{
    public class ObservableCollectionWithProperties<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        protected bool changeProperty<T1>(ref T1 value, T1 newValue, string propertyName)
        {
            bool ret = false;
            var comparer = EqualityComparer<T1>.Default;
            if (!comparer.Equals(value, newValue))
            {
                ret = true;
                value = newValue;
                this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
            return ret;
        }

        protected void onPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
