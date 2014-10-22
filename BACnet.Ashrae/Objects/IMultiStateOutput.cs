using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IMultiStateOutput
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

        [PropertyId(PropertyIdentifier.NumberOfStates)]
        uint NumberOfStates { get; }

        [PropertyId(PropertyIdentifier.StateText)]
        Option<ReadOnlyArray<string>> StateText { get; }

        [PropertyId(PropertyIdentifier.PriorityArray)]
        PriorityArray PriorityArray { get; }

        [PropertyId(PropertyIdentifier.RelinquishDefault)]
        uint RelinquishDefault { get; }

        [PropertyId(PropertyIdentifier.TimeDelay)]
        Option<uint> TimeDelay { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.FeedbackValue)]
        Option<uint> FeedbackValue { get; }

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
