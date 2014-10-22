using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum EventType : uint
	{
		ChangeOfBitstring = 0,
		ChangeOfState = 1,
		ChangeOfValue = 2,
		CommandFailure = 3,
		FloatingLimit = 4,
		OutOfRange = 5,
		ChangeOfLifeSafety = 8,
		Extended = 9,
		BufferReady = 10,
		UnsignedRange = 11
	}
}
