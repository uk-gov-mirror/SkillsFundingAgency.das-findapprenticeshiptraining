using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace SFA.DAS.FAT.Infrastructure.UnitTests.HttpMessageHandlerMock
{
    public static class MessageHandler
    {
        public static Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, string url, string key)
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Get)
                        && c.Headers.Contains("Ocp-Apim-Subscription-Key")
                        && c.Headers.GetValues("Ocp-Apim-Subscription-Key").First().Equals(key)
                        && c.RequestUri.AbsoluteUri.Equals(url)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}