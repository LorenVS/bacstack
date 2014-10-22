using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Tagging
{
    public class TagWriter
    {
        private Stream _stream;
        private BinaryWriter _writer;

        /// <summary>
        /// Constructs a new TagWriter instance
        /// </summary>
        /// <param name="stream">The stream to write tags to</param>
        public TagWriter(Stream stream)
        {
            this._stream = stream;
            this._writer = new BinaryWriter(_stream);
        }

        /// <summary>
        /// Writes a tag header to the stream
        /// </summary>
        /// <param name="tagNumber">The tag number of the header, or 255 for an application tag</param>
        /// <param name="defaultTag">The default application tag if the tag number is 255</param>
        /// <param name="lvt">The 3-bit Length/Value/Type value</param>
        private void _writeHeader(byte tagNumber, ApplicationTag defaultTag, byte lvt)
        {
            bool context = (tagNumber != 255);
            byte tag = (context ? tagNumber : (byte)defaultTag);

            byte header = 0x00;

            if (tag >= 0x0F)
                header |= 0xF0;
            else
                header |= (byte)(tag << 4);
            
            if (context)
                header |= 0x08;
            header |= lvt;

            _writer.Write(header);
            if (tag >= 0x0F)
                _writer.Write(tag);
        }

        /// <summary>
        /// Writes a tag header with a value LVT
        /// </summary>
        /// <param name="tagNumber">The tag number of the header, or 255 for an application tag</param>
        /// <param name="defaultTag">The default application tag if the tag number is 255</param>
        /// <param name="value">The boolean value LVT</param>
        private void _writeValueHeader(byte tagNumber, ApplicationTag defaultTag, bool value)
        {
            _writeHeader(tagNumber, defaultTag, (byte)(value ? 0x01 : 0x00));
        }

        /// <summary>
        /// Writes a tag header with a length LVT
        /// </summary>
        /// <param name="tagNumber">The tag number of the header, or 255 for an application tag</param>
        /// <param name="defaultTag">The default application tag if the tag number is 255</param>
        /// <param name="length">The length LVT of the tag</param>
        private void _writeLengthHeader(byte tagNumber, ApplicationTag defaultTag, int length)
        {
            _writeHeader(tagNumber, defaultTag, (byte)(length >= 0x05 ? 0x05 : length));
            if(length >= ushort.MaxValue)
            {
                _writer.Write((byte)0xFF);
                _writer.Write(new Union4((uint)length).ReverseLE().UInt32);
            }
            else if(length >= 254)
            {
                _writer.Write((byte)0xFE);
                _writer.Write(new Union2((ushort)length).ReverseLE().UInt16);
            }
            else if(length >= 5)
            {
                _writer.Write((byte)length);
            }
        }

        /// <summary>
        /// Writes a tag header with a type LVT
        /// </summary>
        /// <param name="tagNumber">The tag number of the header, or 255 for no tag</param>
        /// <param name="type">The type LVT (open/close)</param>
        private void _writeTypeHeader(byte tagNumber, TagType type)
        {
            if (tagNumber == 255)
                return;
            _writeHeader(tagNumber, ApplicationTag.Null, (byte)(type == TagType.Open ? 0x06 : 0x07));
        }

        /// <summary>
        /// Writes a null tag to the stream
        /// </summary>
        /// <param name="value">The null value</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteNull(Null value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.Null, 0);
        }

        /// <summary>
        /// Writes a boolean tag to the stream
        /// </summary>
        /// <param name="value">The boolean value</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteBoolean(bool value, byte tag = 255)
        {
            if(tag == 255)
            {
                _writeValueHeader(tag, ApplicationTag.Boolean, value);
            }
            else
            {
                _writeLengthHeader(tag, ApplicationTag.Boolean, 1);
                _writer.Write((byte)(value ? 0x01 : 0x00));
            }
        }

        /// <summary>
        /// Writes an unsigned tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteUnsigned8(byte value, byte tag = 255)
        {
            _writeLengthHeader(value, ApplicationTag.Unsigned, 1);
            _writer.Write(value);
        }

        /// <summary>
        /// Writes an unsigned tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteUnsigned16(ushort value, byte tag = 255)
        {
            WriteUnsigned64(value, tag);
        }

        /// <summary>
        /// Writes an unsigned tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteUnsigned32(uint value, byte tag = 255)
        {
            WriteUnsigned64(value, tag);
        }

        /// <summary>
        /// Writes an unsigned tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteUnsigned64(ulong value, byte tag = 255)
        {
            int length;

            if (value < (1ul << 8))
                length = 1;
            else if (value < (1ul << 16))
                length = 2;
            else if (value < (1ul << 24))
                length = 3;
            else if (value < (1ul << 32))
                length = 4;
            else if (value < (1ul << 40))
                length = 5;
            else if (value < (1ul << 48))
                length = 6;
            else if (value < (1ul << 56))
                length = 7;
            else
                length = 8;

            _writeLengthHeader(tag, ApplicationTag.Unsigned, length);
            value = new Union8(value << (64 - (length * 8))).Reverse().UInt64;

            for(int i = 0; i < length; i++)
            {
                _writer.Write((byte)value);
                value >>= 8;
            }
        }

        /// <summary>
        /// Writes a signed tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteSigned8(sbyte value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.Signed, 1);
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a signed tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteSigned16(short value, byte tag = 255)
        {
            WriteSigned64(value, tag);
        }

        /// <summary>
        /// Writes a signed tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteSigned32(int value, byte tag = 255)
        {
            WriteSigned64(value, tag);
        }

        /// <summary>
        /// Writes a signed tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteSigned64(long value, byte tag = 255)
        {
            int length;

            if (value < (1L << 7) && value >= (-1L << 7))
                length = 1;
            else if (value < (1L << 15) && value >= (-1L << 15))
                length = 2;
            else if (value < (1L << 23) && value >= (-1L << 23))
                length = 3;
            else if (value < (1L << 31) && value >= (-1L << 31))
                length = 4;
            else if (value < (1L << 39) && value >= (-1L << 39))
                length = 5;
            else if (value < (1L << 47) && value >= (-1L << 47))
                length = 6;
            else if (value < (1L << 55) && value >= (-1L << 55))
                length = 7;
            else
                length = 8;

            _writeLengthHeader(tag, ApplicationTag.Signed, length);
            var bytes = new Union8(value).ReverseLE();
            switch (length)
            {
                case 8: _writer.Write(bytes.Byte0); goto case 7;
                case 7: _writer.Write(bytes.Byte1); goto case 6;
                case 6: _writer.Write(bytes.Byte2); goto case 5;
                case 5: _writer.Write(bytes.Byte3); goto case 4;
                case 4: _writer.Write(bytes.Byte4); goto case 3;
                case 3: _writer.Write(bytes.Byte5); goto case 2;
                case 2: _writer.Write(bytes.Byte6); goto case 1;
                case 1: _writer.Write(bytes.Byte7); break;
            }
        }


        /// <summary>
        /// Writes a float32 tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteFloat32(float value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.Float32, 4);
            _writer.Write(new Union4(value).ReverseLE().UInt32);
        }

        /// <summary>
        /// Writes a float64 tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteFloat64(double value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.Float64, 8);
            _writer.Write(new Union8(value).ReverseLE().UInt64);
        }

        /// <summary>
        /// Writes an octet string tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteOctetString(byte[] value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.OctetString, value.Length);
            _writer.Write(value, 0, value.Length);
        }

        /// <summary>
        /// Writes a char string tag to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteCharString(string value, byte tag = 255)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            _writeLengthHeader(tag, ApplicationTag.CharString, bytes.Length + 1);
            _writer.Write((byte)0);
            _writer.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes a bitstring value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteBitString8(BitString8 value, byte tag = 255)
        {
            // TODO: implement
        }

        /// <summary>
        /// Writes a bitstring value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteBitString24(BitString24 value, byte tag = 255)
        {
            // TODO: implement
        }

        /// <summary>
        /// Writes a bitstring value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteBitString56(BitString56 value, byte tag = 255)
        {
            // TODO: implement
        }

        /// <summary>
        /// Writes an enumerated value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 fo an application tag</param>
        public void WriteEnumerated(uint value, byte tag = 255)
        {
            int length = 0;

            if (value < (1 << 8))
                length = 1;
            else if (value < (1 << 16))
                length = 2;
            else if (value < (1 << 24))
                length = 3;
            else
                length = 4;

            _writeLengthHeader(tag, ApplicationTag.Enumerated, length);
            for(int i = 0; i < length; i++)
            {
                _writer.Write((byte)value);
                value >>= 8;
            }
        }

        /// <summary>
        /// Writes a date value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteDate(Date value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.Date, 4);
            _writer.Write(value.Year);
            _writer.Write(value.Month);
            _writer.Write(value.Day);
            _writer.Write(value.DayOfWeek);
        }
        
        /// <summary>
        /// Writes a time value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteTime(Time value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.Time, 4);
            _writer.Write(value.Hour);
            _writer.Write(value.Minute);
            _writer.Write(value.Second);
            _writer.Write(value.Hundredths);
        }

        /// <summary>
        /// Writes an object id value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The tag number, or 255 for an application tag</param>
        public void WriteObjectId(ObjectId value, byte tag = 255)
        {
            _writeLengthHeader(tag, ApplicationTag.ObjectId, 4);
            uint val = (uint)(value.Type << 22);
            val |= value.Instance;
            _writer.Write(new Union4(val).ReverseLE().UInt32);
        }

        /// <summary>
        /// Writes a generic value to the stream
        /// </summary>
        /// <param name="value">The value to write</param>
        /// <param name="tag">The wrapper tag number, or 255 for no wrapper tag</param>
        public void WriteGeneric(GenericValue value, byte tag = 255)
        {
            var tvalue = value as TaggedGenericValue;
            if (tvalue == null)
                throw new Exception("Can only write TaggedGenericValue instances");

            _writeTypeHeader(tag, TagType.Open);
            _writer.Write(tvalue.tag, 0, tvalue.tag.Length);
            _writeTypeHeader(tag, TagType.Close);
        }

        /// <summary>
        /// Writes an open tag to the stream
        /// </summary>
        /// <param name="tag">The context tag number of the tag, or 255 for no tag</param>
        public void WriteOpenTag(byte tag)
        {
            _writeTypeHeader(tag, TagType.Open);
        }

        /// <summary>
        /// Writes a close tag to the stream
        /// </summary>
        /// <param name="tag">The context tag number of the tag, or 255 for no tag</param>
        public void WriteCloseTag(byte tag)
        {
            _writeTypeHeader(tag, TagType.Close);
        }

    }
}
