using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum Maintenance : uint
	{
		None = 0,
		PeriodicTest = 1,
		NeedServiceOperational = 2,
		NeedServiceInoperative = 3
	}
}
