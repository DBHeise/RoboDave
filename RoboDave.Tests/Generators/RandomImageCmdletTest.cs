
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
    public class RandomImageCmdletTest : RoboDave.Tests.PSCmdletTest
    {
        
        private void BasicAlgoVerify(String type)
        {
            ps.AddCommand("New-RandomImage")
                .AddParameter("Type", type);

            var list = this.DoInvoke<Bitmap>();
            Assert.AreEqual(1, list.Count);

            foreach (var hr in list)
            {
                Assert.AreEqual(512, hr.Width);
                Assert.AreEqual(512, hr.Height);

                hr.Dispose();
            }
        }

        [TestMethod] public void BasicAlgoVerify_Random() { this.BasicAlgoVerify("Random"); }
        [TestMethod] public void BasicAlgoVerify_Pixel() { this.BasicAlgoVerify("Pixel"); }
        [TestMethod] public void BasicAlgoVerify_SimpleShape() { this.BasicAlgoVerify("SimpleShape"); }


    }
}
