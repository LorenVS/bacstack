using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum Reliability : uint
	{
		NoFaultDetected = 0,
		NoSensor = 1,
		OverRange = 2,
		UnderRange = 3,
		OpenLoop = 4,
		ShortedLoop = 5,
		NoOutput = 6,
		UnreliableOther = 7,
		ProcessError = 8,
		MultiStateFault = 9,
		ConfigurationError = 10
	}
}
