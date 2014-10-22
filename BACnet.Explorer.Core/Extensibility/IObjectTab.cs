using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;
using BACnet.Client;
using BACnet.Client.Descriptors;

namespace BACnet.Explorer.Core.Extensibility
{
    public interface IObjectTab
    {
        /// <summary>
        /// The order in which the tab should appear
        /// </summary>
        /// 
        int Order { get; }
        /// <summary>
        /// Determines whether this object tab
        /// should be created for a specific object
        /// </summary>
        /// <param name="objectInfo">The object info of the object</param>
        /// <returns>True if the tab should be created, false otherwise</returns>
        bool Active(ObjectInfo objectInfo);

        /// <summary>
        /// Creates the tab page
        /// </summary>
        /// <param name="objectInfo">The object to create the page for</param>
        /// <returns>The tab page</returns>
        TabPage Create(ObjectInfo objectInfo);
    }
}
