using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types.Schemas
{
    public class PrimitiveSchema : ISchema
    {
        public static readonly PrimitiveSchema NullSchema = new PrimitiveSchema(ValueType.Null);
        public static readonly PrimitiveSchema BooleanSchema = new PrimitiveSchema(ValueType.Boolean);
        public static readonly PrimitiveSchema Unsigned8Schema = new PrimitiveSchema(ValueType.Unsigned8);
        public static readonly PrimitiveSchema Unsigned16Schema = new PrimitiveSchema(ValueType.Unsigned16);
        public static readonly PrimitiveSchema Unsigned32Schema = new PrimitiveSchema(ValueType.Unsigned32);
        public static readonly PrimitiveSchema Unsigned64Schema = new PrimitiveSchema(ValueType.Unsigned64);
        public static readonly PrimitiveSchema Signed8Schema = new PrimitiveSchema(ValueType.Signed8);
        public static readonly PrimitiveSchema Signed16Schema = new PrimitiveSchema(ValueType.Signed16);
        public static readonly PrimitiveSchema Signed32Schema = new PrimitiveSchema(ValueType.Signed32);
        public static readonly PrimitiveSchema Signed64Schema = new PrimitiveSchema(ValueType.Signed64);
        public static readonly PrimitiveSchema Float32Schema = new PrimitiveSchema(ValueType.Float32);
        public static readonly PrimitiveSchema Float64Schema = new PrimitiveSchema(ValueType.Float64);
        public static readonly PrimitiveSchema OctetStringSchema = new PrimitiveSchema(ValueType.OctetString);
        public static readonly PrimitiveSchema CharStringSchema = new PrimitiveSchema(ValueType.CharString);
        public static readonly PrimitiveSchema BitString8Schema = new PrimitiveSchema(ValueType.BitString8);
        public static readonly PrimitiveSchema BitString24Schema = new PrimitiveSchema(ValueType.BitString24);
        public static readonly PrimitiveSchema BitString56Schema = new PrimitiveSchema(ValueType.BitString56);
        public static readonly PrimitiveSchema EnumeratedSchema = new PrimitiveSchema(ValueType.Enumerated);
        public static readonly PrimitiveSchema DateSchema = new PrimitiveSchema(ValueType.Date);
        public static readonly PrimitiveSchema TimeSchema = new PrimitiveSchema(ValueType.Time);
        public static readonly PrimitiveSchema ObjectIdSchema = new PrimitiveSchema(ValueType.ObjectId);
        public static readonly PrimitiveSchema GenericSchema = new PrimitiveSchema(ValueType.Generic);
        public static readonly PrimitiveSchema EOF = new PrimitiveSchema(ValueType.EOF);


        /// <summary>
        /// The value type of the schema
        /// </summary>
        public ValueType Type { get; private set; }

        /// <summary>
        /// Constructs a new PrimitiveSchema instance
        /// </summary>
        /// <param name="type">The value type of the schema</param>
        private PrimitiveSchema(ValueType type)
        {
            this.Type = type;
        }
    }
}
