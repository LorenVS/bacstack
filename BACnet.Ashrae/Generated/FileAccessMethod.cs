using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum FileAccessMethod : uint
	{
		RecordAccess = 0,
		StreamAccess = 1
	}
}
