using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum ProgramState : uint
	{
		Idle = 0,
		Loading = 1,
		Running = 2,
		Waiting = 3,
		Halted = 4,
		Unloading = 5
	}
}
