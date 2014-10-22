using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public enum TokenType
    {
        DefinedAs,
        LeftBrace,
        RightBrace,
        LeftParentheses,
        RightParentheses,
        LeftBracket,
        RightBracket,
        Comma,
        Ellipsis,

        Null,
        Boolean,
        Unsigned,
        Unsigned8,
        Unsigned16,
        Signed,
        Float32,
        Float64,
        OctetString,
        CharString,
        Date,
        Time,
        ObjectId,
        Generic,

        Enumerated,
        BitString,
        Sequence,
        Of,
        Choice,
        Optional,
        Size,
        End,

        Identifier,
        Integer

    }
}
