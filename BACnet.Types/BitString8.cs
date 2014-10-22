using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    public struct BitString8
    {
        /// <summary>
        /// The schema for bitstring 8 values
        /// </summary>
        public static readonly ISchema Schema = PrimitiveSchema.BitString8Schema;

        /// <summary>
        /// Loads a bitstring8 value from the stream
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        /// <returns>The bitstring 8 value</returns>
        public static BitString8 Load(IValueStream stream)
        {
            return stream.GetBitString8();
        }

        /// <summary>
        /// Saves a bitstring8 value a sink
        /// </summary>
        /// <param name="sink">The sink to save to</param>
        /// <param name="value">The value to save</param>
        public static void Save(IValueSink sink, BitString8 value)
        {
            sink.PutBitString8(value);
        }

        private byte _length;
        private byte _flags;

        /// <summary>
        /// The length of the bitstring
        /// </summary>
        public byte Length { get { return _length; } }

        /// <summary>
        /// Retrieves the value of the <paramref name="index"/>th bit
        /// </summary>
        /// <param name="index">The index of the bit to retrieve</param>
        /// <returns>True if the bit is set, false otherwise</returns>
        public bool this[int index]
        {
            get
            {
                var mask = (byte)(0x80 >> index);
                return (_flags & mask) > 0;
            }
        }

        /// <summary>
        /// Constructs a new bitstring8 instance
        /// </summary>
        /// <param name="length">The length of the bitstring</param>
        /// <param name="flags">The flag values</param>
        public BitString8(byte length, byte flags)
        {
            this._length = length;
            this._flags = flags;
        }

        /// <summary>
        /// Creates a copy of this bitstring with the
        /// supplied length
        /// </summary>
        /// <param name="length">The length of the new bitstring</param>
        /// <returns>The new bitstring instance</returns>
        public BitString8 WithLength(byte length)
        {
            return new BitString8(length, _flags);
        }

        /// <summary>
        /// Creates a copy of this bitstring with
        /// a certain bit overriden
        /// </summary>
        /// <param name="index">The bit to override</param>
        /// <param name="set">The value to override to</param>
        /// <returns>The new bitstring</returns>
        public BitString8 WithBit(int index, bool set = true)
        {
            var mask = (byte)(0x80 >> index);
            byte flags = _flags;
            if (set)
                flags |= mask;
            else
                flags &= (byte)~mask;
            return new BitString8(_length, flags);
        }
    }
}
