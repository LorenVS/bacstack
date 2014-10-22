using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AuthenticateRequest
	{
		public uint PseudoRandomNumber { get; private set; }

		public Option<byte> ExpectedInvokeID { get; private set; }

		public Option<string> OperatorName { get; private set; }

		public Option<string> OperatorPassword { get; private set; }

		public Option<bool> StartEncipheredSession { get; private set; }

		public AuthenticateRequest(uint pseudoRandomNumber, Option<byte> expectedInvokeID, Option<string> operatorName, Option<string> operatorPassword, Option<bool> startEncipheredSession)
		{
			this.PseudoRandomNumber = pseudoRandomNumber;
			this.ExpectedInvokeID = expectedInvokeID;
			this.OperatorName = operatorName;
			this.OperatorPassword = operatorPassword;
			this.StartEncipheredSession = startEncipheredSession;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("PseudoRandomNumber", 0, Value<uint>.Schema),
			new FieldSchema("ExpectedInvokeID", 1, Value<Option<byte>>.Schema),
			new FieldSchema("OperatorName", 2, Value<Option<string>>.Schema),
			new FieldSchema("OperatorPassword", 3, Value<Option<string>>.Schema),
			new FieldSchema("StartEncipheredSession", 4, Value<Option<bool>>.Schema));

		public static AuthenticateRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var pseudoRandomNumber = Value<uint>.Load(stream);
			var expectedInvokeID = Value<Option<byte>>.Load(stream);
			var operatorName = Value<Option<string>>.Load(stream);
			var operatorPassword = Value<Option<string>>.Load(stream);
			var startEncipheredSession = Value<Option<bool>>.Load(stream);
			stream.LeaveSequence();
			return new AuthenticateRequest(pseudoRandomNumber, expectedInvokeID, operatorName, operatorPassword, startEncipheredSession);
		}

		public static void Save(IValueSink sink, AuthenticateRequest value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.PseudoRandomNumber);
			Value<Option<byte>>.Save(sink, value.ExpectedInvokeID);
			Value<Option<string>>.Save(sink, value.OperatorName);
			Value<Option<string>>.Save(sink, value.OperatorPassword);
			Value<Option<bool>>.Save(sink, value.StartEncipheredSession);
			sink.LeaveSequence();
		}
	}
}
