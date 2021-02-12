using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SFA.DAS.FAT.Domain.Configuration;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Infrastructure.Api;
using SFA.DAS.FAT.Infrastructure.UnitTests.HttpMessageHandlerMock;

namespace SFA.DAS.FAT.Infrastructure.UnitTests.Api
{
    public class WhenCallingPostOnTheApiClient
    {
        [Test, AutoData]
        public async Task Then_The_Endpoint_Is_Called(
            PostData postContent,
            int id,
            FindApprenticeshipTrainingApi config)
        {
            //Arrange
            var configMock = new Mock<IOptions<FindApprenticeshipTrainingApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Accepted
            };
            var postTestRequest = new PostTestRequest(id,"https://test.local") {Data = postContent};
            var expectedUrl = postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl, config.Key, "post");
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            
            //Act
            await apiClient.Post(postTestRequest);

            //Assert
            httpMessageHandler.Protected()
                .Verify<Task<HttpResponseMessage>>(
                    "SendAsync", Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(c =>
                        c.Method.Equals(HttpMethod.Post)
                        && c.RequestUri.AbsoluteUri.Equals(expectedUrl)),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
        
        [Test, AutoData]
        public void Then_If_It_Is_Not_Successful_An_Exception_Is_Thrown(
            PostData postContent,
            int id,
            FindApprenticeshipTrainingApi config)
        {
            //Arrange
            var configMock = new Mock<IOptions<FindApprenticeshipTrainingApi>>();
            configMock.Setup(x => x.Value).Returns(config);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            var postTestRequest = new PostTestRequest(id,"https://test.local") {Data = postContent};
            var expectedUrl = postTestRequest.PostUrl;
            var httpMessageHandler = MessageHandler.SetupMessageHandlerMock(response, expectedUrl, config.Key, "post");
            var client = new HttpClient(httpMessageHandler.Object);
            var apiClient = new ApiClient(client, configMock.Object);
            
            //Act Assert
            Assert.ThrowsAsync<HttpRequestException>(() => apiClient.Post(postTestRequest));
            
        }
        
        private class PostTestRequest : IPostApiRequest<PostData>
        {
            private readonly int _id;

            public PostTestRequest (int id, string baseUrl)
            {
                _id = id;
                BaseUrl = baseUrl;
            }
            public PostData Data { get; set; }
            public string PostUrl => $"{BaseUrl}/test-url/post/{_id}";
            public string BaseUrl { get; }
        }

        public class PostData
        {
            public string Id { get; set; }
        }

    }
}