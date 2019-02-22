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
    public class MadLibTests
    {
        private MadLib target;

        public MadLibTests()
        {
        }

        [TestInitialize]
        public void TestInit()
        {
            this.target = new MadLib();
        }
        [TestCleanup]
        public void TestCleanup()
        {            
            this.target = null;
        }

        [TestMethod]
        public void GenerateMadLib_Basic()
        {
            String input = "the quick brown fox jumped over the lazy dog";
            String content = target.GenerateMadLib(input);
            String expected = "the [ADJECTIVE-1] [COLOR-2] [ANIMAL-3] [VERB_PASTTENSE-4] [ADVERB-5] the [ADJECTIVE-6] [ANIMAL-7]";
            Assert.AreEqual(expected, content);
        }

        [TestMethod]
        public void Generate_Basic()
        {
            String input = "the [ADJECTIVE-1] [COLOR-2] [ANIMAL-3] [VERB_PASTTENSE-4] [ADVERB-5] the [ADJECTIVE-6] [ANIMAL-7]";
            String result = target.Generate(input);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Contains("["));
        }
    }
}