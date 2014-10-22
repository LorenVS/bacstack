using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct ResultFlags
	{
	public enum Bits : byte
	{
		FirstItem = 0,
		LastItem = 1,
		MoreItems = 2
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public ResultFlags(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public ResultFlags WithLength(byte length) { return new ResultFlags(_bitstring.WithLength(length)); }

		public ResultFlags WithBit(Bits bit, bool set = true) { return new ResultFlags(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static ResultFlags Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new ResultFlags(temp);
		}

		public static void Save(IValueSink sink, ResultFlags value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
