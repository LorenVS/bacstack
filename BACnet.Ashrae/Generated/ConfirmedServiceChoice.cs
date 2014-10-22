using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public enum ConfirmedServiceChoice : uint
	{
		AcknowledgeAlarm = 0,
		ConfirmedCOVNotification = 1,
		ConfirmedEventNotification = 2,
		GetAlarmSummary = 3,
		GetEnrollmentSummary = 4,
		GetEventInformation = 29,
		SubscribeCOV = 5,
		SubscribeCOVProperty = 28,
		LifeSafetyOperation = 27,
		AtomicReadFile = 6,
		AtomicWriteFile = 7,
		AddListElement = 8,
		RemoveListElement = 9,
		CreateObject = 10,
		DeleteObject = 11,
		ReadProperty = 12,
		ReadPropertyConditional = 13,
		ReadPropertyMultiple = 14,
		ReadRange = 26,
		WriteProperty = 15,
		WritePropertyMultiple = 16,
		DeviceCommunicationControl = 17,
		ConfirmedPrivateTransfer = 18,
		ConfirmedTextMessage = 19,
		ReinitializeDevice = 20,
		VtOpen = 21,
		VtClose = 22,
		VtData = 23,
		Authenticate = 24,
		RequestKey = 25
	}
}
