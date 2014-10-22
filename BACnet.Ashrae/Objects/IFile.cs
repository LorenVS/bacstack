using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;


namespace BACnet.Ashrae.Objects
{
    public interface IFile
    {
        [PropertyId(PropertyIdentifier.ObjectIdentifier)]
        ObjectId ObjectIdentifier { get; }

        [PropertyId(PropertyIdentifier.ObjectName)]
        string ObjectName { get; }

        [PropertyId(PropertyIdentifier.ObjectType)]
        ObjectType ObjectType { get; }

        [PropertyId(PropertyIdentifier.Description)]
        Option<string> Description { get; }

        [PropertyId(PropertyIdentifier.FileType)]
        string FileType { get; }

        [PropertyId(PropertyIdentifier.FileSize)]
        uint FileSize { get; }

        [PropertyId(PropertyIdentifier.ModificationDate)]
        DateAndTime ModificationDate { get; }

        [PropertyId(PropertyIdentifier.Archive)]
        bool Archive { get; }

        [PropertyId(PropertyIdentifier.ReadOnly)]
        bool ReadOnly { get; }

        [PropertyId(PropertyIdentifier.FileAccessMethod)]
        FileAccessMethod FileAccessMethod { get; }

        [PropertyId(PropertyIdentifier.RecordCount)]
        Option<uint> RecordCount { get; }

        [PropertyId(PropertyIdentifier.ProfileName)]
        Option<string> ProfileName { get; }
    }
}
