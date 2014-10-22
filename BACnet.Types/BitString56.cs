using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types.Schemas;

namespace BACnet.Types
{
    public struct BitString56
    {
        /// <summary>
        /// The schema for bitstring56 values
        /// </summary>
        public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

        /// <summary>
        /// Loads a bitstring56 value from a stream
        /// </summary>
        /// <param name="stream">The stream to load from</param>
        /// <returns></returns>
        public static BitString56 Load(IValueStream stream)
        {
            return stream.GetBitString56();
        }

        /// <summary>
        /// Saves a bitstring56 value to a stream
        /// </summary>
        /// <param name="sink">The sink to save to</param>
        /// <param name="value">The value to save</param>
        public static void Save(IValueSink sink, BitString56 value)
        {
            sink.PutBitString56(value);
        }

        private byte _length;
        private ulong _flags;

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
                var mask = (ulong)(0x8000000000000000 >> index);
                return (_flags & mask) > 0;
            }
        }

        /// <summary>
        /// Constructs a new bitstring16 instance
        /// </summary>
        /// <param name="length">The length of the bitstring</param>
        /// <param name="flags">The flag values</param>
        public BitString56(byte length, ulong flags)
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
        public BitString56 WithLength(byte length)
        {
            return new BitString56(length, _flags);
        }


        /// <summary>
        /// Creates a copy of this bitstring with
        /// a certain bit overriden
        /// </summary>
        /// <param name="index">The bit to override</param>
        /// <param name="set">The value to override to</param>
        /// <returns>The new bitstring</returns>
        public BitString56 WithBit(int index, bool set = true)
        {
            var mask = (ulong)(0x8000000000000000 >> index);
            ulong flags = _flags;
            if (set)
                flags |= mask;
            else
                flags &= (ulong)~mask;
            return new BitString56(_length, flags);
        }
    }
}
