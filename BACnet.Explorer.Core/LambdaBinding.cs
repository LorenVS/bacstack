using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Eto;

namespace BACnet.Explorer.Core
{
    public class LambdaBinding<TObj, TValue> : IndirectBinding<TValue>
    {
        /// <summary>
        /// The binding getter
        /// </summary>
        private Func<TObj, TValue> _getter;

        /// <summary>
        /// The binding setter
        /// </summary>
        private Action<TObj, TValue> _setter;

        /// <summary>
        /// Constructs a new lambda binding
        /// </summary>
        /// <param name="expr">The property expression</param>
        public LambdaBinding(Func<TObj, TValue> getter, Action<TObj, TValue> setter = null)
        {
            this._getter = getter;
            this._setter = setter;
        }

        /// <summary>
        /// Gets the value of the binding
        /// </summary>
        /// <param name="dataItem">The data item to retrieve the value for</param>
        /// <returns>The binding value</returns>
        protected override TValue InternalGetValue(object dataItem)
        {
            return _getter((TObj)dataItem);
        }

        /// <summary>
        /// Sets the value of the binding
        /// </summary>
        /// <param name="dataItem">The data item to set the value for</param>
        /// <param name="value">The binding value</param>
        protected override void InternalSetValue(object dataItem, TValue value)
        {
            if(_setter != null)
                _setter((TObj)dataItem, value);
        }
    }
}
