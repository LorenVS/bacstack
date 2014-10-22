using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public  partial class AtomicWriteFileRequest
	{
		public ObjectId FileIdentifier { get; private set; }

		public AccessMethodType AccessMethod { get; private set; }

		public AtomicWriteFileRequest(ObjectId fileIdentifier, AccessMethodType accessMethod)
		{
			this.FileIdentifier = fileIdentifier;
			this.AccessMethod = accessMethod;
		}

		public static readonly ISchema Schema = new SequenceSchema(false, 
			new FieldSchema("FileIdentifier", 255, Value<ObjectId>.Schema),
			new FieldSchema("AccessMethod", 255, Value<AccessMethodType>.Schema));

		public static AtomicWriteFileRequest Load(IValueStream stream)
		{
			stream.EnterSequence();
			var fileIdentifier = Value<ObjectId>.Load(stream);
			var accessMethod = Value<AccessMethodType>.Load(stream);
			stream.LeaveSequence();
			return new AtomicWriteFileRequest(fileIdentifier, accessMethod);
		}

		public static void Save(IValueSink sink, AtomicWriteFileRequest value)
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

			public static AccessMethodType NewStreamAccess(int fileStartPosition, byte[] fileData)
			{
				return new StreamAccess(fileStartPosition, fileData);
			}

			public bool IsRecordAccess { get { return this.Tag == Tags.RecordAccess; } }

			public RecordAccess AsRecordAccess { get { return (RecordAccess)this; } }

			public static AccessMethodType NewRecordAccess(int fileStartRecord, uint recordCount, ReadOnlyArray<byte[]> fileRecordData)
			{
				return new RecordAccess(fileStartRecord, recordCount, fileRecordData);
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

			public byte[] FileData { get; private set; }

			public StreamAccess(int fileStartPosition, byte[] fileData)
			{
				this.FileStartPosition = fileStartPosition;
				this.FileData = fileData;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("FileStartPosition", 255, Value<int>.Schema),
				new FieldSchema("FileData", 255, Value<byte[]>.Schema));

			public static new StreamAccess Load(IValueStream stream)
			{
				stream.EnterSequence();
				var fileStartPosition = Value<int>.Load(stream);
				var fileData = Value<byte[]>.Load(stream);
				stream.LeaveSequence();
				return new StreamAccess(fileStartPosition, fileData);
			}

			public static void Save(IValueSink sink, StreamAccess value)
			{
				sink.EnterSequence();
				Value<int>.Save(sink, value.FileStartPosition);
				Value<byte[]>.Save(sink, value.FileData);
				sink.LeaveSequence();
			}
		}
		public  partial class RecordAccess : AccessMethodType
		{
			public override Tags Tag { get { return Tags.RecordAccess; } }

			public int FileStartRecord { get; private set; }

			public uint RecordCount { get; private set; }

			public ReadOnlyArray<byte[]> FileRecordData { get; private set; }

			public RecordAccess(int fileStartRecord, uint recordCount, ReadOnlyArray<byte[]> fileRecordData)
			{
				this.FileStartRecord = fileStartRecord;
				this.RecordCount = recordCount;
				this.FileRecordData = fileRecordData;
			}

			public static readonly new ISchema Schema = new SequenceSchema(false, 
				new FieldSchema("FileStartRecord", 255, Value<int>.Schema),
				new FieldSchema("RecordCount", 255, Value<uint>.Schema),
				new FieldSchema("FileRecordData", 255, Value<ReadOnlyArray<byte[]>>.Schema));

			public static new RecordAccess Load(IValueStream stream)
			{
				stream.EnterSequence();
				var fileStartRecord = Value<int>.Load(stream);
				var recordCount = Value<uint>.Load(stream);
				var fileRecordData = Value<ReadOnlyArray<byte[]>>.Load(stream);
				stream.LeaveSequence();
				return new RecordAccess(fileStartRecord, recordCount, fileRecordData);
			}

			public static void Save(IValueSink sink, RecordAccess value)
			{
				sink.EnterSequence();
				Value<int>.Save(sink, value.FileStartRecord);
				Value<uint>.Save(sink, value.RecordCount);
				Value<ReadOnlyArray<byte[]>>.Save(sink, value.FileRecordData);
				sink.LeaveSequence();
			}
		}
	}
}
