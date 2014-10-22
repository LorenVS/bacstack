using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class GetAlarmSummaryRequest
	{
		public GetAlarmSummaryRequest()
		{
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
);

		public static GetAlarmSummaryRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			stream.LeaveSequence();
			return new GetAlarmSummaryRequest();
		}

		public static void Save(IValueSink sink, GetAlarmSummaryRequest value)
		{
			sink.EnterSequence();
			sink.LeaveSequence();
		}
	}
}
