using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AuthenticateAck
	{
		public uint ModifiedRandomNumber { get; private set; }

		public AuthenticateAck(uint modifiedRandomNumber)
		{
			this.ModifiedRandomNumber = modifiedRandomNumber;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("ModifiedRandomNumber", 255, Value<uint>.Schema));

		public static AuthenticateAck Load(IValueStream stream)
		{
			stream.EnterSequence();
			var modifiedRandomNumber = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new AuthenticateAck(modifiedRandomNumber);
		}

		public static void Save(IValueSink sink, AuthenticateAck value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.ModifiedRandomNumber);
			sink.LeaveSequence();
		}
	}
}
