using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;

namespace BACnet.Core.App.Transactions
{
    public interface IClientTransactionHandle
    {
        /// <summary>
        /// Gets the result of the transaction
        /// </summary>
        /// <returns>The response stream</returns>
        Stream GetResponse();

        /// <summary>
        /// Aborts the transaction
        /// </summary>
        void Abort(AbortReason reason);
    }
}
