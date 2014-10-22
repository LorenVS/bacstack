using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public interface IValueSink
    {
        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutNull(Null value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutBoolean(bool value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutUnsigned8(byte value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutUnsigned16(ushort value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutUnsigned32(uint value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutUnsigned64(ulong value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutSigned8(sbyte value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutSigned16(short value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutSigned32(int value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutSigned64(long value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutFloat32(float value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutFloat64(double value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutOctetString(byte[] value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutCharString(string value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutBitString8(BitString8 value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutBitString24(BitString24 value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutBitString56(BitString56 value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutEnumerated(Enumerated value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutDate(Date value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutTime(Time value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutObjectId(ObjectId value);

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value</param>
        void PutGeneric(GenericValue value);

        /// <summary>
        /// Optionally enters a value
        /// </summary>
        /// <param name="hasValue">Whether the option has a value</param>
        void EnterOption(bool hasValue);

        /// <summary>
        /// Puts the start of a sequence value
        /// </summary>
        void EnterSequence();

        /// <summary>
        /// Puts the end of a sequence value
        /// </summary>
        void LeaveSequence();
        
        /// <summary>
        /// Puts the start of a choice value
        /// </summary>
        /// <param name="choice">The active choice</param>
        void EnterChoice(byte choice);

        /// <summary>
        /// Puts the end of a choice value
        /// </summary>
        void LeaveChoice();

        /// <summary>
        /// Puts the start of an array value
        /// </summary>
        void EnterArray();

        /// <summary>
        /// Puts the end of an array value
        /// </summary>
        void LeaveArray();
    }
}
