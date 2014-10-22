using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace BACnet.Explorer.Core.Models
{
    public class PortType : PropertyChangedBase, IListItem
    {
        public static PortType[] All()
        {
            return new PortType[]
            {
                new PortType()
                {
                    Name = "Foreign Device Port",
                    Type = typeof(ForeignDevicePortProcess)
                }
            };
        }

        /// <summary>
        /// The name of the port type
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if(changeProperty(ref _name, value, "Name"))
                {
                    onPropertyChanged("Key");
                    onPropertyChanged("Text");
                }

            }
        }
        private string _name;
        string IListItem.Key { get { return Name; } }
        string IListItem.Text { get { return Name; } set { Name = value; } }

        /// <summary>
        /// The type of process for the port
        /// </summary>
        public Type Type
        {
            get { return _type; }
            set { changeProperty(ref _type, value, "Type"); }
        }
        private Type _type;
    }
}
