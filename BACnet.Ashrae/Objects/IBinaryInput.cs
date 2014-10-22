using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IBinaryInput
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.PresentValue)]
        BinaryPV PresentValue { get; }

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

        [PropertyId(PropertyIdentifier.Polarity)]
        Polarity Polarity { get; }

        [PropertyId(PropertyIdentifier.InactiveText)]
        Option<string> InactiveText { get; }

        [PropertyId(PropertyIdentifier.ActiveText)]
        Option<string> ActiveText { get; }

        [PropertyId(PropertyIdentifier.ChangeOfStateTime)]
        Option<DateAndTime> ChangeOfStateTime { get; }

        [PropertyId(PropertyIdentifier.ChangeOfStateCount)]
        Option<uint> ChangeOfStateCount { get; }

        [PropertyId(PropertyIdentifier.TimeOfStateCountReset)]
        Option<DateAndTime> TimeOfStateCountReset { get; }

        [PropertyId(PropertyIdentifier.ElapsedActiveTime)]
        Option<uint> ElapsedActiveTime { get; }

        [PropertyId(PropertyIdentifier.TimeOfActiveTimeReset)]
        Option<DateAndTime> TimeOfActiveTimeReset { get; }

        [PropertyId(PropertyIdentifier.TimeDelay)]
        Option<uint> TimeDelay { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.AlarmValue)]
        Option<BinaryPV> AlarmValue { get; }

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
