using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboDave.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoboDave.Random.Tests
{
    [TestClass]
    public class StringGeneratorTests
    {

        [TestMethod]
        public void GetStringTest_Basic_AaZz()
        {
            BasicTest(StringType.AaZz);
        }

        [TestMethod]
        public void GetStringTest_Basic_AlphaNumeric()
        {
            BasicTest(StringType.AlphaNumeric);
        }

        [TestMethod]
        public void GetStringTest_Basic_Digits()
        {
            BasicTest(StringType.Digits);
        }

        [TestMethod]
        public void GetStringTest_AllStringTypes()
        {
            foreach (StringType st in Enum.GetValues(typeof(StringType)))
                BasicTest(st);
        }


        private void BasicTest(StringType type)
        {
            ulong length = 255;
            string actual;
            actual = StringGenerator.GetString(type, length);
            AssertStringType(type, actual);

            if (type != StringType.Name && type != StringType.Sentence && type != StringType.Word && type != StringType.Email && type != StringType.EmailSimple && type != StringType.TLD && type != StringType.Uri)
                Assert.AreEqual((int)length, actual.Length);

        }

        public static void AssertStringType(StringType st, String actual)
        {
            Assert.IsTrue(Regex.IsMatch(actual, GetStringTypeRegEx(st)));
        }

        private static String GetStringTypeRegEx(StringType type)
        {
            String ans = String.Empty;
            switch (type)
            {
                case StringType.AaZz:
                    ans = "[a-zA-Z]*";
                    break;
                case StringType.AlphaNumeric:
                    ans = "[a-zA-Z0-9]*";
                    break;
                case StringType.Digits:
                    ans = "[0-9]*";
                    break;
                case StringType.LowerCase:
                    ans = "[a-z]*";
                    break;
                case StringType.UpperCase:
                    ans = "[A-Z]*";
                    break;
                case StringType.Hex:
                    ans = "[0-9A-F]*";
                    break;
                default:
                    ans = ".*";
                    break;
            }
            return ans;
        }

    }
}