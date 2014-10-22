using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Client.Descriptors
{
    /// <summary>
    /// Constructs a new instance of the descriptor
    /// </summary>
    /// <param name="vendorId">The vendor id of the descriptor</param>
    /// <param name="deviceInstance">The device instance of the descriptor</param>
    /// <param name="objectIdentifier">The object identifier of the descriptor</param>
    /// <returns>The descriptor instance</returns>
    public delegate ObjectInfo DescriptorFactory(ushort vendorId, uint deviceInstance, ObjectId objectIdentifier);
}
