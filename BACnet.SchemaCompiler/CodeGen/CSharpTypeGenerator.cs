using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACnet.SchemaParser;

namespace BACnet.SchemaCompiler.CodeGen
{
    public class CSharpTypeGenerator
    {
        /// <summary>
        /// Reserved keywords
        /// </summary>
        private static readonly HashSet<string> _reserved = new HashSet<string>()
        {
            "double",
            "enum",
            "null",
            "object"
        };

        /// <summary>
        /// The directory to place generated files
        /// </summary>
        private string _directory;

        /// <summary>
        /// The namespace of the generated types
        /// </summary>
        private string _namespace;

        /// <summary>
        /// Constructs a new csharp type generator instance
        /// </summary>
        /// <param name="directory">The directory to place generated files</param>
        /// <param name="ns">The namespace of the generated types</param>
        public CSharpTypeGenerator(string directory, string ns)
        {
            this._directory = directory;
            this._namespace = ns;
        }

        /// <summary>
        /// Gets a CSharp type name from a definition name
        /// </summary>
        /// <param name="name">The definition name</param>
        /// <returns>The name to use for the CSharp type</returns>
        private string _transformTypeName(string name)
        {
            switch (name)
            {
                // there is a name clash between "BACnet-Error" and "Error"
                // since we are removing the "BACnet" prefix
                case "BACnet-Error": return "ServiceError";
                case "BACnetObjectIdentifier": return "ObjectId";
            }

            name = name
                .Replace("BACnet", string.Empty)
                .Replace("ACK", "Ack");

            var split = name.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < split.Length; i++)
            {
                split[i] = Char.ToUpper(split[i][0]) + split[i].Substring(1);
            }



            return String.Join(string.Empty, split);
        }

        /// <summary>
        /// Gets a CSharp field name from
        /// </summary>
        /// <param name="name">The raw, parsed field name</param>
        /// <returns>The csharp field name</returns>
        private string _transformFieldName(string name)
        {
            return _transformTypeName(name);
        }

        /// <summary>
        /// Transforms a field name to a type name for an anonymous
        /// type
        /// </summary>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>The type name</returns>
        private string _fieldNameToTypeName(string fieldName)
        {
            return _transformTypeName(fieldName);
        }

        /// <summary>
        /// Transforms a field name into a temporary variable name
        /// </summary>
        /// <param name="fieldName">The field name</param>
        /// <returns>The temporary name</returns>
        private string _fieldNameToTempName(string fieldName)
        {
            string name = _transformTypeName(fieldName);
            if (Char.IsUpper(name[0]))
                name = Char.ToLower(name[0]) + name.Substring(1);

            if (_reserved.Contains(name))
                name = "@" + name;

            return name;
        }

        /// <summary>
        /// Gets the type name for a defined type
        /// </summary>
        /// <param name="typeName">The name of the type</param>
        /// <param name="fieldName">The name of the current field</param>
        /// <param name="definition">The defined type</param>
        /// <returns>The type name</returns>
        private string _getDefinitionName(string typeName, string fieldName, TypeDefinition definition)
        {
            if (!string.IsNullOrEmpty(typeName))
                return typeName;

            if (definition.Type == DefinitionType.Primitive)
            {
                var prim = ((PrimitiveDefinition)definition).Primitive;
                switch (prim)
                {
                    case PrimitiveType.Null: return "Null";
                    case PrimitiveType.Boolean: return "bool";
                    case PrimitiveType.Unsigned8: return "byte";
                    case PrimitiveType.Unsigned16: return "ushort";
                    case PrimitiveType.Unsigned32: return "uint";
                    case PrimitiveType.Unsigned64: return "ulong";
                    case PrimitiveType.Signed8: return "sbyte";
                    case PrimitiveType.Signed16: return "short";
                    case PrimitiveType.Signed32: return "int";
                    case PrimitiveType.Signed64: return "long";
                    case PrimitiveType.Float32: return "float";
                    case PrimitiveType.Float64: return "double";
                    case PrimitiveType.OctetString: return "byte[]";
                    case PrimitiveType.CharString: return "string";
                    case PrimitiveType.BitString8: return "BitString8";
                    case PrimitiveType.BitString24: return "BitString24";
                    case PrimitiveType.BitString56: return "BitString56";
                    case PrimitiveType.Enumerated: return "Enumerated";
                    case PrimitiveType.Date: return "Date";
                    case PrimitiveType.Time: return "Time";
                    case PrimitiveType.ObjectId: return "ObjectId";
                    case PrimitiveType.Generic: return "GenericValue";
                    default:
                        throw new InvalidDataException(prim.ToString());
                }
            }
            else if (definition.Type == DefinitionType.Name)
            {
                var name = ((NameDefinition)definition).Name;
                return _transformTypeName(name);
            }
            else if(definition.Type == DefinitionType.Option)
            {
                var option = (OptionDefinition)definition;
                return "Option<" + _getDefinitionName(null, fieldName, option.ElementType) + ">";
            }
            else if(definition.Type == DefinitionType.Array)
            {
                var array = (ArrayDefinition)definition;
                return "ReadOnlyArray<" + _getDefinitionName(null, fieldName, array.ElementType) + ">";
            }
            else
            {
                return _fieldNameToTypeName(fieldName);
            }
        }

