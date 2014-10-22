using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum DeviceStatus : uint
	{
		Operational = 0,
		OperationalReadOnly = 1,
		DownloadRequired = 2,
		DownloadInProgress = 3,
		NonOperational = 4,
		BackupInProgress = 5
	}
}
