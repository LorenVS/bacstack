using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    public struct Option<T>
    {
        /// <summary>
        /// Loads an option value from a stream
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        /// <returns>The loaded option value</returns>
        public static Option<T> Load(IValueStream stream)
        {
            bool hasValue = stream.OptionHasValue();
            if (hasValue)
                return new Option<T>(Value<T>.Load(stream));
            else
                return new Option<T>();
        }

        /// <summary>
        /// Saves an option value to a sink
        /// </summary>
        /// <param name="sink">The sink to save to</param>
        /// <param name="option">The option to save</param>
        public static void Save(IValueSink sink, Option<T> option)
        {
            sink.EnterOption(option.HasValue);
            if (option.HasValue)
                Value<T>.Save(sink, option.Value);
        }

        /// <summary>
        /// The schema for optional values
        /// </summary>
        public static readonly ISchema Schema = new OptionSchema(Value<T>.Schema);

        /// <summary>
        /// Shared global none instance
        /// </summary>
        public static readonly Option<T> None = new Option<T>();

        /// <summary>
        /// True if the option has a value
        /// </summary>
        public bool HasValue { get; private set; }
        
        /// <summary>
        /// The value
        /// </summary>
        public T Value { get; private set; }

        public Option(T value) : this() { HasValue = true; Value = value; }

        public static implicit operator Option<T>(T value)
        {
            return new Types.Option<T>(value);
        }

        /// <summary>
        /// Returns a string representation of this object
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return HasValue ? Value.ToString() : string.Empty;
        }
    }
}
