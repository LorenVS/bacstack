using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class ServiceError
	{
		public abstract Tags Tag { get; }

		public bool IsOther { get { return this.Tag == Tags.Other; } }

		public Error AsOther { get { return ((OtherWrapper)this).Item; } }

		public static ServiceError NewOther(Error other)
		{
			return new OtherWrapper(other);
		}

		public bool IsAcknowledgeAlarm { get { return this.Tag == Tags.AcknowledgeAlarm; } }

		public Error AsAcknowledgeAlarm { get { return ((AcknowledgeAlarmWrapper)this).Item; } }

		public static ServiceError NewAcknowledgeAlarm(Error acknowledgeAlarm)
		{
			return new AcknowledgeAlarmWrapper(acknowledgeAlarm);
		}

		public bool IsConfirmedCOVNotification { get { return this.Tag == Tags.ConfirmedCOVNotification; } }

		public Error AsConfirmedCOVNotification { get { return ((ConfirmedCOVNotificationWrapper)this).Item; } }

		public static ServiceError NewConfirmedCOVNotification(Error confirmedCOVNotification)
		{
			return new ConfirmedCOVNotificationWrapper(confirmedCOVNotification);
		}

		public bool IsConfirmedEventNotification { get { return this.Tag == Tags.ConfirmedEventNotification; } }

		public Error AsConfirmedEventNotification { get { return ((ConfirmedEventNotificationWrapper)this).Item; } }

		public static ServiceError NewConfirmedEventNotification(Error confirmedEventNotification)
		{
			return new ConfirmedEventNotificationWrapper(confirmedEventNotification);
		}

		public bool IsGetAlarmSummary { get { return this.Tag == Tags.GetAlarmSummary; } }

		public Error AsGetAlarmSummary { get { return ((GetAlarmSummaryWrapper)this).Item; } }

		public static ServiceError NewGetAlarmSummary(Error getAlarmSummary)
		{
			return new GetAlarmSummaryWrapper(getAlarmSummary);
		}

		public bool IsGetEnrollmentSummary { get { return this.Tag == Tags.GetEnrollmentSummary; } }

		public Error AsGetEnrollmentSummary { get { return ((GetEnrollmentSummaryWrapper)this).Item; } }

		public static ServiceError NewGetEnrollmentSummary(Error getEnrollmentSummary)
		{
			return new GetEnrollmentSummaryWrapper(getEnrollmentSummary);
		}

		public bool IsGetEventInformation { get { return this.Tag == Tags.GetEventInformation; } }

		public Error AsGetEventInformation { get { return ((GetEventInformationWrapper)this).Item; } }

		public static ServiceError NewGetEventInformation(Error getEventInformation)
		{
			return new GetEventInformationWrapper(getEventInformation);
		}

		public bool IsSubscribeCOV { get { return this.Tag == Tags.SubscribeCOV; } }

		public Error AsSubscribeCOV { get { return ((SubscribeCOVWrapper)this).Item; } }

		public static ServiceError NewSubscribeCOV(Error subscribeCOV)
		{
			return new SubscribeCOVWrapper(subscribeCOV);
		}

		public bool IsSubscribeCOVProperty { get { return this.Tag == Tags.SubscribeCOVProperty; } }

		public Error AsSubscribeCOVProperty { get { return ((SubscribeCOVPropertyWrapper)this).Item; } }

		public static ServiceError NewSubscribeCOVProperty(Error subscribeCOVProperty)
		{
			return new SubscribeCOVPropertyWrapper(subscribeCOVProperty);
		}

		public bool IsLifeSafetyOperation { get { return this.Tag == Tags.LifeSafetyOperation; } }

		public Error AsLifeSafetyOperation { get { return ((LifeSafetyOperationWrapper)this).Item; } }

		public static ServiceError NewLifeSafetyOperation(Error lifeSafetyOperation)
		{
			return new LifeSafetyOperationWrapper(lifeSafetyOperation);
		}

		public bool IsAtomicReadFile { get { return this.Tag == Tags.AtomicReadFile; } }

		public Error AsAtomicReadFile { get { return ((AtomicReadFileWrapper)this).Item; } }

		public static ServiceError NewAtomicReadFile(Error atomicReadFile)
		{
			return new AtomicReadFileWrapper(atomicReadFile);
		}

		public bool IsAtomicWriteFile { get { return this.Tag == Tags.AtomicWriteFile; } }

		public Error AsAtomicWriteFile { get { return ((AtomicWriteFileWrapper)this).Item; } }

		public static ServiceError NewAtomicWriteFile(Error atomicWriteFile)
		{
			return new AtomicWriteFileWrapper(atomicWriteFile);
		}

		public bool IsAddListElement { get { return this.Tag == Tags.AddListElement; } }

		public ChangeListError AsAddListElement { get { return ((AddListElementWrapper)this).Item; } }

		public static ServiceError NewAddListElement(ChangeListError addListElement)
		{
			return new AddListElementWrapper(addListElement);
		}

		public bool IsRemoveListElement { get { return this.Tag == Tags.RemoveListElement; } }

		public ChangeListError AsRemoveListElement { get { return ((RemoveListElementWrapper)this).Item; } }

		public static ServiceError NewRemoveListElement(ChangeListError removeListElement)
		{
			return new RemoveListElementWrapper(removeListElement);
		}

		public bool IsCreateObject { get { return this.Tag == Tags.CreateObject; } }

		public CreateObjectError AsCreateObject { get { return ((CreateObjectWrapper)this).Item; } }

		public static ServiceError NewCreateObject(CreateObjectError createObject)
		{
			return new CreateObjectWrapper(createObject);
		}

		public bool IsDeleteObject { get { return this.Tag == Tags.DeleteObject; } }

		public Error AsDeleteObject { get { return ((DeleteObjectWrapper)this).Item; } }

		public static ServiceError NewDeleteObject(Error deleteObject)
		{
			return new DeleteObjectWrapper(deleteObject);
		}

		public bool IsReadProperty { get { return this.Tag == Tags.ReadProperty; } }

		public Error AsReadProperty { get { return ((ReadPropertyWrapper)this).Item; } }

		public static ServiceError NewReadProperty(Error readProperty)
		{
			return new ReadPropertyWrapper(readProperty);
		}

		public bool IsReadPropertyConditional { get { return this.Tag == Tags.ReadPropertyConditional; } }

		public Error AsReadPropertyConditional { get { return ((ReadPropertyConditionalWrapper)this).Item; } }

		public static ServiceError NewReadPropertyConditional(Error readPropertyConditional)
		{
			return new ReadPropertyConditionalWrapper(readPropertyConditional);
		}

		public bool IsReadPropertyMultiple { get { return this.Tag == Tags.ReadPropertyMultiple; } }

		public Error AsReadPropertyMultiple { get { return ((ReadPropertyMultipleWrapper)this).Item; } }

		public static ServiceError NewReadPropertyMultiple(Error readPropertyMultiple)
		{
			return new ReadPropertyMultipleWrapper(readPropertyMultiple);
		}

		public bool IsReadRange { get { return this.Tag == Tags.ReadRange; } }

		public Error AsReadRange { get { return ((ReadRangeWrapper)this).Item; } }

		public static ServiceError NewReadRange(Error readRange)
		{
			return new ReadRangeWrapper(readRange);
		}

		public bool IsWriteProperty { get { return this.Tag == Tags.WriteProperty; } }

		public Error AsWriteProperty { get { return ((WritePropertyWrapper)this).Item; } }

		public static ServiceError NewWriteProperty(Error writeProperty)
		{
			return new WritePropertyWrapper(writeProperty);
		}

		public bool IsWritePropertyMultiple { get { return this.Tag == Tags.WritePropertyMultiple; } }

		public WritePropertyMultipleError AsWritePropertyMultiple { get { return ((WritePropertyMultipleWrapper)this).Item; } }

		public static ServiceError NewWritePropertyMultiple(WritePropertyMultipleError writePropertyMultiple)
		{
			return new WritePropertyMultipleWrapper(writePropertyMultiple);
		}

		public bool IsDeviceCommunicationControl { get { return this.Tag == Tags.DeviceCommunicationControl; } }

		public Error AsDeviceCommunicationControl { get { return ((DeviceCommunicationControlWrapper)this).Item; } }

		public static ServiceError NewDeviceCommunicationControl(Error deviceCommunicationControl)
		{
			return new DeviceCommunicationControlWrapper(deviceCommunicationControl);
		}

		public bool IsConfirmedPrivateTransfer { get { return this.Tag == Tags.ConfirmedPrivateTransfer; } }

		public ConfirmedPrivateTransferError AsConfirmedPrivateTransfer { get { return ((ConfirmedPrivateTransferWrapper)this).Item; } }

		public static ServiceError NewConfirmedPrivateTransfer(ConfirmedPrivateTransferError confirmedPrivateTransfer)
		{
			return new ConfirmedPrivateTransferWrapper(confirmedPrivateTransfer);
		}

		public bool IsConfirmedTextMessage { get { return this.Tag == Tags.ConfirmedTextMessage; } }

		public Error AsConfirmedTextMessage { get { return ((ConfirmedTextMessageWrapper)this).Item; } }

		public static ServiceError NewConfirmedTextMessage(Error confirmedTextMessage)
		{
			return new ConfirmedTextMessageWrapper(confirmedTextMessage);
		}

		public bool IsReinitializeDevice { get { return this.Tag == Tags.ReinitializeDevice; } }

		public Error AsReinitializeDevice { get { return ((ReinitializeDeviceWrapper)this).Item; } }

		public static ServiceError NewReinitializeDevice(Error reinitializeDevice)
		{
			return new ReinitializeDeviceWrapper(reinitializeDevice);
		}

		public bool IsVtOpen { get { return this.Tag == Tags.VtOpen; } }

		public Error AsVtOpen { get { return ((VtOpenWrapper)this).Item; } }

		public static ServiceError NewVtOpen(Error vtOpen)
		{
			return new VtOpenWrapper(vtOpen);
		}

		public bool IsVtClose { get { return this.Tag == Tags.VtClose; } }

		public VTCloseError AsVtClose { get { return ((VtCloseWrapper)this).Item; } }

		public static ServiceError NewVtClose(VTCloseError vtClose)
		{
			return new VtCloseWrapper(vtClose);
		}

		public bool IsVtData { get { return this.Tag == Tags.VtData; } }

		public Error AsVtData { get { return ((VtDataWrapper)this).Item; } }

		public static ServiceError NewVtData(Error vtData)
		{
			return new VtDataWrapper(vtData);
		}

		public bool IsAuthenticate { get { return this.Tag == Tags.Authenticate; } }

		public Error AsAuthenticate { get { return ((AuthenticateWrapper)this).Item; } }

		public static ServiceError NewAuthenticate(Error authenticate)
		{
			return new AuthenticateWrapper(authenticate);
		}

		public bool IsRequestKey { get { return this.Tag == Tags.RequestKey; } }

		public Error AsRequestKey { get { return ((RequestKeyWrapper)this).Item; } }

		public static ServiceError NewRequestKey(Error requestKey)
		{
			return new RequestKeyWrapper(requestKey);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("Other", 127, Value<Error>.Schema),
			new FieldSchema("AcknowledgeAlarm", 0, Value<Error>.Schema),
			new FieldSchema("ConfirmedCOVNotification", 1, Value<Error>.Schema),
			new FieldSchema("ConfirmedEventNotification", 2, Value<Error>.Schema),
			new FieldSchema("GetAlarmSummary", 3, Value<Error>.Schema),
			new FieldSchema("GetEnrollmentSummary", 4, Value<Error>.Schema),
			new FieldSchema("GetEventInformation", 29, Value<Error>.Schema),
			new FieldSchema("SubscribeCOV", 5, Value<Error>.Schema),
			new FieldSchema("SubscribeCOVProperty", 28, Value<Error>.Schema),
			new FieldSchema("LifeSafetyOperation", 27, Value<Error>.Schema),
			new FieldSchema("AtomicReadFile", 6, Value<Error>.Schema),
			new FieldSchema("AtomicWriteFile", 7, Value<Error>.Schema),
			new FieldSchema("AddListElement", 8, Value<ChangeListError>.Schema),
			new FieldSchema("RemoveListElement", 9, Value<ChangeListError>.Schema),
			new FieldSchema("CreateObject", 10, Value<CreateObjectError>.Schema),
			new FieldSchema("DeleteObject", 11, Value<Error>.Schema),
			new FieldSchema("ReadProperty", 12, Value<Error>.Schema),
			new FieldSchema("ReadPropertyConditional", 13, Value<Error>.Schema),
			new FieldSchema("ReadPropertyMultiple", 14, Value<Error>.Schema),
			new FieldSchema("ReadRange", 26, Value<Error>.Schema),
			new FieldSchema("WriteProperty", 15, Value<Error>.Schema),
			new FieldSchema("WritePropertyMultiple", 16, Value<WritePropertyMultipleError>.Schema),
			new FieldSchema("DeviceCommunicationControl", 17, Value<Error>.Schema),
			new FieldSchema("ConfirmedPrivateTransfer", 18, Value<ConfirmedPrivateTransferError>.Schema),
			new FieldSchema("ConfirmedTextMessage", 19, Value<Error>.Schema),
			new FieldSchema("ReinitializeDevice", 20, Value<Error>.Schema),
			new FieldSchema("VtOpen", 21, Value<Error>.Schema),
			new FieldSchema("VtClose", 22, Value<VTCloseError>.Schema),
			new FieldSchema("VtData", 23, Value<Error>.Schema),
			new FieldSchema("Authenticate", 24, Value<Error>.Schema),
			new FieldSchema("RequestKey", 25, Value<Error>.Schema));

		public static ServiceError Load(IValueStream stream)
		{
			ServiceError ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.Other:
					ret = Value<OtherWrapper>.Load(stream);
					break;
				case Tags.AcknowledgeAlarm:
					ret = Value<AcknowledgeAlarmWrapper>.Load(stream);
					break;
				case Tags.ConfirmedCOVNotification:
					ret = Value<ConfirmedCOVNotificationWrapper>.Load(stream);
					break;
				case Tags.ConfirmedEventNotification:
					ret = Value<ConfirmedEventNotificationWrapper>.Load(stream);
					break;
				case Tags.GetAlarmSummary:
					ret = Value<GetAlarmSummaryWrapper>.Load(stream);
					break;
				case Tags.GetEnrollmentSummary:
					ret = Value<GetEnrollmentSummaryWrapper>.Load(stream);
					break;
				case Tags.GetEventInformation:
					ret = Value<GetEventInformationWrapper>.Load(stream);
					break;
				case Tags.SubscribeCOV:
					ret = Value<SubscribeCOVWrapper>.Load(stream);
					break;
				case Tags.SubscribeCOVProperty:
					ret = Value<SubscribeCOVPropertyWrapper>.Load(stream);
					break;
				case Tags.LifeSafetyOperation:
					ret = Value<LifeSafetyOperationWrapper>.Load(stream);
					break;
				case Tags.AtomicReadFile:
					ret = Value<AtomicReadFileWrapper>.Load(stream);
					break;
				case Tags.AtomicWriteFile:
					ret = Value<AtomicWriteFileWrapper>.Load(stream);
					break;
				case Tags.AddListElement:
					ret = Value<AddListElementWrapper>.Load(stream);
					break;
				case Tags.RemoveListElement:
					ret = Value<RemoveListElementWrapper>.Load(stream);
					break;
				case Tags.CreateObject:
					ret = Value<CreateObjectWrapper>.Load(stream);
					break;
				case Tags.DeleteObject:
					ret = Value<DeleteObjectWrapper>.Load(stream);
					break;
				case Tags.ReadProperty:
					ret = Value<ReadPropertyWrapper>.Load(stream);
					break;
				case Tags.ReadPropertyConditional:
					ret = Value<ReadPropertyConditionalWrapper>.Load(stream);
					break;
				case Tags.ReadPropertyMultiple:
					ret = Value<ReadPropertyMultipleWrapper>.Load(stream);
					break;
				case Tags.ReadRange:
					ret = Value<ReadRangeWrapper>.Load(stream);
					break;
				case Tags.WriteProperty:
					ret = Value<WritePropertyWrapper>.Load(stream);
					break;
				case Tags.WritePropertyMultiple:
					ret = Value<WritePropertyMultipleWrapper>.Load(stream);
					break;
				case Tags.DeviceCommunicationControl:
					ret = Value<DeviceCommunicationControlWrapper>.Load(stream);
					break;
				case Tags.ConfirmedPrivateTransfer:
					ret = Value<ConfirmedPrivateTransferWrapper>.Load(stream);
					break;
				case Tags.ConfirmedTextMessage:
					ret = Value<ConfirmedTextMessageWrapper>.Load(stream);
					break;
				case Tags.ReinitializeDevice:
					ret = Value<ReinitializeDeviceWrapper>.Load(stream);
					break;
				case Tags.VtOpen:
					ret = Value<VtOpenWrapper>.Load(stream);
					break;
				case Tags.VtClose:
					ret = Value<VtCloseWrapper>.Load(stream);
					break;
				case Tags.VtData:
					ret = Value<VtDataWrapper>.Load(stream);
					break;
				case Tags.Authenticate:
					ret = Value<AuthenticateWrapper>.Load(stream);
					break;
				case Tags.RequestKey:
					ret = Value<RequestKeyWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, ServiceError value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.Other:
					Value<OtherWrapper>.Save(sink, (OtherWrapper)value);
					break;
				case Tags.AcknowledgeAlarm:
					Value<AcknowledgeAlarmWrapper>.Save(sink, (AcknowledgeAlarmWrapper)value);
					break;
				case Tags.ConfirmedCOVNotification:
					Value<ConfirmedCOVNotificationWrapper>.Save(sink, (ConfirmedCOVNotificationWrapper)value);
					break;
				case Tags.ConfirmedEventNotification:
					Value<ConfirmedEventNotificationWrapper>.Save(sink, (ConfirmedEventNotificationWrapper)value);
					break;
				case Tags.GetAlarmSummary:
					Value<GetAlarmSummaryWrapper>.Save(sink, (GetAlarmSummaryWrapper)value);
					break;
				case Tags.GetEnrollmentSummary:
					Value<GetEnrollmentSummaryWrapper>.Save(sink, (GetEnrollmentSummaryWrapper)value);
					break;
				case Tags.GetEventInformation:
					Value<GetEventInformationWrapper>.Save(sink, (GetEventInformationWrapper)value);
					break;
				case Tags.SubscribeCOV:
					Value<SubscribeCOVWrapper>.Save(sink, (SubscribeCOVWrapper)value);
					break;
				case Tags.SubscribeCOVProperty:
					Value<SubscribeCOVPropertyWrapper>.Save(sink, (SubscribeCOVPropertyWrapper)value);
					break;
				case Tags.LifeSafetyOperation:
					Value<LifeSafetyOperationWrapper>.Save(sink, (LifeSafetyOperationWrapper)value);
					break;
				case Tags.AtomicReadFile:
					Value<AtomicReadFileWrapper>.Save(sink, (AtomicReadFileWrapper)value);
					break;
				case Tags.AtomicWriteFile:
					Value<AtomicWriteFileWrapper>.Save(sink, (AtomicWriteFileWrapper)value);
					break;
				case Tags.AddListElement:
					Value<AddListElementWrapper>.Save(sink, (AddListElementWrapper)value);
					break;
				case Tags.RemoveListElement:
					Value<RemoveListElementWrapper>.Save(sink, (RemoveListElementWrapper)value);
					break;
				case Tags.CreateObject:
					Value<CreateObjectWrapper>.Save(sink, (CreateObjectWrapper)value);
					break;
				case Tags.DeleteObject:
					Value<DeleteObjectWrapper>.Save(sink, (DeleteObjectWrapper)value);
					break;
				case Tags.ReadProperty:
					Value<ReadPropertyWrapper>.Save(sink, (ReadPropertyWrapper)value);
					break;
				case Tags.ReadPropertyConditional:
					Value<ReadPropertyConditionalWrapper>.Save(sink, (ReadPropertyConditionalWrapper)value);
					break;
				case Tags.ReadPropertyMultiple:
					Value<ReadPropertyMultipleWrapper>.Save(sink, (ReadPropertyMultipleWrapper)value);
					break;
				case Tags.ReadRange:
					Value<ReadRangeWrapper>.Save(sink, (ReadRangeWrapper)value);
					break;
				case Tags.WriteProperty:
					Value<WritePropertyWrapper>.Save(sink, (WritePropertyWrapper)value);
					break;
				case Tags.WritePropertyMultiple:
					Value<WritePropertyMultipleWrapper>.Save(sink, (WritePropertyMultipleWrapper)value);
					break;
				case Tags.DeviceCommunicationControl:
					Value<DeviceCommunicationControlWrapper>.Save(sink, (DeviceCommunicationControlWrapper)value);
					break;
				case Tags.ConfirmedPrivateTransfer:
					Value<ConfirmedPrivateTransferWrapper>.Save(sink, (ConfirmedPrivateTransferWrapper)value);
					break;
				case Tags.ConfirmedTextMessage:
					Value<ConfirmedTextMessageWrapper>.Save(sink, (ConfirmedTextMessageWrapper)value);
					break;
				case Tags.ReinitializeDevice:
					Value<ReinitializeDeviceWrapper>.Save(sink, (ReinitializeDeviceWrapper)value);
					break;
				case Tags.VtOpen:
					Value<VtOpenWrapper>.Save(sink, (VtOpenWrapper)value);
					break;
				case Tags.VtClose:
					Value<VtCloseWrapper>.Save(sink, (VtCloseWrapper)value);
					break;
				case Tags.VtData:
					Value<VtDataWrapper>.Save(sink, (VtDataWrapper)value);
					break;
				case Tags.Authenticate:
					Value<AuthenticateWrapper>.Save(sink, (AuthenticateWrapper)value);
					break;
				case Tags.RequestKey:
					Value<RequestKeyWrapper>.Save(sink, (RequestKeyWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			Other = 0,
			AcknowledgeAlarm = 1,
			ConfirmedCOVNotification = 2,
			ConfirmedEventNotification = 3,
			GetAlarmSummary = 4,
			GetEnrollmentSummary = 5,
			GetEventInformation = 6,
			SubscribeCOV = 7,
			SubscribeCOVProperty = 8,
			LifeSafetyOperation = 9,
			AtomicReadFile = 10,
			AtomicWriteFile = 11,
			AddListElement = 12,
			RemoveListElement = 13,
			CreateObject = 14,
			DeleteObject = 15,
			ReadProperty = 16,
			ReadPropertyConditional = 17,
			ReadPropertyMultiple = 18,
			ReadRange = 19,
			WriteProperty = 20,
			WritePropertyMultiple = 21,
			DeviceCommunicationControl = 22,
			ConfirmedPrivateTransfer = 23,
			ConfirmedTextMessage = 24,
			ReinitializeDevice = 25,
			VtOpen = 26,
			VtClose = 27,
			VtData = 28,
			Authenticate = 29,
			RequestKey = 30
		}

		public  partial class OtherWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.Other; } }

			public Error Item { get; private set; }

			public OtherWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new OtherWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new OtherWrapper(temp);
			}

			public static void Save(IValueSink sink, OtherWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class AcknowledgeAlarmWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.AcknowledgeAlarm; } }

			public Error Item { get; private set; }

			public AcknowledgeAlarmWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new AcknowledgeAlarmWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new AcknowledgeAlarmWrapper(temp);
			}

			public static void Save(IValueSink sink, AcknowledgeAlarmWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedCOVNotificationWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ConfirmedCOVNotification; } }

			public Error Item { get; private set; }

			public ConfirmedCOVNotificationWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ConfirmedCOVNotificationWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ConfirmedCOVNotificationWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedCOVNotificationWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedEventNotificationWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ConfirmedEventNotification; } }

			public Error Item { get; private set; }

			public ConfirmedEventNotificationWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ConfirmedEventNotificationWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ConfirmedEventNotificationWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedEventNotificationWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class GetAlarmSummaryWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.GetAlarmSummary; } }

			public Error Item { get; private set; }

			public GetAlarmSummaryWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new GetAlarmSummaryWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new GetAlarmSummaryWrapper(temp);
			}

			public static void Save(IValueSink sink, GetAlarmSummaryWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class GetEnrollmentSummaryWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.GetEnrollmentSummary; } }

			public Error Item { get; private set; }

			public GetEnrollmentSummaryWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new GetEnrollmentSummaryWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new GetEnrollmentSummaryWrapper(temp);
			}

			public static void Save(IValueSink sink, GetEnrollmentSummaryWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class GetEventInformationWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.GetEventInformation; } }

			public Error Item { get; private set; }

			public GetEventInformationWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new GetEventInformationWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new GetEventInformationWrapper(temp);
			}

			public static void Save(IValueSink sink, GetEventInformationWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class SubscribeCOVWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.SubscribeCOV; } }

			public Error Item { get; private set; }

			public SubscribeCOVWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new SubscribeCOVWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new SubscribeCOVWrapper(temp);
			}

			public static void Save(IValueSink sink, SubscribeCOVWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class SubscribeCOVPropertyWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.SubscribeCOVProperty; } }

			public Error Item { get; private set; }

			public SubscribeCOVPropertyWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new SubscribeCOVPropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new SubscribeCOVPropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, SubscribeCOVPropertyWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class LifeSafetyOperationWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.LifeSafetyOperation; } }

			public Error Item { get; private set; }

			public LifeSafetyOperationWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new LifeSafetyOperationWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new LifeSafetyOperationWrapper(temp);
			}

			public static void Save(IValueSink sink, LifeSafetyOperationWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class AtomicReadFileWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.AtomicReadFile; } }

			public Error Item { get; private set; }

			public AtomicReadFileWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new AtomicReadFileWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new AtomicReadFileWrapper(temp);
			}

			public static void Save(IValueSink sink, AtomicReadFileWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class AtomicWriteFileWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.AtomicWriteFile; } }

			public Error Item { get; private set; }

			public AtomicWriteFileWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new AtomicWriteFileWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new AtomicWriteFileWrapper(temp);
			}

			public static void Save(IValueSink sink, AtomicWriteFileWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class AddListElementWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.AddListElement; } }

			public ChangeListError Item { get; private set; }

			public AddListElementWrapper(ChangeListError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ChangeListError>.Schema;

			public static new AddListElementWrapper Load(IValueStream stream)
			{
				var temp = Value<ChangeListError>.Load(stream);
				return new AddListElementWrapper(temp);
			}

			public static void Save(IValueSink sink, AddListElementWrapper value)
			{
				Value<ChangeListError>.Save(sink, value.Item);
			}

		}

		public  partial class RemoveListElementWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.RemoveListElement; } }

			public ChangeListError Item { get; private set; }

			public RemoveListElementWrapper(ChangeListError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ChangeListError>.Schema;

			public static new RemoveListElementWrapper Load(IValueStream stream)
			{
				var temp = Value<ChangeListError>.Load(stream);
				return new RemoveListElementWrapper(temp);
			}

			public static void Save(IValueSink sink, RemoveListElementWrapper value)
			{
				Value<ChangeListError>.Save(sink, value.Item);
			}

		}

		public  partial class CreateObjectWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.CreateObject; } }

			public CreateObjectError Item { get; private set; }

			public CreateObjectWrapper(CreateObjectError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<CreateObjectError>.Schema;

			public static new CreateObjectWrapper Load(IValueStream stream)
			{
				var temp = Value<CreateObjectError>.Load(stream);
				return new CreateObjectWrapper(temp);
			}

			public static void Save(IValueSink sink, CreateObjectWrapper value)
			{
				Value<CreateObjectError>.Save(sink, value.Item);
			}

		}

		public  partial class DeleteObjectWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.DeleteObject; } }

			public Error Item { get; private set; }

			public DeleteObjectWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new DeleteObjectWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new DeleteObjectWrapper(temp);
			}

			public static void Save(IValueSink sink, DeleteObjectWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ReadProperty; } }

			public Error Item { get; private set; }

			public ReadPropertyWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ReadPropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ReadPropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyConditionalWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ReadPropertyConditional; } }

			public Error Item { get; private set; }

			public ReadPropertyConditionalWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ReadPropertyConditionalWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ReadPropertyConditionalWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyConditionalWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyMultipleWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ReadPropertyMultiple; } }

			public Error Item { get; private set; }

			public ReadPropertyMultipleWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ReadPropertyMultipleWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ReadPropertyMultipleWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyMultipleWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ReadRangeWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ReadRange; } }

			public Error Item { get; private set; }

			public ReadRangeWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ReadRangeWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ReadRangeWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadRangeWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class WritePropertyWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.WriteProperty; } }

			public Error Item { get; private set; }

			public WritePropertyWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new WritePropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new WritePropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, WritePropertyWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class WritePropertyMultipleWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.WritePropertyMultiple; } }

			public WritePropertyMultipleError Item { get; private set; }

			public WritePropertyMultipleWrapper(WritePropertyMultipleError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<WritePropertyMultipleError>.Schema;

			public static new WritePropertyMultipleWrapper Load(IValueStream stream)
			{
				var temp = Value<WritePropertyMultipleError>.Load(stream);
				return new WritePropertyMultipleWrapper(temp);
			}

			public static void Save(IValueSink sink, WritePropertyMultipleWrapper value)
			{
				Value<WritePropertyMultipleError>.Save(sink, value.Item);
			}

		}

		public  partial class DeviceCommunicationControlWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.DeviceCommunicationControl; } }

			public Error Item { get; private set; }

			public DeviceCommunicationControlWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new DeviceCommunicationControlWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new DeviceCommunicationControlWrapper(temp);
			}

			public static void Save(IValueSink sink, DeviceCommunicationControlWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedPrivateTransferWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ConfirmedPrivateTransfer; } }

			public ConfirmedPrivateTransferError Item { get; private set; }

			public ConfirmedPrivateTransferWrapper(ConfirmedPrivateTransferError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ConfirmedPrivateTransferError>.Schema;

			public static new ConfirmedPrivateTransferWrapper Load(IValueStream stream)
			{
				var temp = Value<ConfirmedPrivateTransferError>.Load(stream);
				return new ConfirmedPrivateTransferWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedPrivateTransferWrapper value)
			{
				Value<ConfirmedPrivateTransferError>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedTextMessageWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ConfirmedTextMessage; } }

			public Error Item { get; private set; }

			public ConfirmedTextMessageWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ConfirmedTextMessageWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ConfirmedTextMessageWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedTextMessageWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class ReinitializeDeviceWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.ReinitializeDevice; } }

			public Error Item { get; private set; }

			public ReinitializeDeviceWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new ReinitializeDeviceWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new ReinitializeDeviceWrapper(temp);
			}

			public static void Save(IValueSink sink, ReinitializeDeviceWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class VtOpenWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.VtOpen; } }

			public Error Item { get; private set; }

			public VtOpenWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new VtOpenWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new VtOpenWrapper(temp);
			}

			public static void Save(IValueSink sink, VtOpenWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class VtCloseWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.VtClose; } }

			public VTCloseError Item { get; private set; }

			public VtCloseWrapper(VTCloseError item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<VTCloseError>.Schema;

			public static new VtCloseWrapper Load(IValueStream stream)
			{
				var temp = Value<VTCloseError>.Load(stream);
				return new VtCloseWrapper(temp);
			}

			public static void Save(IValueSink sink, VtCloseWrapper value)
			{
				Value<VTCloseError>.Save(sink, value.Item);
			}

		}

		public  partial class VtDataWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.VtData; } }

			public Error Item { get; private set; }

			public VtDataWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new VtDataWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new VtDataWrapper(temp);
			}

			public static void Save(IValueSink sink, VtDataWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class AuthenticateWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.Authenticate; } }

			public Error Item { get; private set; }

			public AuthenticateWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new AuthenticateWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new AuthenticateWrapper(temp);
			}

			public static void Save(IValueSink sink, AuthenticateWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}

		public  partial class RequestKeyWrapper : ServiceError
		{
			public override Tags Tag { get { return Tags.RequestKey; } }

			public Error Item { get; private set; }

			public RequestKeyWrapper(Error item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<Error>.Schema;

			public static new RequestKeyWrapper Load(IValueStream stream)
			{
				var temp = Value<Error>.Load(stream);
				return new RequestKeyWrapper(temp);
			}

			public static void Save(IValueSink sink, RequestKeyWrapper value)
			{
				Value<Error>.Save(sink, value.Item);
			}

		}
	}
}
