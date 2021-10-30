using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static IngameScript.Program;

namespace KEZ_OS_TESTS
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Class1 asd = new Class1();

            Assert.AreEqual("qwe", asd.qweasd(true));
            Assert.AreEqual("asd", asd.qweasd(false));
        }
    }
}
