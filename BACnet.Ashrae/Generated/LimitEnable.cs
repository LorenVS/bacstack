using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct LimitEnable
	{
	public enum Bits : byte
	{
		LowLimitEnable = 0,
		HighLimitEnable = 1
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public LimitEnable(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public LimitEnable WithLength(byte length) { return new LimitEnable(_bitstring.WithLength(length)); }

		public LimitEnable WithBit(Bits bit, bool set = true) { return new LimitEnable(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static LimitEnable Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new LimitEnable(temp);
		}

		public static void Save(IValueSink sink, LimitEnable value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
