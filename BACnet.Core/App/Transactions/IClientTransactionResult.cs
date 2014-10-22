using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Core.App.Transactions
{
    public interface IClientTransactionResult
    {
        /// <summary>
        /// Determines whether the client transaction
        /// result has any content (eg. if it was a complex ack)
        /// </summary>
        /// <returns>True if the result has content, false otherwise</returns>
        bool HasContent();

        /// <summary>
        /// Gets the content stream for the transaction
        /// </summary>
        /// <returns>The stream</returns>
        Stream GetStream();
    }
}
