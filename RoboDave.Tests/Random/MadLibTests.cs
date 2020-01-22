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

        [TestMethod]
        public void Generate_Modifier_TitleCase()
        {
            String[] inputList = new string[] { "[noun:titlecase]", "[adjective:titlecase]", "[verb:titlecase]", "[name:titlecase]", "[boyname:titlecase]", "[girlname:titlecase]", "[lastname:titlecase]", "[person:titlecase]", "[relation:titlecase]", "[title:titlecase]", "[occupation:titlecase]", "[place:titlecase]", "[thing:titlecase]", "[animal:titlecase]", "[plural_animal:titlecase]", "[object:titlecase]", "[plural_object:titlecase]", "[food:titlecase]", "[plural_food:titlecase]", "[verb_presenttense:titlecase]", "[verb_progressive:titlecase]", "[verb_pasttense:titlecase]", "[color:titlecase]", "[adverb:titlecase]", "[interjection:titlecase]", "[top5000:titlecase]" };
            foreach (String input in inputList)
            {
                String result = target.Generate(input);
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Contains("["));
                char[] ansList = result.ToCharArray();
                for (int i = 0; i < ansList.Length; i++)
                {
                    Char c = ansList[i];
                    if (Char.IsLetter(c))
                    {
                        if (i == 0)
                        {
                            Assert.IsTrue(Char.IsUpper(c), String.Format("Character is not upper case: '{0}',{1},'{2}'", c, i, result));
                        }
                        else
                        {
                            Assert.IsTrue(Char.IsLower(c), String.Format("Character is not lower case: '{0}',{1},'{2}'", c, i, result));
                        }
                    }
                }
            }
        }
        [TestMethod]
        public void Generate_Modifier_UpperCase()
        {
            String[] inputList = new string[] { "[noun:uppercase]", "[adjective:uppercase]", "[verb:uppercase]", "[name:uppercase]", "[boyname:uppercase]", "[girlname:uppercase]", "[lastname:uppercase]", "[person:uppercase]", "[relation:uppercase]", "[title:uppercase]", "[occupation:uppercase]", "[place:uppercase]", "[thing:uppercase]", "[animal:uppercase]", "[plural_animal:uppercase]", "[object:uppercase]", "[plural_object:uppercase]", "[food:uppercase]", "[plural_food:uppercase]", "[verb_presenttense:uppercase]", "[verb_progressive:uppercase]", "[verb_pasttense:uppercase]", "[color:uppercase]", "[adverb:uppercase]", "[interjection:uppercase]", "[top5000:uppercase]" };
            foreach (String input in inputList)
            {
                String result = target.Generate(input);
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Contains("["));
                char[] ansList = result.ToCharArray();
                for (int i = 0; i < ansList.Length; i++)
                {
                    Char c = ansList[i];
                    if (Char.IsLetter(c))
                    {
                        Assert.IsTrue(Char.IsUpper(c), String.Format("Chacter is not upper case: '{0}',{1},'{2}'", c, i, result));
                    }
                }
            }
        }
        [TestMethod]
        public void Generate_Modifier_LowerCase()
        {
            String[] inputList = new string[] { "[noun:lowercase]", "[adjective:lowercase]", "[verb:lowercase]", "[name:lowercase]", "[boyname:lowercase]", "[girlname:lowercase]", "[lastname:lowercase]", "[person:lowercase]", "[relation:lowercase]", "[title:lowercase]", "[occupation:lowercase]", "[place:lowercase]", "[thing:lowercase]", "[animal:lowercase]", "[plural_animal:lowercase]", "[object:lowercase]", "[plural_object:lowercase]", "[food:lowercase]", "[plural_food:lowercase]", "[verb_presenttense:lowercase]", "[verb_progressive:lowercase]", "[verb_pasttense:lowercase]", "[color:lowercase]", "[adverb:lowercase]", "[interjection:lowercase]", "[top5000:lowercase]" };
            foreach (String input in inputList)
            {

                String result = target.Generate(input);
                Assert.IsNotNull(result);
                Assert.IsFalse(result.Contains("["));
                char[] ansList = result.ToCharArray();
                for (int i = 0; i < ansList.Length; i++)
                {
                    Char c = ansList[i];
                    if (Char.IsLetter(c))
                    {
                        Assert.IsTrue(Char.IsLower(c), String.Format("Character is not lower case: '{0}',{1},'{2}'", c, i, result));
                    }
                }
            }
        }
    }
}