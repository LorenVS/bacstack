using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using BACnet.Ashrae;
using BACnet.Types;
using BACnet.Explorer.Core.Controls;


namespace BACnet.Explorer.Core.Extensibility
{
    public interface IEditorProvider
    {
        /// <summary>
        /// Whether or not this editor provider provides
        /// an editor for the supplied type
        /// </summary>
        /// <typeparam name="T">The type of editor desired</typeparam>
        /// <returns>True if an editor can be provided for the type, false otherwise</returns>
        bool ProvidesEditorFor<T>();

        /// <summary>
        /// Creates a new editor
        /// </summary>
        /// <typeparam name="T">The type of editor desired</typeparam>
        /// <returns>The editor instance</returns>
        IEditor<T> CreateEditor<T>();
    }
}
