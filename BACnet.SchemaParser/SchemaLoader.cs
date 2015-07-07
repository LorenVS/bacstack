using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.SchemaParser
{
    public class SchemaLoader
    {
        /// <summary>
        /// The registrar to load schemas into
        /// </summary>
        private readonly Registrar _registrar;
        
        /// <summary>
        /// Constructs a new schema loader instance
        /// </summary>
        /// <param name="registrar">The registrar to load schemas into</param>
        public SchemaLoader(Registrar registrar)
        {
            Contract.Requires(registrar != null);
            _registrar = registrar;
        }

        /// <summary>
        /// Loads a schema information from a parser
        /// </summary>
        /// <param name="parser">The parser to load from</param>
        public void LoadFromParser(Parser parser)
        {
            NamedType type = parser.Next();
            while(type != null)
            {
                
                type = parser.Next();
            }
        }

        /// <summary>
        /// Translates a type definition to a schema
        /// </summary>
        /// <param name="definition">The type definition</param>
        /// <returns>The schema instance, or null if the type can't be resolved yet</returns>
        private ISchema _definitionToSchema(TypeDefinition definition)
        {
        }

        
    }
}
