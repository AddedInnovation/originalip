using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AI.netFORUM.Web.Modules;
using System.Web;
using Moq;
using System.Net;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace AI.netFORUM.Web.Tests.Modules
{
    [TestClass]
    public class PassOriginalIPTest : HttpUnitTestBase
    {
        public PassOriginalIPTest()
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
            var module = new PassOriginalIP();
            module.OnBeginRequest(null);
        }

        [TestMethod]
        public void ContextWithInvalidIPAddress()
        {
            var serverVariables = new NameValueCollection();

            this.request.Setup(x => x.ServerVariables).Returns(serverVariables).Verifiable();
            this.SetupHeader("notanipaddress");

            var module = new PassOriginalIP();
            module.OnBeginRequest(context.Object);

            request.VerifyAll();
            Assert.AreEqual(0, serverVariables.Count);
        }

        [TestMethod]
        public void ContextWithASingleIPAddress()
        {
            var serverVariables = new NameValueCollection();
            var ip = "10.5.1.1";

            this.SetupHeader(ip);
            this.request.Setup(x => x.ServerVariables).Returns(serverVariables).Verifiable();

            var module = new PassOriginalIP();
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

            var module = new PassOriginalIP();
            module.OnBeginRequest(context.Object);

            request.VerifyAll();

            Assert.AreEqual(2, serverVariables.Count);
            Assert.AreEqual("10.5.1.1", serverVariables["REMOTE_ADDR"]);
            Assert.AreEqual("10.5.1.1", serverVariables["REMOTE_HOST"]);
        }
    }
}
