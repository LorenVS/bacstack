using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Client;
using BACnet.Client.Descriptors;

namespace BACnet.Explorer.Core.Models
{
    public class MainFormModel : PropertyChangedBase
    {
        /// <summary>
        /// The session that is active on the form
        /// </summary>
        public Session Session
        {
            get { return _session; }
            set { changeProperty(ref _session, value, "Session"); }
        }
        private Session _session;

        /// <summary>
        /// The device info of the currently selected device
        /// </summary>
        public ObjectInfo SelectedDevice
        {
            get { return _selectedDevice; }
            set { changeProperty(ref _selectedDevice, value, "SelectedDevice"); }
        }
        private ObjectInfo _selectedDevice;

        /// <summary>
        /// The object info of the currently selected object
        /// </summary>
        public ObjectInfo SelectedObject
        {
            get { return _selectedObject; }
            set { changeProperty(ref _selectedObject, value, "SelectedObject"); }
        }
        private ObjectInfo _selectedObject;
    }
}
