using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct ObjectTypesSupported
	{
	public enum Bits : byte
	{
		AnalogInput = 0,
		AnalogOutput = 1,
		AnalogValue = 2,
		BinaryInput = 3,
		BinaryOutput = 4,
		BinaryValue = 5,
		Calendar = 6,
		Command = 7,
		Device = 8,
		EventEnrollment = 9,
		File = 10,
		Group = 11,
		Loop = 12,
		MultiStateInput = 13,
		MultiStateOutput = 14,
		NotificationClass = 15,
		Program = 16,
		Schedule = 17,
		Averaging = 18,
		MultiStateValue = 19,
		TrendLog = 20,
		LifeSafetyPoint = 21,
		LifeSafetyZone = 22,
		Accumulator = 23,
		PulseConverter = 24
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public ObjectTypesSupported(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public ObjectTypesSupported WithLength(byte length) { return new ObjectTypesSupported(_bitstring.WithLength(length)); }

		public ObjectTypesSupported WithBit(Bits bit, bool set = true) { return new ObjectTypesSupported(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static ObjectTypesSupported Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new ObjectTypesSupported(temp);
		}

		public static void Save(IValueSink sink, ObjectTypesSupported value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
