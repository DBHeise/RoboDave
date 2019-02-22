using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboDave.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Random.Tests
{
    [TestClass]
    public class StringTokenizerTests
    {
        [TestMethod]
        public void Looping_Simple()
        {
            int expected = 10;
            StringTokenizer st = new StringTokenizer("This is a simple test!");
            int actual = 0;
            foreach (StringToken token in st)
            {
                Assert.IsNotNull(token);
                actual++;
            }

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Looping_Complex()
        {
            int expected = 110;
            String text = "The [ANIMAL-foo] and the [ANIMAL-2]\r\n\r\nA [ANIMAL-fly] sat on the axle-tree of a [OBJECT-240842], and addressing the [ANIMAL-2]\r\nsaid, \"How slow you are! Why do you not go faster? See if I do not prick\r\nyour neck with my [OBJECT].\"";
            StringTokenizer st = new StringTokenizer(text);
            int actual = 0;
            foreach (StringToken token in st)
            {
                System.Diagnostics.Debug.WriteLine("\"{0}\" - {1}", token.String, token.Type);
                Assert.IsNotNull(token);
                actual++;
            }
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void Verification_Simple()
        {
            StringTokenizer st = new StringTokenizer("This is a simple test!");
            Assert.IsNull(st.Current);

            Assert.IsTrue(st.MoveNext());
            StringToken token = st.Current;
            Assert.AreEqual(StringTokenType.Word, token.Type);
            Assert.AreEqual("This", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Whitespace, token.Type);
            Assert.AreEqual(" ", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Word, token.Type);
            Assert.AreEqual("is", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Whitespace, token.Type);
            Assert.AreEqual(" ", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Word, token.Type);
            Assert.AreEqual("a", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Whitespace, token.Type);
            Assert.AreEqual(" ", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Word, token.Type);
            Assert.AreEqual("simple", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Whitespace, token.Type);
            Assert.AreEqual(" ", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Word, token.Type);
            Assert.AreEqual("test", token.String);

            Assert.IsTrue(st.MoveNext());
            token = st.Current;
            Assert.AreEqual(StringTokenType.Punctuation, token.Type);
            Assert.AreEqual("!", token.String);

            Assert.IsFalse(st.MoveNext());
        }

    }
}