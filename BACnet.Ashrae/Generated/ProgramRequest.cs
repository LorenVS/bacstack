using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum ProgramRequest : uint
	{
		Ready = 0,
		Load = 1,
		Run = 2,
		Halt = 3,
		Restart = 4,
		Unload = 5
	}
}
