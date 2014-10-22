using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using BACnet.Core;

namespace BACnet.Explorer.Core.Models
{
    public abstract class Process : PropertyChangedBase, IListItem
    {
        /// <summary>
        /// The name of the process
        /// </summary>
        public string Name {
            get { return _name; }
            set
            {
                if (changeProperty(ref _name, value, "Name"))
                {
                    // dependent properties
                    onPropertyChanged("Key");
                    onPropertyChanged("Text");
                }
            }
        }
        private string _name;
        string IListItem.Key { get { return Name; } }
        string IListItem.Text { get { return Name; } set { Name = value; } }

        /// <summary>
        /// The process id of the process
        /// </summary>
        public int ProcessId
        {
            get { return _processId; }
            set { changeProperty(ref _processId, value, "PropertyId"); }
        }
        private int _processId;

        /// <summary>
        /// Creates a process options instance
        /// </summary>
        /// <returns>The process options instance</returns>
        public abstract IProcessOptions CreateOptions();
    }
}
