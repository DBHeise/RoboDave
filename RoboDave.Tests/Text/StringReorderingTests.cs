using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboDave.Random;
using RoboDave.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Text.Tests
{
    [TestClass]
    public class StringReorderingTests
    {
        [TestMethod, TestCategory("Unit")]
        public void EncodeTest_Basic()
        {
            StringReordering sr = new StringReordering();
            String input = "this is a test string";
            String output = sr.Encode(input);
            Assert.IsNotNull(output);
            Assert.AreNotEqual(input, output);
        }

        [TestMethod, TestCategory("Unit")]
        public void EncoderDecoder_RoundTrip()
        {
            StringReordering sr = new StringReordering();
            String input = StringGenerator.GetString(StringType.Sentence);
            String output = sr.Encode(input);
            String actual = sr.Decode(output);
            Assert.AreEqual(input, actual);
        }

        [TestMethod, TestCategory("Unit")]
        public void DecodeTest_Basic()
        {
            String expected = "this is a test string";
            String input = "\"{5}{6}{3}{2}{4}{1}{0}\" -f  'ng','tri','tes','s a ','t s','this',' i'";
            DecoderTest(expected, input);
        }

        [DataTestMethod]
        [DataRow("abcdef", "'{0}{1}{2}{3}{4}{5}'-f'a','b','c','d','e','f'")]
        [DataRow("abcdef", "\"{0}{1}{2}{3}{4}{5}\" -format \"a\",\"b\",\"c\",\"d\",\"e\",\"f\"")]
        [DataRow("a'b'cd\"e\"f", "\"{0}{1}{2}{3}{4}{5}\" -f \"a\",\"'b'\",\"c\",\"d\",\"`\"e`\"\",\"f\"")]
        [DataRow("ab'c'd'e'f", "'{0}{1}{2}{3}{4}{5}'-format'a','b',\"'c'\",'d','`'e`'','f'")]
        public void DecoderTest(String expected, String input)
        {
            StringReordering sr = new StringReordering();
            String output = sr.Decode(input);
            Assert.IsNotNull(output);
            Assert.AreNotEqual(input, output);
            Assert.AreEqual(expected, output);
        }
    }
}
