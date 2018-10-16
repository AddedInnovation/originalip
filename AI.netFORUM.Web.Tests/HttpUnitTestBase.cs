using Moq;
using System.Web;

namespace AI.netFORUM.Web.Tests
{
    public abstract class HttpUnitTestBase
    {
        protected readonly Mock<HttpContextBase> context;
        protected readonly Mock<HttpRequestBase> request;
        protected readonly Mock<HttpResponseBase> response;

        public HttpUnitTestBase()
        {
            context = new Mock<HttpContextBase>();
            request = new Mock<HttpRequestBase>();
            response = new Mock<HttpResponseBase>();

            context.Setup(x => x.Request).Returns(request.Object);
            context.Setup(x => x.Response).Returns(response.Object);
        }
    }
}
