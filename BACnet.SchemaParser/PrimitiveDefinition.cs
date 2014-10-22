using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class PrimitiveDefinition : TypeDefinition
    {
        public static readonly PrimitiveDefinition Null = new PrimitiveDefinition(PrimitiveType.Null);
        public static readonly PrimitiveDefinition Boolean = new PrimitiveDefinition(PrimitiveType.Boolean);
        public static readonly PrimitiveDefinition Unsigned8 = new PrimitiveDefinition(PrimitiveType.Unsigned8);
        public static readonly PrimitiveDefinition Unsigned16 = new PrimitiveDefinition(PrimitiveType.Unsigned16);
        public static readonly PrimitiveDefinition Unsigned32 = new PrimitiveDefinition(PrimitiveType.Unsigned32);
        public static readonly PrimitiveDefinition Unsigned64 = new PrimitiveDefinition(PrimitiveType.Unsigned64);
        public static readonly PrimitiveDefinition Signed8 = new PrimitiveDefinition(PrimitiveType.Signed8);
        public static readonly PrimitiveDefinition Signed16 = new PrimitiveDefinition(PrimitiveType.Signed16);
        public static readonly PrimitiveDefinition Signed32 = new PrimitiveDefinition(PrimitiveType.Signed32);
        public static readonly PrimitiveDefinition Signed64 = new PrimitiveDefinition(PrimitiveType.Signed64);
        public static readonly PrimitiveDefinition Float32 = new PrimitiveDefinition(PrimitiveType.Float32);
        public static readonly PrimitiveDefinition Float64 = new PrimitiveDefinition(PrimitiveType.Float64);
        public static readonly PrimitiveDefinition OctetString = new PrimitiveDefinition(PrimitiveType.OctetString);
        public static readonly PrimitiveDefinition CharString = new PrimitiveDefinition(PrimitiveType.CharString);
        public static readonly PrimitiveDefinition BitString8 = new PrimitiveDefinition(PrimitiveType.BitString8);
        public static readonly PrimitiveDefinition BitString24 = new PrimitiveDefinition(PrimitiveType.BitString24);
        public static readonly PrimitiveDefinition BitString56 = new PrimitiveDefinition(PrimitiveType.BitString56);
        public static readonly PrimitiveDefinition Enumerated = new PrimitiveDefinition(PrimitiveType.Enumerated);
        public static readonly PrimitiveDefinition Date = new PrimitiveDefinition(PrimitiveType.Date);
        public static readonly PrimitiveDefinition Time = new PrimitiveDefinition(PrimitiveType.Time);
        public static readonly PrimitiveDefinition ObjectId = new PrimitiveDefinition(PrimitiveType.ObjectId);
        public static readonly PrimitiveDefinition Generic = new PrimitiveDefinition(PrimitiveType.Generic);

        /// <summary>
        /// The definition type for primitive defintions
        /// </summary>
        public override DefinitionType Type
        {
            get { return DefinitionType.Primitive; }
        }

        /// <summary>
        /// The primitive type being referenced
        /// </summary>
        public PrimitiveType Primitive { get; private set; }

        /// <summary>
        /// Constructs a new primitive reference instance
        /// </summary>
        /// <param name="primitive">The primitive being referenced</param>
        private PrimitiveDefinition(PrimitiveType primitive)
        {
            this.Primitive = primitive;
        }
    }
}
