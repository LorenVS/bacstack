using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface ITrendLog
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.LogEnable)]
        bool LogEnable { get; }

        [PropertyId(PropertyIdentifier.StartTime)]
        Option<DateAndTime> StartTime { get; }

        [PropertyId(PropertyIdentifier.StopTime)]
        Option<DateAndTime> StopTime { get; }

        [PropertyId(PropertyIdentifier.LogDeviceObjectProperty)]
        Option<DeviceObjectPropertyReference> LogDeviceObjectProperty { get; }

        [PropertyId(PropertyIdentifier.LogInterval)]
        Option<uint> LogInterval { get; }

        [PropertyId(PropertyIdentifier.CovResubscriptionInterval)]
        Option<uint> CovResubscriptionInterval { get; }

        [PropertyId(PropertyIdentifier.ClientCovIncrement)]
        Option<ClientCOV> ClientCovIncrement { get; }

        [PropertyId(PropertyIdentifier.StopWhenFull)]
        bool StopWhenFull { get; }

        [PropertyId(PropertyIdentifier.BufferSize)]
        uint BufferSize { get; }

        [PropertyId(PropertyIdentifier.LogBuffer)]
        ReadOnlyArray<LogRecord> LogBuffer { get; }

        [PropertyId(PropertyIdentifier.RecordCount)]
        uint RecordCount { get; }

        [PropertyId(PropertyIdentifier.TotalRecordCount)]
        uint TotalRecordCount { get; }

        [PropertyId(PropertyIdentifier.NotificationThreshold)]
        Option<uint> NotificationThreshold { get; }

        [PropertyId(PropertyIdentifier.RecordsSinceNotification)]
        Option<uint> RecordsSinceNotification { get; }

        [PropertyId(PropertyIdentifier.LastNotifyRecord)]
        Option<uint> LastNotifyRecord { get; }

        [PropertyId(PropertyIdentifier.EventState)]
        EventState EventState { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.EventEnable)]
        Option<EventTransitionBits> EventEnable { get; }

        [PropertyId(PropertyIdentifier.AckedTransitions)]
        Option<EventTransitionBits> AckedTransitions { get; }

        [PropertyId(PropertyIdentifier.NotifyType)]
        Option<NotifyType> NotifyType { get; }

        [PropertyId(PropertyIdentifier.EventTimeStamps)]
        Option<ReadOnlyArray<TimeStamp>> EventTimeStamps { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
