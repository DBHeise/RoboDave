
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
    public class RandomBitmapCmdletTest : RoboDave.Tests.PSCmdletTest
    {

        [TestMethod]
        public void BasicAlgoVerify()
        {
            int width = Rando.RandomInt(10, 512);
            int height = Rando.RandomInt(10, 512);

            ps.AddCommand("New-RandomBitmap")
              .AddParameter("Width", width)
              .AddParameter("Height", height);

            var list = this.DoInvoke<Bitmap>();
            Assert.AreEqual(1, list.Count);

            foreach(var hr in list)
            {
                Assert.AreEqual(width, hr.Width);
                Assert.AreEqual(height, hr.Height);

                hr.Dispose();
            }
        }
    }
}
