using System;
using System.Collections.Generic;
using System.IO;

namespace BACnet.Core
{
    public class MultiBufferStream : Stream
    {
        /// <summary>
        /// The list of buffers
        /// </summary>
        private List<BufferSegment> _buffers;

        /// <summary>
        /// The index of the current buffer
        /// </summary>
        private int _bufferIndex;

        /// <summary>
        /// The current buffer;
        /// </summary>
        private BufferSegment _buffer;

        /// <summary>
        /// The offset within the current buffer
        /// </summary>
        private int _offset;

        /// <summary>
        /// Constructs a new multi buffer stream instance
        /// </summary>
        /// <param name="buffers"></param>
        public MultiBufferStream(List<BufferSegment> buffers)
        {
            _buffers = buffers;
            _bufferIndex = 0;
            if (_buffers.Count > 0)
            {
                _buffer = _buffers[0];
                _offset = _buffer.Offset;
            }
        }

        /// <summary>
        /// Advances to the next buffer
        /// </summary>
        /// <returns>True if another buffer is available, false otherwise</returns>
        private bool _nextBuffer()
        {
            bool ret = false;
            _bufferIndex++;

            if(_bufferIndex < _buffers.Count)
            {
                _buffer = _buffers[_bufferIndex];
                _offset = _buffer.Offset;
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// Whther or not this stream can be read from
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Whether or not this stream can be seeked
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                // TODO: implement seek
                return false;
            }
        }

        /// <summary>
        /// Whether or not this stream can be written to
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// The length of the stream
        /// </summary>
        public override long Length
        {
            get
            {
                long length = 0;
                for (int i = 0; i < _buffers.Count; i++)
                    length += _buffers[i].End - _buffers[i].Offset;
                return length;
            }
        }

        /// <summary>
        /// The current position within the stream
        /// </summary>
        public override long Position
        {
            get
            {
                long position = 0;
                for (int i = 0; i < _bufferIndex; i++)
                    position += _buffers[i].End - _buffers[i].Offset;
                position += _offset;
                return position;
            }

            set
            {
                // TODO: Implement
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Flushes the stream
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Seeks to a position within the stream
        /// </summary>
        /// <param name="offset">The offset to seek to</param>
        /// <param name="origin">The origin of the offset</param>
        /// <returns>The new position within the stream</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Sets the length of the stream
        /// </summary>
        /// <param name="value">The new length of the stream</param>
        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Reads from the stream
        /// </summary>
        /// <param name="buffer">The buffer to read into</param>
        /// <param name="offset">The offset to read into</param>
        /// <param name="count">The number of bytes to read</param>
        /// <returns>The number of bytes read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = 0;

            while(count > 0 && _bufferIndex < _buffers.Count)
            {
                int toRead = Math.Min(count, _buffer.End - _offset);
                Array.Copy(_buffer.Buffer, _offset, buffer, offset, toRead);
                count -= toRead;
                read += toRead;
                offset += toRead;
                _offset += toRead;

                if (count > 0 && !_nextBuffer())
                    break;
            }

            return read;
        }

        /// <summary>
        /// Writes to the stream
        /// </summary>
        /// <param name="buffer">The buffer containing the bytes to write</param>
        /// <param name="offset">The offset of the bytes to write</param>
        /// <param name="count">The number of bytes to write</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException();
        }
    }
}
