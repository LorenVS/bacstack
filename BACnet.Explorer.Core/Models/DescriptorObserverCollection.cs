using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Client.Descriptors;
using Eto.Forms;

namespace BACnet.Explorer.Core.Models
{
    public class DescriptorObserverCollection<TValue, TKey> : ObservableCollection<TValue>, IDescriptorObserver<TValue, TKey>
        where TValue : ICloneable, ISyncable, IStronglyKeyed<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The eto application
        /// </summary>
        private Application _application;

        /// <summary>
        /// Constructs a new descriptor observer collection
        /// </summary>
        /// <param name="application">The eto application</param>
        public DescriptorObserverCollection(Application application)
        {
            this._application = application;
        }

#region IDescriptorSubscription Implementation

        void IDescriptorObserver<TValue, TKey>.Close()
        {

        }

        void IDescriptorObserver<TValue, TKey>.Add(IEnumerable<TValue> values)
        {
            _application.AsyncInvoke(() =>
            {
                foreach (var value in values)
                {
                    this.Add((TValue)value.Clone());
                }
            });
        }

        void IDescriptorObserver<TValue, TKey>.Add(TValue value)
        {
            _application.AsyncInvoke(() =>
            {
                this.Add((TValue)value.Clone());
            });
        }

        void IDescriptorObserver<TValue, TKey>.Remove(IEnumerable<TValue> values)
        {
            _application.AsyncInvoke(() =>
            {
                var keys = values.Select(value => value.Key).ToArray();
                for(int i = 0; i < this.Count; i++)
                {
                    if(keys.Contains(this[i].Key))
                    {
                        this.RemoveItem(i);
                        i--;
                    }
                }
            });
        }

        void IDescriptorObserver<TValue, TKey>.Remove(TValue value)
        {
            _application.AsyncInvoke(() =>
            {
                for(int i = 0; i < this.Count; i++)
                {
                    if (this[i].Key.Equals(value.Key))
                    {
                        this.RemoveItem(i);
                        i--;
                    }
                }
            });
        }

        void IDescriptorObserver<TValue, TKey>.Update(TValue value)
        {
            _application.AsyncInvoke(() =>
            {
                for(int i = 0; i < this.Count; i++)
                {
                    var existing = this[i];
                    if (existing.Key.Equals(value.Key))
                    {
                        var clone = (TValue)value.Clone();
                        this.SetItem(i, clone);
                    }
                }
            });
        }

        void IDescriptorObserver<TValue, TKey>.Update(IEnumerable<TValue> values)
        {
            _application.AsyncInvoke(() =>
            {
                var dict = values.ToDictionary(val => val.Key);
                for (int i = 0; i < this.Count; i++)
                {
                    var existing = this[i];
                    if (dict.ContainsKey(existing.Key))
                    {
                        var clone = (TValue)dict[existing.Key].Clone();
                        this.SetItem(i, clone);
                    }
                }
            });
        }

#endregion
    }
}
