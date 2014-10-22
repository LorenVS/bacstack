using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;
using BACnet.Client;
using BACnet.Types;
using BACnet.Explorer.Core.Controls;
using BACnet.Explorer.Core.Extensibility;

namespace BACnet.Explorer.Core
{
    public static class Extensions
    {
        /// <summary>
        /// Creates an editor for an object property
        /// </summary>
        /// <typeparam name="TObj">The type of object</typeparam>
        /// <typeparam name="TProp">The type of the property</typeparam>
        /// <param name="obj">The object handle</param>
        /// <param name="expression">The property expression</param>
        /// <returns>The editor</returns>
        public static PropertyEditorBinding<TProp> BindEditor<TObj, TProp>(
            this ObjectHandle<TObj> obj,
            Editor<TProp> editor,
            Expression<Func<TObj, TProp>> expression)
        {
            var reference = ObjectHelpers.GetObjectPropertyReference(
                obj.ObjectIdentifier,
                expression);

            var binding = new PropertyEditorBinding<TProp>(
                obj.DeviceInstance,
                reference,
                obj.Client,
                editor);

            return binding;            
        }
    }
}
