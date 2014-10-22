using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Core
{
    public class SubscriptionList<T> : IDisposable
    {
        /// <summary>
        /// The array of observers that have been registered
        /// </summary>
        private IObserver<T>[] _array;

        /// <summary>
        /// Retrieves the list of observers currently registered
        /// </summary>
        public ReadOnlyArray<IObserver<T>> Array
        {
            get { return new ReadOnlyArray<IObserver<T>>(false, _array); }
        }

        /// <summary>
        /// Constructs a new subscription list instance
        /// </summary>
        public SubscriptionList()
        {
            _array = new IObserver<T>[0];
        }

        /// <summary>
        /// Disposes of the subscription list
        /// </summary>
        public void Dispose()
        {
            Clear();
        }

        /// <summary>
        /// Registers an observer with the subscription list
        /// </summary>
        /// <param name="observer">The observer to register</param>
        public void Register(IObserver<T> observer)
        {
            IObserver<T>[] old = _array;
            IObserver<T>[] @new = null;

            do
            {
                old = _array;
                @new = new IObserver<T>[old.Length + 1];
                System.Array.Copy(old, @new, old.Length);
                @new[@new.Length - 1] = observer;
            } while(Interlocked.Exchange(ref _array, @new) != old);
        }

        /// <summary>
        /// Unregisters an observer with the subscription list
        /// </summary>
        /// <param name="observer">The observer to unregister</param>
        /// <param name="complete">True if the OnCompleted event should be fired</param>
        /// <returns>True if an observer was unregistered, false otherwise</returns>
        public bool Unregister(IObserver<T> observer, bool complete = true)
        {
            IObserver<T>[] old = _array;
            IObserver<T>[] @new = null;
            bool removed = false;

            do
            {
                old = _array;
                if (!old.Contains(observer))
                    break;

                @new = new IObserver<T>[old.Length - 1];
                System.Array.Copy(old, @new, @new.Length);

                for (int i = 0; i < @new.Length; i++)
                {
                    if (@new[i] == observer)
                    {
                        removed = true;
                        @new[i] = old[old.Length - 1];
                    }
                }

                if (!removed)
                    break; // should never happen

            } while (Interlocked.Exchange(ref _array, @new) != old);

            if (complete && removed)
            {
                Task.Factory.StartNew(() =>
                {
                    observer.OnCompleted();
                });
            }

            return removed;
        }

        /// <summary>
        /// Clears the subscription list, removing all observers
        /// </summary>
        /// <param name="complete">True if the OnCompleted event should be called for each observer</param>
        /// <returns>The list of removed observers</returns>
        public IObserver<T>[] Clear(bool complete = true)
        {
            IObserver<T>[] @new = new IObserver<T>[0];
            var old = Interlocked.Exchange(ref _array, @new);

            if (complete)
            {
                Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < old.Length; i++)
                    {
                        old[i].OnCompleted();
                    }
                });
            }

            return old;
        }

        /// <summary>
        /// Propagates a data element to each observer
        /// </summary>
        /// <param name="data">The data element to propagate</param>
        public void Next(T data)
        {
            var array = this.Array;
            for(int i = 0; i < array.Length; i++)
            {
                array[i].OnNext(data);
            }
        }
    }
}
