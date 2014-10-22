using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum AbortReason : uint
	{
		Other = 0,
		BufferOverflow = 1,
		InvalidApduInThisState = 2,
		PreemptedByHigherPriorityTask = 3,
		SegmentationNotSupported = 4
	}
}
