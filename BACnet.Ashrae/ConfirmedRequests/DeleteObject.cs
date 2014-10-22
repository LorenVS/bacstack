using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Ashrae
{
    public partial class DeleteObjectRequest : IConfirmedRequest
    {
        /// <summary>
        /// The service choice for delete object requests
        /// </summary>
        public ConfirmedServiceChoice ServiceChoice { get { return ConfirmedServiceChoice.DeleteObject; } }
    }
}
