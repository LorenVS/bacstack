using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Types
{
    public interface IGenericValueProvider
    {
        /// <summary>
        /// Creates a stream for converting a generic value
        /// to a specific value
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="schema">The schema for the output type</param>
        /// <returns>The stream object</returns>
        IValueStream CreateStream(GenericValue value, ISchema schema);
    }
}
