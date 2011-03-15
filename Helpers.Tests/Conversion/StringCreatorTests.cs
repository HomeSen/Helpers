using System;
using NUnit.Framework;

namespace HomeSen.Helpers.Conversion.Tests
{
    [TestFixture]
    public class StringCreatorTests
    {
        [SetUp]
        public void Init()
        {
        }

        [TearDown]
        public void Cleanup()
        {
        }

        [Test]
        public void DumpByteArray_NullValue_ReturnsEmptyString()
        {
            byte[] arr = null;
            string actual = StringCreator.DumpByteArray(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void DumpByteArray_EmptyArray_ReturnsEmptyString()
        {
            byte[] arr = new byte[] { };
            string actual = StringCreator.DumpByteArray(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void DumpByteArray_ArrayWithOneElement_Returns()
        {
            byte[] arr = new byte[] { 61 };
            string expected = "{ 61 }";
            string actual = StringCreator.DumpByteArray(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DumpByteArray_ArrayWithTwoElements_Returns()
        {
            byte[] arr = new byte[] { 61, 255 };
            string expected = "{ 61, 255 }";
            string actual = StringCreator.DumpByteArray(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DumpByteArray_ArrayWithTenElements_Returns()
        {
            byte[] arr = new byte[] { 61, 78, 99, 1, 254, 3, 22, 89, 199, 128 };
            string expected = "{ 61, 78, 99, 1, 254, 3, 22, 89, 199, 128 }";
            string actual = StringCreator.DumpByteArray(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToHexString_NullArray_ReturnsEmptyString()
        {
            byte[] arr = null;
            string actual = StringCreator.ByteArrayToHexString(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void ByteArrayToHexString_EmptyArray_ReturnsEmptyString()
        {
            byte[] arr = new byte[] { }; ;
            string actual = StringCreator.ByteArrayToHexString(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void ByteArrayToHexString_ArrayWithOneElement_Returns()
        {
            byte[] arr = new byte[] { 16 };
            string expected = "10";
            string actual = StringCreator.ByteArrayToHexString(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToHexString_ArrayWithTwoElement_Returns()
        {
            byte[] arr = new byte[] { 3, 255 };
            string expected = "03FF";
            string actual = StringCreator.ByteArrayToHexString(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToHexString_ArrayWithTenElement_Returns()
        {
            byte[] arr = new byte[] { 61, 78, 99, 1, 254, 3, 22, 89, 199, 128 };
            string expected = "3D4E6301FE031659C780";
            string actual = StringCreator.ByteArrayToHexString(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToUTF8String_NullArray_ReturnsEmptyString()
        {
            byte[] arr = null;
            string actual = StringCreator.ByteArrayToUTF8String(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void ByteArrayToUTF8String_EmptyArray_ReturnsEmptyString()
        {
            byte[] arr = new byte[] { };
            string actual = StringCreator.ByteArrayToUTF8String(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void ByteArrayToUTF8String_ArrayWithString_Returns()
        {
            byte[] arr = new byte[] { 78, 85, 110, 105, 116, 32, 84, 101, 115, 116 };
            string expected = "NUnit Test";
            string actual = StringCreator.ByteArrayToUTF8String(arr);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToBase64String_NullArray_ReturnsEmptyString()
        {
            byte[] arr = null;
            string actual = StringCreator.ByteArrayToBase64String(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void ByteArrayToBase64String_EmptyArray_ReturnsEmptyString()
        {
            byte[] arr = new byte[] { };
            string actual = StringCreator.ByteArrayToBase64String(arr);

            Assert.IsNullOrEmpty(actual);
        }

        [Test]
        public void ByteArrayToBase64String_ArrayWithString_Returns()
        {
            byte[] arr = new byte[] { 78, 85, 110, 105, 116, 32, 84, 101, 115, 116 };
            string expected = "TlVuaXQgVGVzdA=="; // = "NUnit Test"
            string actual = StringCreator.ByteArrayToBase64String(arr);

            Assert.AreEqual(expected, actual);
        }
    }
}
