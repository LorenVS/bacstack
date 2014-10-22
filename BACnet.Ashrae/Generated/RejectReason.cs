using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum RejectReason : uint
	{
		Other = 0,
		BufferOverflow = 1,
		InconsistentParameters = 2,
		InvalidParameterDataType = 3,
		InvalidTag = 4,
		MissingRequiredParameter = 5,
		ParameterOutOfRange = 6,
		TooManyArguments = 7,
		UndefinedEnumeration = 8,
		UnrecognizedService = 9
	}
}
