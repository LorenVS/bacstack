using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACnet.SchemaCompiler.CodeGen
{
    public static class Extensions
    {
        public static string ToAccessString(this Access access)
        {
            switch (access)
            {
                case Access.Public:
                    return "public";
                case Access.Private:
                    return "private";
                case Access.Internal:
                    return "internal";
            }
            throw new Exception();
        }
    }
}
