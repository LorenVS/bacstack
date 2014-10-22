using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct ServicesSupported
	{
	public enum Bits : byte
	{
		AcknowledgeAlarm = 0,
		ConfirmedCOVNotification = 1,
		ConfirmedEventNotification = 2,
		GetAlarmSummary = 3,
		GetEnrollmentSummary = 4,
		SubscribeCOV = 5,
		AtomicReadFile = 6,
		AtomicWriteFile = 7,
		AddListElement = 8,
		RemoveListElement = 9,
		CreateObject = 10,
		DeleteObject = 11,
		ReadProperty = 12,
		ReadPropertyConditional = 13,
		ReadPropertyMultiple = 14,
		WriteProperty = 15,
		WritePropertyMultiple = 16,
		DeviceCommunicationControl = 17,
		ConfirmedPrivateTransfer = 18,
		ConfirmedTextMessage = 19,
		ReinitializeDevice = 20,
		VtOpen = 21,
		VtClose = 22,
		VtData = 23,
		Authenticate = 24,
		RequestKey = 25,
		IAm = 26,
		IHave = 27,
		UnconfirmedCOVNotification = 28,
		UnconfirmedEventNotification = 29,
		UnconfirmedPrivateTransfer = 30,
		UnconfirmedTextMessage = 31,
		TimeSynchronization = 32,
		WhoHas = 33,
		WhoIs = 34,
		ReadRange = 35,
		UtcTimeSynchronization = 36,
		LifeSafetyOperation = 37,
		SubscribeCOVProperty = 38,
		GetEventInformation = 39
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public ServicesSupported(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public ServicesSupported WithLength(byte length) { return new ServicesSupported(_bitstring.WithLength(length)); }

		public ServicesSupported WithBit(Bits bit, bool set = true) { return new ServicesSupported(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static ServicesSupported Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new ServicesSupported(temp);
		}

		public static void Save(IValueSink sink, ServicesSupported value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
