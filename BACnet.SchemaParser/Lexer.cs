using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaParser
{
    public class Lexer
    {
        /// <summary>
        /// Dictionary of various punctuation string token types
        /// </summary>
        private static readonly Dictionary<string, TokenType> _symbolTypes = new Dictionary<string, TokenType>()
        {
            { "::=", TokenType.DefinedAs },
            { "{", TokenType.LeftBrace },
            { "}", TokenType.RightBrace },
            { "(", TokenType.LeftParentheses },
            { ")", TokenType.RightParentheses },
            { "[", TokenType.LeftBracket },
            { "]", TokenType.RightBracket },
            { ",", TokenType.Comma },
            { "..", TokenType.Ellipsis },
            { "...", TokenType.Ellipsis }
        };

        /// <summary>
        /// Dictionary of various identifier string token types
        /// </summary>
        private static readonly Dictionary<string, TokenType> _identifierTypes = new Dictionary<string, TokenType>()
        {
            { "NULL", TokenType.Null },
            { "BOOLEAN", TokenType.Boolean },
            { "Unsigned", TokenType.Unsigned },
            { "Unsigned8", TokenType.Unsigned8 },
            { "Unsigned16", TokenType.Unsigned16 },
            { "Unsigned32", TokenType.Unsigned },
            { "INTEGER", TokenType.Signed },
            { "REAL", TokenType.Float32 },
            { "DOUBLE", TokenType.Float64 },
            { "OCTET STRING", TokenType.OctetString },
            { "CharacterString", TokenType.CharString },
            { "BIT STRING", TokenType.BitString },
            { "ENUMERATED", TokenType.Enumerated },
            { "ABSTRACT-SYNTAX.&Type", TokenType.Generic },
            { "ABSTRACT-SYNTAX.&TYPE", TokenType.Generic },
            { "SEQUENCE", TokenType.Sequence },
            { "OF", TokenType.Of },
            { "CHOICE", TokenType.Choice },
            { "OPTIONAL", TokenType.Optional },
            { "SIZE", TokenType.Size },
            { "END", TokenType.End }
        };

        /// <summary>
        /// The reader containing the text to parse
        /// </summary>
        private TextReader _reader;

        /// <summary>
        /// The next line
        /// </summary>
        private string _line;

        /// <summary>
        /// The current column
        /// </summary>
        private int _column;

        /// <summary>
        /// Constructs a new lexer instance
        /// </summary>
        /// <param name="reader">The text reader to parse</param>
        public Lexer(TextReader reader)
        {
            this._reader = reader;
            _nextLine();
        }

        /// <summary>
        /// Moves the lexer to the next line
        /// </summary>
        private void _nextLine()
        {
            _column = 0;
            _line = _reader.ReadLine();
        }
        
        /// <summary>
        /// Determines whether the lexer is currently at the end of the input
        /// </summary>
        /// <returns>True if the end of input has been reached</returns>
        private bool _eof()
        {
            return _line == null;
        }

        /// <summary>
        /// Peeks at the next character
        /// </summary>
        /// <returns>The next character</returns>
        private char _peekChar()
        {
            return _column == _line.Length ? '\n' : _line[_column];
        }

        /// <summary>
        /// Consumes the next character
        /// </summary>
        /// <returns>The next character</returns>
        private char _readChar()
        {
            char ret = _peekChar();

            _column++;
            if(_column > _line.Length)
            {
                _nextLine();
            }

            return ret;
        }

        /// <summary>
        /// Reads an integer token
        /// </summary>
        /// <returns>The token</returns>
        private string _readInteger()
        {
            string s = string.Empty;
            
            while(!_eof() && Char.IsDigit(_peekChar()))
            {
                s += _readChar();
            }

            return s;
        }

        /// <summary>
        /// Skips a white space region
        /// </summary>
        private void _skipWhiteSpace()
        {
            while(!_eof() && Char.IsWhiteSpace(_peekChar()))
            {
                _readChar();
            }
        }

        /// <summary>
        /// Reads a symbol string
        /// </summary>
        /// <returns>The string</returns>
        private string _readSymbolString()
        {
            Func<char, bool> isSymbol = (c => Char.IsSymbol(c) || Char.IsPunctuation(c));
            string s = string.Empty;
            while(!_eof() && isSymbol(_peekChar()))
            {
                s += _readChar();
                if (_symbolTypes.ContainsKey(s) && (_eof() || !_symbolTypes.ContainsKey(s + _peekChar())))
                    break;
            }
            return s;
        }

        /// <summary>
        /// Reads an identifier string
        /// </summary>
        /// <returns>The string</returns>
        private string _readIdentifierString()
        {
            Func<Char, bool> isIdentifierChar = (c => Char.IsLetter(c) || Char.IsDigit(c) || c == '-' || c == '&' || c == '.');
            string s = string.Empty;

            while(!_eof() && isIdentifierChar(_peekChar()))
            {
                s += _readChar();
            }


            
            if(_line != null && _line.Length >= _column + " STRING".Length && _line.Substring(_column, " STRING".Length) == " STRING")
            {
                // BIT STRING or OCTET STRING or CHAR STRING
                s += " STRING";
                for (int i = 0; i < " STRING".Length; i++)
                    _readChar();
            }

            return s;
        }

        /// <summary>
        /// Resolves a token
        /// </summary>
        /// <param name="types">The token types</param>
        /// <param name="value">The token value</param>
        /// <returns>The token</returns>
        private Token _resolve(Dictionary<string, TokenType> types, string value, bool shouldThrow = false)
        {
            TokenType type;
            if(types.TryGetValue(value, out type))
            {
                return new Token(type, value);
            }
            else if(shouldThrow)
            {
                throw new InvalidDataException(value);
            }
            else
            {
                return null;
            }
        }

        
        /// <summary>
        /// Reads the next token from the lexer
        /// </summary>
        /// <returns></returns>
        public Token Next()
        {
        retry:
            if (_eof())
                return null;

            char c = _peekChar();

            if (Char.IsWhiteSpace(c))
            {
                _skipWhiteSpace();
                goto retry;
            }

            if(c == '–')
            {
                // long dash, skip
                _readChar();
                goto retry;
            }
            
            if(Char.IsSymbol(c) || Char.IsPunctuation(c))
            {
                string symbol = _readSymbolString();
                return _resolve(_symbolTypes, symbol, true);
            }

            if(Char.IsLetter(c))
            {
                string ident = _readIdentifierString();
                var token = _resolve(_identifierTypes, ident, false);
                if (token == null)
                    token = new Token(TokenType.Identifier, ident);
                return token;
            }

            if (Char.IsDigit(c))
            {
                string number = _readInteger();
                return new Token(TokenType.Integer, number);
            }

            throw new InvalidDataException(c.ToString());
        }
    }
}
