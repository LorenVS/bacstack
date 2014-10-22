using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum VTClass : uint
	{
		DefaultTerminal = 0,
		AnsiX364 = 1,
		DecVt52 = 2,
		DecVt100 = 3,
		DecVt220 = 4,
		Hp70094 = 5,
		Ibm3130 = 6
	}
}
