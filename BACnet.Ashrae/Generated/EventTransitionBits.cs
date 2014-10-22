using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct EventTransitionBits
	{
	public enum Bits : byte
	{
		ToOffnormal = 0,
		ToFault = 1,
		ToNormal = 2
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public EventTransitionBits(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public EventTransitionBits WithLength(byte length) { return new EventTransitionBits(_bitstring.WithLength(length)); }

		public EventTransitionBits WithBit(Bits bit, bool set = true) { return new EventTransitionBits(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static EventTransitionBits Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new EventTransitionBits(temp);
		}

		public static void Save(IValueSink sink, EventTransitionBits value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
