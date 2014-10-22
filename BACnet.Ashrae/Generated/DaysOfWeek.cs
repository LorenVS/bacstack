using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct DaysOfWeek
	{
	public enum Bits : byte
	{
		Monday = 0,
		Tuesday = 1,
		Wednesday = 2,
		Thursday = 3,
		Friday = 4,
		Saturday = 5,
		Sunday = 6
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public DaysOfWeek(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public DaysOfWeek WithLength(byte length) { return new DaysOfWeek(_bitstring.WithLength(length)); }

		public DaysOfWeek WithBit(Bits bit, bool set = true) { return new DaysOfWeek(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static DaysOfWeek Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new DaysOfWeek(temp);
		}

		public static void Save(IValueSink sink, DaysOfWeek value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
