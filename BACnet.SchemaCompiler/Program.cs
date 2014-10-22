using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.SchemaParser;
using BACnet.SchemaCompiler.CodeGen;

namespace BACnet.SchemaCompiler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"C:\git\bacnet\bacnet.txt"))
            {
                Lexer lexer = new Lexer(reader);
                Parser parser = new Parser(lexer);
                CSharpTypeGenerator gen = new CSharpTypeGenerator(@"C:\git\bacnet\BACnet.Ashrae\generated", "BACnet.Ashrae");

                NamedType type = parser.Next();
                while(type != null)
                {
                    gen.Generate(type);
                    type = parser.Next();
                }
            }
        }
    }
}
