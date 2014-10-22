using System;
using System.Reflection;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    public static class Value<T>
    {
        /// <summary>
        /// Function that loads a value of type T
        /// from a value stream
        /// </summary>
        private static readonly Func<IValueStream, T> _loader;

        public static Func<IValueStream, T> Loader { get { return _loader; } }

        /// <summary>
        /// Loads a value of type T
        /// from a value stream
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        public static T Load(IValueStream stream)
        {
            return _loader(stream);
        }

        /// <summary>
        /// Function that saves a value of type T
        /// to a value sink
        /// </summary>
        private static readonly Action<IValueSink, T> _saver;
        public static Action<IValueSink, T> Saver { get { return _saver; } }

        /// <summary>
        /// Saves a value of type T to a value sink
        /// </summary>
        /// <param name="sink">The sink to save to</param>
        /// <param name="value">The value to save</param>
        public static void Save(IValueSink sink, T value)
        {
            _saver(sink, value);
        }

        /// <summary>
        /// The schema for values of type Ts
        /// </summary>
        private static readonly ISchema _schema;

        /// <summary>
        /// The schema for values of type T
        /// </summary>
        public static ISchema Schema { get { return _schema; } }

        /// <summary>
        /// Performs necessary casting to convert a function
        /// to the loader type
        /// </summary>
        /// <typeparam name="T2">Temporary type variable</typeparam>
        /// <param name="func">The function to convert</param>
        /// <returns>The converted function</returns>
        private static Func<IValueStream, T> getLoader<T2>(Func<IValueStream, T2> func)
        {
            return (Func<IValueStream, T>)(object)func;
        }

        /// <summary>
        /// Performs the necessary casting to convert a function
        /// to the saver type
        /// </summary>
        /// <typeparam name="T2">Temporary type variable</typeparam>
        /// <param name="func">The function to convert</param>
        /// <returns>The converted function</returns>
        private static Action<IValueSink, T> getSaver<T2>(Action<IValueSink, T2> func)
        {
            return (Action<IValueSink, T>)(object)func;
        }


        /// <summary>
        /// Static constructor which initializes
        /// the _loader and _saver functions
        /// </summary>
        static Value()
        {

            if (typeof(T) == typeof(Null))
            {
                _loader = getLoader(s => s.GetNull());
                _saver = getSaver<Null>((s, v) => s.PutNull(v));
                _schema = PrimitiveSchema.NullSchema;
            }
            else if (typeof(T) == typeof(bool))
            {
                _loader = getLoader(s => s.GetBoolean());
                _saver = getSaver<bool>((s, v) => s.PutBoolean(v));
                _schema = PrimitiveSchema.BooleanSchema;
            }
            else if (typeof(T) == typeof(byte))
            {
                _loader = getLoader(s => s.GetUnsigned8());
                _saver = getSaver<byte>((s, v) => s.PutUnsigned8(v));
                _schema = PrimitiveSchema.Unsigned8Schema;
            }
            else if (typeof(T) == typeof(ushort))
            {
                _loader = getLoader(s => s.GetUnsigned16());
                _saver = getSaver<ushort>((s, v) => s.PutUnsigned16(v));
                _schema = PrimitiveSchema.Unsigned16Schema;
            }
            else if (typeof(T) == typeof(uint))
            {
                _loader = getLoader(s => s.GetUnsigned32());
                _saver = getSaver<uint>((s, v) => s.PutUnsigned32(v));
                _schema = PrimitiveSchema.Unsigned32Schema;
            }
            else if (typeof(T) == typeof(ulong))
            {
                _loader = getLoader(s => s.GetUnsigned64());
                _saver = getSaver<ulong>((s, v) => s.PutUnsigned64(v));
                _schema = PrimitiveSchema.Unsigned64Schema;
            }
            else if (typeof(T) == typeof(sbyte))
            {
                _loader = getLoader(s => s.GetSigned8());
                _saver = getSaver<sbyte>((s, v) => s.PutSigned8(v));
                _schema = PrimitiveSchema.Signed8Schema;
            }
            else if (typeof(T) == typeof(short))
            {
                _loader = getLoader(s => s.GetSigned16());
                _saver = getSaver<short>((s, v) => s.PutSigned16(v));
                _schema = PrimitiveSchema.Signed16Schema;
            }
            else if (typeof(T) == typeof(int))
            {
                _loader = getLoader(s => s.GetSigned32());
                _saver = getSaver<int>((s, v) => s.PutSigned32(v));
                _schema = PrimitiveSchema.Signed32Schema;
            }
            else if (typeof(T) == typeof(long))
            {
                _loader = getLoader(s => s.GetSigned64());
                _saver = getSaver<long>((s, v) => s.PutSigned64(v));
                _schema = PrimitiveSchema.Signed64Schema;
            }
            else if (typeof(T) == typeof(float))
            {
                _loader = getLoader(s => s.GetFloat32());
                _saver = getSaver<float>((s, v) => s.PutFloat32(v));
                _schema = PrimitiveSchema.Float32Schema;
            }
            else if (typeof(T) == typeof(double))
            {
                _loader = getLoader(s => s.GetFloat64());
                _saver = getSaver<double>((s, v) => s.PutFloat64(v));
                _schema = PrimitiveSchema.Float64Schema;
            }
            else if (typeof(T) == typeof(byte[]))
            {
                _loader = getLoader(s => s.GetOctetString());
                _saver = getSaver<byte[]>((s, v) => s.PutOctetString(v));
                _schema = PrimitiveSchema.OctetStringSchema;
            }
            else if (typeof(T) == typeof(string))
            {
                _loader = getLoader(s => s.GetCharString());
                _saver = getSaver<string>((s, v) => s.PutCharString(v));
                _schema = PrimitiveSchema.CharStringSchema;
            }
            else if (typeof(T) == typeof(BitString8))
            {
                _loader = getLoader(s => s.GetBitString8());
                _saver = getSaver<BitString8>((s, v) => s.PutBitString8(v));
                _schema = PrimitiveSchema.BitString8Schema;
            }
            else if (typeof(T) == typeof(BitString24))
            {
                _loader = getLoader(s => s.GetBitString24());
                _saver = getSaver<BitString24>((s, v) => s.PutBitString24(v));
                _schema = PrimitiveSchema.BitString24Schema;
            }
            else if (typeof(T) == typeof(BitString56))
            {
                _loader = getLoader(s => s.GetBitString56());
                _saver = getSaver<BitString56>((s, v) => s.PutBitString56(v));
                _schema = PrimitiveSchema.BitString56Schema;
            }
            else if (typeof(T) == typeof(Enumerated))
            {
                _loader = getLoader(s => (Enumerated)s.GetEnumerated());
                _saver = getSaver<Enumerated>((s, v) => s.PutEnumerated(v));
                _schema = PrimitiveSchema.EnumeratedSchema;
            }
            else if (typeof(T) == typeof(Date))
            {
                _loader = getLoader(s => s.GetDate());
                _saver = getSaver<Date>((s, v) => s.PutDate(v));
                _schema = PrimitiveSchema.DateSchema;
            }
            else if (typeof(T) == typeof(Time))
            {
                _loader = getLoader(s => s.GetTime());
                _saver = getSaver<Time>((s, v) => s.PutTime(v));
                _schema = PrimitiveSchema.TimeSchema;
            }
            else if (typeof(T) == typeof(ObjectId))
            {
                _loader = getLoader(s => s.GetObjectId());
                _saver = getSaver<ObjectId>((s, v) => s.PutObjectId(v));
                _schema = PrimitiveSchema.ObjectIdSchema;
            }
            else if(typeof(T) == typeof(GenericValue))
            {
                _loader = getLoader(s => s.GetGeneric());
                _saver = getSaver<GenericValue>((s, v) => s.PutGeneric(v));
                _schema = PrimitiveSchema.GenericSchema;
            }
            else if(typeof(T).IsEnum)
            {
                _loader = (stream) => {
                    return (T)(object)stream.GetEnumerated();
                };

                _saver = (sink, value) => {
                    sink.PutEnumerated((uint)(object)value);
                };

                _schema = PrimitiveSchema.EnumeratedSchema;
            }
            else
            {
                var loaderMethod = typeof(T).GetMethod("Load", BindingFlags.Public | BindingFlags.Static);
                var saverMethod = typeof(T).GetMethod("Save", BindingFlags.Public | BindingFlags.Static);
                var schemaField = typeof(T).GetField("Schema", BindingFlags.Public | BindingFlags.Static);
                _loader = (Func<IValueStream, T>)loaderMethod.CreateDelegate(typeof(Func<IValueStream, T>));
                _saver = (Action<IValueSink, T>)saverMethod.CreateDelegate(typeof(Action<IValueSink, T>));
                _schema = (ISchema)schemaField.GetValue(null);
            }
        }
    }
}
