using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae.Objects
{
    public interface IGroup
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.ListOfGroupMembers)]
        ReadOnlyArray<ReadAccessSpecification> ListOfGroupMembers { get; }

        [PropertyId(PropertyIdentifier.PresentValue)]
        ReadOnlyArray<ReadAccessResult> PresentValue { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
