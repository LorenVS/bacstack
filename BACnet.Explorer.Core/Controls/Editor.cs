using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace BACnet.Explorer.Core.Controls
{
    public abstract class Editor<T> : IEditor<T>
    {
        /// <summary>
        /// The control to render for the editor
        /// </summary>
        public abstract Control Control { get; }

        /// <summary>
        /// The current value of the UI control
        /// </summary>
        protected abstract T controlValue { get; set; }

        /// <summary>
        /// True if a pristine value has already been
        /// supplied to the editor, false otherwise
        /// </summary>
        public bool Loaded
        {
            get { return _loaded; }
        }

        /// <summary>
        /// Whether or not the current value of the editor has changed
        /// from its pristine value
        /// </summary>
        public bool Changed
        {
            get
            {
                if (!_loaded)
                    return false;
                return Comparer<T>.Default.Compare(_pristineValue, controlValue) != 0;
            }
        }
        
        /// <summary>
        /// Whether or not the editor
        /// is enabled for editing
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;   
            }
            set
            {
                _enabled = value;
                Control.Enabled = (_enabled && _loaded);
            }
        }
        private bool _enabled;

        /// <summary>
        /// The current value of the editor
        /// </summary>
        public T CurrentValue
        {
            get { return controlValue; }
            set { controlValue = value; }
        }

        /// <summary>
        /// The pristine value that is being
        /// edited
        /// </summary>
        public T PristineValue
        {
            get { return _pristineValue; }
            set
            {
                _pristineValue = value;
                if (!_loaded || !Changed)
                    controlValue = value;
                _loaded = true;
                Control.Enabled = (_enabled && _loaded);
            }
        }

        /// <summary>
        /// Whether or not a pristine value has been loaded
        /// </summary>
        private bool _loaded;

        /// <summary>
        /// The pristine value of the editor
        /// </summary>
        private T _pristineValue;

        /// <summary>
        /// Resets the editor back to its pristine state
        /// </summary>
        public void Reset()
        {
            if(_loaded)
                controlValue = PristineValue;
        }
    }
}
