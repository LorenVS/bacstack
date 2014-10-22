using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.Explorer.Core.Models
{
    public class Session : PropertyChangedBase
    {
        /// <summary>
        /// The name of the session
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { changeProperty(ref _name, value, "Name"); }
        }
        private string _name;

        /// <summary>
        /// The processes that are configured for the session
        /// </summary>
        public ObservableCollection<Process> Processes { get; private set; }

        public Session()
        {
            this.Name = Constants.SessionDefaultName;
            this.Processes = new ObservableCollection<Process>();
        }
    }
}
