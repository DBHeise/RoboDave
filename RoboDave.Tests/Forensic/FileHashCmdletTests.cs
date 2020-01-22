
namespace RoboDave.Forensic.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RoboDave.Forensic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class FileHashCmdletTests : RoboDave.Tests.PSCmdletTest
    {
       


        [TestMethod]
        public void cTor()
        {
            var cit = new FileHashCmdlet();
            Assert.IsNotNull(cit);
            Assert.IsTrue(cit is PSCmdlet);
        }

        [TestMethod]
        public void BaiscAlgoVerify()
        {
            var testFile = System.IO.Path.GetTempFileName();
            testFiles.Add(testFile);
            System.IO.File.WriteAllText(testFile, "this is a simple test",Encoding.ASCII);
                        
            ps.AddCommand("Get-FileHashBulk").AddParameter("InputFiles",testFile);
            var list = this.DoInvoke<HashResult>();
            Assert.AreEqual(3, list.Count);

            foreach (var hr in list)
            {
                switch(hr.Algorithm)
                {
                    case "md5":
                        Assert.AreEqual("5C7318CF2435793698963C0E64F36AB1", hr.Hash);
                        break;
                    case "sha1":
                        Assert.AreEqual("2ACDDDB97C144820E5741B4218FC77C2BCA8EFA5", hr.Hash);
                        break;
                    case "sha256":
                        Assert.AreEqual("174E8AAED6221C2D83FE5F074BB31D15789E6C6BD0BE6656553511544E9F5EA9", hr.Hash);
                        break;
                    default:
                        Assert.Fail("Unexpected Algorithm");
                        break;
                }
            }

        }
    }
}