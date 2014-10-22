using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BACnet.Tagging;
using BACnet.Types;
using BACnet.Types.Schemas;

namespace BACnet.Tagging.Tests
{
    [TestClass]
    public class TagReaderStreamTests
    {
        public enum TestChoice
        {
            Float,
            Double,
            String
        }

        private interface ITestChoice
        {
            TestChoice Choice { get; }
        }

        public class FloatChoice : ITestChoice
        {
            public TestChoice Choice { get { return TestChoice.Float; } }

            public float Value { get; private set; }

            public FloatChoice(float value)
            {
                this.Value = value;
            }

            public static FloatChoice Load(IValueStream stream)
            {
                return new FloatChoice(stream.GetFloat32());
            }
        }

        public class DoubleChoice : ITestChoice
        {
            public TestChoice Choice { get { return TestChoice.Double; } }

            public double Value { get; private set; }

            public DoubleChoice(double value)
            {
                this.Value = value;
            }

            public static DoubleChoice Load(IValueStream stream)
            {
                return new DoubleChoice(stream.GetFloat64());
            }
        }

        public class StringChoice : ITestChoice
        {
            public TestChoice Choice { get { return TestChoice.String; } }

            public string Value { get; private set; }

            public StringChoice(string value)
            {
                this.Value = value;
            }

            public static StringChoice Load(IValueStream stream)
            {
                return new StringChoice(stream.GetCharString());
            }
        }

        private class TestType
        {
            public Null NullValue { get; private set; }

            public bool BooleanValue { get; private set; }

            public Option<byte> OptionalValue { get; private set; }

            public ITestChoice ChoiceValue { get; private set; }

            public ReadOnlyArray<ushort> ArrayValue { get; private set; }

            public TestType(Null nullValue, bool booleanValue, Option<byte> optionalValue, ITestChoice choiceValue, ReadOnlyArray<ushort> arrayValue)
            {
                this.NullValue = nullValue;
                this.BooleanValue = booleanValue;
                this.OptionalValue = optionalValue;
                this.ChoiceValue = choiceValue;
                this.ArrayValue = arrayValue;
            }

            public static TestType Load(IValueStream stream)
            {
                stream.EnterSequence();
                var nullValue = stream.GetNull();
                var booleanValue = stream.GetBoolean();
                var optionalValue = Value<Option<byte>>.Loader(stream);

                var choice = stream.EnterChoice();
                ITestChoice choiceValue = null;
                switch (choice)
                {
                    case 0:
                        choiceValue = FloatChoice.Load(stream);
                        break;
                    case 1:
                        choiceValue = DoubleChoice.Load(stream);
                        break;
                    case 2:
                        choiceValue = StringChoice.Load(stream);
                        break;
                }
                stream.LeaveChoice();

                var arrayValue = ReadOnlyArray<ushort>.Load(stream);

                return new TestType(nullValue, booleanValue, optionalValue, choiceValue, arrayValue);
            }
        }

        public static readonly ISchema Schema =
            new SequenceSchema(false,
                new FieldSchema("NullValue", 255, PrimitiveSchema.NullSchema),
                new FieldSchema("BooleanValue", 1, PrimitiveSchema.BooleanSchema),
                new FieldSchema("OptionalValue", 2, new OptionSchema(PrimitiveSchema.Unsigned8Schema)),
                new FieldSchema("ChoiceValue", 3,
                    new ChoiceSchema(false,
                        new FieldSchema("FloatChoice", 255, PrimitiveSchema.Float32Schema),
                        new FieldSchema("DoubleChoice", 255, PrimitiveSchema.Float64Schema),
                        new FieldSchema("StringChoice", 1, PrimitiveSchema.CharStringSchema))),
                new FieldSchema("ArrayValue", 4, new ArraySchema(PrimitiveSchema.Unsigned16Schema)));


        private TagReader fromHexString(string str)
        {
            var chars = str.ToUpper().Where(c => Char.IsDigit(c) || (c >= 'A' && c <= 'F')).ToArray();
            str = new string(chars);

            var bytes = Enumerable.Range(0, str.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(str.Substring(x, 2), 16))
                .ToArray();

            var ms = new MemoryStream(bytes);
            return new TagReader(ms);
        }

        [TestMethod]
        public void ReadTest1()
        {
            var reader = fromHexString("00-19-01-29-40-3E-44-00-00-00-00-3F-4E-21-00-21-01-21-02-4F");
            var stream = new TagReaderStream(reader, Schema);
            var test = TestType.Load(stream);

            Assert.AreEqual(test.BooleanValue, true);
            Assert.IsTrue(test.OptionalValue.HasValue);
            Assert.AreEqual(test.OptionalValue.Value, 64);
            Assert.AreEqual(test.ChoiceValue.Choice, TestChoice.Float);
            Assert.AreEqual(((FloatChoice)test.ChoiceValue).Value, 0.00);
            Assert.AreEqual(test.ArrayValue.Length, 3);
            for(int i = 0; i < test.ArrayValue.Length; i++)
            {
                Assert.AreEqual(test.ArrayValue[i], (ushort)i);
            }
        }



    }
}