        /// <summary>
        /// Generates a definition
        /// </summary>
        /// <param name="name">The name of the type</param>
        /// <param name="def">The definition</param>
        private void _generateDefinition(CSharpEmitter emitter, string name, string fieldName, TypeDefinition def, bool root = false, string tag = null, string choiceBase = null)
        {
            switch(def.Type)
            {
                case DefinitionType.Enumeration:
                    _generateEnumeration(emitter, name, fieldName, (EnumerationDefinition)def);
                    break;
                case DefinitionType.BitString:
                    _generateBitString(emitter, name, fieldName, (BitStringDefinition)def);
                    break;
                case DefinitionType.Sequence:
                    _generateSequence(emitter, name, fieldName, (SequenceDefinition)def, root, tag, choiceBase);
                    break;
                case DefinitionType.Choice:
                    _generateChoice(emitter, name, fieldName, (ChoiceDefinition)def, root);
                    break;
                case DefinitionType.Array:
                    _generateArrayType(emitter, name, fieldName, (ArrayDefinition)def, root);
                    break;
                case DefinitionType.Option:
                    _generateOptionType(emitter, name, fieldName, (OptionDefinition)def, root);
                    break;
                case DefinitionType.Name:
                    _generateNameType(emitter, name, fieldName, (NameDefinition)def, root);
                    break;
                case DefinitionType.Primitive:
                    _generatePrimitiveType(emitter, name, fieldName, (PrimitiveDefinition)def, root);
                    break;
            }
        }

        /// <summary>
        /// Generates a wrapper type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="name">The name of the wrapper type</param>
        /// <param name="wrappedType">The name of the type to wrap</param>
        /// <returns>The emitter for the type body</returns>
        private CSharpEmitter _generateWrapperType(CSharpEmitter emitter, string name, string wrappedType, string choiceBase = null, string tag = null)
        {
            var typeEmitter = choiceBase == null ? emitter.Class(name) : emitter.Class(name, false, new string[] { choiceBase });;

            if(tag != null)
            {
                typeEmitter.OverrideProperty("Tag", "Tags", "return Tags." + tag + ";");
                typeEmitter.WriteLine();
            }

            typeEmitter.Property("Item", wrappedType, Access.Public);
            typeEmitter.WriteLine();

            using (var cons = typeEmitter.Constructor(name, new Parameter[] { new Parameter(wrappedType, "item") }))
            {
                cons.WriteLine("this.Item = item;");
            }

            typeEmitter.WriteLine();
            typeEmitter.StaticReadonlyField("Schema", "ISchema", "Value<" + wrappedType + ">.Schema", 
                @new:!string.IsNullOrEmpty(choiceBase));

            typeEmitter.WriteLine();
            using (var load = typeEmitter.StaticMethod("Load", name,
                new Parameter[] { new Parameter("IValueStream", "stream") },
                @new: !string.IsNullOrEmpty(choiceBase)))
            {
                load.WriteLine("var temp = Value<" + wrappedType + ">.Load(stream);");
                load.WriteLine("return new " + name + "(temp);");
            }

            typeEmitter.WriteLine();
            using (var save = typeEmitter.StaticMethod("Save", "void",
                new Parameter[] { new Parameter("IValueSink", "sink"), new Parameter(name, "value") }))
            {
                save.WriteLine("Value<" + wrappedType + ">.Save(sink, value.Item);");
            }
            typeEmitter.WriteLine();

            return typeEmitter;
        }

        /// <summary>
        /// Generates an array type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The array definition</param>
        /// <param name="root">True if this is the root type, false otherwise</param>
        private void _generateArrayType(CSharpEmitter emitter, string typeName, string fieldName, ArrayDefinition def, bool root)
        {
            if(root)
            {
                typeName = _getDefinitionName(typeName, fieldName, def);
                var elementTypeName = _getDefinitionName(null, "element", def);
                using (var wrapper = _generateWrapperType(emitter, typeName, elementTypeName))
                {
                    if(root)
                    {
                        _generateDefinition(wrapper, null, "element", def.ElementType);
                    }
                }
            }

            if (!root)
                _generateDefinition(emitter, null, fieldName, def.ElementType);
        }

