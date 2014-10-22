using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using BACnet.Ashrae;
using BACnet.Types;
using BACnet.Client;

namespace BACnet.Explorer.Core.Controls
{
    public interface IEditor<T> : IEditor
    {
        /// <summary>
        /// The pristine value that
        /// is being edited
        /// </summary>
        T PristineValue { get; set; }

        /// <summary>
        /// The current value of the editor
        /// </summary>
        T CurrentValue { get; set; }
    }

    public interface IEditor
    {
        /// <summary>
        /// The eto control to render for the editor
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Whethe or not a value has been
        /// loaded from the remote propety
        /// </summary>
        bool Loaded { get; }

        /// <summary>
        /// Enables or disables the editor
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Whether or not this editor's value has changed
        /// from the property's value
        /// </summary>
        bool Changed { get; }

        /// <summary>
        /// Resets the form back to its pristine state
        /// </summary>
        void Reset();
    }
}
