using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IPulseConverter
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.PresentValue)]
        float PresentValue { get; }

        [PropertyId(PropertyIdentifier.InputReference)]
        Option<ObjectPropertyReference> InputReference { get; }

        [PropertyId(PropertyIdentifier.StatusFlags)]
        StatusFlags StatusFlags { get; }

        [PropertyId(PropertyIdentifier.EventState)]
        EventState EventState { get; }

        [PropertyId(PropertyIdentifier.Reliability)]
        Option<Reliability> Reliability { get; }

        [PropertyId(PropertyIdentifier.OutOfService)]
        bool OutOfService { get; }

        [PropertyId(PropertyIdentifier.Units)]
        EngineeringUnits Units { get; }

        [PropertyId(PropertyIdentifier.ScaleFactor)]
        float ScaleFactor { get; }

        [PropertyId(PropertyIdentifier.AdjustValue)]
        float AdjustValue { get; }

        [PropertyId(PropertyIdentifier.Count)]
        uint Count { get; }

        [PropertyId(PropertyIdentifier.UpdateTime)]
        DateAndTime UpdateTime { get; }

        [PropertyId(PropertyIdentifier.CountChangeTime)]
        DateAndTime CountChangeTime { get; }

        [PropertyId(PropertyIdentifier.CountBeforeChange)]
        uint CountBeforeChange { get; }

        [PropertyId(PropertyIdentifier.CovIncrement)]
        Option<float> CovIncrement { get; }

        [PropertyId(PropertyIdentifier.CovPeriod)]
        Option<uint> CovPeriod { get; }

        [PropertyId(PropertyIdentifier.NotificationClass)]
        Option<uint> NotificationClass { get; }

        [PropertyId(PropertyIdentifier.TimeDelay)]
        Option<uint> TimeDelay { get; }

        [PropertyId(PropertyIdentifier.HighLimit)]
        Option<float> HighLimit { get; }

        [PropertyId(PropertyIdentifier.LowLimit)]
        Option<float> LowLimit { get; }

        [PropertyId(PropertyIdentifier.Deadband)]
        Option<float> Deadband { get; }

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
