using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Tagging
{
    internal class TaggedValueProvider : IGenericValueProvider
    {
        /// <summary>
        /// The singleton instance of TaggedValueProvider
        /// </summary>
        internal static readonly TaggedValueProvider Instance = new TaggedValueProvider();

        /// <summary>
        /// Attempts to interpret a TaggedGenericType instance as
        /// a specific type
        /// </summary>
        /// <param name="value">The value to interpet</param>
        /// <param name="schema">The type to interpret as</param>
        /// <param name="output">The output type</param>
        /// <returns>True if the interpretation succeeds, false otherwise</returns>
        public IValueStream CreateStream(GenericValue value, ISchema schema)
        {
            var tagged = (TaggedGenericValue)value;
            MemoryStream ms = new MemoryStream(tagged.tag, tagged.offset, tagged.end - tagged.offset);
            TagReader reader = new TagReader(ms);
            TagReaderStream stream = new TagReaderStream(reader, schema);
            return stream;
        }
    }
}
