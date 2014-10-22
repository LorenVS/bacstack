using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;

namespace BACnet.Ashrae
{
    public partial class WritePropertyRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for write property requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.WriteProperty; } }

        public WritePropertyRequest(ObjectId objectIdentifier, PropertyIdentifier propertyIdentifier, Option<uint> propertyArrayIndex, GenericValue propertyValue)
            : this(objectIdentifier, propertyIdentifier, propertyArrayIndex, propertyValue, Option<byte>.None) { }
    }
}
