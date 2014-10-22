using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BACnet.Tagging;

namespace BACnet.Tagging.Tests
{
    [TestClass]
    public class TagReaderTests
    {
        private TagReader fromHexString(string str)
        {
            var bytes = Enumerable.Range(0, str.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(str.Substring(x, 2), 16))
                .ToArray();

            var ms = new MemoryStream(bytes);
            return new TagReader(ms);
        }

        [TestMethod]
        public void ReadNullTest()
        {
            var reader = fromHexString("00");
            reader.ReadNull();

            reader = fromHexString("18");
            reader.ReadNull(1);

            reader = fromHexString("F8FE");
            reader.ReadNull(254);
        }

        [TestMethod]
        public void ReadBooleanTest()
        {
            var reader = fromHexString("11");
            Assert.IsTrue(reader.ReadBoolean() == true);

            reader = fromHexString("10");
            Assert.IsTrue(reader.ReadBoolean() == false);

            reader = fromHexString("5901");
            Assert.IsTrue(reader.ReadBoolean(5) == true);

            reader = fromHexString("6900");
            Assert.IsTrue(reader.ReadBoolean(6) == false);
        }

        [TestMethod]
        public void ReadUnsignedTest()
        {
            var reader = fromHexString("2101");
            Assert.IsTrue(reader.ReadUnsigned8() == 1);

            reader = fromHexString("5AFFFF");
            Assert.IsTrue(reader.ReadUnsigned16(5) == 65535);

            reader = fromHexString("F9FE00");
            Assert.IsTrue(reader.ReadUnsigned8(254) == 0);
        }

        [TestMethod]
        public void ReadSignedTest()
        {
            var reader = fromHexString("31FF");
            Assert.IsTrue(reader.ReadSigned8() == -1);

            reader = fromHexString("5901");
            Assert.IsTrue(reader.ReadSigned8(5) == 1);

            reader = fromHexString("FAFE8000");
            Assert.IsTrue(reader.ReadSigned16(254) == short.MinValue);
        }

        [TestMethod]
        public void ReadFloat32Test()
        {
            var reader = fromHexString("4400000000");
            Assert.IsTrue(reader.ReadFloat32() == 0);

            reader = fromHexString("8C00000000");
            Assert.IsTrue(reader.ReadFloat32(8) == 0);
        }

        [TestMethod]
        public void ReadFloat64Test()
        {
            var reader = fromHexString("55080000000000000000");
            Assert.IsTrue(reader.ReadFloat64() == 0);
        }

        [TestMethod]
        public void ReadOctetStringTest()
        {
            var reader = fromHexString("6401020304");
            var str = BitConverter.ToString(reader.ReadOctetString());
            Assert.IsTrue(str == "01-02-03-04");

            reader = fromHexString("7D0807060504030201");
            str = BitConverter.ToString(reader.ReadOctetString(7));
            Assert.IsTrue(str == "07-06-05-04-03-02-01");
        }

        [TestMethod]
        public void ReadCharStringTest()
        {
            var reader = fromHexString("750C0048656C6C6F20576F726C64");
            Assert.IsTrue(reader.ReadCharString() == "Hello World");
        }

        [TestMethod]
        public void ReadEnumeratedTest()
        {
            var reader = fromHexString("920100");
            Assert.AreEqual(reader.ReadEnumerated(), 256u);
        }

        [TestMethod]
        public void ReadDateTest()
        {
            var reader = fromHexString("A4640102FF");
            var date = reader.ReadDate();
            Assert.IsTrue(date.Year == 100);
            Assert.IsTrue(date.Month == 1);
            Assert.IsTrue(date.Day == 2);
            Assert.IsTrue(date.DayOfWeek == 255);
        }
    }
}
