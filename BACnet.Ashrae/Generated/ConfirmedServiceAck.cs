using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class ConfirmedServiceAck
	{
		public abstract Tags Tag { get; }

		public bool IsGetAlarmSummary { get { return this.Tag == Tags.GetAlarmSummary; } }

		public GetAlarmSummaryAck AsGetAlarmSummary { get { return ((GetAlarmSummaryWrapper)this).Item; } }

		public static ConfirmedServiceAck NewGetAlarmSummary(GetAlarmSummaryAck getAlarmSummary)
		{
			return new GetAlarmSummaryWrapper(getAlarmSummary);
		}

		public bool IsGetEnrollmentSummary { get { return this.Tag == Tags.GetEnrollmentSummary; } }

		public GetEnrollmentSummaryAck AsGetEnrollmentSummary { get { return ((GetEnrollmentSummaryWrapper)this).Item; } }

		public static ConfirmedServiceAck NewGetEnrollmentSummary(GetEnrollmentSummaryAck getEnrollmentSummary)
		{
			return new GetEnrollmentSummaryWrapper(getEnrollmentSummary);
		}

		public bool IsGetEventInformation { get { return this.Tag == Tags.GetEventInformation; } }

		public GetEventInformationAck AsGetEventInformation { get { return ((GetEventInformationWrapper)this).Item; } }

		public static ConfirmedServiceAck NewGetEventInformation(GetEventInformationAck getEventInformation)
		{
			return new GetEventInformationWrapper(getEventInformation);
		}

		public bool IsAtomicReadFile { get { return this.Tag == Tags.AtomicReadFile; } }

		public AtomicReadFileAck AsAtomicReadFile { get { return ((AtomicReadFileWrapper)this).Item; } }

		public static ConfirmedServiceAck NewAtomicReadFile(AtomicReadFileAck atomicReadFile)
		{
			return new AtomicReadFileWrapper(atomicReadFile);
		}

		public bool IsAtomicWriteFile { get { return this.Tag == Tags.AtomicWriteFile; } }

		public AtomicWriteFileAck AsAtomicWriteFile { get { return ((AtomicWriteFileWrapper)this).Item; } }

		public static ConfirmedServiceAck NewAtomicWriteFile(AtomicWriteFileAck atomicWriteFile)
		{
			return new AtomicWriteFileWrapper(atomicWriteFile);
		}

		public bool IsCreateObject { get { return this.Tag == Tags.CreateObject; } }

		public CreateObjectAck AsCreateObject { get { return ((CreateObjectWrapper)this).Item; } }

		public static ConfirmedServiceAck NewCreateObject(CreateObjectAck createObject)
		{
			return new CreateObjectWrapper(createObject);
		}

		public bool IsReadProperty { get { return this.Tag == Tags.ReadProperty; } }

		public ReadPropertyAck AsReadProperty { get { return ((ReadPropertyWrapper)this).Item; } }

		public static ConfirmedServiceAck NewReadProperty(ReadPropertyAck readProperty)
		{
			return new ReadPropertyWrapper(readProperty);
		}

		public bool IsReadPropertyConditional { get { return this.Tag == Tags.ReadPropertyConditional; } }

		public ReadPropertyConditionalAck AsReadPropertyConditional { get { return ((ReadPropertyConditionalWrapper)this).Item; } }

		public static ConfirmedServiceAck NewReadPropertyConditional(ReadPropertyConditionalAck readPropertyConditional)
		{
			return new ReadPropertyConditionalWrapper(readPropertyConditional);
		}

		public bool IsReadPropertyMultiple { get { return this.Tag == Tags.ReadPropertyMultiple; } }

		public ReadPropertyMultipleAck AsReadPropertyMultiple { get { return ((ReadPropertyMultipleWrapper)this).Item; } }

		public static ConfirmedServiceAck NewReadPropertyMultiple(ReadPropertyMultipleAck readPropertyMultiple)
		{
			return new ReadPropertyMultipleWrapper(readPropertyMultiple);
		}

		public bool IsReadRange { get { return this.Tag == Tags.ReadRange; } }

		public ReadRangeAck AsReadRange { get { return ((ReadRangeWrapper)this).Item; } }

		public static ConfirmedServiceAck NewReadRange(ReadRangeAck readRange)
		{
			return new ReadRangeWrapper(readRange);
		}

		public bool IsConfirmedPrivateTransfer { get { return this.Tag == Tags.ConfirmedPrivateTransfer; } }

		public ConfirmedPrivateTransferAck AsConfirmedPrivateTransfer { get { return ((ConfirmedPrivateTransferWrapper)this).Item; } }

		public static ConfirmedServiceAck NewConfirmedPrivateTransfer(ConfirmedPrivateTransferAck confirmedPrivateTransfer)
		{
			return new ConfirmedPrivateTransferWrapper(confirmedPrivateTransfer);
		}

		public bool IsVtOpen { get { return this.Tag == Tags.VtOpen; } }

		public VTOpenAck AsVtOpen { get { return ((VtOpenWrapper)this).Item; } }

		public static ConfirmedServiceAck NewVtOpen(VTOpenAck vtOpen)
		{
			return new VtOpenWrapper(vtOpen);
		}

		public bool IsVtData { get { return this.Tag == Tags.VtData; } }

		public VTDataAck AsVtData { get { return ((VtDataWrapper)this).Item; } }

		public static ConfirmedServiceAck NewVtData(VTDataAck vtData)
		{
			return new VtDataWrapper(vtData);
		}

		public bool IsAuthenticate { get { return this.Tag == Tags.Authenticate; } }

		public AuthenticateAck AsAuthenticate { get { return ((AuthenticateWrapper)this).Item; } }

		public static ConfirmedServiceAck NewAuthenticate(AuthenticateAck authenticate)
		{
			return new AuthenticateWrapper(authenticate);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("GetAlarmSummary", 3, Value<GetAlarmSummaryAck>.Schema),
			new FieldSchema("GetEnrollmentSummary", 4, Value<GetEnrollmentSummaryAck>.Schema),
			new FieldSchema("GetEventInformation", 29, Value<GetEventInformationAck>.Schema),
			new FieldSchema("AtomicReadFile", 6, Value<AtomicReadFileAck>.Schema),
			new FieldSchema("AtomicWriteFile", 7, Value<AtomicWriteFileAck>.Schema),
			new FieldSchema("CreateObject", 10, Value<CreateObjectAck>.Schema),
			new FieldSchema("ReadProperty", 12, Value<ReadPropertyAck>.Schema),
			new FieldSchema("ReadPropertyConditional", 13, Value<ReadPropertyConditionalAck>.Schema),
			new FieldSchema("ReadPropertyMultiple", 14, Value<ReadPropertyMultipleAck>.Schema),
			new FieldSchema("ReadRange", 26, Value<ReadRangeAck>.Schema),
			new FieldSchema("ConfirmedPrivateTransfer", 18, Value<ConfirmedPrivateTransferAck>.Schema),
			new FieldSchema("VtOpen", 21, Value<VTOpenAck>.Schema),
			new FieldSchema("VtData", 23, Value<VTDataAck>.Schema),
			new FieldSchema("Authenticate", 24, Value<AuthenticateAck>.Schema));

		public static ConfirmedServiceAck Load(IValueStream stream)
		{
			ConfirmedServiceAck ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.GetAlarmSummary:
					ret = Value<GetAlarmSummaryWrapper>.Load(stream);
					break;
				case Tags.GetEnrollmentSummary:
					ret = Value<GetEnrollmentSummaryWrapper>.Load(stream);
					break;
				case Tags.GetEventInformation:
					ret = Value<GetEventInformationWrapper>.Load(stream);
					break;
				case Tags.AtomicReadFile:
					ret = Value<AtomicReadFileWrapper>.Load(stream);
					break;
				case Tags.AtomicWriteFile:
					ret = Value<AtomicWriteFileWrapper>.Load(stream);
					break;
				case Tags.CreateObject:
					ret = Value<CreateObjectWrapper>.Load(stream);
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
				case Tags.ConfirmedPrivateTransfer:
					ret = Value<ConfirmedPrivateTransferWrapper>.Load(stream);
					break;
				case Tags.VtOpen:
					ret = Value<VtOpenWrapper>.Load(stream);
					break;
				case Tags.VtData:
					ret = Value<VtDataWrapper>.Load(stream);
					break;
				case Tags.Authenticate:
					ret = Value<AuthenticateWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, ConfirmedServiceAck value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.GetAlarmSummary:
					Value<GetAlarmSummaryWrapper>.Save(sink, (GetAlarmSummaryWrapper)value);
					break;
				case Tags.GetEnrollmentSummary:
					Value<GetEnrollmentSummaryWrapper>.Save(sink, (GetEnrollmentSummaryWrapper)value);
					break;
				case Tags.GetEventInformation:
					Value<GetEventInformationWrapper>.Save(sink, (GetEventInformationWrapper)value);
					break;
				case Tags.AtomicReadFile:
					Value<AtomicReadFileWrapper>.Save(sink, (AtomicReadFileWrapper)value);
					break;
				case Tags.AtomicWriteFile:
					Value<AtomicWriteFileWrapper>.Save(sink, (AtomicWriteFileWrapper)value);
					break;
				case Tags.CreateObject:
					Value<CreateObjectWrapper>.Save(sink, (CreateObjectWrapper)value);
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
				case Tags.ConfirmedPrivateTransfer:
					Value<ConfirmedPrivateTransferWrapper>.Save(sink, (ConfirmedPrivateTransferWrapper)value);
					break;
				case Tags.VtOpen:
					Value<VtOpenWrapper>.Save(sink, (VtOpenWrapper)value);
					break;
				case Tags.VtData:
					Value<VtDataWrapper>.Save(sink, (VtDataWrapper)value);
					break;
				case Tags.Authenticate:
					Value<AuthenticateWrapper>.Save(sink, (AuthenticateWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			GetAlarmSummary = 0,
			GetEnrollmentSummary = 1,
			GetEventInformation = 2,
			AtomicReadFile = 3,
			AtomicWriteFile = 4,
			CreateObject = 5,
			ReadProperty = 6,
			ReadPropertyConditional = 7,
			ReadPropertyMultiple = 8,
			ReadRange = 9,
			ConfirmedPrivateTransfer = 10,
			VtOpen = 11,
			VtData = 12,
			Authenticate = 13
		}

		public  partial class GetAlarmSummaryWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.GetAlarmSummary; } }

			public GetAlarmSummaryAck Item { get; private set; }

			public GetAlarmSummaryWrapper(GetAlarmSummaryAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GetAlarmSummaryAck>.Schema;

			public static new GetAlarmSummaryWrapper Load(IValueStream stream)
			{
				var temp = Value<GetAlarmSummaryAck>.Load(stream);
				return new GetAlarmSummaryWrapper(temp);
			}

			public static void Save(IValueSink sink, GetAlarmSummaryWrapper value)
			{
				Value<GetAlarmSummaryAck>.Save(sink, value.Item);
			}

		}

		public  partial class GetEnrollmentSummaryWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.GetEnrollmentSummary; } }

			public GetEnrollmentSummaryAck Item { get; private set; }

			public GetEnrollmentSummaryWrapper(GetEnrollmentSummaryAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GetEnrollmentSummaryAck>.Schema;

			public static new GetEnrollmentSummaryWrapper Load(IValueStream stream)
			{
				var temp = Value<GetEnrollmentSummaryAck>.Load(stream);
				return new GetEnrollmentSummaryWrapper(temp);
			}

			public static void Save(IValueSink sink, GetEnrollmentSummaryWrapper value)
			{
				Value<GetEnrollmentSummaryAck>.Save(sink, value.Item);
			}

		}

		public  partial class GetEventInformationWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.GetEventInformation; } }

			public GetEventInformationAck Item { get; private set; }

			public GetEventInformationWrapper(GetEventInformationAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<GetEventInformationAck>.Schema;

			public static new GetEventInformationWrapper Load(IValueStream stream)
			{
				var temp = Value<GetEventInformationAck>.Load(stream);
				return new GetEventInformationWrapper(temp);
			}

			public static void Save(IValueSink sink, GetEventInformationWrapper value)
			{
				Value<GetEventInformationAck>.Save(sink, value.Item);
			}

		}

		public  partial class AtomicReadFileWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.AtomicReadFile; } }

			public AtomicReadFileAck Item { get; private set; }

			public AtomicReadFileWrapper(AtomicReadFileAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AtomicReadFileAck>.Schema;

			public static new AtomicReadFileWrapper Load(IValueStream stream)
			{
				var temp = Value<AtomicReadFileAck>.Load(stream);
				return new AtomicReadFileWrapper(temp);
			}

			public static void Save(IValueSink sink, AtomicReadFileWrapper value)
			{
				Value<AtomicReadFileAck>.Save(sink, value.Item);
			}

		}

		public  partial class AtomicWriteFileWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.AtomicWriteFile; } }

			public AtomicWriteFileAck Item { get; private set; }

			public AtomicWriteFileWrapper(AtomicWriteFileAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AtomicWriteFileAck>.Schema;

			public static new AtomicWriteFileWrapper Load(IValueStream stream)
			{
				var temp = Value<AtomicWriteFileAck>.Load(stream);
				return new AtomicWriteFileWrapper(temp);
			}

			public static void Save(IValueSink sink, AtomicWriteFileWrapper value)
			{
				Value<AtomicWriteFileAck>.Save(sink, value.Item);
			}

		}

		public  partial class CreateObjectWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.CreateObject; } }

			public CreateObjectAck Item { get; private set; }

			public CreateObjectWrapper(CreateObjectAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<CreateObjectAck>.Schema;

			public static new CreateObjectWrapper Load(IValueStream stream)
			{
				var temp = Value<CreateObjectAck>.Load(stream);
				return new CreateObjectWrapper(temp);
			}

			public static void Save(IValueSink sink, CreateObjectWrapper value)
			{
				Value<CreateObjectAck>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.ReadProperty; } }

			public ReadPropertyAck Item { get; private set; }

			public ReadPropertyWrapper(ReadPropertyAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadPropertyAck>.Schema;

			public static new ReadPropertyWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadPropertyAck>.Load(stream);
				return new ReadPropertyWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyWrapper value)
			{
				Value<ReadPropertyAck>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyConditionalWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.ReadPropertyConditional; } }

			public ReadPropertyConditionalAck Item { get; private set; }

			public ReadPropertyConditionalWrapper(ReadPropertyConditionalAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadPropertyConditionalAck>.Schema;

			public static new ReadPropertyConditionalWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadPropertyConditionalAck>.Load(stream);
				return new ReadPropertyConditionalWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyConditionalWrapper value)
			{
				Value<ReadPropertyConditionalAck>.Save(sink, value.Item);
			}

		}

		public  partial class ReadPropertyMultipleWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.ReadPropertyMultiple; } }

			public ReadPropertyMultipleAck Item { get; private set; }

			public ReadPropertyMultipleWrapper(ReadPropertyMultipleAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadPropertyMultipleAck>.Schema;

			public static new ReadPropertyMultipleWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadPropertyMultipleAck>.Load(stream);
				return new ReadPropertyMultipleWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadPropertyMultipleWrapper value)
			{
				Value<ReadPropertyMultipleAck>.Save(sink, value.Item);
			}

		}

		public  partial class ReadRangeWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.ReadRange; } }

			public ReadRangeAck Item { get; private set; }

			public ReadRangeWrapper(ReadRangeAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ReadRangeAck>.Schema;

			public static new ReadRangeWrapper Load(IValueStream stream)
			{
				var temp = Value<ReadRangeAck>.Load(stream);
				return new ReadRangeWrapper(temp);
			}

			public static void Save(IValueSink sink, ReadRangeWrapper value)
			{
				Value<ReadRangeAck>.Save(sink, value.Item);
			}

		}

		public  partial class ConfirmedPrivateTransferWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.ConfirmedPrivateTransfer; } }

			public ConfirmedPrivateTransferAck Item { get; private set; }

			public ConfirmedPrivateTransferWrapper(ConfirmedPrivateTransferAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<ConfirmedPrivateTransferAck>.Schema;

			public static new ConfirmedPrivateTransferWrapper Load(IValueStream stream)
			{
				var temp = Value<ConfirmedPrivateTransferAck>.Load(stream);
				return new ConfirmedPrivateTransferWrapper(temp);
			}

			public static void Save(IValueSink sink, ConfirmedPrivateTransferWrapper value)
			{
				Value<ConfirmedPrivateTransferAck>.Save(sink, value.Item);
			}

		}

		public  partial class VtOpenWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.VtOpen; } }

			public VTOpenAck Item { get; private set; }

			public VtOpenWrapper(VTOpenAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<VTOpenAck>.Schema;

			public static new VtOpenWrapper Load(IValueStream stream)
			{
				var temp = Value<VTOpenAck>.Load(stream);
				return new VtOpenWrapper(temp);
			}

			public static void Save(IValueSink sink, VtOpenWrapper value)
			{
				Value<VTOpenAck>.Save(sink, value.Item);
			}

		}

		public  partial class VtDataWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.VtData; } }

			public VTDataAck Item { get; private set; }

			public VtDataWrapper(VTDataAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<VTDataAck>.Schema;

			public static new VtDataWrapper Load(IValueStream stream)
			{
				var temp = Value<VTDataAck>.Load(stream);
				return new VtDataWrapper(temp);
			}

			public static void Save(IValueSink sink, VtDataWrapper value)
			{
				Value<VTDataAck>.Save(sink, value.Item);
			}

		}

		public  partial class AuthenticateWrapper : ConfirmedServiceAck
		{
			public override Tags Tag { get { return Tags.Authenticate; } }

			public AuthenticateAck Item { get; private set; }

			public AuthenticateWrapper(AuthenticateAck item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<AuthenticateAck>.Schema;

			public static new AuthenticateWrapper Load(IValueStream stream)
			{
				var temp = Value<AuthenticateAck>.Load(stream);
				return new AuthenticateWrapper(temp);
			}

			public static void Save(IValueSink sink, AuthenticateWrapper value)
			{
				Value<AuthenticateAck>.Save(sink, value.Item);
			}

		}
	}
}
