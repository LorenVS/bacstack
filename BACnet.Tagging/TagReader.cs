using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Tagging
{
    public class TagReader
    {
        private Stream _stream;
        private BinaryReader _reader;
        private bool _hasHeader;
        private byte _tagNumber;
        private bool _isContext;
        private LVT _lvt;
        private int _length;
        private bool _value;
        private TagType _type;

        /// <summary>
        /// Constructs a new TagReader instance
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        public TagReader(Stream stream)
        {
            this._stream = stream;
            this._reader = new BinaryReader(_stream);
        }

        /// <summary>
        /// Reads the next header from the input stream
        /// if it hasn't already been read
        /// </summary>
        /// <returns>True if a header was read, false if the end of a stream was found</returns>
        private bool _readHeader()
        {
            try
            {
                byte header = _reader.ReadByte();
                _tagNumber = (byte)(header >> 4);
                _isContext = (header & 0x08) > 0;
                byte lvt = (byte)(header & 0x07);

                if (_tagNumber == 0x0F)
                    _tagNumber = _reader.ReadByte();

                if (!_isContext && _tagNumber == 0x01)
                {
                    // boolean app tag, LVT is a value
                    _lvt = LVT.Value;
                    _value = lvt == 1 ? true : false;
                }
                else if (_isContext && lvt == 0x06)
                {
                    // open tag, LVT is type
                    _lvt = LVT.Type;
                    _type = TagType.Open;
                }
                else if (_isContext && lvt == 0x07)
                {
                    // close tag, LVT is type
                    _lvt = LVT.Type;
                    _type = TagType.Close;
                }
                else
                {
                    // LVT is length
                    _lvt = LVT.Length;

                    if (lvt == 0x05)
                    {
                        _length = _reader.ReadByte();
                        if (_length == 254)
                        {
                            _length = _reader.ReadByte();
                            _length <<= 8;
                            _length |= _reader.ReadByte();
                        }
                        else if (_length == 255)
                        {
                            _length = _reader.ReadByte();
                            _length <<= 8;
                            _length |= _reader.ReadByte();
                            _length <<= 8;
                            _length = _reader.ReadByte();
                            _length <<= 8;
                            _length |= _reader.ReadByte();
                        }
                    }
                    else
                        _length = lvt;
                }

                _hasHeader = true;
                return true;
            }
            catch(EndOfStreamException)
            {
                return false;
            }
        }

        /// <summary>
        /// Reads the next header from the input stream
        /// </summary>
        private void _nextHeader()
        {
            if(!_hasHeader)
                _readHeader();
            _hasHeader = false;
        }

        /// <summary>
        /// Peeks at the next tag header, without consuming it
        /// </summary>
        /// <returns>True if a header was found, false if the end of stream was reached</returns>
        private bool _peekHeader()
        {
            return _hasHeader || _readHeader();
        }

        /// <summary>
        /// Ensures that the current tag meets an expected
        /// tag value
        /// </summary>
        /// <param name="tag">The expected tag number, or 255 for an application tag</param>
        /// <param name="defaultTag">The expected application tag if <paramref name="tag"/> is 255</param>
        private void _ensureTag(byte tag, ApplicationTag defaultTag)
        {
            bool context = (tag != 255);
            byte tagNumber = (tag == 255 ? (byte)defaultTag : tag);

            _nextHeader();
            if (_isContext != context || _tagNumber != tagNumber)
                throw new UnexpectedTagException();
        }

        /// <summary>
        /// Ensures that the current tag meets an expected
        /// tag value
        /// </summary>
        /// <param name="tag">The expected context tag number</param>
        private void _ensureTag(byte tag)
        {
            _nextHeader();
            if (!_isContext|| _tagNumber != tag)
                throw new UnexpectedTagException();
        }

        /// <summary>
        /// Ensures that the current tag lvt is an expected value
        /// </summary>
        /// <param name="lvt">The expected lvt value</param>
        private void _ensureLVT(LVT lvt)
        {
            if (_lvt != lvt)
                throw new InvalidTagException();
        }

        /// <summary>
        /// Ensures that the current tag length is an expected value
        /// </summary>
        /// <param name="length">The expected length value</param>
        private void _ensureLength(int length)
        {
            if (_lvt != LVT.Length)
                throw new InvalidTagException();
            if (_length != length)
                throw new InvalidTagException();
        }

        /// <summary>
        /// Ensures that the current tag length falls within
        /// an expected range
        /// </summary>
        /// <param name="min">The minimum expected length</param>
        /// <param name="max">The maximum expected length</param>
        private void _ensureLength(int min, int max)
        {
            if (_lvt != LVT.Length)
                throw new InvalidTagException();
            if (_length < min || _length > max)
                throw new InvalidTagException();
        }

        /// <summary>
        /// Ensures that the type of the read tag
        /// is an expected value
        /// </summary>
        /// <param name="type">The expected type</param>
        private void _ensureType(TagType type)
        {
            if (_lvt != LVT.Type)
                throw new InvalidTagException();
            else if (_type != type)
                throw new InvalidTagException();
        }

        /// <summary>
        /// Reads a null tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the null tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public Null ReadNull(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Null);
            _ensureLength(0);
            return new Null();
        }

        /// <summary>
        /// Reads a boolean tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the boolean tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public bool ReadBoolean(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Boolean);
            if(tag == 255)
            {
                // app encoded boolean, uses LVT.Value
                _ensureLVT(LVT.Value);
                return _value;
            }
            else
            {
                // context encoded boolean, length=1
                _ensureLength(1);
                return _reader.ReadByte() == 0x01 ? true : false;
            }
        }

        /// <summary>
        /// Reads an unsigned tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the unsigned tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public byte ReadUnsigned8(byte tag = 255)
        {
            return (byte)ReadUnsigned64(tag);
        }

        /// <summary>
        /// Reads an unsigned tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the unsigned tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public ushort ReadUnsigned16(byte tag = 255)
        {
            return (ushort)ReadUnsigned64(tag);
        }

        /// <summary>
        /// Reads an unsigned tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the unsigned tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public uint ReadUnsigned32(byte tag = 255)
        {
            return (uint)ReadUnsigned64(tag);
        }
        
        /// <summary>
        /// Reads an unsigned tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the unsigned tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public ulong ReadUnsigned64(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Unsigned);
            _ensureLength(1, 8);

            ulong value = _reader.ReadByte();
            for(int i = 1; i < _length; i++)
            {
                value <<= 8;
                value |= _reader.ReadByte();
            }

            return value;
        }

        /// <summary>
        /// Reads a signed tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the signed tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public sbyte ReadSigned8(byte tag = 255)
        {
            return (sbyte)ReadSigned64(tag);
        }
        
        /// <summary>
        /// Reads a signed tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the signed tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public short ReadSigned16(byte tag = 255)
        {
            return (short)ReadSigned64(tag);
        }

        /// <summary>
        /// Reads a signed tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the signed tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public int ReadSigned32(byte tag = 255)
        {
            return (int)ReadSigned64(tag);
        }

        /// <summary>
        /// Reads a signed tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the signed tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public long ReadSigned64(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Signed);
            _ensureLength(1, 8);

            long value = _reader.ReadSByte();
            for(int i = 1; i < _length; i++)
            {
                value <<= 8;
                value |= _reader.ReadByte();
            }
            return value;
        }

        /// <summary>
        /// Reads a float32 tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the float32 tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public float ReadFloat32(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Float32);
            _ensureLength(4);
            return new Union4(_reader.ReadUInt32()).ReverseLE().Float32;
        }

        /// <summary>
        /// Reads a float64 tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the float64 tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public double ReadFloat64(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Float64);
            _ensureLength(8);
            return new Union8(_reader.ReadUInt64()).ReverseLE().Float64;
        }

        /// <summary>
        /// Reads an octet string tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the octet string tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public byte[] ReadOctetString(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.OctetString);
            _ensureLVT(LVT.Length);
            return _reader.ReadBytes((int)_length);
        }

        /// <summary>
        /// Reads a char string tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the char string tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public string ReadCharString(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.CharString);
            _ensureLVT(LVT.Length);

            // we need at least one byte for the encoding
            if (_length < 1)
                throw new InvalidTagException();

            // we only support ANSI for now
            CharStringEncoding encoding = (CharStringEncoding)_reader.ReadByte();
            if (encoding != CharStringEncoding.ANSI)
                throw new InvalidTagException();

            var bytes = _reader.ReadBytes((int)_length - 1);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Reads a bitstring tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the bitstring tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public BitString8 ReadBitString8(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.BitString);
            _ensureLength(2, 8);

            byte unused = _reader.ReadByte();
            byte length = (byte)((_length - 1 ) * 8 - unused);

            ulong flags = _reader.ReadByte();
            for(int i = 1; i < _length - 1; i++)
            {
                flags <<= 8;
                flags |= _reader.ReadByte();
            }
            flags >>= (length - 2) * 8;

            return new BitString8(length, (byte)flags);
        }

        /// <summary>
        /// Reads a bitstring tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the bitstring tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public BitString24 ReadBitString24(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.BitString);
            _ensureLength(2, 8);

            byte unused = _reader.ReadByte();
            byte length = (byte)((_length - 1) * 8 - unused);

            ulong flags = _reader.ReadByte();
            for (int i = 1; i < _length - 1; i++)
            {
                flags <<= 8;
                flags |= _reader.ReadByte();
            }
            flags >>= (length - 4) * 8;

            return new BitString24(length, (uint)flags);
        }

        /// <summary>
        /// Reads a bitstring tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the bitstring tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public BitString56 ReadBitString56(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.BitString);
            _ensureLength(2, 8);

            byte unused = _reader.ReadByte();
            byte length = (byte)((_length - 1) * 8 - unused);

            ulong flags = _reader.ReadByte();
            for (int i = 1; i < _length - 2; i++)
            {
                flags <<= 8;
                flags |= _reader.ReadByte();
            }
            flags >>= (_length - 8) * 8;

            return new BitString56(length, flags);
        }

        /// <summary>
        /// Reads an enumerated tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the enumerated tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public uint ReadEnumerated(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Enumerated);
            _ensureLength(1, 4);

            uint value = _reader.ReadByte();
            for(int i = 1; i < _length; i++)
            {
                value <<= 8;
                value |= _reader.ReadByte();
            }

            return value;
        }

        /// <summary>
        /// Reads a date tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the date tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public Date ReadDate(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Date);
            _ensureLength(4);

            return new Date(
                _reader.ReadByte(),
                _reader.ReadByte(),
                _reader.ReadByte(),
                _reader.ReadByte());
        }

        /// <summary>
        /// Reads a time tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the time tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public Time ReadTime(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.Time);
            _ensureLength(4);

            return new Time(
                _reader.ReadByte(),
                _reader.ReadByte(),
                _reader.ReadByte(),
                _reader.ReadByte());
        }

        /// <summary>
        /// Reads an object id tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the object id tag, or 255 for an application tag</param>
        /// <returns>The read value</returns>
        public ObjectId ReadObjectId(byte tag = 255)
        {
            _ensureTag(tag, ApplicationTag.ObjectId);
            _ensureLength(4);

            uint val = new Union4(_reader.ReadUInt32()).ReverseLE().UInt32;
            ushort type = (ushort)(val >> 22);
            uint instance = (val & 0x003FFFFF);
            return new ObjectId(type, instance);
        }

        /// <summary>
        /// Reads a generic value from the stream
        /// </summary>
        /// <param name="tag">The tag number of the wrapping tags, or 255 for no wrapping tag</param>
        /// <returns>The read value</returns>
        public GenericValue ReadGeneric(byte tag = 255)
        {
            CopyingStream cstream = new CopyingStream(_stream);
            BinaryReader creader = new BinaryReader(cstream);
            Stream ostream = _stream;
            BinaryReader oreader = _reader;
            long end = 0;
            int depth = 0;

            ReadOpenTag(tag);

            if (_hasHeader)
                throw new Exception("Can't read generic tag when header is already cached");

            try
            {
                _stream = cstream;
                _reader = creader;

                while (depth >= 0)
                {
                    end = cstream.Position;
                    _peekHeader();

                    // TODO: EOF handling
                    
                    if (_lvt == LVT.Type)
                    {
                        if (depth == 0 && _type == TagType.Close && _tagNumber == tag)
                            break;
                        else if (_type == TagType.Open)
                            depth++;
                        else if (_type == TagType.Close)
                            depth--;
                    }
                    else if (_lvt == LVT.Length)
                    {
                        cstream.Skip(_length);
                    }

                    _hasHeader = false;
                }
            }
            finally
            {
                _stream = ostream;
                _reader = oreader;
            }

            ReadCloseTag(tag);

            return new TaggedGenericValue(cstream.MemoryStream.GetBuffer(), 0, (int)end);
        }

        /// <summary>
        /// Reads an open tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the open tag, or 255 for no open tag</param>
        public void ReadOpenTag(byte tag)
        {
            if (tag == 255)
                return;

            _ensureTag(tag);
            _ensureType(TagType.Open);
        }

        /// <summary>
        /// Reads a close tag from the stream
        /// </summary>
        /// <param name="tag">The tag number of the close tag, or 255 for no close tag</param>
        public void ReadCloseTag(byte tag)
        {
            if (tag == 255)
                return;

            _ensureTag(tag);
            _ensureType(TagType.Close);
        }
        
        /// <summary>
        /// Checks whether the next tag to be read matches
        /// a certain tag
        /// </summary>
        /// <param name="tag">The tag number to check for, or 255 for an application tag</param>
        /// <param name="defaultTag">The application tag to check for</param>
        /// <returns>True if the next tag matches, false otherwise</returns>
        public bool AtTag(byte tag, ApplicationTag defaultTag)
        {
            bool ret = false;
            bool hasHeader = _peekHeader();

            if(hasHeader)
            {
                bool context = (tag != 255);
                byte tagNumber = (context ? tag : (byte)defaultTag);
                return (_isContext == context && _tagNumber == tagNumber);
            }

            return ret;
        }

        /// <summary>
        /// Checks whether the next tag is an open tag
        /// with a certain tag number
        /// </summary>
        /// <param name="tag">The tag number to check for, or 255 for no tag</param>
        /// <returns>True if the next tag matches, false otherwise</returns>
        public bool AtOpenTag(byte tag)
        {
            // terminate if we are at the end of the stream, since
            // we can't possibly have another tag
            if (!_hasHeader && _reader.PeekChar() == -1)
                return false;
            else
            {
                _peekHeader();
                return (_isContext && _tagNumber == tag && _lvt == LVT.Type && _type == TagType.Open);
            }
        }

        /// <summary>
        /// Checks whether the next tag is a close tag
        /// with a certain tag number
        /// </summary>
        /// <param name="tag">The tag number to check for, or 255 for no tag</param>
        /// <returns>True if the next tag matches, false otherwise</returns>
        public bool AtCloseTag(byte tag)
        {
            // terminate if we are at the end of the stream, since
            // we can't possibly have another tag
            if (!_hasHeader && _reader.PeekChar() == -1)
                return false;
            else
            {
                _peekHeader();
                return (_isContext && _tagNumber == tag && _lvt == LVT.Type && _type == TagType.Close);
            }
        }


        /// <summary>
        /// Determines whether the reader is at the end of the stream
        /// </summary>
        /// <returns>True if the reader is at the end of the stream, false otherwise</returns>
        public bool EOF()
        {
            return !_hasHeader && !_peekHeader();
        }


        private class CopyingStream : Stream
        {
#region Invalid Operations

            public override bool CanRead
            {
                get
                {
                    return true;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return false;
                }
            }

            public override long Length
            {
                get
                {
                    throw new InvalidOperationException();
                }
            }
            
            public override void Flush()
            {
                throw new InvalidOperationException();
            }
            
            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new InvalidOperationException();
            }

            public override void SetLength(long value)
            {
                throw new InvalidOperationException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new InvalidOperationException();
            }

            #endregion

            private Stream _source;
            public MemoryStream MemoryStream { get; private set; }

            public CopyingStream(Stream source)
            {
                this.MemoryStream = new MemoryStream();
                this._source = source;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                count = _source.Read(buffer, offset, count);
                MemoryStream.Write(buffer, offset, count);
                return count;
            }

            public void Skip(int count)
            {
                MemoryStream.SetLength(MemoryStream.Length + count);
                var buffer = MemoryStream.GetBuffer();
                _source.Read(buffer, (int)MemoryStream.Position, count);
                MemoryStream.Position += count;
            }

            public override long Position
            {
                get
                {
                    return MemoryStream.Position;
                }

                set
                {
                    throw new InvalidOperationException();
                }
            }
        }

    }
}
 