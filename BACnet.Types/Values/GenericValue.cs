using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public abstract class GenericValue : IValue
    {
        /// <summary>
        /// The value type for generic values
        /// </summary>
        public ValueType Type { get { return ValueType.Generic; } }

        /// <summary>
        /// The provider for this generic value
        /// </summary>
        public abstract IGenericValueProvider Provider { get; }

        /// <summary>
        /// Reinterprets a generic object as a strongly typed instance
        /// </summary>
        /// <typeparam name="T">The type to interpret as</typeparam>
        /// <returns>The interpreted value</returns>
        public T As<T>()
        {
            if (typeof(T) == typeof(GenericValue))
                return (T)(object)this;

            var schema = Value<T>.Schema;
            var stream = this.Provider.CreateStream(this, schema);
            return Value<T>.Loader(stream);
        }
    }
}
