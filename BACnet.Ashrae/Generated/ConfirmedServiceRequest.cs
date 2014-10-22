using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class ConfirmedServiceRequest
	{
		public abstract Tags Tag { get; }

		public bool IsAcknowledgeAlarm { get { return this.Tag == Tags.AcknowledgeAlarm; } }

		public AcknowledgeAlarmRequest AsAcknowledgeAlarm { get { return ((AcknowledgeAlarmWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewAcknowledgeAlarm(AcknowledgeAlarmRequest acknowledgeAlarm)
		{
			return new AcknowledgeAlarmWrapper(acknowledgeAlarm);
		}

		public bool IsConfirmedCOVNotification { get { return this.Tag == Tags.ConfirmedCOVNotification; } }

		public ConfirmedCOVNotificationRequest AsConfirmedCOVNotification { get { return ((ConfirmedCOVNotificationWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewConfirmedCOVNotification(ConfirmedCOVNotificationRequest confirmedCOVNotification)
		{
			return new ConfirmedCOVNotificationWrapper(confirmedCOVNotification);
		}

		public bool IsConfirmedEventNotification { get { return this.Tag == Tags.ConfirmedEventNotification; } }

		public ConfirmedEventNotificationRequest AsConfirmedEventNotification { get { return ((ConfirmedEventNotificationWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewConfirmedEventNotification(ConfirmedEventNotificationRequest confirmedEventNotification)
		{
			return new ConfirmedEventNotificationWrapper(confirmedEventNotification);
		}

		public bool IsGetEnrollmentSummary { get { return this.Tag == Tags.GetEnrollmentSummary; } }

		public GetEnrollmentSummaryRequest AsGetEnrollmentSummary { get { return ((GetEnrollmentSummaryWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewGetEnrollmentSummary(GetEnrollmentSummaryRequest getEnrollmentSummary)
		{
			return new GetEnrollmentSummaryWrapper(getEnrollmentSummary);
		}

		public bool IsGetEventInformation { get { return this.Tag == Tags.GetEventInformation; } }

		public GetEventInformationRequest AsGetEventInformation { get { return ((GetEventInformationWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewGetEventInformation(GetEventInformationRequest getEventInformation)
		{
			return new GetEventInformationWrapper(getEventInformation);
		}

		public bool IsSubscribeCOV { get { return this.Tag == Tags.SubscribeCOV; } }

		public SubscribeCOVRequest AsSubscribeCOV { get { return ((SubscribeCOVWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewSubscribeCOV(SubscribeCOVRequest subscribeCOV)
		{
			return new SubscribeCOVWrapper(subscribeCOV);
		}

		public bool IsSubscribeCOVProperty { get { return this.Tag == Tags.SubscribeCOVProperty; } }

		public SubscribeCOVPropertyRequest AsSubscribeCOVProperty { get { return ((SubscribeCOVPropertyWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewSubscribeCOVProperty(SubscribeCOVPropertyRequest subscribeCOVProperty)
		{
			return new SubscribeCOVPropertyWrapper(subscribeCOVProperty);
		}

		public bool IsLifeSafetyOperation { get { return this.Tag == Tags.LifeSafetyOperation; } }

		public LifeSafetyOperationRequest AsLifeSafetyOperation { get { return ((LifeSafetyOperationWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewLifeSafetyOperation(LifeSafetyOperationRequest lifeSafetyOperation)
		{
			return new LifeSafetyOperationWrapper(lifeSafetyOperation);
		}

		public bool IsAtomicReadFile { get { return this.Tag == Tags.AtomicReadFile; } }

		public AtomicReadFileRequest AsAtomicReadFile { get { return ((AtomicReadFileWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewAtomicReadFile(AtomicReadFileRequest atomicReadFile)
		{
			return new AtomicReadFileWrapper(atomicReadFile);
		}

		public bool IsAtomicWriteFile { get { return this.Tag == Tags.AtomicWriteFile; } }

		public AtomicWriteFileRequest AsAtomicWriteFile { get { return ((AtomicWriteFileWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewAtomicWriteFile(AtomicWriteFileRequest atomicWriteFile)
		{
			return new AtomicWriteFileWrapper(atomicWriteFile);
		}

		public bool IsAddListElement { get { return this.Tag == Tags.AddListElement; } }

		public AddListElementRequest AsAddListElement { get { return ((AddListElementWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewAddListElement(AddListElementRequest addListElement)
		{
			return new AddListElementWrapper(addListElement);
		}

		public bool IsRemoveListElement { get { return this.Tag == Tags.RemoveListElement; } }

		public RemoveListElementRequest AsRemoveListElement { get { return ((RemoveListElementWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewRemoveListElement(RemoveListElementRequest removeListElement)
		{
			return new RemoveListElementWrapper(removeListElement);
		}

		public bool IsCreateObject { get { return this.Tag == Tags.CreateObject; } }

		public CreateObjectRequest AsCreateObject { get { return ((CreateObjectWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewCreateObject(CreateObjectRequest createObject)
		{
			return new CreateObjectWrapper(createObject);
		}

		public bool IsDeleteObject { get { return this.Tag == Tags.DeleteObject; } }

		public DeleteObjectRequest AsDeleteObject { get { return ((DeleteObjectWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewDeleteObject(DeleteObjectRequest deleteObject)
		{
			return new DeleteObjectWrapper(deleteObject);
		}

		public bool IsReadProperty { get { return this.Tag == Tags.ReadProperty; } }

		public ReadPropertyRequest AsReadProperty { get { return ((ReadPropertyWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewReadProperty(ReadPropertyRequest readProperty)
		{
			return new ReadPropertyWrapper(readProperty);
		}

		public bool IsReadPropertyConditional { get { return this.Tag == Tags.ReadPropertyConditional; } }

		public ReadPropertyConditionalRequest AsReadPropertyConditional { get { return ((ReadPropertyConditionalWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewReadPropertyConditional(ReadPropertyConditionalRequest readPropertyConditional)
		{
			return new ReadPropertyConditionalWrapper(readPropertyConditional);
		}

		public bool IsReadPropertyMultiple { get { return this.Tag == Tags.ReadPropertyMultiple; } }

		public ReadPropertyMultipleRequest AsReadPropertyMultiple { get { return ((ReadPropertyMultipleWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewReadPropertyMultiple(ReadPropertyMultipleRequest readPropertyMultiple)
		{
			return new ReadPropertyMultipleWrapper(readPropertyMultiple);
		}

		public bool IsReadRange { get { return this.Tag == Tags.ReadRange; } }

		public ReadRangeRequest AsReadRange { get { return ((ReadRangeWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewReadRange(ReadRangeRequest readRange)
		{
			return new ReadRangeWrapper(readRange);
		}

		public bool IsWriteProperty { get { return this.Tag == Tags.WriteProperty; } }

		public WritePropertyRequest AsWriteProperty { get { return ((WritePropertyWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewWriteProperty(WritePropertyRequest writeProperty)
		{
			return new WritePropertyWrapper(writeProperty);
		}

		public bool IsWritePropertyMultiple { get { return this.Tag == Tags.WritePropertyMultiple; } }

		public WritePropertyMultipleRequest AsWritePropertyMultiple { get { return ((WritePropertyMultipleWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewWritePropertyMultiple(WritePropertyMultipleRequest writePropertyMultiple)
		{
			return new WritePropertyMultipleWrapper(writePropertyMultiple);
		}

		public bool IsDeviceCommunicationControl { get { return this.Tag == Tags.DeviceCommunicationControl; } }

		public DeviceCommunicationControlRequest AsDeviceCommunicationControl { get { return ((DeviceCommunicationControlWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewDeviceCommunicationControl(DeviceCommunicationControlRequest deviceCommunicationControl)
		{
			return new DeviceCommunicationControlWrapper(deviceCommunicationControl);
		}

		public bool IsConfirmedPrivateTransfer { get { return this.Tag == Tags.ConfirmedPrivateTransfer; } }

		public ConfirmedPrivateTransferRequest AsConfirmedPrivateTransfer { get { return ((ConfirmedPrivateTransferWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewConfirmedPrivateTransfer(ConfirmedPrivateTransferRequest confirmedPrivateTransfer)
		{
			return new ConfirmedPrivateTransferWrapper(confirmedPrivateTransfer);
		}

		public bool IsConfirmedTextMessage { get { return this.Tag == Tags.ConfirmedTextMessage; } }

		public ConfirmedTextMessageRequest AsConfirmedTextMessage { get { return ((ConfirmedTextMessageWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewConfirmedTextMessage(ConfirmedTextMessageRequest confirmedTextMessage)
		{
			return new ConfirmedTextMessageWrapper(confirmedTextMessage);
		}

		public bool IsReinitializeDevice { get { return this.Tag == Tags.ReinitializeDevice; } }

		public ReinitializeDeviceRequest AsReinitializeDevice { get { return ((ReinitializeDeviceWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewReinitializeDevice(ReinitializeDeviceRequest reinitializeDevice)
		{
			return new ReinitializeDeviceWrapper(reinitializeDevice);
		}

		public bool IsVtOpen { get { return this.Tag == Tags.VtOpen; } }

		public VTOpenRequest AsVtOpen { get { return ((VtOpenWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewVtOpen(VTOpenRequest vtOpen)
		{
			return new VtOpenWrapper(vtOpen);
		}

		public bool IsVtClose { get { return this.Tag == Tags.VtClose; } }

		public VTCloseRequest AsVtClose { get { return ((VtCloseWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewVtClose(VTCloseRequest vtClose)
		{
			return new VtCloseWrapper(vtClose);
		}

		public bool IsVtData { get { return this.Tag == Tags.VtData; } }

		public VTDataRequest AsVtData { get { return ((VtDataWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewVtData(VTDataRequest vtData)
		{
			return new VtDataWrapper(vtData);
		}

		public bool IsAuthenticate { get { return this.Tag == Tags.Authenticate; } }

		public AuthenticateRequest AsAuthenticate { get { return ((AuthenticateWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewAuthenticate(AuthenticateRequest authenticate)
		{
			return new AuthenticateWrapper(authenticate);
		}

		public bool IsRequestKey { get { return this.Tag == Tags.RequestKey; } }

		public RequestKeyRequest AsRequestKey { get { return ((RequestKeyWrapper)this).Item; } }

		public static ConfirmedServiceRequest NewRequestKey(RequestKeyRequest requestKey)
		{
			return new RequestKeyWrapper(requestKey);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("AcknowledgeAlarm", 0, Value<AcknowledgeAlarmRequest>.Schema),
			new FieldSchema("ConfirmedCOVNotification", 1, Value<ConfirmedCOVNotificationRequest>.Schema),
			new FieldSchema("ConfirmedEventNotification", 2, Value<ConfirmedEventNotificationRequest>.Schema),
			new FieldSchema("GetEnrollmentSummary", 4, Value<GetEnrollmentSummaryRequest>.Schema),
			new FieldSchema("GetEventInformation", 29, Value<GetEventInformationRequest>.Schema),
			new FieldSchema("SubscribeCOV", 5, Value<SubscribeCOVRequest>.Schema),
			new FieldSchema("SubscribeCOVProperty", 28, Value<SubscribeCOVPropertyRequest>.Schema),
			new FieldSchema("LifeSafetyOperation", 27, Value<LifeSafetyOperationRequest>.Schema),
			new FieldSchema("AtomicReadFile", 6, Value<AtomicReadFileRequest>.Schema),
			new FieldSchema("AtomicWriteFile", 7, Value<AtomicWriteFileRequest>.Schema),
			new FieldSchema("AddListElement", 8, Value<AddListElementRequest>.Schema),
			new FieldSchema("RemoveListElement", 9, Value<RemoveListElementRequest>.Schema),
			new FieldSchema("CreateObject", 10, Value<CreateObjectRequest>.Schema),
			new FieldSchema("DeleteObject", 11, Value<DeleteObjectRequest>.Schema),
			new FieldSchema("ReadProperty", 12, Value<ReadPropertyRequest>.Schema),
			new FieldSchema("ReadPropertyConditional", 13, Value<ReadPropertyConditionalRequest>.Schema),
			new FieldSchema("ReadPropertyMultiple", 14, Value<ReadPropertyMultipleRequest>.Schema),
			new FieldSchema("ReadRange", 26, Value<ReadRangeRequest>.Schema),
			new FieldSchema("WriteProperty", 15, Value<WritePropertyRequest>.Schema),
			new FieldSchema("WritePropertyMultiple", 16, Value<WritePropertyMultipleRequest>.Schema),
			new FieldSchema("DeviceCommunicationControl", 17, Value<DeviceCommunicationControlRequest>.Schema),
			new FieldSchema("ConfirmedPrivateTransfer", 18, Value<ConfirmedPrivateTransferRequest>.Schema),
			new FieldSchema("ConfirmedTextMessage", 19, Value<ConfirmedTextMessageRequest>.Schema),
			new FieldSchema("ReinitializeDevice", 20, Value<ReinitializeDeviceRequest>.Schema),
			new FieldSchema("VtOpen", 21, Value<VTOpenRequest>.Schema),
			new FieldSchema("VtClose", 22, Value<VTCloseRequest>.Schema),
			new FieldSchema("VtData", 23, Value<VTDataRequest>.Schema),
			new FieldSchema("Authenticate", 24, Value<AuthenticateRequest>.Schema),
			new FieldSchema("RequestKey", 25, Value<RequestKeyRequest>.Schema));

		public static ConfirmedServiceRequest Load(IValueStream stream)
		{
			ConfirmedServiceRequest ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.AcknowledgeAlarm:
					ret = Value<AcknowledgeAlarmWrapper>.Load(stream);
					break;
				case Tags.ConfirmedCOVNotification:
					ret = Value<ConfirmedCOVNotificationWrapper>.Load(stream);
					break;
				case Tags.ConfirmedEventNotification:
					ret = Value<ConfirmedEventNotificationWrapper>.Load(stream);
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

		public static void Save(IValueSink sink, ConfirmedServiceRequest value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.AcknowledgeAlarm:
					Value<AcknowledgeAlarmWrapper>.Save(sink, (AcknowledgeAlarmWrapper)value);
					break;
				case Tags.ConfirmedCOVNotification:
					Value<ConfirmedCOVNotificationWrapper>.Save(sink, (ConfirmedCOVNotificationWrapper)value);
					break;
				case Tags.ConfirmedEventNotification:
					Value<ConfirmedEventNotificationWrapper>.Save(sink, (ConfirmedEventNotificationWrapper)value);
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
			AcknowledgeAlarm = 0,
			ConfirmedCOVNotification = 1,
			ConfirmedEventNotification = 2,
			GetEnrollmentSummary = 3,
			GetEventInformation = 4,
			SubscribeCOV = 5,
			SubscribeCOVProperty = 6,
			LifeSafetyOperation = 7,
			AtomicReadFile = 8,
			AtomicWriteFile = 9,
			AddListElement = 10,
			RemoveListElement = 11,
			CreateObject = 12,
			DeleteObject = 13,
			ReadProperty = 14,
			ReadPropertyConditional = 15,
			ReadPropertyMultiple = 16,
			ReadRange = 17,
			WriteProperty = 18,
			WritePropertyMultiple = 19,
			DeviceCommunicationControl = 20,
			ConfirmedPrivateTransfer = 21,
			ConfirmedTextMessage = 22,
			ReinitializeDevice = 23,
			VtOpen = 24,
			VtClose = 25,
			VtData = 26,
			Authenticate = 27,
			RequestKey = 28
		}

		public  partial class AcknowledgeAlarmWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.AcknowledgeAlarm; } }

			public AcknowledgeAlarmRequest Item { get; private set; }

			public AcknowledgeAlarmWrapper(AcknowledgeAlarmRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AcknowledgeAlarmRequest>.Schema;

			public static new AcknowledgeAlarmWrapper Load(IValueStream stream)
			{
				var temp = Value<AcknowledgeAlarmRequest>.Load(stream);
				return new AcknowledgeAlarmWrapper(temp);
			}

			public static void Save(IValueSink sink, AcknowledgeAlarmWrapper value)
			{
				Value<AcknowledgeAlarmRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedCOVNotificationWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ConfirmedCOVNotification; } }

			public ConfirmedCOVNotificationRequest Item { get; private set; }

			public ConfirmedCOVNotificationWrapper(ConfirmedCOVNotificationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ConfirmedCOVNotificationRequest>.Schema;

			public static new ConfirmedCOVNotificationWrapper Load(IValueStream stream)
			{
				var temp = Value<ConfirmedCOVNotificationRequest>.Load(stream);
				return new ConfirmedCOVNotificationWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedCOVNotificationWrapper value)
			{
				Value<ConfirmedCOVNotificationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedEventNotificationWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ConfirmedEventNotification; } }

			public ConfirmedEventNotificationRequest Item { get; private set; }

			public ConfirmedEventNotificationWrapper(ConfirmedEventNotificationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ConfirmedEventNotificationRequest>.Schema;

			public static new ConfirmedEventNotificationWrapper Load(IValueStream stream)
			{
				var temp = Value<ConfirmedEventNotificationRequest>.Load(stream);
				return new ConfirmedEventNotificationWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedEventNotificationWrapper value)
			{
				Value<ConfirmedEventNotificationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class GetEnrollmentSummaryWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.GetEnrollmentSummary; } }

			public GetEnrollmentSummaryRequest Item { get; private set; }

			public GetEnrollmentSummaryWrapper(GetEnrollmentSummaryRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GetEnrollmentSummaryRequest>.Schema;

			public static new GetEnrollmentSummaryWrapper Load(IValueStream stream)
			{
				var temp = Value<GetEnrollmentSummaryRequest>.Load(stream);
				return new GetEnrollmentSummaryWrapper(temp);
			}

			public static void Save(IValueSink sink, GetEnrollmentSummaryWrapper value)
			{
				Value<GetEnrollmentSummaryRequest>.Save(sink, value.Item);
			}

		}

		public  partial class GetEventInformationWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.GetEventInformation; } }

			public GetEventInformationRequest Item { get; private set; }

			public GetEventInformationWrapper(GetEventInformationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GetEventInformationRequest>.Schema;

			public static new GetEventInformationWrapper Load(IValueStream stream)
			{
				var temp = Value<GetEventInformationRequest>.Load(stream);
				return new GetEventInformationWrapper(temp);
			}

			public static void Save(IValueSink sink, GetEventInformationWrapper value)
			{
				Value<GetEventInformationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class SubscribeCOVWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.SubscribeCOV; } }

			public SubscribeCOVRequest Item { get; private set; }

			public SubscribeCOVWrapper(SubscribeCOVRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<SubscribeCOVRequest>.Schema;

			public static new SubscribeCOVWrapper Load(IValueStream stream)
			{
				var temp = Value<SubscribeCOVRequest>.Load(stream);
				return new SubscribeCOVWrapper(temp);
			}

			public static void Save(IValueSink sink, SubscribeCOVWrapper value)
			{
				Value<SubscribeCOVRequest>.Save(sink, value.Item);
			}

		}

		public  partial class SubscribeCOVPropertyWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.SubscribeCOVProperty; } }

			public SubscribeCOVPropertyRequest Item { get; private set; }

			public SubscribeCOVPropertyWrapper(SubscribeCOVPropertyRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<SubscribeCOVPropertyRequest>.Schema;

			public static new SubscribeCOVPropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<SubscribeCOVPropertyRequest>.Load(stream);
				return new SubscribeCOVPropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, SubscribeCOVPropertyWrapper value)
			{
				Value<SubscribeCOVPropertyRequest>.Save(sink, value.Item);
			}

		}

		public  partial class LifeSafetyOperationWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.LifeSafetyOperation; } }

			public LifeSafetyOperationRequest Item { get; private set; }

			public LifeSafetyOperationWrapper(LifeSafetyOperationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<LifeSafetyOperationRequest>.Schema;

			public static new LifeSafetyOperationWrapper Load(IValueStream stream)
			{
				var temp = Value<LifeSafetyOperationRequest>.Load(stream);
				return new LifeSafetyOperationWrapper(temp);
			}

			public static void Save(IValueSink sink, LifeSafetyOperationWrapper value)
			{
				Value<LifeSafetyOperationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class AtomicReadFileWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.AtomicReadFile; } }

			public AtomicReadFileRequest Item { get; private set; }

			public AtomicReadFileWrapper(AtomicReadFileRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AtomicReadFileRequest>.Schema;

			public static new AtomicReadFileWrapper Load(IValueStream stream)
			{
				var temp = Value<AtomicReadFileRequest>.Load(stream);
				return new AtomicReadFileWrapper(temp);
			}

			public static void Save(IValueSink sink, AtomicReadFileWrapper value)
			{
				Value<AtomicReadFileRequest>.Save(sink, value.Item);
			}

		}

		public  partial class AtomicWriteFileWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.AtomicWriteFile; } }

			public AtomicWriteFileRequest Item { get; private set; }

			public AtomicWriteFileWrapper(AtomicWriteFileRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AtomicWriteFileRequest>.Schema;

			public static new AtomicWriteFileWrapper Load(IValueStream stream)
			{
				var temp = Value<AtomicWriteFileRequest>.Load(stream);
				return new AtomicWriteFileWrapper(temp);
			}

			public static void Save(IValueSink sink, AtomicWriteFileWrapper value)
			{
				Value<AtomicWriteFileRequest>.Save(sink, value.Item);
			}

		}

		public  partial class AddListElementWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.AddListElement; } }

			public AddListElementRequest Item { get; private set; }

			public AddListElementWrapper(AddListElementRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AddListElementRequest>.Schema;

			public static new AddListElementWrapper Load(IValueStream stream)
			{
				var temp = Value<AddListElementRequest>.Load(stream);
				return new AddListElementWrapper(temp);
			}

			public static void Save(IValueSink sink, AddListElementWrapper value)
			{
				Value<AddListElementRequest>.Save(sink, value.Item);
			}

		}

		public  partial class RemoveListElementWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.RemoveListElement; } }

			public RemoveListElementRequest Item { get; private set; }

			public RemoveListElementWrapper(RemoveListElementRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<RemoveListElementRequest>.Schema;

			public static new RemoveListElementWrapper Load(IValueStream stream)
			{
				var temp = Value<RemoveListElementRequest>.Load(stream);
				return new RemoveListElementWrapper(temp);
			}

			public static void Save(IValueSink sink, RemoveListElementWrapper value)
			{
				Value<RemoveListElementRequest>.Save(sink, value.Item);
			}

		}

		public  partial class CreateObjectWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.CreateObject; } }

			public CreateObjectRequest Item { get; private set; }

			public CreateObjectWrapper(CreateObjectRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<CreateObjectRequest>.Schema;

			public static new CreateObjectWrapper Load(IValueStream stream)
			{
				var temp = Value<CreateObjectRequest>.Load(stream);
				return new CreateObjectWrapper(temp);
			}

			public static void Save(IValueSink sink, CreateObjectWrapper value)
			{
				Value<CreateObjectRequest>.Save(sink, value.Item);
			}

		}

		public  partial class DeleteObjectWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.DeleteObject; } }

			public DeleteObjectRequest Item { get; private set; }

			public DeleteObjectWrapper(DeleteObjectRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<DeleteObjectRequest>.Schema;

			public static new DeleteObjectWrapper Load(IValueStream stream)
			{
				var temp = Value<DeleteObjectRequest>.Load(stream);
				return new DeleteObjectWrapper(temp);
			}

			public static void Save(IValueSink sink, DeleteObjectWrapper value)
			{
				Value<DeleteObjectRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ReadProperty; } }

			public ReadPropertyRequest Item { get; private set; }

			public ReadPropertyWrapper(ReadPropertyRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadPropertyRequest>.Schema;

			public static new ReadPropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadPropertyRequest>.Load(stream);
				return new ReadPropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyWrapper value)
			{
				Value<ReadPropertyRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyConditionalWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ReadPropertyConditional; } }

			public ReadPropertyConditionalRequest Item { get; private set; }

			public ReadPropertyConditionalWrapper(ReadPropertyConditionalRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadPropertyConditionalRequest>.Schema;

			public static new ReadPropertyConditionalWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadPropertyConditionalRequest>.Load(stream);
				return new ReadPropertyConditionalWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyConditionalWrapper value)
			{
				Value<ReadPropertyConditionalRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyMultipleWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ReadPropertyMultiple; } }

			public ReadPropertyMultipleRequest Item { get; private set; }

			public ReadPropertyMultipleWrapper(ReadPropertyMultipleRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadPropertyMultipleRequest>.Schema;

			public static new ReadPropertyMultipleWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadPropertyMultipleRequest>.Load(stream);
				return new ReadPropertyMultipleWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyMultipleWrapper value)
			{
				Value<ReadPropertyMultipleRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ReadRangeWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ReadRange; } }

			public ReadRangeRequest Item { get; private set; }

			public ReadRangeWrapper(ReadRangeRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadRangeRequest>.Schema;

			public static new ReadRangeWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadRangeRequest>.Load(stream);
				return new ReadRangeWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadRangeWrapper value)
			{
				Value<ReadRangeRequest>.Save(sink, value.Item);
			}

		}

		public  partial class WritePropertyWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.WriteProperty; } }

			public WritePropertyRequest Item { get; private set; }

			public WritePropertyWrapper(WritePropertyRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<WritePropertyRequest>.Schema;

			public static new WritePropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<WritePropertyRequest>.Load(stream);
				return new WritePropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, WritePropertyWrapper value)
			{
				Value<WritePropertyRequest>.Save(sink, value.Item);
			}

		}

		public  partial class WritePropertyMultipleWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.WritePropertyMultiple; } }

			public WritePropertyMultipleRequest Item { get; private set; }

			public WritePropertyMultipleWrapper(WritePropertyMultipleRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<WritePropertyMultipleRequest>.Schema;

			public static new WritePropertyMultipleWrapper Load(IValueStream stream)
			{
				var temp = Value<WritePropertyMultipleRequest>.Load(stream);
				return new WritePropertyMultipleWrapper(temp);
			}

			public static void Save(IValueSink sink, WritePropertyMultipleWrapper value)
			{
				Value<WritePropertyMultipleRequest>.Save(sink, value.Item);
			}

		}

		public  partial class DeviceCommunicationControlWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.DeviceCommunicationControl; } }

			public DeviceCommunicationControlRequest Item { get; private set; }

			public DeviceCommunicationControlWrapper(DeviceCommunicationControlRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<DeviceCommunicationControlRequest>.Schema;

			public static new DeviceCommunicationControlWrapper Load(IValueStream stream)
			{
				var temp = Value<DeviceCommunicationControlRequest>.Load(stream);
				return new DeviceCommunicationControlWrapper(temp);
			}

			public static void Save(IValueSink sink, DeviceCommunicationControlWrapper value)
			{
				Value<DeviceCommunicationControlRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedPrivateTransferWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ConfirmedPrivateTransfer; } }

			public ConfirmedPrivateTransferRequest Item { get; private set; }

			public ConfirmedPrivateTransferWrapper(ConfirmedPrivateTransferRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ConfirmedPrivateTransferRequest>.Schema;

			public static new ConfirmedPrivateTransferWrapper Load(IValueStream stream)
			{
				var temp = Value<ConfirmedPrivateTransferRequest>.Load(stream);
				return new ConfirmedPrivateTransferWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedPrivateTransferWrapper value)
			{
				Value<ConfirmedPrivateTransferRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedTextMessageWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ConfirmedTextMessage; } }

			public ConfirmedTextMessageRequest Item { get; private set; }

			public ConfirmedTextMessageWrapper(ConfirmedTextMessageRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ConfirmedTextMessageRequest>.Schema;

			public static new ConfirmedTextMessageWrapper Load(IValueStream stream)
			{
				var temp = Value<ConfirmedTextMessageRequest>.Load(stream);
				return new ConfirmedTextMessageWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedTextMessageWrapper value)
			{
				Value<ConfirmedTextMessageRequest>.Save(sink, value.Item);
			}

		}

		public  partial class ReinitializeDeviceWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.ReinitializeDevice; } }

			public ReinitializeDeviceRequest Item { get; private set; }

			public ReinitializeDeviceWrapper(ReinitializeDeviceRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReinitializeDeviceRequest>.Schema;

			public static new ReinitializeDeviceWrapper Load(IValueStream stream)
			{
				var temp = Value<ReinitializeDeviceRequest>.Load(stream);
				return new ReinitializeDeviceWrapper(temp);
			}

			public static void Save(IValueSink sink, ReinitializeDeviceWrapper value)
			{
				Value<ReinitializeDeviceRequest>.Save(sink, value.Item);
			}

		}

		public  partial class VtOpenWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.VtOpen; } }

			public VTOpenRequest Item { get; private set; }

			public VtOpenWrapper(VTOpenRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<VTOpenRequest>.Schema;

			public static new VtOpenWrapper Load(IValueStream stream)
			{
				var temp = Value<VTOpenRequest>.Load(stream);
				return new VtOpenWrapper(temp);
			}

			public static void Save(IValueSink sink, VtOpenWrapper value)
			{
				Value<VTOpenRequest>.Save(sink, value.Item);
			}

		}

		public  partial class VtCloseWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.VtClose; } }

			public VTCloseRequest Item { get; private set; }

			public VtCloseWrapper(VTCloseRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<VTCloseRequest>.Schema;

			public static new VtCloseWrapper Load(IValueStream stream)
			{
				var temp = Value<VTCloseRequest>.Load(stream);
				return new VtCloseWrapper(temp);
			}

			public static void Save(IValueSink sink, VtCloseWrapper value)
			{
				Value<VTCloseRequest>.Save(sink, value.Item);
			}

		}

		public  partial class VtDataWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.VtData; } }

			public VTDataRequest Item { get; private set; }

			public VtDataWrapper(VTDataRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<VTDataRequest>.Schema;

			public static new VtDataWrapper Load(IValueStream stream)
			{
				var temp = Value<VTDataRequest>.Load(stream);
				return new VtDataWrapper(temp);
			}

			public static void Save(IValueSink sink, VtDataWrapper value)
			{
				Value<VTDataRequest>.Save(sink, value.Item);
			}

		}

		public  partial class AuthenticateWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.Authenticate; } }

			public AuthenticateRequest Item { get; private set; }

			public AuthenticateWrapper(AuthenticateRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AuthenticateRequest>.Schema;

			public static new AuthenticateWrapper Load(IValueStream stream)
			{
				var temp = Value<AuthenticateRequest>.Load(stream);
				return new AuthenticateWrapper(temp);
			}

			public static void Save(IValueSink sink, AuthenticateWrapper value)
			{
				Value<AuthenticateRequest>.Save(sink, value.Item);
			}

		}

		public  partial class RequestKeyWrapper : ConfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.RequestKey; } }

			public RequestKeyRequest Item { get; private set; }

			public RequestKeyWrapper(RequestKeyRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<RequestKeyRequest>.Schema;

			public static new RequestKeyWrapper Load(IValueStream stream)
			{
				var temp = Value<RequestKeyRequest>.Load(stream);
				return new RequestKeyWrapper(temp);
			}

			public static void Save(IValueSink sink, RequestKeyWrapper value)
			{
				Value<RequestKeyRequest>.Save(sink, value.Item);
			}

		}
	}
}
