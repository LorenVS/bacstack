using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    public struct BitString24
    {
        /// <summary>
        /// The schema for bitstring24 values
        /// </summary>
        public static readonly ISchema Schema = PrimitiveSchema.BitString24Schema;

        /// <summary>
        /// Loads a bitstring24 value from a stream
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        /// <returns>The bitstring24 value</returns>
        public static BitString24 Load(IValueStream stream)
        {
            return stream.GetBitString24();
        }

        /// <summary>
        /// Saves a bitstring24 value to a sink
        /// </summary>
        /// <param name="sink">The sink to save to</param>
        /// <param name="value">The value to save</param>
        public static void Save(IValueSink sink, BitString24 value)
        {
            sink.PutBitString24(value);
        }

        private byte _length;
        private uint _flags;

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
                var mask = (uint)(0x80000000 >> index);
                return (_flags & mask) > 0;
            }
        }

        /// <summary>
        /// Constructs a new bitstring16 instance
        /// </summary>
        /// <param name="length">The length of the bitstring</param>
        /// <param name="flags">The flag values</param>
        public BitString24(byte length, uint flags)
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
        public BitString24 WithLength(byte length)
        {
            return new BitString24(length, _flags);
        }


        /// <summary>
        /// Creates a copy of this bitstring with
        /// a certain bit overriden
        /// </summary>
        /// <param name="index">The bit to override</param>
        /// <param name="set">The value to override to</param>
        /// <returns>The new bitstring</returns>
        public BitString24 WithBit(int index, bool set = true)
        {
            var mask = (ushort)(0x80000000 >> index);
            uint flags = _flags;
            if (set)
                flags |= mask;
            else
                flags &= (uint)~mask;
            return new BitString24(_length, flags);
        }
    }
}
