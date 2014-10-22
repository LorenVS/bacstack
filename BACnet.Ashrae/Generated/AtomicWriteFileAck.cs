using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class AtomicWriteFileAck
	{
		public abstract Tags Tag { get; }

		public bool IsFileStartPosition { get { return this.Tag == Tags.FileStartPosition; } }

		public int AsFileStartPosition { get { return ((FileStartPositionWrapper)this).Item; } }

		public static AtomicWriteFileAck NewFileStartPosition(int fileStartPosition)
		{
			return new FileStartPositionWrapper(fileStartPosition);
		}

		public bool IsFileStartRecord { get { return this.Tag == Tags.FileStartRecord; } }

		public int AsFileStartRecord { get { return ((FileStartRecordWrapper)this).Item; } }

		public static AtomicWriteFileAck NewFileStartRecord(int fileStartRecord)
		{
			return new FileStartRecordWrapper(fileStartRecord);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("FileStartPosition", 0, Value<int>.Schema),
			new FieldSchema("FileStartRecord", 1, Value<int>.Schema));

		public static AtomicWriteFileAck Load(IValueStream stream)
		{
			AtomicWriteFileAck ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.FileStartPosition:
					ret = Value<FileStartPositionWrapper>.Load(stream);
					break;
				case Tags.FileStartRecord:
					ret = Value<FileStartRecordWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, AtomicWriteFileAck value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.FileStartPosition:
					Value<FileStartPositionWrapper>.Save(sink, (FileStartPositionWrapper)value);
					break;
				case Tags.FileStartRecord:
					Value<FileStartRecordWrapper>.Save(sink, (FileStartRecordWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			FileStartPosition = 0,
			FileStartRecord = 1
		}

		public  partial class FileStartPositionWrapper : AtomicWriteFileAck
		{
			public override Tags Tag { get { return Tags.FileStartPosition; } }

			public int Item { get; private set; }

			public FileStartPositionWrapper(int item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<int>.Schema;

			public static new FileStartPositionWrapper Load(IValueStream stream)
			{
				var temp = Value<int>.Load(stream);
				return new FileStartPositionWrapper(temp);
			}

			public static void Save(IValueSink sink, FileStartPositionWrapper value)
			{
				Value<int>.Save(sink, value.Item);
			}

		}

		public  partial class FileStartRecordWrapper : AtomicWriteFileAck
		{
			public override Tags Tag { get { return Tags.FileStartRecord; } }

			public int Item { get; private set; }

			public FileStartRecordWrapper(int item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<int>.Schema;

			public static new FileStartRecordWrapper Load(IValueStream stream)
			{
				var temp = Value<int>.Load(stream);
				return new FileStartRecordWrapper(temp);
			}

			public static void Save(IValueSink sink, FileStartRecordWrapper value)
			{
				Value<int>.Save(sink, value.Item);
			}

		}
	}
}
