using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum LifeSafetyOperation : uint
	{
		None = 0,
		Silence = 1,
		SilenceAudible = 2,
		SilenceVisual = 3,
		Reset = 4,
		ResetAlarm = 5,
		ResetFault = 6,
		Unsilence = 7,
		UnsilenceAudible = 8,
		UnsilenceVisual = 9
	}
}
