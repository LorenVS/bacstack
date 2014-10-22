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
    public class TagWriterSink : IValueSink
    {
        /// <summary>
        /// The tag writer instance to write to
        /// </summary>
        private TagWriter _writer;

        /// <summary>
        /// The current schema state
        /// </summary>
        private SchemaState _state;

        /// <summary>
        /// The stack of schemas to write
        /// </summary>
        private Stack<SchemaState> _stack;

        /// <summary>
        /// Constructs a new TagWriterSink
        /// </summary>
        /// <param name="writer">The tag writer instance to write to</param>
        /// <param name="schema">The schema for the types to write</param>
        public TagWriterSink(TagWriter writer, ISchema schema)
        {
            this._writer = writer;
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
        /// Moves the current state to the next value to read
        /// </summary>
        private void _moveNext()
        {
            if (_stack.Count == 0)
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
                var newParent = new SchemaState(array, parent.Tag, parent.Index + 1);
                _stack.Push(newParent);
                _state = new SchemaState(array.ElementType, 255, -1);
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
                switch (_state.Schema.Type)
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
                    case Types.ValueType.List:
                        if (_state.Index == -1)
                            return StreamOp.EnterList;
                        else
                            return StreamOp.LeaveList;
                    case Types.ValueType.EOF:
                        return StreamOp.EOF;
                }

                throw new Exception("Unknown value type, can't convert to stream op");
            }
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutNull(Null value)
        {
            _require(StreamOp.Null);
            _writer.WriteNull(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutBoolean(bool value)
        {
            _require(StreamOp.Boolean);
            _writer.WriteBoolean(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutUnsigned8(byte value)
        {
            _require(StreamOp.Unsigned8);
            _writer.WriteUnsigned8(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutUnsigned16(ushort value)
        {
            _require(StreamOp.Unsigned16);
            _writer.WriteUnsigned16(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutUnsigned32(uint value)
        {
            _require(StreamOp.Unsigned32);
            _writer.WriteUnsigned32(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutUnsigned64(ulong value)
        {
            _require(StreamOp.Unsigned64);
            _writer.WriteUnsigned64(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutSigned8(sbyte value)
        {
            _require(StreamOp.Signed8);
            _writer.WriteSigned8(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutSigned16(short value)
        {
            _require(StreamOp.Signed16);
            _writer.WriteSigned16(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutSigned32(int value)
        {
            _require(StreamOp.Signed32);
            _writer.WriteSigned32(value, _state.Tag);
            _moveNext();
        }
        
        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutSigned64(long value)
        {
            _require(StreamOp.Signed64);
            _writer.WriteSigned64(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutFloat32(float value)
        {
            _require(StreamOp.Float32);
            _writer.WriteFloat32(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutFloat64(double value)
        {
            _require(StreamOp.Float64);
            _writer.WriteFloat64(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutOctetString(byte[] value)
        {
            _require(StreamOp.OctetString);
            _writer.WriteOctetString(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutCharString(string value)
        {
            _require(StreamOp.CharString);
            _writer.WriteCharString(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutBitString8(BitString8 value)
        {
            _require(StreamOp.BitString8);
            _writer.WriteBitString8(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutBitString24(BitString24 value)
        {
            _require(StreamOp.BitString24);
            _writer.WriteBitString24(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutBitString56(BitString56 value)
        {
            _require(StreamOp.BitString56);
            _writer.WriteBitString56(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutEnumerated(Enumerated value)
        {
            _require(StreamOp.Enumerated);
            _writer.WriteEnumerated(value.Value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutDate(Date value)
        {
            _require(StreamOp.Date);
            _writer.WriteDate(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutTime(Time value)
        {
            _require(StreamOp.Time);
            _writer.WriteTime(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutObjectId(ObjectId value)
        {
            _require(StreamOp.ObjectId);
            _writer.WriteObjectId(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Puts a value into the sink
        /// </summary>
        /// <param name="value">The value to put</param>
        public void PutGeneric(GenericValue value)
        {
            _require(StreamOp.Generic);
            _writer.WriteGeneric(value, _state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Optionally enters  value
        /// </summary>
        /// <param name="hasValue">True if there is a value, false otherwise</param>
        public void EnterOption(bool hasValue)
        {
            _require(StreamOp.Option);

            if (hasValue)
            {
                var option = (OptionSchema)_state.Schema;
                _stack.Push(_state);
                _state = new SchemaState(option.ElementType, _state.Tag, -1);
            }
            else
            {
                _moveNext();
            }
        }

        /// <summary>
        /// Enters a sequence value
        /// </summary>
        public void EnterSequence()
        {
            _require(StreamOp.EnterSequence);
            _writer.WriteOpenTag(_state.Tag);
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
            _writer.WriteCloseTag(_state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Enters a choice value
        /// </summary>
        /// <param name="choiceIndex">The active choice</param>
        public void EnterChoice(byte choiceIndex)
        {
            _require(StreamOp.EnterChoice);
            _writer.WriteOpenTag(_state.Tag);

            var choice = ((ChoiceSchema)_state.Schema);
            var field = choice.Fields[choiceIndex];
            _stack.Push(new SchemaState(choice, _state.Tag, choiceIndex));
            _state = new SchemaState(field.Type, field.Tag, -1);
        }


        /// <summary>
        /// Leaves a choice value
        /// </summary>
        public void LeaveChoice()
        {
            _require(StreamOp.LeaveChoice);
            _writer.WriteCloseTag(_state.Tag);
            _moveNext();
        }

        /// <summary>
        /// Enters an array value
        /// </summary>
        public void EnterArray()
        {
            _require(StreamOp.EnterArray);
            var array = (ArraySchema)_state.Schema;
            _writer.WriteOpenTag(_state.Tag);
            _stack.Push(new SchemaState(array, _state.Tag, 0));
            _state = new SchemaState(array.ElementType, 255, -1);
        }

        /// <summary>
        /// Leaves an array value
        /// </summary>
        public void LeaveArray()
        {
            if (_stack.Peek().Schema.Type != Types.ValueType.Array)
                throw new InvalidOperationException();
            _state = _stack.Pop();
            _writer.WriteCloseTag(_state.Tag);
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

            public SchemaState(ISchema schema, byte tag, int index)
                : this()
            {
                this.Schema = schema;
                this.Tag = tag;
                this.Index = index;
            }
        }
    }
}
