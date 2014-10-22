using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core.Controls
{
    public interface IEditorBinding
    {
        /// <summary>
        /// Refreshes the source value of the binding
        /// </summary>
        void Refresh();

        /// <summary>
        /// Updates the source value with the current editor value
        /// </summary>
        void Update();
    }
}
