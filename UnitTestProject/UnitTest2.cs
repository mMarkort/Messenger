using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Messenger;
namespace UnitTestProject
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void AuthTest()
        {
            var page = new Messenger.Messenger.AuthPage();
            Assert.IsTrue(page);
        }
    }
}
