using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum ProgramError : uint
	{
		Normal = 0,
		LoadFailed = 1,
		Internal = 2,
		Program = 3,
		Other = 4
	}
}
