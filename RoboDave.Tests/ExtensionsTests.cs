using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboDave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        #region SplitAtFirst

        [TestMethod]
        public void SplitAtFirstTest_NotThere()
        {
            String input = "foo";
            Char splitter = ' ';
            String[] expected = new String[] { "foo", null };
            String[] actual;
            actual = input.SplitAtFirst(splitter);
            ExtensionsTests.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SplitAtFirstTest_Basic()
        {
            String input = "foo,bar";
            Char splitter = ',';
            String[] expected = new String[] { "foo", "bar" };
            String[] actual;
            actual = input.SplitAtFirst(splitter);
            ExtensionsTests.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SplitAtFirstTest_EndEmpty()
        {
            String input = "foo,";
            Char splitter = ',';
            String[] expected = new String[] { "foo", "" };
            String[] actual;
            actual = input.SplitAtFirst(splitter);
            ExtensionsTests.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SplitAtFirstTest_Begining()
        {
            String input = ",bar";
            Char splitter = ',';
            String[] expected = new String[] { "", "bar" };
            String[] actual;
            actual = input.SplitAtFirst(splitter);
            ExtensionsTests.AreEqual(expected, actual);
        }

        #endregion

        public static void AreEqual(String[] expected, String[] actual)
        {
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual<String>(expected[i], actual[i]);
            }
        }

        [DataTestMethod]
        [DataRow("abcdef", "bc", "ef", 0, "d")]
        [DataRow("abcdef", "ef", "gh", 0, "")]
        [DataRow("abcdef", "ab", "gh", 0, "cdef")]
        [DataRow("abcdef", "ab", "gh", 0, "cdef")]
        [DataRow("", "", "", 0, "")]
        [DataRow("abc123defabc456defabc789def", "abc", "def", 2, "456")]
        [DataRow("abc123defabc456defabc789def", "abc", "def", 10, "789")]

        public void GetTxtBtwnTest(String input, String start, String end, int startIndex, String expected)
        {
            string result = input.GetTxtBtwn(start, end, startIndex);
            Assert.AreEqual<String>(expected, result);
        }
        

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("test", "test")]
        [DataRow("\t", " ")]
        [DataRow("\n", " ")]
        [DataRow("\r", " ")]
        [DataRow("\r\n", "  ")]
        [DataRow("\u0009", " ")]
        [DataRow("\u000A", " ")]
        [DataRow("\u000B", " ")]
        [DataRow("\u000C", " ")]
        [DataRow("\u000D", " ")]
        [DataRow("\u0020", " ")]
        [DataRow("\u0085", " ")]
        [DataRow("\u00A0", " ")]
        [DataRow("\u1680", " ")]
        [DataRow("\u2000", " ")]
        [DataRow("\u2001", " ")]
        [DataRow("\u2002", " ")]
        [DataRow("\u2003", " ")]
        [DataRow("\u2004", " ")]
        [DataRow("\u2005", " ")]
        [DataRow("\u2006", " ")]
        [DataRow("\u2007", " ")]
        [DataRow("\u2008", " ")]
        [DataRow("\u2009", " ")]
        [DataRow("\u200A", " ")]
        [DataRow("\u2028", " ")]
        [DataRow("\u2029", " ")]
        [DataRow("\u202F", " ")]
        [DataRow("\u205F", " ")]
        [DataRow("\u3000", " ")]
        [DataRow("test\ttest\rtest\n ", "test test test  ")]


        public void ConvertWhitespaceToSpacesTest(String input, String expected)
        {
            string result = input.ConvertWhitespaceToSpaces();
            Assert.AreEqual<String>(expected, result);
        }

    }
}
