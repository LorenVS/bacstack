using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum SilencedState : uint
	{
		Unsilenced = 0,
		AudibleSilenced = 1,
		VisibleSilenced = 2,
		AllSilenced = 3
	}
}
