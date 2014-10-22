using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class Token
    {
        /// <summary>
        /// The type of token
        /// </summary>
        public TokenType Type { get; private set; }

        /// <summary>
        /// The token's value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Constructs a new token instance
        /// </summary>
        /// <param name="type">The type of token</param>
        /// <param name="value">The token's value</param>
        public Token(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

    }
}
