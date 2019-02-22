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
    }
}