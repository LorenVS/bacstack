using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core.Extensibility
{
    public interface ICustomTool
    {
        /// <summary>
        /// The tool group that the tool belongs to
        /// </summary>
        string Group { get; }

        /// <summary>
        /// The name of the tool, which goes into the tools menu
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Whether or not the tool should be active
        /// </summary>
        /// <returns>True if the tool is active, false otherwise</returns>
        bool Active();

        /// <summary>
        /// Launches the tool
        /// </summary>
        void Launch();
    }
}
