
namespace RoboDave.Tests.Generators
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RoboDave.Generators;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;


    [TestClass]
    public class RandomFileCmdletTest : RoboDave.Tests.PSCmdletTest
    {

        [TestMethod]
        public void cTor()
        {
            var cit = new RandomFileCmdlet();
            Assert.IsNotNull(cit);
            Assert.IsTrue(cit is PSCmdlet);
        }

        [TestMethod]
        public void BasicAlgoVerify()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);
            
            ps.AddCommand("New-RandomFile")                
                .AddParameter("OutputFile", tmpFile);

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            foreach (var hr in list)
            {
                Assert.AreEqual(1024, hr.Length);
                Assert.AreEqual(tmpFile, hr.FullName);
            }
        }

        [TestMethod]
        public void DigitOnlyTest()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);
            long size = Rando.RandomInt(500, 5000);

            ps.AddCommand("New-RandomFile")
                .AddParameter("Size",size )
                .AddParameter("OutputFile", tmpFile)
                .AddParameter("StringType", "Digits");

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            foreach (var hr in list)
            {
                Assert.AreEqual(size, hr.Length);
                Assert.AreEqual(tmpFile, hr.FullName);

                Char[] text= System.IO.File.ReadAllText(hr.FullName).ToCharArray();
                for (int i = 0; i < text.Length; i++)
                {
                    Char c = text[i];
                    Assert.IsTrue(Char.IsDigit(c), String.Format("Character is not a digit: '{0}',{1}", c, i));
                }
            }
        }

        [TestMethod]
        public void WordTest()
        {
            String tmpFile = System.IO.Path.GetTempFileName();
            this.testFiles.Add(tmpFile);
            long size = Rando.RandomInt(500, 5000);

            ps.AddCommand("New-RandomFile")
                .AddParameter("Size", size)
                .AddParameter("OutputFile", tmpFile)
                .AddParameter("StringType", "Word")
                .AddParameter("Seperator", ";");

            var list = this.DoInvoke<FileInfo>();
            Assert.AreEqual(1, list.Count);

            foreach (var hr in list)
            {
                Assert.IsTrue(hr.Length >= size, String.Format("Output size is smaller than requested: expected={0}, actual={1}", size, hr.Length));                
                Assert.AreEqual(tmpFile, hr.FullName);

                Char[] text = System.IO.File.ReadAllText(hr.FullName).ToCharArray();
                for (int i = 0; i < text.Length; i++)
                {
                    Char c = text[i];
                    Assert.IsTrue((Char.IsLetter(c) || c==';' || c==' ' || c=='-' || c=='\''), String.Format("Character is not an expected value: '{0}',{1}", c, i ));
                }
            }
        }
    }
}
