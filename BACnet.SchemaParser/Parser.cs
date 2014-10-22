using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class Parser
    {
        private Lexer _lexer;

        private Token _token;

        private bool _hasToken;

        /// <summary>
        /// Constructs a new parser instance
        /// </summary>
        /// <param name="lexer">The lexer instance</param>
        public Parser(Lexer lexer)
        {
            this._lexer = lexer;
            this._token = null;
            this._hasToken = false;
        }

        /// <summary>
        /// Retrieves the next token without consuming it
        /// </summary>
        /// <returns>The next token</returns>
        private Token _peek()
        {
            if (!_hasToken)
            {
                _token = _lexer.Next();
                _hasToken = true;
            }
            return _token;
        }

        /// <summary>
        /// Retrieves the type of the next token without consuming it
        /// </summary>
        /// <returns>The type of the next token</returns>
        private TokenType _peekType()
        {
            return _peek().Type;
        }

        /// <summary>
        /// Retrieves the next token and consumes it
        /// </summary>
        /// <returns>The next token</returns>
        private Token _read()
        {
            var ret = _peek();
            _hasToken = false;
            return ret;
        }

        /// <summary>
        /// Gets the value of the next token of a certain type
        /// </summary>
        /// <param name="type">The type of token to retrieve</param>
        /// <returns>The token's value</returns>
        private string _get(TokenType type)
        {
            var token = _read();
            if (token.Type != type)
                throw new InvalidDataException(token.Type.ToString());
            return token.Value;
        }

        /// <summary>
        /// Determines whether the next token has a certain type
        /// </summary>
        /// <param name="type">The type of token to check for</param>
        /// <returns>True if the next token has the type, false otherwise</returns>
        private bool _at(TokenType type)
        {
            var token = _peek();
            return token.Type == type;
        }

        /// <summary>
        /// Skips a range specifier (1..16) if present
        /// </summary>
        private void _skipRangeSpecifier()
        {
            if (_peekType() == TokenType.LeftParentheses)
            {
                _read();
                if (_peekType() == TokenType.Integer)
                    _read();
                if (_peekType() == TokenType.Ellipsis)
                    _read();
                if (_peekType() == TokenType.Integer)
                    _read();
                _get(TokenType.RightParentheses);
            }   
        }

        /// <summary>
        /// Skips a length specifier (SIZE(1..16)) if present
        /// </summary>
        private void _skipLengthSpecifier()
        {
            if (_peekType() == TokenType.LeftParentheses)
            {
                _read();
                if (_peekType() == TokenType.Size)
                {
                    _read();
                    _skipRangeSpecifier();
                }
                _get(TokenType.RightParentheses);
            }
        }

        /// <summary>
        /// Parses a type definition
        /// </summary>
        /// <returns>The type definition</returns>
        private TypeDefinition _parseDefinition(bool eagerOptional = true)
        {
            TokenType type;
            TypeDefinition definition;

            type = _peekType();

            switch (type)
            {
                case TokenType.Null:
                    _read();
                    definition = PrimitiveDefinition.Null;
                    break;
                case TokenType.Boolean:
                    _read();
                    definition = PrimitiveDefinition.Boolean;
                    break;
                case TokenType.Unsigned:
                    _read();
                    definition = PrimitiveDefinition.Unsigned32;
                    _skipRangeSpecifier();
                    break;
                case TokenType.Unsigned8:
                    _read();
                    definition = PrimitiveDefinition.Unsigned8;
                    _skipRangeSpecifier();
                    break;
                case TokenType.Unsigned16:
                    _read();
                    definition = PrimitiveDefinition.Unsigned16;
                    _skipRangeSpecifier();
                    break;
                case TokenType.Signed:
                    _read();
                    definition = PrimitiveDefinition.Signed32;
                    break;
                case TokenType.Float32:
                    _read();
                    definition = PrimitiveDefinition.Float32;
                    break;
                case TokenType.Float64:
                    _read();
                    definition = PrimitiveDefinition.Float64;
                    break;
                case TokenType.OctetString:
                    _read();
                    definition = PrimitiveDefinition.OctetString;
                    _skipLengthSpecifier();
                    break;
                case TokenType.CharString:
                    _read();
                    definition = PrimitiveDefinition.CharString;
                    _skipLengthSpecifier();
                    break;
                    // bitstring handled below
                    // enumerated handled below
                case TokenType.Date:
                    _read();
                    definition = PrimitiveDefinition.Date;
                    break;
                case TokenType.Time:
                    _read();
                    definition = PrimitiveDefinition.Time;
                    break;
                case TokenType.ObjectId:
                    _read();
                    definition = PrimitiveDefinition.ObjectId;
                    break;
                case TokenType.Generic:
                    _read();
                    definition = PrimitiveDefinition.Generic;
                    break;
                case TokenType.Enumerated:
                    definition = _parseEnumeration();
                    break;
                case TokenType.BitString:
                    definition = _parseBitString();
                    break;
                case TokenType.Sequence:
                    definition = _parseSequence();
                    break;
                case TokenType.Choice:
                    definition = _parseChoice();
                    break;
                case TokenType.Identifier:
                    definition = new NameDefinition(_get(TokenType.Identifier));
                    break;
                default:
                    throw new InvalidDataException(type.ToString());
            }

            if(eagerOptional && _peekType() == TokenType.Optional)
            {
                definition = new OptionDefinition(definition);
                _read();
            }

            return definition;
        }

        /// <summary>
        /// Parses an enumeration option definition
        /// </summary>
        /// <returns>The option definition</returns>
        private EnumerationOptionDefinition _parseEnumerationOption()
        {
            string name;
            int value;

            name = _get(TokenType.Identifier);
            _get(TokenType.LeftParentheses);
            value = Convert.ToInt32(_get(TokenType.Integer));
            _get(TokenType.RightParentheses);

            if (_peekType() == TokenType.Comma)
                _read();
            if (_peekType() == TokenType.Ellipsis)
                _read();

            return new EnumerationOptionDefinition(name, value);
        }

        /// <summary>
        /// Parses an enumeration definition
        /// </summary>
        /// <returns>The type definition</returns>
        private TypeDefinition _parseEnumeration()
        {
            _get(TokenType.Enumerated);
            if(_peekType() != TokenType.LeftBrace)
            {
                // this is just a primitive enumerated value, not
                // a full enumeration
                return PrimitiveDefinition.Enumerated;
            }

            List<EnumerationOptionDefinition> options = new List<EnumerationOptionDefinition>();

            _get(TokenType.LeftBrace);
            while(_peekType() != TokenType.RightBrace)
            {
                options.Add(_parseEnumerationOption());
            }
            _get(TokenType.RightBrace);

            return new EnumerationDefinition(options.ToArray());
        }

        /// <summary>
        /// Parses a bit definition
        /// </summary>
        /// <returns>The bit definition</returns>
        private BitDefinition _parseBitDefinition()
        {
            string name;
            int index;

            name = _get(TokenType.Identifier);
            _get(TokenType.LeftParentheses);
            index = Convert.ToInt32(_get(TokenType.Integer));
            _get(TokenType.RightParentheses);

            if (_peekType() == TokenType.Comma)
                _read();
            if (_peekType() == TokenType.Ellipsis)
                _read();

            return new BitDefinition(name, index);
        }

        /// <summary>
        /// Parses a bitstring definition
        /// </summary>
        /// <returns>The type definition</returns>
        private TypeDefinition _parseBitString()
        {
            _get(TokenType.BitString);
            if(_peekType() != TokenType.LeftBrace)
            {
                // just a primitive bitstring
                return PrimitiveDefinition.BitString56;
            }

            List<BitDefinition> bits = new List<BitDefinition>();

            _get(TokenType.LeftBrace);
            while(_peekType() != TokenType.RightBrace)
            {
                bits.Add(_parseBitDefinition());
            }
            _get(TokenType.RightBrace);

            return new BitStringDefinition(bits.ToArray());
        }

        /// <summary>
        /// Parses a field definition
        /// </summary>
        /// <returns>The field definition</returns>
        private FieldDefinition _parseField()
        {
            string name;
            byte tag = 255;
            TypeDefinition definition;

            name = _get(TokenType.Identifier);
            if(_peekType() == TokenType.LeftBracket)
            {
                _get(TokenType.LeftBracket);
                tag = Convert.ToByte(_get(TokenType.Integer));
                _get(TokenType.RightBracket);
            }
            definition = _parseDefinition();

            if (_peekType() == TokenType.Comma)
                _read();
            if (_peekType() == TokenType.Ellipsis)
                _read();

            return new FieldDefinition(name, tag, definition);
        }

        /// <summary>
        /// Parses a sequence definition
        /// </summary>
        /// <returns>The type definition</returns>
        private TypeDefinition _parseSequence()
        {
            _get(TokenType.Sequence);

            if(_peekType() == TokenType.Size)
            {
                // SIZE (3) or just SIZE 3
                _read();
                if (_peekType() == TokenType.LeftParentheses)
                    _read();
                if (_peekType() == TokenType.Integer)
                    _read();
                if (_peekType() == TokenType.RightParentheses)
                    _read();
            }

            if(_peekType() == TokenType.Of)
            {
                // actually an array, not a sequence
                _get(TokenType.Of);
                return new ArrayDefinition(_parseDefinition(false));
            }

            List<FieldDefinition> fields = new List<FieldDefinition>();

            _get(TokenType.LeftBrace);
            while(_peekType() != TokenType.RightBrace)
            {
                fields.Add(_parseField());
            }
            _get(TokenType.RightBrace);

            return new SequenceDefinition(fields.ToArray());
        }

        /// <summary>
        /// Parses a choice definition
        /// </summary>
        /// <returns>The type definition</returns>
        private TypeDefinition _parseChoice()
        {
            List<FieldDefinition> fields = new List<FieldDefinition>();

            _get(TokenType.Choice);
            _get(TokenType.LeftBrace);
            while (_peekType() != TokenType.RightBrace)
            {
                fields.Add(_parseField());
            }
            _get(TokenType.RightBrace);

            return new ChoiceDefinition(fields.ToArray());
        }

        /// <summary>
        /// Retrieves the next named type
        /// </summary>
        /// <returns>The named type instance</returns>
        public NamedType Next()
        {
            string name;
            TypeDefinition definition;

            if (_peekType() == TokenType.End)
                return null;

            name = _get(TokenType.Identifier);
            _get(TokenType.DefinedAs);
            definition = _parseDefinition();

            return new NamedType(name, definition);
        }


    }
}
