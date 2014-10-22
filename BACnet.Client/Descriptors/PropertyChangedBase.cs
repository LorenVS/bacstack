using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Descriptors
{
    public class PropertyChangedBase : INotifyPropertyChanged
    {
        private event PropertyChangedEventHandler _propertyChanged;

        /// <summary>
        /// Event raised whenever a property on this object changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        /// <summary>
        /// Changes a property on an object, invoking the PropertyChanged event
        /// if necessary
        /// </summary>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="obj">The INotifyPropertyChanged object</param>
        /// <param name="field">The backing field of the property</param>
        /// <param name="value">The new value of the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>True if the property was changed, false otherwise</returns>
        protected bool changeProperty<T>(ref T field, T value, string propertyName)
        {
            bool changed = false;
            var comparer = EqualityComparer<T>.Default;
            if (!comparer.Equals(field, value))
            {
                field = value;
                changed = true;
                var ev = _propertyChanged;
                if (ev != null)
                    ev(this, new PropertyChangedEventArgs(propertyName));
            }
            return changed;
        }
    }
}
