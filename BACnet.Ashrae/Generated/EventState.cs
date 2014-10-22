using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum EventState : uint
	{
		Normal = 0,
		Fault = 1,
		Offnormal = 2,
		HighLimit = 3,
		LowLimit = 4,
		LifeSafetyAlarm = 5
	}
}
