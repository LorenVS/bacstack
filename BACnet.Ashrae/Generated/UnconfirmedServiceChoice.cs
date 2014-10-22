using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum UnconfirmedServiceChoice : uint
	{
		IAm = 0,
		IHave = 1,
		UnconfirmedCOVNotification = 2,
		UnconfirmedEventNotification = 3,
		UnconfirmedPrivateTransfer = 4,
		UnconfirmedTextMessage = 5,
		TimeSynchronization = 6,
		WhoHas = 7,
		WhoIs = 8,
		UtcTimeSynchronization = 9
	}
}
