using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Tagging
{
    public class TaggedGenericValue : GenericValue
    {
        /// <summary>
        /// An empty tagged generic value instance
        /// </summary>
        public static readonly TaggedGenericValue Empty = new TaggedGenericValue(new byte[] { }, 0, 0);

        /// <summary>
        /// Encodes a value as a tagged generic value
        /// </summary>
        /// <typeparam name="T">The type of value to encode</typeparam>
        /// <param name="value">The value to encode</param>
        /// <returns>The generic value</returns>
        public static GenericValue Encode<T>(T value)
        {
            byte[] tag = null;

            using(var ms = new MemoryStream())
            {
                TagWriter writer = new TagWriter(ms);
                TagWriterSink sink = new TagWriterSink(writer, Value<T>.Schema);
                Value<T>.Save(sink, value);
                tag = ms.ToArray();
            }

            return new TaggedGenericValue(tag, 0, tag.Length);
        }

        /// <summary>
        /// The value provider for tagged generic values
        /// </summary>
        public override IGenericValueProvider Provider { get { return TaggedValueProvider.Instance; } }

        /// <summary>
        /// The tagged value
        /// </summary>
        internal byte[] tag;

        /// <summary>
        /// The start of the tag content in the buffer
        /// </summary>
        internal int offset;

        /// <summary>
        /// The end of the tag content in the buffer
        /// </summary>
        internal int end;

        /// <summary>
        /// Constructs a new TaggedGenericValue instance
        /// </summary>
        /// <param name="tag">The tagged value</param>
        internal TaggedGenericValue(byte[] tag, int offset, int end)
        {
            this.tag = tag;
            this.offset = offset;
            this.end = end;
        }
    }
}
