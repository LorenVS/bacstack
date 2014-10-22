using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Ashrae;

namespace BACnet.Client
{
    public class ErrorOr<T>
    {
        /// <summary>
        /// Whether or not this object contains an error
        /// </summary>
        public bool IsError { get; private set; }

        /// <summary>
        /// Whether or not this object contains a value
        /// </summary>
        public bool IsValue { get { return !IsError; } }

        /// <summary>
        /// The error contained in this object if <see cref="IsError"/>
        /// is true
        /// </summary>
        public Error Error { get; private set; }

        /// <summary>
        /// The value contained in this object if <see cref="IsError"/>
        /// is false
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Constructs a new error
        /// </summary>
        /// <param name="error">The error to wrap</param>
        public ErrorOr(Error error)
        {
            this.IsError = true;
            this.Error = error;
        }

        /// <summary>
        /// Constructs a new value
        /// </summary>
        /// <param name="value">The value to wrap</param>
        public ErrorOr(T value)
        {
            this.IsError = false;
            this.Value = value;
        }
    }
}
