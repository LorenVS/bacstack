using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum LifeSafetyState : uint
	{
		Quiet = 0,
		PreAlarm = 1,
		Alarm = 2,
		Fault = 3,
		FaultPreAlarm = 4,
		FaultAlarm = 5,
		NotReady = 6,
		Active = 7,
		Tamper = 8,
		TestAlarm = 9,
		TestActive = 10,
		TestFault = 11,
		TestFaultAlarm = 12,
		Holdup = 13,
		Duress = 14,
		TamperAlarm = 15,
		Abnormal = 16,
		EmergencyPower = 17,
		Delayed = 18,
		Blocked = 19,
		LocalAlarm = 20,
		GeneralAlarm = 21,
		Supervisory = 22,
		TestSupervisory = 23
	}
}
