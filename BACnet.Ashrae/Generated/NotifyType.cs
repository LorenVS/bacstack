using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum NotifyType : uint
	{
		Alarm = 0,
		Event = 1,
		AckNotification = 2
	}
}
