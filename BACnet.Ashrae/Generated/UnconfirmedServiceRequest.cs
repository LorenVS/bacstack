using System;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Ashrae
{
	public abstract  partial class UnconfirmedServiceRequest
	{
		public abstract Tags Tag { get; }

		public bool IsIAm { get { return this.Tag == Tags.IAm; } }

		public IAmRequest AsIAm { get { return ((IAmWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewIAm(IAmRequest iAm)
		{
			return new IAmWrapper(iAm);
		}

		public bool IsIHave { get { return this.Tag == Tags.IHave; } }

		public IHaveRequest AsIHave { get { return ((IHaveWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewIHave(IHaveRequest iHave)
		{
			return new IHaveWrapper(iHave);
		}

		public bool IsUnconfirmedCOVNotification { get { return this.Tag == Tags.UnconfirmedCOVNotification; } }

		public UnconfirmedCOVNotificationRequest AsUnconfirmedCOVNotification { get { return ((UnconfirmedCOVNotificationWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewUnconfirmedCOVNotification(UnconfirmedCOVNotificationRequest unconfirmedCOVNotification)
		{
			return new UnconfirmedCOVNotificationWrapper(unconfirmedCOVNotification);
		}

		public bool IsUnconfirmedEventNotification { get { return this.Tag == Tags.UnconfirmedEventNotification; } }

		public UnconfirmedEventNotificationRequest AsUnconfirmedEventNotification { get { return ((UnconfirmedEventNotificationWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewUnconfirmedEventNotification(UnconfirmedEventNotificationRequest unconfirmedEventNotification)
		{
			return new UnconfirmedEventNotificationWrapper(unconfirmedEventNotification);
		}

		public bool IsUnconfirmedPrivateTransfer { get { return this.Tag == Tags.UnconfirmedPrivateTransfer; } }

		public UnconfirmedPrivateTransferRequest AsUnconfirmedPrivateTransfer { get { return ((UnconfirmedPrivateTransferWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewUnconfirmedPrivateTransfer(UnconfirmedPrivateTransferRequest unconfirmedPrivateTransfer)
		{
			return new UnconfirmedPrivateTransferWrapper(unconfirmedPrivateTransfer);
		}

		public bool IsUnconfirmedTextMessage { get { return this.Tag == Tags.UnconfirmedTextMessage; } }

		public UnconfirmedTextMessageRequest AsUnconfirmedTextMessage { get { return ((UnconfirmedTextMessageWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewUnconfirmedTextMessage(UnconfirmedTextMessageRequest unconfirmedTextMessage)
		{
			return new UnconfirmedTextMessageWrapper(unconfirmedTextMessage);
		}

		public bool IsTimeSynchronization { get { return this.Tag == Tags.TimeSynchronization; } }

		public TimeSynchronizationRequest AsTimeSynchronization { get { return ((TimeSynchronizationWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewTimeSynchronization(TimeSynchronizationRequest timeSynchronization)
		{
			return new TimeSynchronizationWrapper(timeSynchronization);
		}

		public bool IsWhoHas { get { return this.Tag == Tags.WhoHas; } }

		public WhoHasRequest AsWhoHas { get { return ((WhoHasWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewWhoHas(WhoHasRequest whoHas)
		{
			return new WhoHasWrapper(whoHas);
		}

		public bool IsWhoIs { get { return this.Tag == Tags.WhoIs; } }

		public WhoIsRequest AsWhoIs { get { return ((WhoIsWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewWhoIs(WhoIsRequest whoIs)
		{
			return new WhoIsWrapper(whoIs);
		}

		public bool IsUtcTimeSynchronization { get { return this.Tag == Tags.UtcTimeSynchronization; } }

		public UTCTimeSynchronizationRequest AsUtcTimeSynchronization { get { return ((UtcTimeSynchronizationWrapper)this).Item; } }

		public static UnconfirmedServiceRequest NewUtcTimeSynchronization(UTCTimeSynchronizationRequest utcTimeSynchronization)
		{
			return new UtcTimeSynchronizationWrapper(utcTimeSynchronization);
		}

		public static readonly ISchema Schema = new ChoiceSchema(false, 
			new FieldSchema("IAm", 0, Value<IAmRequest>.Schema),
			new FieldSchema("IHave", 1, Value<IHaveRequest>.Schema),
			new FieldSchema("UnconfirmedCOVNotification", 2, Value<UnconfirmedCOVNotificationRequest>.Schema),
			new FieldSchema("UnconfirmedEventNotification", 3, Value<UnconfirmedEventNotificationRequest>.Schema),
			new FieldSchema("UnconfirmedPrivateTransfer", 4, Value<UnconfirmedPrivateTransferRequest>.Schema),
			new FieldSchema("UnconfirmedTextMessage", 5, Value<UnconfirmedTextMessageRequest>.Schema),
			new FieldSchema("TimeSynchronization", 6, Value<TimeSynchronizationRequest>.Schema),
			new FieldSchema("WhoHas", 7, Value<WhoHasRequest>.Schema),
			new FieldSchema("WhoIs", 8, Value<WhoIsRequest>.Schema),
			new FieldSchema("UtcTimeSynchronization", 9, Value<UTCTimeSynchronizationRequest>.Schema));

		public static UnconfirmedServiceRequest Load(IValueStream stream)
		{
			UnconfirmedServiceRequest ret = null;
			Tags tag = (Tags)stream.EnterChoice();
			switch(tag)
			{
				case Tags.IAm:
					ret = Value<IAmWrapper>.Load(stream);
					break;
				case Tags.IHave:
					ret = Value<IHaveWrapper>.Load(stream);
					break;
				case Tags.UnconfirmedCOVNotification:
					ret = Value<UnconfirmedCOVNotificationWrapper>.Load(stream);
					break;
				case Tags.UnconfirmedEventNotification:
					ret = Value<UnconfirmedEventNotificationWrapper>.Load(stream);
					break;
				case Tags.UnconfirmedPrivateTransfer:
					ret = Value<UnconfirmedPrivateTransferWrapper>.Load(stream);
					break;
				case Tags.UnconfirmedTextMessage:
					ret = Value<UnconfirmedTextMessageWrapper>.Load(stream);
					break;
				case Tags.TimeSynchronization:
					ret = Value<TimeSynchronizationWrapper>.Load(stream);
					break;
				case Tags.WhoHas:
					ret = Value<WhoHasWrapper>.Load(stream);
					break;
				case Tags.WhoIs:
					ret = Value<WhoIsWrapper>.Load(stream);
					break;
				case Tags.UtcTimeSynchronization:
					ret = Value<UtcTimeSynchronizationWrapper>.Load(stream);
					break;
				default:
					throw new Exception();
			}
			stream.LeaveChoice();
			return ret;
		}

		public static void Save(IValueSink sink, UnconfirmedServiceRequest value)
		{
			sink.EnterChoice((byte)value.Tag);
			switch(value.Tag)
			{
				case Tags.IAm:
					Value<IAmWrapper>.Save(sink, (IAmWrapper)value);
					break;
				case Tags.IHave:
					Value<IHaveWrapper>.Save(sink, (IHaveWrapper)value);
					break;
				case Tags.UnconfirmedCOVNotification:
					Value<UnconfirmedCOVNotificationWrapper>.Save(sink, (UnconfirmedCOVNotificationWrapper)value);
					break;
				case Tags.UnconfirmedEventNotification:
					Value<UnconfirmedEventNotificationWrapper>.Save(sink, (UnconfirmedEventNotificationWrapper)value);
					break;
				case Tags.UnconfirmedPrivateTransfer:
					Value<UnconfirmedPrivateTransferWrapper>.Save(sink, (UnconfirmedPrivateTransferWrapper)value);
					break;
				case Tags.UnconfirmedTextMessage:
					Value<UnconfirmedTextMessageWrapper>.Save(sink, (UnconfirmedTextMessageWrapper)value);
					break;
				case Tags.TimeSynchronization:
					Value<TimeSynchronizationWrapper>.Save(sink, (TimeSynchronizationWrapper)value);
					break;
				case Tags.WhoHas:
					Value<WhoHasWrapper>.Save(sink, (WhoHasWrapper)value);
					break;
				case Tags.WhoIs:
					Value<WhoIsWrapper>.Save(sink, (WhoIsWrapper)value);
					break;
				case Tags.UtcTimeSynchronization:
					Value<UtcTimeSynchronizationWrapper>.Save(sink, (UtcTimeSynchronizationWrapper)value);
					break;
				default:
					throw new Exception();
			}
			sink.LeaveChoice();
		}

		public enum Tags : byte
		{
			IAm = 0,
			IHave = 1,
			UnconfirmedCOVNotification = 2,
			UnconfirmedEventNotification = 3,
			UnconfirmedPrivateTransfer = 4,
			UnconfirmedTextMessage = 5,
			TimeSynchronization = 6,
			WhoHas = 7,
			WhoIs = 8,
			UtcTimeSynchronization = 9
		}

		public  partial class IAmWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.IAm; } }

			public IAmRequest Item { get; private set; }

			public IAmWrapper(IAmRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<IAmRequest>.Schema;

			public static new IAmWrapper Load(IValueStream stream)
			{
				var temp = Value<IAmRequest>.Load(stream);
				return new IAmWrapper(temp);
			}

			public static void Save(IValueSink sink, IAmWrapper value)
			{
				Value<IAmRequest>.Save(sink, value.Item);
			}

		}

		public  partial class IHaveWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.IHave; } }

			public IHaveRequest Item { get; private set; }

			public IHaveWrapper(IHaveRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<IHaveRequest>.Schema;

			public static new IHaveWrapper Load(IValueStream stream)
			{
				var temp = Value<IHaveRequest>.Load(stream);
				return new IHaveWrapper(temp);
			}

			public static void Save(IValueSink sink, IHaveWrapper value)
			{
				Value<IHaveRequest>.Save(sink, value.Item);
			}

		}

		public  partial class UnconfirmedCOVNotificationWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.UnconfirmedCOVNotification; } }

			public UnconfirmedCOVNotificationRequest Item { get; private set; }

			public UnconfirmedCOVNotificationWrapper(UnconfirmedCOVNotificationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<UnconfirmedCOVNotificationRequest>.Schema;

			public static new UnconfirmedCOVNotificationWrapper Load(IValueStream stream)
			{
				var temp = Value<UnconfirmedCOVNotificationRequest>.Load(stream);
				return new UnconfirmedCOVNotificationWrapper(temp);
			}

			public static void Save(IValueSink sink, UnconfirmedCOVNotificationWrapper value)
			{
				Value<UnconfirmedCOVNotificationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class UnconfirmedEventNotificationWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.UnconfirmedEventNotification; } }

			public UnconfirmedEventNotificationRequest Item { get; private set; }

			public UnconfirmedEventNotificationWrapper(UnconfirmedEventNotificationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<UnconfirmedEventNotificationRequest>.Schema;

			public static new UnconfirmedEventNotificationWrapper Load(IValueStream stream)
			{
				var temp = Value<UnconfirmedEventNotificationRequest>.Load(stream);
				return new UnconfirmedEventNotificationWrapper(temp);
			}

			public static void Save(IValueSink sink, UnconfirmedEventNotificationWrapper value)
			{
				Value<UnconfirmedEventNotificationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class UnconfirmedPrivateTransferWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.UnconfirmedPrivateTransfer; } }

			public UnconfirmedPrivateTransferRequest Item { get; private set; }

			public UnconfirmedPrivateTransferWrapper(UnconfirmedPrivateTransferRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<UnconfirmedPrivateTransferRequest>.Schema;

			public static new UnconfirmedPrivateTransferWrapper Load(IValueStream stream)
			{
				var temp = Value<UnconfirmedPrivateTransferRequest>.Load(stream);
				return new UnconfirmedPrivateTransferWrapper(temp);
			}

			public static void Save(IValueSink sink, UnconfirmedPrivateTransferWrapper value)
			{
				Value<UnconfirmedPrivateTransferRequest>.Save(sink, value.Item);
			}

		}

		public  partial class UnconfirmedTextMessageWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.UnconfirmedTextMessage; } }

			public UnconfirmedTextMessageRequest Item { get; private set; }

			public UnconfirmedTextMessageWrapper(UnconfirmedTextMessageRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<UnconfirmedTextMessageRequest>.Schema;

			public static new UnconfirmedTextMessageWrapper Load(IValueStream stream)
			{
				var temp = Value<UnconfirmedTextMessageRequest>.Load(stream);
				return new UnconfirmedTextMessageWrapper(temp);
			}

			public static void Save(IValueSink sink, UnconfirmedTextMessageWrapper value)
			{
				Value<UnconfirmedTextMessageRequest>.Save(sink, value.Item);
			}

		}

		public  partial class TimeSynchronizationWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.TimeSynchronization; } }

			public TimeSynchronizationRequest Item { get; private set; }

			public TimeSynchronizationWrapper(TimeSynchronizationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<TimeSynchronizationRequest>.Schema;

			public static new TimeSynchronizationWrapper Load(IValueStream stream)
			{
				var temp = Value<TimeSynchronizationRequest>.Load(stream);
				return new TimeSynchronizationWrapper(temp);
			}

			public static void Save(IValueSink sink, TimeSynchronizationWrapper value)
			{
				Value<TimeSynchronizationRequest>.Save(sink, value.Item);
			}

		}

		public  partial class WhoHasWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.WhoHas; } }

			public WhoHasRequest Item { get; private set; }

			public WhoHasWrapper(WhoHasRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<WhoHasRequest>.Schema;

			public static new WhoHasWrapper Load(IValueStream stream)
			{
				var temp = Value<WhoHasRequest>.Load(stream);
				return new WhoHasWrapper(temp);
			}

			public static void Save(IValueSink sink, WhoHasWrapper value)
			{
				Value<WhoHasRequest>.Save(sink, value.Item);
			}

		}

		public  partial class WhoIsWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.WhoIs; } }

			public WhoIsRequest Item { get; private set; }

			public WhoIsWrapper(WhoIsRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<WhoIsRequest>.Schema;

			public static new WhoIsWrapper Load(IValueStream stream)
			{
				var temp = Value<WhoIsRequest>.Load(stream);
				return new WhoIsWrapper(temp);
			}

			public static void Save(IValueSink sink, WhoIsWrapper value)
			{
				Value<WhoIsRequest>.Save(sink, value.Item);
			}

		}

		public  partial class UtcTimeSynchronizationWrapper : UnconfirmedServiceRequest
		{
			public override Tags Tag { get { return Tags.UtcTimeSynchronization; } }

			public UTCTimeSynchronizationRequest Item { get; private set; }

			public UtcTimeSynchronizationWrapper(UTCTimeSynchronizationRequest item)
			{
				this.Item = item;
			}

			public static readonly new ISchema Schema = Value<UTCTimeSynchronizationRequest>.Schema;

			public static new UtcTimeSynchronizationWrapper Load(IValueStream stream)
			{
				var temp = Value<UTCTimeSynchronizationRequest>.Load(stream);
				return new UtcTimeSynchronizationWrapper(temp);
			}

			public static void Save(IValueSink sink, UtcTimeSynchronizationWrapper value)
			{
				Value<UTCTimeSynchronizationRequest>.Save(sink, value.Item);
			}

		}
	}
}
