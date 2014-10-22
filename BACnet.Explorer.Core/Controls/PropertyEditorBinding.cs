using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Client;
using BACnet.Types;

namespace BACnet.Explorer.Core.Controls
{
    public class PropertyEditorBinding<TProp> : IEditorBinding
    {
        /// <summary>
        /// The device instance of the source property
        /// </summary>
        public uint DeviceInstance { get; private set; }

        /// <summary>
        /// The source property reference
        /// </summary>
        public ObjectPropertyReference Reference { get; private set; }

        /// <summary>
        /// The client to use for reading property values
        /// </summary>
        public Client.Client Client { get; private set; }

        /// <summary>
        /// The editor to bind to
        /// </summary>
        public IEditor<TProp> Editor { get; private set; }
        
        /// <summary>
        /// Creates a new property editor binding instance
        /// </summary>
        /// <param name="deviceInstance">The device instance of the source property</param>
        /// <param name="reference">The source property reference</param>
        /// <param name="client">The client to use for reading property values</param>
        /// <param name="editor">The editor to bind to</param>
        public PropertyEditorBinding(uint deviceInstance, ObjectPropertyReference reference, Client.Client client, IEditor<TProp> editor)
        {
            this.DeviceInstance = deviceInstance;
            this.Reference = reference;
            this.Client = client;
            this.Editor = editor;
        }

        /// <summary>
        /// Refreshes the editor value from the source property
        /// </summary>
        public void Refresh()
        {
            Client.ReadQueue.Enqueue<TProp>(DeviceInstance, Reference,
                value => Editor.PristineValue = value);
        }

        /// <summary>
        /// Updates the source property using the editor value
        /// </summary>
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
