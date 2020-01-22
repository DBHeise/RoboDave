
namespace RoboDave.Tests.Network
{

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RoboDave.Generators;
    using RoboDave.Network;
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
    public class RemoteAddressesCmdletTest : RoboDave.Tests.PSCmdletTest
    {
        [TestMethod]
        public void BasicAlgoVerify()
        {
            ps.AddCommand("Get-RemoteAddresses");              

            var list = this.DoInvoke<RemoteAddressInfo>();
            Assert.AreEqual(6, list.Count);
        }
    }
}
