using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class ActionList
	{
		public ReadOnlyArray<ActionCommand> Action { get; private set; }

		public ActionList(ReadOnlyArray<ActionCommand> action)
		{
			this.Action = action;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("Action", 0, Value<ReadOnlyArray<ActionCommand>>.Schema));

		public static ActionList Load(IValueStream stream)
		{
			stream.EnterSequence();
			var action = Value<ReadOnlyArray<ActionCommand>>.Load(stream);
			stream.LeaveSequence();
			return new ActionList(action);
		}

		public static void Save(IValueSink sink, ActionList value)
		{
			sink.EnterSequence();
			Value<ReadOnlyArray<ActionCommand>>.Save(sink, value.Action);
			sink.LeaveSequence();
		}
	}
}
