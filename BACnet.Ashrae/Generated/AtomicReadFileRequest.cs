using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AtomicReadFileRequest
	{
		public ObjectId FileIdentifier { get; private set; }

		public AccessMethodType AccessMethod { get; private set; }

		public AtomicReadFileRequest(ObjectId fileIdentifier, AccessMethodType accessMethod)
		{
			this.FileIdentifier = fileIdentifier;
			this.AccessMethod = accessMethod;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("FileIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("AccessMethod", 255, Value<AccessMethodType>.Schema));

		public static AtomicReadFileRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var fileIdentifier = Value<ObjectId>.Load(stream);
			var accessMethod = Value<AccessMethodType>.Load(stream);
			stream.LeaveSequence();
			return new AtomicReadFileRequest(fileIdentifier, accessMethod);
		}

		public static void Save(IValueSink sink, AtomicReadFileRequest value)
		{
			sink.EnterSequence();
			Value<ObjectId>.Save(sink, value.FileIdentifier);
			Value<AccessMethodType>.Save(sink, value.AccessMethod);
			sink.LeaveSequence();
		}

		public enum Tags : byte
		{
			StreamAccess = 0,
			RecordAccess = 1
		}

		public abstract  partial class AccessMethodType
		{
			public abstract Tags Tag { get; }

			public bool IsStreamAccess { get { return this.Tag == Tags.StreamAccess; } }

			public StreamAccess AsStreamAccess { get { return (StreamAccess)this; } }

			public static AccessMethodType NewStreamAccess(int fileStartPosition, uint requestedOctetCount)
			{
				return new StreamAccess(fileStartPosition, requestedOctetCount);
			}

			public bool IsRecordAccess { get { return this.Tag == Tags.RecordAccess; } }

			public RecordAccess AsRecordAccess { get { return (RecordAccess)this; } }

			public static AccessMethodType NewRecordAccess(int fileStartRecord, uint requestedRecordCount)
			{
				return new RecordAccess(fileStartRecord, requestedRecordCount);
			}

			public static readonly ISchema Schema = new ChoiceSchema(false, 
				new FieldSchema("StreamAccess", 0, Value<StreamAccess>.Schema),
				new FieldSchema("RecordAccess", 1, Value<RecordAccess>.Schema));

			public static AccessMethodType Load(IValueStream stream)
			{
				AccessMethodType ret = null;
				Tags tag = (Tags)stream.EnterChoice();
				switch(tag)
				{
					case Tags.StreamAccess:
						ret = Value<StreamAccess>.Load(stream);
						break;
					case Tags.RecordAccess:
						ret = Value<RecordAccess>.Load(stream);
						break;
					default:
						throw new Exception();
				}
				stream.LeaveChoice();
				return ret;
			}

			public static void Save(IValueSink sink, AccessMethodType value)
			{
				sink.EnterChoice((byte)value.Tag);
				switch(value.Tag)
				{
					case Tags.StreamAccess:
						Value<StreamAccess>.Save(sink, (StreamAccess)value);
						break;
					case Tags.RecordAccess:
						Value<RecordAccess>.Save(sink, (RecordAccess)value);
						break;
					default:
						throw new Exception();
				}
				sink.LeaveChoice();
			}
		}
		public  partial class StreamAccess : AccessMethodType
		{
			public override Tags Tag { get { return Tags.StreamAccess; } }

			public int FileStartPosition { get; private set; }

			public uint RequestedOctetCount { get; private set; }

			public StreamAccess(int fileStartPosition, uint requestedOctetCount)
			{
				this.FileStartPosition = fileStartPosition;
				this.RequestedOctetCount = requestedOctetCount;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("FileStartPosition", 255, Value<int>.Schema),
				new FieldSchema("RequestedOctetCount", 255, Value<uint>.Schema));

			public static new StreamAccess Load(IValueStream stream)
			{
				stream.EnterSequence();
				var fileStartPosition = Value<int>.Load(stream);
				var requestedOctetCount = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new StreamAccess(fileStartPosition, requestedOctetCount);
			}

			public static void Save(IValueSink sink, StreamAccess value)
			{
				sink.EnterSequence();
				Value<int>.Save(sink, value.FileStartPosition);
				Value<uint>.Save(sink, value.RequestedOctetCount);
				sink.LeaveSequence();
			}
		}
		public  partial class RecordAccess : AccessMethodType
		{
			public override Tags Tag { get { return Tags.RecordAccess; } }

			public int FileStartRecord { get; private set; }

			public uint RequestedRecordCount { get; private set; }

			public RecordAccess(int fileStartRecord, uint requestedRecordCount)
			{
				this.FileStartRecord = fileStartRecord;
				this.RequestedRecordCount = requestedRecordCount;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("FileStartRecord", 255, Value<int>.Schema),
				new FieldSchema("RequestedRecordCount", 255, Value<uint>.Schema));

			public static new RecordAccess Load(IValueStream stream)
			{
				stream.EnterSequence();
				var fileStartRecord = Value<int>.Load(stream);
				var requestedRecordCount = Value<uint>.Load(stream);
				stream.LeaveSequence();
				return new RecordAccess(fileStartRecord, requestedRecordCount);
			}

			public static void Save(IValueSink sink, RecordAccess value)
			{
				sink.EnterSequence();
				Value<int>.Save(sink, value.FileStartRecord);
				Value<uint>.Save(sink, value.RequestedRecordCount);
				sink.LeaveSequence();
			}
		}
	}
}
