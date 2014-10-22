using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ConfirmedTextMessageRequest
	{
		public ObjectId TextMessageSourceDevice { get; private set; }

		public Option<MessageClassType> MessageClass { get; private set; }

		public MessagePriorityType MessagePriority { get; private set; }

		public string Message { get; private set; }

		public ConfirmedTextMessageRequest(ObjectId textMessageSourceDevice, Option<MessageClassType> messageClass, MessagePriorityType messagePriority, string message)
		{
			this.TextMessageSourceDevice = textMessageSourceDevice;
			this.MessageClass = messageClass;
			this.MessagePriority = messagePriority;
			this.Message = message;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("TextMessageSourceDevice", 0, Value<ObjectId>.Schema),
			new FieldSchema("MessageClass", 1, Value<Option<MessageClassType>>.Schema),
			new FieldSchema("MessagePriority", 2, Value<MessagePriorityType>.Schema),
			new FieldSchema("Message", 3, Value<string>.Schema));

		public static ConfirmedTextMessageRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var textMessageSourceDevice = Value<ObjectId>.Load(stream);
			var messageClass = Value<Option<MessageClassType>>.Load(stream);
			var messagePriority = Value<MessagePriorityType>.Load(stream);
			var message = Value<string>.Load(stream);
			stream.LeaveSequence();
			return new ConfirmedTextMessageRequest(textMessageSourceDevice, messageClass, messagePriority, message);
		}

		public static void Save(IValueSink sink, ConfirmedTextMessageRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.TextMessageSourceDevice);
			Value<Option<MessageClassType>>.Save(sink, value.MessageClass);
			Value<MessagePriorityType>.Save(sink, value.MessagePriority);
			Value<string>.Save(sink, value.Message);
			sink.LeaveSequence();
		}

		public enum Tags : byte
		{
			Numeric = 0,
			Character = 1
		}

		public abstract  partial class MessageClassType
		{
			public abstract Tags Tag { get; }

			public bool IsNumeric { get { return this.Tag == Tags.Numeric; } }

			public uint AsNumeric { get { return ((NumericWrapper)this).Item; } }

			public static MessageClassType NewNumeric(uint numeric)
			{
				return new NumericWrapper(numeric);
			}

			public bool IsCharacter { get { return this.Tag == Tags.Character; } }

			public string AsCharacter { get { return ((CharacterWrapper)this).Item; } }

			public static MessageClassType NewCharacter(string character)
			{
				return new CharacterWrapper(character);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("Numeric", 0, Value<uint>.Schema),
				new FieldSchema("Character", 1, Value<string>.Schema));

			public static MessageClassType Load(IValueStream stream)
			{
				MessageClassType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.Numeric:
						ret = Value<NumericWrapper>.Load(stream);
						break;
					case Tags.Character:
						ret = Value<CharacterWrapper>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, MessageClassType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.Numeric:
						Value<NumericWrapper>.Save(sink, (NumericWrapper)value);
						break;
					case Tags.Character:
						Value<CharacterWrapper>.Save(sink, (CharacterWrapper)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}

		public  partial class NumericWrapper : MessageClassType
		{
			public override Tags Tag { get { return Tags.Numeric; } }

			public uint Item { get; private set; }

			public NumericWrapper(uint item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<uint>.Schema;

			public static new NumericWrapper Load(IValueStream stream)
			{
				var temp = Value<uint>.Load(stream);
				return new NumericWrapper(temp);
			}

			public static void Save(IValueSink sink, NumericWrapper value)
			{
				Value<uint>.Save(sink, value.Item);
			}

		}

		public  partial class CharacterWrapper : MessageClassType
		{
			public override Tags Tag { get { return Tags.Character; } }

			public string Item { get; private set; }

			public CharacterWrapper(string item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<string>.Schema;

			public static new CharacterWrapper Load(IValueStream stream)
			{
				var temp = Value<string>.Load(stream);
				return new CharacterWrapper(temp);
			}

			public static void Save(IValueSink sink, CharacterWrapper value)
			{
				Value<string>.Save(sink, value.Item);
			}

		}
		public enum MessagePriorityType : uint
		{
			Normal = 0,
			Urgent = 1
		}
	}
}
