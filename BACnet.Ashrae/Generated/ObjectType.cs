using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum ObjectType : uint
	{
		Accumulator = 23,
		AnalogInput = 0,
		AnalogOutput = 1,
		AnalogValue = 2,
		Averaging = 18,
		BinaryInput = 3,
		BinaryOutput = 4,
		BinaryValue = 5,
		Calendar = 6,
		Command = 7,
		Device = 8,
		EventEnrollment = 9,
		File = 10,
		Group = 11,
		LifeSafetyPoint = 21,
		LifeSafetyZone = 22,
		Loop = 12,
		MultiStateInput = 13,
		MultiStateOutput = 14,
		MultiStateValue = 19,
		NotificationClass = 15,
		Program = 16,
		PulseConverter = 24,
		Schedule = 17,
		TrendLog = 20
	}
}
