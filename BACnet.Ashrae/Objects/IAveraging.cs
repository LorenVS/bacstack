using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IAveraging
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.MinimumValue)]
        float MinimumValue { get; }

        [PropertyId(PropertyIdentifier.MinimumValueTimestamp)]
        Option<DateTime> MinimumValueTimestamp { get; }

        [PropertyId(PropertyIdentifier.AverageValue)]
        Option<float> AverageValue { get; }

        [PropertyId(PropertyIdentifier.VarianceValue)]
        Option<float> VarianceValue { get; }

        [PropertyId(PropertyIdentifier.MaximumValue)]
        float MaximumValue { get; }

        [PropertyId(PropertyIdentifier.MaximumValueTimestamp)]
        Option<DateTime> MaximumValueTimestamp { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.AttemptedSamples)]
        uint AttemptedSamples { get; }

        [PropertyId(PropertyIdentifier.ValidSamples)]
        uint ValidSamples { get; }

        [PropertyId(PropertyIdentifier.ObjectPropertyReference)]
        DeviceObjectPropertyReference ObjectPropertyReference { get; }

        [PropertyId(PropertyIdentifier.WindowInterval)]
        uint WindowInterval { get; }

        [PropertyId(PropertyIdentifier.WindowSamples)]
        uint WindowSamples { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
