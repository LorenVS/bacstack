using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Tagging
{
    public class TagReaderStream : IValueStream
    {
        /// <summary>
        /// The tag reader instance to read from
        /// </summary>
        private TagReader _reader;

        /// <summary>
        /// The current schema state
        /// </summary>
        private SchemaState _state;

        /// <summary>
        /// The stack of schemas to read
        /// </summary>
        private Stack<SchemaState> _stack;
        
        /// <summary>
        /// Constructs a new TagReaderStream
        /// </summary>
        /// <param name="reader">The tag reader instance to read from</param>
        /// <param name="schema">The schema for the types to read</param>
        public TagReaderStream(TagReader reader, ISchema schema)
        {
            this._reader = reader;
            this._state = new SchemaState(schema, 255, -1);
            this._stack = new Stack<SchemaState>(4);
        }

        /// <summary>
        /// Requires that the next operation is expected
        /// </summary>
        /// <param name="op">The expected operation</param>
        private void _require(StreamOp op)
        {
            if (Next != op)
                throw new InvalidOperationException();
        }
        
        /// <summary>
        /// Determines whether the tag reader is at the end of an array
        /// or list
        /// </summary>
        /// <param name="tag">The tag number of the containing array or list</param>
        /// <returns>True if the end has been reached, false otherwise</returns>
        private bool _atEnd(byte tag)
        {
            if (tag == 255)
            {
                // TODO: EOF logic
                return false;
            }
            else
            {
                return _reader.AtCloseTag(tag);
            }
        }

        /// <summary>
        /// Moves the current state to the next value to read
        /// </summary>
        private void _moveNext()
        {
            if(_stack.Count == 0)
            {
                _state = new SchemaState(PrimitiveSchema.EOF, 255, -1);
                return;
            }

            var parent = _stack.Pop();
            if (parent.Schema.Type == Types.ValueType.Option)
            {
                // we skip option states on the way back up
                _moveNext();
            }
            else if (parent.Schema.Type == Types.ValueType.Sequence)
            {
                var sequence = (SequenceSchema)parent.Schema;
                var newParent = new SchemaState(sequence, parent.Tag, parent.Index + 1);

                if (newParent.Index == sequence.Fields.Length)
                {
                    // we have read all of fields
                    _state = newParent;
                }
                else
                {
                    // we still have at least 1 field to read
                    _stack.Push(newParent);
                    var field = sequence.Fields[newParent.Index];
                    _state = new SchemaState(field.Type, field.Tag, -1);

                }
            }
            else if (parent.Schema.Type == Types.ValueType.Choice)
            {
                var choice = (ChoiceSchema)parent.Schema;
                _state = new SchemaState(choice, parent.Tag, choice.Fields.Length);
            }
            else if (parent.Schema.Type == Types.ValueType.Array)
            {
                var array = (ArraySchema)parent.Schema;

                // TODO: Might still need better logic for detecting
                // whether we are at the end of the array

                if (_reader.EOF())
                {
                    _state = new SchemaState(array, parent.Tag, parent.Index + 1);
                }
                else if (array.ElementType == PrimitiveSchema.GenericSchema)
                {
                    if (_reader.AtTag(parent.Tag, ApplicationTag.Null))
                    {
                        _state = new SchemaState(array, parent.Tag, parent.Index + 1);
                    }
                    else
                    {
                        var newParent = new SchemaState(array, parent.Tag, parent.Index + 1);
                        _stack.Push(newParent);
                        _state = new SchemaState(array.ElementType, 255, -1);
                    }
                }
                else
                {
                    var elementExpected = Utils.GetExpectedTag(255, array.ElementType);
                    if (_reader.AtTag(elementExpected.ContextTag, elementExpected.ApplicationTag))
                    {
                        var newParent = new SchemaState(array, parent.Tag, parent.Index + 1);
                        _stack.Push(newParent);
                        _state = new SchemaState(array.ElementType, 255, -1);
                    }
                    else
                    {
                        _state = new SchemaState(array, parent.Tag, parent.Index + 1);
                    }
                }
            }
            else
                throw new Exception("unknown parent schema state, can't move next");
        }

        /// <summary>
        /// The next operation that the stream expects
        /// </summary>
        public StreamOp Next
        {
            get
            {
                switch(_state.Schema.Type)
                {
                    case Types.ValueType.Null:
                        return StreamOp.Null;
                    case Types.ValueType.Boolean:
                        return StreamOp.Boolean;
                    case Types.ValueType.Unsigned8:
                        return StreamOp.Unsigned8;
                    case Types.ValueType.Unsigned16:
                        return StreamOp.Unsigned16;
                    case Types.ValueType.Unsigned32:
                        return StreamOp.Unsigned32;
                    case Types.ValueType.Unsigned64:
                        return StreamOp.Unsigned64;
                    case Types.ValueType.Signed8:
                        return StreamOp.Signed8;
                    case Types.ValueType.Signed16:
                        return StreamOp.Signed16;
                    case Types.ValueType.Signed32:
                        return StreamOp.Signed32;
                    case Types.ValueType.Signed64:
                        return StreamOp.Signed64;
                    case Types.ValueType.Float32:
                        return StreamOp.Float32;
                    case Types.ValueType.Float64:
                        return StreamOp.Float64;
                    case Types.ValueType.OctetString:
                        return StreamOp.OctetString;
                    case Types.ValueType.CharString:
                        return StreamOp.CharString;
                    case Types.ValueType.BitString8:
                        return StreamOp.BitString8;
                    case Types.ValueType.BitString24:
                        return StreamOp.BitString24;
                    case Types.ValueType.BitString56:
                        return StreamOp.BitString56;
                    case Types.ValueType.Enumerated:
                        return StreamOp.Enumerated;
                    case Types.ValueType.Date:
                        return StreamOp.Date;
                    case Types.ValueType.Time:
                        return StreamOp.Time;
                    case Types.ValueType.ObjectId:
                        return StreamOp.ObjectId;
                    case Types.ValueType.Generic:
                        return StreamOp.Generic;
                    case Types.ValueType.Option:
                        return StreamOp.Option;
                    case Types.ValueType.Sequence:
                        var sequence = (SequenceSchema)_state.Schema;
                        if (sequence.Fields.Length == _state.Index)
                            return StreamOp.LeaveSequence;
                        else
                            return StreamOp.EnterSequence;
                    case Types.ValueType.Choice:
                        var choice = (ChoiceSchema)_state.Schema;
                        if (choice.Fields.Length == _state.Index)
                            return StreamOp.LeaveChoice;
                        else
                            return StreamOp.EnterChoice;
                    case Types.ValueType.Array:
                        if (_state.Index == -1)
                            return StreamOp.EnterArray;
                        else
                            return StreamOp.LeaveArray;
                    case Types.ValueType.EOF:
                        return StreamOp.EOF;
                }

                throw new Exception("Unknown value type, can't convert to stream op");
            }
        }
        
        /// <summary>
        /// Retrieves a null value from the tream
        /// </summary>
        /// <returns>The value</returns>
        public Null GetNull()
        {
            _require(StreamOp.Null);
            var ret = _reader.ReadNull(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a boolean value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public bool GetBoolean()
        {
            _require(StreamOp.Boolean);
            var ret = _reader.ReadBoolean(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public byte GetUnsigned8()
        {
            _require(StreamOp.Unsigned8);
            var ret = _reader.ReadUnsigned8(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public ushort GetUnsigned16()
        {
            _require(StreamOp.Unsigned16);
            var ret = _reader.ReadUnsigned16(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public uint GetUnsigned32()
        {
            _require(StreamOp.Unsigned32);
            var ret = _reader.ReadUnsigned32(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an unsigned value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public ulong GetUnsigned64()
        {
            _require(StreamOp.Unsigned64);
            var ret = _reader.ReadUnsigned64(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public sbyte GetSigned8()
        {
            _require(StreamOp.Signed8);
            var ret = _reader.ReadSigned8(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public short GetSigned16()
        {
            _require(StreamOp.Signed16);
            var ret = _reader.ReadSigned16(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public int GetSigned32()
        {
            _require(StreamOp.Signed32);
            var ret = _reader.ReadSigned32(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a signed value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public long GetSigned64()
        {
            _require(StreamOp.Signed64);
            var ret = _reader.ReadSigned64(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a float32 value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public float GetFloat32()
        {
            _require(StreamOp.Float32);
            var ret = _reader.ReadFloat32(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a float64 value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public double GetFloat64()
        {
            _require(StreamOp.Float64);
            var ret = _reader.ReadFloat64(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an octet stream value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public byte[] GetOctetString()
        {
            _require(StreamOp.OctetString);
            var ret = _reader.ReadOctetString(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an char stream value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public string GetCharString()
        {
            _require(StreamOp.CharString);
            var ret = _reader.ReadCharString(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an bit string value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public BitString8 GetBitString8()
        {
            _require(StreamOp.BitString8);
            var ret = _reader.ReadBitString8(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an bit string value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public BitString24 GetBitString24()
        {
            _require(StreamOp.BitString24);
            var ret = _reader.ReadBitString24(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an bit string value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public BitString56 GetBitString56()
        {
            _require(StreamOp.BitString56);
            var ret = _reader.ReadBitString56(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an enumerated value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public uint GetEnumerated()
        {
            _require(StreamOp.Enumerated);
            var ret = _reader.ReadEnumerated(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a date value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public Date GetDate()
        {
            _require(StreamOp.Date);
            var ret = _reader.ReadDate(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a time value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public Time GetTime()
        {
            _require(StreamOp.Time);
            var ret = _reader.ReadTime(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves an object id value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public ObjectId GetObjectId()
        {
            _require(StreamOp.ObjectId);
            var ret = _reader.ReadObjectId(_state.Tag);
            _moveNext();
            return ret;
        }

        /// <summary>
        /// Retrieves a generic value from the stream
        /// </summary>
        /// <returns>The value</returns>
        public GenericValue GetGeneric()
        {
            _require(StreamOp.Generic);
            var ret = _reader.ReadGeneric(_state.Tag);
            _moveNext();
            return ret;
        }


        /// <summary>
        /// Determines whether an option has a value
        /// </summary>
        /// <returns>True if the option has a value, false otherwise</returns>
        public bool OptionHasValue()
        {
            _require(StreamOp.Option);

            // get the expected tag for an option of this type
            var option = (OptionSchema)_state.Schema;
            var expected = Utils.GetExpectedTag(_state.Tag, option.ElementType);
            bool hasValue = _reader.AtTag(expected.ContextTag, expected.ApplicationTag);
            if(hasValue)
            {
                _stack.Push(_state);
                _state = new SchemaState(option.ElementType, _state.Tag, -1);
            }
            else
            {
                _moveNext();
            }

            return hasValue;
        }

        /// <summary>
        /// Enters a sequence value
        /// </summary>
        public void EnterSequence()
        {
            _require(StreamOp.EnterSequence);
            _reader.ReadOpenTag(_state.Tag);
            var sequence = (SequenceSchema)_state.Schema;
            var field = sequence.Fields[0];
            _stack.Push(new SchemaState(sequence, _state.Tag, 0));
            _state = new SchemaState(field.Type, field.Tag, -1);
        }

        /// <summary>
        /// Leaves a sequence value
        /// </summary>
        public void LeaveSequence()
        {
            _require(StreamOp.LeaveSequence);
            _reader.ReadCloseTag(_state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Enters a choice value
        /// </summary>
        /// <returns>The index of the active choice</returns>
        public byte EnterChoice()
        {
            _require(StreamOp.EnterChoice);
            _reader.ReadOpenTag(_state.Tag);

            var choice = ((ChoiceSchema)_state.Schema);
            for(int i = 0; i < choice.Fields.Length; i++)
            {
                var field = choice.Fields[i];
                var expected = Utils.GetExpectedTag(field.Tag, field.Type);

                if (_reader.AtTag(expected.ContextTag, expected.ApplicationTag))
                {
                    _stack.Push(new SchemaState(choice, _state.Tag, (byte)i));
                    _state = new SchemaState(field.Type, field.Tag, -1);
                    return (byte)i;
                }
            }

            throw new UnexpectedTagException();

        }


        /// <summary>
        /// Leaves a choice value
        /// </summary>
        public void LeaveChoice()
        {
            _require(StreamOp.LeaveChoice);
            _reader.ReadCloseTag(_state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Enters an array value
        /// </summary>
        public void EnterArray()
        {
            _require(StreamOp.EnterArray);
            var array = (ArraySchema)_state.Schema;
            _reader.ReadOpenTag(_state.Tag);
            _stack.Push(new SchemaState(array, _state.Tag, 0));
            _moveNext();
        }

        /// <summary>
        /// Leaves an array value
        /// </summary>
        public void LeaveArray()
        {
            _require(StreamOp.LeaveArray);
            _reader.ReadCloseTag(_state.Tag);
            _moveNext();
        }


        private struct SchemaState
        {
            /// <summary>
            /// The schema being read
            /// </summary>
            public ISchema Schema { get; private set; }

            /// <summary>
            /// The tag for the state
            /// </summary>
            public byte Tag { get; private set; }

            /// <summary>
            /// The index within the schema (eg field index)
            /// </summary>
            public int Index { get; private set; }

            public SchemaState(ISchema schema, byte tag, int index) : this()
            {
                this.Schema = schema;
                this.Tag = tag;
                this.Index = index;
            }
        }
    }
}
 