using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IAccumulator
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.PresentValue)]
        uint PresentValue { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.DeviceType)]
        Option<string> DeviceType { get; }

        [PropertyId(PropertyIdentifier.StatusFlags)]
        StatusFlags StatusFlags { get; }

        [PropertyId(PropertyIdentifier.EventState)]
        EventState EventState { get; }

        [PropertyId(PropertyIdentifier.Reliability)]
        Option<Reliability> Reliability { get; }

        [PropertyId(PropertyIdentifier.OutOfService)]
        bool OutOfService { get; }

        [PropertyId(PropertyIdentifier.Scale)]
        Scale Scale { get; }

        [PropertyId(PropertyIdentifier.Units)]
        EngineeringUnits Units { get; }

        [PropertyId(PropertyIdentifier.Prescale)]
        Option<Prescale> Prescale { get; }

        [PropertyId(PropertyIdentifier.MaxPresValue)]
        uint MaxPresValue { get; }

        [PropertyId(PropertyIdentifier.ValueChangeTime)]
        Option<DateTime> ValueChangeTime { get; }

        [PropertyId(PropertyIdentifier.ValueBeforeChange)]
        Option<uint> ValueBeforeChange { get; }

        [PropertyId(PropertyIdentifier.ValueSet)]
        Option<uint> ValueSet { get; }

        [PropertyId(PropertyIdentifier.LoggingRecord)]
        Option<AccumulatorRecord> LoggingRecord { get; }

        [PropertyId(PropertyIdentifier.LoggingObject)]
        Option<ObjectId> LoggingObject { get; }

        [PropertyId(PropertyIdentifier.PulseRate)]
        Option<uint> PulseRate { get; }

        [PropertyId(PropertyIdentifier.HighLimit)]
        Option<uint> HighLimit { get; }

        [PropertyId(PropertyIdentifier.LowLimit)]
        Option<uint> LowLimit { get; }

        [PropertyId(PropertyIdentifier.LimitMonitoringInterval)]
        Option<uint> LimitMonitoringInterval { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.TimeDelay)]
        Option<uint> TimeDelay { get; }

        [PropertyId(PropertyIdentifier.LimitEnable)]
        Option<LimitEnable> LimitEnable { get; }

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
