using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoboDave.Text;
using System;

namespace RoboDave.Tests.Text
{
    [TestClass]
    public class CustomBaseTests
    {
        [TestMethod] public void ConvertToBaseAlpha_Simple()
        {
            Assert.AreEqual("5", CustomBase.ConvertToBaseAlpha(5));
            Assert.AreEqual("A", CustomBase.ConvertToBaseAlpha(10));
            Assert.AreEqual("42", CustomBase.ConvertToBaseAlpha(146));
            Assert.AreEqual("LOBRXV", CustomBase.ConvertToBaseAlpha(1310649907));
            Assert.AreEqual("ROBODAVE", CustomBase.ConvertToBaseAlpha(2168781265994));
        }

        [TestMethod] public void ConvertFromBaseAlpha_Simple()
        {
            Assert.AreEqual((ulong)5, CustomBase.ConvertFromBaseAlpha("5"));
            Assert.AreEqual((ulong)10, CustomBase.ConvertFromBaseAlpha("A"));
            Assert.AreEqual((ulong)146, CustomBase.ConvertFromBaseAlpha("42"));
            Assert.AreEqual((ulong)1310649907, CustomBase.ConvertFromBaseAlpha("LOBRXV"));
            Assert.AreEqual((ulong)2168781265994, CustomBase.ConvertFromBaseAlpha("ROBODAVE"));
        }
    }
}
