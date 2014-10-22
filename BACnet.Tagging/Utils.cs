using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Tagging
{
    internal static class Utils
    {
        internal struct Tag
        {
            public byte ContextTag { get; private set; }
            public ApplicationTag ApplicationTag { get; private set; }

            public Tag(byte contextTag) : this()
            {
                this.ContextTag = contextTag;
            }

            public Tag(ApplicationTag applicationTag) : this()
            {
                this.ContextTag = 255;
                this.ApplicationTag = applicationTag;
            }
        }

        /// <summary>
        /// Retrieves the expected application tag for a schema type
        /// </summary>
        /// <param name="tag">The input context tag</param>
        /// <param name="schema">The schema</param>
        /// <returns>The context or app tag that is expected</returns>
        internal static Tag GetExpectedTag(byte tag, ISchema schema)
        {
            if (tag != 255)
                return new Tag(tag);

            switch(schema.Type)
            {
                case Types.ValueType.Null:
                    return new Tag(ApplicationTag.Null);
                case Types.ValueType.Boolean:
                    return new Tag(ApplicationTag.Boolean);
                case Types.ValueType.Unsigned8:
                case Types.ValueType.Unsigned16:
                case Types.ValueType.Unsigned32:
                case Types.ValueType.Unsigned64:
                    return new Tag(ApplicationTag.Unsigned);
                case Types.ValueType.Signed8:
                case Types.ValueType.Signed16:
                case Types.ValueType.Signed32:
                case Types.ValueType.Signed64:
                    return new Tag(ApplicationTag.Signed);
                case Types.ValueType.Float32:
                    return new Tag(ApplicationTag.Float32);
                case Types.ValueType.Float64:
                    return new Tag(ApplicationTag.Float64);
                case Types.ValueType.OctetString:
                    return new Tag(ApplicationTag.OctetString);
                case Types.ValueType.CharString:
                    return new Tag(ApplicationTag.CharString);
                case Types.ValueType.BitString8:
                case Types.ValueType.BitString24:
                case Types.ValueType.BitString56:
                    return new Tag(ApplicationTag.BitString);
                case Types.ValueType.Enumerated:
                    return new Tag(ApplicationTag.Enumerated);;
                case Types.ValueType.Date:
                    return new Tag(ApplicationTag.Date);
                case Types.ValueType.Time:
                    return new Tag(ApplicationTag.Time);
                case Types.ValueType.ObjectId:
                    return new Tag(ApplicationTag.ObjectId);
                case Types.ValueType.Sequence:
                    var field = ((SequenceSchema)schema).Fields[0];
                    return GetExpectedTag(field.Tag, field.Type);
                case Types.ValueType.Array:
                    return GetExpectedTag(255, ((ArraySchema)schema).ElementType);

                case Types.ValueType.Generic:
                case Types.ValueType.Option:
                case Types.ValueType.Choice:
                    throw new Exception("Can't get expected tag for generic, option or choice types");
            }

            throw new Exception("Can't get expected tag for unknown type");
        }
    }
}
