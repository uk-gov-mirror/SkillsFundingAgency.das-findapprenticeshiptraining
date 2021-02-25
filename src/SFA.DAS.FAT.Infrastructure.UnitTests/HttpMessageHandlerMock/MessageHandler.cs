using System;
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
        public static Mock<HttpMessageHandler> SetupMessageHandlerMock(HttpResponseMessage response, string url, string key, string httpMethod = "get")
        {
            var method = HttpMethod.Get;
            if (httpMethod.Equals("get", StringComparison.CurrentCultureIgnoreCase))
            {
                method = HttpMethod.Get;
            }
            else if (httpMethod.Equals("post", StringComparison.CurrentCultureIgnoreCase))
            {
                method = HttpMethod.Post;
            }
            else if (httpMethod.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                method = HttpMethod.Delete;
            }
            
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(c =>
                         c.Method.Equals(method)
                        && c.Headers.Contains("Ocp-Apim-Subscription-Key")
                        && c.Headers.GetValues("Ocp-Apim-Subscription-Key").First().Equals(key)
                        && c.Headers.Contains("X-Version")
                        && c.Headers.GetValues("X-Version").First().Equals("1")
                        && c.RequestUri.AbsoluteUri.Equals(url)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync((HttpRequestMessage request, CancellationToken token) => response);
            return httpMessageHandler;
        }
    }
}