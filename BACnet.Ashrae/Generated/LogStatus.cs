using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public struct LogStatus
	{
	public enum Bits : byte
	{
		LogDisabled = 0,
		BufferPurged = 1
	}

		private BitString56 _bitstring;

		public byte Length { get { return _bitstring.Length; } }

		public bool this[Bits bit] { get { return _bitstring[(int)bit]; } }

		public LogStatus(BitString56 bitstring)
		{
			this._bitstring = bitstring;
		}

		public LogStatus WithLength(byte length) { return new LogStatus(_bitstring.WithLength(length)); }

		public LogStatus WithBit(Bits bit, bool set = true) { return new LogStatus(_bitstring.WithBit((int)bit, set)); }

		public static readonly ISchema Schema = PrimitiveSchema.BitString56Schema;

		public static LogStatus Load(IValueStream stream)
		{
			var temp = Value<BitString56>.Load(stream);
			return new LogStatus(temp);
		}

		public static void Save(IValueSink sink, LogStatus value)
		{
			Value<BitString56>.Save(sink, value._bitstring);
		}
	}
}
