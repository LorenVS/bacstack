using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core.Models
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool changeProperty<T>(ref T value, T newValue, string propertyName)
        {
            bool ret = false;
            var comparer = EqualityComparer<T>.Default;
            if(!comparer.Equals(value, newValue))
            {
                ret = true;
                value = newValue;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            return ret;
        }

        protected void onPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