        /// <summary>
        /// Generates an option type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The option definition</param>
        /// <param name="root">True if this is the root type, false otherwise</param>
        private void _generateOptionType(CSharpEmitter emitter, string typeName, string fieldName, OptionDefinition def, bool root)
        {
            if(root)
            {
                typeName = _getDefinitionName(typeName, fieldName, def);
                var elementTypeName = _getDefinitionName(null, "element", def);
                using (var wrapper = _generateWrapperType(emitter, typeName, elementTypeName))
                {
                    if(root)
                    {
                        _generateDefinition(wrapper, null, "element", def.ElementType);
                    }
                }
            }

            if (!root)
                _generateDefinition(emitter, null, fieldName, def.ElementType);
        }

        /// <summary>
        /// Generates an name alias type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The name definition</param>
        /// <param name="root">True if this is the root type, false otherwise</param>
        private void _generateNameType(CSharpEmitter emitter, string typeName, string fieldName, NameDefinition def, bool root)
        {
            if(root)
            {
                typeName = _getDefinitionName(typeName, fieldName, def);
                var elementTypeName = _getDefinitionName(null, null, def);
                using (var wrapper = _generateWrapperType(emitter, typeName, elementTypeName))
                {
                }
            }
        }

        /// <summary>
        /// Generates an array type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The primitive definition</param>
        /// <param name="root">True if this is the root type, false otherwise</param>
        private void _generatePrimitiveType(CSharpEmitter emitter, string typeName, string fieldName, PrimitiveDefinition def, bool root)
        {
            if (root)
            {
                typeName = _getDefinitionName(typeName, fieldName, def);
                var elementTypeName = _getDefinitionName(null, null, def);
                using (var wrapper = _generateWrapperType(emitter, typeName, elementTypeName))
                {
                }
            }
        }

        /// <summary>
        /// Generates an enumeration type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The enumeration definition</param>
        private void _generateEnumeration(CSharpEmitter emitter, string typeName, string fieldName, EnumerationDefinition def)
        {
            typeName = _getDefinitionName(typeName, fieldName, def);
            using(var e = emitter.Enum(typeName))
            {
                for(int i = 0; i < def.Options.Length; i++)
                {
                    var opt = def.Options[i];
                    var optName = _transformFieldName(opt.Name);
                    e.EnumValue(optName, opt.Value, i == def.Options.Length - 1);
                }
            }
        }

        /// <summary>
        /// Generates a bit string type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The bitstring definition</param>
        private void _generateBitString(CSharpEmitter emitter, string typeName, string fieldName, BitStringDefinition def)
        {
            typeName = _getDefinitionName(typeName, fieldName, def);
            using (var s = emitter.Struct(typeName))
            {
                using (var bits = emitter.Enum("Bits", "byte"))
                {
                    for (int i = 0; i < def.Bits.Length; i++)
                    {
                        var bit = def.Bits[i];
                        bits.EnumValue(_transformFieldName(bit.Name), bit.Index, i == def.Bits.Length - 1);
                    }
                }

                s.WriteLine();
                s.WriteLine("private BitString56 _bitstring;");

                s.WriteLine();
                s.WriteLineRaw("public byte Length { get { return _bitstring.Length; } }");

                s.WriteLine();
                s.WriteLine("public bool this[Bits bit] {{ get {{ return _bitstring[(int)bit]; }} }}");

                s.WriteLine();
                using (var cons = s.Constructor(typeName, new Parameter[] { new Parameter("BitString56", "bitstring") }))
                {
                    cons.WriteLine("this._bitstring = bitstring;");
                }

                s.WriteLine();
                s.WriteLine("public {0} WithLength(byte length) {{ return new {0}(_bitstring.WithLength(length)); }}", typeName);

                s.WriteLine();
                s.WriteLine("public {0} WithBit(Bits bit, bool set = true) {{ return new {0}(_bitstring.WithBit((int)bit, set)); }}", typeName);

                s.WriteLine();
                s.StaticReadonlyField("Schema", "ISchema", "PrimitiveSchema.BitString56Schema");

                s.WriteLine();
                using (var load = s.StaticMethod("Load", typeName, new Parameter[] { new Parameter("IValueStream", "stream") }))
                {
                    load.WriteLine("var temp = Value<BitString56>.Load(stream);");
                    load.WriteLine("return new {0}(temp);", typeName);
                }

                s.WriteLine();
                using (var save = s.StaticMethod("Save", "void", new Parameter[] { new Parameter("IValueSink", "sink"), new Parameter(typeName, "value") }))
                {
                    save.WriteLine("Value<BitString56>.Save(sink, value._bitstring);");
                }
            }
        }

        /// <summary>
        /// Generates types for sequence fields
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="def">The sequence definition</param>
        private void _generateSequenceFields(CSharpEmitter emitter, SequenceDefinition def)
        {
            foreach(var field in def.Fields)
            {
                var effectiveFieldName = _sequenceFieldEffectiveNameForType(field);
                _generateDefinition(emitter, null, effectiveFieldName, field.Type);
            }
        }

