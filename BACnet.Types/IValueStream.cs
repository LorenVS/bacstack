using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public interface IValueStream
    {
        /// <summary>
        /// The next operation that the stream expects
        /// </summary>
        StreamOp Next { get; }

        /// <summary>
        /// Retrieves a null value from the tream
        /// </summary>
        /// <returns>The value</returns>
        Null GetNull();

        /// <summary>
        /// Retrieves a boolean value from the stream
        /// </summary>
        /// <returns>The value</returns>
        bool GetBoolean();

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        byte GetUnsigned8();

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        ushort GetUnsigned16();

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        uint GetUnsigned32();

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        ulong GetUnsigned64();

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        sbyte GetSigned8();

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        short GetSigned16();

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        int GetSigned32();

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        long GetSigned64();

        /// <summary>
        /// Retrieves a float32 value from the stream
        /// </summary>
        /// <returns>The value</returns>
        float GetFloat32();

        /// <summary>
        /// Retrieves a float64 value from the stream
        /// </summary>
        /// <returns>The value</returns>
        double GetFloat64();

        /// <summary>
        /// Retrieves an octet stream value from the stream
        /// </summary>
        /// <returns>The value</returns>
        byte[] GetOctetString();

        /// <summary>
        /// Retrieves an char stream value from the stream
        /// </summary>
        /// <returns>The value</returns>
        string GetCharString();

        /// <summary>
        /// Retrieves an bit string value from the stream
        /// </summary>
        /// <returns>The value</returns>
        BitString8 GetBitString8();

        /// <summary>
        /// Retrieves an bit string value from the stream
        /// </summary>
        /// <returns>The value</returns>
        BitString24 GetBitString24();

        /// <summary>
        /// Retrieves an bit string value from the stream
        /// </summary>
        /// <returns>The value</returns>
        BitString56 GetBitString56();

        /// <summary>
        /// Retrieves an enumerated value from the stream
        /// </summary>
        /// <returns>The value</returns>
        uint GetEnumerated();

        /// <summary>
        /// Retrieves a date value from the stream
        /// </summary>
        /// <returns>The value</returns>
        Date GetDate();

        /// <summary>
        /// Retrieves a time value from the stream
        /// </summary>
        /// <returns>The value</returns>
        Time GetTime();

        /// <summary>
        /// Retrieves an object id value from the stream
        /// </summary>
        /// <returns>The value</returns>
        ObjectId GetObjectId();

        /// <summary>
        /// Retrieves a generic value from the stream
        /// </summary>
        /// <returns>The value</returns>
        GenericValue GetGeneric();

        /// <summary>
        /// Determines whether an option has a value
        /// </summary>
        /// <returns>True if the option has a value, false otherwise</returns>
        bool OptionHasValue();

        /// <summary>
        /// Enters a sequence value
        /// </summary>
        void EnterSequence();

        /// <summary>
        /// Leaves a sequence value
        /// </summary>
        void LeaveSequence();

        /// <summary>
        /// Enters a choice value
        /// </summary>
        /// <returns>The index of the active choice</returns>
        byte EnterChoice();

        /// <summary>
        /// Leaves a choice value
        /// </summary>
        void LeaveChoice();

        /// <summary>
        /// Enters an array value
        /// </summary>
        void EnterArray();

        /// <summary>
        /// Leaves an array value
        /// </summary>
        void LeaveArray();
    }
}
