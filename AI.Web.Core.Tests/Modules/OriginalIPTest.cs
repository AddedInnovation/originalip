using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Net;
using Added.Web.Core.Modules;

namespace Added.netFORUM.Web.Tests.Modules
{
    [TestClass]
    public class OriginalIPTest : HttpUnitTestBase
    {
        public OriginalIPTest()
        {
        }

        protected void SetupHeader(params string[] values)
        {
            request.Setup(x => x.Headers).Returns(
                new WebHeaderCollection() { { "X-Forwarded-For", string.Join(",",values) } }).Verifiable();
        }

        [TestMethod]
        public void ContextNull()
        {
            var module = new OriginalIP();
            module.OnBeginRequest(null);
        }

        [TestMethod]
        public void ContextWithInvalidIPAddress()
        {
            var serverVariables = new NameValueCollection();

            this.request.Setup(x => x.ServerVariables).Returns(serverVariables);
            this.SetupHeader("notanipaddress");

            var module = new OriginalIP();
            module.OnBeginRequest(context.Object);

            request.Verify();
            Assert.AreEqual(0, serverVariables.Count);
        }

        [TestMethod]
        public void ContextWithASingleIPAddress()
        {
            var serverVariables = new NameValueCollection();
            var ip = "10.5.1.1";

            this.SetupHeader(ip);
            this.request.Setup(x => x.ServerVariables).Returns(serverVariables).Verifiable();

            var module = new OriginalIP();
            module.OnBeginRequest(context.Object);

            request.VerifyAll();

            Assert.AreEqual(2, serverVariables.Count);
            Assert.AreEqual(ip, serverVariables["REMOTE_ADDR"]);
            Assert.AreEqual(ip, serverVariables["REMOTE_HOST"]);
        }

        [TestMethod]
        public void ContextWithMultipleIPAddresses()
        {
            var serverVariables = new NameValueCollection();
            var ip = "10.5.1.1, 10.5.1.2, 10.5.1.3";

            this.SetupHeader(ip);
            this.request.Setup(x => x.ServerVariables).Returns(serverVariables).Verifiable();

            var module = new OriginalIP();
            module.OnBeginRequest(context.Object);

            request.VerifyAll();

            Assert.AreEqual(2, serverVariables.Count);
            Assert.AreEqual("10.5.1.1", serverVariables["REMOTE_ADDR"]);
            Assert.AreEqual("10.5.1.1", serverVariables["REMOTE_HOST"]);
        }
    }
}
