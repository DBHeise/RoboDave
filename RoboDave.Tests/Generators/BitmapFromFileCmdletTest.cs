

namespace RoboDave.Tests.Generators
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RoboDave.Generators;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class BitmapFromFileCmdletTest : RoboDave.Tests.PSCmdletTest
    {
        [TestMethod]
        public void BasicAlgoVerify()
        {
            var testFile = System.IO.Path.GetTempFileName();
            testFiles.Add(testFile);

            byte[] buffer = new byte[512];
            for (int i = 0; i < 512; i++) { buffer[i] = 1; }
            System.IO.File.WriteAllBytes(testFile, buffer);

            ps.AddCommand("Get-BitmapFromFile")
                .AddParameter("InputFile", testFile);

            var list = this.DoInvoke<Bitmap>();
            Assert.AreEqual(1, list.Count);

            foreach (var hr in list)
            {
                Assert.AreEqual(23, hr.Width);
                Assert.AreEqual(23, hr.Height);

                hr.Dispose();
            }
        }
    }
}
