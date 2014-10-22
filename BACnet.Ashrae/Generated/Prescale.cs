using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class Prescale
	{
		public uint Multiplier { get; private set; }

		public uint ModuloDivide { get; private set; }

		public Prescale(uint multiplier, uint moduloDivide)
		{
			this.Multiplier = multiplier;
			this.ModuloDivide = moduloDivide;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Multiplier", 0, Value<uint>.Schema),
			new FieldSchema("ModuloDivide", 1, Value<uint>.Schema));

		public static Prescale Load(IValueStream stream)
		{
			stream.EnterSequence();
			var multiplier = Value<uint>.Load(stream);
			var moduloDivide = Value<uint>.Load(stream);
			stream.LeaveSequence();
			return new Prescale(multiplier, moduloDivide);
		}

		public static void Save(IValueSink sink, Prescale value)
		{
			sink.EnterSequence();
			Value<uint>.Save(sink, value.Multiplier);
			Value<uint>.Save(sink, value.ModuloDivide);
			sink.LeaveSequence();
		}
	}
}
