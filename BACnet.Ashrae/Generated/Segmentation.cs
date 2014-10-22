using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum Segmentation : uint
	{
		SegmentedBoth = 0,
		SegmentedTransmit = 1,
		SegmentedReceive = 2,
		NoSegmentation = 3
	}
}
