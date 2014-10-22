using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Client.Descriptors
{
    public interface IDescriptorObserver<TValue, TKey> where TValue : ICloneable, ISyncable, IStronglyKeyed<TKey>
    {
        /// <summary>
        /// Signifies the end of the subscription
        /// </summary>
        void Close();

        /// <summary>
        /// Adds a single value to the subscription
        /// </summary>
        /// <param name="value">The value</param>
        void Add(TValue value);

        /// <summary>
        /// Adds multiple values to the subscription
        /// </summary>
        /// <param name="values">The values to add</param>
        void Add(IEnumerable<TValue> values);

        /// <summary>
        /// Removes a single value from the subscription
        /// </summary>
        /// <param name="value">The value to remove</param>
        void Remove(TValue value);

        /// <summary>
        /// Removes multiple value from the subscription
        /// </summary>
        /// <param name="values">The values to remove</param>
        void Remove(IEnumerable<TValue> values);

        /// <summary>
        /// Updates a single property
        /// </summary>
        /// <param name="value">The value to update</param>
        void Update(TValue value);

        /// <summary>
        /// Updates multiple values
        /// </summary>
        /// <param name="values">The values to update</param>
        void Update(IEnumerable<TValue> values);
    }
}
