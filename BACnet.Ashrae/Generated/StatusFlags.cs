using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct StatusFlags
	{
	public enum Bits : byte
	{
		InAlarm = 0,
		Fault = 1,
		Overridden = 2,
		OutOfService = 3
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public StatusFlags(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public StatusFlags WithLength(byte length) { return new StatusFlags(_bitstring.WithLength(length)); }

		public StatusFlags WithBit(Bits bit, bool set = true) { return new StatusFlags(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static StatusFlags Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new StatusFlags(temp);
		}

		public static void Save(IValueSink sink, StatusFlags value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