        /// <summary>
        /// Determines whether a sequence field type name needs -type appended
        /// to it so it won't conflict with a property name
        /// </summary>
        /// <param name="field">The sequence field</param>
        /// <returns>True if the suffix is needed, false otherwise</returns>
        private bool _sequenceFieldNeedsTypeSuffix(FieldDefinition field)
        {
            var type = field.Type;
            if (type.Type == DefinitionType.Option)
                type = ((OptionDefinition)type).ElementType;
            if (type.Type == DefinitionType.Array)
                type = ((ArrayDefinition)type).ElementType;

            switch (type.Type)
            {
                case DefinitionType.Enumeration:
                case DefinitionType.BitString:
                case DefinitionType.Sequence:
                case DefinitionType.Choice:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the effective field name for a sequence field
        /// that is used to determine the type name for the field
        /// </summary>
        /// <param name="field">The field</param>
        /// <returns>The effective field name</returns>
        private string _sequenceFieldEffectiveNameForType(FieldDefinition field)
        {
            string name = field.Name;
            if (_sequenceFieldNeedsTypeSuffix(field))
                name = name + "-type";
            return name;
        }

        /// <summary>
        /// Returns the type name for a sequence field
        /// </summary>
        /// <param name="field">The field</param>
        /// <returns>The type name</returns>
        private string _sequenceFieldTypeName(FieldDefinition field)
        {
            return _getDefinitionName(null, _sequenceFieldEffectiveNameForType(field), field.Type);
        }
        
        /// <summary>
        /// Generates a sequence type
        /// </summary>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The sequence definition</param>
        private void _generateSequence(CSharpEmitter emitter, string typeName, string fieldName, SequenceDefinition def, bool root = false, string tag = null, string choiceBase = null)
        {
            string[] bases = choiceBase == null ? new string[0] { } : new string[] { choiceBase };
            typeName = _getDefinitionName(typeName, fieldName, def);
            using(var cls = emitter.Class(typeName, false, bases))
            {
                if(tag != null)
                {
                    cls.OverrideProperty("Tag", "Tags", "return Tags." + tag + ";");
                    cls.WriteLine();
                }


                foreach (var field in def.Fields)
                {
                    var fieldName2 = _transformFieldName(field.Name);
                    var typeName2 = _sequenceFieldTypeName(field);
                    cls.Property(fieldName2, typeName2);
                    cls.WriteLine();
                }

                var cparams = def.Fields.Select(f => new Parameter(
                    _sequenceFieldTypeName(f),
                    _fieldNameToTempName(f.Name)))
                    .ToArray();

                using (var cons = cls.Constructor(typeName, cparams, Access.Public))
                {
                    foreach (var field in def.Fields)
                    {
                        cons.WriteLine("this.{0} = {1};",
                            _transformFieldName(field.Name),
                            _fieldNameToTempName(field.Name));
                    }
                }

                cls.WriteLine();
                var schemaStr = "new SequenceSchema(false, " + Environment.NewLine
                    + string.Join("," + Environment.NewLine,
                        def.Fields.Select(f => cls.IndentString(1) + "new FieldSchema(\"" + _transformFieldName(f.Name) + "\", " + f.Tag
                        + ", Value<" + _sequenceFieldTypeName(f) + ">.Schema)").ToArray())
                    + ")";
                cls.StaticReadonlyField("Schema", "ISchema", schemaStr, @new:!string.IsNullOrEmpty(choiceBase));

                cls.WriteLine();
                using (var load = cls.StaticMethod("Load", typeName, new Parameter[] { new Parameter("IValueStream", "stream") },
                    @new:!string.IsNullOrEmpty(choiceBase)))
                {
                    load.WriteLine("stream.EnterSequence();");
                    foreach(var field in def.Fields)
                    {
                        load.WriteLine("var {0} = Value<{1}>.Load(stream);",
                            _fieldNameToTempName(field.Name),
                            _sequenceFieldTypeName(field));
                    }
                    load.WriteLine("stream.LeaveSequence();");

                    load.WriteLine("return new " + typeName + "("
                        + string.Join(", ", def.Fields.Select(f => _fieldNameToTempName(f.Name)).ToArray())
                        + ");");
                }

                cls.WriteLine();
                using (var save = cls.StaticMethod("Save", "void", new Parameter[] { new Parameter("IValueSink", "sink"), new Parameter(typeName, "value") }))
                {
                    save.WriteLine("sink.EnterSequence();");
                    foreach (var field in def.Fields)
                    {
                        save.WriteLine("Value<{0}>.Save(sink, value.{1});",
                            _sequenceFieldTypeName(field),
                            _transformFieldName(field.Name));
                    }
                    save.WriteLine("sink.LeaveSequence();");
                }

                if (root)
                    _generateSequenceFields(cls, def);
            }

            if (!root)
                _generateSequenceFields(emitter, def);
        }

        /// <summary>
        /// Determines whether a choice option requires a wrapper type
        /// </summary>
        /// <param name="field">The choice field</param>
        /// <returns>True if a wrapper is required, false otherwise</returns>
        private bool _choiceOptionNeedsWrapper(FieldDefinition field)
        {
            return field.Type.Type != DefinitionType.Sequence;
        }

        /// <summary>
        /// Gets the parameters that need to be forwarded to a choice option's
        /// constructor
        /// </summary>
        /// <param name="field">The choice field</param>
        /// <returns>The parameter array</returns>
        private Parameter[] _getChoiceOptionForwardedParameters(FieldDefinition field)
        {
            if(_choiceOptionNeedsWrapper(field))
            {
                var wrappedTypeName = _getDefinitionName(null, field.Name, field.Type);
                var tempName = _fieldNameToTempName(field.Name);

                return new Parameter[] {
                    new Parameter(wrappedTypeName, tempName)
                };
            }
            else
            {
                var seq = (SequenceDefinition)field.Type;
                List<Parameter> ps = new List<Parameter>();

                foreach(var f in seq.Fields)
                {
                    var typeName = _getDefinitionName(null, f.Name, f.Type);
                    var tempName = _fieldNameToTempName(f.Name);
                    ps.Add(new Parameter(typeName, tempName));
                }

                return ps.ToArray();
            }

        }
        
        /// <summary>
        /// If necessary, generates a wrapper for each non-sequence, non-choice option
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="choiceName">The name of the choice type</param>
        /// <param name="def">The choice definition</param>
        private void _generateChoiceOptionWrappers(CSharpEmitter emitter, string choiceName, ChoiceDefinition def)
        {
            foreach(var field in def.Fields)
            {
                bool needsWrapper = def.Fields.Any(f => _choiceOptionNeedsWrapper(f));
                var fieldName = _transformFieldName(field.Name);
                var typeName = _getDefinitionName(null, fieldName, field.Type);

                if (needsWrapper)
                {
                    emitter.WriteLine();
                    using (var wrapper = _generateWrapperType(emitter, fieldName + "Wrapper", typeName, choiceName, fieldName))
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Generates a choice option
        /// </summary>
        /// <param name="field">The field to generate the option for</param>
        private void _generateChoiceOption(CSharpEmitter emitter, string choiceName, FieldDefinition field)
        {
            var tag = _transformFieldName(field.Name);
            _generateDefinition(emitter, null, field.Name, field.Type, false, tag, choiceName);
        }

        
        /// <summary>
        /// Generates the option types for a choice
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="def">The choice definition</param>
        private void _generateChoiceOptions(CSharpEmitter emitter, string name, ChoiceDefinition def)
        {
            _generateChoiceOptionWrappers(emitter, name, def);

            foreach (var field in def.Fields)
            {
                _generateChoiceOption(emitter, name, field);
            }
        }

        /// <summary>
        /// Generates the tags enumeration for a choice
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="def">The choice definition</param>
        private void _generateChoiceTags(CSharpEmitter emitter, ChoiceDefinition def)
        {
            emitter.WriteLine();
            using (var tagEmitter = emitter.Enum("Tags", "byte"))
            {
                for (int i = 0; i < def.Fields.Length; i++)
                {
                    var field = def.Fields[i];
                    var fieldName2 = _transformFieldName(field.Name);
                    tagEmitter.EnumValue(fieldName2, i, i == def.Fields.Length - 1);
                }
            }
        }

        /// <summary>
        /// Generates a choice type
        /// </summary>
        /// <param name="emitter">The emitter to write to</param>
        /// <param name="typeName">The name of the type</param>
        /// <param name="def">The choice definition</param>
        private void _generateChoice(CSharpEmitter emitter, string typeName, string fieldName, ChoiceDefinition def, bool root = false)
        {
            typeName = _getDefinitionName(typeName, fieldName, def);

            if (!root)
            {
                _generateChoiceTags(emitter, def);
                emitter.WriteLine();
            }

            using(var cls = emitter.Class(typeName, true))
            {
                cls.AbstractProperty("Tag", "Tags", Access.Public);

                foreach(var field in def.Fields)
                {
                    var tag = _transformFieldName(field.Name);

                    var optionTypeName = _choiceOptionNeedsWrapper(field)
                        ? _transformFieldName(field.Name) + "Wrapper"
                        : _getDefinitionName(null, field.Name, field.Type);

                    var valueTypeName = _getDefinitionName(null, field.Name, field.Type);
                    
                    cls.WriteLine();
                    cls.Property("Is" + tag, "bool", "return this.Tag == Tags." + tag + ";");

                    cls.WriteLine();
                    cls.Property("As" + tag, valueTypeName,
                        _choiceOptionNeedsWrapper(field)
                            ? "return ((" + optionTypeName + ")this).Item;"
                            : "return (" + optionTypeName + ")this;");

                    cls.WriteLine();
                    Parameter[] parameters = _getChoiceOptionForwardedParameters(field);
                    using(var method = cls.StaticMethod("New" + tag, typeName, parameters))
                    {
                        var paramsArr = parameters.Select(p => p.Name).ToArray();
                        var paramsStr = string.Join(", ", paramsArr);
                        method.WriteLine("return new " + optionTypeName + "(" + paramsStr + ");");
                    }

                }


                cls.WriteLine();
                var schemaStr = "new ChoiceSchema(false, " + Environment.NewLine
                    + string.Join("," + Environment.NewLine,
                        def.Fields.Select(f => cls.IndentString(1) + "new FieldSchema(\"" + _transformFieldName(f.Name) + "\", " + f.Tag
                        + ", Value<" + _getDefinitionName(null, f.Name, f.Type) + ">.Schema)").ToArray())
                    + ")";
                cls.StaticReadonlyField("Schema", "ISchema", schemaStr);

                cls.WriteLine();
                using (var load = cls.StaticMethod("Load", typeName, new Parameter[] { new Parameter("IValueStream", "stream") }))
                {
                    load.WriteLine("{0} ret = null;", typeName);
                    load.WriteLine("Tags tag = (Tags)stream.EnterChoice();");
                    using (var sw = load.Switch("tag"))
                    {
                        foreach(var field in def.Fields)
                        {
                            var tag = _transformFieldName(field.Name);
                            var optionTypeName = _choiceOptionNeedsWrapper(field)
                                ? _transformFieldName(field.Name) + "Wrapper"
                                : _getDefinitionName(null, field.Name, field.Type);

                            sw.Case("Tags." + tag);
                            sw.WriteLine("ret = Value<{0}>.Load(stream);", optionTypeName);
                            sw.Break();
                        }

                        sw.Default();
                        sw.WriteLine("throw new Exception();");
                        sw.Indent--;
                    }
                    load.WriteLine("stream.LeaveChoice();");
                    load.WriteLine("return ret;");
                }

                cls.WriteLine();
                using (var save = cls.StaticMethod("Save", "void", new Parameter[] { new Parameter("IValueSink", "sink"), new Parameter(typeName, "value") }))
                {
                    save.WriteLine("sink.EnterChoice((byte)value.Tag);");
                    using (var sw = save.Switch("value.Tag"))
                    {
                        foreach(var field in def.Fields)
                        {
                            var tag = _transformFieldName(field.Name);
                            var optionTypeName = _choiceOptionNeedsWrapper(field)
                                ? _transformFieldName(field.Name) + "Wrapper"
                                : _getDefinitionName(null, field.Name, field.Type);

                            sw.Case("Tags." + tag);
                            sw.WriteLine("Value<{0}>.Save(sink, ({0})value);", optionTypeName);
                            sw.Break();
                        }

                        sw.Default();
                        sw.WriteLine("throw new Exception();");
                        sw.Indent--;
                    }
                    save.WriteLine("sink.LeaveChoice();");
                }


                if (root)
                {
                    _generateChoiceTags(cls, def);
                    _generateChoiceOptions(cls, typeName, def);
                }
            }

            if (!root)
            {
                _generateChoiceOptions(emitter, typeName, def);
            }
        }
        
        /// <summary>
        /// Generates code for a named type
        /// </summary>
        /// <param name="type">The type to generate for</param>
        public void Generate(NamedType type)
        {
            string name = _transformTypeName(type.Name);
            string path = Path.Combine(_directory, name + ".cs");
            using(var emitter = new CSharpEmitter(path))
            {
                emitter.EmitUsing("System");
                emitter.EmitUsing("BACnet.Types");
                emitter.EmitUsing("BACnet.Types.Schemas");
                emitter.WriteLine();
                
                using(var ns = emitter.Namespace(_namespace))
                {
                    _generateDefinition(ns, name, null, type.Definition, true, null);
                }
            }
        }

        public struct Parameter
        {
            public string Type { get; private set; }
            public string Name { get; private set; }

            public Parameter(string type, string name) : this()
            {
                this.Type = type;
                this.Name = name;
            }
        }

        public class CSharpEmitter : IDisposable
        {
            /// <summary>
            /// The stream writer to write to
            /// </summary>
            private StreamWriter _writer;

            private int _indent;
            private bool _isScope;

            public int Indent { get { return _indent; } set { _indent = value; } }

            /// <summary>
            /// Constructs a new csharp emitter
            /// </summary>
            /// <param name="path"></param>
            public CSharpEmitter(string path)
            {
                _writer = new StreamWriter(path);
                _indent = 0;
                _isScope = false;
            }

            /// <summary>
            /// Constructs a scoped emitter
            /// </summary>
            /// <param name="writer">The stream writer to write to</param>
            /// <param name="indent">The tab indent to start at</param>
            private CSharpEmitter(StreamWriter writer, int indent = 0)
            {
                _writer = writer;
                _indent = indent;
                _isScope = true;
            }

            /// <summary>
            /// Creates a new child scope emitter
            /// </summary>
            /// <returns>The child scope emitter</returns>
            private CSharpEmitter _newScope()
            {
                return new CSharpEmitter(_writer, _indent + 1);
            }

            /// <summary>
            /// Disposes of the emitter
            /// </summary>
            public void Dispose()
            {
                if(!_isScope)
                    _writer.Dispose();
                else
                {
                    _indent--;
                    WriteLineRaw("}");
                }
            }

            /// <summary>
            /// Retrieves the current indentation string
            /// </summary>
            /// <param name="modifier">The modifier to apply to the indentation level</param>
            /// <returns>The indentation string</returns>
            public string IndentString(int modifier)
            {
                int indent = _indent + modifier;
                string ret = string.Empty;
                for (int i = 0; i < indent; i++)
                    ret += '\t';
                return ret;
            }

            /// <summary>
            /// Writes a single line to the stream
            /// </summary>
            /// <param name="fmt">The format string</param>
            /// <param name="args">The format args</param>
            public void WriteLine(string fmt, params object[] args)
            {
                for (int i = 0; i < _indent; i++)
                    _writer.Write('\t');
                _writer.WriteLine(fmt, args);
            }

            /// <summary>
            /// Writes a single line of raw text to the stream
            /// </summary>
            /// <param name="text">The text to write</param>
            public void WriteLineRaw(string text)
            {
                for (int i = 0; i < _indent; i++)
                    _writer.Write('\t');
                _writer.WriteLine(text);
            }

            /// <summary>
            /// Writes a blank line
            /// </summary>
            public void WriteLine()
            {
                _writer.WriteLine();
            }

            /// <summary>
            /// Emits a using clause
            /// </summary>
            /// <param name="ns">The namespace</param>
            public void EmitUsing(string ns)
            {
                WriteLine("using {0};", ns);
            }

            /// <summary>
            /// Enters a namespace
            /// </summary>
            /// <param name="ns">The namespace</param>
            /// <returns>The scoped generator</returns>
            public CSharpEmitter Namespace(string ns)
            {
                WriteLine("namespace {0}", ns);
                WriteLineRaw("{");
                return _newScope();
            }

            /// <summary>
            /// Enters a class
            /// </summary>
            /// <param name="name">The name of the class</param>
            /// <param name="bases">The base types</param>
            /// <returns>The scoped generator</returns>
            public CSharpEmitter Class(string name, bool isAbstract = false, string[] bases = null, Access access = Access.Public)
            {
                string @abstract = isAbstract ? "abstract " : string.Empty;
                string tail = (bases == null || bases.Length == 0) ? string.Empty : " : " + string.Join(", ", bases);
                WriteLine("{0} {1} partial class {2}{3}",
                    access.ToAccessString(),
                    @abstract,
                    name,
                    tail);
                WriteLineRaw("{");
                return _newScope();
            }

            /// <summary>
            /// Enters a struct
            /// </summary>
            /// <param name="name">The name of the struct</param>
            /// <param name="access">The access specifier for the struct</param>
            /// <returns>The struct generator</returns>
            public CSharpEmitter Struct(string name, Access access = Access.Public)
            {
                WriteLine("{0} struct {1}",
                    access.ToAccessString(),
                    name);
                WriteLineRaw("{");
                return _newScope();
            }

            /// <summary>
            /// Enters an enum
            /// </summary>
            /// <param name="name">The name of the enum</param>
            /// <param name="baseType">The base type</param>
            /// <returns>The scoped generator</returns>
            public CSharpEmitter Enum(string name, string baseType = "uint")
            {
                WriteLine("public enum {0} : {1}", name, baseType);
                WriteLineRaw("{");
                return _newScope();
            }

            /// <summary>
            /// Generates an enumeration value
            /// </summary>
            /// <param name="name">The name of the value</param>
            /// <param name="value">The value</param>
            public void EnumValue(string name, int value, bool last = false)
            {
                string tail = last ? string.Empty : ",";
                WriteLine("{0} = {1}{2}", name, value, tail);
            }

            /// <summary>
            /// Enters a switch
            /// </summary>
            /// <param name="expression">The expression to switch on</param>
            /// <returns>The emitter for the switch</returns>
            public CSharpEmitter Switch(string expression)
            {
                WriteLine("switch({0})", expression);
                WriteLineRaw("{");
                return _newScope();
            }

            /// <summary>
            /// Generates a case
            /// </summary>
            /// <param name="expression">The case expression</param>
            public void Case(string expression)
            {
                WriteLine("case {0}:", expression);
                _indent++;
            }

            /// <summary>
            /// Generates a break
            /// </summary>
            public void Break()
            {
                WriteLine("break;");
                _indent--;
            }

            /// <summary>
            /// Enters a default case
            /// </summary>
            public void Default()
            {
                WriteLine("default:");
                _indent++;
            }

            /// <summary>
            /// Creates a new property
            /// </summary>
            /// <param name="name">The name of the property</param>
            /// <param name="type">The type of the property</param>
            /// <param name="access">The access specifier for the property</param>
            /// <param name="setter">The setter specifier for the property</param>
            public void Property(string name, string type, Access access = Access.Public, Access setter = Access.Private)
            {
                WriteLine("{0} {1} {2} {{ get; {3} set; }}",
                    access.ToAccessString(),
                    type,
                    name,
                    setter.ToAccessString());
            }

            /// <summary>
            /// Creates a new get-only property
            /// </summary>
            /// <param name="name">The name of the property</param>
            /// <param name="type">The type of the property</param>
            /// <param name="body">The body of the property</param>
            /// <param name="access">The access specifier for the property</param>
            public void Property(string name, string type, string body, Access access = Access.Public)
            {
                WriteLine("{0} {1} {2} {{ get {{ {3} }} }}",
                    access.ToAccessString(),
                    type,
                    name,
                    body);
            }

            /// <summary>
            /// Creates a new abstract property
            /// </summary>
            /// <param name="name">The name of the property</param>
            /// <param name="type">The type of the property</param>
            /// <param name="access">The access specifier for the property</param>
            public void AbstractProperty(string name, string type, Access access = Access.Public)
            {

                WriteLine("{0} abstract {1} {2} {{ get; }}",
                    access.ToAccessString(),
                    type,
                    name);
            }

            /// <summary>
            /// Overrides a get-only property
            /// </summary>
            /// <param name="name">The name of the property</param>
            /// <param name="type">The type of the property</param>
            /// <param name="body">The body of the property accessor</param>
            /// <param name="access">The access level of the property</param>
            public void OverrideProperty(string name, string type, string body, Access access = Access.Public)
            {
                WriteLine("{0} override {1} {2} {{ get {{ {3} }} }}",
                    access.ToAccessString(),
                    type,
                    name,
                    body);
            }

            /// <summary>
            /// Creates a static readonly field
            /// </summary>
            /// <param name="name">The name of the field</param>
            /// <param name="type">The type of the field</param>
            /// <param name="intializer">The intitializer for the field</param>
            /// <param name="access">The access specifier for the field</param>
            public void StaticReadonlyField(string name, string type, string initializer, Access access = Access.Public, bool @new = false)
            {
                WriteLine("{0} static readonly {1}{2} {3} = {4};",
                    access.ToAccessString(),
                    @new ? "new " : string.Empty,
                    type,
                    name,
                    initializer);
            }

            /// <summary>
            /// Enters a new static method
            /// </summary>
            /// <param name="name">The name of the method</param>
            /// <param name="returnType">The return type of the method</param>
            /// <param name="parameters">The parameters of the method</param>
            /// <param name="access">The access specifier of the method</param>
            /// <returns>The method emitter</returns>
            public CSharpEmitter StaticMethod(string name, string returnType, Parameter[] parameters, Access access = Access.Public, bool @new = false)
            {
                var paramsArr = parameters.Select(p => p.Type + " " + p.Name).ToArray();
                var paramsStr = String.Join(", ", paramsArr);

                WriteLine("{0} static {1}{2} {3}({4})",
                    access.ToAccessString(),
                    @new ? "new " : string.Empty,
                    returnType,
                    name,
                    paramsStr);
                WriteLineRaw("{");

                return _newScope();
            }

            /// <summary>
            /// Enters a new constructor
            /// </summary>
            /// <param name="name">The type name</param>
            /// <param name="parameters">The constructor parameters</param>
            /// <returns></returns>
            public CSharpEmitter Constructor(string name, Parameter[] parameters, Access access = Access.Public, bool deferDefault = false)
            {
                var paramsArr = parameters.Select(p => p.Type + " " + p.Name).ToArray();
                var paramsStr = String.Join(", ", paramsArr);
                var deferStr = deferDefault ? " : this()" : string.Empty;

                WriteLine("{0} {1}({2}){3}",
                    access.ToAccessString(),
                    name,
                    paramsStr,
                    deferStr);
                WriteLineRaw("{");

                return _newScope();
            }

        }
    }
}
