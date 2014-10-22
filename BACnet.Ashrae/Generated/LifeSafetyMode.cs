using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum LifeSafetyMode : uint
	{
		Off = 0,
		On = 1,
		Test = 2,
		Manned = 3,
		Unmanned = 4,
		Armed = 5,
		Disarmed = 6,
		Prearmed = 7,
		Slow = 8,
		Fast = 9,
		Disconnected = 10,
		Enabled = 11,
		Disabled = 12,
		AutomaticReleaseDisabled = 13,
		Default = 14
	}
}
