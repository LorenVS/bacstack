using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    public struct ReadOnlyArray<T> : IEnumerable<T>
    {
        /// <summary>
        /// Loads a read only array from a value stream
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        /// <returns>The read only array</returns>
        public static ReadOnlyArray<T> Load(IValueStream stream)
        {
            var loader = Value<T>.Loader;

            List<T> temp = new List<T>();
            stream.EnterArray();
            while(stream.Next != StreamOp.LeaveArray)
            {
                temp.Add(loader(stream));
            }
            stream.LeaveArray();
            return new ReadOnlyArray<T>(temp);
        }

        /// <summary>
        /// Saves a read only array to a value sink
        /// </summary>
        /// <param name="sink">The sink to save to</param>
        /// <param name="array">The read only array</param>
        public static void Save(IValueSink sink, ReadOnlyArray<T> array)
        {
            var saver = Value<T>.Saver;

            sink.EnterArray();
            for(int i = 0; i < array.Length; i++)
            {
                saver(sink, array[i]);
            }
            sink.LeaveArray();
        }

        /// <summary>
        /// The schema for array values
        /// </summary>
        public static readonly ISchema Schema = new ArraySchema(Value<T>.Schema);

        /// <summary>
        /// The underlying array
        /// </summary>
        private T[] _array;

        /// <summary>
        /// The length of the array
        /// </summary>
        public int Length { get { return _array == null ? 0 : _array.Length; } }

        /// <summary>
        /// Retrieves the <paramref name="index"/>th element in the array
        /// </summary>
        /// <param name="index">The index of the element to retrieve</param>
        /// <returns>The element</returns>
        public T this[int index]
        {
            get { return _array[index]; }
        }

        /// <summary>
        /// Constructs a new read only array
        /// </summary>
        /// <param name="array">The array to wrap</param>
        /// <param name="clone">True if the array must be cloned, false otherwise</param>
        public ReadOnlyArray(T[] array, bool clone = true)
        {
            if(array == null || !clone)
            {
                _array = array;
            }
            else
            {
                _array = new T[array.Length];
                Array.Copy(array, _array, array.Length);
            }
        }


        /// <summary>
        /// Constructs a new read only array
        /// </summary>
        /// <param name="clone">True if the array must be cloned, false otherwise</param>
        /// <param name="array">The array to wrap</param>
        public ReadOnlyArray(bool clone, params T[] array)
            : this(array, clone)
        { }

        /// <summary>
        /// Constructs a new read only array
        /// </summary>
        /// <param name="list">The list containing the array's elements</param>
        public ReadOnlyArray(List<T> list)
        {
            if (list.Count != 0)
            {
                _array = new T[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    _array[i] = list[i];
                }
            }
            else
                _array = null;
        }

        /// <summary>
        /// Constructs a new read only array from an enumerable series
        /// </summary>
        /// <param name="series">The series to enumerate</param>
        public ReadOnlyArray(IEnumerable<T> series)
        {
            this._array = series.ToArray();
        }

        public ReadOnlyArrayEnumerator GetEnumerator()
        {
            return new ReadOnlyArrayEnumerator(this);
        }
        
        public struct ReadOnlyArrayEnumerator : IEnumerator<T>
        {
            private ReadOnlyArray<T> _array;
            private int _i;

            public ReadOnlyArrayEnumerator(ReadOnlyArray<T> array)
            {
                this._array = array;
                this._i = -1;
            }

            public T Current
            {
                get
                {
                    return _array._array[_i];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return _array._array[_i];
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                _i++;
                return _i < _array.Length;
            }

            public void Reset()
            {
                _i = 0;
            }
        }

        #region IEnumerable Implementation

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (_array == null)
                return System.Linq.Enumerable.Empty<T>().GetEnumerator();
            return ((IEnumerable<T>)_array).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if(_array == null)
                return System.Linq.Enumerable.Empty<T>().GetEnumerator();
            return ((IEnumerable<T>)_array).GetEnumerator();
        }

        #endregion
    }
}
